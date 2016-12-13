using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GalaxyFutures.Sfit.Api;
using log4net;
using Newtonsoft.Json;

namespace WinCtp
{
    public partial class FrmMain : Form , IMainView
    {
        private readonly ILog _log;
        private readonly  ConcurrentQueue<CtpTrade> _tradeQueue;

        private readonly BackgroundWorker _workerQryTrade;
        private readonly BackgroundWorker _workerFollowOrder;

        private readonly IDictionary<string, BindingSource> _dicds;

        private readonly TaskScheduler _syncSch;

        #region 初始化
        public FrmMain()
        {
            InitializeComponent();
            _log = LogManager.GetLogger("CTP");
            _tradeQueue = new ConcurrentQueue<CtpTrade>();
            _dicds = new ConcurrentDictionary<string, BindingSource>();
            _workerQryTrade = new BackgroundWorker();
            _workerFollowOrder = new BackgroundWorker();
            _syncSch = TaskScheduler.FromCurrentSynchronizationContext();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            tcSubInstrument.TabPages.Clear();
            tcMstInstrument.TabPages.Clear();
            _workerQryTrade.DoWork += OnDoWorkQryTrade;
            _workerFollowOrder.DoWork += OnDoWorkFollowOrder;

            LoadBaseInfo();

            timerQryTrade.Enabled = false;
            timerQryTrade.Interval = 10*1000;
            timerFollowOrder.Enabled = true;
            timerFollowOrder.Interval = 5 * 1000;

            var brokers = BrokerInfo.GetAll();
            var users = UserInfoList.Load();
            var mstUsers = users.GetMst();
            dsMstUser.DataSource = mstUsers;
            var subUsers = users.GetSub();
            dsSubUser.DataSource = subUsers;

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

                api.SubscribePrivateTopic(CtpResumeType.Restart);
                api.SubscribePublicTopic(CtpResumeType.Quick);
                
                ua.Start();
                UserApi.This[u.UserId] = ua;
            }
        }

        private void LoadBaseInfo()
        {
            //买卖
            var ludir = new List<LookupObject>
            {
                new LookupObject(CtpDirectionType.Buy, "买"),
                new LookupObject(CtpDirectionType.Sell, "卖")
            };
            cmbDirection.Bind(ludir);
            gcSubOrderDirection.Bind(ludir);
            gcSubTradeDirection.Bind(ludir);
            //开平标志
            var luof = new List<LookupObject>
            {
                new LookupObject(CtpOffsetFlagType.Open, "开"),
                new LookupObject(CtpOffsetFlagType.Close, "平")
            };
            cmbOffsetFlag.Bind(luof);
            gcSubTradeOffsetFlag.Bind(luof);
            //组合开平标志
            var lucof = new List<LookupObject>
            {
                new LookupObject(((char)CtpOffsetFlagType.Open).ToString(), "开"),
                new LookupObject(((char)CtpOffsetFlagType.Close).ToString(), "平")
            };
            gcSubOrderCombOffsetFlag.Bind(lucof);
            //报单状态
            var lustatus = new List<LookupObject>
            {
                new LookupObject(CtpOrderStatusType.Unknown, "未知"),//表示Thost已经接受用户的委托指令，还没有转发到交易所
                new LookupObject(CtpOrderStatusType.AllTraded, "全部成交"),
                new LookupObject(CtpOrderStatusType.Canceled, "撤单"),
                new LookupObject(CtpOrderStatusType.NoTradeNotQueueing, "未成交O队"),//未成交不在队列中
                new LookupObject(CtpOrderStatusType.NoTradeQueueing, "未成交I队"),//未成交还在队列中
                new LookupObject(CtpOrderStatusType.NotTouched, "未触发"),
                new LookupObject(CtpOrderStatusType.PartTradedNotQueueing, "部分成交O队"),//部分成交不在队列中
                new LookupObject(CtpOrderStatusType.PartTradedQueueing, "部分成交I队"),//部分成交还在队列中
                new LookupObject(CtpOrderStatusType.Touched, "已触发")
            };
            gcSubOrderStatus.Bind(lustatus);
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
                var reqId = RequestId.NewId();
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
            if (rspInfo != null && rspInfo.ErrorID != 0)
                return;
            if (response == null)
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
            if (rspInfo != null && rspInfo.ErrorID != 0)
                return;
            if (response == null)
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
            _log.DebugFormat("OnFrontDisconnected:response[{0}]", response);
        }

        private void OnFrontConnected(object sender)
        {
            _log.Debug("OnFrontConnected");
        }

        private void OnRspError(object sender, CtpRspInfo rspInfo, int requestId, bool isLast)
        {
            _log.DebugFormat("OnRspError[{0}]\nrspInfo:{1}",
                requestId,
                JsonConvert.SerializeObject(rspInfo));
        }

        #region 账户

        private void OnRspUserLogout(object sender, CtpUserLogout response, CtpRspInfo rspInfo, int requestId, bool isLast)
        {
            _log.DebugFormat("OnRspUserLogout[{0}]\nresponse:{1}\nrspInfo:{2}",
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
                var u = (CtpSubUser)dsSubUser[i];
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
            //主账户登录
            for (var i = 0; i < dsMstUser.Count; i++)
            {
                var u = (CtpUserInfo)dsMstUser[i];
                if (u.ReqId != requestId)
                    continue;
                u.IsLogin = true;
                dsMstUser.ResetItem(i);
                QryInvestorPosition(u);
                return;
            }
            //子账户登录
            for (var i = 0; i < dsSubUser.Count; i++)
            {
                var u = (CtpSubUser)dsSubUser[i];
                if (u.ReqId != requestId)
                    continue;
                u.IsLogin = true;
                if (response != null)
                {
                    u.FrontId = response.FrontID;
                    u.SessionId = response.SessionID;
                    u.MaxOrderRef = Convert.ToInt32(response.MaxOrderRef);
                }
                dsSubUser.ResetItem(i);

                if (!_dicds.ContainsKey(u.UserId))
                    _dicds[u.UserId] = new BindingSource { DataSource = typeof(InvestorPositionInfo) };
                var ds = _dicds[u.UserId];
                ds.Clear();

                //得到一个同步上下文调度器
                //var t = new Task(() =>
                //{
                //    var tp = new TabPage(u.UserId);
                //    tp.Name = $"tpp{u.UserId}";
                //    var gv = CreatePosGridView(u.UserId);
                //    gv.DataSource = ds;
                //    tp.Controls.Add(gv);
                //    tcSubInstrument.TabPages.Add(tp);
                //});

                ////在Task的ContinueWith方法中，指定这个同步上下文调度器，我们更新了form的Text属性
                ////去掉这个syncSch，你就会发现要出异常
                //t.ContinueWith(task => task, _syncSch);
                //t.Start();

                QrySettlementInfoConfirm(u);
                //QryInvestorPosition(u);
                return;
            }
        }

        private static DataGridViewEx CreatePosGridView(string userId)
        {
            var gv = new DataGridViewEx();
            gv.Visible = true;
            gv.Name = $"gvp{userId}";
            gv.Columns.AddRange(
                new DataGridViewTextBoxColumn
                {
                    Name = $"gcInvestorId{userId}",
                    HeaderText = "投资者",
                    DataPropertyName = "InvestorId",
                    ReadOnly = true,
                    Width = 78
                }, new DataGridViewTextBoxColumn
                {
                    Name = $"gcInstrumentId{userId}",
                    HeaderText = "合约",
                    DataPropertyName = "InstrumentId",
                    ReadOnly = true,
                    Width = 78
                });
            gv.Dock = DockStyle.Fill;
            return gv;
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
                _log.InfoFormat("ReqUserLogin[{0}]:{1}\nrequest:{2}",
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
            if (timerQryTrade.Enabled)
            {
                timerQryTrade.Stop();
                tsmiListen.Text = "启动监听";
            }
            else
            {
                timerQryTrade.Start();
                tsmiListen.Text = "停止监听";
            }
        }

        #region 查询主账户成交单
        /// <summary>
        /// 触发查询主账户成交单。
        /// </summary>
        private void timerQryTrade_Tick(object sender, EventArgs e)
        {
            if (!_workerQryTrade.IsBusy)
                _workerQryTrade.RunWorkerAsync();
        }
        
        private void OnDoWorkQryTrade(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            var qry = new CtpQryTrade();
            foreach (CtpMstUser user in dsMstUser)
            {
                if (!user.IsChecked || !user.IsLogin)
                    continue;
                qry.BrokerID = user.BrokerId;
                qry.InvestorID = user.UserId;
                qry.InstrumentID = "TA705";
                qry.ExchangeID = "";
                var api = user.TraderApi();
                var reqId = RequestId.TradeQryId();
                var rsp = api.ReqQryTrade(qry, reqId);
                _log.DebugFormat("ReqQryTrade[{0}]:{1}\nrequest:{2}", 
                    reqId, Rsp.This[rsp],
                    JsonConvert.SerializeObject(qry));
            }
        }

        /// <summary>
        /// 查询成交单回报。
        /// </summary>
        private void OnRspQryTrade(object sender, CtpTrade response, CtpRspInfo rspInfo, int requestId, bool isLast)
        {
            _log.DebugFormat("OnRspQryTrade[{0}]\nresponse:{1}\nrspInfo:{2}",
                requestId,
                JsonConvert.SerializeObject(response), 
                JsonConvert.SerializeObject(rspInfo));
            if (rspInfo != null && rspInfo.ErrorID == 0 && response != null)
                _tradeQueue.Enqueue(response);
        }
        #endregion

        #region 子账户跟单
        /// <summary>
        /// 触发子账户跟单。
        /// </summary>
        /// <remarks>从主账号成交单队列获取跟单数据。</remarks>
        private void timerFollowOrder_Tick(object sender, EventArgs e)
        {
            if (_tradeQueue.Count == 0)
                return;
            if (!_workerFollowOrder.IsBusy)
                _workerFollowOrder.RunWorkerAsync();
        }

        /// <summary>
        /// 处理子账户跟单。
        /// </summary>
        private void OnDoWorkFollowOrder(object sender, DoWorkEventArgs args)
        {
            bool b;
            do
            {
                CtpTrade ctpTrade;
                b = _tradeQueue.TryDequeue(out ctpTrade);
                if (!b || ctpTrade == null)
                    continue;
                //同步到主账户成交单列表
                SyncToMstTrade(ctpTrade);
                //
                string fk = $"{ctpTrade.ExchangeID}_{ctpTrade.OrderSysID}";//跟单主键

                var idx = dsSubOrder.Find("OrderSysId", ctpTrade.OrderSysID);
                if (idx >= 0)
                    continue;
                foreach (CtpSubUser u in dsSubUser)
                {
                    if(!u.IsLogin)
                        continue;
                    var req = u.FollowOrder(ctpTrade);
                    if(req == null)
                        continue;
                    var e = dsSubOrder.Cast<OrderBase>().Any(o => o.InvestorId == u.UserId && o.FollowKey == fk);
                    if (!e)
                        e = dsSubTradeInfo.Cast<OrderBase>().Any(o => o.InvestorId == u.UserId && o.FollowKey == fk);
                    if (e)
                        continue;//该单已经跟过
                    var reqId = RequestId.OrderInsertId();
                    req.RequestID = reqId;
                    var rsp = u.TraderApi().ReqOrderInsert(req, reqId);
                    _log.DebugFormat("ReqOrderInsert[{0}]:{1}\n{2}", 
                        reqId, Rsp.This[rsp], 
                        JsonConvert.SerializeObject(req));
                    var t = new OrderInfo(req);
                    t.FollowKey = fk;
                    dsSubOrder.Add(t);
                }
            } while (!b);
            dsSubTradeInfo.ResetBindings(false);
        }

        private void SyncToMstTrade(CtpTrade trade)
        {
            var e = dsMstTradeInfo.Cast<TradeInfo>().Any(o => o.ExchangeId == trade.ExchangeID && o.OrderSysId == trade.OrderSysID);
            if (e)
                return;
            var t = new TradeInfo(trade);
            dsMstTradeInfo.Add(t);
        }

        /// <summary>
        /// 手工下单。
        /// </summary>
        /// <remarks>
        /// 报单必须输入的字段列表：
        /// BrokerID、InvestorID、InstrumentID、ExchangeID、
        /// OrderPriceType、Direction、VolumeTotalOriginal、TimeCondition、VolumeCondition、
        /// ContingentCondition 、ForceCloseReason。
        /// </remarks>
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
                req.InstrumentID = cmbInstrumentId.Text;
                //该字段用来指定该报单是开仓，平仓还是平今仓。
                //该字段是一个长度为5 的字符数组，可以同时用来描述单腿合约和组合合约的报单属性。单腿合约只需要为
                //数组的第1 个元素赋值，组合合约需要为数组的第1 & 2 个元素赋值。字符取值为枚举值，在头文件
                //“ThostFtdcUserApiStruct.h”中可以查到。
                req.CombOffsetFlag = ((char)Convert.ToByte(cmbOffsetFlag.SelectedValue)).ToString();
                req.CombHedgeFlag = ((char)CtpHedgeFlagType.Speculation).ToString();
                req.OrderPriceType = CtpOrderPriceTypeType.LimitPrice;
                req.Direction = Convert.ToByte(cmbDirection.SelectedValue);
                req.VolumeTotalOriginal = (int)numVolume.Value;
                req.TimeCondition = CtpTimeConditionType.GFD;
                req.GTDDate = "";
                req.VolumeCondition = CtpVolumeConditionType.AV;
                req.ContingentCondition = CtpContingentConditionType.Immediately;
                req.ForceCloseReason = CtpForceCloseReasonType.NotForceClose;

                req.OrderRef = u.GetOrderRef();
                req.MinVolume = 1;
                req.IsAutoSuspend = 0;
                req.UserForceClose = 0;
                req.LimitPrice = (double)numPrice.Value;
 
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
        /// 报单成交回报。
        /// </summary>
        private void OnRtnTrade(object sender, CtpTrade response)
        {
            // ExchangeID + OrderSysID
            _log.DebugFormat("OnRtnTrade\nresponse:{0}", 
                JsonConvert.SerializeObject(response));
            if (response == null)
                return;
            var idx = -1;
            for (var i = 0; i < dsSubOrder.Count; i++)
            {
                var od = (OrderInfo)dsSubOrder[i];
                if (od.ExchangeId != response.ExchangeID || 
                    od.OrderSysId != response.OrderSysID)
                    continue;
                idx = i;
                break;
            }
            //从委托单列表移除
            if(idx >= 0)
                dsSubOrder.RemoveAt(idx);
            //添加到成交单列表
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
            //收到委托回报时，使用 FrontID、SessionID 过滤出自己的委托回报。同时记下关联的ExchangeID、OrderSysID。
            _log.InfoFormat("OnRtnOrder\nresponse:{0}", 
                JsonConvert.SerializeObject(response));
            if (response == null)
                return;
            for (var i = 0; i < dsSubOrder.Count; i++)
            {
                var od = (OrderInfo)dsSubOrder[i];
                if (od.FrontId != response.FrontID || 
                    od.SessionId != response.SessionID ||
                    od.OrderRef != response.OrderRef)
                    continue;
                od.ExchangeId = response.ExchangeID;
                od.OrderSysId = response.OrderSysID;
                od.OrderStatus = response.OrderStatus;
                od.ErrorMsg = response.StatusMsg;
                dsSubOrder.ResetItem(i);
                Thread.Sleep(500);
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
            if (response == null || rspInfo == null)
                return;
            for (var i = 0; i < dsSubOrder.Count; i++)
            {
                var od = (OrderInfo) dsSubOrder[i];
                if (od.BrokerId == response.BrokerID && 
                    od.InvestorId == response.InvestorID &&
                    od.OrderRef == response.OrderRef)
                {
                    od.ErrorMsg = $"[{rspInfo.ErrorID}]{rspInfo.ErrorMsg}";
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
        #endregion

        #region 持仓

        /// <summary>
        /// 查询持仓。
        /// </summary>
        private void QryInvestorPosition(CtpUserInfo user)
        {
            var req = new CtpQryInvestorPosition();
            req.BrokerID = user.BrokerId;
            req.InvestorID = user.UserId;
            var reqId = RequestId.NewId();
            var rsp = user.TraderApi().ReqQryInvestorPosition(req, reqId);
            _log.DebugFormat("ReqQryInvestorPosition[{0}]:{1}\n{2}",
                reqId,
                Rsp.This[rsp],
                JsonConvert.SerializeObject(req));
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
            foreach (CtpUserInfo u in dsSubUser)
            {
                if(u.UserId != response.InvestorID)
                    continue;
                var ds = _dicds[response.InvestorID];
                var info = new InvestorPositionInfo(response);
                ds.Add(info);
                return;
            }
            foreach (CtpUserInfo u in dsMstUser)
            {
                if (u.UserId != response.InvestorID)
                    continue;
                var ds = _dicds[response.InvestorID];
                var info = new InvestorPositionInfo(response);
                ds.Add(info);
                return;
            }
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
            if (rspInfo == null || rspInfo.ErrorID != 0 || response == null)
                return;
        }
        #endregion
    }
}
