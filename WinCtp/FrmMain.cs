using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using GalaxyFutures.Sfit.Api;
using log4net;
using Newtonsoft.Json;

namespace WinCtp
{
    public partial class FrmMain : Form , IMainView
    {
        private readonly ILog _log;
        private readonly MainViewImpl _impl;
        private bool _listening;
        private readonly  ConcurrentQueue<CtpTrade> _tradeQueue;
        private readonly ConcurrentQueue<CtpOrder> _inputOrderQueue;

        private readonly BackgroundWorker _workerTimerReturnOrder;
        private readonly BackgroundWorker _workerTimerQryTrade;
        private readonly BackgroundWorker _workerTimerInsertOrder;

        #region 初始化
        public FrmMain()
        {
            InitializeComponent();

            _log = LogManager.GetLogger("CTP");
            _impl = new MainViewImpl(this);
            _tradeQueue = new ConcurrentQueue<CtpTrade>();
            _inputOrderQueue = new ConcurrentQueue<CtpOrder>();

            _workerTimerReturnOrder = new BackgroundWorker();
            _workerTimerReturnOrder.DoWork += WorkerTimerReturnOrderOnDoWork;
            _workerTimerQryTrade = new BackgroundWorker();
            _workerTimerQryTrade.DoWork += WorkerTimerQryTradeOnDoWork;
            _workerTimerInsertOrder = new BackgroundWorker();
            _workerTimerInsertOrder.DoWork += WorkerTimerInsertOrderOnDoWork;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _listening = false;
            //买卖
            var ludir = new List<LookupObject>
            {
                new LookupObject(CtpDirectionType.Buy, "买"),
                new LookupObject(CtpDirectionType.Sell, "卖")
            };
            cmbDirection.Bind(ludir);
            gcSubOrderDirection.Bind(ludir);
            //开平标志
            var luof = new List<LookupObject>
            {
                new LookupObject(CtpOffsetFlagType.Open, "开"),
                new LookupObject(CtpOffsetFlagType.Close, "平")
            };
            cmbOffsetFlag.Bind(luof);
            //组合开平标志
            var lucof = new List<LookupObject>
            {
                new LookupObject(((char)CtpOffsetFlagType.Open).ToString(), "开"),
                new LookupObject(((char)CtpOffsetFlagType.Close).ToString(), "平")
            };
            gcSubOrderCombOffsetFlag.Bind(lucof);
            //报单状态
            var lustatus= new List<LookupObject>
            {
                new LookupObject(CtpOrderStatusType.Unknown, "未知"),//表示Thost已经接受用户的委托指令，还没有转发到交易所
                new LookupObject(CtpOrderStatusType.AllTraded, "全部成交"),
                new LookupObject(CtpOrderStatusType.Canceled, "撤单"),
                new LookupObject(CtpOrderStatusType.NoTradeNotQueueing, "未成交不在队列中"),
                new LookupObject(CtpOrderStatusType.NoTradeQueueing, "未成交还在队列中"),
                new LookupObject(CtpOrderStatusType.NotTouched, "未触发"),
                new LookupObject(CtpOrderStatusType.PartTradedNotQueueing, "部分成交不在队列中"),
                new LookupObject(CtpOrderStatusType.PartTradedQueueing, "部分成交还在队列中"),
                new LookupObject(CtpOrderStatusType.Touched, "已触发")
            };
            gcSubOrderStatus.Bind(lustatus);

            var brokers = BrokerInfo.GetAll();
            var users = new UserInfoList(UserInfo.GetAll());
            
            var mstUsers = users.GetMst();
            dsMstUser.DataSource = mstUsers;
            var subUsers = users.GetSub();
            dsSubUser.DataSource = subUsers;

            tcSubInstrument.TabPages.Clear();
            foreach (var u in subUsers)
            {
                var tp = new TabPage(u.UserId);
                tcSubInstrument.TabPages.Add(tp);
            }

            tcMstInstrument.TabPages.Clear();
            foreach (var u in mstUsers)
            {
                var tp = new TabPage(u.UserId);
                tcMstInstrument.TabPages.Add(tp);
            }

            var us = new List<CtpUserInfo>();
            us.AddRange(mstUsers);
            us.AddRange(subUsers);
            foreach (var u in us)
            {
                u.Broker = brokers.First(o => o.Id == u.BrokerId).Clone();
            }
            foreach (var u in us)
            {
                var ua = new UserApi(u.UserId,u.Broker.TraderFrontAddress);
                var api = ua.TraderApi;

                api.OnFrontConnected += OnFrontConnected;
                api.OnFrontDisconnected += OnFrontDisconnected;
                api.OnRspUserLogin += OnRspUserLogin;
                api.OnRspUserLogout += OnRspUserLogout;
                api.OnRspError += OnRspError;

                api.OnRtnOrder += OnRtnOrder;
                api.OnRtnTrade += OnRtnTrade;
                api.OnRspOrderInsert += OnRspOrderInsert;
                api.OnErrRtnOrderInsert += OnErrRtnOrderInsert;

                api.OnRspQryInvestorPosition += OnRspQryInvestorPosition;
                api.OnRspQryInvestorPositionDetail += OnRspQryInvestorPositionDetail;

                api.OnRspQryTrade += OnRspQryTrade;

                api.OnRspQrySettlementInfo += OnRspQrySettlementInfo;
                api.OnRspSettlementInfoConfirm += OnRspSettlementInfoConfirm;
                api.OnRspQrySettlementInfoConfirm += OnRspQrySettlementInfoConfirm;

                api.SubscribePrivateTopic(CtpResumeType.Quick);
                api.SubscribePublicTopic(CtpResumeType.Quick);

                UserApi.This[u.UserId] = ua;
                ua.Start();
            }
        }

        #endregion

        #region 基础信息管理
        private void ibtnUser_Click(object sender, EventArgs e)
        {
            new FrmUser().ShowDialog(this);
        }

        private void ibtnOrderInsertConfig_Click(object sender, EventArgs e)
        {
            new FrmUserInsertOrderConfig().ShowDialog(this);
        }

        private void ibtnBroker_Click(object sender, EventArgs e)
        {
            new FrmBroker().ShowDialog(this);
        }
        #endregion

        #region 结算

        private void tsmiSettlementInfoConfirm_Click(object sender, EventArgs e)
        {
            gvSubOrder.EndEdit();
            dsSubUser.EndEdit();
            for (var i = 0; i < dsSubUser.Count; i++)
            {
                var u = (CtpSubUser)dsSubUser[i];
                if(!u.IsChecked || !u.IsLogin)
                    continue;
                var req = new CtpSettlementInfoConfirm();
                req.BrokerID = u.BrokerId;
                req.InvestorID = u.UserId;
                var reqId = 0;
                var rsp = u.TraderApi().ReqSettlementInfoConfirm(req, reqId);
                _log.DebugFormat("ReqSettlementInfoConfirm[{0}]:{1}\nrequest:{2}",
                    reqId,
                    Rsp.This[rsp], 
                    JsonConvert.SerializeObject(req));
            }
        }

        /// <summary>
        /// 查询结算信息确认。
        /// </summary>
        private void QrySettlementInfoConfirm(CtpUserInfo u)
        {
            var api = u.TraderApi();
            var req = new CtpQrySettlementInfoConfirm();
            req.BrokerID = u.BrokerId;
            req.InvestorID = u.UserId;
            var rsp = api.ReqQrySettlementInfoConfirm(req, 0);
            _log.DebugFormat("ReqQrySettlementInfoConfirm:{0}\nrequest:{1}", Rsp.This[rsp], JsonConvert.SerializeObject(req));
        }

        /// <summary>
        /// 查询结算信息确认响应。
        /// </summary>
        private void OnRspQrySettlementInfoConfirm(object sender, CtpSettlementInfoConfirm response, CtpRspInfo rspInfo, int requestId, bool isLast)
        {
            _log.DebugFormat("OnRspQrySettlementInfoConfirm[{0}]\nresponse:{1}\nrspInfo:{2}",
               requestId,
               JsonConvert.SerializeObject(response),
               JsonConvert.SerializeObject(rspInfo));
            if (rspInfo == null || rspInfo.ErrorID != 0 || response == null)
                return;
            for (var i = 0; i < dsSubUser.Count; i++)
            {
                var u = (CtpSubUser)dsSubUser[i];
                if (u.BrokerId == response.BrokerID && u.UserId == response.InvestorID)
                {
                    //"ConfirmDate":"20161205","ConfirmTime":"15:22:09"
                    var dt = response.ConfirmDate + response.ConfirmTime;
                    DateTime d;
                    if (!DateTime.TryParseExact(dt, "yyyyMMddHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out d))
                        break;
                    u.SettlementInfoConfirmTime = d;
                    dsSubUser.ResetItem(i);
                    break;
                }
            }
        }

        /// <summary>
        /// 确认结算信息响应。
        /// </summary>
        private void OnRspSettlementInfoConfirm(object sender, CtpSettlementInfoConfirm response, CtpRspInfo rspInfo, int requestId, bool isLast)
        {
            _log.DebugFormat("OnRspSettlementInfoConfirm[{0}]\nresponse:{1}\nrspInfo:{2}",
                requestId,
                JsonConvert.SerializeObject(response), 
                JsonConvert.SerializeObject(rspInfo));
            if (rspInfo == null || rspInfo.ErrorID != 0 || response == null)
                return;
            for (var i = 0; i < dsSubUser.Count; i++)
            {
                var u = (CtpSubUser)dsSubUser[i];
                if (u.BrokerId == response.BrokerID && u.UserId == response.InvestorID)
                {
                    var dt = response.ConfirmDate + response.ConfirmTime;
                    _log.Debug($"确认结算信息：orig {dt}");
                    DateTime d;
                    if (!DateTime.TryParseExact(dt, "yyyyMMddHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out d))
                        break;
                    _log.Debug($"确认结算信息：dest {d}");
                    u.SettlementInfoConfirmTime = d;
                    dsSubUser.ResetItem(i);
                    break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnRspQrySettlementInfo(object sender, CtpSettlementInfo response, CtpRspInfo rspInfo, int requestId, bool isLast)
        {
            _log.DebugFormat("OnRspQrySettlementInfo[{0}]\nresponse:{1}\nrspInfo:{2}",
                requestId,
                JsonConvert.SerializeObject(response), 
                JsonConvert.SerializeObject(rspInfo));
            if (rspInfo == null || rspInfo.ErrorID == 0 || response == null)
                return;
            for (var i = 0; i < dsSubUser.Count; i++)
            {
                var u = (CtpSubUser)dsSubUser[i];
                if (u.BrokerId == response.BrokerID && u.UserId == response.InvestorID)
                {
                    dsSubUser.ResetItem(i);
                    break;
                }
            }
        }
        #endregion

        private void OnFrontDisconnected(object sender, int response)
        {
            _log.DebugFormat("TradeApiOnFrontDisconnected [{0}]", response);
        }

        private void OnFrontConnected(object sender)
        {
            _log.Debug("TradeApiOnFrontConnected");
        }

        private void OnRspError(object sender, CtpRspInfo rspInfo, int requestId, bool isLast)
        {
            _log.DebugFormat("TradeApiOnRspError[{0}]\nrspInfo:{1}",
                requestId,
                JsonConvert.SerializeObject(rspInfo));
        }

        #region 账户

        private void OnRspUserLogout(object sender, CtpUserLogout response, CtpRspInfo rspInfo, int requestId, bool isLast)
        {
            _log.DebugFormat("TradeApiOnRspUserLogout[{0}]\nresponse:{1}\nrspInfo:{2}",
               requestId,
               JsonConvert.SerializeObject(response),
               JsonConvert.SerializeObject(rspInfo));
            if (rspInfo == null || rspInfo.ErrorID != 0)
                return;
            for (var i = 0; i < dsMstUser.Count; i++)
            {
                var u = (CtpUserInfo)dsMstUser[i];
                if (u.ReqId == requestId)
                {
                    u.IsLogin = false;
                    dsMstUser.ResetItem(i);
                    return;
                }
            }
            for (var i = 0; i < dsSubUser.Count; i++)
            {
                var u = (CtpUserInfo)dsSubUser[i];
                if (u.ReqId == requestId)
                {
                    u.IsLogin = false;
                    dsSubUser.ResetItem(i);
                    return;
                }
            }
        }

        private void OnRspUserLogin(object sender, CtpRspUserLogin response, CtpRspInfo rspInfo, int requestId, bool isLast)
        {
            _log.DebugFormat("OnRspUserLogin[{0}]\nresponse:{1}\nrspInfo:{2}",
                requestId,
                JsonConvert.SerializeObject(response), 
                JsonConvert.SerializeObject(rspInfo));
            if (rspInfo == null || rspInfo.ErrorID != 0)
                return;
            for (var i = 0; i < dsMstUser.Count; i++)
            {
                var u = (CtpUserInfo)dsMstUser[i];
                if (u.ReqId == requestId)
                {
                    u.IsLogin = true;
                    dsMstUser.ResetItem(i);
                    return;
                }
            }
            for (var i = 0; i < dsSubUser.Count; i++)
            {
                var u = (CtpSubUser)dsSubUser[i];
                if (u.ReqId == requestId)
                {
                    u.IsLogin = true;
                    if (response != null)
                    {
                        u.FrontId = response.FrontID;
                        u.SessionId = response.SessionID;
                        u.MaxOrderRef = Convert.ToInt32(response.MaxOrderRef);
                    }
                    dsSubUser.ResetItem(i);
                    QrySettlementInfoConfirm(u);
                    return;
                }
            }
        }

        private void tsmiSelectAllMstUser_Click(object sender, EventArgs e)
        {
            if (dsMstUser.Count == 0)
                return;
            foreach (CtpUserInfo user in dsMstUser)
            {
                user.IsChecked = true;
            }
            dsMstUser.ResetBindings(false);
        }

        private void tsmiMstUserLogin_Click(object sender, EventArgs e)
        {
            if (dsMstUser.Count == 0)
                return;
            gvMstUser.EndEdit();
            dsMstUser.EndEdit();
            foreach (CtpMstUser user in dsMstUser)
            {
                if (!user.IsChecked || user.IsLogin)
                    continue;
                var api = user.TraderApi();
                var userLoginReq = new CtpReqUserLogin();
                userLoginReq.BrokerID = user.BrokerId;
                userLoginReq.UserID = user.UserId;
                userLoginReq.Password = user.Password;
                userLoginReq.UserProductInfo = "JCTP";
                userLoginReq.ProtocolInfo = "X";
                userLoginReq.InterfaceProductInfo = "X";
                var rsp = api.ReqUserLogin(userLoginReq, user.ReqId);
                _log.DebugFormat("ReqUserLogin:{0}", Rsp.This[rsp]);
            }
        }

        private void tsmiMstUserLogout_Click(object sender, EventArgs e)
        {
            if (dsMstUser.Count == 0)
                return;
            gvMstUser.EndEdit();
            dsMstUser.EndEdit();
            foreach (CtpMstUser user in dsMstUser)
            {
                if (!user.IsChecked || !user.IsLogin)
                    continue;
                var api = user.TraderApi();
                var req = new CtpUserLogout
                {
                    BrokerID = user.BrokerId,
                    UserID = user.UserId
                };
                var rsp = api.ReqUserLogout(req, user.ReqId);
                _log.DebugFormat("ReqUserLogout:{0}", Rsp.This[rsp]);
            }
        }
        
        private void tsmiSelectAllSubUser_Click(object sender, EventArgs e)
        {
            if (dsSubUser.Count == 0)
                return;
            foreach (CtpUserInfo user in dsSubUser)
            {
                user.IsChecked = true;
            }
            dsSubUser.ResetBindings(false);
        }

        private void tsmiSubUserLogin_Click(object sender, EventArgs e)
        {
            if (dsSubUser.Count == 0)
                return;
            gvSubUser.EndEdit();
            dsSubUser.EndEdit();
            foreach (CtpSubUser user in dsSubUser)
            {
                if (!user.IsChecked || user.IsLogin)
                    continue;
                var api = user.TraderApi();
                var req = new CtpReqUserLogin();
                req.BrokerID = user.BrokerId;
                req.UserID = user.UserId;
                req.Password = user.Password;
                req.UserProductInfo = "JCTP";
                req.ProtocolInfo = "X";
                req.InterfaceProductInfo = "X";
                var reqId = user.ReqId;
                var rsp = api.ReqUserLogin(req, reqId);
                _log.InfoFormat("ReqUserLogout[{0}]:{1}\nrequest:{2}",
                    reqId, Rsp.This[rsp], JsonConvert.SerializeObject(req));
            }
        }

        private void tsmiSubUserLogout_Click(object sender, EventArgs e)
        {
            if (dsSubUser.Count == 0)
                return;
            gvSubUser.EndEdit();
            dsSubUser.EndEdit();
            foreach (CtpSubUser user in dsSubUser)
            {
                if (!user.IsChecked || !user.IsLogin)
                    continue;
                var api = user.TraderApi();
                var req = new CtpUserLogout
                {
                    BrokerID = user.BrokerId,
                    UserID = user.UserId
                };
                var reqId = user.ReqId;
                var rsp = api.ReqUserLogout(req, reqId);
                _log.InfoFormat("ReqUserLogout[{0}]:{1}\nrequest:{2}",
                    reqId, Rsp.This[rsp], JsonConvert.SerializeObject(req));
            }
        }
        #endregion

        private void tsmiListen_Click(object sender, EventArgs e)
        {
            if (_listening)
            {
                timerQryTrade.Stop();
                timerInsertOrder.Stop();
                timerReturnOrder.Stop();
                tsmiListen.Text = "开始监听";
            }
            else
            {
                timerQryTrade.Start();
                timerInsertOrder.Start();
                timerReturnOrder.Start();
                tsmiListen.Text = "停止监听";
            }
        }

        #region 查询主账户成交单
        /// <summary>
        /// 触发查询成交单。
        /// </summary>
        private void timerQryTrade_Tick(object sender, EventArgs e)
        {
            if (!_workerTimerQryTrade.IsBusy)
                _workerTimerQryTrade.RunWorkerAsync();
        }
        
        private void WorkerTimerQryTradeOnDoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            var qry = new CtpQryTrade();
            foreach (CtpMstUser user in dsMstUser)
            {
                if (!user.IsChecked || !user.IsLogin)
                    continue;
                qry.BrokerID = user.BrokerId;
                qry.InvestorID = user.UserId;
                var api = user.Broker.TraderApi;
                api.ReqQryTrade(qry, RequestId.TradeQryId());
            }
        }

        /// <summary>
        /// 查询成交单回报。
        /// </summary>
        private void OnRspQryTrade(object sender, CtpTrade response, CtpRspInfo rspInfo, int requestId, bool isLast)
        {
            _log.DebugFormat("TradeApiOnOnRspQryTrade[requestId={0}]\nresponse:{1}\nrspInfo:{2}",
                requestId,
                JsonConvert.SerializeObject(response), 
                JsonConvert.SerializeObject(rspInfo));
            if (rspInfo != null && rspInfo.ErrorID == 0 && response != null)
                _tradeQueue.Enqueue(response);
        }
        #endregion

        #region 子账户跟单
        /// <summary>
        /// 触发跟单（报单）。
        /// </summary>
        private void timerInsertOrder_Tick(object sender, EventArgs e)
        {
            if(!_workerTimerInsertOrder.IsBusy)
                _workerTimerInsertOrder.RunWorkerAsync();
        }

        private void WorkerTimerInsertOrderOnDoWork(object sender, DoWorkEventArgs args)
        {
            if (_tradeQueue.Count == 0)
                return;
            bool b;
            do
            {
                CtpTrade ctpTrade;
                b = _tradeQueue.TryDequeue(out ctpTrade);
                if (!b || ctpTrade == null)
                    continue;
                var idx = dsSubOrder.Find("OrderSysId", ctpTrade.OrderSysID);
                if (idx >= 0)
                    continue;
                foreach (CtpSubUser u in dsSubUser)
                {
                    if (!u.InsertOrder(ctpTrade))
                        continue;
                    var t = new OrderInfo(ctpTrade);
                    dsSubOrder.Add(t);
                }
            } while (!b);
            dsSubTradeInfo.ResetBindings(false);
        }

        /// <summary>
        /// 手工下单。
        /// </summary>
        private void btnInsertOrder_Click(object sender, EventArgs e)
        {
            if (dsSubUser.Count == 0)
                return;
            gvSubUser.EndEdit();
            dsSubUser.EndEdit();
            for (var i = 0; i < dsSubUser.Count; i++)
            {
                var u = (CtpSubUser) dsSubUser[i];
                if(!u.IsChecked || !u.IsLogin)
                    continue;
                var req = new CtpInputOrder();
                req.BrokerID = u.BrokerId;
                req.InvestorID = u.UserId;
                req.UserID = u.UserId;
                //该字段用来指定该报单是开仓，平仓还是平今仓。
                //该字段是一个长度为5 的字符数组，可以同时用来描述单腿合约和组合合约的报单属性。单腿合约只需要为
                //数组的第1 个元素赋值，组合合约需要为数组的第1 & 2 个元素赋值。字符取值为枚举值，在头文件
                //“ThostFtdcUserApiStruct.h”中可以查到。
                req.CombOffsetFlag = ((char)Convert.ToByte(cmbOffsetFlag.SelectedValue)).ToString();
                req.CombHedgeFlag = ((char) CtpHedgeFlagType.Speculation).ToString();
                req.Direction = Convert.ToByte(cmbDirection.SelectedValue);
                req.InstrumentID = cmbInstrumentId.Text;
                req.OrderRef = u.GetOrderRef();
                req.VolumeTotalOriginal = (int)numVolume.Value;
                req.VolumeCondition = CtpVolumeConditionType.AV;
                req.MinVolume = 1;
                req.ForceCloseReason = CtpForceCloseReasonType.NotForceClose;
                req.IsAutoSuspend = 0;
                req.UserForceClose = 0;
                req.OrderPriceType = CtpOrderPriceTypeType.LimitPrice;
                req.LimitPrice = (double)numPrice.Value;
                req.TimeCondition = CtpTimeConditionType.GFD;

                var reqId = RequestId.OrderInsertId();
                req.RequestID = reqId;
                var rsp = u.TraderApi().ReqOrderInsert(req, reqId);
                _log.DebugFormat("ReqOrderInsert[{0}]:{1}\nrequest:{2}",
                    reqId, Rsp.This[rsp], JsonConvert.SerializeObject(req));
                if (rsp == 0)
                {
                    var od = new OrderInfo(req);
                    od.FrontId = u.FrontId;
                    od.SessionId = u.SessionId;
                    dsSubOrder.Insert(0, od);
                }
            }
        }

        /// <summary>
        /// 成交回报。
        /// </summary>
        private void OnRtnTrade(object sender, CtpTrade response)
        {
            _log.DebugFormat("OnRtnTrade\nresponse:{0}", JsonConvert.SerializeObject(response));
            if (response == null)
                return;
            var ord = new TradeInfo(response);
            dsSubTradeInfo.Add(ord);
            dsSubTradeInfo.ResetBindings(false);
        }

        /// <summary>
        /// 报单回报。
        /// </summary>
        /// <remarks>报单状态发生变化时。</remarks>
        private void OnRtnOrder(object sender, CtpOrder response)
        {
            _log.InfoFormat("OnRtnOrder\nresponse:{0}", 
                JsonConvert.SerializeObject(response));
            if (response == null)
                return;
            for (var i = 0; i < dsSubOrder.Count; i++)
            {
                var od = (OrderInfo)dsSubOrder[i];
                if (od.BrokerId == response.BrokerID &&
                    od.InvestorId == response.InvestorID &&
                    od.OrderRef == response.OrderRef)
                {
                    od.OrderStatus = response.OrderStatus;
                    dsSubOrder.ResetItem(i);
                }
            }
        }

        /// <summary>
        /// 没有通过参数校验，拒绝接受报单指令。用户收到此消息，其中包含了错误编码和错误消息。
        /// </summary>
        private void OnRspOrderInsert(object sender, CtpInputOrder response, CtpRspInfo rspInfo, int requestId, bool isLast)
        {
            _log.ErrorFormat("OnRspOrderInsert[{0}]\nresponse:{1}\nrspInfo:{2}",
                requestId,
                JsonConvert.SerializeObject(response),
                JsonConvert.SerializeObject(rspInfo));
            if (response == null)
                return;
            for (var i = 0; i < dsSubOrder.Count; i++)
            {
                var od = (OrderInfo) dsSubOrder[i];
                if (od.BrokerId == response.BrokerID && 
                    od.InvestorId == response.InvestorID &&
                    od.OrderRef == response.OrderRef)
                {
                    od.ErrorMsg = rspInfo?.ErrorMsg;
                    dsSubOrder.ResetItem(i);
                }
            }
        }

        /// <summary>
        /// 报单录入错误回报。
        /// </summary>
        private void OnErrRtnOrderInsert(object sender, CtpInputOrder response, CtpRspInfo rspInfo)
        {
            _log.ErrorFormat("OnErrRtnOrderInsert\nrspInfo:{0}\nresponse:{1}", 
                JsonConvert.SerializeObject(rspInfo), 
                JsonConvert.SerializeObject(response));
        }

        /// <summary>
        /// 报单响应同步。
        /// </summary>
        private void timerReturnOrder_Tick(object sender, EventArgs e)
        {
            if (_inputOrderQueue.Count == 0)
                return;
            if (!_workerTimerReturnOrder.IsBusy)
                _workerTimerReturnOrder.RunWorkerAsync();
        }

        private void WorkerTimerReturnOrderOnDoWork(object sender, DoWorkEventArgs args)
        {
            CtpOrder od;
            if (!_inputOrderQueue.TryDequeue(out od) || od == null)
                return;
            int p = -1;
            for (var i = 0; i < dsSubOrder.Count; i++)
            {
                var order = (OrderInfo)dsSubOrder[i];
                if (order.ExchangeId == od.ExchangeID &&
                    order.OrderSysId == od.OrderSysID)
                {
                    p = i;
                    break;
                }
            }
            if (p < 0)
            {
                var o = new OrderInfo(od);
                dsSubOrder.Add(o);
            }
            else
            {
                var o = (OrderInfo)dsSubOrder[p];
                o.OrderStatus = od.OrderStatus;
                dsSubOrder.ResetItem(p);
            }
        }
        #endregion

        #region 持仓

        private void QryInvestorPosition()
        {
            var req = new CtpQryInvestorPosition();
            foreach (CtpMstUser u in dsMstUser)
            {
                req.InvestorID = u.UserId;
                u.TraderApi().ReqQryInvestorPosition(req, 0);
            }
            foreach (CtpSubUser u in dsSubUser)
            {
                req.InvestorID = u.UserId;
                u.TraderApi().ReqQryInvestorPosition(req, 0);
            }
        }

        /// <summary>
        /// 持仓查询响应。
        /// </summary>
        private void OnRspQryInvestorPosition(object sender, CtpInvestorPosition response, CtpRspInfo rspInfo, int requestId, bool isLast)
        {
            _log.DebugFormat("OnRspQryInvestorPosition[{0}]\nrspInfo:{1}\nresponse:{2}",
                requestId,
                JsonConvert.SerializeObject(rspInfo), 
                JsonConvert.SerializeObject(response));
            if (rspInfo == null || rspInfo.ErrorID != 0 || response == null)
                return;
            var info = new InvestorPositionInfo(response);
        }

        /// <summary>
        /// 持仓明细查询响应。
        /// </summary>
        private void OnRspQryInvestorPositionDetail(object sender, CtpInvestorPositionDetail response, CtpRspInfo rspInfo, int requestId, bool isLast)
        {
            _log.DebugFormat("OnRspQryInvestorPositionDetail[{0}]\nrspInfo:{1}\nresponse:{2}",
                requestId,
                JsonConvert.SerializeObject(rspInfo),
                JsonConvert.SerializeObject(response));
        }
        #endregion
    }
}
