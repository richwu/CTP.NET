using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using GalaxyFutures.Sfit.Api;
using log4net;
using Newtonsoft.Json;

namespace WinCtpTests
{
    public partial class Form1 : Form
    {
        private CtpMdApi _mdApi;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var log = LogManager.GetLogger("CTP");
            var fp = Path.Combine(Application.StartupPath, @"flow\md\");
            if (!Directory.Exists(fp))
                Directory.CreateDirectory(fp);
            _mdApi = new CtpMdApi(fp);

            _mdApi.OnFrontConnected += s =>
            {
                log.Debug("OnFrontConnected");
                var userLoginReq = new CtpReqUserLogin();
                userLoginReq.BrokerID = "9999";
                userLoginReq.UserID = "071249";
                userLoginReq.Password = "123456";
                userLoginReq.UserProductInfo = "CTP.NET";
                userLoginReq.ProtocolInfo = "CTP.NET";
                userLoginReq.InterfaceProductInfo = "CTP.NET";
                var rsp = _mdApi.ReqUserLogin(userLoginReq, 1);
                log.DebugFormat("ReqUserLogin[{0}]", rsp);
            };
            _mdApi.OnFrontDisconnected += (s, response) =>
            {
                log.DebugFormat("OnFrontDisconnected:{0}", response);
            };
            _mdApi.OnRspSubMarketData += (o, response, info, id, last) =>
            {
                log.DebugFormat("OnRspSubMarketData\nresponse:{0}\nrspInfo:{1}", 
                    JsonConvert.SerializeObject(response), 
                    JsonConvert.SerializeObject(info));
            };
            _mdApi.OnRtnDepthMarketData += (s, response) =>
            {
                log.DebugFormat("OnRtnDepthMarketData\nresponse:{0}", JsonConvert.SerializeObject(response));
            };
            _mdApi.OnRspUserLogin += (o, response, info, id, last) =>
            {
                var r = _mdApi.SubscribeMarketData(new[] { "CF711" });
                log.DebugFormat("SubscribeMarketData[{0}]", r);
            };
            _mdApi.OnRtnForQuoteRsp += (o, response) =>
            {
                log.DebugFormat("OnRtnForQuoteRsp\nresponse:{0}", JsonConvert.SerializeObject(response));
            };
            _mdApi.OnRspSubForQuoteRsp += (o, response, info, id, last) =>
            {
                log.DebugFormat("OnRspSubForQuoteRsp\nresponse:{0}\nrspInfo:{1}",
                   JsonConvert.SerializeObject(response),
                   JsonConvert.SerializeObject(info));
            };
            var worker = new BackgroundWorker();
            worker.DoWork += (o, args) =>
            {
                _mdApi.RegisterFront("tcp://180.168.146.187:10031");
                _mdApi.Init();
                _mdApi.Join();
            };
            worker.RunWorkerAsync();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _mdApi.Release();
        }
    }
}
