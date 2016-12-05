using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
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
            tsslTradeApiStatus.Text = string.Empty;
            tsslBroker.Text = string.Empty;
            _listening = false;

            var direction = new List<LookupObject>
            {
                new LookupObject(CtpDirectionType.Buy, "买"),
                new LookupObject(CtpDirectionType.Sell, "卖")
            };
            cmbDirection.Bind(direction);

            var offsetFlag = new List<LookupObject>
            {
                new LookupObject(CtpOffsetFlagType.Open, "开"),
                new LookupObject(CtpOffsetFlagType.Close, "平")
            };
            cmbOffsetFlag.Bind(offsetFlag);

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
                var b = u.Broker;
                var api = b.InitApi();
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
                api.SubscribePrivateTopic(CtpResumeType.Quick);
                api.SubscribePublicTopic(CtpResumeType.Quick);
                b.Start();
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
        private void OnRspSettlementInfoConfirm(object sender, CtpSettlementInfoConfirm response, CtpRspInfo rspInfo, int requestId, bool isLast)
        {
            _log.DebugFormat("TradeApiOnRspSettlementInfoConfirm\nresponse:{0}\nrspInfo:{1}", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(rspInfo));
        }

        private void OnRspQrySettlementInfo(object sender, CtpSettlementInfo response, CtpRspInfo rspInfo, int requestId, bool isLast)
        {
            _log.DebugFormat("TradeApiOnRspQrySettlementInfo\nresponse:{0}\nrspInfo:{1}", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(rspInfo));
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
            _log.DebugFormat("TradeApiOnRspUserLogin[{0}]\nresponse:{1}\nrspInfo:{2}",
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
                var u = (CtpUserInfo)dsSubUser[i];
                if (u.ReqId == requestId)
                {
                    u.IsLogin = true;
                    dsSubUser.ResetItem(i);
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
                var api = user.TraderApi;
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
                var api = user.TraderApi;
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
                var api = user.TraderApi;
                var userLoginReq = new CtpReqUserLogin();
                userLoginReq.BrokerID = user.BrokerId;
                userLoginReq.UserID = user.UserId;
                userLoginReq.Password = user.Password;
                userLoginReq.UserProductInfo = "JCTP";
                userLoginReq.ProtocolInfo = "X";
                userLoginReq.InterfaceProductInfo = "X";
                var rsp = api.ReqUserLogin(userLoginReq, user.ReqId);
                _log.InfoFormat("ReqUserLogin:{0}", Rsp.This[rsp]);
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
                var api = user.TraderApi;
                var req = new CtpUserLogout
                {
                    BrokerID = user.BrokerId,
                    UserID = user.UserId
                };
                var rsp = api.ReqUserLogout(req, user.ReqId);
                _log.InfoFormat("ReqUserLogout:{0}", Rsp.This[rsp]);
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
            var user = (CtpSubUser) dsSubUser.Current;
            if (user == null)
            {
                MsgBox.Info("请选中要下单的子账户");
                return;
            }
            var req = new CtpInputOrder();
            //该字段用来指定该报单是开仓，平仓还是平今仓。
            //该字段是一个长度为5 的字符数组，可以同时用来描述单腿合约和组合合约的报单属性。单腿合约只需要为
            //数组的第1 个元素赋值，组合合约需要为数组的第1 & 2 个元素赋值。字符取值为枚举值，在头文件
            //“ThostFtdcUserApiStruct.h”中可以查到。
            req.CombOffsetFlag = ((char)Convert.ToByte(cmbOffsetFlag.SelectedValue)).ToString();
            req.Direction = Convert.ToByte(cmbDirection.SelectedValue);
            req.InstrumentID = cmbInstrumentId.Text;
            req.LimitPrice = (double)numPrice.Value;
            req.VolumeTotalOriginal = (int)numVolume.Value;
            var reqId = RequestId.OrderInsertId();
            var rsp = user.TraderApi.ReqOrderInsert(req, reqId);
            _log.DebugFormat("ReqOrderInsert[{0}],rsp[{1}]\nrequest:{2}",
                reqId, rsp, JsonConvert.SerializeObject(req));
            MsgBox.Info(Rsp.This[rsp]);
        }

        /// <summary>
        /// 成交回报。
        /// </summary>
        private void OnRtnTrade(object sender, CtpTrade response)
        {
            _log.DebugFormat("TradeApiOnRtnTrade\nresponse:{0}", JsonConvert.SerializeObject(response));
            if (response == null)
                return;
            var ord = new OrderInfo(response);
            dsSubOrder.Add(ord);
            dsSubOrder.ResetBindings(false);
        }

        /// <summary>
        /// 报单回报。
        /// </summary>
        /// <remarks>报单状态发生变化时。</remarks>
        private void OnRtnOrder(object sender, CtpOrder response)
        {
            _log.InfoFormat("TradeApiOnRtnOrder\nresponse:{0}", JsonConvert.SerializeObject(response));
            if (response != null)
                _inputOrderQueue.Enqueue(response);
        }

        /// <summary>
        /// 没有通过参数校验，拒绝接受报单指令。用户收到此消息，其中包含了错误编码和错误消息。
        /// </summary>
        private void OnRspOrderInsert(object sender, CtpInputOrder response, CtpRspInfo rspInfo, int requestId, bool isLast)
        {
            _log.ErrorFormat("TradeApiOnRspOrderInsert[requestId={0}]\nresponse:{1}\nrspInfo:{2}",
                requestId,
                JsonConvert.SerializeObject(response),
                JsonConvert.SerializeObject(rspInfo));
        }

        /// <summary>
        /// 报单录入错误回报。
        /// </summary>
        private void OnErrRtnOrderInsert(object sender, CtpInputOrder response, CtpRspInfo rspInfo)
        {
            _log.ErrorFormat("OnErrRtnOrderInsert\nrspInfo:{0}\nresponse:{1}", JsonConvert.SerializeObject(rspInfo), JsonConvert.SerializeObject(response));
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
                u.TraderApi.ReqQryInvestorPosition(req, 0);
            }
            foreach (CtpSubUser u in dsSubUser)
            {
                req.InvestorID = u.UserId;
                u.TraderApi.ReqQryInvestorPosition(req, 0);
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
