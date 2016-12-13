using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Trade2015
{
	public class Proxy
	{
		private delegate int DefCreateApi(string pPath);

		private delegate int DefApi();

		private delegate int DefReqConnect(string pFront);

		private delegate int DefReqUserLogin(string pInvestor, string pPwd, string pBroker, string IP);

		private delegate void DefReqUserLogout();

		private delegate string DefGetTradingDay();

		private delegate int DefReqOrderInsert(string pInstrument, DirectionType pDirection, EOffsetType pOffset, double pPrice, int pVolume, string pRef, HedgeType pHedge, OrderType pType);

		private delegate int DefReqOrderAction(string InstrumentID, int _FrontID, int _SessionID, string _OrderRef);

		private delegate int DefReqQryInstrumentCommissionRate(string pInstrument);

		private delegate int DefReqQryInstrumentMarginRate(string pInstrument);

		private delegate int DefReqQryInstrument(string pInstrument);

		private delegate void Reg(IntPtr pPtr);

		public delegate void FrontConnected();

		public delegate void RspUserLogin(UserLoginField pUserLogin, InfoField pInfo);

		public delegate void RspUserLogout(int pReason);

		public delegate void RtnError(int pErrId, string pMsg);

		public delegate void RtnNotice(string pMsg);

		public delegate void RtnExchangeStatus(string pExchange, ExchangeStatusType pStatus);

		public delegate void RspQryInstrument(InstrumentField pInstrument, InfoField pInfo, bool pLast);

		public delegate void RspQryOrder(OrderField pField, InfoField pInfo, bool pLast);

		public delegate void RspQryTrade(TradeField pField, bool pLast);

		public delegate void RspQryPosition(PositionField pPosition, bool pLast);

		public delegate void RspQrySettlementInfo(SettlementInfoField pSettlement, bool pLast);

		public delegate void RspQryPositionDetail(PositionDetailField pPositionDetail, bool pLast);

		public delegate void RspQryTradingAccount(TradingAccount pAccount);

		public delegate void RtnOrder(OrderField pOrder);

		public delegate void RtnTrade(TradeField pTrade);

		public delegate void RspQryInvestor(InvestorField pTrade, bool pLast);

		public delegate void RspQryInstrumentCommissionRate(InstrumentCommissionRateField pTrade, bool pLast);

		public delegate void RspQryInstrumentMarginRate(InstrumentMarginRateField pTrade, bool pLast);

		public delegate void RspOrderInsert(ErrOrderField pTrade, InfoField pLast);

		public delegate void ErrRtnOrderInsert(ErrOrderField pTrade, InfoField pLast);

		public IntPtr _handle;

		private Proxy.FrontConnected _OnFrontConnected;

		private Proxy.RspUserLogin _OnRspUserLogin;

		private Proxy.RspUserLogout _OnRspUserLogout;

		private Proxy.RtnError _OnRtnError;

		private Proxy.RtnNotice _OnRtnNotice;

		private Proxy.RtnExchangeStatus _OnRtnExchangeStatus;

		private Proxy.RspQryInstrument _OnRspQryInstrument;

		private Proxy.RspQryOrder _OnRspQryOrder;

		private Proxy.RspQryTrade _OnRspQryTrade;

		private Proxy.RspQryPosition _OnRspQryPositiont;

		private Proxy.RspQrySettlementInfo _OnRspQrySettlementInfo;

		private Proxy.RspQryPositionDetail _OnRspQryPositiontDetail;

		private Proxy.RspQryTradingAccount _OnRspQryTradingAccount;

		private Proxy.RtnOrder _OnRtnOrder;

		private Proxy.RtnOrder _OnRtnCancel;

		private Proxy.RtnTrade _OnRtnTrade;

		private Proxy.RspQryInvestor _OnRspQryInvestor;

		private Proxy.RspQryInstrumentCommissionRate _OnRspQryInstrumentCommissionRate;

		private Proxy.RspQryInstrumentMarginRate _OnRspQryInstrumentMarginRate;

		private Proxy.RspOrderInsert _OnRspOrderInsert;

		private Proxy.ErrRtnOrderInsert _OnErrRtnOrderInsert;

		private string _file;

		public event Proxy.FrontConnected OnFrontConnected
		{
			add
			{
				this._OnFrontConnected = (Proxy.FrontConnected)Delegate.Combine(this._OnFrontConnected, value);
				Proxy.Reg reg = Proxy.Invoke(this._handle, "?RegOnFrontConnected@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg;
				reg(Marshal.GetFunctionPointerForDelegate(this._OnFrontConnected));
			}
			remove
			{
				this._OnFrontConnected = (Proxy.FrontConnected)Delegate.Remove(this._OnFrontConnected, value);
				(Proxy.Invoke(this._handle, "?RegOnFrontConnected@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnFrontConnected));
			}
		}

		public event Proxy.RspUserLogin OnRspUserLogin
		{
			add
			{
				this._OnRspUserLogin = (Proxy.RspUserLogin)Delegate.Combine(this._OnRspUserLogin, value);
				(Proxy.Invoke(this._handle, "?RegOnRspUserLogin@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnRspUserLogin));
			}
			remove
			{
				this._OnRspUserLogin = (Proxy.RspUserLogin)Delegate.Remove(this._OnRspUserLogin, value);
				(Proxy.Invoke(this._handle, "?RegOnRspUserLogin@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnRspUserLogin));
			}
		}

		public event Proxy.RspUserLogout OnRspUserLogout
		{
			add
			{
				this._OnRspUserLogout = (Proxy.RspUserLogout)Delegate.Combine(this._OnRspUserLogout, value);
				(Proxy.Invoke(this._handle, "?RegOnRspUserLogout@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnRspUserLogout));
			}
			remove
			{
				this._OnRspUserLogout = (Proxy.RspUserLogout)Delegate.Remove(this._OnRspUserLogout, value);
				(Proxy.Invoke(this._handle, "?RegOnRspUserLogout@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnRspUserLogout));
			}
		}

		public event Proxy.RtnError OnRtnError
		{
			add
			{
				this._OnRtnError = (Proxy.RtnError)Delegate.Combine(this._OnRtnError, value);
				(Proxy.Invoke(this._handle, "?RegOnRtnError@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnRtnError));
			}
			remove
			{
				this._OnRtnError = (Proxy.RtnError)Delegate.Remove(this._OnRtnError, value);
				(Proxy.Invoke(this._handle, "?RegOnRtnError@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnRtnError));
			}
		}

		public event Proxy.RtnNotice OnRtnNotice
		{
			add
			{
				this._OnRtnNotice = (Proxy.RtnNotice)Delegate.Combine(this._OnRtnNotice, value);
				(Proxy.Invoke(this._handle, "?RegOnRtnNotice@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnRtnNotice));
			}
			remove
			{
				this._OnRtnNotice = (Proxy.RtnNotice)Delegate.Remove(this._OnRtnNotice, value);
				(Proxy.Invoke(this._handle, "?RegOnRtnNotice@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnRtnNotice));
			}
		}

		public event Proxy.RtnExchangeStatus OnRtnExchangeStatus
		{
			add
			{
				this._OnRtnExchangeStatus = (Proxy.RtnExchangeStatus)Delegate.Combine(this._OnRtnExchangeStatus, value);
				(Proxy.Invoke(this._handle, "?RegOnRtnExchangeStatus@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnRtnExchangeStatus));
			}
			remove
			{
				this._OnRtnExchangeStatus = (Proxy.RtnExchangeStatus)Delegate.Remove(this._OnRtnExchangeStatus, value);
				(Proxy.Invoke(this._handle, "?RegOnRtnExchangeStatus@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnRtnExchangeStatus));
			}
		}

		public event Proxy.RspQryInstrument OnRspQryInstrument
		{
			add
			{
				this._OnRspQryInstrument = (Proxy.RspQryInstrument)Delegate.Combine(this._OnRspQryInstrument, value);
				(Proxy.Invoke(this._handle, "?RegOnRspQryInstrument@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnRspQryInstrument));
			}
			remove
			{
				this._OnRspQryInstrument = (Proxy.RspQryInstrument)Delegate.Remove(this._OnRspQryInstrument, value);
				(Proxy.Invoke(this._handle, "?RegOnRspQryInstrument@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnRspQryInstrument));
			}
		}

		public event Proxy.RspQryOrder OnRspQryOrder
		{
			add
			{
				this._OnRspQryOrder = (Proxy.RspQryOrder)Delegate.Combine(this._OnRspQryOrder, value);
				(Proxy.Invoke(this._handle, "?RegOnRspQryOrder@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnRspQryOrder));
			}
			remove
			{
				this._OnRspQryOrder = (Proxy.RspQryOrder)Delegate.Remove(this._OnRspQryOrder, value);
				(Proxy.Invoke(this._handle, "?RegOnRspQryOrder@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnRspQryOrder));
			}
		}

		public event Proxy.RspQryTrade OnRspQryTrade
		{
			add
			{
				this._OnRspQryTrade = (Proxy.RspQryTrade)Delegate.Combine(this._OnRspQryTrade, value);
				(Proxy.Invoke(this._handle, "?RegOnRspQryTrade@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnRspQryTrade));
			}
			remove
			{
				this._OnRspQryTrade = (Proxy.RspQryTrade)Delegate.Remove(this._OnRspQryTrade, value);
				(Proxy.Invoke(this._handle, "?RegOnRspQryTrade@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnRspQryTrade));
			}
		}

		public event Proxy.RspQryPosition OnRspQryPositiont
		{
			add
			{
				this._OnRspQryPositiont = (Proxy.RspQryPosition)Delegate.Combine(this._OnRspQryPositiont, value);
				(Proxy.Invoke(this._handle, "?RegOnRspQryPosition@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnRspQryPositiont));
			}
			remove
			{
				this._OnRspQryPositiont = (Proxy.RspQryPosition)Delegate.Remove(this._OnRspQryPositiont, value);
				(Proxy.Invoke(this._handle, "?RegOnRspQryPosition@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnRspQryPositiont));
			}
		}

		public event Proxy.RspQrySettlementInfo OnRspQrySettlementInfo
		{
			add
			{
				this._OnRspQrySettlementInfo = (Proxy.RspQrySettlementInfo)Delegate.Combine(this._OnRspQrySettlementInfo, value);
				(Proxy.Invoke(this._handle, "?RegOnRspQrySettlementInfo@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnRspQrySettlementInfo));
			}
			remove
			{
				this._OnRspQrySettlementInfo = (Proxy.RspQrySettlementInfo)Delegate.Remove(this._OnRspQrySettlementInfo, value);
				(Proxy.Invoke(this._handle, "?RegOnRspQrySettlementInfo@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnRspQrySettlementInfo));
			}
		}

		public event Proxy.RspQryPositionDetail OnRspQryPositiontDetail
		{
			add
			{
				this._OnRspQryPositiontDetail = (Proxy.RspQryPositionDetail)Delegate.Combine(this._OnRspQryPositiontDetail, value);
				(Proxy.Invoke(this._handle, "?RegOnRspQryPositionDetail@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnRspQryPositiontDetail));
			}
			remove
			{
				this._OnRspQryPositiontDetail = (Proxy.RspQryPositionDetail)Delegate.Remove(this._OnRspQryPositiontDetail, value);
				(Proxy.Invoke(this._handle, "?RegOnRspQryPositionDetail@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnRspQryPositiontDetail));
			}
		}

		public event Proxy.RspQryTradingAccount OnRspQryTradingAccount
		{
			add
			{
				this._OnRspQryTradingAccount = (Proxy.RspQryTradingAccount)Delegate.Combine(this._OnRspQryTradingAccount, value);
				(Proxy.Invoke(this._handle, "?RegOnRspQryTradingAccount@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnRspQryTradingAccount));
			}
			remove
			{
				this._OnRspQryTradingAccount = (Proxy.RspQryTradingAccount)Delegate.Remove(this._OnRspQryTradingAccount, value);
				(Proxy.Invoke(this._handle, "?RegOnRspQryTradingAccount@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnRspQryTradingAccount));
			}
		}

		public event Proxy.RtnOrder OnRtnOrder
		{
			add
			{
				this._OnRtnOrder = (Proxy.RtnOrder)Delegate.Combine(this._OnRtnOrder, value);
				(Proxy.Invoke(this._handle, "?RegOnRtnOrder@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnRtnOrder));
			}
			remove
			{
				this._OnRtnOrder = (Proxy.RtnOrder)Delegate.Remove(this._OnRtnOrder, value);
				(Proxy.Invoke(this._handle, "?RegOnRtnOrder@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnRtnOrder));
			}
		}

		public event Proxy.RtnOrder OnRtnCancel
		{
			add
			{
				this._OnRtnCancel = (Proxy.RtnOrder)Delegate.Combine(this._OnRtnCancel, value);
				(Proxy.Invoke(this._handle, "?RegOnRtnCancel@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnRtnCancel));
			}
			remove
			{
				this._OnRtnCancel = (Proxy.RtnOrder)Delegate.Remove(this._OnRtnCancel, value);
				(Proxy.Invoke(this._handle, "?RegOnRtnCancel@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnRtnCancel));
			}
		}

		public event Proxy.RtnTrade OnRtnTrade
		{
			add
			{
				this._OnRtnTrade = (Proxy.RtnTrade)Delegate.Combine(this._OnRtnTrade, value);
				(Proxy.Invoke(this._handle, "?RegOnRtnTrade@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnRtnTrade));
			}
			remove
			{
				this._OnRtnTrade = (Proxy.RtnTrade)Delegate.Remove(this._OnRtnTrade, value);
				(Proxy.Invoke(this._handle, "?RegOnRtnTrade@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnRtnTrade));
			}
		}

		public event Proxy.RspQryInvestor OnRspQryInvestor
		{
			add
			{
				this._OnRspQryInvestor = (Proxy.RspQryInvestor)Delegate.Combine(this._OnRspQryInvestor, value);
				(Proxy.Invoke(this._handle, "?RegOnRspQryInvestor@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnRspQryInvestor));
			}
			remove
			{
				this._OnRspQryInvestor = (Proxy.RspQryInvestor)Delegate.Remove(this._OnRspQryInvestor, value);
				(Proxy.Invoke(this._handle, "?RegOnRspQryInvestor@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnRspQryInvestor));
			}
		}

		public event Proxy.RspQryInstrumentCommissionRate OnRspQryInstrumentCommissionRate
		{
			add
			{
				this._OnRspQryInstrumentCommissionRate = (Proxy.RspQryInstrumentCommissionRate)Delegate.Combine(this._OnRspQryInstrumentCommissionRate, value);
				(Proxy.Invoke(this._handle, "?RegOnRspQryInstrumentCommissionRate@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnRspQryInstrumentCommissionRate));
			}
			remove
			{
				this._OnRspQryInstrumentCommissionRate = (Proxy.RspQryInstrumentCommissionRate)Delegate.Remove(this._OnRspQryInstrumentCommissionRate, value);
				(Proxy.Invoke(this._handle, "?RegOnRspQryInstrumentCommissionRate@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnRspQryInstrumentCommissionRate));
			}
		}

		public event Proxy.RspQryInstrumentMarginRate OnRspQryInstrumentMarginRate
		{
			add
			{
				this._OnRspQryInstrumentMarginRate = (Proxy.RspQryInstrumentMarginRate)Delegate.Combine(this._OnRspQryInstrumentMarginRate, value);
				(Proxy.Invoke(this._handle, "?RegOnRspQryInstrumentMarginRate@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnRspQryInstrumentMarginRate));
			}
			remove
			{
				this._OnRspQryInstrumentMarginRate = (Proxy.RspQryInstrumentMarginRate)Delegate.Remove(this._OnRspQryInstrumentMarginRate, value);
				(Proxy.Invoke(this._handle, "?RegOnRspQryInstrumentMarginRate@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnRspQryInstrumentMarginRate));
			}
		}

		public event Proxy.RspOrderInsert OnRspOrderInsert
		{
			add
			{
				this._OnRspOrderInsert = (Proxy.RspOrderInsert)Delegate.Combine(this._OnRspOrderInsert, value);
				(Proxy.Invoke(this._handle, "?RegOnRspOrderInsert@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnRspOrderInsert));
			}
			remove
			{
				this._OnRspOrderInsert = (Proxy.RspOrderInsert)Delegate.Remove(this._OnRspOrderInsert, value);
				(Proxy.Invoke(this._handle, "?RegOnRspOrderInsert@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnRspOrderInsert));
			}
		}

		public event Proxy.ErrRtnOrderInsert OnErrRtnOrderInsert
		{
			add
			{
				this._OnErrRtnOrderInsert = (Proxy.ErrRtnOrderInsert)Delegate.Combine(this._OnErrRtnOrderInsert, value);
				(Proxy.Invoke(this._handle, "?RegOnErrRtnOrderInsert@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnErrRtnOrderInsert));
			}
			remove
			{
				this._OnErrRtnOrderInsert = (Proxy.ErrRtnOrderInsert)Delegate.Remove(this._OnErrRtnOrderInsert, value);
				(Proxy.Invoke(this._handle, "?RegOnErrRtnOrderInsert@@YGXPAX@Z", typeof(Proxy.Reg)) as Proxy.Reg)(Marshal.GetFunctionPointerForDelegate(this._OnErrRtnOrderInsert));
			}
		}

		public Proxy(string account, string pFile)
		{
			this.LoadDll(account, pFile);
		}

		~Proxy()
		{
			Proxy.FreeLibrary(this._handle);
			if (File.Exists(this._file))
			{
				File.Delete(this._file);
			}
		}

		[DllImport("kernel32.dll")]
		private static extern IntPtr LoadLibrary(string lpFileName);

		[DllImport("kernel32.dll")]
		private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

		[DllImport("kernel32", SetLastError = true)]
		protected static extern bool FreeLibrary(IntPtr hModule);

		private static Delegate Invoke(IntPtr pHModule, string lpProcName, Type t)
		{
			if (pHModule == IntPtr.Zero)
			{
				throw new Exception(" 函数库模块的句柄为空 , 请确保已进行 LoadDll 操作 !");
			}
			IntPtr procAddress = Proxy.GetProcAddress(pHModule, lpProcName);
			if (procAddress == IntPtr.Zero)
			{
				throw new Exception(" 没有找到 :" + lpProcName + " 这个函数的入口点 ");
			}
			return Marshal.GetDelegateForFunctionPointer(procAddress, t);
		}

		protected void LoadDll(string account, string pFile)
		{
			if (File.Exists(pFile))
			{
				this._file = account + ".dll";
				File.Copy(pFile, this._file, true);
				this._handle = Proxy.LoadLibrary(this._file);
			}
			if (this._handle == IntPtr.Zero)
			{
				throw new Exception(string.Format(" 没有找到 :{0}.", Environment.CurrentDirectory + "\\" + pFile));
			}
		}

		public int ReqConnect(string sPath, string pFront)
		{
			((Proxy.DefCreateApi)Proxy.Invoke(this._handle, "?CreateApi@@YGXPAD@Z", typeof(Proxy.DefCreateApi)))(sPath);
			return ((Proxy.DefReqConnect)Proxy.Invoke(this._handle, "?ReqConnect@@YGHPAD@Z", typeof(Proxy.DefReqConnect)))(pFront);
		}

		public int ReqUserLogin(string pInvestor, string pPwd, string pBroker, string IP)
		{
			return ((Proxy.DefReqUserLogin)Proxy.Invoke(this._handle, "?ReqUserLogin@@YGHPAD000@Z", typeof(Proxy.DefReqUserLogin)))(pInvestor, pPwd, pBroker, IP);
		}

		public void ReqUserLogout()
		{
			((Proxy.DefReqUserLogout)Proxy.Invoke(this._handle, "?ReqUserLogout@@YGXXZ", typeof(Proxy.DefReqUserLogout)))();
		}

		public string GetTradingDay()
		{
			return ((Proxy.DefGetTradingDay)Proxy.Invoke(this._handle, "?GetTradingDay@@YGPBDXZ", typeof(Proxy.DefGetTradingDay)))();
		}

		public int ReqQryOrder()
		{
			return ((Proxy.DefApi)Proxy.Invoke(this._handle, "?ReqQryOrder@@YGHXZ", typeof(Proxy.DefApi)))();
		}

		public int ReqQryTrade()
		{
			return ((Proxy.DefApi)Proxy.Invoke(this._handle, "?ReqQryTrade@@YGHXZ", typeof(Proxy.DefApi)))();
		}

		public int ReqQryPosition()
		{
			return ((Proxy.DefApi)Proxy.Invoke(this._handle, "?ReqQryPosition@@YGHXZ", typeof(Proxy.DefApi)))();
		}

		public int ReqQryPositionDetail()
		{
			return ((Proxy.DefApi)Proxy.Invoke(this._handle, "?ReqQryPositionDetail@@YGHXZ", typeof(Proxy.DefApi)))();
		}

		public int ReqQryAccount()
		{
			return ((Proxy.DefApi)Proxy.Invoke(this._handle, "?ReqQryAccount@@YGHXZ", typeof(Proxy.DefApi)))();
		}

		public int ReqOrderInsert(string pInstrument, DirectionType pDirection, EOffsetType pOffset, double pPrice, int pVolume, string pRef, HedgeType pHedge, OrderType pType)
		{
			return ((Proxy.DefReqOrderInsert)Proxy.Invoke(this._handle, "?ReqOrderInsert@@YGHPADW4DirectionType@@W4EOffsetType@@NH0W4HedgeType@@W4OrderType@@@Z", typeof(Proxy.DefReqOrderInsert)))(pInstrument, pDirection, pOffset, pPrice, pVolume, pRef, pHedge, pType);
		}

		public int ReqOrderAction(string InstrumentID, int _FrontID, int _SessionID, string _OrderRef)
		{
			return ((Proxy.DefReqOrderAction)Proxy.Invoke(this._handle, "?ReqOrderAction@@YGHPADHH0@Z", typeof(Proxy.DefReqOrderAction)))(InstrumentID, _FrontID, _SessionID, _OrderRef);
		}

		public int ReqSettlementInfoConfirm()
		{
			return ((Proxy.DefApi)Proxy.Invoke(this._handle, "?ReqSettlementInfoConfirm@@YGHXZ", typeof(Proxy.DefApi)))();
		}

		public int ReqQryInvestor()
		{
			return ((Proxy.DefApi)Proxy.Invoke(this._handle, "?ReqQryInvestor@@YGHXZ", typeof(Proxy.DefApi)))();
		}

		public int ReqSettlementInfo(string date)
		{
			return ((Proxy.DefReqQryInstrument)Proxy.Invoke(this._handle, "?ReqQrySettlementInfo@@YGHPAD@Z", typeof(Proxy.DefReqQryInstrument)))(date);
		}

		public int ReqQryInstrumentCommissionRate(string pInstrument)
		{
			return ((Proxy.DefReqQryInstrumentCommissionRate)Proxy.Invoke(this._handle, "?ReqQryInstrumentCommissionRate@@YGHPAD@Z", typeof(Proxy.DefReqQryInstrumentCommissionRate)))(pInstrument);
		}

		public int ReqQryInstrumentMarginRate(string pInstrument)
		{
			return ((Proxy.DefReqQryInstrumentMarginRate)Proxy.Invoke(this._handle, "?ReqQryInstrumentMarginRate@@YGHPAD@Z", typeof(Proxy.DefReqQryInstrumentMarginRate)))(pInstrument);
		}

		public int ReqQryInstrument(string pInstrument)
		{
			return ((Proxy.DefReqQryInstrument)Proxy.Invoke(this._handle, "?ReqQryInstrument@@YGHPAD@Z", typeof(Proxy.DefReqQryInstrument)))(pInstrument);
		}
	}
}
