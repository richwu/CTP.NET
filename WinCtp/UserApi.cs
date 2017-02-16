using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using GalaxyFutures.Sfit.Api;

namespace WinCtp
{
    public class UserApi : IDisposable
    {
        public static IDictionary<string, UserApi> This => _holder ?? (_holder = new Dictionary<string, UserApi>());
        private static IDictionary<string, UserApi> _holder;

        public CtpTraderApi TraderApi { get; private set; }

        private readonly string _userId;
        private readonly string _traderFrontAddress;
        private BackgroundWorker _worker;

        public UserApi(string userId, string traderFrontAddress)
        {
            _userId = userId;
            _traderFrontAddress = traderFrontAddress;
            InitApi();
        }

        private void InitApi()
        {
            var fp = Path.Combine(Application.StartupPath, $@"flow\{_userId}\");
            if (!Directory.Exists(fp))
                Directory.CreateDirectory(fp);
            TraderApi = new CtpTraderApi(fp);
            _worker = new BackgroundWorker
            {
                WorkerSupportsCancellation = true,
                WorkerReportsProgress = true
            };
            _worker.DoWork += OnDoWork;
        }

        private void OnDoWork(object sender, DoWorkEventArgs args)
        {
            TraderApi.RegisterFront(_traderFrontAddress);
            TraderApi.Init();
            TraderApi.Join();
        }

        public void Start()
        {
            _worker.RunWorkerAsync();
        }

        public void Dispose()
        {
            if(_worker != null && _worker.IsBusy)
                _worker.CancelAsync();
            TraderApi?.Release();
        }
    }
}