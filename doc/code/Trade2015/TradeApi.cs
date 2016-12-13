using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Trade2015
{
	public class TradeApi
	{
		public delegate void TAAnswer(TradeApi.ObjectAndKey key);

		public delegate void TAMsg(string[] key);

		public class ObjectAndKey
		{
			public object Object
			{
				get;
				set;
			}

			public string Key
			{
				get;
				set;
			}

			public ObjectAndKey(object obj, string key)
			{
				this.Object = obj;
				this.Key = key;
			}
		}

		public class Order
		{
			public string Params = null;

			public object Field = null;

			public TradeApi QryTradeApi
			{
				get;
				set;
			}

			public TradeApi.EnumQryOrder QryOrderType
			{
				get;
				set;
			}

			public Order(TradeApi.EnumQryOrder _qryType, TradeApi _tradeapi, string _params = null, object _field = null)
			{
				this.QryOrderType = _qryType;
				this.QryTradeApi = _tradeapi;
				this.Params = _params;
				this.Field = _field;
			}
		}

		public enum EnumQryOrder
		{
			QryOrder,
			QryTrade,
			QryIntorverPosition,
			QryInvestorPositionDetail,
			QryInstrumentCommissionRate,
			QryInstrumentMarginRate,
			QryTradingAccount,
			QryParkedOrderAction,
			QryParkedOrder,
			QryContractBank,
			QueryBankAccountMoneyByFuture,
			QrySettlementInfo,
			SettlementInfoConfirm,
			QryHistoryTrade,
			SettlementInfo,
			QryTransferSerial,
			QryInvestor
		}

		public delegate void FrontConnected(object sender, EventArgs e);

		public delegate void RspUserLogin(object sender, IntEventArgs e);

		public delegate void RspUserLogout(object sender, IntEventArgs e);

		public delegate void RtnError(object sender, ErrorEventArgs e);

		public delegate void RtnNotice(object sender, StringEventArgs e);

		public delegate void RtnExchangeStatus(object sender, StatusEventArgs e);

		public delegate void RtnOrder(object sender, OrderArgs e);

		public delegate void RtnTrade(object sender, TradeArgs e);

		public bool Isconned = false;

		public int apiType = 0;

		private bool needQryRate = true;

		private string RatePath = "";

		private Dictionary<string, List<string>> DlistVariety = new Dictionary<string, List<string>>();

		public string passWord;

		public string LoginState;

		private string clientName;

		public int ordernum = 0;

		public DateTime lastordertime;

		public bool apiIsBusy = false;

		public List<TradeApi.Order> listQry = new List<TradeApi.Order>();

		public List<string[]> listTransDatas = new List<string[]>();

		private int controlordernum = 1000;

		private string instrument4QryRate = null;

		private ListView listInvestorPosition = new ListView();

		public DataTable dtInstruments = new DataTable("Instruments");

		public DataTable dtPosition = new DataTable();

		public DataTable dtPositionDetail = new DataTable();

		private string clientip = "";

		public bool IsSimulationData = false;

		public bool IsLogin = false;

		public bool begin = false;

		public bool SubListenbegin = false;

		public bool SubProofbegin = false;

		public string Path = "";

		public int LoginCount = 0;

		public string Group = "";

		public bool IsQrying = false;

		private object lockOrderRef = new object();

		private static int newref = 0;

		private Proxy _proxy;

		public TimeSpan tsDiffDCE = default(TimeSpan);

		public TimeSpan tsDiffSHFE = default(TimeSpan);

		public TimeSpan tsDiffCZCE = default(TimeSpan);

		public TimeSpan tsDiffCFFEX = default(TimeSpan);

		public ConcurrentDictionary<string, ExchangeStatusType> DicExcStatus = new ConcurrentDictionary<string, ExchangeStatusType>();

		protected ConcurrentDictionary<string, TimeSpan> DicExcLoginTime = new ConcurrentDictionary<string, TimeSpan>();

		public ConcurrentDictionary<string, InstrumentField> DicInstrumentField = new ConcurrentDictionary<string, InstrumentField>();

		public ConcurrentDictionary<int, OrderField> DicOrderField = new ConcurrentDictionary<int, OrderField>();

		public ConcurrentDictionary<string, TradeField> DicTradeField = new ConcurrentDictionary<string, TradeField>();

		public ConcurrentDictionary<string, PositionField> DicPositionField = new ConcurrentDictionary<string, PositionField>();

		public TradingAccount TradingAccount = new TradingAccount();

		private TradeApi.FrontConnected _OnFrontConnected;

		private TradeApi.RspUserLogin _OnRspUserLogin;

		private TradeApi.RspUserLogout _OnRspUserLogout;

		private TradeApi.RtnError _OnRtnError;

		private TradeApi.RtnNotice _OnRtnNotice;

		private TradeApi.RtnExchangeStatus _OnRtnExchangeStatus;

		private TradeApi.RtnOrder _OnRtnOrder;

		private TradeApi.RtnOrder _OnRtnCancel;

		private TradeApi.RtnTrade _OnRtnTrade;

		private string settlement = "";

		public event TradeApi.TAAnswer OnAnswer;

		public event TradeApi.TAMsg OnMsg;

		public event TradeApi.FrontConnected OnFrontConnected
		{
			add
			{
				this._OnFrontConnected = (TradeApi.FrontConnected)Delegate.Combine(this._OnFrontConnected, value);
			}
			remove
			{
				this._OnFrontConnected = (TradeApi.FrontConnected)Delegate.Remove(this._OnFrontConnected, value);
			}
		}

		public event TradeApi.RspUserLogin OnRspUserLogin
		{
			add
			{
				this._OnRspUserLogin = (TradeApi.RspUserLogin)Delegate.Combine(this._OnRspUserLogin, value);
			}
			remove
			{
				this._OnRspUserLogin = (TradeApi.RspUserLogin)Delegate.Remove(this._OnRspUserLogin, value);
			}
		}

		public event TradeApi.RspUserLogout OnRspUserLogout
		{
			add
			{
				this._OnRspUserLogout = (TradeApi.RspUserLogout)Delegate.Combine(this._OnRspUserLogout, value);
			}
			remove
			{
				this._OnRspUserLogout = (TradeApi.RspUserLogout)Delegate.Remove(this._OnRspUserLogout, value);
			}
		}

		public event TradeApi.RtnError OnRtnError
		{
			add
			{
				this._OnRtnError = (TradeApi.RtnError)Delegate.Combine(this._OnRtnError, value);
			}
			remove
			{
				this._OnRtnError = (TradeApi.RtnError)Delegate.Remove(this._OnRtnError, value);
			}
		}

		public event TradeApi.RtnNotice OnRtnNotice
		{
			add
			{
				this._OnRtnNotice = (TradeApi.RtnNotice)Delegate.Combine(this._OnRtnNotice, value);
			}
			remove
			{
				this._OnRtnNotice = (TradeApi.RtnNotice)Delegate.Remove(this._OnRtnNotice, value);
			}
		}

		public event TradeApi.RtnExchangeStatus OnRtnExchangeStatus
		{
			add
			{
				this._OnRtnExchangeStatus = (TradeApi.RtnExchangeStatus)Delegate.Combine(this._OnRtnExchangeStatus, value);
			}
			remove
			{
				this._OnRtnExchangeStatus = (TradeApi.RtnExchangeStatus)Delegate.Remove(this._OnRtnExchangeStatus, value);
			}
		}

		public event TradeApi.RtnOrder OnRtnOrder
		{
			add
			{
				this._OnRtnOrder = (TradeApi.RtnOrder)Delegate.Combine(this._OnRtnOrder, value);
			}
			remove
			{
				this._OnRtnOrder = (TradeApi.RtnOrder)Delegate.Remove(this._OnRtnOrder, value);
			}
		}

		public event TradeApi.RtnOrder OnRtnCancel
		{
			add
			{
				this._OnRtnCancel = (TradeApi.RtnOrder)Delegate.Combine(this._OnRtnCancel, value);
			}
			remove
			{
				this._OnRtnCancel = (TradeApi.RtnOrder)Delegate.Remove(this._OnRtnCancel, value);
			}
		}

		public event TradeApi.RtnTrade OnRtnTrade
		{
			add
			{
				this._OnRtnTrade = (TradeApi.RtnTrade)Delegate.Combine(this._OnRtnTrade, value);
			}
			remove
			{
				this._OnRtnTrade = (TradeApi.RtnTrade)Delegate.Remove(this._OnRtnTrade, value);
			}
		}

		public string TradingDay
		{
			get;
			protected set;
		}

		public string FrontAddr
		{
			get;
			set;
		}

		public string MFrontAddr
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

		public int FrontID
		{
			get;
			set;
		}

		public int SessionID
		{
			get;
			set;
		}

		public int MaxOrderRef
		{
			get;
			set;
		}

		public void execQryList()
		{
			while (this.begin)
			{
				if (!this.apiIsBusy && this.listQry.Count != 0)
				{
					TradeApi.Order order = this.listQry[0];
					Thread.Sleep(1000);
					this.apiIsBusy = true;
					switch (order.QryOrderType)
					{
					case TradeApi.EnumQryOrder.QryOrder:
						order.QryTradeApi.ReqQryOrder();
						break;
					case TradeApi.EnumQryOrder.QryTrade:
						order.QryTradeApi.ReqQryTrade();
						break;
					case TradeApi.EnumQryOrder.QryIntorverPosition:
						order.QryTradeApi.ReqQryPosition();
						break;
					case TradeApi.EnumQryOrder.QryInvestorPositionDetail:
						order.QryTradeApi.ReqQryPositionDetail();
						break;
					case TradeApi.EnumQryOrder.QryInstrumentCommissionRate:
						this.instrument4QryRate = order.Params;
						order.QryTradeApi.ReqQryInstrumentCommissionRate(order.Params);
						break;
					case TradeApi.EnumQryOrder.QryInstrumentMarginRate:
						order.QryTradeApi.ReqQryInstrumentMarginRate(order.Params);
						break;
					case TradeApi.EnumQryOrder.QryTradingAccount:
						if (this.IsLogin)
						{
							order.QryTradeApi.ReqQryAccount();
						}
						break;
					case TradeApi.EnumQryOrder.QryParkedOrderAction:
					case TradeApi.EnumQryOrder.QryParkedOrder:
					case TradeApi.EnumQryOrder.QryContractBank:
					case TradeApi.EnumQryOrder.QueryBankAccountMoneyByFuture:
					case TradeApi.EnumQryOrder.QrySettlementInfo:
					case TradeApi.EnumQryOrder.SettlementInfoConfirm:
					case TradeApi.EnumQryOrder.QryTransferSerial:
						goto IL_15C;
					case TradeApi.EnumQryOrder.QryHistoryTrade:
						break;
					case TradeApi.EnumQryOrder.SettlementInfo:
						order.QryTradeApi.ReqSettlementInfo(order.Params);
						break;
					case TradeApi.EnumQryOrder.QryInvestor:
						order.QryTradeApi.ReqQryInvestor();
						break;
					default:
						goto IL_15C;
					}
					IL_165:
					this.listQry.Remove(order);
					continue;
					IL_15C:
					this.apiIsBusy = false;
					goto IL_165;
				}
				Thread.Sleep(100);
			}
		}

		public int UpOrderRef()
		{
			Monitor.Enter(this.lockOrderRef);
			if (this.MaxOrderRef > TradeApi.newref)
			{
				TradeApi.newref = this.MaxOrderRef;
			}
			TradeApi.newref++;
			Monitor.Exit(this.lockOrderRef);
			return TradeApi.newref;
		}

		public void execTread()
		{
			while (this.begin)
			{
				if (this.listTransDatas.Count == 0)
				{
					Thread.Sleep(1);
				}
				else
				{
					try
					{
						string[] array = this.listTransDatas[0];
						if (this.ordernum == 0)
						{
							this.lastordertime = DateTime.Now;
						}
						int num = this.ReqOrderInsert(array[0], (array[1] == "Buy") ? DirectionType.Buy : DirectionType.Sell, (array[2] == "open") ? EOffsetType.Open : ((array[2] == "close") ? EOffsetType.Close : ((array[2] == "closetoday") ? EOffsetType.CloseToday : EOffsetType.CloseYesterday)), double.Parse(array[3]), Convert.ToInt32(array[4]), array[5], HedgeType.Speculation, OrderType.Limit);
						if (num == 0)
						{
							OrderField orderField = new OrderField();
							orderField.OrderSysID = "";
							orderField.VolumeTraded = 0;
							orderField.InsertTime = DateTime.Now.ToString("HH:mm:ss");
							orderField.InsertDate = DateTime.Now.ToString("yyyyMMdd");
							orderField.UpdateTime = "";
							orderField.StatusMsg = "";
							orderField.UserProductInfo = "";
							orderField.InvestorID = this.InvestorID;
							orderField.InstrumentID = array[0];
							orderField.Direction = ((array[1] == "Buy") ? DirectionType.Buy : DirectionType.Sell);
							orderField.Offset = ((array[2] == "open") ? EOffsetType.Open : ((array[2] == "close") ? EOffsetType.Close : ((array[2] == "closetoday") ? EOffsetType.CloseToday : EOffsetType.CloseYesterday)));
							orderField.Status = OrderStatus.Ordered;
							orderField.LimitPrice = double.Parse(array[3]);
							orderField.VolumeTotalOriginal = Convert.ToInt32(array[4]);
							orderField.VolumeTotal = Convert.ToInt32(array[4]);
							orderField.OrderRef = array[5];
							orderField.FrontID = this.FrontID;
							orderField.SessionID = this.SessionID;
							this.OnAnswer(new TradeApi.ObjectAndKey(orderField, string.Concat(new object[]
							{
								orderField.FrontID,
								",",
								orderField.SessionID,
								",",
								orderField.OrderRef
							})));
						}
						this.ordernum++;
						this.listTransDatas.RemoveAt(0);
						Thread.Sleep(1);
					}
					catch
					{
					}
				}
			}
		}

		public TradeApi(string dllname, string _investor, string _pwd, string _broker, string _addr, string _mdiaddr, string _group, bool subListen, bool subproof, string type)
		{
			this.apiType = ((type == "金牛") ? 1 : ((type == "CTP") ? 0 : 2));
			this.MaxOrderRef = 0;
			this.FrontAddr = _addr;
			this.MFrontAddr = _mdiaddr;
			this.BrokerID = _broker;
			this.InvestorID = _investor;
			this.passWord = _pwd;
			this.lastordertime = DateTime.Now;
			this.IsSimulationData = true;
			this._proxy = new Proxy(_investor, dllname);
			this._proxy.OnFrontConnected += new Proxy.FrontConnected(this._import_OnFrontConnected);
			this._proxy.OnRspUserLogin += new Proxy.RspUserLogin(this._import_OnRspUserLogin);
			this._proxy.OnRspQrySettlementInfo += new Proxy.RspQrySettlementInfo(this._import_OnRspQrySettlementInfo);
			this._proxy.OnRspUserLogout += new Proxy.RspUserLogout(this._import_OnRspUserLogout);
			this._proxy.OnRspQryInstrument += new Proxy.RspQryInstrument(this._import_OnRspQryInstrument);
			this._proxy.OnRspQryOrder += new Proxy.RspQryOrder(this._import_OnRspQryOrder);
			this._proxy.OnRspQryPositiont += new Proxy.RspQryPosition(this._import_OnRspQryPositiont);
			this._proxy.OnRspQryTrade += new Proxy.RspQryTrade(this._import_OnRspQryTrade);
			this._proxy.OnRspQryTradingAccount += new Proxy.RspQryTradingAccount(this._import_OnRspQryTradingAccount);
			this._proxy.OnRtnCancel += new Proxy.RtnOrder(this._import_OnRtnCancel);
			this._proxy.OnRtnError += new Proxy.RtnError(this._import_OnRtnError);
			this._proxy.OnRtnExchangeStatus += new Proxy.RtnExchangeStatus(this._import_OnRtnExchangeStatus);
			this._proxy.OnRtnNotice += new Proxy.RtnNotice(this._import_OnRtnNotice);
			this._proxy.OnRtnOrder += new Proxy.RtnOrder(this._import_OnRtnOrder);
			this._proxy.OnRtnTrade += new Proxy.RtnTrade(this._import_OnRtnTrade);
			this._proxy.OnRspQryInvestor += new Proxy.RspQryInvestor(this._import_OnRspQryInvestor);
			this._proxy.OnRspQryPositiontDetail += new Proxy.RspQryPositionDetail(this._import_OnRspQryPositiontDetail);
			this._proxy.OnRspQryInstrumentCommissionRate += new Proxy.RspQryInstrumentCommissionRate(this._import_OnRspQryInstrumentCommissionRate);
			this._proxy.OnRspQryInstrumentMarginRate += new Proxy.RspQryInstrumentMarginRate(this._import_OnRspQryInstrumentMarginRate);
			this._proxy.OnErrRtnOrderInsert += new Proxy.ErrRtnOrderInsert(this._import_OnErrRtnOrderInsert);
			this._proxy.OnRspOrderInsert += new Proxy.RspOrderInsert(this._import_OnRspOrderInsert);
			string text = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf("\\"));
			DirectoryInfo directoryInfo = new DirectoryInfo(text);
			DirectoryInfo directoryInfo2 = new DirectoryInfo(string.Concat(new string[]
			{
				text,
				"\\DLL\\",
				_investor,
				"_",
				_broker
			}));
			this.Path = directoryInfo2.FullName + "\\";
			if (!directoryInfo2.Exists)
			{
				directoryInfo2.Create();
				directoryInfo2.Attributes = FileAttributes.Hidden;
			}
			Directory.SetCurrentDirectory(directoryInfo2.FullName);
			DirectoryInfo directoryInfo3 = new DirectoryInfo("tdi");
			if (!directoryInfo3.Exists)
			{
				directoryInfo3.Create();
			}
			Directory.SetCurrentDirectory(directoryInfo3.FullName);
			if (File.Exists(directoryInfo3.FullName + "\\" + DateTime.Today.ToString("yyyy-MM-dd") + ".txt"))
			{
				this.RatePath = directoryInfo3.FullName + "\\" + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
				this.needQryRate = false;
			}
			else
			{
				File.Create(directoryInfo3.FullName + "\\" + DateTime.Today.ToString("yyyy-MM-dd") + ".txt").Close();
				this.RatePath = directoryInfo3.FullName + "\\" + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
			}
			Directory.SetCurrentDirectory(text);
			this.dtInstruments.Columns.Add("品种代码", string.Empty.GetType());
			this.dtInstruments.Columns.Add("合约", string.Empty.GetType());
			this.dtInstruments.Columns.Add("名称", string.Empty.GetType());
			this.dtInstruments.Columns.Add("交易所", string.Empty.GetType());
			this.dtInstruments.Columns.Add("合约数量", -2147483648.GetType());
			this.dtInstruments.Columns.Add("最小波动", double.NaN.GetType());
			this.dtInstruments.Columns.Add("保证金-多", double.NaN.GetType());
			this.dtInstruments.Columns.Add("保证金-空", double.NaN.GetType());
			this.dtInstruments.Columns.Add("开仓手续费", Type.GetType("System.Double"));
			this.dtInstruments.Columns.Add("平仓手续费", Type.GetType("System.Double"));
			this.dtInstruments.Columns.Add("平今手续费", Type.GetType("System.Double"));
			this.dtInstruments.Columns.Add("开仓手续费率", Type.GetType("System.Double"));
			this.dtInstruments.Columns.Add("平仓手续费率", Type.GetType("System.Double"));
			this.dtInstruments.Columns.Add("平今手续费率", Type.GetType("System.Double"));
			this.dtInstruments.Columns.Add("最大下单量-限", -2147483648.GetType());
			this.dtInstruments.Columns.Add("最小下单量-限", -2147483648.GetType());
			this.dtInstruments.Columns.Add("自选", true.GetType());
			this.dtInstruments.Columns["自选"].DefaultValue = false;
			this.dtInstruments.Columns.Add("套利", true.GetType());
			this.dtInstruments.Columns["套利"].DefaultValue = false;
			this.dtInstruments.Columns.Add("跌停价", double.NaN.GetType());
			this.dtInstruments.Columns.Add("涨停价", double.NaN.GetType());
			this.dtInstruments.Columns.Add("买价", double.NaN.GetType());
			this.dtInstruments.Columns.Add("卖价", double.NaN.GetType());
			this.dtInstruments.Columns.Add("最新价", double.NaN.GetType());
			this.dtInstruments.Columns.Add("今开", double.NaN.GetType());
			this.dtInstruments.Columns.Add("昨收", double.NaN.GetType());
			this.dtInstruments.Columns.Add("今收", double.NaN.GetType());
			this.dtInstruments.PrimaryKey = new DataColumn[]
			{
				this.dtInstruments.Columns["合约"]
			};
			this.dtPosition.Columns.Add("投资者帐户", string.Empty.GetType());
			this.dtPosition.Columns.Add("合约", string.Empty.GetType());
			this.dtPosition.Columns.Add("买卖", string.Empty.GetType());
			this.dtPosition.Columns.Add("总持仓", Type.GetType("System.Int32"));
			this.dtPosition.Columns.Add("昨仓", Type.GetType("System.Int32"));
			this.dtPosition.Columns.Add("今仓", Type.GetType("System.Int32"));
			this.dtPosition.Columns.Add("可平昨", Type.GetType("System.Int32"));
			this.dtPosition.Columns.Add("可平今", Type.GetType("System.Int32"));
			this.dtPosition.Columns.Add("持仓均价", double.NaN.GetType());
			this.dtPosition.Columns.Add("开仓均价", double.NaN.GetType());
			this.dtPosition.Columns.Add("持仓盈亏", double.NaN.GetType());
			this.dtPosition.Columns.Add("空头占用保证金", double.NaN.GetType());
			this.dtPosition.Columns.Add("多头占用保证金", double.NaN.GetType());
			this.dtPosition.Columns.Add("投保", string.Empty.GetType());
			this.dtPositionDetail.Columns.Add("投资者帐户", string.Empty.GetType());
			this.dtPositionDetail.Columns.Add("编号", string.Empty.GetType());
			this.dtPositionDetail.Columns.Add("合约", string.Empty.GetType());
			this.dtPositionDetail.Columns.Add("买卖", string.Empty.GetType());
			this.dtPositionDetail.Columns.Add("持仓类型", string.Empty.GetType());
			this.dtPositionDetail.Columns.Add("成交手数", Type.GetType("System.Int32"));
			this.dtPositionDetail.Columns.Add("成交价格", double.NaN.GetType());
			this.dtPositionDetail.Columns.Add("成交时间", Type.GetType("System.Int32"));
			this.clientip = TradeApi.GetFirstIP().ToString();
			this.Group = _group;
			this.begin = true;
			new Thread(new ThreadStart(this.execQryList))
			{
				IsBackground = true,
				Name = "qry" + _investor
			}.Start();
			if (_group == "子帐户")
			{
				this.SubListenbegin = subListen;
				this.SubProofbegin = subproof;
				new Thread(new ThreadStart(this.execTread))
				{
					IsBackground = true,
					Name = "tre" + _investor
				}.Start();
			}
		}

		public static IPAddress GetFirstIP()
		{
			string hostName = Dns.GetHostName();
			IPHostEntry hostEntry = Dns.GetHostEntry(hostName);
			IPAddress[] addressList = hostEntry.AddressList;
			IPAddress result;
			for (int i = 0; i < addressList.Length; i++)
			{
				IPAddress iPAddress = addressList[i];
				if (iPAddress.AddressFamily != AddressFamily.InterNetworkV6)
				{
					result = iPAddress;
					return result;
				}
			}
			result = ((hostEntry.AddressList != null && hostEntry.AddressList.Length > 0) ? hostEntry.AddressList[0] : new IPAddress(0L));
			return result;
		}

		private void _import_OnRtnNotice(string pMsg)
		{
			if (this._OnRtnNotice != null)
			{
				this._OnRtnNotice(this, new StringEventArgs
				{
					Value = pMsg
				});
			}
		}

		private void _import_OnRtnError(int pErrId, string pMsg)
		{
			if (this._OnRtnError != null)
			{
				this._OnRtnError(this, new ErrorEventArgs
				{
					ErrorID = pErrId,
					ErrorMsg = pMsg
				});
			}
		}

		private void _import_OnRtnExchangeStatus(string pExchange, ExchangeStatusType pStatus)
		{
			this.DicExcStatus.AddOrUpdate(pExchange, pStatus, (string k, ExchangeStatusType v) => pStatus);
			if (this._OnRtnExchangeStatus != null)
			{
				this._OnRtnExchangeStatus(this, new StatusEventArgs
				{
					Exchange = pExchange,
					Status = pStatus
				});
			}
		}

		private void _import_OnRtnOrder(OrderField pOrder)
		{
			this.OnAnswer(new TradeApi.ObjectAndKey(pOrder, string.Concat(new object[]
			{
				pOrder.FrontID,
				",",
				pOrder.SessionID,
				",",
				pOrder.OrderRef
			})));
			DataRow dataRow = this.dtInstruments.Rows.Find(pOrder.InstrumentID);
			if (dataRow != null && dataRow["保证金-多"].ToString() == "0" && dataRow["保证金-空"].ToString() == "0")
			{
				dataRow["保证金-多"] = double.NaN;
				dataRow["保证金-空"] = double.NaN;
				this.listQry.Add(new TradeApi.Order(TradeApi.EnumQryOrder.QryInstrumentMarginRate, this, pOrder.InstrumentID, null));
			}
		}

		private void _import_OnRtnTrade(TradeField pTrade)
		{
			this.OnAnswer(new TradeApi.ObjectAndKey(pTrade, "1"));
			this.OnAnswer(new TradeApi.ObjectAndKey(pTrade, "持仓"));
		}

		private void _import_OnRtnCancel(OrderField pOrder)
		{
			OrderField orAdd = this.DicOrderField.GetOrAdd(pOrder.OrderID, new OrderField());
			FieldInfo[] fields = pOrder.GetType().GetFields();
			for (int i = 0; i < fields.Length; i++)
			{
				FieldInfo fieldInfo = fields[i];
				orAdd.GetType().GetField(fieldInfo.Name).SetValue(orAdd, Convert.ChangeType(fieldInfo.GetValue(pOrder), orAdd.GetType().GetField(fieldInfo.Name).FieldType));
			}
			orAdd.Status = OrderStatus.Canceled;
			if (this._OnRtnCancel != null)
			{
				this._OnRtnCancel(this, new OrderArgs
				{
					Value = orAdd
				});
			}
		}

		private void _import_OnRspQryTrade(TradeField pTrade, bool pLast)
		{
			if (pTrade.InstrumentID != "")
			{
				this.OnAnswer(new TradeApi.ObjectAndKey(pTrade, "0"));
			}
			if (pLast)
			{
				this.apiIsBusy = false;
			}
		}

		private void _import_OnRspQryOrder(OrderField pOrder, InfoField pRspInfo, bool pLast)
		{
			if (pRspInfo.ErrorID == 0)
			{
				if (pOrder.InstrumentID != "")
				{
					this.OnAnswer(new TradeApi.ObjectAndKey(pOrder, string.Concat(new object[]
					{
						pOrder.FrontID,
						",",
						pOrder.SessionID,
						",",
						pOrder.OrderRef
					})));
				}
			}
			if (pLast)
			{
				this.apiIsBusy = false;
			}
		}

		private void _import_OnRspQryTradingAccount(TradingAccount pAccount)
		{
			this.OnAnswer(new TradeApi.ObjectAndKey(pAccount, ""));
			this.apiIsBusy = false;
		}

		private void _import_OnRspQryPositiont(PositionField pInvestorPosition, bool pLast)
		{
			if (pInvestorPosition.InstrumentID != "")
			{
				if (!pInvestorPosition.InstrumentID.Contains("&"))
				{
					DataRow[] array = this.dtPosition.Select(string.Concat(new string[]
					{
						"合约='",
						pInvestorPosition.InstrumentID,
						"'and 买卖='",
						(pInvestorPosition.Direction == DirectionType.Buy) ? "买" : "    卖",
						"'"
					}));
					int num = (int)this.dtInstruments.Rows.Find(pInvestorPosition.InstrumentID)["合约数量"];
					DataRow dataRow;
					if (array.Length == 0)
					{
						dataRow = this.dtPosition.NewRow();
						dataRow["投资者帐户"] = pInvestorPosition.InvestorID;
						dataRow["合约"] = pInvestorPosition.InstrumentID;
						dataRow["买卖"] = ((pInvestorPosition.Direction == DirectionType.Buy) ? "买" : "    卖");
						dataRow["昨仓"] = 0;
						dataRow["今仓"] = 0;
						dataRow["总持仓"] = 0;
						dataRow["可平昨"] = 0;
						dataRow["可平今"] = 0;
						if (pInvestorPosition.Position > 0)
						{
							if (pInvestorPosition.YdPosition > 0)
							{
								dataRow["昨仓"] = (pInvestorPosition.Position - pInvestorPosition.TdPosition).ToString();
								dataRow["可平昨"] = (pInvestorPosition.Position - pInvestorPosition.TdPosition).ToString();
							}
							if (pInvestorPosition.TdPosition > 0)
							{
								dataRow["今仓"] = pInvestorPosition.TdPosition.ToString();
								dataRow["可平今"] = pInvestorPosition.TdPosition.ToString();
							}
						}
						dataRow["总持仓"] = Convert.ToInt32(dataRow["昨仓"].ToString()) + Convert.ToInt32(dataRow["今仓"].ToString());
						dataRow["持仓均价"] = ((num == 0) ? "0" : (pInvestorPosition.PositionCost / (double)num).ToString("F3"));
						dataRow["开仓均价"] = ((num == 0) ? "0" : (pInvestorPosition.OpenCost / (double)num).ToString("F3"));
						dataRow["持仓盈亏"] = pInvestorPosition.PositionProfit.ToString("F3");
						dataRow["空头占用保证金"] = ((pInvestorPosition.Direction == DirectionType.Buy) ? "0" : pInvestorPosition.UseMargin.ToString("F2"));
						dataRow["多头占用保证金"] = ((pInvestorPosition.Direction == DirectionType.Buy) ? pInvestorPosition.UseMargin.ToString("F2") : "0");
						dataRow["投保"] = ((pInvestorPosition.Hedge == HedgeType.Speculation) ? "投机" : "套利");
						this.dtPosition.Rows.Add(dataRow);
					}
					else
					{
						dataRow = array[0];
						if (pInvestorPosition.PositionDate == PositionDateType.History)
						{
							dataRow["昨仓"] = (pInvestorPosition.Position - pInvestorPosition.TdPosition).ToString();
							dataRow["可平昨"] = (pInvestorPosition.Position - pInvestorPosition.TdPosition).ToString();
						}
						else
						{
							dataRow["今仓"] = pInvestorPosition.TdPosition.ToString();
							dataRow["可平今"] = pInvestorPosition.TdPosition.ToString();
						}
						dataRow["总持仓"] = (Convert.ToInt32(dataRow["昨仓"].ToString()) + Convert.ToInt32(dataRow["今仓"].ToString())).ToString();
						dataRow["持仓均价"] = (Convert.ToDouble(dataRow["持仓均价"].ToString()) + ((num == 0) ? 0.0 : (pInvestorPosition.PositionCost / (double)num))).ToString("F3");
						dataRow["开仓均价"] = (Convert.ToDouble(dataRow["开仓均价"].ToString()) + ((num == 0) ? 0.0 : (pInvestorPosition.OpenCost / (double)num))).ToString("F3");
						dataRow["持仓盈亏"] = (Convert.ToDouble(dataRow["持仓盈亏"].ToString()) + pInvestorPosition.PositionProfit).ToString("F3");
						dataRow["空头占用保证金"] = ((pInvestorPosition.Direction == DirectionType.Buy) ? "0" : (Convert.ToDouble(dataRow["空头占用保证金"].ToString()) + pInvestorPosition.UseMargin).ToString("F2"));
						dataRow["多头占用保证金"] = ((pInvestorPosition.Direction == DirectionType.Buy) ? (Convert.ToDouble(dataRow["多头占用保证金"].ToString()) + pInvestorPosition.UseMargin).ToString("F2") : "0");
					}
					if (dataRow["昨仓"].ToString() == "0" && dataRow["今仓"].ToString() == "0")
					{
						this.dtPosition.Rows.Remove(dataRow);
					}
				}
			}
			if (pLast)
			{
				for (int i = 0; i < this.dtPosition.Rows.Count; i++)
				{
					this.dtPosition.Rows[i]["持仓均价"] = (Convert.ToDouble(this.dtPosition.Rows[i]["持仓均价"].ToString()) / (double)Convert.ToInt32(this.dtPosition.Rows[i]["总持仓"].ToString())).ToString("F3");
					this.dtPosition.Rows[i]["开仓均价"] = (Convert.ToDouble(this.dtPosition.Rows[i]["开仓均价"].ToString()) / (double)Convert.ToInt32(this.dtPosition.Rows[i]["总持仓"].ToString())).ToString("F3");
				}
				if (this.dtPosition.Rows.Count >= 0)
				{
					DataTable dataTable = new DataTable();
					dataTable = this.dtPosition.Copy();
					dataTable.Rows.Clear();
					for (int i = 0; i < this.dtPosition.Rows.Count; i++)
					{
						dataTable.ImportRow(this.dtPosition.Rows[i]);
					}
					this.OnAnswer(new TradeApi.ObjectAndKey(dataTable, "持仓@" + pInvestorPosition.InvestorID));
				}
				this.apiIsBusy = false;
				this.dtPosition.Clear();
			}
		}

		private void _import_OnRspQryInstrument(InstrumentField pInstrument, InfoField pInfo, bool pLast)
		{
			if (pInfo.ErrorID == 0)
			{
				if (this.dtInstruments.Rows.Find(pInstrument.InstrumentID) == null && !pInstrument.InstrumentID.Contains("&"))
				{
					pInstrument.LongMarginRatio = 0.0;
					pInstrument.ShortMarginRatio = 0.0;
					DataRow dataRow = this.dtInstruments.Rows.Add(new object[]
					{
						pInstrument.ProductID,
						pInstrument.InstrumentID,
						pInstrument.InstrumentName,
						pInstrument.ExchangeID,
						pInstrument.VolumeMultiple,
						pInstrument.PriceTick,
						pInstrument.LongMarginRatio,
						pInstrument.ShortMarginRatio,
						DBNull.Value,
						DBNull.Value,
						DBNull.Value,
						DBNull.Value,
						DBNull.Value,
						DBNull.Value,
						pInstrument.MaxLimitOrderVolume,
						pInstrument.MinMarketOrderVolume,
						false
					});
					if (!this.DlistVariety.ContainsKey(pInstrument.ProductID))
					{
						this.DlistVariety.Add(pInstrument.ProductID, new List<string>
						{
							pInstrument.InstrumentID
						});
					}
					else
					{
						this.DlistVariety[pInstrument.ProductID].Add(pInstrument.InstrumentID);
					}
				}
				if (pLast)
				{
					this.ReqQryAccount();
					this.listQry.Add(new TradeApi.Order(TradeApi.EnumQryOrder.QryOrder, this, null, null));
					this.listQry.Add(new TradeApi.Order(TradeApi.EnumQryOrder.QryTrade, this, null, null));
					if (this.needQryRate)
					{
						foreach (List<string> current in this.DlistVariety.Values)
						{
							this.listQry.Add(new TradeApi.Order(TradeApi.EnumQryOrder.QryInstrumentCommissionRate, this, current[0], null));
						}
					}
					else
					{
						using (FileStream fileStream = File.OpenRead(this.RatePath))
						{
							StreamReader streamReader = new StreamReader(fileStream);
							string[] array = streamReader.ReadToEnd().Split(new string[]
							{
								"\r\n"
							}, StringSplitOptions.RemoveEmptyEntries);
							for (int i = 0; i < array.Length; i++)
							{
								string[] array2 = array[i].Split(new string[]
								{
									"\t"
								}, StringSplitOptions.RemoveEmptyEntries);
								DataRow[] array3 = this.dtInstruments.Select("品种代码 = '" + array2[0] + "'");
								for (int j = 0; j < array3.Length; j++)
								{
									array3[j]["开仓手续费"] = Convert.ToDouble(array2[1]);
									array3[j]["平仓手续费"] = Convert.ToDouble(array2[2]);
									array3[j]["平今手续费"] = Convert.ToDouble(array2[3]);
									array3[j]["开仓手续费率"] = Convert.ToDouble(array2[4]);
									array3[j]["平仓手续费率"] = Convert.ToDouble(array2[5]);
									array3[j]["平今手续费率"] = Convert.ToDouble(array2[6]);
								}
								this.DlistVariety.Remove(array2[0]);
							}
						}
						foreach (List<string> current in this.DlistVariety.Values)
						{
							if (!current[0].Contains("&"))
							{
								this.listQry.Add(new TradeApi.Order(TradeApi.EnumQryOrder.QryInstrumentCommissionRate, this, current[0], null));
							}
						}
					}
					this.OnMsg(new string[]
					{
						this.InvestorID,
						"查合约完成"
					});
					this.OnMsg(new string[]
					{
						this.InvestorID,
						"查详细..."
					});
					this.listQry.Add(new TradeApi.Order(TradeApi.EnumQryOrder.QryInvestor, this, null, null));
					this.listQry.Add(new TradeApi.Order(TradeApi.EnumQryOrder.QryIntorverPosition, this, null, null));
					this.listQry.Add(new TradeApi.Order(TradeApi.EnumQryOrder.QryInvestorPositionDetail, this, null, null));
				}
			}
		}

		private void _import_OnRspQryInvestor(InvestorField pInvestor, bool pLast)
		{
			this.OnAnswer(new TradeApi.ObjectAndKey(pInvestor, this.InvestorID));
			if (pLast)
			{
				this.apiIsBusy = false;
			}
		}

		private void _import_OnRspQryPositiontDetail(PositionDetailField pInvestorPositionDetail, bool pLast)
		{
			if (pInvestorPositionDetail.TradingDay != "")
			{
				DataRow dataRow = this.dtInstruments.Rows.Find(pInvestorPositionDetail.InstrumentID);
				if (dataRow != null && dataRow["保证金-多"].ToString() == "0")
				{
					dataRow["保证金-多"] = pInvestorPositionDetail.MarginRateByMoney;
					dataRow["保证金-空"] = pInvestorPositionDetail.MarginRateByMoney;
				}
				if (pInvestorPositionDetail.Volume > 0)
				{
					DataRow dataRow2 = this.dtPositionDetail.NewRow();
					dataRow2["编号"] = pInvestorPositionDetail.TradeID.Trim();
					dataRow2["投资者帐户"] = pInvestorPositionDetail.InvestorID;
					dataRow2["合约"] = pInvestorPositionDetail.InstrumentID;
					dataRow2["买卖"] = ((pInvestorPositionDetail.Direction == DirectionType.Buy) ? "买" : "    卖");
					dataRow2["持仓类型"] = ((Convert.ToInt32(pInvestorPositionDetail.OpenDate) < Convert.ToInt32(pInvestorPositionDetail.TradingDay)) ? "昨仓" : "今仓");
					dataRow2["成交手数"] = pInvestorPositionDetail.Volume.ToString();
					dataRow2["成交价格"] = pInvestorPositionDetail.OpenPrice.ToString();
					dataRow2["成交时间"] = pInvestorPositionDetail.OpenDate;
					this.dtPositionDetail.Rows.Add(dataRow2);
				}
			}
			if (pLast)
			{
				if (this.dtPositionDetail.Rows.Count >= 0)
				{
					DataTable dataTable = new DataTable();
					dataTable = this.dtPositionDetail.Copy();
					dataTable.Rows.Clear();
					for (int i = 0; i < this.dtPositionDetail.Rows.Count; i++)
					{
						dataTable.ImportRow(this.dtPositionDetail.Rows[i]);
					}
					this.OnAnswer(new TradeApi.ObjectAndKey(dataTable, "持仓明细@" + pInvestorPositionDetail.InvestorID));
				}
				this.OnMsg(new string[]
				{
					this.InvestorID,
					"已登录"
				});
				this.LoginCount++;
				this.IsLogin = true;
				this.apiIsBusy = false;
				this.dtPositionDetail.Clear();
			}
		}

		private void _import_OnRspOrderInsert(ErrOrderField pInputOrder, InfoField pInfo)
		{
			if (pInfo.ErrorID != 0)
			{
				this.OnAnswer(new TradeApi.ObjectAndKey(pInputOrder, string.Concat(new object[]
				{
					this.FrontID,
					",",
					this.SessionID,
					";",
					pInfo.ErrorMsg
				})));
			}
		}

		private void _import_OnErrRtnOrderInsert(ErrOrderField pInputOrder, InfoField pInfo)
		{
			if (pInfo.ErrorID != 0)
			{
				this.OnAnswer(new TradeApi.ObjectAndKey(pInputOrder, string.Concat(new object[]
				{
					this.FrontID,
					",",
					this.SessionID,
					";",
					pInfo.ErrorMsg
				})));
			}
		}

		private void _import_OnRspQryInstrumentCommissionRate(InstrumentCommissionRateField pInstrumentCommissionRate, bool bIsLast)
		{
			if (bIsLast)
			{
				if (pInstrumentCommissionRate.InstrumentID != "" && this.instrument4QryRate.StartsWith(pInstrumentCommissionRate.InstrumentID))
				{
					string instrumentID = pInstrumentCommissionRate.InstrumentID;
					Regex regex = new Regex("[a-zA-Z]+");
					Match match = regex.Match(instrumentID);
					string value = match.Value;
					DataRow[] array = this.dtInstruments.Select("品种代码='" + value + "'");
					for (int i = 0; i < array.Length; i++)
					{
						array[i]["开仓手续费"] = pInstrumentCommissionRate.OpenRatioByVolume;
						array[i]["平仓手续费"] = pInstrumentCommissionRate.CloseRatioByVolume;
						array[i]["平今手续费"] = pInstrumentCommissionRate.CloseTodayRatioByVolume;
						array[i]["开仓手续费率"] = pInstrumentCommissionRate.OpenRatioByMoney;
						array[i]["平仓手续费率"] = pInstrumentCommissionRate.CloseRatioByMoney;
						array[i]["平今手续费率"] = pInstrumentCommissionRate.CloseTodayRatioByMoney;
					}
					using (StreamWriter streamWriter = File.AppendText(this.RatePath))
					{
						string value2 = string.Concat(new object[]
						{
							value,
							"\t",
							pInstrumentCommissionRate.OpenRatioByVolume,
							"\t",
							pInstrumentCommissionRate.CloseRatioByVolume,
							"\t",
							pInstrumentCommissionRate.CloseTodayRatioByVolume.ToString(),
							"\t",
							pInstrumentCommissionRate.OpenRatioByMoney.ToString(),
							"\t",
							pInstrumentCommissionRate.CloseRatioByMoney,
							"\t",
							pInstrumentCommissionRate.CloseTodayRatioByMoney.ToString(),
							"\t"
						});
						streamWriter.WriteLine(value2);
						streamWriter.Flush();
					}
				}
				this.apiIsBusy = false;
			}
		}

		private void _import_OnRspQryInstrumentMarginRate(InstrumentMarginRateField pInstrumentMarginRate, bool bIsLast)
		{
			DataRow dataRow = this.dtInstruments.Rows.Find(pInstrumentMarginRate.InstrumentID);
			if (dataRow != null)
			{
				if (pInstrumentMarginRate.IsRelative == 0)
				{
					dataRow["保证金-多"] = pInstrumentMarginRate.LongMarginRatioByMoney;
					dataRow["保证金-空"] = pInstrumentMarginRate.ShortMarginRatioByMoney;
				}
				else
				{
					dataRow["保证金-多"] = (double)this.dtInstruments.Rows.Find(pInstrumentMarginRate.InstrumentID)["保证金-多"] + pInstrumentMarginRate.LongMarginRatioByMoney;
					dataRow["保证金-空"] = (double)this.dtInstruments.Rows.Find(pInstrumentMarginRate.InstrumentID)["保证金-空"] + pInstrumentMarginRate.ShortMarginRatioByMoney;
				}
			}
			if (bIsLast)
			{
				this.OnAnswer(new TradeApi.ObjectAndKey(pInstrumentMarginRate, this.Group));
				this.apiIsBusy = false;
			}
		}

		private void _import_OnRspUserLogout(int pReason)
		{
			string text = pReason.ToString();
			string text2;
			if (text != null)
			{
				if (text == "4097")
				{
					text2 = "网络读失败中断";
					goto IL_85;
				}
				if (text == "4098")
				{
					text2 = "网络写失败中断";
					goto IL_85;
				}
				if (text == "8193")
				{
					text2 = "接收心跳超时";
					goto IL_85;
				}
				if (text == "8194")
				{
					text2 = "发送心跳失败";
					goto IL_85;
				}
				if (text == "8195")
				{
					text2 = "收到错误报文";
					goto IL_85;
				}
			}
			text2 = "断开连接";
			IL_85:
			this.OnMsg(new string[]
			{
				this.InvestorID,
				text2
			});
			this.IsLogin = false;
		}

		private void _import_OnRspQrySettlementInfo(SettlementInfoField pSettlement, bool pLast)
		{
			this.settlement += pSettlement.Content;
			if (pLast)
			{
				this.OnAnswer(new TradeApi.ObjectAndKey(this.settlement, "结算单"));
				this.apiIsBusy = false;
				this.settlement = "";
			}
		}

		private void _import_OnRspUserLogin(UserLoginField pUserLogin, InfoField pInfo)
		{
			if (pInfo.ErrorID == 0)
			{
				this.TradingDay = pUserLogin.TradingDay;
				this.FrontID = pUserLogin.FrontID;
				this.SessionID = pUserLogin.SessionID;
				this.MaxOrderRef = (string.IsNullOrWhiteSpace(pUserLogin.MaxOrderRef) ? 0 : int.Parse(pUserLogin.MaxOrderRef));
				this.OnMsg(new string[]
				{
					pUserLogin.UserID,
					"登录成功"
				});
				this.OnMsg(new string[]
				{
					pUserLogin.UserID,
					"查询结算确认..."
				});
				TimeSpan t = default(TimeSpan);
				if (!TimeSpan.TryParse(pUserLogin.SHFETime, out t))
				{
					if (!TimeSpan.TryParse(pUserLogin.FFEXTime, out t))
					{
						if (!TimeSpan.TryParse(pUserLogin.CZCETime, out t))
						{
							if (!TimeSpan.TryParse(pUserLogin.DCETime, out t))
							{
								t = TimeSpan.Parse("0");
							}
						}
					}
				}
				this.tsDiffSHFE = DateTime.Now.TimeOfDay - t;
				if (TimeSpan.TryParse(pUserLogin.CZCETime, out this.tsDiffCZCE))
				{
					this.tsDiffCZCE = DateTime.Now.TimeOfDay - this.tsDiffCZCE;
				}
				else
				{
					this.tsDiffCZCE = this.tsDiffSHFE;
				}
				if (TimeSpan.TryParse(pUserLogin.DCETime, out this.tsDiffDCE))
				{
					this.tsDiffDCE = DateTime.Now.TimeOfDay - this.tsDiffDCE;
				}
				else
				{
					this.tsDiffDCE = this.tsDiffSHFE;
				}
				if (TimeSpan.TryParse(pUserLogin.FFEXTime, out this.tsDiffCFFEX))
				{
					this.tsDiffCFFEX = DateTime.Now.TimeOfDay - this.tsDiffCFFEX;
				}
				else
				{
					this.tsDiffCFFEX = this.tsDiffSHFE;
				}
				this.ReqSettlementInfoConfirm();
			}
			else
			{
				this.OnMsg(new string[]
				{
					this.InvestorID,
					pInfo.ErrorMsg
				});
			}
		}

		private void _import_OnFrontConnected()
		{
			this.OnMsg(new string[]
			{
				this.InvestorID,
				"连接成功"
			});
			this.OnMsg(new string[]
			{
				this.InvestorID,
				"登录中..."
			});
			this.ReqUserLogin();
		}

		public int ReqConnect()
		{
			return this._proxy.ReqConnect(this.Path, this.FrontAddr);
		}

		public void ReqDisConnect()
		{
			this._proxy.ReqUserLogout();
		}

		public int ReqUserLogin()
		{
			this.Isconned = true;
			return this._proxy.ReqUserLogin(this.InvestorID, this.passWord, this.BrokerID, this.clientip.Split(new string[]
			{
				"."
			}, StringSplitOptions.RemoveEmptyEntries)[3]);
		}

		public void ReqUserLogout()
		{
			this._proxy.ReqUserLogout();
		}

		public int ReqOrderInsert(string pInstrument, DirectionType pDirection, EOffsetType pOffset, double pPrice, int pVolume, string pRef, HedgeType pHedge = HedgeType.Speculation, OrderType pType = OrderType.Limit)
		{
			return this._proxy.ReqOrderInsert(pInstrument, pDirection, pOffset, pPrice, pVolume, pRef, pHedge, pType);
		}

		public int ReqOrderAction(string InstrumentID, int _FrontID, int _SessionID, string _OrderRef)
		{
			return this._proxy.ReqOrderAction(InstrumentID, _FrontID, _SessionID, _OrderRef);
		}

		public int ReqQryOrder()
		{
			return this._proxy.ReqQryOrder();
		}

		public int ReqQryTrade()
		{
			return this._proxy.ReqQryTrade();
		}

		public int ReqQryInstrument(string pInstrument)
		{
			return this._proxy.ReqQryInstrument(pInstrument);
		}

		public int ReqQryPosition()
		{
			return this._proxy.ReqQryPosition();
		}

		public int ReqQryPositionDetail()
		{
			return this._proxy.ReqQryPositionDetail();
		}

		public int ReqQryAccount()
		{
			return this._proxy.ReqQryAccount();
		}

		public int ReqQryInvestor()
		{
			return this._proxy.ReqQryInvestor();
		}

		public int ReqQryInstrumentCommissionRate(string pInstrument)
		{
			return this._proxy.ReqQryInstrumentCommissionRate(pInstrument);
		}

		public int ReqQryInstrumentMarginRate(string pInstrument)
		{
			return this._proxy.ReqQryInstrumentMarginRate(pInstrument);
		}

		public int ReqSettlementInfo(string date)
		{
			return this._proxy.ReqSettlementInfo(date);
		}

		public int ReqSettlementInfoConfirm()
		{
			return this._proxy.ReqSettlementInfoConfirm();
		}
	}
}
