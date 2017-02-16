using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using GalaxyFutures.Sfit.Api;
using log4net;
using Newtonsoft.Json;

namespace WinCtp
{
    /// <summary>
    /// 交易接口。
    /// </summary>
    public class TraderApi : IDisposable
    {
        private readonly CtpTraderApi _api;
        private readonly BackgroundWorker _worker;
        private readonly ILog _log;
        private readonly IMainView _view;

        private readonly string _frontAddress;
        private readonly string _brokerId;
        private readonly string _userId;
        private readonly string _pwd;

        public bool IsConnected { get; private set; }

        public bool IsLogin { get; private set; }

        public int MaxOrderRef { get; set; }

        public int FrontId { get; set; }

        public int SessionId { get; set; }

        public TraderApi(string brokerId, string userId, string pwd, string frontAddress, IMainView view)
        {
            _brokerId = brokerId;
            _userId = userId;
            _pwd = pwd;
            _frontAddress = frontAddress;
            _view = view;

            IsConnected = false;
            IsLogin = false;

            var fp = Path.Combine(Application.StartupPath, $@"flow\{userId}\");
            if (!Directory.Exists(fp))
                Directory.CreateDirectory(fp);
            _api = new CtpTraderApi(fp);
            InitApi();

            _worker = new BackgroundWorker
            {
                WorkerSupportsCancellation = true,
                WorkerReportsProgress = true
            };
            _worker.DoWork += OnDoWork;
            _log = LogManager.GetLogger($"CTP.{userId}");
        }

        public string GetOrderRef()
        {
            var r = MaxOrderRef;
            MaxOrderRef++;
            return r.ToString("d12");
        }

        private void InitApi()
        {
            _api.OnFrontConnected += sender =>
            {
                IsConnected = true;
                _log.Debug("OnFrontConnected");
            };
            _api.OnFrontDisconnected += (sender, response) =>
            {
                IsConnected = false;
                _log.DebugFormat("OnFrontDisconnected\nresponse:{0}", response);
            };
            _api.OnRspError += OnRspError;
            _api.OnRspUserLogin += OnRspUserLogin;
            _api.OnRspUserLogout += OnRspUserLogout;

            _api.OnRtnTrade+= OnRtnTrade;
        }

        private void OnRtnTrade(object sender, CtpTrade response)
        {
            _log.DebugFormat("OnRtnTrade\n{0}",JsonConvert.SerializeObject(response));
            if (response == null)
                return;
            _view.NotifyTrade(response);
        }

        private void OnDoWork(object sender, DoWorkEventArgs args)
        {
            _api.RegisterFront(_frontAddress);
            _api.Init();
            _api.Join();
        }

        public void Start()
        {
            _worker.RunWorkerAsync();
        }

        public bool Login()
        {
            if (!IsConnected)
                return false;
            var userLoginReq = new CtpReqUserLogin();
            userLoginReq.BrokerID = _brokerId;
            userLoginReq.UserID = _userId;
            userLoginReq.Password = _pwd;
            userLoginReq.UserProductInfo = "CTP.NET";
            userLoginReq.ProtocolInfo = "CTP.NET";
            userLoginReq.InterfaceProductInfo = "CTP.NET";
            var rsp = _api.ReqUserLogin(userLoginReq, 1);
            _log.DebugFormat("ReqUserLogin:{0}\nBrokerID={1}&UserID={2}", Rsp.This[rsp], _brokerId, _userId);
            return rsp == 0;
        }

        public bool Logout()
        {
            if (!IsLogin)
                return false;
            var req = new CtpUserLogout
            {
                BrokerID = _brokerId,
                UserID = _userId
            };
            var rsp = _api.ReqUserLogout(req, 1);
            _log.DebugFormat("ReqUserLogout:{0}", Rsp.This[rsp]);
            return rsp == 0;
        }

        private void OnRspUserLogout(object sender, CtpUserLogout response, CtpRspInfo rspInfo, int requestId, bool isLast)
        {
            if (rspInfo == null || rspInfo.ErrorID == 0)
            {
                IsLogin = false;
            }
        }

        private void OnRspUserLogin(object sender, CtpRspUserLogin response, CtpRspInfo rspInfo, int requestId, bool isLast)
        {
            if (rspInfo == null || rspInfo.ErrorID == 0)
            {
                IsLogin = true;
                FrontId = response.FrontID;
                SessionId = response.SessionID;
                MaxOrderRef = Convert.ToInt32(response.MaxOrderRef);
            }
        }

        private void OnRspError(object sender, CtpRspInfo rspInfo, int requestId, bool isLast)
        {
            _log.DebugFormat("OnRspError\nrequestId=&{0}ErrorID={1}&ErrorMsg={2}&isLast={3}",
                requestId, rspInfo.ErrorID, rspInfo.ErrorMsg, isLast);
        }

        public void Dispose()
        {
            if (_worker != null)
            {
                if (_worker.IsBusy)
                    _worker.CancelAsync();
                _worker.Dispose();
            }
            if (_api != null)
            {
                _api.Release();
                _api.Dispose();
            }
        }
    }
}