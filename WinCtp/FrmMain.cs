﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Amib.Threading;
using GalaxyFutures.Sfit.Api;
using log4net;
using Newtonsoft.Json;

namespace WinCtp
{
    internal delegate void SimpleDelegate();

    /// <summary>
    /// CTP.NET
    /// </summary>
    /// <remarks>
    /// CTP 仅对查询进行流量限制，对交易指令没有限制。如果有在途的查询，不允
    /// 许发新的查询。1 秒钟最多允许发送 1 个查询。返回值“-2”表示“未处理请求超过许
    /// 可数”，“-3”表示“每秒发送请求数超过许可数”。
    /// </remarks>
    public partial class FrmMain : Form , IMainView
    {
        private readonly ILog _log;
        private bool _listening;

        private readonly ConcurrentQueue<CtpInvestorPosition> _positionQueue;//持仓队列

        private readonly IDictionary<string, BindingSource> _dicds;

        private bool _qryInstrumentDone;//标识合约是否已查询
        private readonly IWorkItemsGroup _treadPool;

        private readonly ConcurrentDictionary<string, bool> _loginUser;//[UserID,MstOrSub] true/Mst,false/Sub
        private readonly MainViewImpl _impl;

        private MdApi _mdApi;

        #region 初始化
        public FrmMain()
        {
            InitializeComponent();
            tcSubInstrument.TabPages.Clear();
            tcMstInstrument.TabPages.Clear();
            _log = LogManager.GetLogger("CTP");
            _impl = new MainViewImpl(this);

            _loginUser = new ConcurrentDictionary<string, bool>();
            _positionQueue = new ConcurrentQueue<CtpInvestorPosition>();
            _dicds = new ConcurrentDictionary<string, BindingSource>();
            _qryInstrumentDone = false;
            _listening = false;
            _treadPool = _impl.InitThreedPool();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            tpMstOrder.Parent = null;
            cmbInstrumentId.Items.Clear();
            cmbInstrumentId.Items.Add(string.Empty);

            LoadBaseInfo();

            var brokers = BrokerInfo.GetAll();
            var users = UserInfoList.Load();
            var mstUsers = users.GetMst();
            dsMstUser.DataSource = mstUsers;
            foreach (var u in mstUsers)
            {
                if (!_dicds.ContainsKey(u.UserId))
                    _dicds[u.UserId] = new BindingSource { DataSource = typeof(InvestorPositionInfo) };
                var ds = _dicds[u.UserId];
                ds.Clear();
                var tp = new TabPage(u.UserId);
                tp.Name = $"tpp{u.UserId}";
                var gv = _impl.CreatePositionView(u.UserId);
                gv.DataSource = ds;
                tp.Controls.Add(gv);
                tcMstInstrument.TabPages.Add(tp);
            }
            var subUsers = users.GetSub();
            foreach (var u in subUsers)
            {
                if (!_dicds.ContainsKey(u.UserId))
                    _dicds[u.UserId] = new BindingSource { DataSource = typeof(InvestorPositionInfo) };
                var ds = _dicds[u.UserId];
                ds.Clear();
                var tp = new TabPage(u.UserId);
                tp.Name = $"tpp{u.UserId}";
                var gv = _impl.CreatePositionView(u.UserId);
                gv.DataSource = ds;
                tp.Controls.Add(gv);
                tcSubInstrument.TabPages.Add(tp);
            }
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

                api.OnRtnTrade += OnRtnTrade;
                api.OnRtnOrder += OnRtnOrder;
                api.OnRspOrderInsert += OnRspOrderInsert;
                api.OnErrRtnOrderInsert += OnErrRtnOrderInsert;
                api.OnRspOrderAction += OnRspOrderAction;
                api.OnErrRtnOrderAction += OnErrRtnOrderAction;

                api.OnRspQryInvestorPosition += OnRspQryInvestorPosition;
                api.OnRspQryInvestorPositionDetail += OnRspQryInvestorPositionDetail;

                api.OnRspQrySettlementInfo += OnRspQrySettlementInfo;
                api.OnRspSettlementInfoConfirm += OnRspSettlementInfoConfirm;
                api.OnRspQrySettlementInfoConfirm += OnRspQrySettlementInfoConfirm;
                api.OnRspQryInstrument += OnRspQryInstrument;

                //TERT_RESTART:从本交易日开始重传，
                //TERT_RESUME:从上次收到的续传，
                //TERT_QUICK: 只传送登录后的内容。
                //每次都重传是因为在订阅时（SubscribePrivateTopic / SubscribePublicTopic）选择了 TERT_RESTART 方式
                api.SubscribePrivateTopic(CtpResumeType.Quick);
                api.SubscribePublicTopic(CtpResumeType.Quick);
                
                ua.Start();
                UserApi.This[u.UserId] = ua;
            }
        }

        private void LoadBaseInfo()
        {
            //买卖
            var ludir = _impl.GetDirectionLookup();
            cmbDirection.Bind(ludir);
            gcSubOrderDirection.Bind(ludir);
            gcSubTradeDirection.Bind(ludir);
            gcMstTradeDirection.Bind(ludir);
            //开平标志
            var luof = _impl.GetOffsetFlagLookup();
            cmbOffsetFlag.Bind(luof);
            gcSubTradeOffsetFlag.Bind(luof);
            gcMstTradeOffsetFlag.Bind(luof);
            //组合开平标志
            var lucof = _impl.GetCombOffsetFlagLookup();
            gcSubOrderCombOffsetFlag.Bind(lucof);
            //报单状态
            var lustatus = _impl.GetOrderStatusLookup();
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

        private void ibtnSetting_Click(object sender, EventArgs e)
        {
            new FrmSetting().ShowDialog(this);
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
            var reqId = RequestId.NewId();
            var rsp = api.ReqQrySettlementInfoConfirm(req, reqId);
            _log.DebugFormat("ReqQrySettlementInfoConfirm[{0}]:{1}\nrequest:{2}", reqId,
                Rsp.This[rsp], JsonConvert.SerializeObject(req));
        }

        /// <summary>
        /// 查询合约。
        /// </summary>
        private void QryInstrument(CtpUserInfo u)
        {
            var api = u.TraderApi();
            var req = new CtpQryInstrument();
            var reqId = RequestId.NewId();
            var rsp = api.ReqQryInstrument(req, reqId);
            _log.DebugFormat("ReqQryInstrument[{0}]:{1}\nrequest:{2}", reqId,
                Rsp.This[rsp], JsonConvert.SerializeObject(req));
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
            _log.InfoFormat("{0}.OnFrontDisconnected\nresponse:{1}", sender,response);
        }

        private void OnFrontConnected(object sender)
        {
            _log.InfoFormat("{0}.OnFrontConnected", sender);
        }

        private void OnRspError(object sender, CtpRspInfo rspInfo, int requestId, bool isLast)
        {
            _log.DebugFormat("{0}.OnRspError[{1}]\nrspInfo:{2}",
                sender,
                requestId,
                JsonConvert.SerializeObject(rspInfo));
        }

        private void OnRspQryInstrument(object sender, CtpInstrument response, CtpRspInfo rspInfo, int requestId, bool isLast)
        {
            _log.DebugFormat("OnRspQryInstrument[{0}]\nresponse:{1}\nrspInfo:{2}",
                requestId,
                JsonConvert.SerializeObject(response),
                JsonConvert.SerializeObject(rspInfo));
            if(rspInfo != null && rspInfo.ErrorID != 0)
                return;
            if(response == null)
                return;
            DataCache.Instrument[response.InstrumentID] = response;
            if (isLast)
            {
                _treadPool.QueueWorkItem(SetInstrumentCombox);
                _treadPool.QueueWorkItem(SubscribeMarketData);
            }
        }

        private void SetInstrumentCombox()
        {
            cmbInstrumentId.Items.Clear();
            cmbInstrumentId.Items.Add(string.Empty);
            var items = DataCache.Instrument.Values;
            foreach (var i in items)
            {
                if (InvokeRequired)
                    Invoke(new SimpleDelegate(() => { cmbInstrumentId.Items.Add(i.InstrumentID); }));
                else cmbInstrumentId.Items.Add(i.InstrumentID);
            }
        }

        private void SubscribeMarketData()
        {
            var items = DataCache.Instrument.Values;
            foreach (var i in items)
            {
                _mdApi.SubscribeMarketData(i.InstrumentID);
            }
        }

        #region 账户

        private void OnRspUserLogout(object sender, CtpUserLogout response, CtpRspInfo rspInfo, int requestId, bool isLast)
        {
            _log.DebugFormat("OnRspUserLogout[{0}]\nresponse:{1}\nrspInfo:{2}",
               requestId,
               JsonConvert.SerializeObject(response),
               JsonConvert.SerializeObject(rspInfo));
            if (rspInfo != null && rspInfo.ErrorID != 0)
                return;
            if (response == null)
                return;
            bool rs;
            _loginUser.TryRemove(response.UserID, out rs);
            for (var i = 0; i < dsMstUser.Count; i++)
            {
                var u = (CtpUserInfo)dsMstUser[i];
                if (!Equals(u.BrokerId, response.BrokerID) || 
                    !Equals(u.UserId, response.UserID))
                    continue;
                u.IsLogin = false;
                dsMstUser.ResetItem(i);
                return;
            }
            for (var i = 0; i < dsSubUser.Count; i++)
            {
                var u = (CtpSubUser)dsSubUser[i];
                if (!Equals(u.BrokerId, response.BrokerID) || 
                    !Equals(u.UserId, response.UserID))
                    continue;
                u.IsLogin = false;
                u.FrontId = 0;
                u.SessionId = 0;
                u.MaxOrderRef = 0;
                dsSubUser.ResetItem(i);
                return;
            }
        }

        private void OnRspUserLogin(object sender, CtpRspUserLogin response, CtpRspInfo rspInfo, int requestId, bool isLast)
        {
            _log.DebugFormat("OnRspUserLogin[{0}]\nresponse:{1}\nrspInfo:{2}",
                requestId,
                JsonConvert.SerializeObject(response), 
                JsonConvert.SerializeObject(rspInfo));
            if (rspInfo != null && rspInfo.ErrorID != 0)
                return;
            if(response == null)
                return;
            //主账户登录
            for (var i = 0; i < dsMstUser.Count; i++)
            {
                var u = (CtpUserInfo)dsMstUser[i];
                if (!Equals(u.BrokerId,response.BrokerID) || 
                    !Equals(u.UserId,response.UserID))
                    continue;
                u.IsLogin = true;
                dsMstUser.ResetItem(i);
                _loginUser[response.UserID] = true;
                //查询持仓
                QryInvestorPosition(u);
                return;
            }
            //子账户登录
            for (var i = 0; i < dsSubUser.Count; i++)
            {
                var u = (CtpSubUser)dsSubUser[i];
                if (!Equals(u.BrokerId, response.BrokerID) ||
                    !Equals(u.UserId, response.UserID))
                    continue;
                u.IsLogin = true;
                u.FrontId = response.FrontID;
                u.SessionId = response.SessionID;
                u.MaxOrderRef = Convert.ToInt32(response.MaxOrderRef);
                dsSubUser.ResetItem(i);
                _loginUser[response.UserID] = false;
                //查询结算确认
                QrySettlementInfoConfirm(u);
                //查询持仓
                _treadPool.QueueWorkItem(() => { Thread.Sleep(1100); QryInvestorPosition(u); });
                //启动行情
                if (_mdApi == null && !string.IsNullOrWhiteSpace(u.Broker.MarketFrontAddress))
                {
                    lock (typeof(MdApi))
                    {
                        if (_mdApi == null)
                        {
                            _treadPool.QueueWorkItem(() =>
                            {
                                _mdApi = new MdApi(u.BrokerId, u.UserId, u.Password, u.Broker.MarketFrontAddress, this);
                                _mdApi.Start();
                            });
                        }
                    }
                }
                //查询合约
                if (!_qryInstrumentDone)
                {
                    _treadPool.QueueWorkItem(() =>
                    {
                        Thread.Sleep(2200);
                        QryInstrument(u);
                    });
                    _qryInstrumentDone = true;
                }
                return;
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
                userLoginReq.UserProductInfo = "CTP.NET";
                userLoginReq.ProtocolInfo = "CTP.NET";
                userLoginReq.InterfaceProductInfo = "CTP.NET";
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
                req.UserProductInfo = "CTP.NET";
                req.ProtocolInfo = "CTP.NET";
                req.InterfaceProductInfo = "CTP.NET";
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
            if (_listening)
            {
                _listening = false;
                tsmiListen.Text = "启动监听";
            }
            else
            {
                _listening = true;
                tsmiListen.Text = "停止监听";
            }
        }

        #region 子账户跟单

        /// <summary>
        /// 处理子账户跟单。
        /// </summary>
        private void FollowOrder(CtpTrade ctpTrade)
        {
            //新的成交单，子账户跟单
            foreach (CtpSubUser u in dsSubUser)
            {
                if (!u.IsLogin)
                    continue;
                var req = u.FollowOrder(ctpTrade);
                if (req == null)
                    continue;
                var reqId = RequestId.OrderInsertId();
                req.RequestID = reqId;
                var rsp = u.TraderApi().ReqOrderInsert(req, reqId);
                _log.DebugFormat("ReqOrderInsert[{0}]跟单:{1}\n{2}",
                    reqId, Rsp.This[rsp],
                    JsonConvert.SerializeObject(req));
                var t = new OrderInfo(req);
                t.FrontId = u.FrontId;
                t.SessionId = u.SessionId;
                if (rsp != 0)
                    t.ErrorMsg = $"[{rsp}]{Rsp.This[rsp]}";

                if (InvokeRequired)
                    Invoke(new SimpleDelegate(() => { dsSubOrder.Add(t); }));
                else dsSubOrder.Add(t);
            }
        }

        private static bool Equals(string s1,string s2)
        {
            var ss1 = (s1 ?? string.Empty).Trim().ToLower();
            var ss2 = (s2 ?? string.Empty).Trim().ToLower();
            return ss1 == ss2;
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
                _log.DebugFormat("ReqOrderInsert[{0}]下单:{1}\nrequest:{2}",
                    reqId, Rsp.This[rsp], JsonConvert.SerializeObject(req));
                var od = new OrderInfo(req);
                od.FrontId = u.FrontId;
                od.SessionId = u.SessionId;
                if (rsp != 0)
                    od.ErrorMsg = $"[{rsp}]{Rsp.This[rsp]}";
                if (InvokeRequired)
                    Invoke(new SimpleDelegate(() => { dsSubOrder.Insert(0, od); }));
                else dsSubOrder.Insert(0, od);
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
            bool isMst;
            var rs = _loginUser.TryGetValue(response.InvestorID, out isMst);
            if (!rs)//未知账户的回报
                return;
            if (isMst)
            {
                if(_listening)
                    FollowOrder(response);
                var ord = new TradeInfo(response);
                if (InvokeRequired)
                    Invoke(new SimpleDelegate(()=> { dsMstTradeInfo.Add(ord); }));
                else dsMstTradeInfo.Add(ord);
            }
            else
            {
                //添加到成交单列表
                var ord = new TradeInfo(response);
                if (InvokeRequired)
                    Invoke(new SimpleDelegate(() => { dsSubTradeInfo.Add(ord); }));
                else dsSubTradeInfo.Add(ord);
            }
        }

        /// <summary>
        /// 报单回报（报单状态发生变化时）。
        /// </summary>
        /// <remarks>
        /// 1、交易所收到报单后，通过校验。
        /// 2、Thost接受了撤单指令；
        /// 3、交易所收到撤单后，通过校验，执行了撤单操作。
        /// </remarks>
        private void OnRtnOrder(object sender, CtpOrder response)
        {
            //收到委托回报时，使用 FrontID、SessionID、OrderRef过滤出自己的委托回报。同时记下关联的ExchangeID、OrderSysID。
            _log.DebugFormat("OnRtnOrder\nresponse:{0}",
                JsonConvert.SerializeObject(response));
            if (response == null)
                return;
            bool isMst;
            var rs = _loginUser.TryGetValue(response.InvestorID, out isMst);
            if (!rs || isMst)//未知账户的回报或者是主账户的回报
                return;
            
            Invoke(new SimpleDelegate(() =>
            {
                for (var i = 0; i < dsSubOrder.Count; i++)
                {
                    var od = (OrderInfo)dsSubOrder[i];
                    if (Equals(od.FrontId, response.FrontID) ||
                        Equals(od.SessionId, response.SessionID) ||
                        Equals(od.OrderRef, response.OrderRef))
                        continue;
                    od.ExchangeId = response.ExchangeID;
                    od.OrderSysId = response.OrderSysID;
                    od.OrderStatus = response.OrderStatus;
                    od.ErrorMsg = response.StatusMsg;
                    dsSubOrder.ResetItem(i);
                    break;
                }
            }));
        }

        /// <summary>
        /// Thost收到报单指令，如果没有通过参数校验，拒绝接受报单指令。用户就会收到OnRspOrderInsert消息，其中包含了错误编码和错误消息。
        /// 如果Thost接受了报单指令，用户不会收到OnRspOrderInser，而会收到OnRtnOrder，用来更新委托状态。
        /// </summary>
        private void OnRspOrderInsert(object sender, CtpInputOrder response, CtpRspInfo rspInfo, int requestId, bool isLast)
        {
            _log.ErrorFormat("OnRspOrderInsert[{0}]\nresponse:{1}\nrspInfo:{2}",
                requestId,
                JsonConvert.SerializeObject(response),
                JsonConvert.SerializeObject(rspInfo));
            if (response == null || rspInfo == null)
                return;
            bool isMst;
            var rs = _loginUser.TryGetValue(response.InvestorID, out isMst);
            if (!rs)//未知账户的回报
                return;
            if(isMst)
                return;
            Invoke(new SimpleDelegate(() =>
            {
                for (var i = 0; i < dsSubOrder.Count; i++)
                {
                    var od = (OrderInfo)dsSubOrder[i];
                    if (!Equals(od.BrokerId, response.BrokerID) ||
                        !Equals(od.InvestorId, response.InvestorID) ||
                        !Equals(od.OrderRef, response.OrderRef))
                        continue;
                    od.ErrorMsg = $"[{rspInfo.ErrorID}]{rspInfo.ErrorMsg}";
                    dsSubOrder.ResetItem(i);
                    break;
                }
            }));
        }

        /// <summary>
        /// 如果交易所认为报单错误，用户就会收到OnErrRtnOrder。
        /// </summary>
        private void OnErrRtnOrderInsert(object sender, CtpInputOrder response, CtpRspInfo rspInfo)
        {
            _log.ErrorFormat("OnErrRtnOrderInsert\nrspInfo:{0}\nresponse:{1}", 
                JsonConvert.SerializeObject(rspInfo), 
                JsonConvert.SerializeObject(response));
            if (response == null || rspInfo == null)
                return;
            bool isMst;
            var rs = _loginUser.TryGetValue(response.InvestorID, out isMst);
            if (!rs)//未知账户的回报
                return;
            if (isMst)
                return;
            Invoke(new SimpleDelegate(() =>
            {
                for (var i = 0; i < dsSubOrder.Count; i++)
                {
                    var od = (OrderInfo)dsSubOrder[i];
                    if (!Equals(od.BrokerId, response.BrokerID) ||
                        !Equals(od.InvestorId, response.InvestorID) ||
                        !Equals(od.OrderRef, response.OrderRef))
                        continue;
                    od.ErrorMsg = $"[{rspInfo.ErrorID}]{rspInfo.ErrorMsg}";
                    dsSubOrder.ResetItem(i);
                    break;
                }
            }));
        }

        /// <summary>
        /// Thost收到撤单指令，如果没有通过参数校验，拒绝接受撤单指令。用户就会收到OnRspOrderAction消息，其中包含了错误编码和错误消息。
        /// 如果Thost接受了撤单指令，用户不会收到OnRspOrderAction，而会收到OnRtnOrder，用来更新委托状态。
        /// </summary>
        private void OnRspOrderAction(object sender, CtpInputOrderAction response, CtpRspInfo rspInfo, int requestId, bool isLast)
        {
            _log.DebugFormat("OnRspOrderAction[{0}]\nresponse:{1}\nrspInfo:{2}",
                requestId,
                JsonConvert.SerializeObject(response),
                JsonConvert.SerializeObject(rspInfo));
            if (response == null || rspInfo == null)
                return;
            bool isMst;
            var rs = _loginUser.TryGetValue(response.InvestorID, out isMst);
            if (!rs)//未知账户的回报
                return;
            if (isMst)
                return;
            Invoke(new SimpleDelegate(() =>
            {
                for (var i = 0; i < dsSubOrder.Count; i++)
                {
                    var od = (OrderInfo)dsSubOrder[i];
                    if (!Equals(od.FrontId, response.FrontID) ||
                        !Equals(od.SessionId, response.SessionID) ||
                        !Equals(od.OrderRef, response.OrderRef))
                        continue;
                    od.ErrorMsg = $"[{rspInfo.ErrorID}]{rspInfo.ErrorMsg}";
                    dsSubOrder.ResetItem(i);
                    break;
                }
            }));
        }

        /// <summary>
        /// 如果交易所认为报单错误，用户就会收到OnErrRtnOrderAction
        /// </summary>
        private void OnErrRtnOrderAction(object sender, CtpOrderAction response, CtpRspInfo rspInfo)
        {
            _log.DebugFormat("OnErrRtnOrderAction\nresponse:{0}\nrspInfo:{1}",
                JsonConvert.SerializeObject(response),
                JsonConvert.SerializeObject(rspInfo));
            if (response == null || rspInfo == null)
                return;
            bool isMst;
            var rs = _loginUser.TryGetValue(response.InvestorID, out isMst);
            if (!rs)//未知账户的回报
                return;
            if (isMst)
                return;
            Invoke(new SimpleDelegate(() =>
            {
                for (var i = 0; i < dsSubOrder.Count; i++)
                {
                    var od = (OrderInfo)dsSubOrder[i];
                    if (!Equals(od.FrontId, response.FrontID) ||
                        !Equals(od.SessionId, response.SessionID) ||
                        !Equals(od.OrderRef, response.OrderRef))
                        continue;
                    od.ErrorMsg = $"[{rspInfo.ErrorID}]{rspInfo.ErrorMsg}";
                    dsSubOrder.ResetItem(i);
                    break;
                }
            }));
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
            _log.DebugFormat("OnRspQryInvestorPosition\nrequestId:{0}\nrspInfo:{1}\nresponse:{2}\nisLast:{3}",
                requestId,
                JsonConvert.SerializeObject(rspInfo), 
                JsonConvert.SerializeObject(response), 
                isLast);
            if (rspInfo != null && rspInfo.ErrorID != 0)
                return;
            if (response == null)
                return;
            
            _positionQueue.Enqueue(response);
            if (!isLast)
                return;
            if (InvokeRequired)
                Invoke(new SimpleDelegate(DoRspQryInvestorPosition));
            else DoRspQryInvestorPosition();
        }
        
        private void DoRspQryInvestorPosition()
        {
            CtpInvestorPosition info;
            var rs = _positionQueue.TryDequeue(out info);
            while (rs)
            {
                BindingSource ds;
                if (!_dicds.TryGetValue(info.InvestorID, out ds))
                    return;
                ds.Add(new InvestorPositionInfo(info));
                rs = _positionQueue.TryDequeue(out info);
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
        }

        /// <summary>
        /// 刷新子账户持仓。
        /// </summary>
        private void tsmiRePosiSub_Click(object sender, EventArgs e)
        {
            try
            {
                tsmiRePosiSub.Enabled = false;
                for (var i = 0; i < dsSubUser.Count; i++)
                {
                    var u = (CtpUserInfo)dsSubUser[i];
                    if (!u.IsChecked || !u.IsLogin)
                        continue;
                    BindingSource ds;
                    if (!_dicds.TryGetValue(u.UserId, out ds))
                        return;
                    ds.Clear();
                    ds.ResetBindings(false);
                    QryInvestorPosition(u);
                }
                Thread.Sleep(1000);
            }
            finally
            {
                tsmiRePosiSub.Enabled = true;
            }
        }

        /// <summary>
        /// 刷新主账户持仓。
        /// </summary>
        private void tsmiRePosiMst_Click(object sender, EventArgs e)
        {
            try
            {
                tsmiRePosiMst.Enabled = false;
                for (var i = 0; i < dsMstUser.Count; i++)
                {
                    var u = (CtpUserInfo)dsMstUser[i];
                    if (!u.IsChecked || !u.IsLogin)
                        continue;
                    BindingSource ds;
                    if (!_dicds.TryGetValue(u.UserId, out ds))
                        return;
                    ds.Clear();
                    ds.ResetBindings(false);
                    QryInvestorPosition(u);
                }
                Thread.Sleep(1000);
            }
            finally
            {
                tsmiRePosiMst.Enabled = true;
            }
        }
        #endregion

        private void tsmiCancelOrder_Click(object sender, EventArgs e)
        {
            if (dsSubOrder.Current == null)
                return;
            //不可撤单状态：AllTraded\Canceled\NoTradeNotQueueing\PartTradedNotQueueing
            var od = (OrderInfo)dsSubOrder.Current;
            if (od.OrderStatus == CtpOrderStatusType.AllTraded ||
                od.OrderStatus == CtpOrderStatusType.Canceled ||
                od.OrderStatus == CtpOrderStatusType.NoTradeNotQueueing ||
                od.OrderStatus == CtpOrderStatusType.PartTradedNotQueueing)
            {
                MsgBox.Info("当前状态不可撤单");
                return;
            }
            CtpSubUser user = null;
            foreach (CtpSubUser u in dsSubUser)
            {
                if (u.UserId != od.InvestorId)
                    continue;
                user = u;
                break;
            }
            if (user == null || !user.IsLogin)
            {
                MsgBox.Info($"用户[{od.InvestorId}]未登录");
                return;
            }
            var req = new CtpInputOrderAction();
            req.FrontID = od.FrontId;
            req.SessionID = od.SessionId;
            req.OrderRef = od.OrderRef;
            req.ActionFlag = CtpActionFlagType.Delete;
            req.BrokerID = od.BrokerId;
            req.InvestorID = od.InvestorId;
            var api = user.TraderApi();
            var reqId = RequestId.NewId();
            var rsp = api.ReqOrderAction(req, reqId);
            if (rsp != 0)
                od.ErrorMsg = $"[{rsp}]{Rsp.This[rsp]}";
            dsSubOrder.ResetCurrentItem();

            MsgBox.Info("撤单已提交");
        }

        private void ibtnClose_Click(object sender, EventArgs e)
        {
            if(MsgBox.Ask("您确定关闭程序？"))
                Close();
        }

        public void NotifyMdStatus(int flag,string msg = null)
        {
            switch (flag)
            {
                case 0:
                    tsslMdStatus.ForeColor = Color.Gray;
                    break;
                case 1:
                    tsslMdStatus.ForeColor = Color.Green;
                    break;
            }
        }

        public void NotifyTrade(CtpTrade response)
        {
            // ExchangeID + OrderSysID
            bool isMst;
            var rs = _loginUser.TryGetValue(response.InvestorID, out isMst);
            if (!rs)//未知账户的回报
                return;
            if (isMst)
            {
                if (_listening)
                    FollowOrder(response);
                var ord = new TradeInfo(response);
                if (InvokeRequired)
                    Invoke(new SimpleDelegate(() => { dsMstTradeInfo.Add(ord); }));
                else dsMstTradeInfo.Add(ord);
            }
            else
            {
                //添加到成交单列表
                var ord = new TradeInfo(response);
                if (InvokeRequired)
                    Invoke(new SimpleDelegate(() => { dsSubTradeInfo.Add(ord); }));
                else dsSubTradeInfo.Add(ord);
            }
        }

        public void Run(WorkItemCallback callback)
        {
            _treadPool.QueueWorkItem(callback);
        }

        private void ibtnReg_Click(object sender, EventArgs e)
        {
            new FrmRegister().ShowDialog(this);
        }
    }
}
