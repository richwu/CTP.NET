using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using GalaxyFutures.Sfit.Api;
using log4net;
using Newtonsoft.Json;

namespace WinCtp
{
    public partial class FrmMain : Form
    {
        private readonly ILog _log;
        private bool _listening;
        private readonly  ConcurrentQueue<CtpTrade> _tradeQueue;
        private readonly ConcurrentQueue<CtpInputOrder> _inputOrderQueue;

        public FrmMain()
        {
            InitializeComponent();
            _log = LogManager.GetLogger("CTP");
            _tradeQueue = new ConcurrentQueue<CtpTrade>();
            _inputOrderQueue = new ConcurrentQueue<CtpInputOrder>();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            tsslTradeApiStatus.Text = string.Empty;
            tsslBroker.Text = string.Empty;
            _listening = false;

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
                api.OnFrontConnected += TradeApiOnFrontConnected;
                api.OnFrontDisconnected += TradeApiOnFrontDisconnected;
                api.OnRspUserLogin += TradeApiOnRspUserLogin;
                api.OnRspUserLogout += TradeApiOnRspUserLogout;
                api.OnRspError += TradeApiOnRspError;
                api.OnRtnOrder += TradeApiOnRtnOrder;
                api.OnRtnTrade += TradeApiOnRtnTrade;
                api.OnRspQryTrade += TradeApiOnRspQryTrade;
                api.OnRspQrySettlementInfo += TradeApiOnRspQrySettlementInfo;
                api.OnRspOrderInsert += TradeApiOnRspOrderInsert;
                api.OnRspSettlementInfoConfirm += TradeApiOnRspSettlementInfoConfirm;
                api.SubscribePrivateTopic(CtpResumeType.Quick);
                api.SubscribePublicTopic(CtpResumeType.Quick);
                b.Start();
            }
        }

        private void TradeApiOnRspSettlementInfoConfirm(object sender, CtpSettlementInfoConfirm response, CtpRspInfo rspInfo, int requestId, bool isLast)
        {
            _log.InfoFormat("TradeApiOnRspSettlementInfoConfirm\nresponse:{0}\nrspInfo:{1}", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(rspInfo));
        }

        private void TradeApiOnRspQrySettlementInfo(object sender, CtpSettlementInfo response, CtpRspInfo rspInfo, int requestId, bool isLast)
        {
            _log.InfoFormat("TradeApiOnRspQrySettlementInfo\nresponse:{0}\nrspInfo:{1}", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(rspInfo));
        }

        private void TradeApiOnRspError(object sender, CtpRspInfo rspInfo, int requestId, bool isLast)
        {
            _log.InfoFormat("TradeApiOnRspError\nrspInfo:{0}", JsonConvert.SerializeObject(rspInfo));
        }

        private void TradeApiOnRspUserLogout(object sender, CtpUserLogout response, CtpRspInfo rspInfo, int requestId, bool isLast)
        {
            _log.InfoFormat("TradeApiOnRspUserLogout requestId[{0}],response.UserID[{1}]", requestId, response.UserID);
            if (rspInfo.ErrorID != 0)
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

        private void TradeApiOnRspUserLogin(object sender, CtpRspUserLogin response, CtpRspInfo rspInfo, int requestId, bool isLast)
        {
            _log.InfoFormat("TradeApiOnRspUserLogin\nresponse:{0}\nrspInfo:{1}", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(rspInfo));
            if (rspInfo.ErrorID != 0)
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

        private void TradeApiOnFrontDisconnected(object sender, int response)
        {
            _log.InfoFormat("TradeApiOnFrontDisconnected [{0}]", response);
        }
         
        private void TradeApiOnFrontConnected(object sender)
        {
            _log.Info("TradeApiOnFrontConnected");
        }

        #region 主账户
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
                _log.InfoFormat("ReqUserLogin rsp[{0}]", rsp);
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
                _log.InfoFormat("ReqUserLogout rsp[{0}]", rsp);
            }
        }
        #endregion

        #region 子账户
        
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
                _log.InfoFormat("ReqUserLogin rsp[{0}]", rsp);
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
                _log.InfoFormat("ReqUserLogout rsp[{0}]", rsp);
            }
        }
        #endregion

        #region 
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

        /// <summary>
        /// 触发查询成交单。
        /// </summary>
        private void timerQryTrade_Tick(object sender, EventArgs e)
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
        private void TradeApiOnRspQryTrade(object sender, CtpTrade response, CtpRspInfo rspInfo, int requestId, bool isLast)
        {
            _log.InfoFormat("TradeApiOnOnRspQryTrade\nresponse:{0}", JsonConvert.SerializeObject(response));
            if (rspInfo.ErrorID == 0 && response != null)
                _tradeQueue.Enqueue(response);
        }

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

        /// <summary>
        /// 触发跟单（报单）。
        /// </summary>
        private void timerInsertOrder_Tick(object sender, EventArgs e)
        {
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

        private void TradeApiOnRtnTrade(object sender, CtpTrade response)
        {
            _log.InfoFormat("TradeApiOnRtnTrade\nresponse:{0}", JsonConvert.SerializeObject(response));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="response"></param>
        private void TradeApiOnRtnOrder(object sender, CtpOrder response)
        {
            _log.InfoFormat("TradeApiOnRtnOrder\nresponse:{0}", JsonConvert.SerializeObject(response));
        }

        /// <summary>
        /// 报单回报。
        /// </summary>
        private void TradeApiOnRspOrderInsert(object sender, CtpInputOrder response, CtpRspInfo rspInfo, int requestId, bool isLast)
        {
            _log.InfoFormat("TradeApiOnRspOrderInsert\nresponse:{0}\nrspInfo:{1}", JsonConvert.SerializeObject(response), JsonConvert.SerializeObject(rspInfo));
            if(rspInfo.ErrorID == 0 && response != null)
                _inputOrderQueue.Enqueue(response);
        }

        /// <summary>
        /// 报单响应同步。
        /// </summary>
        private void timerReturnOrder_Tick(object sender, EventArgs e)
        {

        }
    }
}
