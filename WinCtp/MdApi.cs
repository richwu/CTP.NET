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
    /// 行情接口。
    /// </summary>
    public class MdApi : IDisposable
    {
        private readonly CtpMdApi _api;
        private readonly BackgroundWorker _worker;
        private readonly ILog _log;

        private readonly string _frontAddress;
        private readonly string _brokerId;
        private readonly string _userId;
        private readonly string _pwd;

        public bool IsConnected { get; private set; }

        public bool IsLogin { get; private set; }

        private readonly IMainView _view;

        public MdApi(string brokerId, string userId, string pwd, string frontAddress, IMainView view)
        {
            _brokerId = brokerId;
            _userId = userId;
            _pwd = pwd;
            _frontAddress = frontAddress;
            _view = view;

            IsConnected = false;
            IsLogin = false;

            var fp = Path.Combine(Application.StartupPath, $@"flow\md\{userId}\");
            if (!Directory.Exists(fp))
                Directory.CreateDirectory(fp);
            _api = new CtpMdApi(fp);
            InitApi();

            _worker = new BackgroundWorker
            {
                WorkerSupportsCancellation = true,
                WorkerReportsProgress = true
            };
            _worker.DoWork += OnDoWork;
            _log = LogManager.GetLogger($"CTP.MD.{userId}");
        }

        private void InitApi()
        {
            _api.OnFrontConnected += sender =>
            {
                IsConnected = true;
                _log.Debug("OnFrontConnected");
                Login();
            };
            _api.OnFrontDisconnected += (sender, response) =>
            {
                IsConnected = false;
                _log.DebugFormat("OnFrontDisconnected\nresponse:{0}", response);
            };
            _api.OnRspError += OnRspError;
            _api.OnRspUserLogin += OnRspUserLogin;
            _api.OnRspUserLogout += OnRspUserLogout;
            _api.OnRtnDepthMarketData += OnRtnDepthMarketData;
            _api.OnRspSubMarketData += OnRspSubMarketData;
        }

        private void OnRspSubMarketData(object sender, CtpSpecificInstrument response, CtpRspInfo rspInfo, int requestId, bool isLast)
        {
            _log.DebugFormat("OnRspSubMarketData\nrequestId=&{0}ErrorID={1}&ErrorMsg={2}&isLast={3}&InstrumentID={4}",
                requestId, rspInfo.ErrorID, rspInfo.ErrorMsg, isLast, response.InstrumentID);
        }

        private void OnRtnDepthMarketData(object sender, CtpDepthMarketData response)
        {
            if (response != null)
                DataCache.DepthMarketData[response.InstrumentID] = new DepthMarketData(response);
            _log.DebugFormat("OnRtnDepthMarketData\n{0}", JsonConvert.SerializeObject(response));
        }

        private void OnRspUserLogout(object sender, CtpUserLogout response, CtpRspInfo rspInfo, int requestId, bool isLast)
        {
            if (rspInfo == null || rspInfo.ErrorID == 0)
            {
                IsLogin = false;
                _view?.NotifyMdStatus(0);
            }
        }

        private void OnRspUserLogin(object sender, CtpRspUserLogin response, CtpRspInfo rspInfo, int requestId, bool isLast)
        {
            if (rspInfo == null || rspInfo.ErrorID == 0)
            {
                IsLogin = true;
                _view?.NotifyMdStatus(1);
            }
        }

        private void OnRspError(object sender, CtpRspInfo rspInfo, int requestId, bool isLast)
        {
            _view?.NotifyMdStatus(-9, rspInfo.ErrorMsg);
            _log.DebugFormat("OnRspError\nrequestId=&{0}ErrorID={1}&ErrorMsg={2}&isLast={3}",
                requestId, rspInfo.ErrorID, rspInfo.ErrorMsg, isLast);
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

        private void Login()
        {
            var userLoginReq = new CtpReqUserLogin();
            userLoginReq.BrokerID = _brokerId;
            userLoginReq.UserID = _userId;
            userLoginReq.Password = _pwd;
            userLoginReq.UserProductInfo = "CTP.NET";
            userLoginReq.ProtocolInfo = "CTP.NET";
            userLoginReq.InterfaceProductInfo = "CTP.NET";
            var rsp = _api.ReqUserLogin(userLoginReq, 1);
            _log.DebugFormat("ReqUserLogin:{0}\nBrokerID={1}&UserID={2}", Rsp.This[rsp], _brokerId, _userId);
        }

        public bool SubscribeMarketData(string instrumentId)
        {
            var rsp = _api.SubscribeMarketData(new[] { instrumentId });
            _log.DebugFormat("SubscribeMarketData:{0}\ninstrumentID={1}", Rsp.This[rsp], instrumentId);
            return rsp == 0;
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