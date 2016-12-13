using System;
using System.Windows.Forms;

namespace CTPTractor
{
	public class MdApi
	{
		public delegate void TAMdMsg(object msg);

		private delegate void noneParamOrder();

		private delegate void connect(string p1);

		private delegate int params2String(string p1, string p2);

		private delegate int params3String(string p1, string p2, string p3);

		private delegate string getDay();

		private delegate int subMarketData(string[] _instruments, int nCount);

		private delegate int unSubMarketData(string[] ins, int cnt);

		private delegate int regEven(System.Delegate even);

		public delegate void RspError(ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);

		public delegate void HeartBeatWarning(int nTimeLapse);

		private delegate void FrontConnected();

		public delegate void FrontDisconnected(int nReason);

		public delegate void RspUserLogin(ref CThostFtdcRspUserLoginField pRspUserLogin, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);

		public delegate void RspUserLogout(ref CThostFtdcUserLogoutField pUserLogout, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);

		public delegate void RspSubMarketData(ref CThostFtdcSpecificInstrumentField pSpecificInstrument, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);

		public delegate void RspUnSubMarketData(ref CThostFtdcSpecificInstrumentField pSpecificInstrument, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);

		public delegate void RtnDepthMarketData(ref CThostFtdcDepthMarketDataField pDepthMarketData);

		public int apiType = 0;

		private string password;

		private dld funs = new dld();

		private MdApi.RspError rspError;

		private MdApi.HeartBeatWarning heartBeatWarning;

		private MdApi.FrontConnected frontConnected;

		private MdApi.FrontDisconnected frontDisconnected;

		private MdApi.RspUserLogin rspUserLogin;

		private MdApi.RspUserLogout rspUserLogout;

		private MdApi.RspSubMarketData rspSubMarketData;

		private MdApi.RspUnSubMarketData rspUnSubMarketData;

		private MdApi.RtnDepthMarketData rtnDepthMarketData;

		public event MdApi.TAMdMsg OnMdMsg;

		public event MdApi.RspError OnRspError
		{
			add
			{
				this.rspError = (MdApi.RspError)System.Delegate.Combine(this.rspError, value);
				(this.funs.Invoke("?RegOnRspError@@YGXP6GHPAUCThostFtdcRspInfoField@@H_N@Z@Z", typeof(MdApi.regEven)) as MdApi.regEven)(this.rspError);
			}
			remove
			{
				this.rspError = (MdApi.RspError)System.Delegate.Remove(this.rspError, value);
				(this.funs.Invoke("?RegOnRspError@@YGXP6GHPAUCThostFtdcRspInfoField@@H_N@Z@Z", typeof(MdApi.regEven)) as MdApi.regEven)(this.rspError);
			}
		}

		public event MdApi.HeartBeatWarning OnHeartBeatWarning
		{
			add
			{
				this.heartBeatWarning = (MdApi.HeartBeatWarning)System.Delegate.Combine(this.heartBeatWarning, value);
				(this.funs.Invoke("?RegOnHeartBeatWarning@@YGXP6GHH@Z@Z", typeof(MdApi.regEven)) as MdApi.regEven)(this.heartBeatWarning);
			}
			remove
			{
				this.heartBeatWarning = (MdApi.HeartBeatWarning)System.Delegate.Remove(this.heartBeatWarning, value);
				(this.funs.Invoke("?RegOnHeartBeatWarning@@YGXP6GHH@Z@Z", typeof(MdApi.regEven)) as MdApi.regEven)(this.heartBeatWarning);
			}
		}

		private event MdApi.FrontConnected OnFrontConnected
		{
			add
			{
				this.frontConnected = (MdApi.FrontConnected)System.Delegate.Combine(this.frontConnected, value);
				(this.funs.Invoke("?RegOnFrontConnected@@YGXP6GHXZ@Z", typeof(MdApi.regEven)) as MdApi.regEven)(this.frontConnected);
			}
			remove
			{
				this.frontConnected = (MdApi.FrontConnected)System.Delegate.Remove(this.frontConnected, value);
				(this.funs.Invoke("?RegOnFrontConnected@@YGXP6GHXZ@Z", typeof(MdApi.regEven)) as MdApi.regEven)(this.frontConnected);
			}
		}

		public event MdApi.FrontDisconnected OnFrontDisconnected
		{
			add
			{
				this.frontDisconnected = (MdApi.FrontDisconnected)System.Delegate.Combine(this.frontDisconnected, value);
				(this.funs.Invoke("?RegOnFrontDisconnected@@YGXP6GHH@Z@Z", typeof(MdApi.regEven)) as MdApi.regEven)(this.frontDisconnected);
			}
			remove
			{
				this.frontDisconnected = (MdApi.FrontDisconnected)System.Delegate.Remove(this.frontDisconnected, value);
				(this.funs.Invoke("?RegOnFrontDisconnected@@YGXP6GHH@Z@Z", typeof(MdApi.regEven)) as MdApi.regEven)(this.frontDisconnected);
			}
		}

		public event MdApi.RspUserLogin OnRspUserLogin
		{
			add
			{
				this.rspUserLogin = (MdApi.RspUserLogin)System.Delegate.Combine(this.rspUserLogin, value);
				(this.funs.Invoke("?RegOnRspUserLogin@@YGXP6GHPAUCThostFtdcRspUserLoginField@@PAUCThostFtdcRspInfoField@@H_N@Z@Z", typeof(MdApi.regEven)) as MdApi.regEven)(this.rspUserLogin);
			}
			remove
			{
				this.rspUserLogin = (MdApi.RspUserLogin)System.Delegate.Remove(this.rspUserLogin, value);
				(this.funs.Invoke("?RegOnRspUserLogin@@YGXP6GHPAUCThostFtdcRspUserLoginField@@PAUCThostFtdcRspInfoField@@H_N@Z@Z", typeof(MdApi.regEven)) as MdApi.regEven)(this.rspUserLogin);
			}
		}

		public event MdApi.RspUserLogout OnRspUserLogout
		{
			add
			{
				this.rspUserLogout = (MdApi.RspUserLogout)System.Delegate.Combine(this.rspUserLogout, value);
				(this.funs.Invoke("?RegOnRspUserLogout@@YGXP6GHPAUCThostFtdcUserLogoutField@@PAUCThostFtdcRspInfoField@@H_N@Z@Z", typeof(MdApi.regEven)) as MdApi.regEven)(this.rspUserLogout);
			}
			remove
			{
				this.rspUserLogout = (MdApi.RspUserLogout)System.Delegate.Remove(this.rspUserLogout, value);
				(this.funs.Invoke("?RegOnRspUserLogout@@YGXP6GHPAUCThostFtdcUserLogoutField@@PAUCThostFtdcRspInfoField@@H_N@Z@Z", typeof(MdApi.regEven)) as MdApi.regEven)(this.rspUserLogout);
			}
		}

		public event MdApi.RspSubMarketData OnRspSubMarketData
		{
			add
			{
				this.rspSubMarketData = (MdApi.RspSubMarketData)System.Delegate.Combine(this.rspSubMarketData, value);
				(this.funs.Invoke("?RegOnRspSubMarketData@@YGXP6GHPAUCThostFtdcSpecificInstrumentField@@PAUCThostFtdcRspInfoField@@H_N@Z@Z", typeof(MdApi.regEven)) as MdApi.regEven)(this.rspSubMarketData);
			}
			remove
			{
				this.rspSubMarketData = (MdApi.RspSubMarketData)System.Delegate.Remove(this.rspSubMarketData, value);
				(this.funs.Invoke("?RegOnRspSubMarketData@@YGXP6GHPAUCThostFtdcSpecificInstrumentField@@PAUCThostFtdcRspInfoField@@H_N@Z@Z", typeof(MdApi.regEven)) as MdApi.regEven)(this.rspSubMarketData);
			}
		}

		public event MdApi.RspUnSubMarketData OnRspUnSubMarketData
		{
			add
			{
				this.rspUnSubMarketData = (MdApi.RspUnSubMarketData)System.Delegate.Combine(this.rspUnSubMarketData, value);
				(this.funs.Invoke("?RegOnRspUnSubMarketData@@YGXP6GHPAUCThostFtdcSpecificInstrumentField@@PAUCThostFtdcRspInfoField@@H_N@Z@Z", typeof(MdApi.regEven)) as MdApi.regEven)(this.rspUnSubMarketData);
			}
			remove
			{
				this.rspUnSubMarketData = (MdApi.RspUnSubMarketData)System.Delegate.Remove(this.rspUnSubMarketData, value);
				(this.funs.Invoke("?RegOnRspUnSubMarketData@@YGXP6GHPAUCThostFtdcSpecificInstrumentField@@PAUCThostFtdcRspInfoField@@H_N@Z@Z", typeof(MdApi.regEven)) as MdApi.regEven)(this.rspUnSubMarketData);
			}
		}

		public event MdApi.RtnDepthMarketData OnRtnDepthMarketData
		{
			add
			{
				if (value == null)
				{
					this.rtnDepthMarketData = null;
				}
				else
				{
					this.rtnDepthMarketData = (MdApi.RtnDepthMarketData)System.Delegate.Combine(this.rtnDepthMarketData, value);
				}
				(this.funs.Invoke("?RegOnRtnDepthMarketData@@YGXP6GHPAUCThostFtdcDepthMarketDataField@@@Z@Z", typeof(MdApi.regEven)) as MdApi.regEven)(this.rtnDepthMarketData);
			}
			remove
			{
				this.rtnDepthMarketData = (MdApi.RtnDepthMarketData)System.Delegate.Remove(this.rtnDepthMarketData, value);
				(this.funs.Invoke("?RegOnRtnDepthMarketData@@YGXP6GHPAUCThostFtdcDepthMarketDataField@@@Z@Z", typeof(MdApi.regEven)) as MdApi.regEven)(this.rtnDepthMarketData);
			}
		}

		public string FrontAddr
		{
			get;
			set;
		}

		public string BrokerID
		{
			get;
			set;
		}

		public string InvestorID
		{
			get;
			set;
		}

		public MdApi(string _investor, string _pwd, string _broker, string _addr, int type, bool _saveHistory = false)
		{
			string str = System.Windows.Forms.Application.ExecutablePath.Substring(0, System.Windows.Forms.Application.ExecutablePath.LastIndexOf("\\"));
			this.funs.LoadDll(str + "\\MdApi.dll");
			this.FrontAddr = "tcp://180.166.12.241:41213";
			this.BrokerID = "4020";
			this.InvestorID = "";
			this.password = "";
			this.OnFrontConnected += new MdApi.FrontConnected(this.MdApi_OnFrontConnected);
			this.OnFrontDisconnected += new MdApi.FrontDisconnected(this.MdApi_OnFrontDisconnected);
			this.OnRspUserLogin += new MdApi.RspUserLogin(this.MdApi_OnRspUserLogin);
			this.OnRtnDepthMarketData += new MdApi.RtnDepthMarketData(this.MdApi_OnRtnDepthMarketData);
			this.OnRspUnSubMarketData += new MdApi.RspUnSubMarketData(this.MdApi_OnRspUnSubMarketData);
			this.OnRspError += new MdApi.RspError(this.MdApi_OnRspError);
		}

		private void MdApi_OnFrontConnected()
		{
			this.UserLogin();
		}

		private void MdApi_OnFrontDisconnected(int nReason)
		{
			this.OnMdMsg("行情断开@");
		}

		private void MdApi_OnRspUserLogin(ref CThostFtdcRspUserLoginField pRspUserLogin, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
		{
			this.OnMdMsg("行情登录成功@");
			if (bIsLast && pRspInfo.ErrorID == 0)
			{
				this.OnMdMsg("开始订阅行情@");
			}
			else
			{
				this.OnMdMsg(string.Concat(new object[]
				{
					"行情登录出错@",
					pRspInfo.ErrorID,
					":",
					pRspInfo.ErrorMsg
				}));
			}
		}

		private void MdApi_OnRspError(ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
		{
			if (!bIsLast || pRspInfo.ErrorID != 0)
			{
				this.OnMdMsg(string.Concat(new object[]
				{
					"订阅行情出错@",
					pRspInfo.ErrorID,
					":",
					pRspInfo.ErrorMsg
				}));
			}
		}

		private void MdApi_OnRspUnSubMarketData(ref CThostFtdcSpecificInstrumentField pSpecificInstrument, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
		{
			if (bIsLast)
			{
			}
		}

		private void MdApi_OnRtnDepthMarketData(ref CThostFtdcDepthMarketDataField pDepthMarketData)
		{
			this.OnMdMsg(pDepthMarketData);
		}

		public string GetTradingDay()
		{
			return (this.funs.Invoke("?GetTradingDay@@YGPBDXZ", typeof(MdApi.getDay)) as MdApi.getDay)();
		}

		public void Login()
		{
			(this.funs.Invoke("?Connect@@YGXPAD@Z", typeof(MdApi.connect)) as MdApi.connect)(this.FrontAddr);
		}

		public void DisConnect()
		{
			(this.funs.Invoke("?DisConnect@@YGXXZ", typeof(MdApi.noneParamOrder)) as MdApi.noneParamOrder)();
		}

		private void UserLogin()
		{
			(this.funs.Invoke("?ReqUserLogin@@YGHQAD00@Z", typeof(MdApi.params3String)) as MdApi.params3String)(this.BrokerID, this.InvestorID, this.password);
		}

		public void UserLogout()
		{
			(this.funs.Invoke("?ReqUserLogout@@YGHQAD0@Z", typeof(MdApi.params2String)) as MdApi.params2String)(this.BrokerID, this.InvestorID);
		}

		public void SubMarketData(params string[] instruments)
		{
			(this.funs.Invoke("?SubMarketData@@YGHQAPADH@Z", typeof(MdApi.subMarketData)) as MdApi.subMarketData)(instruments, instruments.Length);
		}

		public void UnSubMarketData(params string[] instruments)
		{
			(this.funs.Invoke("?UnSubscribeMarketData@@YGHQAPADH@Z", typeof(MdApi.unSubMarketData)) as MdApi.unSubMarketData)(instruments, (instruments == null) ? 0 : instruments.Length);
		}
	}
}
