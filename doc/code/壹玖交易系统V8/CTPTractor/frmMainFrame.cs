using CTPTractor.Properties;
using DevExpress.Data;
using DevExpress.LookAndFeel;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraTab;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Trade2015;

namespace CTPTractor
{
	public class frmMainFrame : System.Windows.Forms.Form
	{
		private delegate frmMainFrame.ObjectAndKey NewTaskDelegate(frmMainFrame.ObjectAndKey ms);

		private enum EnumProgessState
		{
			OnMdConnected,
			OnMdDisConnected,
			Connect,
			OnConnected,
			DisConnect,
			OnDisConnect,
			Login,
			OnLogin,
			Logout,
			OnLogout,
			QrySettleConfirmInfo,
			OnQrySettleConfirmInfo,
			QrySettleInfo,
			OnQrySettleInfo,
			SettleConfirm,
			OnSettleConfirm,
			QryInstrument,
			OnQryInstrument,
			QryOrder,
			OnQryOrder,
			QryTrade,
			OnQryTrade,
			QryPosition,
			OnQryPosition,
			QryAccount,
			OnQryAccount,
			QryParkedOrder,
			OnQryParkedOrder,
			QryParkedOrderAction,
			OnQryParkedOrderAction,
			QryDepthMarketData,
			OnQryDepthMarketData,
			QryPositionDetail,
			OnQryPositionDetail,
			Other,
			OnError,
			OrderInsert,
			OnErrOrderInsert,
			OnRtnOrder,
			OnRtnTrade,
			OrderAction,
			OnErrOrderAction,
			OnOrderAction,
			OnRtnTradingNotice,
			OnRtnInstrumentStatus,
			RemovePackedOrder,
			OnRemovePackedOrder,
			RemovePackedOrderAction,
			OnRemovePackedOrderAction,
			ParkedOrder,
			OnParkedOrder,
			ParkedOrderAction,
			OnParkedOrderAction,
			QuickClose,
			QuickLock,
			UpdateUserPassword,
			OnUpdateUserPassword,
			UpdateAccountPassword,
			OnUpdateAccountPassword,
			FutureToBank,
			OnFutureToBank,
			BankToFuture,
			OnBankToFuture,
			QryBankAccountMoney,
			OnQryBankAccountMoney,
			QuickCover,
			OnQryInstrumentMarginRate,
			QryInstrumentMarginRate
		}

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

		public class hotkeyIDAndCode
		{
			public int keyID
			{
				get;
				set;
			}

			public string keyCode
			{
				get;
				set;
			}

			public string keystr
			{
				get;
				set;
			}
		}

		private delegate void InvokeCallback(string msg, bool error);

		private HotKeys hotkey = new HotKeys();

		private System.Collections.Generic.List<frmMainFrame.hotkeyIDAndCode> hotkeyidandcode = new System.Collections.Generic.List<frmMainFrame.hotkeyIDAndCode>();

		private System.Data.DataSet ds_GroupPosition = new System.Data.DataSet();

		private System.Data.DataTable dt_AllPosition = new System.Data.DataTable();

		private System.Data.DataSet ds_GroupPositionDetail = new System.Data.DataSet();

		private System.Data.DataTable dt_AllTrade = new System.Data.DataTable("alltrade");

		private System.Data.DataTable dt_AllDeal = new System.Data.DataTable("alldeal");

		private System.Data.DataTable dt_MainTrade = new System.Data.DataTable("maintrade");

		private System.Data.DataTable dt_MainDeal = new System.Data.DataTable("maindeal");

		private System.Data.DataTable dt_account_dg = new System.Data.DataTable();

		private System.Data.DataTable dt_mainaccount_dg = new System.Data.DataTable();

		private System.Data.DataTable dt_AccountTradeSet = new System.Data.DataTable();

		private System.Data.DataTable dt_CodeSet = new System.Data.DataTable();

		private System.Data.DataTable dt_SubAccountSet = new System.Data.DataTable();

		private System.Data.DataTable DtTB = new System.Data.DataTable();

		private System.Data.DataTable dtMarketData = new System.Data.DataTable("MarketData");

		private FrmPosition frmp;

		private System.Collections.Generic.Dictionary<string, TradeApi> Dic_TradeApi = new System.Collections.Generic.Dictionary<string, TradeApi>();

		private System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string[]>> Dic_MainCheck = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string[]>>();

		private System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string[]>> Dic_SeparateCheck = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string[]>>();

		private System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string[]>> Dic_ColorCheck = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string[]>>();

		private System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string[]>> Dic_MainTrade = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string[]>>();

		private ConcurrentDictionary<string, System.Collections.Generic.List<MSOrderMode>> Dic_SeparateTrade = new ConcurrentDictionary<string, System.Collections.Generic.List<MSOrderMode>>();

		private SQLiteConnectionStringBuilder connstr = new SQLiteConnectionStringBuilder();

		private GetID gid = new GetID();

		private ReadTxtToDatatable readttod = new ReadTxtToDatatable();

		private bool freshOrderPrice = false;

		private MdApi mdApi = null;

		private System.Threading.Thread threadFreshMarketData = null;

		private System.Collections.Generic.List<CThostFtdcDepthMarketDataField> listMarketDatas = new System.Collections.Generic.List<CThostFtdcDepthMarketDataField>();

		private string xmlTrade = System.Windows.Forms.Application.ExecutablePath.Substring(0, System.Windows.Forms.Application.ExecutablePath.LastIndexOf("\\")) + "\\XG_Trade.xml";

		private string xmlDeal = System.Windows.Forms.Application.ExecutablePath.Substring(0, System.Windows.Forms.Application.ExecutablePath.LastIndexOf("\\")) + "\\XG_Deal.xml";

		private string xmlMainTrade = System.Windows.Forms.Application.ExecutablePath.Substring(0, System.Windows.Forms.Application.ExecutablePath.LastIndexOf("\\")) + "\\XG_mainTrade.xml";

		private string xmlMainDeal = System.Windows.Forms.Application.ExecutablePath.Substring(0, System.Windows.Forms.Application.ExecutablePath.LastIndexOf("\\")) + "\\XG_mainDeal.xml";

		private string dbpath = System.Windows.Forms.Application.ExecutablePath.Substring(0, System.Windows.Forms.Application.ExecutablePath.LastIndexOf("\\")) + "\\Msg.db";

		private IniOP ini = new IniOP(System.Windows.Forms.Application.ExecutablePath.Substring(0, System.Windows.Forms.Application.ExecutablePath.LastIndexOf("\\")) + "\\user.ini");

		private bool firstloginaccount = true;

		private bool BeginListen = false;

		private bool TradeORDeal = false;

		private System.Threading.Thread[] th_tread;

		private System.Threading.Thread[] th_sepCheck;

		private System.Threading.Thread[] th_colorCheck;

		private System.Collections.Generic.List<string[]> listprintmsg = new System.Collections.Generic.List<string[]>();

		private System.IO.StreamWriter logsw;

		private System.IO.FileStream logfs;

		private System.Threading.Thread th_print;

		private bool havechangecode = false;

		private System.Threading.Thread th_changcode;

		private string select_code = "";

		private float prooftime = 1f;

		private string brokerspath = System.Windows.Forms.Application.ExecutablePath.Substring(0, System.Windows.Forms.Application.ExecutablePath.LastIndexOf("\\")) + "\\brokers\\";

		private System.Collections.Generic.List<Brokers> List_brokers = new System.Collections.Generic.List<Brokers>();

		private int roundwrong = 0;

		private object CheckLock = new object();

		private object AddLock = new object();

		private object AllTradeLock = new object();

		private object tmTradeLock = new object();

		private object DetailLock = new object();

		private System.Collections.Generic.List<string> HasAddMarket = new System.Collections.Generic.List<string>();

		private System.Collections.Generic.List<string> NeedAddMarket = new System.Collections.Generic.List<string>();

		private bool has_regedit = false;

		public int max = 1;

		private bool beginHandCheck = false;

		private System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string[]>> dlchecktrans = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string[]>>();

		private frmConfirm fcf = new frmConfirm();

		private int saifbuy = 0;

		private int saifsell = 0;

		private int saihbuy = 0;

		private int saihsell = 0;

		private int saicbuy = 0;

		private int saicsell = 0;

		private int saaubuy = 0;

		private int saausell = 0;

		private int saagbuy = 0;

		private int saagsell = 0;

		private int sacubuy = 0;

		private int sacusell = 0;

		private int sarubuy = 0;

		private int sarusell = 0;

		private int sanibuy = 0;

		private int sanisell = 0;

		private int sarbbuy = 0;

		private int sarbsell = 0;

		private int saibuy = 0;

		private int saisell = 0;

		private int saybuy = 0;

		private int saysell = 0;

		private int sambuy = 0;

		private int samsell = 0;

		private int sappbuy = 0;

		private int sappsell = 0;

		private int salbuy = 0;

		private int salsell = 0;

		private int saMAbuy = 0;

		private int saMAsell = 0;

		private int saRMbuy = 0;

		private int saRMsell = 0;

		private int saSRbuy = 0;

		private int saSRsell = 0;

		private int aifbuy = 0;

		private int aifsell = 0;

		private int aihbuy = 0;

		private int aihsell = 0;

		private int aicbuy = 0;

		private int aicsell = 0;

		private int aaubuy = 0;

		private int aausell = 0;

		private int aagbuy = 0;

		private int aagsell = 0;

		private int acubuy = 0;

		private int acusell = 0;

		private int arubuy = 0;

		private int arusell = 0;

		private int anibuy = 0;

		private int anisell = 0;

		private int arbbuy = 0;

		private int arbsell = 0;

		private int aibuy = 0;

		private int aisell = 0;

		private int aybuy = 0;

		private int aysell = 0;

		private int ambuy = 0;

		private int amsell = 0;

		private int appbuy = 0;

		private int appsell = 0;

		private int albuy = 0;

		private int alsell = 0;

		private int aMAbuy = 0;

		private int aMAsell = 0;

		private int aRMbuy = 0;

		private int aRMsell = 0;

		private int aSRbuy = 0;

		private int aSRsell = 0;

		private IContainer components = null;

		private System.Windows.Forms.DateTimePicker dateTimePicker1;

		private System.Windows.Forms.Panel panel1;

		private System.Windows.Forms.DomainUpDown domainUpDown1;

		private System.Windows.Forms.Button butLogin;

		private System.Windows.Forms.Timer timer1;

		private System.Windows.Forms.Button butLoginOut;

		private System.Windows.Forms.ColumnHeader columnHeader45;

		private System.Windows.Forms.ColumnHeader columnHeader53;

		private System.Windows.Forms.ColumnHeader columnHeader46;

		private System.Windows.Forms.ColumnHeader columnHeader47;

		private System.Windows.Forms.ColumnHeader columnHeader48;

		private System.Windows.Forms.ColumnHeader columnHeader49;

		private System.Windows.Forms.ColumnHeader columnHeader50;

		private System.Windows.Forms.ColumnHeader columnHeader51;

		private System.Windows.Forms.ColumnHeader columnHeader54;

		private System.Windows.Forms.ColumnHeader columnHeader15;

		private System.Windows.Forms.ColumnHeader columnHeader17;

		private System.Windows.Forms.ColumnHeader columnHeader18;

		private System.Windows.Forms.ColumnHeader columnHeader19;

		private System.Windows.Forms.ColumnHeader columnHeader20;

		private System.Windows.Forms.ColumnHeader columnHeader21;

		private System.Windows.Forms.ColumnHeader columnHeader22;

		private System.Windows.Forms.ColumnHeader columnHeader23;

		private System.Windows.Forms.ColumnHeader columnHeader24;

		private System.Windows.Forms.ComboBox comboBoxErrMsg;

		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;

		private System.Windows.Forms.ToolStripMenuItem Revoke;

		private System.Windows.Forms.ToolStripMenuItem FullRevoke;

		private System.Windows.Forms.RadioButton radioButtonMd;

		private System.Windows.Forms.ToolTip toolTip1;

		private System.Windows.Forms.Button butSelAll;

		private System.Windows.Forms.MenuStrip menuStrip1;

		private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItem;

		private System.Windows.Forms.ToolStripMenuItem AccountSetTSMI;

		private System.Windows.Forms.ToolStripMenuItem TradingAccountTSMI;

		private System.Windows.Forms.ColumnHeader columnHeader36;

		private System.Windows.Forms.ColumnHeader columnHeader37;

		private System.Windows.Forms.ColumnHeader columnHeader38;

		private System.Windows.Forms.ColumnHeader columnHeader39;

		private System.Windows.Forms.ColumnHeader columnHeader40;

		private System.Windows.Forms.ColumnHeader columnHeader41;

		private System.Windows.Forms.ColumnHeader columnHeader42;

		private System.Windows.Forms.ColumnHeader columnHeader43;

		private System.Windows.Forms.ColumnHeader columnHeader44;

		private System.Windows.Forms.ColumnHeader columnHeader52;

		private System.Windows.Forms.ColumnHeader columnHeader55;

		private System.Windows.Forms.ColumnHeader columnHeader56;

		private System.Windows.Forms.ColumnHeader columnHeader57;

		private System.Windows.Forms.ColumnHeader columnHeader58;

		private System.Windows.Forms.ColumnHeader columnHeader59;

		private System.Windows.Forms.ColumnHeader account;

		private System.Windows.Forms.ColumnHeader num;

		private System.Windows.Forms.ColumnHeader code;

		private System.Windows.Forms.ColumnHeader directortype;

		private System.Windows.Forms.ColumnHeader offsetflag;

		private System.Windows.Forms.ColumnHeader status;

		private System.Windows.Forms.ColumnHeader price;

		private System.Windows.Forms.ColumnHeader orderhand;

		private System.Windows.Forms.ColumnHeader notrade;

		private System.Windows.Forms.ColumnHeader tradehand;

		private System.Windows.Forms.ColumnHeader ordertime;

		private System.Windows.Forms.ColumnHeader tradetime;

		private System.Windows.Forms.ColumnHeader tradeprice;

		private System.Windows.Forms.ColumnHeader detailstatus;

		private System.Windows.Forms.ColumnHeader custome;

		private System.Windows.Forms.ColumnHeader columnHeader34;

		private System.Windows.Forms.ColumnHeader columnHeader25;

		private System.Windows.Forms.ColumnHeader columnHeader26;

		private System.Windows.Forms.ColumnHeader columnHeader27;

		private System.Windows.Forms.ColumnHeader columnHeader28;

		private System.Windows.Forms.ColumnHeader columnHeader29;

		private System.Windows.Forms.ColumnHeader columnHeader30;

		private System.Windows.Forms.ColumnHeader columnHeader31;

		private System.Windows.Forms.ColumnHeader columnHeader32;

		private System.Windows.Forms.ColumnHeader columnHeader33;

		private System.Windows.Forms.ColumnHeader columnHeader35;

		private GridControl xdgAccount;

		private BandedGridView bgvAccount;

		private BandedGridColumn choose;

		private BandedGridColumn colAccount;

		private BandedGridColumn colInvestor;

		private BandedGridColumn colState;

		private BandedGridColumn colBalance;

		private BandedGridColumn colPreBalance;

		private BandedGridColumn colFrozenCash;

		private BandedGridColumn colCurrMargin;

		private BandedGridColumn colAvailable;

		private BandedGridColumn colCloseProfit;

		private BandedGridColumn colPositionProfit;

		private BandedGridColumn colCommission;

		private BandedGridColumn colDeposit;

		private BandedGridColumn colWithdraw;

		private BandedGridColumn colFrozenMargin;

		private RepositoryItemCheckEdit repositoryItemCheckEdit3;

		private RepositoryItemSpinEdit repositoryItemSpinEdit1;

		private System.Windows.Forms.ToolStripMenuItem TSMI_Position;

		private System.Windows.Forms.ToolStripSeparator 按帐户分组ToolStripMenuItem;

		private System.Windows.Forms.ToolStripMenuItem InvestorPositionTSMI;

		private System.Windows.Forms.Timer tmMoneyRefresh;

		private System.Windows.Forms.ToolStripMenuItem PassWordTSMI;

		private DraggableTabControl tab;

		private System.Windows.Forms.Button buttonMarketPrice;

		private System.Windows.Forms.NumericUpDown numericUpDownPrice;

		private System.Windows.Forms.Button buttonOrder;

		private System.Windows.Forms.Label label7;

		private System.Windows.Forms.Button buttonCancel;

		private System.Windows.Forms.Label label15;

		private System.Windows.Forms.NumericUpDown numericUpDownVolume;

		private System.Windows.Forms.Label labelVolumeMax;

		private System.Windows.Forms.Label label3;

		private System.Windows.Forms.Label label6;

		private System.Windows.Forms.Label labelLower;

		private System.Windows.Forms.Button buttonPrice;

		private System.Windows.Forms.Label labelUpper;

		private System.Windows.Forms.Label label5;

		private GridControl xdgMainAccount;

		private BandedGridView bgcMainAccount;

		private BandedGridColumn bandedGridColumn1;

		private BandedGridColumn bandedGridColumn2;

		private BandedGridColumn bandedGridColumn3;

		private BandedGridColumn bandedGridColumn5;

		private SplitContainerControl spcAccount;

		private SplitContainerControl spcPosition;

		private SplitContainerControl spcMain;

		private SplitContainerControl spcMainAccount;

		private SplitContainerControl splitContainerControl2;

		private System.Windows.Forms.Button btnMainSelAll;

		private System.Windows.Forms.Button butMainLoginOut;

		private System.Windows.Forms.Button butMainLogin;

		private SplitContainerControl spcMainPosition;

		private DraggableTabControl tabMain;

		private XtraTabControl xtraTabControl1;

		private XtraTabPage xtabTrade;

		private XtraTabPage xtabViewTrade;

		private System.Windows.Forms.RadioButton rdoError;

		private System.Windows.Forms.RadioButton rdoRevoke;

		private System.Windows.Forms.RadioButton rdoDeal;

		private System.Windows.Forms.RadioButton rdoSuspend;

		private System.Windows.Forms.RadioButton rdoAll;

		private GridControl xdgDeal;

		private GridView gvDeal;

		private GridColumn gridColumn12;

		private GridColumn gridColumn13;

		private GridColumn gridColumn14;

		private GridColumn gridColumn15;

		private GridColumn gridColumn16;

		private GridColumn gridColumn18;

		private GridColumn gridColumn21;

		private GridColumn gridColumn23;

		private GridColumn gridColumn24;

		private GridColumn gridColumn25;

		private GridColumn gridColumn26;

		private XtraTabControl xtraTabControl2;

		private XtraTabPage xtraTabPage1;

		private System.Windows.Forms.RadioButton rdoMainRevoke;

		private System.Windows.Forms.RadioButton rdoMainDeal;

		private System.Windows.Forms.RadioButton rdoMainSuspend;

		private System.Windows.Forms.RadioButton rdoMainAll;

		private GridControl xdgMianTrade;

		private GridView gvMainTrade;

		private GridColumn gridColumn19;

		private GridColumn gridColumn20;

		private GridColumn gridColumn22;

		private GridColumn gridColumn27;

		private GridColumn gridColumn28;

		private GridColumn gridColumn29;

		private GridColumn gridColumn30;

		private GridColumn gridColumn31;

		private GridColumn gridColumn32;

		private GridColumn gridColumn33;

		private GridColumn gridColumn34;

		private GridColumn gridColumn35;

		private GridColumn gridColumn36;

		private GridColumn gridColumn37;

		private GridColumn gridColumn38;

		private GridColumn gridColumn39;

		private XtraTabPage xtraTabPage2;

		private GridControl xdgMainDeal;

		private GridView gvMainDeal;

		private GridColumn gridColumn40;

		private GridColumn gridColumn41;

		private GridColumn gridColumn42;

		private GridColumn gridColumn43;

		private GridColumn gridColumn44;

		private GridColumn gridColumn45;

		private GridColumn gridColumn46;

		private GridColumn gridColumn47;

		private GridColumn gridColumn48;

		private GridColumn gridColumn49;

		private GridColumn gridColumn50;

		private System.Windows.Forms.ToolStripMenuItem PositionTradeByAccountTSMI;

		private System.Windows.Forms.ToolStripMenuItem TSMI_BeginListen;

		private System.Windows.Forms.ToolStripMenuItem CodeSetTSMI;

		private ComboBoxEdit comboBoxInstrument;

		private System.Windows.Forms.RadioButton radioButton1;

		private System.Windows.Forms.RadioButton radioButton2;

		private ComboBoxEdit comboBoxDirector;

		private ComboBoxEdit comboBoxOffset;

		private System.Windows.Forms.ImageList imageList1;

		private System.Windows.Forms.ToolStripMenuItem PositionCheckTSMI;

		private System.Windows.Forms.ToolStripMenuItem SubAccountSetTSMI;

		private System.Windows.Forms.TextBox textBox1;

		private System.Windows.Forms.Label label1;

		private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;

		private System.Windows.Forms.Label label2;

		private System.Windows.Forms.LinkLabel linkLabel1;

		private System.Windows.Forms.Button button4;

		private System.Windows.Forms.Button button3;

		private System.Windows.Forms.Button button2;

		private System.Windows.Forms.Button button1;

		private System.Windows.Forms.Button button5;

		private BandedGridColumn bandedGridColumn9;

		private BandedGridColumn bandedGridColumn12;

		private BandedGridColumn bandedGridColumn11;

		private System.Windows.Forms.ToolStripMenuItem 结算单ToolStripMenuItem;

		private BandedGridColumn bandedGridColumn13;

		private BandedGridColumn bandedGridColumn14;

		private GridControl xdgTrade;

		private GridView gvTrade;

		private GridColumn bandedGridColumn4;

		private GridColumn bandedGridColumn6;

		private GridColumn bandedGridColumn7;

		private GridColumn bandedGridColumn8;

		private GridColumn gridColumn1;

		private GridColumn gridColumn2;

		private GridColumn gridColumn3;

		private GridColumn gridColumn4;

		private GridColumn gridColumn5;

		private GridColumn gridColumn6;

		private GridColumn gridColumn7;

		private GridColumn gridColumn8;

		private GridColumn gridColumn9;

		private GridColumn gridColumn10;

		private GridColumn gridColumn11;

		private GridColumn bandedGridColumn15;

		private System.Windows.Forms.ToolStripMenuItem 导出ToolStripMenuItem;

		private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;

		private System.Windows.Forms.ToolStripMenuItem 导出数据ToolStripMenuItem;

		private System.Windows.Forms.ContextMenuStrip contextMenuStrip4;

		private System.Windows.Forms.ToolStripMenuItem 导出数据ToolStripMenuItem2;

		private System.Windows.Forms.ContextMenuStrip contextMenuStrip3;

		private System.Windows.Forms.ToolStripMenuItem 导出数据ToolStripMenuItem1;

		private BandedGridColumn bandedGridColumn16;

		private GridColumn gridColumn17;

		private BandedGridColumn bandedGridColumn10;

		private BandedGridColumn bandedGridColumn18;

		private BandedGridColumn bandedGridColumn19;

		private BandedGridColumn bandedGridColumn17;

		private BandedGridColumn bandedGridColumn22;

		private BandedGridColumn bandedGridColumn21;

		private BandedGridColumn bandedGridColumn20;

		private GridBand gridBand3;

		private GridBand gridBand4;

		private BandedGridColumn bandedGridColumn30;

		private BandedGridColumn bandedGridColumn29;

		private BandedGridColumn bandedGridColumn28;

		private BandedGridColumn bandedGridColumn27;

		private BandedGridColumn bandedGridColumn26;

		private BandedGridColumn bandedGridColumn25;

		private BandedGridColumn bandedGridColumn24;

		private BandedGridColumn bandedGridColumn23;

		private BandedGridColumn bandedGridColumn33;

		private BandedGridColumn bandedGridColumn32;

		private BandedGridColumn bandedGridColumn31;

		private BandedGridColumn bandedGridColumn34;

		private GridBand gridBand1;

		private GridBand gridBand2;

		private BandedGridColumn bandedGridColumn47;

		private BandedGridColumn bandedGridColumn46;

		private BandedGridColumn bandedGridColumn45;

		private BandedGridColumn bandedGridColumn44;

		private BandedGridColumn bandedGridColumn43;

		private BandedGridColumn bandedGridColumn42;

		private BandedGridColumn bandedGridColumn41;

		private BandedGridColumn bandedGridColumn40;

		private BandedGridColumn bandedGridColumn39;

		private BandedGridColumn bandedGridColumn38;

		private BandedGridColumn bandedGridColumn37;

		private BandedGridColumn bandedGridColumn36;

		private System.Windows.Forms.Button button6;

		private System.Windows.Forms.Button button7;

		public void printmsgout()
		{
			while (true)
			{
				if (this.listprintmsg.Count > 0)
				{
					try
					{
						lock (this)
						{
							string[] array = this.listprintmsg[0];
							string str = array[1];
							string text = System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss.fff") + "|" + str;
							this.logsw.WriteLine(text);
							if (array[0] == "1")
							{
							}
							base.BeginInvoke(new System.Action<string>(this.combprint), new object[]
							{
								text
							});
							this.listprintmsg.RemoveAt(0);
						}
					}
					catch (System.Exception ex)
					{
						base.BeginInvoke(new System.Action<string>(this.combprint), new object[]
						{
							"输出日志异常:" + ex.ToString()
						});
					}
					finally
					{
					}
				}
				else
				{
					System.Threading.Thread.Sleep(50);
				}
			}
		}

		public void combprint(string msg)
		{
			this.comboBoxErrMsg.Items.Insert(0, msg);
			this.comboBoxErrMsg.SelectedIndex = 0;
		}

		public frmMainFrame()
		{
			this.InitializeComponent();
			base.SetStyle(System.Windows.Forms.ControlStyles.ResizeRedraw | System.Windows.Forms.ControlStyles.AllPaintingInWmPaint | System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer, true);
		}

		public System.Collections.Generic.List<Brokers> GetBrokersXmlToList(string xmlpath)
		{
			System.Collections.Generic.List<Brokers> list = new System.Collections.Generic.List<Brokers>();
			System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(xmlpath);
			System.IO.FileInfo[] files = directoryInfo.GetFiles();
			for (int i = 0; i < files.Length; i++)
			{
				System.IO.FileInfo fileInfo = files[i];
				if (fileInfo.Extension == ".xml")
				{
					try
					{
						XmlDocument xmlDocument = new XmlDocument();
						xmlDocument.Load(fileInfo.FullName);
						XmlElement documentElement = xmlDocument.DocumentElement;
						System.Collections.IEnumerator enumerator = documentElement.ChildNodes.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								XmlNode xmlNode = (XmlNode)enumerator.Current;
								Brokers b = default(Brokers);
								b.BrokerName = xmlNode.Attributes["BrokerName"].Value;
								b.BrokerID = xmlNode.Attributes["BrokerID"].Value;
								b.Trading = "tcp://" + xmlNode.SelectSingleNode("./Servers//Trading").FirstChild.InnerText;
								b.MarketData = "tcp://" + xmlNode.SelectSingleNode("./Servers//MarketData").FirstChild.InnerText;
								if (!list.Exists((Brokers x) => x.BrokerID == b.BrokerID) && !list.Exists((Brokers x) => x.BrokerName == b.BrokerName))
								{
									list.Add(b);
								}
							}
						}
						finally
						{
							System.IDisposable disposable = enumerator as System.IDisposable;
							if (disposable != null)
							{
								disposable.Dispose();
							}
						}
					}
					catch (System.Exception var_7_1A8)
					{
					}
					finally
					{
					}
				}
			}
			return list;
		}

		private void frmMainFrame_Load(object sender, System.EventArgs e)
		{
			try
			{
				this.List_brokers = this.GetBrokersXmlToList(this.brokerspath);
				string path = System.Windows.Forms.Application.ExecutablePath.Substring(0, System.Windows.Forms.Application.ExecutablePath.LastIndexOf("\\")) + "\\" + System.DateTime.Today.ToString("yyyyMMdd") + "log.txt";
				this.logfs = new System.IO.FileStream(path, System.IO.FileMode.Append, System.Security.AccessControl.FileSystemRights.Write, System.IO.FileShare.ReadWrite, 1, System.IO.FileOptions.Asynchronous);
				this.logsw = new System.IO.StreamWriter(this.logfs);
				this.logsw.AutoFlush = true;
				if (this.th_print == null)
				{
					this.th_print = new System.Threading.Thread(delegate
					{
						this.printmsgout();
					});
					this.th_print.IsBackground = true;
					this.th_print.Start();
				}
				frmConfirm expr_E5 = this.fcf;
				expr_E5.sc = (frmConfirm.selectcomfirm)System.Delegate.Combine(expr_E5.sc, new frmConfirm.selectcomfirm(this.SelectConfirm));
				this.dtMarketData.Columns.Add("合约", string.Empty.GetType());
				this.dtMarketData.Columns.Add("名称", string.Empty.GetType());
				this.dtMarketData.Columns.Add("交易所", string.Empty.GetType());
				this.dtMarketData.Columns.Add("最新价", double.NaN.GetType());
				this.dtMarketData.Columns.Add("涨跌", double.NaN.GetType());
				this.dtMarketData.Columns.Add("涨幅", double.NaN.GetType());
				this.dtMarketData.Columns.Add("现手", -2147483648.GetType());
				this.dtMarketData.Columns.Add("总手", -2147483648.GetType());
				this.dtMarketData.Columns.Add("持仓", double.NaN.GetType());
				this.dtMarketData.Columns.Add("仓差", double.NaN.GetType());
				this.dtMarketData.Columns.Add("买价", double.NaN.GetType());
				this.dtMarketData.Columns.Add("买量", double.NaN.GetType());
				this.dtMarketData.Columns.Add("卖价", double.NaN.GetType());
				this.dtMarketData.Columns.Add("卖量", double.NaN.GetType());
				this.dtMarketData.Columns.Add("均价", double.NaN.GetType());
				this.dtMarketData.Columns.Add("最高", double.NaN.GetType());
				this.dtMarketData.Columns.Add("最低", double.NaN.GetType());
				this.dtMarketData.Columns.Add("涨停", double.NaN.GetType());
				this.dtMarketData.Columns.Add("跌停", double.NaN.GetType());
				this.dtMarketData.Columns.Add("开盘", double.NaN.GetType());
				this.dtMarketData.Columns.Add("昨结", double.NaN.GetType());
				this.dtMarketData.Columns.Add("时间", string.Empty.GetType());
				this.dtMarketData.Columns.Add("时间差", double.NaN.GetType());
				this.dtMarketData.Columns.Add("自选", true.GetType());
				this.dtMarketData.Columns.Add("套利", true.GetType());
				this.dtMarketData.Columns.Add("合约数量", -2147483648.GetType());
				this.dtMarketData.Columns.Add("最小波动", double.NaN.GetType());
				this.dtMarketData.Columns["自选"].DefaultValue = false;
				this.dtMarketData.Columns["套利"].DefaultValue = false;
				this.dtMarketData.PrimaryKey = new System.Data.DataColumn[]
				{
					this.dtMarketData.Columns["合约"]
				};
				this.dt_account_dg.Columns.Add("投资者帐户", string.Empty.GetType());
				this.dt_account_dg.Columns.Add("投资者", string.Empty.GetType());
				this.dt_account_dg.Columns.Add("登录状态", string.Empty.GetType());
				this.dt_account_dg.Columns.Add("动态权益", double.NaN.GetType());
				this.dt_account_dg.Columns.Add("IF持仓", string.Empty.GetType());
				this.dt_account_dg.Columns.Add("IC持仓", string.Empty.GetType());
				this.dt_account_dg.Columns.Add("IH持仓", string.Empty.GetType());
				this.dt_account_dg.Columns.Add("au持仓", string.Empty.GetType());
				this.dt_account_dg.Columns.Add("ag持仓", string.Empty.GetType());
				this.dt_account_dg.Columns.Add("cu持仓", string.Empty.GetType());
				this.dt_account_dg.Columns.Add("ru持仓", string.Empty.GetType());
				this.dt_account_dg.Columns.Add("ni持仓", string.Empty.GetType());
				this.dt_account_dg.Columns.Add("rb持仓", string.Empty.GetType());
				this.dt_account_dg.Columns.Add("i持仓", string.Empty.GetType());
				this.dt_account_dg.Columns.Add("y持仓", string.Empty.GetType());
				this.dt_account_dg.Columns.Add("m持仓", string.Empty.GetType());
				this.dt_account_dg.Columns.Add("pp持仓", string.Empty.GetType());
				this.dt_account_dg.Columns.Add("l持仓", string.Empty.GetType());
				this.dt_account_dg.Columns.Add("MA持仓", string.Empty.GetType());
				this.dt_account_dg.Columns.Add("RM持仓", string.Empty.GetType());
				this.dt_account_dg.Columns.Add("SR持仓", string.Empty.GetType());
				this.dt_account_dg.Columns.Add("上次结算", double.NaN.GetType());
				this.dt_account_dg.Columns.Add("冻结资金", double.NaN.GetType());
				this.dt_account_dg.Columns.Add("占用保证金", double.NaN.GetType());
				this.dt_account_dg.Columns.Add("可用资金", double.NaN.GetType());
				this.dt_account_dg.Columns.Add("平仓盈亏", double.NaN.GetType());
				this.dt_account_dg.Columns.Add("持仓盈亏", double.NaN.GetType());
				this.dt_account_dg.Columns.Add("手续费", double.NaN.GetType());
				this.dt_account_dg.Columns.Add("入金", double.NaN.GetType());
				this.dt_account_dg.Columns.Add("出金", double.NaN.GetType());
				this.dt_account_dg.Columns.Add("冻结保证金", double.NaN.GetType());
				this.dt_account_dg.Columns.Add("选择", System.Type.GetType("System.Boolean"));
				this.dt_mainaccount_dg.Columns.Add("投资者帐户", string.Empty.GetType());
				this.dt_mainaccount_dg.Columns.Add("投资者", string.Empty.GetType());
				this.dt_mainaccount_dg.Columns.Add("登录状态", string.Empty.GetType());
				this.dt_mainaccount_dg.Columns.Add("IF持仓", string.Empty.GetType());
				this.dt_mainaccount_dg.Columns.Add("IC持仓", string.Empty.GetType());
				this.dt_mainaccount_dg.Columns.Add("IH持仓", string.Empty.GetType());
				this.dt_mainaccount_dg.Columns.Add("au持仓", string.Empty.GetType());
				this.dt_mainaccount_dg.Columns.Add("ag持仓", string.Empty.GetType());
				this.dt_mainaccount_dg.Columns.Add("cu持仓", string.Empty.GetType());
				this.dt_mainaccount_dg.Columns.Add("ru持仓", string.Empty.GetType());
				this.dt_mainaccount_dg.Columns.Add("ni持仓", string.Empty.GetType());
				this.dt_mainaccount_dg.Columns.Add("rb持仓", string.Empty.GetType());
				this.dt_mainaccount_dg.Columns.Add("i持仓", string.Empty.GetType());
				this.dt_mainaccount_dg.Columns.Add("y持仓", string.Empty.GetType());
				this.dt_mainaccount_dg.Columns.Add("m持仓", string.Empty.GetType());
				this.dt_mainaccount_dg.Columns.Add("pp持仓", string.Empty.GetType());
				this.dt_mainaccount_dg.Columns.Add("l持仓", string.Empty.GetType());
				this.dt_mainaccount_dg.Columns.Add("MA持仓", string.Empty.GetType());
				this.dt_mainaccount_dg.Columns.Add("RM持仓", string.Empty.GetType());
				this.dt_mainaccount_dg.Columns.Add("SR持仓", string.Empty.GetType());
				this.dt_mainaccount_dg.Columns.Add("动态权益", double.NaN.GetType());
				this.dt_mainaccount_dg.Columns.Add("上次结算", double.NaN.GetType());
				this.dt_mainaccount_dg.Columns.Add("冻结资金", double.NaN.GetType());
				this.dt_mainaccount_dg.Columns.Add("占用保证金", double.NaN.GetType());
				this.dt_mainaccount_dg.Columns.Add("可用资金", double.NaN.GetType());
				this.dt_mainaccount_dg.Columns.Add("平仓盈亏", double.NaN.GetType());
				this.dt_mainaccount_dg.Columns.Add("持仓盈亏", double.NaN.GetType());
				this.dt_mainaccount_dg.Columns.Add("手续费", double.NaN.GetType());
				this.dt_mainaccount_dg.Columns.Add("入金", double.NaN.GetType());
				this.dt_mainaccount_dg.Columns.Add("出金", double.NaN.GetType());
				this.dt_mainaccount_dg.Columns.Add("冻结保证金", double.NaN.GetType());
				this.dt_mainaccount_dg.Columns.Add("选择", System.Type.GetType("System.Boolean"));
				this.dt_AllTrade.Columns.Add("投资者", string.Empty.GetType());
				this.dt_AllTrade.Columns.Add("编号", string.Empty.GetType());
				this.dt_AllTrade.Columns.Add("合约", string.Empty.GetType());
				this.dt_AllTrade.Columns.Add("买卖", string.Empty.GetType());
				this.dt_AllTrade.Columns.Add("开平", string.Empty.GetType());
				this.dt_AllTrade.Columns.Add("状态", string.Empty.GetType());
				this.dt_AllTrade.Columns.Add("价格", string.Empty.GetType());
				this.dt_AllTrade.Columns.Add("报单手数", System.Type.GetType("System.Int32"));
				this.dt_AllTrade.Columns.Add("未成交", System.Type.GetType("System.Int32"));
				this.dt_AllTrade.Columns.Add("成交手数", System.Type.GetType("System.Int32"));
				this.dt_AllTrade.Columns.Add("报单时间", System.Type.GetType("System.DateTime"));
				this.dt_AllTrade.Columns.Add("成交时间", string.Empty.GetType());
				this.dt_AllTrade.Columns.Add("成交均价", string.Empty.GetType());
				this.dt_AllTrade.Columns.Add("详细状态", string.Empty.GetType());
				this.dt_AllTrade.Columns.Add("客户信息", string.Empty.GetType());
				this.dt_AllTrade.Columns.Add("序列号", string.Empty.GetType());
				this.dt_AllTrade.Columns.Add("前置编号", string.Empty.GetType());
				this.dt_AllTrade.Columns.Add("会话编号", string.Empty.GetType());
				this.dt_MainTrade.Columns.Add("投资者", string.Empty.GetType());
				this.dt_MainTrade.Columns.Add("编号", string.Empty.GetType());
				this.dt_MainTrade.Columns.Add("合约", string.Empty.GetType());
				this.dt_MainTrade.Columns.Add("买卖", string.Empty.GetType());
				this.dt_MainTrade.Columns.Add("开平", string.Empty.GetType());
				this.dt_MainTrade.Columns.Add("状态", string.Empty.GetType());
				this.dt_MainTrade.Columns.Add("价格", string.Empty.GetType());
				this.dt_MainTrade.Columns.Add("报单手数", System.Type.GetType("System.Int32"));
				this.dt_MainTrade.Columns.Add("未成交", System.Type.GetType("System.Int32"));
				this.dt_MainTrade.Columns.Add("成交手数", System.Type.GetType("System.Int32"));
				this.dt_MainTrade.Columns.Add("报单时间", System.Type.GetType("System.DateTime"));
				this.dt_MainTrade.Columns.Add("成交时间", string.Empty.GetType());
				this.dt_MainTrade.Columns.Add("成交均价", string.Empty.GetType());
				this.dt_MainTrade.Columns.Add("详细状态", string.Empty.GetType());
				this.dt_MainTrade.Columns.Add("客户信息", string.Empty.GetType());
				this.dt_MainTrade.Columns.Add("序列号", string.Empty.GetType());
				this.dt_MainTrade.Columns.Add("前置编号", string.Empty.GetType());
				this.dt_MainTrade.Columns.Add("会话编号", string.Empty.GetType());
				this.dt_AllDeal.Columns.Add("投资者", string.Empty.GetType());
				this.dt_AllDeal.Columns.Add("编号", string.Empty.GetType());
				this.dt_AllDeal.Columns.Add("合约", string.Empty.GetType());
				this.dt_AllDeal.Columns.Add("买卖", string.Empty.GetType());
				this.dt_AllDeal.Columns.Add("开平", string.Empty.GetType());
				this.dt_AllDeal.Columns.Add("成交价格", string.Empty.GetType());
				this.dt_AllDeal.Columns.Add("成交手数", System.Type.GetType("System.Int32"));
				this.dt_AllDeal.Columns.Add("成交时间", string.Empty.GetType());
				this.dt_AllDeal.Columns.Add("报单编号", string.Empty.GetType());
				this.dt_AllDeal.Columns.Add("成交类型", string.Empty.GetType());
				this.dt_AllDeal.Columns.Add("交易所", string.Empty.GetType());
				this.dt_AllDeal.Columns.Add("滑点", string.Empty.GetType());
				this.dt_MainDeal.Columns.Add("投资者", string.Empty.GetType());
				this.dt_MainDeal.Columns.Add("编号", string.Empty.GetType());
				this.dt_MainDeal.Columns.Add("合约", string.Empty.GetType());
				this.dt_MainDeal.Columns.Add("买卖", string.Empty.GetType());
				this.dt_MainDeal.Columns.Add("开平", string.Empty.GetType());
				this.dt_MainDeal.Columns.Add("成交价格", string.Empty.GetType());
				this.dt_MainDeal.Columns.Add("成交手数", System.Type.GetType("System.Int32"));
				this.dt_MainDeal.Columns.Add("成交时间", string.Empty.GetType());
				this.dt_MainDeal.Columns.Add("报单编号", string.Empty.GetType());
				this.dt_MainDeal.Columns.Add("成交类型", string.Empty.GetType());
				this.dt_MainDeal.Columns.Add("交易所", string.Empty.GetType());
				this.dt_AccountTradeSet.Columns.Add("子帐户", string.Empty.GetType());
				this.dt_AccountTradeSet.Columns.Add("品种", string.Empty.GetType());
				this.dt_AccountTradeSet.Columns.Add("主帐户", string.Empty.GetType());
				this.dt_AccountTradeSet.Columns.Add("手数倍率", string.Empty.GetType());
				this.dt_AccountTradeSet.Columns.Add("价格", string.Empty.GetType());
				this.dt_AccountTradeSet.Columns.Add("延时", string.Empty.GetType());
				this.dt_AccountTradeSet.Columns.Add("撤单等待", string.Empty.GetType());
				this.dt_AccountTradeSet.Columns.Add("最大交易量", string.Empty.GetType());
				this.dt_AccountTradeSet.Columns.Add("多头仓差", string.Empty.GetType());
				this.dt_AccountTradeSet.Columns.Add("空头仓差", string.Empty.GetType());
				this.dt_AccountTradeSet.Columns.Add("是否反向", System.Type.GetType("System.Boolean"));
				this.dt_AccountTradeSet.Columns.Add("优先", System.Type.GetType("System.Int32"));
				this.dt_AccountTradeSet.Columns.Add("开多让点", string.Empty.GetType());
				this.dt_AccountTradeSet.Columns.Add("平多让点", string.Empty.GetType());
				this.dt_AccountTradeSet.Columns.Add("开空让点", string.Empty.GetType());
				this.dt_AccountTradeSet.Columns.Add("平空让点", string.Empty.GetType());
				this.dt_CodeSet.Columns.Add("品种", string.Empty.GetType());
				this.dt_CodeSet.Columns.Add("快捷键代码", string.Empty.GetType());
				this.dt_CodeSet.Columns.Add("keys", string.Empty.GetType());
				this.dt_SubAccountSet.Columns.Add("子帐户", string.Empty.GetType());
				this.dt_SubAccountSet.Columns.Add("是否监听", System.Type.GetType("System.Boolean"));
				this.dt_SubAccountSet.Columns.Add("是否校正", System.Type.GetType("System.Boolean"));
				this.readttod.readtxt(ref this.dt_AccountTradeSet, "orderset.txt", 0);
				this.readttod.readtxt(ref this.dt_CodeSet, "codeset.txt", 1);
				this.readttod.readtxt(ref this.dt_SubAccountSet, "SubAccountset.txt", 2);
				this.textBox1.Text = this.ini.ReadValue("设置", "延时时间");
				this.prooftime = ((this.textBox1.Text.Trim() != "") ? System.Convert.ToSingle(this.textBox1.Text.Trim()) : 1f);
				this.xdgTrade.DataSource = this.dt_AllTrade;
				this.xdgDeal.DataSource = this.dt_AllDeal;
				this.xdgMianTrade.DataSource = this.dt_MainTrade;
				this.xdgMainDeal.DataSource = this.dt_MainDeal;
				this.xdgAccount.DataSource = this.dt_account_dg;
				this.xdgMainAccount.DataSource = this.dt_mainaccount_dg;
				System.Data.DataTable dtAccount = this.SelectSQLite();
				bool flag = this.CreateFrm(dtAccount, this.dt_SubAccountSet);
				this.th_tread = new System.Threading.Thread[this.dt_account_dg.Rows.Count];
				this.th_sepCheck = new System.Threading.Thread[this.dt_account_dg.Rows.Count];
				this.th_colorCheck = new System.Threading.Thread[this.dt_account_dg.Rows.Count];
				for (int i = 0; i < this.dt_account_dg.Rows.Count; i++)
				{
					int num = i;
					string id = this.dt_account_dg.Rows[num]["投资者帐户"].ToString();
					this.th_tread[num] = new System.Threading.Thread(delegate
					{
						this.tmTrade(id);
					});
					this.th_tread[num].IsBackground = true;
					this.th_tread[num].Name = "tmtrade" + id;
					this.th_tread[num].Start();
					this.th_sepCheck[num] = new System.Threading.Thread(delegate
					{
						this.SeparateaccountProof(id);
					});
					this.th_sepCheck[num].IsBackground = true;
					this.th_sepCheck[num].Name = "specheck" + id;
					this.th_sepCheck[num].Start();
					this.th_colorCheck[num] = new System.Threading.Thread(delegate
					{
						this.PositionColorCheck(id);
					});
					this.th_colorCheck[num].IsBackground = true;
					this.th_colorCheck[num].Name = "colorcheck" + id;
					this.th_colorCheck[num].Start();
				}
				string text = this.ini.ReadValue("设置", "跟单方式");
				this.radioButton2.Checked = true;
				this.comboBoxDirector.SelectedIndex = 0;
				this.comboBoxOffset.SelectedIndex = 0;
				if (!System.IO.File.Exists(this.xmlMainTrade))
				{
					this.xdgMianTrade.MainView.SaveLayoutToXml(this.xmlMainTrade);
				}
				this.xdgMianTrade.ForceInitialize();
				this.xdgMianTrade.MainView.RestoreLayoutFromXml(this.xmlMainTrade);
				if (!System.IO.File.Exists(this.xmlTrade))
				{
					this.xdgTrade.MainView.SaveLayoutToXml(this.xmlTrade);
				}
				this.xdgTrade.ForceInitialize();
				this.xdgTrade.MainView.RestoreLayoutFromXml(this.xmlTrade);
				if (!System.IO.File.Exists(this.xmlMainDeal))
				{
					this.xdgMainDeal.MainView.SaveLayoutToXml(this.xmlMainDeal);
				}
				this.xdgMainDeal.ForceInitialize();
				this.xdgMainDeal.MainView.RestoreLayoutFromXml(this.xmlMainDeal);
				if (!System.IO.File.Exists(this.xmlDeal))
				{
					this.xdgDeal.MainView.SaveLayoutToXml(this.xmlDeal);
				}
				this.xdgDeal.ForceInitialize();
				this.xdgDeal.MainView.RestoreLayoutFromXml(this.xmlDeal);
				this.th_changcode = new System.Threading.Thread(delegate
				{
					this.ChangeInstrument();
				});
				this.th_changcode.IsBackground = true;
				this.th_changcode.Start();
				System.Threading.Thread thread = new System.Threading.Thread(delegate
				{
					while (true)
					{
						UserKey userKey = new UserKey();
						if (!userKey.Key(1))
						{
							if (this.roundwrong >= 10)
							{
								break;
							}
							this.roundwrong++;
						}
						System.Threading.Thread.Sleep(60000);
					}
					base.BeginInvoke(new Action(delegate
					{
						this.timeUpClose();
					}));
				});
			}
			catch (System.Exception var_8_1D21)
			{
			}
		}

		private void UpdateIFPosition(string id, bool Ismain, string code)
		{
			try
			{
				int length = 0;
				for (int i = 0; i < code.Length; i++)
				{
					if (char.IsNumber(code, i))
					{
						length = i;
						break;
					}
				}
				string text = code.Substring(0, length);
				this.ds_GroupPosition.Tables[id].CaseSensitive = true;
				object obj = this.ds_GroupPosition.Tables[id].Compute("sum(总持仓)", "(合约 like '" + text + "%') and 买卖='买'");
				object obj2 = this.ds_GroupPosition.Tables[id].Compute("sum(总持仓)", "(合约 like '" + text + "%') and 买卖<>'买'");
				string columnName = text + "持仓";
				System.Data.DataRow dataRow;
				if (Ismain)
				{
					dataRow = this.dt_mainaccount_dg.Select("投资者帐户='" + id + "'").FirstOrDefault<System.Data.DataRow>();
				}
				else
				{
					dataRow = this.dt_account_dg.Select("投资者帐户='" + id + "'").FirstOrDefault<System.Data.DataRow>();
				}
				if (dataRow != null)
				{
					dataRow[columnName] = ((obj == System.DBNull.Value) ? 0 : obj).ToString() + "/" + ((obj2 == System.DBNull.Value) ? 0 : obj2).ToString();
				}
			}
			catch (System.Exception ex)
			{
				this.listprintmsg.Add(new string[]
				{
					"1",
					"持仓汇总" + ex.Message
				});
			}
			finally
			{
			}
		}

		private void timeUpClose()
		{
			this.TSMI_BeginListen.BackColor = System.Drawing.Color.Transparent;
			this.BeginListen = false;
			this.TSMI_BeginListen.Text = "启动监听";
			this.spcMain.Enabled = false;
			this.menuStrip1.Enabled = false;
			System.Windows.Forms.MessageBox.Show("注册信息有问题，请联系授权者");
		}

		private System.Data.DataTable SelectSQLite()
		{
			System.Data.DataTable dataTable = new System.Data.DataTable();
			this.connstr.DataSource = this.dbpath;
			this.connstr.Password = this.gid.getRNum();
			try
			{
				using (SQLiteConnection sQLiteConnection = new SQLiteConnection(this.connstr.ToString()))
				{
					sQLiteConnection.Open();
					using (SQLiteCommand sQLiteCommand = new SQLiteCommand(sQLiteConnection))
					{
						sQLiteCommand.CommandText = "SELECT count(*) FROM sqlite_master where type='table' and name='user'";
						int num = System.Convert.ToInt32(sQLiteCommand.ExecuteScalar());
						if (num == 0)
						{
							sQLiteCommand.CommandText = "CREATE TABLE user(id integer NOT NULL PRIMARY KEY AUTOINCREMENT,account  text,accountname text,\r\n                            password text,brokers text,type text,mainaccount bool,choose bool);";
							sQLiteCommand.ExecuteNonQuery();
						}
						else
						{
							sQLiteCommand.CommandText = "SELECT * FROM 'user'";
							using (SQLiteDataAdapter sQLiteDataAdapter = new SQLiteDataAdapter(sQLiteCommand))
							{
								sQLiteDataAdapter.Fill(dataTable);
							}
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				if (System.Windows.Forms.MessageBox.Show(ex.StackTrace + ex.Message) == System.Windows.Forms.DialogResult.OK)
				{
					System.Environment.Exit(System.Environment.ExitCode);
				}
			}
			return dataTable;
		}

		private bool CreateFrm(System.Data.DataTable dtAccount, System.Data.DataTable dtSubAccount)
		{
			bool result;
			try
			{
				System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
				string path = System.Windows.Forms.Application.ExecutablePath.Substring(0, System.Windows.Forms.Application.ExecutablePath.LastIndexOf("\\")) + "\\knAddr.dat";
				System.IO.FileStream fileStream = new System.IO.FileStream(path, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite, System.IO.FileShare.ReadWrite);
				System.IO.StreamReader streamReader = new System.IO.StreamReader(fileStream);
				list = streamReader.ReadToEnd().Split(new string[]
				{
					"\r\n"
				}, System.StringSplitOptions.RemoveEmptyEntries).ToList<string>();
				System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(fileStream);
				streamWriter.AutoFlush = true;
				int i;
				for (i = 0; i < dtAccount.Rows.Count; i++)
				{
					string text = dtAccount.Rows[i]["account"].ToString();
					string a = dtAccount.Rows[i]["mainaccount"].ToString();
					this.frmp = new FrmPosition();
					this.frmp.Name = "Position" + text;
					this.frmp.Text = text;
					this.frmp.backform = new FrmPosition.closeform(this.returnform);
					this.frmp.clv = new FrmPosition.clicklistview(this.positionclick);
					this.frmp.TopLevel = false;
					this.frmp.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
					this.frmp.Dock = System.Windows.Forms.DockStyle.Fill;
					this.frmp.Show();
					this.ds_GroupPosition.Tables.Add(this.CreatGroupPositionDT(text));
					this.ds_GroupPositionDetail.Tables.Add(this.CreatGroupDetailDT(text));
					if (this.List_brokers.Exists((Brokers x) => x.BrokerName == dtAccount.Rows[i]["brokers"].ToString()))
					{
						Brokers brokers = (from x in this.List_brokers
						where x.BrokerName == dtAccount.Rows[i]["brokers"].ToString()
						select x).FirstOrDefault<Brokers>();
						if (dtAccount.Rows[i]["type"].ToString() == "金牛")
						{
							if (!list.Contains(brokers.Trading))
							{
								list.Add(brokers.Trading);
								streamWriter.WriteLine(brokers.Trading);
							}
							if (!list.Contains(brokers.MarketData))
							{
								list.Add(brokers.MarketData);
								streamWriter.WriteLine(brokers.MarketData);
							}
						}
						if (a != "True")
						{
							System.Data.DataRow dataRow = dtSubAccount.Select("子帐户='" + text + "'").FirstOrDefault<System.Data.DataRow>();
							this.tab.TabPages.Add(text, text, "");
							this.tab.TabPages[text].Controls.Add(this.frmp);
							this.dt_account_dg.Rows.Add(new object[]
							{
								text,
								dtAccount.Rows[i]["accountname"].ToString(),
								"未登录",
								0,
								"0/0",
								"0/0",
								"0/0",
								"0/0",
								"0/0",
								"0/0",
								"0/0",
								"0/0",
								"0/0",
								"0/0",
								"0/0",
								"0/0",
								"0/0",
								"0/0",
								"0/0",
								"0/0",
								"0/0",
								0,
								0,
								0,
								0,
								0,
								0,
								0,
								0,
								0,
								0,
								bool.FalseString
							});
							this.Dic_TradeApi.Add(text, new TradeApi((dtAccount.Rows[i]["type"].ToString() == "鑫管家") ? "xgj_ctp_trade_proxy.dll" : "ctp_trade_proxy.dll", text, dtAccount.Rows[i]["password"].ToString(), brokers.BrokerID, brokers.Trading, brokers.MarketData, "子帐户", dataRow != null && System.Convert.ToBoolean(dataRow["是否监听"].ToString()), dataRow != null && System.Convert.ToBoolean(dataRow["是否校正"].ToString()), dtAccount.Rows[i]["type"].ToString()));
							this.Dic_SeparateTrade.TryAdd(text, new System.Collections.Generic.List<MSOrderMode>());
							this.Dic_SeparateCheck.Add(text, new System.Collections.Generic.List<string[]>());
							this.dlchecktrans.Add(text, new System.Collections.Generic.List<string[]>());
							this.Dic_ColorCheck.Add(text, new System.Collections.Generic.List<string[]>());
						}
						else
						{
							this.tabMain.TabPages.Add(text, text, "");
							this.tabMain.TabPages[text].Controls.Add(this.frmp);
							this.dt_mainaccount_dg.Rows.Add(new object[]
							{
								text,
								dtAccount.Rows[i]["accountname"].ToString(),
								"未登录",
								"0/0",
								"0/0",
								"0/0",
								"0/0",
								"0/0",
								"0/0",
								"0/0",
								"0/0",
								"0/0",
								"0/0",
								"0/0",
								"0/0",
								"0/0",
								"0/0",
								"0/0",
								"0/0",
								"0/0",
								0,
								0,
								0,
								0,
								0,
								0,
								0,
								0,
								0,
								0,
								0,
								bool.FalseString
							});
							this.Dic_TradeApi.Add(text, new TradeApi((dtAccount.Rows[i]["type"].ToString() == "鑫管家") ? "xgj_ctp_trade_proxy.dll" : "ctp_trade_proxy.dll", text, dtAccount.Rows[i]["password"].ToString(), brokers.BrokerID, brokers.Trading, brokers.MarketData, "主帐户", false, false, dtAccount.Rows[i]["type"].ToString()));
							this.Dic_MainTrade.Add(text, new System.Collections.Generic.List<string[]>());
							this.Dic_MainCheck.Add(text, new System.Collections.Generic.List<string[]>());
						}
						GridControl gridControl = this.frmp.Controls["xdgPosition"] as GridControl;
						gridControl.DataSource = this.ds_GroupPosition.Tables[text];
						GridControl gridControl2 = this.frmp.Controls["xdgPositionDetail"] as GridControl;
						gridControl2.DataSource = this.ds_GroupPositionDetail.Tables[text];
						string text2 = string.Concat(new string[]
						{
							System.Windows.Forms.Application.ExecutablePath.Substring(0, System.Windows.Forms.Application.ExecutablePath.LastIndexOf("\\")),
							"\\DLL\\",
							text,
							"_",
							brokers.BrokerID.ToString(),
							"\\XG_Position.xml"
						});
						string text3 = string.Concat(new string[]
						{
							System.Windows.Forms.Application.ExecutablePath.Substring(0, System.Windows.Forms.Application.ExecutablePath.LastIndexOf("\\")),
							"\\DLL\\",
							text,
							"_",
							brokers.BrokerID.ToString(),
							"\\XG_PositionDetail.xml"
						});
						if (!System.IO.File.Exists(text2))
						{
							gridControl.MainView.SaveLayoutToXml(text2);
						}
						gridControl.ForceInitialize();
						gridControl.MainView.RestoreLayoutFromXml(text2);
						if (!System.IO.File.Exists(text3))
						{
							gridControl2.MainView.SaveLayoutToXml(text3);
						}
						gridControl2.ForceInitialize();
						gridControl2.MainView.RestoreLayoutFromXml(text3);
					}
				}
				streamWriter.Close();
				fileStream.Close();
				result = true;
			}
			catch (System.Exception ex)
			{
				System.Windows.Forms.MessageBox.Show(ex.StackTrace);
				result = false;
			}
			return result;
		}

		private System.Data.DataTable CreatGroupPositionDT(string DtGroupName)
		{
			return new System.Data.DataTable(DtGroupName)
			{
				Columns = 
				{
					{
						"合约",
						string.Empty.GetType()
					},
					{
						"买卖",
						string.Empty.GetType()
					},
					{
						"总持仓",
						System.Type.GetType("System.Int32")
					},
					{
						"昨仓",
						System.Type.GetType("System.Int32")
					},
					{
						"今仓",
						System.Type.GetType("System.Int32")
					},
					{
						"可平昨",
						System.Type.GetType("System.Int32")
					},
					{
						"可平今",
						System.Type.GetType("System.Int32")
					},
					{
						"持仓均价",
						double.NaN.GetType()
					},
					{
						"开仓均价",
						double.NaN.GetType()
					},
					{
						"持仓盈亏",
						double.NaN.GetType()
					},
					{
						"多头占用保证金",
						double.NaN.GetType()
					},
					{
						"空头占用保证金",
						double.NaN.GetType()
					},
					{
						"投保",
						string.Empty.GetType()
					},
					{
						"校对",
						string.Empty.GetType()
					}
				}
			};
		}

		private System.Data.DataTable CreatGroupDetailDT(string DtGroupName)
		{
			return new System.Data.DataTable(DtGroupName)
			{
				Columns = 
				{
					{
						"编号",
						System.Type.GetType("System.Int64")
					},
					{
						"合约",
						string.Empty.GetType()
					},
					{
						"买卖",
						string.Empty.GetType()
					},
					{
						"持仓类型",
						string.Empty.GetType()
					},
					{
						"成交手数",
						System.Type.GetType("System.Int32")
					},
					{
						"成交价格",
						double.NaN.GetType()
					},
					{
						"成交时间",
						System.Type.GetType("System.Int32")
					}
				}
			};
		}

		private void frmMainFrame_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
		{
			try
			{
				if (System.Windows.Forms.DialogResult.Yes == System.Windows.Forms.MessageBox.Show("确定要关闭该软件？", "关闭确认", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question))
				{
					this.CloseFrm();
				}
				else
				{
					e.Cancel = true;
				}
			}
			catch
			{
			}
		}

		private void CloseFrm()
		{
			try
			{
				this.xdgMianTrade.MainView.SaveLayoutToXml(this.xmlMainTrade);
				this.xdgTrade.MainView.SaveLayoutToXml(this.xmlTrade);
				this.xdgMainDeal.MainView.SaveLayoutToXml(this.xmlMainDeal);
				this.xdgDeal.MainView.SaveLayoutToXml(this.xmlDeal);
				for (int i = 0; i < this.tab.TabPages.Count; i++)
				{
					string brokerID = this.Dic_TradeApi[this.tab.TabPages[i].Text].BrokerID;
					string text = this.tab.TabPages[i].Text;
					GridControl gridControl = this.tab.TabPages[i].Controls["Position" + text].Controls["xdgPosition"] as GridControl;
					GridControl gridControl2 = this.tab.TabPages[i].Controls["Position" + text].Controls["xdgPositionDetail"] as GridControl;
					string xmlFile = string.Concat(new string[]
					{
						System.Windows.Forms.Application.ExecutablePath.Substring(0, System.Windows.Forms.Application.ExecutablePath.LastIndexOf("\\")),
						"\\DLL\\",
						text,
						"_",
						brokerID,
						"\\XG_Position.xml"
					});
					string xmlFile2 = string.Concat(new string[]
					{
						System.Windows.Forms.Application.ExecutablePath.Substring(0, System.Windows.Forms.Application.ExecutablePath.LastIndexOf("\\")),
						"\\DLL\\",
						text,
						"_",
						brokerID,
						"\\XG_PositionDetail.xml"
					});
					gridControl.MainView.SaveLayoutToXml(xmlFile);
					gridControl2.MainView.SaveLayoutToXml(xmlFile2);
				}
				for (int i = 0; i < this.dt_account_dg.Rows.Count; i++)
				{
					string key = this.dt_account_dg.Rows[i]["投资者帐户"].ToString();
					if (this.dt_account_dg.Rows[i]["登录状态"].ToString() != "未登录" || this.Dic_TradeApi[key].IsLogin || this.Dic_TradeApi[key].Isconned)
					{
						try
						{
							this.Dic_TradeApi[key].ReqDisConnect();
						}
						catch (System.Exception ex)
						{
							System.Windows.Forms.MessageBox.Show(ex.StackTrace + ex.Message);
						}
					}
				}
				if (this.mdApi != null)
				{
					this.mdApi.DisConnect();
				}
				if (this.threadFreshMarketData != null)
				{
					this.threadFreshMarketData.Abort();
				}
				this.logfs.Close();
				Task task = new Task(delegate
				{
					System.Threading.Thread.Sleep(3000);
					System.Environment.Exit(0);
					base.Dispose();
				});
				task.Start();
			}
			catch (System.Exception ex)
			{
			}
			finally
			{
			}
		}

		private void updateaccount(System.Data.DataRow dr, double openprice, double transprice, int vol, int unit, string direction, double charge)
		{
			double num;
			if (direction == "Long")
			{
				num = (transprice - openprice) * (double)vol * (double)unit;
			}
			else
			{
				num = (openprice - transprice) * (double)vol * (double)unit;
			}
			dr["动态权益"] = (double.Parse(dr["动态权益"].ToString()) - charge + num).ToString("F0");
			dr["平仓盈亏"] = (double.Parse(dr["平仓盈亏"].ToString()) + num).ToString("F0");
			dr["手续费"] = (double.Parse(dr["手续费"].ToString()) + charge).ToString("F0");
		}

		private void SeparateaccountProof(string subID)
		{
			while (true)
			{
				if (this.Dic_SeparateCheck[subID].Count > 0)
				{
					for (int i = this.Dic_SeparateCheck[subID].Count - 1; i >= 0; i--)
					{
						string[] msg = this.Dic_SeparateCheck[subID][i];
						if (msg[6] == "自动")
						{
							System.Threading.Monitor.Enter(this.AddLock);
						}
						if (msg != null && System.DateTime.Now >= System.Convert.ToDateTime(msg[4]))
						{
							try
							{
								int num = 0;
								this.Dic_SeparateCheck[subID].RemoveAt(i);
								if (msg[6] == "自动")
								{
									Debug.WriteLine(string.Concat(new string[]
									{
										subID,
										",",
										msg[1],
										",",
										msg[4],
										",",
										msg[7]
									}));
								}
								if (this.Dic_TradeApi[subID].IsLogin && this.Dic_TradeApi[subID].SubProofbegin)
								{
									bool flag = false;
									if (msg[6] == "手动")
									{
										num = 1;
									}
									if (msg[6] == "颜色校正")
									{
										num = 2;
									}
									System.Data.DataRow dataRow = this.dtMarketData.Rows.Find(msg[1]);
									int num2 = 0;
									System.Data.DataRow[] array = this.dt_AccountTradeSet.Select(string.Concat(new string[]
									{
										"(品种='",
										msg[1],
										"'or 品种='",
										msg[5],
										"'or 品种='*') and 子帐户='",
										subID,
										"'"
									}), "优先 asc");
									if (array.Length > 0)
									{
										int num3 = System.Convert.ToInt32(msg[3]);
										for (int j = 0; j < array.Length; j++)
										{
											bool isReverse = !(array[j]["是否反向"].ToString() == "") && (bool)array[j]["是否反向"];
											int num4 = 0;
											if (array[j]["多头仓差"].ToString() != "/" && array[j]["空头仓差"].ToString() != "/")
											{
												flag = true;
												this.checkmainaccount(array[j]["主帐户"].ToString(), msg[1], msg[2], msg[7], isReverse, ref num4);
												num4 *= ((array[j]["手数倍率"].ToString() == "/") ? 1 : System.Convert.ToInt32(array[j]["手数倍率"]));
												if (msg[2] == "买")
												{
													num4 -= System.Convert.ToInt32(array[j]["多头仓差"]);
												}
												else
												{
													num4 -= System.Convert.ToInt32(array[j]["空头仓差"]);
												}
												num3 += num4;
											}
										}
										if (flag)
										{
											bool flag2 = false;
											System.Data.DataRow dataRow2 = this.dt_AccountTradeSet.Select(string.Concat(new string[]
											{
												"(品种='",
												msg[1],
												"'or 品种='",
												msg[5],
												"'or 品种='*') and 子帐户='",
												subID,
												"' and 主帐户='",
												msg[0],
												"'"
											})).FirstOrDefault<System.Data.DataRow>();
											if (dataRow2 != null)
											{
												flag2 = (!(dataRow2["是否反向"].ToString() == "") && (bool)dataRow2["是否反向"]);
											}
											if (flag2)
											{
												msg[2] = ((msg[2] == "    卖") ? "买" : "    卖");
											}
											System.Data.DataRow dataRow3 = this.ds_GroupPosition.Tables[subID].Select(string.Concat(new string[]
											{
												"合约='",
												msg[1],
												"' and 买卖='",
												msg[2],
												"'"
											})).FirstOrDefault<System.Data.DataRow>();
											if (dataRow3 != null)
											{
												num2 = num3 - System.Convert.ToInt32(dataRow3["总持仓"].ToString());
											}
											else if (num3 != 0)
											{
												num2 = num3;
											}
											lock (this.AllTradeLock)
											{
												System.Data.DataRow[] array2 = this.dt_AllTrade.Select(string.Concat(new string[]
												{
													"未成交<>'0' and 投资者='",
													subID,
													"' and 合约='",
													msg[1],
													"'and ((买卖='",
													msg[2],
													"'and 开平='开仓')or (买卖<>'",
													msg[2],
													"'and 开平<>'开仓')) and 状态<>'错误'and 状态<>'已撤单'"
												}));
												for (int j = 0; j < array2.Length; j++)
												{
													if (array2[j]["开平"].ToString() == "开仓")
													{
														num2 -= System.Convert.ToInt32(array2[j]["未成交"]);
													}
													else
													{
														num2 += System.Convert.ToInt32(array2[j]["未成交"]);
													}
												}
											}
											if (msg[6] == "自动")
											{
												Debug.WriteLine(string.Concat(new object[]
												{
													subID,
													",合约",
													msg[1],
													msg[2],
													",T",
													num3,
													",s",
													(dataRow3 == null) ? 0 : System.Convert.ToInt32(dataRow3["总持仓"].ToString()),
													",F",
													num2
												}));
											}
											if (num == 2)
											{
												string[] array3 = this.Dic_ColorCheck[subID].FindAll((string[] s) => s[0] == msg[0] && s[1] == msg[1]).FirstOrDefault<string[]>();
												if (array3 != null)
												{
													array3[2] = num2.ToString();
												}
												else
												{
													this.Dic_ColorCheck[subID].Add(new string[]
													{
														msg[1],
														msg[2],
														num2.ToString()
													});
												}
											}
											else
											{
												string text = "open";
												string text2 = (msg[2] == "买") ? "Buy" : "Sell";
												if (num2 < 0)
												{
													text = "close";
													text2 = ((msg[2] == "买") ? "Sell" : "Buy");
													if (dataRow["交易所"].ToString() == "SHFE")
													{
														int num5 = (dataRow3 == null) ? 0 : System.Convert.ToInt32(dataRow3["今仓"].ToString());
														int num6 = (dataRow3 == null) ? 0 : System.Convert.ToInt32(dataRow3["昨仓"].ToString());
														if (num5 > 0)
														{
															int num7;
															if (System.Math.Abs(num2) >= num5)
															{
																num7 = num5;
															}
															else
															{
																num7 = System.Math.Abs(num2);
															}
															string[] item = new string[]
															{
																msg[1],
																text2,
																"closetoday",
																(text2 == "Buy") ? dataRow["涨停"].ToString() : dataRow["跌停"].ToString(),
																num7.ToString(),
																this.Dic_TradeApi[subID].UpOrderRef().ToString()
															};
															if (num == 0)
															{
																this.Dic_TradeApi[subID].listTransDatas.Add(item);
															}
															else
															{
																this.dlchecktrans[subID].Add(item);
															}
															num2 = System.Math.Abs(num2) - num7;
														}
														if (num2 > 0 && num6 > 0)
														{
															int num8;
															if (num2 >= num6)
															{
																num8 = num6;
															}
															else
															{
																num8 = num2;
															}
															string[] item = new string[]
															{
																msg[1],
																text2,
																"close",
																(text2 == "Buy") ? dataRow["涨停"].ToString() : dataRow["跌停"].ToString(),
																num8.ToString(),
																this.Dic_TradeApi[subID].UpOrderRef().ToString()
															};
															if (num == 0)
															{
																this.Dic_TradeApi[subID].listTransDatas.Add(item);
															}
															else
															{
																this.dlchecktrans[subID].Add(item);
															}
														}
													}
													else
													{
														string text3 = this.Dic_TradeApi[subID].UpOrderRef().ToString();
														string[] item = new string[]
														{
															msg[1],
															text2,
															text,
															(text2 == "Buy") ? dataRow["涨停"].ToString() : dataRow["跌停"].ToString(),
															System.Math.Abs(num2).ToString(),
															text3
														};
														if (num == 0)
														{
															this.Dic_TradeApi[subID].listTransDatas.Add(item);
														}
														else
														{
															this.dlchecktrans[subID].Add(item);
														}
													}
												}
												else if (num2 > 0)
												{
													string text3 = this.Dic_TradeApi[subID].UpOrderRef().ToString();
													string[] item = new string[]
													{
														msg[1],
														text2,
														text,
														(text2 == "Buy") ? dataRow["涨停"].ToString() : dataRow["跌停"].ToString(),
														System.Math.Abs(num2).ToString(),
														text3
													};
													if (num == 0)
													{
														this.Dic_TradeApi[subID].listTransDatas.Add(item);
													}
													else
													{
														this.dlchecktrans[subID].Add(item);
													}
												}
											}
										}
									}
								}
							}
							catch (System.Exception var_24_C99)
							{
							}
							finally
							{
								if (msg[6] == "自动")
								{
									System.Threading.Monitor.Exit(this.AddLock);
								}
							}
						}
						else if (msg[6] == "自动")
						{
							System.Threading.Monitor.Exit(this.AddLock);
						}
					}
				}
				System.Threading.Thread.Sleep(20);
			}
		}

		private string instrtovar(string InstrumentID)
		{
			string text = "";
			for (int i = 0; i < InstrumentID.Length; i++)
			{
				if (char.IsNumber(InstrumentID, i))
				{
					break;
				}
				text += InstrumentID[i];
			}
			return text;
		}

		private void checkmainaccount(string accountID, string code, string direction, string type, bool isReverse, ref int num)
		{
			if (isReverse)
			{
				direction = ((direction == "    卖") ? "买" : "    卖");
			}
			System.Data.DataRow dataRow = this.ds_GroupPosition.Tables[accountID].Select(string.Concat(new string[]
			{
				"合约='",
				code,
				"' and 买卖='",
				direction,
				"'"
			})).FirstOrDefault<System.Data.DataRow>();
			num += ((dataRow == null) ? 0 : System.Convert.ToInt32(dataRow["总持仓"]));
			if (type == "委托")
			{
				System.Data.DataRow[] array = this.dt_MainTrade.Select(string.Concat(new string[]
				{
					"未成交<>'0' and 投资者='",
					accountID,
					"' and 合约='",
					code,
					"'and ((买卖='",
					direction,
					"'and 开平='开仓')or (买卖<>'",
					direction,
					"'and 开平<>'开仓')) and 状态<>'错误'and 状态<>'已撤单'"
				}));
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i]["开平"].ToString() == "开仓")
					{
						num += System.Convert.ToInt32(array[i]["未成交"]);
					}
					else
					{
						num -= System.Convert.ToInt32(array[i]["未成交"]);
					}
				}
			}
		}

		private void AgainOrder(string account, MSOrderMode order)
		{
			string[] array = order.子价格.Split(new char[]
			{
				'@'
			});
			System.Data.DataRow dataRow = this.dtMarketData.Rows.Find(order.合约);
			order.子前置编号 = this.Dic_TradeApi[account].FrontID.ToString();
			order.子会话编号 = this.Dic_TradeApi[account].SessionID.ToString();
			order.子序列号 = this.Dic_TradeApi[account].UpOrderRef().ToString();
			double num;
			string text = (array[0] == "0") ? array[1] : ((array[0] == "3") ? (System.Convert.ToDouble(dataRow["最新价"].ToString()) + (double)((order.买卖 == "Buy") ? 1 : -1) * System.Convert.ToDouble(dataRow["最小波动"].ToString())).ToString() : ((array[0] == "1") ? dataRow["最新价"].ToString() : (((num = System.Convert.ToDouble(dataRow["最新价"].ToString()) + (double)(((order.买卖 == "Buy") ? 1 : -1) * System.Convert.ToInt32(array[1])) * System.Convert.ToDouble(dataRow["最小波动"].ToString())) > System.Convert.ToDouble(dataRow["涨停"].ToString())) ? dataRow["涨停"].ToString() : ((num < System.Convert.ToDouble(dataRow["跌停"].ToString())) ? dataRow["跌停"].ToString() : num.ToString()))));
			string text2 = (array[0] == "3") ? (System.Convert.ToDouble(dataRow["最新价"].ToString()) + (double)((order.买卖 == "Buy") ? 1 : -1) * System.Convert.ToDouble(dataRow["最小波动"].ToString())).ToString() : ((array[0] == "1") ? dataRow["最新价"].ToString() : order.价格.ToString());
			this.listprintmsg.Add(new string[]
			{
				"1",
				string.Concat(new string[]
				{
					account,
					"追单:",
					order.主账户,
					"|",
					order.报单编号,
					"|",
					order.成交编号,
					"|",
					order.序列号,
					"|",
					order.合约,
					"|",
					order.买卖,
					"|",
					order.开平,
					"|",
					System.DateTime.Now.ToString("HH:mm:ss.ffff"),
					"|",
					order.子前置编号,
					"|",
					order.子会话编号,
					"|",
					order.子序列号
				})
			});
			if (dataRow["交易所"].ToString() == "SHFE")
			{
				if (order.开平 != "open")
				{
					int num2 = System.Convert.ToInt32(order.手数);
					string text3 = "买";
					if ((order.买卖 == "Buy" && order.开平 != "open") || (order.买卖 == "Sell" && order.开平 == "open"))
					{
						text3 = "    卖";
					}
					System.Data.DataRow dataRow2 = this.ds_GroupPosition.Tables[account].Select(string.Concat(new string[]
					{
						"合约='",
						order.合约,
						"' and 买卖='",
						text3,
						"'"
					})).FirstOrDefault<System.Data.DataRow>();
					int num3 = (dataRow2 == null) ? 0 : System.Convert.ToInt32(dataRow2["今仓"].ToString());
					int num4 = (dataRow2 == null) ? 0 : System.Convert.ToInt32(dataRow2["昨仓"].ToString());
					if (num3 > 0)
					{
						int num5;
						if (num2 >= num3)
						{
							num5 = num3;
						}
						else
						{
							num5 = num2;
						}
						this.Dic_TradeApi[account].listTransDatas.Add(new string[]
						{
							order.合约,
							order.买卖,
							"closetoday",
							text2,
							num5.ToString(),
							order.子序列号
						});
						num2 -= num5;
						order.开平 = "closetoday";
						order.手数 = num5;
						order.子撤单时间 = ((order.撤单等待时间 == "/") ? "" : System.DateTime.Now.AddSeconds(System.Convert.ToDouble(order.撤单等待时间)).AddMilliseconds(100.0).ToString("HH:mm:ss.fff"));
						this.Dic_SeparateTrade[account].Add(order);
					}
					if (num2 > 0 && num4 > 0)
					{
						int 手数;
						if (num2 >= num4)
						{
							手数 = num4;
						}
						else
						{
							手数 = num2;
						}
						this.Dic_TradeApi[account].listTransDatas.Add(new string[]
						{
							order.合约,
							order.买卖,
							"closeyesterday",
							text2,
							手数.ToString(),
							order.子序列号
						});
						order.开平 = "closeyesterday";
						order.手数 = 手数;
						order.子撤单时间 = ((order.撤单等待时间 == "/") ? "" : System.DateTime.Now.AddSeconds(System.Convert.ToDouble(order.撤单等待时间)).AddMilliseconds(100.0).ToString("HH:mm:ss.fff"));
						this.Dic_SeparateTrade[account].Add(order);
					}
				}
				else
				{
					this.Dic_TradeApi[account].listTransDatas.Add(new string[]
					{
						order.合约,
						order.买卖,
						order.开平,
						text,
						order.手数.ToString(),
						order.子序列号
					});
					order.子撤单时间 = ((order.撤单等待时间 == "/") ? "" : System.DateTime.Now.AddSeconds(System.Convert.ToDouble(order.撤单等待时间)).AddMilliseconds(100.0).ToString("HH:mm:ss.fff"));
					this.Dic_SeparateTrade[account].Add(order);
				}
			}
			else
			{
				this.Dic_TradeApi[account].listTransDatas.Add(new string[]
				{
					order.合约,
					order.买卖,
					order.开平,
					(order.开平 != "open") ? text2 : text,
					order.手数.ToString(),
					order.子序列号
				});
				order.子撤单时间 = ((order.撤单等待时间 == "/") ? "" : System.DateTime.Now.AddSeconds(System.Convert.ToDouble(order.撤单等待时间)).AddMilliseconds(100.0).ToString("HH:mm:ss.fff"));
				this.Dic_SeparateTrade[account].Add(order);
			}
		}

		private void Order(string account, MSOrderMode order)
		{
			string[] array = order.子价格.Split(new char[]
			{
				'@'
			});
			System.Data.DataRow dataRow = this.dtMarketData.Rows.Find(order.合约);
			order.子前置编号 = this.Dic_TradeApi[account].FrontID.ToString();
			order.子会话编号 = this.Dic_TradeApi[account].SessionID.ToString();
			order.子追单 = true;
			double num;
			string text = (array[0] == "0") ? array[1] : ((array[0] == "3") ? ((order.买卖 == "Buy") ? dataRow["涨停"].ToString() : dataRow["跌停"].ToString()) : ((array[0] == "1") ? dataRow["最新价"].ToString() : (((num = System.Convert.ToDouble(dataRow["最新价"].ToString()) + (double)(((order.买卖 == "Buy") ? 1 : -1) * System.Convert.ToInt32(array[1])) * System.Convert.ToDouble(dataRow["最小波动"].ToString())) > System.Convert.ToDouble(dataRow["涨停"].ToString())) ? dataRow["涨停"].ToString() : ((num < System.Convert.ToDouble(dataRow["跌停"].ToString())) ? dataRow["跌停"].ToString() : num.ToString()))));
			string text2 = (array[0] == "3") ? ((order.买卖 == "Buy") ? dataRow["涨停"].ToString() : dataRow["跌停"].ToString()) : ((array[0] == "1") ? dataRow["最新价"].ToString() : order.价格.ToString());
			if (dataRow["交易所"].ToString() == "SHFE")
			{
				if (order.开平 != "open")
				{
					int num2 = System.Convert.ToInt32(order.手数);
					string text3 = "买";
					if (order.买卖 == "Buy")
					{
						text3 = "    卖";
					}
					System.Data.DataRow dataRow2 = this.ds_GroupPosition.Tables[account].Select(string.Concat(new string[]
					{
						"合约='",
						order.合约,
						"' and 买卖='",
						text3,
						"'"
					})).FirstOrDefault<System.Data.DataRow>();
					int num3 = (dataRow2 == null) ? 0 : System.Convert.ToInt32(dataRow2["总持仓"].ToString());
					int num4 = (dataRow2 == null) ? 0 : System.Convert.ToInt32(dataRow2["今仓"].ToString());
					int num5 = (dataRow2 == null) ? 0 : System.Convert.ToInt32(dataRow2["昨仓"].ToString());
					if (num3 > 0)
					{
						if (num4 > 0)
						{
							int num6 = (num2 >= num4) ? num4 : num2;
							order.子序列号 = this.Dic_TradeApi[account].UpOrderRef().ToString();
							this.Dic_TradeApi[account].listTransDatas.Add(new string[]
							{
								order.合约,
								order.买卖,
								"closetoday",
								text2,
								num6.ToString(),
								order.子序列号
							});
							num2 -= num6;
							order.开平 = "closetoday";
							order.手数 = num6;
							order.子撤单时间 = ((order.撤单等待时间 == "/") ? "" : System.DateTime.Now.AddSeconds(System.Convert.ToDouble(order.撤单等待时间)).AddMilliseconds(100.0).ToString("HH:mm:ss.fff"));
							this.SeparateAdd(account, order);
						}
						if (num2 > 0 && num5 > 0)
						{
							int num7 = (num2 >= num5) ? num5 : num2;
							order.子序列号 = this.Dic_TradeApi[account].UpOrderRef().ToString();
							this.Dic_TradeApi[account].listTransDatas.Add(new string[]
							{
								order.合约,
								order.买卖,
								"close",
								text2,
								num7.ToString(),
								order.子序列号
							});
							num2 -= num7;
							order.开平 = "closeyesterday";
							order.手数 = num7;
							order.子撤单时间 = ((order.撤单等待时间 == "/") ? "" : System.DateTime.Now.AddSeconds(System.Convert.ToDouble(order.撤单等待时间)).AddMilliseconds(100.0).ToString("HH:mm:ss.fff"));
							this.SeparateAdd(account, order);
						}
					}
					if (num2 > 0)
					{
						order.子序列号 = this.Dic_TradeApi[account].UpOrderRef().ToString();
						this.Dic_TradeApi[account].listTransDatas.Add(new string[]
						{
							order.合约,
							order.买卖,
							"open",
							text,
							num2.ToString(),
							order.子序列号
						});
						order.开平 = "open";
						order.手数 = num2;
						order.子撤单时间 = ((order.撤单等待时间 == "/") ? "" : System.DateTime.Now.AddSeconds(System.Convert.ToDouble(order.撤单等待时间)).AddMilliseconds(100.0).ToString("HH:mm:ss.fff"));
						this.SeparateAdd(account, order);
					}
				}
				else
				{
					int num2 = System.Convert.ToInt32(order.手数);
					string text3 = "买";
					if (order.买卖 == "Buy")
					{
						text3 = "    卖";
					}
					System.Data.DataRow dataRow2 = this.ds_GroupPosition.Tables[account].Select(string.Concat(new string[]
					{
						"合约='",
						order.合约,
						"' and 买卖='",
						text3,
						"'"
					})).FirstOrDefault<System.Data.DataRow>();
					int num3 = (dataRow2 == null) ? 0 : System.Convert.ToInt32(dataRow2["总持仓"].ToString());
					if (num3 > 0)
					{
						order.子序列号 = this.Dic_TradeApi[account].UpOrderRef().ToString();
						this.Dic_TradeApi[account].listTransDatas.Add(new string[]
						{
							order.合约,
							order.买卖,
							"close",
							text2,
							((num2 - num3 > 0) ? num3 : num2).ToString(),
							order.子序列号
						});
						order.开平 = "close";
						order.手数 = ((num2 - num3 > 0) ? num3 : num2);
						order.子撤单时间 = ((order.撤单等待时间 == "/") ? "" : System.DateTime.Now.AddSeconds(System.Convert.ToDouble(order.撤单等待时间)).AddMilliseconds(100.0).ToString("HH:mm:ss.fff"));
						this.SeparateAdd(account, order);
					}
					if (num2 - num3 > 0)
					{
						order.子序列号 = this.Dic_TradeApi[account].UpOrderRef().ToString();
						this.Dic_TradeApi[account].listTransDatas.Add(new string[]
						{
							order.合约,
							order.买卖,
							"open",
							text,
							(num2 - num3).ToString(),
							order.子序列号
						});
						order.开平 = "open";
						order.手数 = num2 - num3;
						order.子撤单时间 = ((order.撤单等待时间 == "/") ? "" : System.DateTime.Now.AddSeconds(System.Convert.ToDouble(order.撤单等待时间)).AddMilliseconds(100.0).ToString("HH:mm:ss.fff"));
						this.SeparateAdd(account, order);
					}
				}
			}
			else
			{
				int num2 = System.Convert.ToInt32(order.手数);
				string text3 = "买";
				if (order.买卖 == "Buy")
				{
					text3 = "    卖";
				}
				System.Data.DataRow dataRow2 = this.ds_GroupPosition.Tables[account].Select(string.Concat(new string[]
				{
					"合约='",
					order.合约,
					"' and 买卖='",
					text3,
					"'"
				})).FirstOrDefault<System.Data.DataRow>();
				int num3 = (dataRow2 == null) ? 0 : System.Convert.ToInt32(dataRow2["总持仓"].ToString());
				if (num3 > 0)
				{
					order.子序列号 = this.Dic_TradeApi[account].UpOrderRef().ToString();
					this.Dic_TradeApi[account].listTransDatas.Add(new string[]
					{
						order.合约,
						order.买卖,
						"close",
						(System.Convert.ToDouble(text2) + (double)((order.买卖 == "Buy") ? order.平空让点 : (order.平多让点 * -1)) * System.Convert.ToDouble(dataRow["最小波动"].ToString())).ToString(),
						((num2 - num3 > 0) ? num3 : num2).ToString(),
						order.子序列号
					});
					order.开平 = "close";
					order.子撤单时间 = ((order.撤单等待时间 == "/") ? "" : System.DateTime.Now.AddSeconds(System.Convert.ToDouble(order.撤单等待时间)).AddMilliseconds(100.0).ToString("HH:mm:ss.fff"));
					order.手数 = ((num2 - num3 > 0) ? num3 : num2);
					this.SeparateAdd(account, order);
					this.listprintmsg.Add(new string[]
					{
						"1",
						string.Concat(new object[]
						{
							account,
							"下单:",
							order.主账户,
							"|",
							order.报单编号,
							"|",
							order.序列号,
							"|",
							order.合约,
							"|",
							order.买卖,
							"|close|",
							order.手数,
							"|",
							text,
							"|",
							System.DateTime.Now.ToString("HH:mm:ss.fff"),
							"|",
							order.子前置编号,
							"|",
							order.子会话编号,
							"|",
							order.子序列号
						})
					});
				}
				if (num2 - num3 > 0)
				{
					order.子序列号 = this.Dic_TradeApi[account].UpOrderRef().ToString();
					this.Dic_TradeApi[account].listTransDatas.Add(new string[]
					{
						order.合约,
						order.买卖,
						"open",
						(array[0] == "0") ? (System.Convert.ToDouble(array[1]) + (double)((order.买卖 == "Buy") ? order.开多让点 : (order.开空让点 * -1)) * System.Convert.ToDouble(dataRow["最小波动"].ToString())).ToString() : text,
						(num2 - num3).ToString(),
						order.子序列号
					});
					order.开平 = "open";
					order.子撤单时间 = ((order.撤单等待时间 == "/") ? "" : System.DateTime.Now.AddSeconds(System.Convert.ToDouble(order.撤单等待时间)).AddMilliseconds(100.0).ToString("HH:mm:ss.fff"));
					order.手数 = num2 - num3;
					this.SeparateAdd(account, order);
					this.listprintmsg.Add(new string[]
					{
						"1",
						string.Concat(new object[]
						{
							account,
							"下单:",
							order.主账户,
							"|",
							order.报单编号,
							"|",
							order.序列号,
							"|",
							order.合约,
							"|",
							order.买卖,
							"|open|",
							order.手数,
							"|",
							text,
							"|",
							System.DateTime.Now.ToString("HH:mm:ss.fff"),
							"|",
							order.子前置编号,
							"|",
							order.子会话编号,
							"|",
							order.子序列号
						})
					});
				}
			}
		}

		private void SeparateAdd(string account, MSOrderMode order)
		{
			MSOrderMode mSOrderMode = new MSOrderMode();
			mSOrderMode.报单编号 = order.报单编号;
			mSOrderMode.主账户 = order.主账户;
			mSOrderMode.前置编号 = order.前置编号;
			mSOrderMode.会话编号 = order.会话编号;
			mSOrderMode.成交编号 = order.成交编号;
			mSOrderMode.报单编号 = order.报单编号;
			mSOrderMode.序列号 = order.序列号;
			mSOrderMode.价格 = order.价格;
			mSOrderMode.主状态 = order.主状态;
			mSOrderMode.子账户 = order.子账户;
			mSOrderMode.子前置编号 = order.子前置编号;
			mSOrderMode.子会话编号 = order.子会话编号;
			mSOrderMode.子序列号 = order.子序列号;
			mSOrderMode.子撤单时间 = order.子撤单时间;
			mSOrderMode.合约 = order.合约;
			mSOrderMode.买卖 = order.买卖;
			mSOrderMode.开平 = order.开平;
			mSOrderMode.手数 = order.手数;
			mSOrderMode.成交手数 = order.成交手数;
			mSOrderMode.子价格 = order.子价格;
			mSOrderMode.主开时间 = order.主开时间;
			mSOrderMode.本地时间 = order.本地时间;
			mSOrderMode.子下单时间 = order.子下单时间;
			mSOrderMode.子状态 = order.子状态;
			mSOrderMode.子追单 = order.子追单;
			mSOrderMode.执行 = true;
			mSOrderMode.撤单等待时间 = order.撤单等待时间;
			mSOrderMode.开多让点 = order.开多让点;
			mSOrderMode.平多让点 = order.平多让点;
			mSOrderMode.开空让点 = order.开空让点;
			mSOrderMode.平空让点 = order.平空让点;
			this.Dic_SeparateTrade[account].Add(mSOrderMode);
		}

		private void tmTrade(string account)
		{
			while (true)
			{
				if (this.Dic_TradeApi[account].IsLogin && this.BeginListen)
				{
					try
					{
						if (this.Dic_SeparateTrade[account].Count > 0)
						{
							bool flag = false;
							try
							{
								System.Collections.Generic.List<MSOrderMode> obj;
								System.Threading.Monitor.Enter(obj = this.Dic_SeparateTrade[account], ref flag);
								System.DateTime time = System.DateTime.Now;
								System.Collections.Generic.List<MSOrderMode> list = this.Dic_SeparateTrade[account].FindAll((MSOrderMode s) => s.子状态 == "" && System.Convert.ToDateTime(s.子下单时间) <= time && !s.执行);
								System.Collections.Generic.List<MSOrderMode> list2 = this.Dic_SeparateTrade[account].FindAll((MSOrderMode s) => (time - ((s.子撤单时间 == "") ? time.AddSeconds(10.0) : System.Convert.ToDateTime(s.子撤单时间))).TotalMilliseconds >= 0.0 && (s.子状态 != "全部成交" && s.子状态 != "已撤单" && s.子状态 != "错误" && s.子状态 != "未知" && s.子状态 != "发单中" && s.子状态 != "") && s.子追单);
								if (list != null)
								{
									for (int i = 0; i < list.Count; i++)
									{
										try
										{
											MSOrderMode mSOrderMode = new MSOrderMode();
											mSOrderMode.报单编号 = list[i].报单编号;
											mSOrderMode.主账户 = list[i].主账户;
											mSOrderMode.前置编号 = list[i].前置编号;
											mSOrderMode.会话编号 = list[i].会话编号;
											mSOrderMode.成交编号 = list[i].成交编号;
											mSOrderMode.报单编号 = list[i].报单编号;
											mSOrderMode.序列号 = list[i].序列号;
											mSOrderMode.价格 = list[i].价格;
											mSOrderMode.主状态 = list[i].主状态;
											mSOrderMode.子账户 = list[i].子账户;
											mSOrderMode.子前置编号 = list[i].子前置编号;
											mSOrderMode.子会话编号 = list[i].子会话编号;
											mSOrderMode.子序列号 = list[i].子序列号;
											mSOrderMode.子撤单时间 = list[i].子撤单时间;
											mSOrderMode.合约 = list[i].合约;
											mSOrderMode.买卖 = list[i].买卖;
											mSOrderMode.开平 = list[i].开平;
											mSOrderMode.手数 = list[i].手数;
											mSOrderMode.成交手数 = list[i].成交手数;
											mSOrderMode.子价格 = list[i].子价格;
											mSOrderMode.主开时间 = list[i].主开时间;
											mSOrderMode.本地时间 = list[i].本地时间;
											mSOrderMode.子下单时间 = list[i].子下单时间;
											mSOrderMode.子状态 = list[i].子状态;
											mSOrderMode.子追单 = list[i].子追单;
											mSOrderMode.执行 = true;
											mSOrderMode.撤单等待时间 = list[i].撤单等待时间;
											mSOrderMode.开多让点 = list[i].开多让点;
											mSOrderMode.平多让点 = list[i].平多让点;
											mSOrderMode.开空让点 = list[i].开空让点;
											mSOrderMode.平空让点 = list[i].平空让点;
											this.Order(account, mSOrderMode);
										}
										catch (System.Exception ex)
										{
											this.listprintmsg.Add(new string[]
											{
												"0",
												string.Concat(new object[]
												{
													account,
													"下单失败:",
													list[i].主账户,
													"|",
													list[i].报单编号,
													"|",
													list[i].成交编号,
													"|",
													list[i].序列号,
													"|",
													list[i].合约,
													"|",
													list[i].买卖,
													"|",
													list[i].开平,
													"|",
													list[i].价格,
													"|",
													System.DateTime.Now.ToString("HH:mm:ss.fff"),
													ex.Message
												})
											});
											throw new System.Exception(string.Concat(new object[]
											{
												account,
												list[i].合约,
												",",
												list[i].买卖,
												",",
												list[i].开平,
												",",
												list[i].价格,
												ex.Message
											}));
										}
										finally
										{
											this.Dic_SeparateTrade[account].Remove(list[i]);
										}
									}
								}
								for (int i = 0; i < list2.Count; i++)
								{
									try
									{
										if (list2[i].子前置编号 != "" && (list2[i].子会话编号 != "" & list2[i].子序列号 != ""))
										{
											this.Dic_TradeApi[account].ReqOrderAction(list2[i].合约, int.Parse(list2[i].子前置编号), int.Parse(list2[i].子会话编号), list2[i].子序列号);
											this.listprintmsg.Add(new string[]
											{
												"1",
												string.Concat(new object[]
												{
													"账户:",
													account,
													"|合约:",
													list2[i].合约,
													"|时间:",
													list2[i].子下单时间,
													"|方向:",
													list2[i].买卖,
													"|类型:",
													list2[i].开平,
													"|Ref",
													list2[i].子序列号,
													" |手数:",
													list2[i].手数,
													"挂单时间超过:",
													list2[i].子撤单时间,
													"|撤单|",
													System.DateTime.Now.ToString("HH:mm:ss:fff")
												})
											});
											int waittime = 0;
											list2[i].子撤单时间 = "";
											MSOrderMode ms = new MSOrderMode();
											ms.报单编号 = list2[i].报单编号;
											ms.主账户 = list2[i].主账户;
											ms.前置编号 = list2[i].前置编号;
											ms.会话编号 = list2[i].会话编号;
											ms.成交编号 = list2[i].成交编号;
											ms.报单编号 = list2[i].报单编号;
											ms.序列号 = list2[i].序列号;
											ms.价格 = list2[i].价格;
											ms.主状态 = list2[i].主状态;
											ms.子账户 = list2[i].子账户;
											ms.子前置编号 = list2[i].子前置编号;
											ms.子会话编号 = list2[i].子会话编号;
											ms.子序列号 = list2[i].子序列号;
											ms.子撤单时间 = list2[i].子撤单时间;
											ms.合约 = list2[i].合约;
											ms.买卖 = list2[i].买卖;
											ms.开平 = list2[i].开平;
											ms.手数 = list2[i].手数;
											ms.成交手数 = list2[i].成交手数;
											ms.子价格 = list2[i].子价格;
											ms.主开时间 = list2[i].主开时间;
											ms.本地时间 = list2[i].本地时间;
											ms.子下单时间 = list2[i].子下单时间;
											ms.子状态 = list2[i].子状态;
											ms.子追单 = list2[i].子追单;
											ms.执行 = true;
											ms.撤单等待时间 = list2[i].撤单等待时间;
											ms.开多让点 = list2[i].开多让点;
											ms.平多让点 = list2[i].平多让点;
											ms.开空让点 = list2[i].开空让点;
											ms.平空让点 = list2[i].平空让点;
											Task task = new Task(delegate
											{
												bool flag2 = true;
												bool flag3 = true;
												int num = 0;
												while (flag2)
												{
													MSOrderMode mSOrderMode2 = this.Dic_SeparateTrade[account].Find((MSOrderMode x) => x.子前置编号 == ms.子前置编号 && x.子会话编号 == ms.子会话编号 && x.子序列号 == ms.子序列号);
													num = mSOrderMode2.成交手数;
													if (mSOrderMode2.子状态 == "全部成交" || mSOrderMode2.子状态 == "错误")
													{
														flag3 = false;
														mSOrderMode2.candel = true;
														break;
													}
													if (mSOrderMode2.子状态 == "已撤单")
													{
														mSOrderMode2.candel = true;
														break;
													}
													System.Threading.Thread.Sleep(1);
													waittime += 10;
													if (waittime > 1000)
													{
														waittime = 0;
														mSOrderMode2.candel = true;
														break;
													}
												}
												if (flag3 && System.Convert.ToInt32(ms.手数) - num > 0)
												{
													ms.手数 = System.Convert.ToInt32(ms.手数) - num;
													ms.子价格 = "3@0";
													this.AgainOrder(account, ms);
												}
											});
											task.Start();
										}
									}
									catch (System.Exception ex)
									{
										throw new System.Exception(string.Concat(new string[]
										{
											account,
											list2[i].合约,
											",",
											list2[i].买卖,
											",",
											list2[i].开平,
											ex.StackTrace
										}));
									}
									finally
									{
									}
								}
								this.Dic_SeparateTrade[account].RemoveAll((MSOrderMode s) => (s.子状态 == "全部成交" || s.子状态 == "已撤单" || s.子状态 == "错误") && s.子追单 && s.candel);
							}
							finally
							{
								if (flag)
								{
									System.Collections.Generic.List<MSOrderMode> obj;
									System.Threading.Monitor.Exit(obj);
								}
							}
						}
					}
					catch (System.Exception ex)
					{
						this.listprintmsg.Add(new string[]
						{
							"2",
							"下单出错" + ex.Message + ex.StackTrace
						});
					}
					finally
					{
					}
				}
				System.Threading.Thread.Sleep(1);
			}
		}

		private void treadSeparateTrade(int type, OrderMode _om)
		{
			try
			{
				string text = "";
				for (int i = 0; i < _om.合约.Length; i++)
				{
					if (char.IsNumber(_om.合约, i))
					{
						break;
					}
					text += _om.合约[i];
				}
				System.Data.DataRow[] array = this.dt_AccountTradeSet.Select(string.Concat(new string[]
				{
					"主帐户='",
					_om.主账户,
					"' and (品种='",
					_om.合约,
					"'or 品种='",
					text,
					"'or 品种='*')"
				}));
				if (array.Length == 0)
				{
					string text2 = string.Concat(new string[]
					{
						"未搜索到相关配置帐户,未执行信号转发:",
						_om.主账户,
						",",
						_om.报单编号,
						",",
						_om.成交编号,
						",",
						_om.序列号,
						",",
						_om.合约,
						",",
						_om.主开时间
					});
					this.listprintmsg.Add(new string[]
					{
						"1",
						text2
					});
				}
				if (type == 0)
				{
					for (int i = 0; i < array.Length; i++)
					{
						string 买卖 = _om.买卖;
						if (this.Dic_TradeApi.ContainsKey(array[i]["子帐户"].ToString().Trim()) && this.Dic_TradeApi[array[i]["子帐户"].ToString().Trim()].IsLogin && this.Dic_TradeApi[array[i]["子帐户"].ToString().Trim()].SubListenbegin)
						{
							if (!(array[i]["是否反向"].ToString() == "") && (bool)array[i]["是否反向"])
							{
								买卖 = ((_om.买卖 == "Buy") ? "Sell" : "Buy");
							}
							MSOrderMode mSOrderMode = new MSOrderMode
							{
								报单编号 = _om.报单编号,
								成交编号 = _om.成交编号,
								手数 = System.Convert.ToInt32(System.Math.Truncate((double)_om.手数 * double.Parse(array[i]["手数倍率"].ToString().Trim()))),
								合约 = _om.合约,
								买卖 = 买卖,
								开平 = _om.开平,
								价格 = _om.价格,
								子价格 = (array[i]["价格"].ToString().Trim() == "/") ? ((_om.价格 == 0.0) ? "1@0" : ("0@" + _om.价格.ToString())) : ((array[i]["价格"].ToString().Trim() == "0") ? "1@0" : ("2@" + array[i]["价格"].ToString().Trim())),
								序列号 = _om.序列号,
								主状态 = _om.主状态,
								主开时间 = _om.主开时间,
								主账户 = _om.主账户,
								子账户 = array[i]["子帐户"].ToString().Trim(),
								子撤单时间 = (array[i]["撤单等待"].ToString().Trim() == "/") ? "" : System.Convert.ToDateTime(_om.本地时间).AddSeconds(double.Parse(array[i]["撤单等待"].ToString().Trim())).ToString("HH:mm:ss.fff"),
								执行 = false,
								子追单 = false,
								子下单时间 = (array[i]["延时"].ToString().Trim() == "/") ? _om.本地时间 : System.Convert.ToDateTime(_om.本地时间).AddSeconds(double.Parse(array[i]["延时"].ToString().Trim())).ToString("HH:mm:ss.fff"),
								撤单等待时间 = array[i]["撤单等待"].ToString().Trim(),
								开多让点 = System.Convert.ToInt32(array[i]["开多让点"].ToString()),
								平多让点 = System.Convert.ToInt32(array[i]["平多让点"].ToString()),
								开空让点 = System.Convert.ToInt32(array[i]["开空让点"].ToString()),
								平空让点 = System.Convert.ToInt32(array[i]["平空让点"].ToString())
							};
							this.Dic_SeparateTrade[array[i]["子帐户"].ToString().Trim()].Add(mSOrderMode);
							this.listprintmsg.Add(new string[]
							{
								"1",
								string.Concat(new string[]
								{
									array[i]["子帐户"].ToString(),
									"执行信号转发:",
									mSOrderMode.主账户,
									"|",
									mSOrderMode.报单编号,
									"|",
									mSOrderMode.成交编号,
									"|",
									mSOrderMode.序列号,
									"|",
									mSOrderMode.合约,
									"|",
									mSOrderMode.买卖,
									"|",
									mSOrderMode.开平,
									"|",
									System.DateTime.Now.ToString("HH:mm:ss:fff")
								})
							});
						}
						else
						{
							string text2 = array[i]["子帐户"].ToString();
							if (!this.Dic_TradeApi.ContainsKey(array[i]["子帐户"].ToString()))
							{
								text2 += "API不存在！";
							}
							if (this.Dic_TradeApi.ContainsKey(array[i]["子帐户"].ToString()) && !this.Dic_TradeApi[array[i]["子帐户"].ToString()].IsLogin)
							{
								text2 += "帐户未登录！";
							}
							if (this.Dic_TradeApi.ContainsKey(array[i]["子帐户"].ToString()) && this.Dic_TradeApi[array[i]["子帐户"].ToString()].IsLogin && !this.Dic_TradeApi[array[i]["子帐户"].ToString().Trim()].SubListenbegin)
							{
								text2 += "子帐户未开启监听！";
							}
							string text3 = text2;
							text2 = string.Concat(new string[]
							{
								text3,
								"未执行信号转发:",
								_om.主账户,
								"|",
								_om.报单编号,
								"|",
								_om.成交编号,
								"|",
								_om.序列号,
								"|",
								_om.合约,
								"|",
								_om.买卖,
								"|",
								_om.开平,
								"|",
								System.DateTime.Now.ToString("HH:mm:ss:fff")
							});
							this.listprintmsg.Add(new string[]
							{
								"1",
								text2
							});
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				this.listprintmsg.Add(new string[]
				{
					"2",
					string.Concat(new string[]
					{
						"转发信号出错",
						_om.主账户,
						"|",
						_om.报单编号,
						"|",
						_om.成交编号,
						"|",
						_om.序列号,
						"|",
						_om.合约,
						"|",
						_om.买卖,
						"|",
						_om.开平,
						"|",
						System.DateTime.Now.ToString("HH:mm:ss:ffff"),
						ex.StackTrace,
						ex.Message
					})
				});
			}
			finally
			{
			}
		}

		private void showStructInListView(TradeApi.ObjectAndKey oak)
		{
			if (oak.Object.GetType() == typeof(InvestorField))
			{
				try
				{
					InvestorField investorField = (InvestorField)oak.Object;
					System.Data.DataRow dataRow;
					if (this.Dic_TradeApi[investorField.InvestorID].Group == "子帐户")
					{
						dataRow = this.dt_account_dg.Select("投资者帐户='" + investorField.InvestorID + "'").FirstOrDefault<System.Data.DataRow>();
					}
					else
					{
						dataRow = this.dt_mainaccount_dg.Select("投资者帐户='" + investorField.InvestorID + "'").FirstOrDefault<System.Data.DataRow>();
					}
					if (dataRow != null)
					{
						dataRow["投资者"] = investorField.InvestorName;
					}
				}
				catch (System.Exception ex)
				{
					System.Windows.Forms.MessageBox.Show("投资者查询返回" + ex.StackTrace + ex.Message);
				}
			}
			else if (oak.Object.GetType() == typeof(TradingAccount))
			{
				try
				{
					TradingAccount tradingAccount = (TradingAccount)oak.Object;
					bool flag = false;
					if (this.Dic_TradeApi[tradingAccount.AccountID].Group == "主帐户")
					{
						flag = true;
					}
					System.Data.DataRow dataRow;
					if (!flag)
					{
						dataRow = this.dt_account_dg.Select("投资者帐户='" + tradingAccount.AccountID + "'").FirstOrDefault<System.Data.DataRow>();
					}
					else
					{
						dataRow = this.dt_mainaccount_dg.Select("投资者帐户='" + tradingAccount.AccountID + "'").FirstOrDefault<System.Data.DataRow>();
					}
					if (dataRow != null)
					{
						dataRow["动态权益"] = string.Format("{0:0,0.0;-0,0.0;0.#}", tradingAccount.Balance);
						dataRow["上次结算"] = string.Format("{0:0,0.0;-0,0.0;0.#}", tradingAccount.PreBalance);
						dataRow["冻结资金"] = string.Format("{0:0,0.0;-0,0.0;0.#}", tradingAccount.FrozenCash);
						dataRow["占用保证金"] = string.Format("{0:0,0.0;-0,0.0;0.#}", tradingAccount.CurrMargin);
						dataRow["可用资金"] = string.Format("{0:0,0.0;-0,0.0;0.#}", tradingAccount.Available);
						dataRow["平仓盈亏"] = string.Format("{0:0,0.0;-0,0.0;0.#}", tradingAccount.CloseProfit);
						dataRow["持仓盈亏"] = string.Format("{0:0,0.0;-0,0.0;0.#}", tradingAccount.PositionProfit);
						dataRow["手续费"] = string.Format("{0:0,0.0;-0,0.0;0.#}", tradingAccount.Commission);
						dataRow["入金"] = string.Format("{0:0,0.0;-0,0.0;0.#}", tradingAccount.Deposit);
						dataRow["出金"] = string.Format("{0:0,0.0;-0,0.0;0.#}", tradingAccount.Withdraw);
						dataRow["冻结保证金"] = string.Format("{0:0,0.0;-0,0.0;0.#}", tradingAccount.FrozenMargin);
					}
				}
				catch (System.Exception ex)
				{
					System.Windows.Forms.MessageBox.Show("资金查询返回" + ex.StackTrace + ex.Message);
				}
			}
			else if (oak.Object.GetType() == typeof(System.Data.DataTable))
			{
				System.Data.DataTable dataTable = (System.Data.DataTable)oak.Object;
				string[] array = oak.Key.Split(new char[]
				{
					'@'
				});
				if (array[0] == "持仓")
				{
					try
					{
						if (dataTable.Rows.Count > 0)
						{
							this.ds_GroupPosition.Tables[array[1]].Clear();
						}
						for (int i = 0; i < dataTable.Rows.Count; i++)
						{
							string text = dataTable.Rows[i]["合约"].ToString();
							if (this.Dic_TradeApi[array[1]].Group == "子帐户")
							{
								if (!this.HasAddMarket.Contains(text))
								{
									this.ADDSubMarketData(text);
								}
							}
							this.ds_GroupPosition.Tables[array[1]].ImportRow(dataTable.Rows[i]);
							this.UpdateIFPosition(array[1], !(this.Dic_TradeApi[array[1]].Group == "子帐户"), text);
						}
					}
					catch (System.Exception ex)
					{
						System.Windows.Forms.MessageBox.Show("持仓返回" + ex.StackTrace + ex.Message);
					}
				}
				else if (array[0] == "持仓明细")
				{
					try
					{
						if (dataTable.Rows.Count > 0)
						{
							this.ds_GroupPositionDetail.Tables[array[1]].Clear();
						}
						for (int i = 0; i < dataTable.Rows.Count; i++)
						{
							this.ds_GroupPositionDetail.Tables[array[1]].ImportRow(dataTable.Rows[i]);
						}
					}
					catch (System.Exception ex)
					{
						System.Windows.Forms.MessageBox.Show("持仓明细返回" + ex.StackTrace + ex.Message);
					}
				}
			}
			else if (oak.Object.GetType() == typeof(OrderField))
			{
				try
				{
					OrderField field = (OrderField)oak.Object;
					bool flag = false;
					if (this.Dic_TradeApi[field.InvestorID].Group == "主帐户")
					{
						flag = true;
					}
					string text2 = "";
					string text3 = field.Status.ToString();
					switch (text3)
					{
					case "Ordered":
						text2 = "发单中";
						break;
					case "Unknown":
						text2 = "未知";
						break;
					case "NoTradeQueueing":
						text2 = "未成交还在队列中";
						break;
					case "NoTradeNotQueueing":
						text2 = "未成交不在队列中";
						break;
					case "PartTradedQueueing":
						text2 = "部分成交还在队列中";
						break;
					case "PartTradedNotQueueing":
						text2 = "部分成交不在队列中";
						break;
					case "Canceled":
						text2 = "已撤单";
						break;
					case "AllTraded":
						text2 = "全部成交";
						break;
					case "NotTouched":
						text2 = "尚未触发";
						break;
					case "Touched":
						text2 = "已触发";
						break;
					}
					System.Data.DataView dataView;
					if (!flag)
					{
						dataView = new System.Data.DataView(this.dt_AllTrade);
					}
					else
					{
						dataView = new System.Data.DataView(this.dt_MainTrade);
					}
					dataView.RowFilter = string.Concat(new string[]
					{
						"投资者='",
						field.InvestorID,
						"'and 序列号='",
						field.OrderRef,
						"'and 前置编号='",
						field.FrontID.ToString(),
						"' and 会话编号='",
						field.SessionID.ToString(),
						"'"
					});
					System.IFormatProvider provider = new System.Globalization.CultureInfo("zh-CN", true);
					System.DateTime dateTime = (field.InsertDate == "0" || field.InsertTime == "") ? System.DateTime.Now : System.DateTime.ParseExact(field.InsertDate + " " + field.InsertTime, "yyyyMMdd HH:mm:ss", provider);
					if (this.Dic_SeparateTrade.ContainsKey(field.InvestorID))
					{
						MSOrderMode mSOrderMode = this.Dic_SeparateTrade[field.InvestorID].FirstOrDefault((MSOrderMode x) => x.子序列号 == field.OrderRef && x.子前置编号 == field.FrontID.ToString() && x.子会话编号 == field.SessionID.ToString());
						if (mSOrderMode != null)
						{
							mSOrderMode.子状态 = text2;
							mSOrderMode.成交手数 = field.VolumeTraded;
						}
					}
					if (dataView.Count == 0)
					{
						System.Data.DataRow dataRow;
						if (!flag)
						{
							dataRow = this.dt_AllTrade.NewRow();
						}
						else
						{
							dataRow = this.dt_MainTrade.NewRow();
						}
						dataRow["投资者"] = field.InvestorID;
						dataRow["编号"] = field.OrderSysID.Trim();
						dataRow["合约"] = field.InstrumentID;
						dataRow["买卖"] = ((field.Direction == DirectionType.Buy) ? "买" : "    卖");
						dataRow["开平"] = ((field.Offset == EOffsetType.Open) ? "开仓" : ((field.Offset == EOffsetType.Close) ? "平仓" : "平今"));
						dataRow["状态"] = text2;
						dataRow["价格"] = field.LimitPrice.ToString();
						dataRow["报单手数"] = field.VolumeTotalOriginal.ToString();
						dataRow["未成交"] = field.VolumeTotalOriginal.ToString();
						dataRow["成交手数"] = field.VolumeTraded.ToString();
						dataRow["报单时间"] = dateTime;
						dataRow["成交时间"] = field.InsertTime.ToString();
						dataRow["成交均价"] = "0";
						dataRow["详细状态"] = field.StatusMsg;
						dataRow["客户信息"] = field.UserProductInfo;
						dataRow["序列号"] = field.OrderRef;
						dataRow["前置编号"] = field.FrontID.ToString();
						dataRow["会话编号"] = field.SessionID.ToString();
						lock (this.AllTradeLock)
						{
							if (!flag)
							{
								this.dt_AllTrade.Rows.InsertAt(dataRow, 0);
							}
							else
							{
								this.dt_MainTrade.Rows.InsertAt(dataRow, 0);
							}
						}
						if (!flag)
						{
							this.gvTrade.MoveFirst();
						}
						else
						{
							this.gvMainTrade.MoveFirst();
						}
					}
					else if (text2 != "发单中")
					{
						dataView[0].BeginEdit();
						dataView[0]["状态"] = text2;
						dataView[0]["成交手数"] = field.VolumeTraded.ToString();
						dataView[0]["报单时间"] = dateTime;
						dataView[0]["成交时间"] = field.UpdateTime;
						dataView[0]["客户信息"] = field.UserProductInfo;
						dataView[0]["编号"] = field.OrderSysID.Trim();
						dataView[0]["详细状态"] = field.StatusMsg;
						dataView[0].EndEdit();
					}
				}
				catch (System.Exception ex)
				{
					System.Windows.Forms.MessageBox.Show("报单返回" + ex.StackTrace + ex.Message);
				}
			}
			else if (oak.Object.GetType() == typeof(TradeField))
			{
				TradeField field = (TradeField)oak.Object;
				bool flag = false;
				if (this.Dic_TradeApi[field.InvestorID].Group == "主帐户")
				{
					flag = true;
				}
				if (!this.HasAddMarket.Contains(field.InstrumentID))
				{
					this.ADDSubMarketData(field.InstrumentID);
				}
				if (oak.Key != "持仓")
				{
					try
					{
						System.Data.DataView dataView2;
						if (!flag)
						{
							dataView2 = new System.Data.DataView(this.dt_AllDeal);
						}
						else
						{
							dataView2 = new System.Data.DataView(this.dt_MainDeal);
						}
						dataView2.RowFilter = string.Concat(new string[]
						{
							"投资者='",
							field.InvestorID,
							"'and  编号='",
							field.TradeID.Trim(),
							"'and  报单编号='",
							field.OrderSysID.Trim(),
							"'"
						});
						if (dataView2.Count == 0)
						{
							if (oak.Key == "1")
							{
								if (this.TradeORDeal && flag && this.BeginListen && this.Dic_TradeApi[field.InvestorID].IsLogin)
								{
									string 买卖 = (field.Direction == DirectionType.Buy) ? "Buy" : "Sell";
									string 开平 = (field.Offset == EOffsetType.Open) ? "open" : ((field.Offset == EOffsetType.Close) ? "close" : ((field.Offset == EOffsetType.CloseToday) ? "closetoday" : "closeyesterday"));
									OrderMode orderMode = new OrderMode
									{
										主账户 = field.InvestorID,
										报单编号 = field.OrderSysID,
										成交编号 = field.TradeID,
										合约 = field.InstrumentID,
										序列号 = field.OrderRef,
										买卖 = 买卖,
										开平 = 开平,
										价格 = field.Price,
										手数 = field.Volume,
										主开时间 = field.TradeTime,
										主状态 = "",
										本地时间 = System.DateTime.Now.ToString("HH:mm:ss.fff")
									};
									this.treadSeparateTrade(0, orderMode);
									string text4 = string.Concat(new object[]
									{
										field.InvestorID,
										"监听到新成交单:",
										field.OrderRef,
										"|",
										field.InstrumentID,
										"|",
										orderMode.买卖,
										"|",
										orderMode.开平,
										"|",
										orderMode.手数,
										"|",
										field.TradeID,
										"|",
										field.OrderSysID,
										"|",
										field.OrderRef,
										"|",
										orderMode.主开时间,
										"|",
										orderMode.本地时间
									});
									this.listprintmsg.Add(new string[]
									{
										"1",
										text4
									});
								}
								else if (this.TradeORDeal && flag)
								{
									string text4 = field.InvestorID + "未转发新成交单:";
									if (!this.BeginListen)
									{
										text4 += "未启动监听！";
									}
									if (!this.Dic_TradeApi[field.InvestorID].IsLogin)
									{
										text4 += "帐户未登录！";
									}
									string text5 = text4;
									text4 = string.Concat(new string[]
									{
										text5,
										field.OrderRef,
										",",
										field.InstrumentID,
										",",
										System.DateTime.Now.ToString("HH:mm:ss:ffff")
									});
									this.listprintmsg.Add(new string[]
									{
										"1",
										text4
									});
								}
							}
							System.Data.DataRowView dataRowView = dataView2.AddNew();
							dataRowView["投资者"] = field.InvestorID;
							dataRowView["编号"] = field.TradeID.Trim();
							dataRowView["合约"] = field.InstrumentID;
							dataRowView["买卖"] = ((field.Direction == DirectionType.Buy) ? "买" : "    卖");
							dataRowView["开平"] = ((field.Offset == EOffsetType.Open) ? "开仓" : ((field.Offset == EOffsetType.Close) ? "平仓" : ((field.Offset == EOffsetType.CloseToday) ? "平今" : "平昨")));
							dataRowView["成交价格"] = field.Price.ToString("F3");
							dataRowView["成交手数"] = field.Volume.ToString();
							dataRowView["成交时间"] = field.TradeTime;
							dataRowView["报单编号"] = field.OrderSysID.Trim();
							dataRowView["成交类型"] = field.TradeType.ToString();
							dataRowView["交易所"] = field.ExchangeID;
							if (!flag)
							{
								double num2 = 0.0;
								MSOrderMode mSOrderMode = this.Dic_SeparateTrade[field.InvestorID].FirstOrDefault((MSOrderMode x) => x.子序列号 == field.OrderRef && x.子前置编号 == this.Dic_TradeApi[field.InvestorID].FrontID.ToString() && x.子会话编号 == this.Dic_TradeApi[field.InvestorID].SessionID.ToString());
								if (mSOrderMode != null)
								{
									num2 = System.Math.Round(field.Price - mSOrderMode.价格, 1);
									if ((field.Direction == DirectionType.Buy && field.Offset == EOffsetType.Open) || (field.Direction == DirectionType.Buy && field.Offset != EOffsetType.Open))
									{
										num2 *= -1.0;
									}
								}
								dataRowView["滑点"] = num2.ToString();
							}
							dataRowView.EndEdit();
							System.Data.DataView dataView;
							if (!flag)
							{
								dataView = new System.Data.DataView(this.dt_AllTrade);
							}
							else
							{
								dataView = new System.Data.DataView(this.dt_MainTrade);
							}
							dataView.RowFilter = string.Concat(new string[]
							{
								"投资者='",
								field.InvestorID,
								"'and 编号='",
								field.OrderSysID.Trim(),
								"'"
							});
							if (dataView.Count != 0)
							{
								lock (this.AllTradeLock)
								{
									dataView[0].BeginEdit();
									System.Data.DataRowView arg_1609_0 = dataView[0];
									string arg_1609_1 = "未成交";
									int num = System.Convert.ToInt32(dataView[0]["未成交"].ToString()) - field.Volume;
									arg_1609_0[arg_1609_1] = num.ToString();
									dataView[0]["成交均价"] = field.Price.ToString();
									dataView[0]["成交时间"] = field.TradeTime;
									dataView[0].EndEdit();
								}
							}
						}
					}
					catch (System.Exception ex)
					{
						System.Windows.Forms.MessageBox.Show("成交回报返回" + ex.StackTrace + ex.Message);
					}
				}
				else
				{
					try
					{
						double num3 = 0.0;
						string text6 = "";
						bool flag4 = true;
						if ((field.Direction == DirectionType.Buy && field.Offset == EOffsetType.Open) || (field.Direction == DirectionType.Sell && field.Offset != EOffsetType.Open))
						{
							text6 = "Long";
							System.Data.DataRow dataRow2;
							num3 = (double)(((dataRow2 = this.Dic_TradeApi[field.InvestorID].dtInstruments.Rows.Find(field.InstrumentID)) == null) ? "0.0" : dataRow2["保证金-多"]);
						}
						else if ((field.Direction == DirectionType.Buy && field.Offset != EOffsetType.Open) || (field.Direction == DirectionType.Sell && field.Offset == EOffsetType.Open))
						{
							text6 = "Short";
							System.Data.DataRow dataRow3;
							num3 = (double)(((dataRow3 = this.Dic_TradeApi[field.InvestorID].dtInstruments.Rows.Find(field.InstrumentID)) == null) ? "0.0" : dataRow3["保证金-空"]);
						}
						if (field.Offset != EOffsetType.Open)
						{
							if (field.Offset == EOffsetType.CloseYesterday)
							{
							}
							flag4 = false;
						}
						if (flag4)
						{
							lock (this.DetailLock)
							{
								if (this.ds_GroupPositionDetail.Tables[field.InvestorID].Select("编号='" + field.TradeID.Trim() + "'").Length == 0)
								{
									System.Data.DataRow dataRow4 = this.ds_GroupPositionDetail.Tables[field.InvestorID].NewRow();
									dataRow4["编号"] = long.Parse(field.TradeID.Trim());
									dataRow4["合约"] = field.InstrumentID;
									dataRow4["买卖"] = ((text6 == "Long") ? "买" : "    卖");
									dataRow4["持仓类型"] = "今仓";
									dataRow4["成交手数"] = field.Volume.ToString();
									dataRow4["成交价格"] = field.Price.ToString();
									dataRow4["成交时间"] = field.TradingDay;
									this.ds_GroupPositionDetail.Tables[field.InvestorID].Rows.Add(dataRow4);
								}
							}
						}
						System.Data.DataRow dataRow5;
						int num4 = (int)(((dataRow5 = this.Dic_TradeApi[field.InvestorID].dtInstruments.Rows.Find(field.InstrumentID)) == null) ? "0" : dataRow5["合约数量"]);
						System.Data.DataRow[] array2 = this.ds_GroupPosition.Tables[field.InvestorID].Select(string.Concat(new string[]
						{
							"合约='",
							field.InstrumentID,
							"'and 买卖='",
							(text6 == "Long") ? "买" : "    卖",
							"'"
						}));
						System.Data.DataRow dataRow6;
						if (!flag)
						{
							dataRow6 = this.dt_account_dg.Select("投资者帐户='" + field.InvestorID + "'").FirstOrDefault<System.Data.DataRow>();
						}
						else
						{
							dataRow6 = this.dt_mainaccount_dg.Select("投资者帐户='" + field.InvestorID + "'").FirstOrDefault<System.Data.DataRow>();
						}
						System.Data.DataRow dataRow7;
						double num5 = (double)(((dataRow7 = this.dtMarketData.Rows.Find(field.InstrumentID)) == null) ? 0.0 : dataRow7["最新价"]);
						if (array2.Length == 0 && field.Offset == EOffsetType.Open)
						{
							System.Data.DataRow dataRow8 = this.ds_GroupPosition.Tables[field.InvestorID].NewRow();
							dataRow8["合约"] = field.InstrumentID;
							dataRow8["买卖"] = ((field.Direction == DirectionType.Buy) ? "买" : "    卖");
							dataRow8["昨仓"] = 0;
							dataRow8["今仓"] = field.Volume.ToString();
							dataRow8["总持仓"] = 0;
							dataRow8["可平昨"] = 0;
							dataRow8["可平今"] = field.Volume.ToString();
							dataRow8["总持仓"] = System.Convert.ToInt32(dataRow8["昨仓"].ToString()) + System.Convert.ToInt32(dataRow8["今仓"].ToString());
							dataRow8["持仓均价"] = field.Price.ToString("F3");
							dataRow8["开仓均价"] = field.Price.ToString("F3");
							if (num5 > 0.0)
							{
								if (text6 == "Long")
								{
									dataRow8["持仓盈亏"] = ((num5 - double.Parse(dataRow8["持仓均价"].ToString())) * (double)int.Parse(dataRow8["总持仓"].ToString()) * (double)num4).ToString("F0");
								}
								else
								{
									dataRow8["持仓盈亏"] = ((double.Parse(dataRow8["持仓均价"].ToString()) - num5) * (double)int.Parse(dataRow8["总持仓"].ToString()) * (double)num4).ToString("F0");
								}
							}
							else
							{
								dataRow8["持仓盈亏"] = (double.Parse(dataRow8["持仓均价"].ToString()) * (double)int.Parse(dataRow8["总持仓"].ToString()) * (double)num4).ToString("F0");
							}
							double num6 = (double)((this.Dic_TradeApi[field.InvestorID].dtInstruments.Rows.Find(field.InstrumentID)["开仓手续费"] == System.DBNull.Value) ? 0.0 : this.Dic_TradeApi[field.InvestorID].dtInstruments.Rows.Find(field.InstrumentID)["开仓手续费"]);
							double num7 = (num6 == 0.0) ? ((double)((this.Dic_TradeApi[field.InvestorID].dtInstruments.Rows.Find(field.InstrumentID)["开仓手续费率"] == System.DBNull.Value) ? 0.0 : this.Dic_TradeApi[field.InvestorID].dtInstruments.Rows.Find(field.InstrumentID)["开仓手续费率"]) * (double)field.Volume * (double)num4 * field.Price) : (num6 * (double)field.Volume);
							dataRow6["动态权益"] = (double.Parse(dataRow6["动态权益"].ToString()) - num7 + double.Parse(dataRow8["持仓盈亏"].ToString())).ToString("F0");
							dataRow6["持仓盈亏"] = (double.Parse(dataRow6["持仓盈亏"].ToString()) + double.Parse(dataRow8["持仓盈亏"].ToString())).ToString("F0");
							dataRow6["手续费"] = (double.Parse(dataRow6["手续费"].ToString()) + num7).ToString("F0");
							dataRow8["空头占用保证金"] = ((dataRow8["买卖"].ToString() == "买") ? "0" : (System.Convert.ToDouble(dataRow8["持仓均价"].ToString()) * (double)System.Convert.ToInt32(dataRow8["总持仓"].ToString()) * (double)num4 * num3).ToString());
							dataRow8["多头占用保证金"] = ((dataRow8["买卖"].ToString() == "买") ? (System.Convert.ToDouble(dataRow8["持仓均价"].ToString()) * (double)System.Convert.ToInt32(dataRow8["总持仓"].ToString()) * (double)num4 * num3).ToString() : "0");
							dataRow8["投保"] = ((field.Hedge == HedgeType.Speculation) ? "投机" : "套利");
							dataRow8["校对"] = "0";
							this.ds_GroupPosition.Tables[field.InvestorID].Rows.Add(dataRow8);
						}
						else if (array2.Length > 0)
						{
							System.Data.DataRow dataRow8 = array2[0];
							int j = field.Volume;
							int num8 = System.Convert.ToInt32(dataRow8["总持仓"].ToString());
							double num9 = System.Convert.ToDouble(dataRow8["持仓均价"].ToString());
							double num10 = System.Convert.ToDouble(dataRow8["开仓均价"].ToString());
							System.Data.DataRow[] array3 = this.ds_GroupPositionDetail.Tables[field.InvestorID].Select(string.Concat(new string[]
							{
								"合约='",
								field.InstrumentID,
								"'and 买卖='",
								(text6 == "Long") ? "买" : "    卖",
								"' and 持仓类型='今仓'"
							}), "编号 desc");
							System.Data.DataRow[] array4 = this.ds_GroupPositionDetail.Tables[field.InvestorID].Select(string.Concat(new string[]
							{
								"合约='",
								field.InstrumentID,
								"'and 买卖='",
								(text6 == "Long") ? "买" : "    卖",
								"' and 持仓类型='昨仓'"
							}), "编号 desc");
							int num;
							if (field.Offset == EOffsetType.Open)
							{
								dataRow8["持仓均价"] = ((num9 * (double)num8 + field.Price * (double)field.Volume) / (double)(num8 + field.Volume)).ToString("F3");
								dataRow8["开仓均价"] = ((num10 * (double)num8 + field.Price * (double)field.Volume) / (double)(num8 + field.Volume)).ToString("F3");
								System.Data.DataRow arg_23F3_0 = dataRow8;
								string arg_23F3_1 = "今仓";
								num = field.Volume + System.Convert.ToInt32(dataRow8["今仓"].ToString());
								arg_23F3_0[arg_23F3_1] = num.ToString();
								System.Data.DataRow arg_242C_0 = dataRow8;
								string arg_242C_1 = "可平今";
								num = field.Volume + System.Convert.ToInt32(dataRow8["可平今"].ToString());
								arg_242C_0[arg_242C_1] = num.ToString();
								double num6 = (double)((this.Dic_TradeApi[field.InvestorID].dtInstruments.Rows.Find(field.InstrumentID)["开仓手续费"] == System.DBNull.Value) ? 0.0 : this.Dic_TradeApi[field.InvestorID].dtInstruments.Rows.Find(field.InstrumentID)["开仓手续费"]);
								double num7 = (num6 == 0.0) ? ((double)((this.Dic_TradeApi[field.InvestorID].dtInstruments.Rows.Find(field.InstrumentID)["开仓手续费率"] == System.DBNull.Value) ? 0.0 : this.Dic_TradeApi[field.InvestorID].dtInstruments.Rows.Find(field.InstrumentID)["开仓手续费率"]) * (double)field.Volume * (double)num4 * field.Price) : (num6 * (double)field.Volume);
								dataRow6["手续费"] = (double.Parse(dataRow6["手续费"].ToString()) + num7).ToString("F0");
								dataRow6["动态权益"] = (double.Parse(dataRow6["动态权益"].ToString()) - num7).ToString("F0");
							}
							else if (field.Offset == EOffsetType.CloseToday)
							{
								double num6 = (double)((this.Dic_TradeApi[field.InvestorID].dtInstruments.Rows.Find(field.InstrumentID)["平今手续费"] == System.DBNull.Value) ? 0.0 : this.Dic_TradeApi[field.InvestorID].dtInstruments.Rows.Find(field.InstrumentID)["平今手续费"]);
								double num7 = (num6 == 0.0) ? ((double)((this.Dic_TradeApi[field.InvestorID].dtInstruments.Rows.Find(field.InstrumentID)["平今手续费率"] == System.DBNull.Value) ? 0.0 : this.Dic_TradeApi[field.InvestorID].dtInstruments.Rows.Find(field.InstrumentID)["平今手续费率"]) * (double)field.Volume * (double)num4 * field.Price) : (num6 * (double)field.Volume);
								for (int i = array3.Length - 1; i >= 0; i--)
								{
									int num11 = System.Convert.ToInt32(array3[i]["成交手数"].ToString());
									double num12 = System.Convert.ToDouble(array3[i]["成交价格"].ToString());
									if (num11 > j)
									{
										dataRow8["持仓均价"] = ((System.Convert.ToDouble(dataRow8["持仓均价"].ToString()) * (double)num8 - num12 * (double)j) / (double)(num8 - j)).ToString("F3");
										dataRow8["开仓均价"] = ((System.Convert.ToDouble(dataRow8["开仓均价"].ToString()) * (double)num8 - num12 * (double)j) / (double)(num8 - j)).ToString("F3");
										System.Data.DataRow arg_2891_0 = array3[i];
										string arg_2891_1 = "成交手数";
										num = num11 - j;
										arg_2891_0[arg_2891_1] = num.ToString();
										this.updateaccount(dataRow6, num12, field.Price, j, num4, text6, num7);
										break;
									}
									if (num11 == j)
									{
										dataRow8["持仓均价"] = ((System.Convert.ToDouble(dataRow8["持仓均价"].ToString()) * (double)num8 - num12 * (double)num11) / (double)(num8 - num11)).ToString("F3");
										dataRow8["开仓均价"] = ((System.Convert.ToDouble(dataRow8["开仓均价"].ToString()) * (double)num8 - num12 * (double)num11) / (double)(num8 - num11)).ToString("F3");
										this.ds_GroupPositionDetail.Tables[field.InvestorID].Rows.Remove(array3[i]);
										this.updateaccount(dataRow6, num12, field.Price, j, num4, text6, num7);
										break;
									}
									dataRow8["持仓均价"] = ((System.Convert.ToDouble(dataRow8["持仓均价"].ToString()) * (double)num8 - num12 * (double)num11) / (double)(num8 - num11)).ToString("F3");
									dataRow8["开仓均价"] = ((System.Convert.ToDouble(dataRow8["开仓均价"].ToString()) * (double)num8 - num12 * (double)num11) / (double)(num8 - num11)).ToString("F3");
									num8 -= num11;
									j -= num11;
									this.ds_GroupPositionDetail.Tables[field.InvestorID].Rows.Remove(array3[i]);
									num7 = ((num6 == 0.0) ? ((double)((this.Dic_TradeApi[field.InvestorID].dtInstruments.Rows.Find(field.InstrumentID)["平今手续费率"] == System.DBNull.Value) ? 0.0 : this.Dic_TradeApi[field.InvestorID].dtInstruments.Rows.Find(field.InstrumentID)["平今手续费率"]) * (double)num11 * (double)num4 * field.Price) : (num6 * (double)num11));
									this.updateaccount(dataRow6, num12, field.Price, num11, num4, text6, num7);
								}
								System.Data.DataRow arg_2B95_0 = dataRow8;
								string arg_2B95_1 = "今仓";
								num = System.Convert.ToInt32(dataRow8["今仓"].ToString()) - field.Volume;
								arg_2B95_0[arg_2B95_1] = num.ToString();
							}
							else if (field.Offset == EOffsetType.CloseYesterday)
							{
								double num13 = (double)this.dtMarketData.Rows.Find(field.InstrumentID)["昨结"];
								double num6 = (double)((this.Dic_TradeApi[field.InvestorID].dtInstruments.Rows.Find(field.InstrumentID)["平仓手续费"] == System.DBNull.Value) ? 0.0 : this.Dic_TradeApi[field.InvestorID].dtInstruments.Rows.Find(field.InstrumentID)["平仓手续费"]);
								double num7 = (num6 == 0.0) ? ((double)((this.Dic_TradeApi[field.InvestorID].dtInstruments.Rows.Find(field.InstrumentID)["平仓手续费率"] == System.DBNull.Value) ? 0.0 : this.Dic_TradeApi[field.InvestorID].dtInstruments.Rows.Find(field.InstrumentID)["平仓手续费率"]) * (double)field.Volume * (double)num4 * field.Price) : (num6 * (double)field.Volume);
								this.updateaccount(dataRow6, num13, field.Price, field.Volume, num4, text6, num7);
								for (int i = array4.Length - 1; i >= 0; i--)
								{
									int num11 = System.Convert.ToInt32(array4[i]["成交手数"].ToString());
									double num12 = System.Convert.ToDouble(array4[i]["成交价格"].ToString());
									if (num11 > j)
									{
										dataRow8["持仓均价"] = ((System.Convert.ToDouble(dataRow8["持仓均价"].ToString()) * (double)num8 - num13 * (double)j) / (double)(num8 - j)).ToString("F3");
										dataRow8["开仓均价"] = ((System.Convert.ToDouble(dataRow8["开仓均价"].ToString()) * (double)num8 - num12 * (double)j) / (double)(num8 - j)).ToString("F3");
										System.Data.DataRow arg_2E78_0 = array4[i];
										string arg_2E78_1 = "成交手数";
										num = num11 - j;
										arg_2E78_0[arg_2E78_1] = num.ToString();
										break;
									}
									if (num11 == j)
									{
										dataRow8["持仓均价"] = ((System.Convert.ToDouble(dataRow8["持仓均价"].ToString()) * (double)num8 - num13 * (double)num11) / (double)(num8 - num11)).ToString("F3");
										dataRow8["开仓均价"] = ((System.Convert.ToDouble(dataRow8["开仓均价"].ToString()) * (double)num8 - num12 * (double)num11) / (double)(num8 - num11)).ToString("F3");
										this.ds_GroupPositionDetail.Tables[field.InvestorID].Rows.Remove(array4[i]);
										break;
									}
									dataRow8["持仓均价"] = ((System.Convert.ToDouble(dataRow8["持仓均价"].ToString()) * (double)num8 - num13 * (double)num11) / (double)(num8 - num11)).ToString("F3");
									dataRow8["开仓均价"] = ((System.Convert.ToDouble(dataRow8["开仓均价"].ToString()) * (double)num8 - num12 * (double)num11) / (double)(num8 - num11)).ToString("F3");
									num8 -= num11;
									j -= num11;
									this.ds_GroupPositionDetail.Tables[field.InvestorID].Rows.Remove(array4[i]);
								}
								System.Data.DataRow arg_305A_0 = dataRow8;
								string arg_305A_1 = "昨仓";
								num = System.Convert.ToInt32(dataRow8["昨仓"].ToString()) - field.Volume;
								arg_305A_0[arg_305A_1] = num.ToString();
							}
							else if (field.Offset == EOffsetType.Close)
							{
								double num14 = (double)((this.Dic_TradeApi[field.InvestorID].dtInstruments.Rows.Find(field.InstrumentID)["平仓手续费"] == System.DBNull.Value) ? 0.0 : this.Dic_TradeApi[field.InvestorID].dtInstruments.Rows.Find(field.InstrumentID)["平仓手续费"]);
								double num15 = (double)((this.Dic_TradeApi[field.InvestorID].dtInstruments.Rows.Find(field.InstrumentID)["平仓手续费率"] == System.DBNull.Value) ? 0.0 : this.Dic_TradeApi[field.InvestorID].dtInstruments.Rows.Find(field.InstrumentID)["平仓手续费率"]);
								double num7 = (num14 == 0.0) ? (num15 * (double)field.Volume * (double)num4 * field.Price) : (num14 * (double)field.Volume);
								if (array4.Length > 0)
								{
									double num13 = (double)this.dtMarketData.Rows.Find(field.InstrumentID)["昨结"];
									this.updateaccount(dataRow6, num13, field.Price, field.Volume, num4, text6, num7);
									for (int i = array4.Length - 1; i >= 0; i--)
									{
										int num11 = System.Convert.ToInt32(array4[i]["成交手数"].ToString());
										double num12 = System.Convert.ToDouble(array4[i]["成交价格"].ToString());
										if (num11 > j)
										{
											dataRow8["持仓均价"] = ((System.Convert.ToDouble(dataRow8["持仓均价"].ToString()) * (double)num8 - num13 * (double)j) / (double)(num8 - j)).ToString("F3");
											dataRow8["开仓均价"] = ((System.Convert.ToDouble(dataRow8["开仓均价"].ToString()) * (double)num8 - num12 * (double)j) / (double)(num8 - j)).ToString("F3");
											System.Data.DataRow arg_3352_0 = array4[i];
											string arg_3352_1 = "成交手数";
											num = num11 - j;
											arg_3352_0[arg_3352_1] = num.ToString();
											j = 0;
											break;
										}
										if (num11 == j)
										{
											dataRow8["持仓均价"] = ((System.Convert.ToDouble(dataRow8["持仓均价"].ToString()) * (double)num8 - num13 * (double)num11) / (double)(num8 - num11)).ToString("F3");
											dataRow8["开仓均价"] = ((System.Convert.ToDouble(dataRow8["开仓均价"].ToString()) * (double)num8 - num12 * (double)num11) / (double)(num8 - num11)).ToString("F3");
											lock (this.DetailLock)
											{
												this.ds_GroupPositionDetail.Tables[field.InvestorID].Rows.Remove(array4[i]);
											}
											j = 0;
											break;
										}
										dataRow8["持仓均价"] = ((System.Convert.ToDouble(dataRow8["持仓均价"].ToString()) * (double)num8 - num13 * (double)num11) / (double)(num8 - num11)).ToString("F3");
										dataRow8["开仓均价"] = ((System.Convert.ToDouble(dataRow8["开仓均价"].ToString()) * (double)num8 - num12 * (double)num11) / (double)(num8 - num11)).ToString("F3");
										num8 -= num11;
										j -= num11;
										lock (this.DetailLock)
										{
											this.ds_GroupPositionDetail.Tables[field.InvestorID].Rows.Remove(array4[i]);
										}
									}
								}
								array3 = this.ds_GroupPositionDetail.Tables[field.InvestorID].Select(string.Concat(new string[]
								{
									"合约='",
									field.InstrumentID,
									"'and 买卖='",
									(text6 == "Long") ? "买" : "    卖",
									"' and 持仓类型='今仓'"
								}), "编号 desc");
								if (array3.Length > 0)
								{
									for (int i = array3.Length - 1; i >= 0; i--)
									{
										int num11 = System.Convert.ToInt32(array3[i]["成交手数"].ToString());
										double num12 = System.Convert.ToDouble(array3[i]["成交价格"].ToString());
										if (num11 > j)
										{
											dataRow8["持仓均价"] = ((System.Convert.ToDouble(dataRow8["持仓均价"].ToString()) * (double)num8 - num12 * (double)j) / (double)(num8 - j)).ToString("F3");
											dataRow8["开仓均价"] = ((System.Convert.ToDouble(dataRow8["开仓均价"].ToString()) * (double)num8 - num12 * (double)j) / (double)(num8 - j)).ToString("F3");
											System.Data.DataRow arg_36EC_0 = array3[i];
											string arg_36EC_1 = "成交手数";
											num = num11 - j;
											arg_36EC_0[arg_36EC_1] = num.ToString();
											num7 = ((num14 == 0.0) ? (num15 * (double)j * (double)num4 * field.Price) : (num14 * (double)j));
											this.updateaccount(dataRow6, num12, field.Price, j, num4, text6, num7);
											j = 0;
											break;
										}
										if (num11 == j)
										{
											dataRow8["持仓均价"] = ((System.Convert.ToDouble(dataRow8["持仓均价"].ToString()) * (double)num8 - num12 * (double)num11) / (double)(num8 - num11)).ToString("F3");
											dataRow8["开仓均价"] = ((System.Convert.ToDouble(dataRow8["开仓均价"].ToString()) * (double)num8 - num12 * (double)num11) / (double)(num8 - num11)).ToString("F3");
											lock (this.DetailLock)
											{
												this.ds_GroupPositionDetail.Tables[field.InvestorID].Rows.Remove(array3[i]);
											}
											num7 = ((num14 == 0.0) ? (num15 * (double)j * (double)num4 * field.Price) : (num14 * (double)j));
											this.updateaccount(dataRow6, num12, field.Price, j, num4, text6, num7);
											j = 0;
											break;
										}
										dataRow8["持仓均价"] = ((System.Convert.ToDouble(dataRow8["持仓均价"].ToString()) * (double)num8 - num12 * (double)num11) / (double)(num8 - num11)).ToString("F3");
										dataRow8["开仓均价"] = ((System.Convert.ToDouble(dataRow8["开仓均价"].ToString()) * (double)num8 - num12 * (double)num11) / (double)(num8 - num11)).ToString("F3");
										num8 -= num11;
										j -= num11;
										lock (this.DetailLock)
										{
											this.ds_GroupPositionDetail.Tables[field.InvestorID].Rows.Remove(array3[i]);
										}
										num7 = ((num14 == 0.0) ? (num15 * (double)num11 * (double)num4 * field.Price) : (num14 * (double)num11));
										this.updateaccount(dataRow6, num12, field.Price, num11, num4, text6, num7);
									}
								}
								j = field.Volume;
								while (j > 0)
								{
									if (System.Convert.ToInt32(dataRow8["昨仓"].ToString()) > 0)
									{
										if (System.Convert.ToInt32(dataRow8["昨仓"].ToString()) >= j)
										{
											System.Data.DataRow arg_3A6A_0 = dataRow8;
											string arg_3A6A_1 = "昨仓";
											num = System.Convert.ToInt32(dataRow8["昨仓"].ToString()) - j;
											arg_3A6A_0[arg_3A6A_1] = num.ToString();
											j = 0;
										}
										else
										{
											j -= System.Convert.ToInt32(dataRow8["昨仓"].ToString());
											dataRow8["昨仓"] = "0";
										}
									}
									if (System.Convert.ToInt32(dataRow8["今仓"].ToString()) > 0)
									{
										if (System.Convert.ToInt32(dataRow8["今仓"].ToString()) >= j)
										{
											System.Data.DataRow arg_3B16_0 = dataRow8;
											string arg_3B16_1 = "今仓";
											num = System.Convert.ToInt32(dataRow8["今仓"].ToString()) - j;
											arg_3B16_0[arg_3B16_1] = num.ToString();
											j = 0;
										}
										else
										{
											j -= System.Convert.ToInt32(dataRow8["今仓"].ToString());
											dataRow8["今仓"] = 0;
										}
									}
								}
							}
							System.Data.DataRow arg_3BA0_0 = dataRow8;
							string arg_3BA0_1 = "总持仓";
							num = System.Convert.ToInt32(dataRow8["昨仓"].ToString()) + System.Convert.ToInt32(dataRow8["今仓"].ToString());
							arg_3BA0_0[arg_3BA0_1] = num.ToString();
							if (num5 > 0.0)
							{
								if (text6 == "Long")
								{
									dataRow8["持仓盈亏"] = ((double.Parse(dataRow8["持仓均价"].ToString()) - num5) * (double)int.Parse(dataRow8["总持仓"].ToString()) * (double)num4).ToString("F0");
								}
								else
								{
									dataRow8["持仓盈亏"] = ((num5 - double.Parse(dataRow8["持仓均价"].ToString())) * (double)int.Parse(dataRow8["总持仓"].ToString()) * (double)num4).ToString("F0");
								}
							}
							else
							{
								dataRow8["持仓盈亏"] = (double.Parse(dataRow8["持仓均价"].ToString()) * (double)int.Parse(dataRow8["总持仓"].ToString()) * (double)num4).ToString("F0");
							}
							dataRow8["空头占用保证金"] = ((dataRow8["买卖"].ToString() == "买") ? "0" : (System.Convert.ToDouble(dataRow8["持仓均价"].ToString()) * (double)System.Convert.ToInt32(dataRow8["总持仓"].ToString()) * (double)num4 * num3).ToString());
							dataRow8["多头占用保证金"] = ((dataRow8["买卖"].ToString() == "买") ? (System.Convert.ToDouble(dataRow8["持仓均价"].ToString()) * (double)System.Convert.ToInt32(dataRow8["总持仓"].ToString()) * (double)num4 * num3).ToString() : "0");
							if (dataRow8["昨仓"].ToString() == "0" && dataRow8["今仓"].ToString() == "0")
							{
								this.ds_GroupPosition.Tables[field.InvestorID].Rows.Remove(dataRow8);
							}
						}
						object obj2 = this.ds_GroupPosition.Tables[field.InvestorID].Compute("Sum(持仓盈亏)", "true");
						dataRow6["动态权益"] = (double.Parse(dataRow6["动态权益"].ToString()) + double.Parse((obj2.ToString() == "") ? "0" : obj2.ToString()) - double.Parse(dataRow6["持仓盈亏"].ToString())).ToString("F0");
						dataRow6["持仓盈亏"] = ((obj2.ToString() == "") ? "0" : ((double)obj2).ToString("F0"));
						object obj3 = this.ds_GroupPosition.Tables[field.InvestorID].Compute("Sum(空头占用保证金)+Sum(多头占用保证金)", "true");
						dataRow6["占用保证金"] = ((obj3.ToString() == "") ? "0" : obj3.ToString());
						this.UpdateIFPosition(field.InvestorID, flag, field.InstrumentID);
					}
					catch (System.Exception ex)
					{
						System.Windows.Forms.MessageBox.Show("成交回报更新持仓" + ex.StackTrace + ex.Message);
					}
				}
			}
			else if (oak.Object.GetType() == typeof(ErrOrderField))
			{
				try
				{
					ErrOrderField field = (ErrOrderField)oak.Object;
					if (this.Dic_TradeApi[field.InvestorID].Group == "子帐户")
					{
						string[] array = oak.Key.Split(new string[]
						{
							";"
						}, System.StringSplitOptions.RemoveEmptyEntries);
						string[] array5 = array[0].Split(new string[]
						{
							","
						}, System.StringSplitOptions.RemoveEmptyEntries);
						System.Data.DataView dataView = new System.Data.DataView(this.dt_AllTrade);
						dataView.RowFilter = string.Concat(new string[]
						{
							"投资者='",
							field.InvestorID,
							"'and 序列号='",
							field.OrderRef,
							"'and 前置编号='",
							array5[0],
							"' and 会话编号='",
							array5[1],
							"'"
						});
						if (dataView.Count == 0)
						{
							System.Data.DataRow dataRow = this.dt_AllTrade.NewRow();
							dataRow["投资者"] = field.InvestorID;
							dataRow["编号"] = "";
							dataRow["合约"] = field.InstrumentID;
							dataRow["买卖"] = ((field.Direction == DirectionType.Buy) ? "买" : "    卖");
							dataRow["开平"] = ((field.Offset == EOffsetType.Open) ? "开仓" : ((field.Offset == EOffsetType.Close) ? "平仓" : "平今"));
							dataRow["状态"] = "错误";
							dataRow["价格"] = field.LimitPrice.ToString();
							dataRow["报单手数"] = field.VolumeTotalOriginal.ToString();
							dataRow["未成交"] = field.VolumeTotalOriginal.ToString();
							dataRow["成交手数"] = "0";
							dataRow["报单时间"] = System.DateTime.Now.ToString("HH:mm:ss");
							dataRow["成交时间"] = "00:00:00";
							dataRow["成交均价"] = "0";
							dataRow["详细状态"] = array[1];
							dataRow["客户信息"] = "";
							dataRow["序列号"] = field.OrderRef;
							lock (this.AllTradeLock)
							{
								this.dt_AllTrade.Rows.InsertAt(dataRow, 0);
							}
							if (this.Dic_SeparateTrade.ContainsKey(field.InvestorID))
							{
								MSOrderMode mSOrderMode = this.Dic_SeparateTrade[field.InvestorID].FirstOrDefault((MSOrderMode x) => x.子序列号 == field.OrderRef && x.子前置编号 == this.Dic_TradeApi[field.InvestorID].FrontID.ToString() && x.子会话编号 == this.Dic_TradeApi[field.InvestorID].SessionID.ToString());
								if (mSOrderMode != null)
								{
									mSOrderMode.子状态 = "错误";
									mSOrderMode.成交手数 = 0;
								}
							}
						}
						else
						{
							dataView[0].BeginEdit();
							dataView[0]["状态"] = "错误";
							dataView[0]["价格"] = field.LimitPrice.ToString();
							dataView[0]["报单手数"] = field.VolumeTotalOriginal.ToString();
							dataView[0]["未成交"] = field.VolumeTotalOriginal.ToString();
							dataView[0]["成交手数"] = "0";
							dataView[0]["报单时间"] = System.DateTime.Now.ToString("HH:mm:ss");
							dataView[0]["成交时间"] = "00:00:00";
							dataView[0]["成交均价"] = "0";
							dataView[0]["详细状态"] = array[1];
							dataView[0]["客户信息"] = "";
							dataView[0]["序列号"] = field.OrderRef;
							dataView[0].EndEdit();
						}
					}
				}
				catch (System.Exception ex)
				{
					System.Windows.Forms.MessageBox.Show("报单录入错误应答" + ex.StackTrace + ex.Message);
				}
			}
			else if (oak.Object.GetType() == typeof(InstrumentMarginRateField))
			{
				try
				{
					InstrumentMarginRateField instrumentMarginRateField = (InstrumentMarginRateField)oak.Object;
					int num4 = (int)this.Dic_TradeApi[instrumentMarginRateField.InvestorID].dtInstruments.Rows.Find(instrumentMarginRateField.InstrumentID)["合约数量"];
					bool flag = this.Dic_TradeApi[instrumentMarginRateField.InvestorID].Group == "主帐户";
					System.Data.DataRow[] array2 = this.ds_GroupPosition.Tables[instrumentMarginRateField.InvestorID].Select("合约='" + instrumentMarginRateField.InstrumentID + "'");
					for (int i = 0; i < array2.Length; i++)
					{
						array2[i]["空头占用保证金"] = ((array2[i]["买卖"].ToString() == "买") ? "0" : (System.Convert.ToDouble(array2[i]["持仓均价"].ToString()) * (double)System.Convert.ToInt32(array2[i]["总持仓"].ToString()) * (double)num4 * instrumentMarginRateField.ShortMarginRatioByMoney).ToString());
						array2[i]["多头占用保证金"] = ((array2[i]["买卖"].ToString() == "买") ? (System.Convert.ToDouble(array2[i]["持仓均价"].ToString()) * (double)System.Convert.ToInt32(array2[i]["总持仓"].ToString()) * (double)num4 * instrumentMarginRateField.LongMarginRatioByMoney).ToString() : "0");
					}
					System.Data.DataRow dataRow6;
					if (!flag)
					{
						dataRow6 = this.dt_account_dg.Select("投资者帐户='" + instrumentMarginRateField.InvestorID + "'").FirstOrDefault<System.Data.DataRow>();
					}
					else
					{
						dataRow6 = this.dt_mainaccount_dg.Select("投资者帐户='" + instrumentMarginRateField.InvestorID + "'").FirstOrDefault<System.Data.DataRow>();
					}
					object obj2 = this.ds_GroupPosition.Tables[instrumentMarginRateField.InvestorID].Compute("Sum(空头占用保证金)+Sum(多头占用保证金)", "true");
					dataRow6["占用保证金"] = ((obj2.ToString() == "") ? "0" : obj2.ToString());
				}
				catch (System.Exception ex)
				{
					System.Windows.Forms.MessageBox.Show("保证金查询返回" + ex.StackTrace + ex.Message);
				}
			}
			else if (oak.Object.GetType() == typeof(string))
			{
				string comfirmmsg = oak.Object.ToString();
				this.SetComfirmmsg(comfirmmsg);
			}
		}

		private void showMsgInListView(string[] oak)
		{
			try
			{
				if (oak.Length == 2 && oak[0] != "" && this.Dic_TradeApi.ContainsKey(oak[0]))
				{
					bool flag = this.Dic_TradeApi[oak[0]].Group == "主帐户";
					System.Data.DataRow dataRow;
					if (!flag)
					{
						dataRow = this.dt_account_dg.Select("投资者帐户='" + oak[0] + "'").FirstOrDefault<System.Data.DataRow>();
					}
					else
					{
						dataRow = this.dt_mainaccount_dg.Select("投资者帐户='" + oak[0] + "'").FirstOrDefault<System.Data.DataRow>();
					}
					if (dataRow != null)
					{
						dataRow["登录状态"] = oak[1];
						this.Dic_TradeApi[oak[0]].LoginState = oak[1];
						if (!flag && oak[1] == "已登录")
						{
							this.listprintmsg.Add(new string[]
							{
								"0",
								oak[0] + ":已登录"
							});
						}
						if (!flag && this.firstloginaccount)
						{
							if (oak[1] == "已登录")
							{
								foreach (string current in this.NeedAddMarket)
								{
									this.ADDSubMarketData(current);
								}
								this.NeedAddMarket.Clear();
								this.SelectCodeName(oak[0]);
								string[] array = new string[this.Dic_TradeApi[oak[0]].dtInstruments.Rows.Count];
								for (int i = 0; i < this.Dic_TradeApi[oak[0]].dtInstruments.Rows.Count; i++)
								{
									array[i] = this.Dic_TradeApi[oak[0]].dtInstruments.Rows[i]["合约"].ToString();
									this.comboBoxInstrument.Properties.Items.Add(array[i]);
									System.Data.DataRow dataRow2;
									if ((dataRow2 = this.dtMarketData.Rows.Find(array[i])) != null)
									{
										dataRow2["名称"] = this.Dic_TradeApi[oak[0]].dtInstruments.Rows[i]["名称"].ToString();
										dataRow2["交易所"] = this.Dic_TradeApi[oak[0]].dtInstruments.Rows[i]["交易所"].ToString();
										dataRow2["合约数量"] = this.Dic_TradeApi[oak[0]].dtInstruments.Rows[i]["合约数量"];
										dataRow2["最小波动"] = this.Dic_TradeApi[oak[0]].dtInstruments.Rows[i]["最小波动"];
									}
								}
								this.firstloginaccount = false;
							}
						}
						if (oak[1] == "网络读失败中断" || oak[1] == "网络写失败中断" || oak[1] == "接收心跳超时" || oak[1] == "发送心跳失败" || oak[1] == "收到错误报文" || oak[1] == "断开连接")
						{
							this.listprintmsg.Add(new string[]
							{
								"0",
								oak[0] + ":" + oak[1]
							});
							try
							{
								this.ds_GroupPosition.Tables[oak[0]].Clear();
								this.ds_GroupPositionDetail.Tables[oak[0]].Clear();
							}
							catch (System.Exception ex)
							{
								System.Windows.Forms.MessageBox.Show(ex.StackTrace + ex.ToString());
							}
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				System.Windows.Forms.MessageBox.Show(ex.StackTrace + ex.ToString());
			}
		}

		private void showMdMsg(object oak)
		{
			try
			{
				if (oak.GetType() == typeof(string))
				{
					string[] array = oak.ToString().Split(new string[]
					{
						"@"
					}, System.StringSplitOptions.RemoveEmptyEntries);
					if (array[0] == "行情登录成功")
					{
						this.listprintmsg.Add(new string[]
						{
							"0",
							"行情登录成功..."
						});
						this.toolTip1.SetToolTip(this.radioButtonMd, "行情连接正常");
						this.radioButtonMd.ForeColor = System.Drawing.Color.Green;
						this.radioButtonMd.Checked = true;
					}
					else if (array[0] == "行情断开")
					{
						this.listprintmsg.Add(new string[]
						{
							"0",
							"行情断开..."
						});
						this.toolTip1.SetToolTip(this.radioButtonMd, "行情断开");
						this.radioButtonMd.ForeColor = System.Drawing.Color.Red;
						this.radioButtonMd.Checked = false;
					}
					else if (array[0] == "开始订阅行情")
					{
						for (int i = 0; i < this.dtMarketData.Rows.Count; i++)
						{
							this.mdApi.SubMarketData(new string[]
							{
								this.dtMarketData.Rows[i]["合约"].ToString()
							});
						}
					}
					else
					{
						this.comboBoxErrMsg.Items.Insert(0, System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss.fff") + "|" + (string)oak);
						this.comboBoxErrMsg.SelectedIndex = 0;
					}
				}
				else if (oak.GetType() == typeof(CThostFtdcDepthMarketDataField))
				{
					CThostFtdcDepthMarketDataField item = (CThostFtdcDepthMarketDataField)oak;
					this.listMarketDatas.Add(item);
				}
			}
			catch (System.Exception ex)
			{
				System.Windows.Forms.MessageBox.Show(ex.StackTrace + ex.Message);
			}
		}

		private void App_Answer(TradeApi.ObjectAndKey key)
		{
			base.Invoke(new System.Action<TradeApi.ObjectAndKey>(this.showStructInListView), new object[]
			{
				key
			});
		}

		private void App_Msg(object key)
		{
			base.BeginInvoke(new System.Action<string[]>(this.showMsgInListView), new object[]
			{
				key
			});
		}

		private void App_MdMsg(object key)
		{
			base.BeginInvoke(new System.Action<object>(this.showMdMsg), new object[]
			{
				key
			});
		}

		private void SelectCodeName(string ID)
		{
			System.Data.DataView dataView = new System.Data.DataView(this.dt_AccountTradeSet);
			System.Data.DataTable dataTable = dataView.ToTable(true, new string[]
			{
				"品种"
			});
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				string text = dataTable.Rows[i]["品种"].ToString();
				bool flag = false;
				for (int j = 0; j < text.Length; j++)
				{
					if (char.IsNumber(text, j))
					{
						flag = true;
						break;
					}
				}
				if (flag)
				{
					if (!this.HasAddMarket.Contains(text))
					{
						this.ADDSubMarketData(text);
					}
				}
				else if (text != "*")
				{
					System.Data.DataRow[] array = this.Dic_TradeApi[ID].dtInstruments.Select("合约 like '" + text + "%'");
					for (int j = 0; j < array.Length; j++)
					{
						if (!this.HasAddMarket.Contains(array[j]["合约"].ToString()))
						{
							this.ADDSubMarketData(array[j]["合约"].ToString());
						}
					}
				}
			}
		}

		private void ADDSubMarketData(string Instrument)
		{
			string text = "";
			try
			{
				System.Data.DataRow dataRow = this.dtMarketData.Rows.Find(Instrument);
				if (dataRow == null)
				{
					for (int i = 0; i < this.dt_account_dg.Rows.Count; i++)
					{
						if (this.dt_account_dg.Rows[i]["登录状态"].ToString() == "已登录")
						{
							text = this.dt_account_dg.Rows[i]["投资者帐户"].ToString();
							break;
						}
					}
					if (text == "")
					{
						if (!this.NeedAddMarket.Contains(Instrument))
						{
							this.NeedAddMarket.Add(Instrument);
						}
					}
					System.Data.DataRow dataRow2;
					if (text != "" && (dataRow2 = this.Dic_TradeApi[text].dtInstruments.Rows.Find(Instrument)) != null)
					{
						this.dtMarketData.Rows.Add(new object[]
						{
							Instrument,
							dataRow2["名称"],
							dataRow2["交易所"],
							0,
							0,
							0,
							0,
							0,
							0,
							0,
							0,
							0,
							0,
							0,
							0,
							0,
							0,
							0,
							0,
							0,
							0,
							"",
							0,
							false,
							false,
							dataRow2["合约数量"],
							dataRow2["最小波动"]
						});
						this.mdApi.SubMarketData(new string[]
						{
							Instrument
						});
						this.HasAddMarket.Add(Instrument);
					}
				}
			}
			catch (System.Exception ex)
			{
				System.Windows.Forms.MessageBox.Show(text + ex.StackTrace + ex.ToString());
			}
		}

		private void freshMarketData()
		{
			while (true)
			{
				if (this.listMarketDatas.Count == 0)
				{
					System.Threading.Thread.Sleep(1);
				}
				else
				{
					try
					{
						CThostFtdcDepthMarketDataField cThostFtdcDepthMarketDataField = this.listMarketDatas[0];
						System.Data.DataRow dataRow = this.dtMarketData.Rows.Find(cThostFtdcDepthMarketDataField.InstrumentID);
						if (dataRow == null)
						{
							this.listMarketDatas.Remove(cThostFtdcDepthMarketDataField);
						}
						else
						{
							dataRow["最新价"] = cThostFtdcDepthMarketDataField.LastPrice;
							dataRow["涨跌"] = cThostFtdcDepthMarketDataField.LastPrice - cThostFtdcDepthMarketDataField.PreSettlementPrice;
							dataRow["涨幅"] = (cThostFtdcDepthMarketDataField.LastPrice - cThostFtdcDepthMarketDataField.PreSettlementPrice) / cThostFtdcDepthMarketDataField.PreSettlementPrice * 100.0;
							dataRow["现手"] = cThostFtdcDepthMarketDataField.Volume - (int)dataRow["总手"];
							dataRow["总手"] = cThostFtdcDepthMarketDataField.Volume;
							dataRow["仓差"] = cThostFtdcDepthMarketDataField.OpenInterest - cThostFtdcDepthMarketDataField.PreOpenInterest;
							if (cThostFtdcDepthMarketDataField.OpenInterest != -1.7976931348623157E+308 && cThostFtdcDepthMarketDataField.OpenInterest != 1.7976931348623157E+308)
							{
								dataRow["持仓"] = cThostFtdcDepthMarketDataField.OpenInterest;
							}
							else
							{
								dataRow["持仓"] = 0;
							}
							if (cThostFtdcDepthMarketDataField.LastPrice != -1.7976931348623157E+308 && cThostFtdcDepthMarketDataField.LastPrice != 1.7976931348623157E+308)
							{
								dataRow["最新价"] = cThostFtdcDepthMarketDataField.LastPrice;
							}
							else
							{
								dataRow["最新价"] = 0;
							}
							if (cThostFtdcDepthMarketDataField.BidPrice1 != -1.7976931348623157E+308 && cThostFtdcDepthMarketDataField.BidPrice1 != 1.7976931348623157E+308)
							{
								dataRow["买价"] = cThostFtdcDepthMarketDataField.BidPrice1;
							}
							else
							{
								dataRow["买价"] = dataRow["最新价"];
							}
							dataRow["买量"] = cThostFtdcDepthMarketDataField.BidVolume1;
							if (cThostFtdcDepthMarketDataField.AskPrice1 != -1.7976931348623157E+308 && cThostFtdcDepthMarketDataField.AskPrice1 != 1.7976931348623157E+308)
							{
								dataRow["卖价"] = cThostFtdcDepthMarketDataField.AskPrice1;
							}
							else
							{
								dataRow["卖价"] = dataRow["最新价"];
							}
							dataRow["卖量"] = cThostFtdcDepthMarketDataField.AskVolume1;
							if (cThostFtdcDepthMarketDataField.HighestPrice != -1.7976931348623157E+308 && cThostFtdcDepthMarketDataField.HighestPrice != 1.7976931348623157E+308)
							{
								dataRow["最高"] = cThostFtdcDepthMarketDataField.HighestPrice;
							}
							if (cThostFtdcDepthMarketDataField.LowestPrice != -1.7976931348623157E+308 && cThostFtdcDepthMarketDataField.LowestPrice != 1.7976931348623157E+308)
							{
								dataRow["最低"] = cThostFtdcDepthMarketDataField.LowestPrice;
							}
							if (cThostFtdcDepthMarketDataField.UpperLimitPrice != -1.7976931348623157E+308 && cThostFtdcDepthMarketDataField.UpperLimitPrice != 1.7976931348623157E+308)
							{
								dataRow["涨停"] = cThostFtdcDepthMarketDataField.UpperLimitPrice;
							}
							if (cThostFtdcDepthMarketDataField.LowerLimitPrice != -1.7976931348623157E+308 && cThostFtdcDepthMarketDataField.LowerLimitPrice != 1.7976931348623157E+308)
							{
								dataRow["跌停"] = cThostFtdcDepthMarketDataField.LowerLimitPrice;
							}
							if (cThostFtdcDepthMarketDataField.OpenPrice != -1.7976931348623157E+308 && cThostFtdcDepthMarketDataField.OpenPrice != 1.7976931348623157E+308)
							{
								dataRow["开盘"] = cThostFtdcDepthMarketDataField.OpenPrice;
							}
							if (cThostFtdcDepthMarketDataField.PreSettlementPrice != -1.7976931348623157E+308 && cThostFtdcDepthMarketDataField.PreSettlementPrice != 1.7976931348623157E+308)
							{
								dataRow["昨结"] = cThostFtdcDepthMarketDataField.PreSettlementPrice;
							}
							dataRow["时间"] = cThostFtdcDepthMarketDataField.UpdateTime;
							System.TimeSpan t = System.DateTime.Now.TimeOfDay;
							try
							{
								t = System.TimeSpan.Parse(cThostFtdcDepthMarketDataField.UpdateTime + "." + cThostFtdcDepthMarketDataField.UpdateMillisec);
							}
							catch
							{
							}
							dataRow["时间差"] = (t - System.DateTime.Now.TimeOfDay).TotalSeconds;
							base.BeginInvoke(new Action<System.Data.DataRow, CThostFtdcDepthMarketDataField>(this.freshOrderRange), new object[]
							{
								dataRow,
								cThostFtdcDepthMarketDataField
							});
							this.listMarketDatas.Remove(cThostFtdcDepthMarketDataField);
						}
					}
					catch (System.Exception var_3_53B)
					{
					}
				}
			}
		}

		private void freshOrderRange(System.Data.DataRow dr, CThostFtdcDepthMarketDataField pDepthMarketData)
		{
			try
			{
				if (pDepthMarketData.LastPrice > 0.0)
				{
					for (int i = 0; i < this.ds_GroupPosition.Tables.Count; i++)
					{
						if (this.ds_GroupPosition.Tables[i].Rows.Count > 0)
						{
							System.Data.DataRow[] array = this.ds_GroupPosition.Tables[i].Select("合约='" + pDepthMarketData.InstrumentID + "'");
							for (int j = 0; j < array.Length; j++)
							{
								if (!double.IsInfinity(pDepthMarketData.LastPrice))
								{
									if (array[j]["买卖"].ToString() != "买")
									{
										array[j]["持仓盈亏"] = ((double.Parse(array[j]["持仓均价"].ToString()) - pDepthMarketData.LastPrice) * (double)int.Parse(array[j]["总持仓"].ToString()) * (double)((int)this.dtMarketData.Rows.Find(pDepthMarketData.InstrumentID)["合约数量"])).ToString("F0");
									}
									else
									{
										array[j]["持仓盈亏"] = ((pDepthMarketData.LastPrice - double.Parse(array[j]["持仓均价"].ToString())) * (double)int.Parse(array[j]["总持仓"].ToString()) * (double)((int)this.dtMarketData.Rows.Find(pDepthMarketData.InstrumentID)["合约数量"])).ToString("F0");
									}
								}
							}
							object obj = this.ds_GroupPosition.Tables[i].Compute("Sum(持仓盈亏)", "true");
							System.Data.DataRow dataRow;
							if (this.Dic_TradeApi[this.ds_GroupPosition.Tables[i].TableName].Group == "子帐户")
							{
								dataRow = this.dt_account_dg.Select("投资者帐户='" + this.ds_GroupPosition.Tables[i].TableName + "'").FirstOrDefault<System.Data.DataRow>();
							}
							else
							{
								dataRow = this.dt_mainaccount_dg.Select("投资者帐户='" + this.ds_GroupPosition.Tables[i].TableName + "'").FirstOrDefault<System.Data.DataRow>();
							}
							dataRow["动态权益"] = (double.Parse(dataRow["动态权益"].ToString()) + double.Parse(obj.ToString()) - double.Parse(dataRow["持仓盈亏"].ToString())).ToString("F0");
							dataRow["持仓盈亏"] = ((double)obj).ToString("F0");
						}
					}
				}
				if (this.freshOrderPrice && this.comboBoxInstrument.Text == pDepthMarketData.InstrumentID && pDepthMarketData.LastPrice > 0.0)
				{
					this.numericUpDownPrice.Minimum = (decimal)pDepthMarketData.LowerLimitPrice;
					this.numericUpDownPrice.Maximum = (decimal)pDepthMarketData.UpperLimitPrice;
					if (this.comboBoxDirector.SelectedIndex == 0)
					{
						this.numericUpDownPrice.Value = (decimal)((double)dr["卖价"]);
					}
					else
					{
						this.numericUpDownPrice.Value = (decimal)((double)dr["买价"]);
					}
					this.numericUpDownPrice.Increment = (decimal)((double)this.dtMarketData.Rows.Find(pDepthMarketData.InstrumentID)["最小波动"]);
					this.labelUpper.Text = dr["涨停"].ToString();
					this.labelLower.Text = dr["跌停"].ToString();
				}
			}
			catch (System.Exception var_5_47E)
			{
			}
		}

		public void returnform(System.Windows.Forms.Form cf)
		{
			cf.TopLevel = false;
			cf.Dock = System.Windows.Forms.DockStyle.Fill;
			cf.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			if (this.Dic_TradeApi[cf.Name.Substring(8)].Group == "子帐户")
			{
				this.tab.TabPages.Add(cf.Text, cf.Text, "");
				this.tab.TabPages[cf.Text].Controls.Add(cf);
				if (this.spcPosition.Collapsed)
				{
					this.spcPosition.Collapsed = false;
				}
			}
			else
			{
				this.tabMain.TabPages.Add(cf.Text, cf.Text, "");
				this.tabMain.TabPages[cf.Text].Controls.Add(cf);
				if (this.spcMainPosition.Collapsed)
				{
					this.spcMainPosition.Collapsed = false;
				}
			}
		}

		private void tab_DoubleClick(object sender, System.EventArgs e)
		{
			if (this.tab.TabPages.Count == 1)
			{
				this.spcPosition.Collapsed = true;
			}
		}

		private void tabMain_DoubleClick(object sender, System.EventArgs e)
		{
			if (this.tabMain.TabPages.Count == 1)
			{
				this.spcMainPosition.Collapsed = true;
			}
		}

		private void butLoginOut_Click(object sender, System.EventArgs e)
		{
			this.LoginOutAccount(false);
		}

		private void butMainLoginOut_Click(object sender, System.EventArgs e)
		{
			this.LoginOutAccount(true);
		}

		private void LoginOutAccount(bool IsMain)
		{
			System.Data.DataRow[] array;
			if (!IsMain)
			{
				array = this.dt_account_dg.Select("选择='True'");
			}
			else
			{
				array = this.dt_mainaccount_dg.Select("选择='True'");
			}
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i]["投资者帐户"].ToString();
				if (array[i]["登录状态"].ToString() == "已登录")
				{
					try
					{
						this.Dic_TradeApi[text].IsLogin = false;
						this.Dic_TradeApi[text].listQry.Clear();
						while (this.Dic_TradeApi[text].apiIsBusy)
						{
							System.Threading.Thread.Sleep(10);
						}
						this.Dic_TradeApi[text].ReqDisConnect();
						this.ds_GroupPosition.Tables[text].Clear();
						this.ds_GroupPositionDetail.Tables[text].Clear();
						if (this.Dic_TradeApi[text].apiType == 1 || this.Dic_TradeApi[text].apiType == 2)
						{
							array[i]["登录状态"] = "断开连接";
						}
						array[i]["动态权益"] = "0";
						array[i]["平仓盈亏"] = "0";
						array[i]["持仓盈亏"] = "0";
						array[i]["IF持仓"] = "0/0";
						array[i]["IC持仓"] = "0/0";
						array[i]["IH持仓"] = "0/0";
						array[i]["au持仓"] = "0/0";
						array[i]["ag持仓"] = "0/0";
					}
					catch (System.Exception ex)
					{
						System.Windows.Forms.MessageBox.Show(ex.StackTrace + ex.Message);
					}
				}
			}
		}

		private void butLogin_Click(object sender, System.EventArgs e)
		{
			this.LoginAccount(false);
		}

		private void butMainLogin_Click(object sender, System.EventArgs e)
		{
			this.LoginAccount(true);
		}

		private void LoginAccount(bool IsMain)
		{
			try
			{
				System.Data.DataRow[] array;
				if (!IsMain)
				{
					array = this.dt_account_dg.Select("选择='True'");
				}
				else
				{
					array = this.dt_mainaccount_dg.Select("选择='True'");
				}
				for (int i = 0; i < array.Length; i++)
				{
					string text = array[i]["投资者帐户"].ToString();
					if (array[i]["登录状态"].ToString() == "未登录")
					{
						if (!this.tmMoneyRefresh.Enabled)
						{
							this.tmMoneyRefresh.Enabled = true;
							this.tmMoneyRefresh.Start();
						}
						this.Dic_TradeApi[text].OnAnswer += new TradeApi.TAAnswer(this.App_Answer);
						this.Dic_TradeApi[text].OnMsg += new TradeApi.TAMsg(this.App_Msg);
						this.Dic_TradeApi[text].ReqConnect();
						array[i]["登录状态"] = "连接中...";
						if (!IsMain && i == 0 && this.mdApi == null)
						{
							this.butMainLogin.Enabled = true;
							this.mdApi = new MdApi(text, this.Dic_TradeApi[text].passWord, this.Dic_TradeApi[text].BrokerID, this.Dic_TradeApi[text].MFrontAddr, this.Dic_TradeApi[text].apiType, false);
							this.mdApi.OnMdMsg += new MdApi.TAMdMsg(this.App_MdMsg);
							this.mdApi.Login();
							this.threadFreshMarketData = new System.Threading.Thread(new System.Threading.ThreadStart(this.freshMarketData));
							this.threadFreshMarketData.Start();
						}
					}
					else if (array[i]["登录状态"].ToString() == "断开连接")
					{
						this.Dic_TradeApi[text].ReqConnect();
						array[i]["登录状态"] = "连接中...";
						if (!IsMain && i == 0 && this.mdApi == null)
						{
							this.butMainLogin.Enabled = true;
							this.threadFreshMarketData = new System.Threading.Thread(new System.Threading.ThreadStart(this.freshMarketData));
							this.threadFreshMarketData.Start();
						}
					}
				}
			}
			catch (System.Exception var_3_27B)
			{
			}
		}

		private void butSelAll_Click(object sender, System.EventArgs e)
		{
			if (this.butSelAll.Text == "全选")
			{
				for (int i = 0; i < this.dt_account_dg.Rows.Count; i++)
				{
					if (this.dt_account_dg.Rows[i]["选择"].ToString() == "False" || this.dt_account_dg.Rows[i]["选择"].ToString() == "")
					{
						this.dt_account_dg.Rows[i]["选择"] = "True";
						this.butSelAll.Text = "全清";
					}
				}
			}
			else
			{
				for (int i = 0; i < this.dt_account_dg.Rows.Count; i++)
				{
					if (this.dt_account_dg.Rows[i]["选择"].ToString() == "True")
					{
						this.dt_account_dg.Rows[i]["选择"] = "False";
						this.butSelAll.Text = "全选";
					}
				}
			}
		}

		private void btnMainSelAll_Click(object sender, System.EventArgs e)
		{
			if (this.btnMainSelAll.Text == "全选")
			{
				for (int i = 0; i < this.dt_mainaccount_dg.Rows.Count; i++)
				{
					if (this.dt_mainaccount_dg.Rows[i]["选择"].ToString() == "False" || this.dt_mainaccount_dg.Rows[i]["选择"].ToString() == "")
					{
						this.dt_mainaccount_dg.Rows[i]["选择"] = "True";
						this.btnMainSelAll.Text = "全清";
					}
				}
			}
			else
			{
				for (int i = 0; i < this.dt_mainaccount_dg.Rows.Count; i++)
				{
					if (this.dt_mainaccount_dg.Rows[i]["选择"].ToString() == "True")
					{
						this.dt_mainaccount_dg.Rows[i]["选择"] = "False";
						this.btnMainSelAll.Text = "全选";
					}
				}
			}
		}

		private void buttonOrder_Click(object sender, System.EventArgs e)
		{
			string text = this.comboBoxInstrument.Text;
			if (!string.IsNullOrWhiteSpace(text))
			{
				EnumDirectionType enumDirectionType = (this.comboBoxDirector.SelectedIndex == 0) ? EnumDirectionType.Buy : EnumDirectionType.Sell;
				EnumOffsetFlagType enumOffsetFlagType = (this.comboBoxOffset.SelectedIndex == 0) ? EnumOffsetFlagType.Open : ((this.comboBoxOffset.SelectedIndex == 2) ? EnumOffsetFlagType.Close : EnumOffsetFlagType.CloseToday);
				double num = (this.comboBoxDirector.SelectedIndex == 0) ? ((double)this.numericUpDownPrice.Value) : ((double)this.numericUpDownPrice.Value);
				int num2 = (int)this.numericUpDownVolume.Value;
				if (num2 > 0)
				{
					System.Data.DataRow[] array = this.dt_account_dg.Select("选择='True'");
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i]["登录状态"].ToString() == "已登录")
						{
							string key = array[i]["投资者帐户"].ToString();
							if (this.freshOrderPrice)
							{
								this.Dic_TradeApi[key].listTransDatas.Add(new string[]
								{
									text,
									(enumDirectionType == EnumDirectionType.Buy) ? "Buy" : "Sell",
									(enumOffsetFlagType == EnumOffsetFlagType.Open) ? "open" : ((enumOffsetFlagType == EnumOffsetFlagType.Close) ? "close" : "closetoday"),
									num.ToString(),
									num2.ToString(),
									this.Dic_TradeApi[key].UpOrderRef().ToString()
								});
							}
							else
							{
								this.Dic_TradeApi[key].listTransDatas.Add(new string[]
								{
									text,
									(enumDirectionType == EnumDirectionType.Buy) ? "Buy" : "Sell",
									(enumOffsetFlagType == EnumOffsetFlagType.Open) ? "open" : ((enumOffsetFlagType == EnumOffsetFlagType.Close) ? "close" : "closetoday"),
									num.ToString(),
									num2.ToString(),
									this.Dic_TradeApi[key].UpOrderRef().ToString()
								});
							}
						}
					}
				}
			}
		}

		private void buttonMarketPrice_Click(object sender, System.EventArgs e)
		{
			string text = this.comboBoxInstrument.Text;
			if (!string.IsNullOrWhiteSpace(text))
			{
				EnumDirectionType enumDirectionType = (this.comboBoxDirector.SelectedIndex == 0) ? EnumDirectionType.Buy : EnumDirectionType.Sell;
				EnumOffsetFlagType enumOffsetFlagType = (this.comboBoxOffset.SelectedIndex == 0) ? EnumOffsetFlagType.Open : ((this.comboBoxOffset.SelectedIndex == 2) ? EnumOffsetFlagType.Close : EnumOffsetFlagType.CloseToday);
				double num = (this.comboBoxDirector.SelectedIndex == 0) ? System.Convert.ToDouble(this.labelUpper.Text) : System.Convert.ToDouble(this.labelLower.Text);
				int num2 = (int)this.numericUpDownVolume.Value;
				if (num2 > 0)
				{
					System.Data.DataRow[] array = this.dt_account_dg.Select("选择='True'");
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i]["登录状态"].ToString() == "已登录")
						{
							string key = array[i]["投资者帐户"].ToString();
							this.Dic_TradeApi[key].listTransDatas.Add(new string[]
							{
								text,
								(enumDirectionType == EnumDirectionType.Buy) ? "Buy" : "Sell",
								(enumOffsetFlagType == EnumOffsetFlagType.Open) ? "open" : ((enumOffsetFlagType == EnumOffsetFlagType.Close) ? "close" : "closetoday"),
								num.ToString(),
								num2.ToString(),
								this.Dic_TradeApi[key].UpOrderRef().ToString()
							});
						}
					}
				}
			}
		}

		private void Revoke_Click(object sender, System.EventArgs e)
		{
			System.Data.DataRow dataRow = this.gvTrade.GetDataRow(this.gvTrade.FocusedRowHandle);
			if (dataRow["状态"].ToString() != "全部成交" && dataRow["状态"].ToString() != "已撤单" && dataRow["状态"].ToString() != "错误")
			{
				this.Dic_TradeApi[dataRow["投资者"].ToString()].ReqOrderAction(dataRow["合约"].ToString(), int.Parse(dataRow["前置编号"].ToString()), int.Parse(dataRow["会话编号"].ToString()), dataRow["序列号"].ToString());
			}
		}

		private void FullRevoke_Click(object sender, System.EventArgs e)
		{
			System.Data.DataView dataView = new System.Data.DataView(this.dt_AllTrade);
			dataView.RowFilter = "状态<> '全部成交' and 状态 <> '已撤单' and 状态 <> '错误'";
			for (int i = 0; i < dataView.Count; i++)
			{
				this.Dic_TradeApi[dataView[i]["投资者"].ToString()].ReqOrderAction(dataView[i]["合约"].ToString(), int.Parse(dataView[i]["前置编号"].ToString()), int.Parse(dataView[i]["会话编号"].ToString()), dataView[i]["序列号"].ToString());
			}
		}

		protected override bool ProcessDialogKey(System.Windows.Forms.Keys keyData)
		{
			bool result;
			if (keyData == System.Windows.Forms.Keys.Return)
			{
				if (base.ActiveControl != null)
				{
					if (this.comboBoxInstrument.IsEditorActive)
					{
						this.comboBoxDirector.Focus();
					}
					else if (this.comboBoxDirector.Focused)
					{
						this.comboBoxOffset.Focus();
					}
					else if (this.comboBoxOffset.Focused)
					{
						this.numericUpDownVolume.Focus();
						this.numericUpDownVolume.Select(0, this.numericUpDownVolume.ToString().Length);
					}
					else if (this.numericUpDownVolume.Focused)
					{
						this.numericUpDownPrice.Focus();
						this.numericUpDownPrice.Select(0, this.numericUpDownPrice.ToString().Length);
					}
					else if (this.numericUpDownPrice.Focused)
					{
						this.comboBoxInstrument.Focus();
					}
					else if (this.buttonOrder.Focused)
					{
						this.comboBoxInstrument.Focus();
					}
				}
				result = true;
			}
			else
			{
				result = base.ProcessDialogKey(keyData);
			}
			return result;
		}

		protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
		{
			System.Windows.Forms.Keys keys = keyData & System.Windows.Forms.Keys.KeyCode;
			bool result;
			if (keys == System.Windows.Forms.Keys.Down)
			{
				if (base.ActiveControl != null)
				{
					if (this.comboBoxInstrument.IsEditorActive)
					{
						if (this.comboBoxInstrument.IsPopupOpen)
						{
							result = base.ProcessCmdKey(ref msg, keyData);
							return result;
						}
						this.comboBoxDirector.Focus();
					}
					else if (this.comboBoxDirector.Focused)
					{
						if (this.comboBoxDirector.IsPopupOpen)
						{
							result = base.ProcessCmdKey(ref msg, keyData);
							return result;
						}
						this.comboBoxOffset.Focus();
					}
					else if (this.comboBoxOffset.Focused)
					{
						if (this.comboBoxOffset.IsPopupOpen)
						{
							result = base.ProcessCmdKey(ref msg, keyData);
							return result;
						}
						this.numericUpDownVolume.Focus();
						this.numericUpDownVolume.Select(0, this.numericUpDownVolume.ToString().Length);
					}
					else if (this.numericUpDownVolume.Focused)
					{
						this.numericUpDownPrice.Focus();
						this.numericUpDownPrice.Select(0, this.numericUpDownPrice.ToString().Length);
					}
					else if (this.numericUpDownPrice.Focused)
					{
						this.comboBoxInstrument.Focus();
					}
					else
					{
						if (!this.buttonOrder.Focused)
						{
							result = base.ProcessCmdKey(ref msg, keyData);
							return result;
						}
						this.comboBoxInstrument.Focus();
					}
				}
				result = true;
			}
			else if (keyData == System.Windows.Forms.Keys.Up)
			{
				if (base.ActiveControl != null)
				{
					if (this.comboBoxInstrument.IsEditorActive)
					{
						if (this.comboBoxInstrument.IsPopupOpen)
						{
							result = base.ProcessCmdKey(ref msg, keyData);
							return result;
						}
						this.numericUpDownPrice.Focus();
						this.numericUpDownPrice.Select(0, this.numericUpDownPrice.ToString().Length);
					}
					else if (this.comboBoxDirector.Focused)
					{
						if (this.comboBoxDirector.IsPopupOpen)
						{
							result = base.ProcessCmdKey(ref msg, keyData);
							return result;
						}
						this.comboBoxInstrument.Focus();
						this.comboBoxInstrument.Select(0, this.comboBoxInstrument.Text.Length);
					}
					else if (this.comboBoxOffset.Focused)
					{
						if (this.comboBoxOffset.IsPopupOpen)
						{
							result = base.ProcessCmdKey(ref msg, keyData);
							return result;
						}
						this.comboBoxDirector.Focus();
					}
					else if (this.numericUpDownPrice.Focused)
					{
						this.numericUpDownVolume.Focus();
						this.numericUpDownVolume.Select(0, this.numericUpDownVolume.ToString().Length);
					}
					else if (this.numericUpDownVolume.Focused)
					{
						this.comboBoxOffset.Focus();
					}
					else
					{
						if (!this.buttonOrder.Focused)
						{
							result = base.ProcessCmdKey(ref msg, keyData);
							return result;
						}
						this.numericUpDownPrice.Focus();
						this.numericUpDownPrice.Select(0, this.numericUpDownPrice.ToString().Length);
					}
				}
				result = true;
			}
			else if (keyData == System.Windows.Forms.Keys.Left)
			{
				if (this.comboBoxInstrument.IsEditorActive)
				{
					if (this.comboBoxInstrument.IsPopupOpen)
					{
						result = base.ProcessCmdKey(ref msg, keyData);
						return result;
					}
					if (this.comboBoxInstrument.SelectedIndex > 0)
					{
						this.comboBoxInstrument.SelectedIndex--;
					}
					else if (this.comboBoxInstrument.SelectedIndex == 0)
					{
						this.comboBoxInstrument.SelectedIndex = this.comboBoxInstrument.Properties.Items.Count - 1;
					}
					this.comboBoxInstrument.Select(0, this.comboBoxInstrument.Text.Length);
				}
				else if (this.comboBoxDirector.IsEditorActive)
				{
					if (this.comboBoxDirector.IsPopupOpen)
					{
						result = base.ProcessCmdKey(ref msg, keyData);
						return result;
					}
					if (this.comboBoxDirector.SelectedIndex > 0)
					{
						this.comboBoxDirector.SelectedIndex = 0;
					}
				}
				else if (this.comboBoxOffset.IsEditorActive)
				{
					if (this.comboBoxOffset.IsPopupOpen)
					{
						result = base.ProcessCmdKey(ref msg, keyData);
						return result;
					}
					if (this.comboBoxOffset.SelectedIndex > 0)
					{
						this.comboBoxOffset.SelectedIndex--;
					}
				}
				else if (this.numericUpDownVolume.Focused)
				{
					if (this.numericUpDownVolume.Value > 0m)
					{
						this.numericUpDownVolume.Value -= this.numericUpDownVolume.Increment;
					}
					this.numericUpDownVolume.Select(0, this.numericUpDownVolume.ToString().Length);
				}
				else
				{
					if (!this.numericUpDownPrice.Focused)
					{
						result = base.ProcessCmdKey(ref msg, keyData);
						return result;
					}
					if (this.numericUpDownPrice.Value >= this.numericUpDownPrice.Increment && this.numericUpDownPrice.Value <= decimal.Parse(this.labelUpper.Text) && this.numericUpDownPrice.Value > decimal.Parse(this.labelLower.Text))
					{
						this.numericUpDownPrice.Value -= this.numericUpDownPrice.Increment;
					}
					this.numericUpDownPrice.Select(0, this.numericUpDownPrice.ToString().Length);
				}
				result = true;
			}
			else if (keyData == System.Windows.Forms.Keys.Right)
			{
				if (this.comboBoxInstrument.IsEditorActive)
				{
					if (this.comboBoxInstrument.IsPopupOpen)
					{
						result = base.ProcessCmdKey(ref msg, keyData);
						return result;
					}
					if (this.comboBoxInstrument.SelectedIndex == this.comboBoxInstrument.Properties.Items.Count - 1)
					{
						this.comboBoxInstrument.SelectedIndex = 0;
					}
					else
					{
						this.comboBoxInstrument.SelectedIndex++;
					}
					this.comboBoxInstrument.Select(0, this.comboBoxInstrument.Text.Length);
				}
				else if (this.comboBoxDirector.IsEditorActive)
				{
					if (this.comboBoxDirector.IsPopupOpen)
					{
						result = base.ProcessCmdKey(ref msg, keyData);
						return result;
					}
					if (this.comboBoxDirector.SelectedIndex < 1)
					{
						this.comboBoxDirector.SelectedIndex = 1;
					}
				}
				else if (this.comboBoxOffset.IsEditorActive)
				{
					if (this.comboBoxOffset.IsPopupOpen)
					{
						result = base.ProcessCmdKey(ref msg, keyData);
						return result;
					}
					if (this.comboBoxOffset.SelectedIndex < 2)
					{
						this.comboBoxOffset.SelectedIndex++;
					}
				}
				else if (this.numericUpDownVolume.Focused)
				{
					this.numericUpDownVolume.Value += this.numericUpDownVolume.Increment;
					this.numericUpDownVolume.Select(0, this.numericUpDownVolume.ToString().Length);
				}
				else
				{
					if (!this.numericUpDownPrice.Focused)
					{
						result = base.ProcessCmdKey(ref msg, keyData);
						return result;
					}
					if (this.numericUpDownPrice.Value < decimal.Parse(this.labelUpper.Text) && this.numericUpDownPrice.Value >= decimal.Parse(this.labelLower.Text))
					{
						this.numericUpDownPrice.Value += this.numericUpDownPrice.Increment;
					}
					this.numericUpDownPrice.Select(0, this.numericUpDownPrice.ToString().Length);
				}
				result = true;
			}
			else
			{
				result = base.ProcessCmdKey(ref msg, keyData);
			}
			return result;
		}

		public void positionclick(int group, System.Data.DataRow dr_focuserow)
		{
			if (this.Dic_TradeApi.ContainsKey(dr_focuserow.Table.TableName) && this.Dic_TradeApi[dr_focuserow.Table.TableName].Group == "子帐户")
			{
				this.comboBoxInstrument.Text = dr_focuserow["合约"].ToString();
				this.orderRangeChangeInstrument(dr_focuserow["合约"].ToString(), (dr_focuserow["买卖"].ToString() == "买") ? "1" : "0", (System.Convert.ToInt32(dr_focuserow["今仓"].ToString()) > 0 && System.Convert.ToInt32(dr_focuserow["昨仓"].ToString()) > 0) ? "2" : ((System.Convert.ToInt32(dr_focuserow["今仓"].ToString()) > 0) ? "2" : "2"), (System.Convert.ToInt32(dr_focuserow["今仓"].ToString()) > 0) ? System.Convert.ToInt32(dr_focuserow["今仓"].ToString()) : System.Convert.ToInt32(dr_focuserow["昨仓"].ToString()));
			}
		}

		private void comboBoxDirector_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (this.buttonPrice.Text == "指定价" && this.labelUpper.Text != "-" && this.labelLower.Text != "-")
			{
				this.numericUpDownPrice.Value = (decimal)double.Parse((this.comboBoxDirector.SelectedIndex == 0) ? this.labelUpper.Text : this.labelLower.Text);
			}
		}

		private void orderRangeChangeInstrument(string _instrument, string _direction = null, string _offset = null, [System.Runtime.CompilerServices.DecimalConstant(0, 0, 0u, 0u, 0u)] decimal _closeLots = default(decimal))
		{
			this.comboBoxDirector.SelectedIndex = ((_direction == null) ? this.comboBoxDirector.SelectedIndex : int.Parse(_direction));
			this.comboBoxOffset.SelectedIndex = ((_offset == null) ? this.comboBoxOffset.SelectedIndex : ((int.Parse(_offset) == 1) ? 1 : 2));
			this.comboBoxInstrument.Text = _instrument;
			System.Data.DataRow dataRow = this.dtMarketData.Rows.Find(_instrument);
			if (dataRow != null)
			{
				this.labelUpper.Text = dataRow["涨停"].ToString();
				this.labelLower.Text = dataRow["跌停"].ToString();
				this.numericUpDownPrice.Maximum = decimal.Parse(dataRow["涨停"].ToString());
				this.numericUpDownPrice.Minimum = decimal.Parse(dataRow["跌停"].ToString());
				if (this.buttonPrice.Text == "跟盘价")
				{
					this.numericUpDownPrice.Value = (decimal)((double)((this.comboBoxDirector.SelectedIndex == 0) ? ((decimal.Parse(dataRow["卖价"].ToString()) > decimal.Parse(dataRow["跌停"].ToString())) ? dataRow["卖价"] : dataRow["跌停"]) : ((decimal.Parse(dataRow["买价"].ToString()) > decimal.Parse(dataRow["跌停"].ToString()) && decimal.Parse(dataRow["买价"].ToString()) < decimal.Parse(dataRow["涨停"].ToString())) ? dataRow["买价"] : dataRow["涨停"])));
				}
				else
				{
					this.numericUpDownPrice.Value = (decimal)double.Parse((this.comboBoxDirector.SelectedIndex == 0) ? this.labelUpper.Text : this.labelLower.Text);
				}
				if (_closeLots != 0m)
				{
					this.numericUpDownVolume.Value = _closeLots;
				}
			}
		}

		private void ChangeInstrument()
		{
			while (true)
			{
				if (this.havechangecode && this.select_code != "")
				{
					if (this.dtMarketData.Rows.Contains(this.select_code))
					{
						System.Data.DataRow dataRow = this.dtMarketData.Rows.Find(this.select_code);
						if (dataRow != null && dataRow["最新价"].ToString() != "0")
						{
							base.BeginInvoke(new Action<string, string, string, decimal>(this.orderRangeChangeInstrument), new object[]
							{
								this.select_code,
								null,
								null,
								0m
							});
							this.havechangecode = false;
							this.select_code = "";
						}
					}
				}
				System.Threading.Thread.Sleep(20);
			}
		}

		private void comboBoxInstrument_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (this.comboBoxInstrument.Text.Trim() != "")
			{
				string text = "";
				System.Data.DataRow[] array = this.dt_account_dg.Select("登录状态='已登录'");
				if (array.Length > 0)
				{
					text = array[0]["投资者帐户"].ToString();
				}
				string text2 = this.comboBoxInstrument.Text;
				if (!(text != "") || this.Dic_TradeApi[text].dtInstruments.Rows.Contains(this.comboBoxInstrument.Text))
				{
					if (this.dtMarketData.Rows.Contains(text2))
					{
						this.havechangecode = false;
						this.select_code = "";
						this.orderRangeChangeInstrument(text2, null, null, 0m);
					}
					else
					{
						try
						{
							this.havechangecode = true;
							this.select_code = text2;
							this.dtMarketData.Rows.Add(new object[]
							{
								text2,
								(text != "") ? ((string)this.Dic_TradeApi[text].dtInstruments.Rows.Find(this.comboBoxInstrument.Text)["名称"]) : "",
								(text != "") ? ((string)this.Dic_TradeApi[text].dtInstruments.Rows.Find(this.comboBoxInstrument.Text)["交易所"]) : "",
								0,
								0,
								0,
								0,
								0,
								0,
								0,
								0,
								0,
								0,
								0,
								0,
								0,
								0,
								0,
								0,
								0,
								0,
								"",
								0,
								false,
								false,
								(text != "") ? this.Dic_TradeApi[text].dtInstruments.Rows.Find(this.comboBoxInstrument.Text)["合约数量"] : 0,
								(text != "") ? this.Dic_TradeApi[text].dtInstruments.Rows.Find(this.comboBoxInstrument.Text)["最小波动"] : 0
							});
							this.mdApi.SubMarketData(new string[]
							{
								text2
							});
						}
						catch
						{
							this.comboBoxErrMsg.Items.Insert(0, System.DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss.fff") + "|订阅合约失败：" + text2);
							this.comboBoxErrMsg.SelectedIndex = 0;
						}
					}
				}
			}
		}

		private void buttonPrice_Click(object sender, System.EventArgs e)
		{
			if (this.buttonPrice.Text == "跟盘价")
			{
				this.freshOrderPrice = false;
				this.buttonPrice.Text = "指定价";
				this.buttonPrice.BackColor = System.Drawing.Color.Transparent;
			}
			else
			{
				this.buttonPrice.Text = "跟盘价";
				this.buttonPrice.BackColor = System.Drawing.Color.FromArgb(255, 128, 128);
				this.freshOrderPrice = true;
			}
		}

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			this.comboBoxInstrument.Text = "";
			this.comboBoxDirector.SelectedIndex = 0;
			this.comboBoxOffset.SelectedIndex = 0;
			this.numericUpDownPrice.Minimum = 0m;
			this.numericUpDownPrice.Maximum = 100m;
			this.numericUpDownPrice.Value = 0m;
			this.numericUpDownVolume.Value = 0m;
		}

		private void numericUpDownPrice_ValueChanged(object sender, System.EventArgs e)
		{
			System.Data.DataRow dataRow = this.dtMarketData.Rows.Find(this.comboBoxInstrument.Text);
			if (dataRow != null)
			{
				this.numericUpDownPrice.Increment = (decimal)((double)dataRow["最小波动"]);
				string text = this.numericUpDownPrice.Increment.ToString();
				this.numericUpDownPrice.DecimalPlaces = ((text.IndexOf('.') == -1) ? 0 : (text.Length - text.IndexOf('.') - 1));
			}
			this.numericUpDownPrice.Select(0, this.numericUpDownPrice.ToString().Length);
		}

		private void AccountSetTSMI_Click(object sender, System.EventArgs e)
		{
			try
			{
				AccountSet accountSet = new AccountSet();
				accountSet.SystemSet_value(this.List_brokers);
				if (accountSet.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					this.CloseFrm();
				}
			}
			catch (System.Exception var_1_31)
			{
			}
		}

		private void TradingAccountTSMI_Click(object sender, System.EventArgs e)
		{
			System.Data.DataRow[] array = this.dt_account_dg.Select("登录状态='已登录'");
			for (int i = 0; i < array.Length; i++)
			{
				string key = array[i]["投资者帐户"].ToString();
				this.Dic_TradeApi[key].listQry.Add(new TradeApi.Order(TradeApi.EnumQryOrder.QryTradingAccount, this.Dic_TradeApi[key], null, null));
			}
		}

		private void InvestorPositionTSMI_Click(object sender, System.EventArgs e)
		{
			System.Data.DataRow[] array = this.dt_account_dg.Select("登录状态='已登录'");
			System.Data.DataRow[] array2 = this.dt_mainaccount_dg.Select("登录状态='已登录'");
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i]["投资者帐户"].ToString();
				this.ds_GroupPosition.Tables[text].Clear();
				this.Dic_TradeApi[text].listQry.Add(new TradeApi.Order(TradeApi.EnumQryOrder.QryIntorverPosition, this.Dic_TradeApi[text], null, null));
			}
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i]["投资者帐户"].ToString();
				this.ds_GroupPosition.Tables[text].Clear();
				this.Dic_TradeApi[text].listQry.Add(new TradeApi.Order(TradeApi.EnumQryOrder.QryIntorverPosition, this.Dic_TradeApi[text], null, null));
			}
		}

		private void tmMoneyRefresh_Tick(object sender, System.EventArgs e)
		{
			foreach (TradeApi current in this.Dic_TradeApi.Values)
			{
				if (current.IsLogin)
				{
					TradeApi.Order item = new TradeApi.Order(TradeApi.EnumQryOrder.QryTradingAccount, current, null, null);
					if (!current.listQry.Exists((TradeApi.Order x) => x.QryOrderType == TradeApi.EnumQryOrder.QryTradingAccount))
					{
						current.listQry.Add(item);
					}
				}
			}
		}

		private void PassWordTSMI_Click(object sender, System.EventArgs e)
		{
			ChangePassWord changePassWord = new ChangePassWord();
			changePassWord.ShowDialog();
		}

		private void rdoTradeChange(bool IsMain, string check_rdo_name)
		{
			ColumnView columnView;
			if (IsMain)
			{
				columnView = this.gvMainTrade;
			}
			else
			{
				columnView = this.gvTrade;
			}
			columnView.ActiveFilter.Clear();
			string filterString = "";
			if (check_rdo_name != null)
			{
				if (!(check_rdo_name == "挂单"))
				{
					if (!(check_rdo_name == "已成交"))
					{
						if (!(check_rdo_name == "已撤单"))
						{
							if (check_rdo_name == "错误单")
							{
								filterString = "[状态] = '错误' ";
							}
						}
						else
						{
							filterString = "[状态] = '已撤单' ";
						}
					}
					else
					{
						filterString = "[状态] = '全部成交' OR [状态] = '部分成交还在队列中' OR [状态] = '部分成交不在队列中'";
					}
				}
				else
				{
					filterString = "[状态] = '未成交还在队列中' OR [状态] = '未成交不在队列中' OR [状态] = '未成交' OR [状态] = '未知' OR [状态] = '发单中'";
				}
			}
			ViewColumnFilterInfo info = new ViewColumnFilterInfo(columnView.Columns["状态"], new ColumnFilterInfo(filterString, ""));
			columnView.ActiveFilter.Add(info);
		}

		private void rdoDeal_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.rdoDeal.Checked)
			{
				this.rdoTradeChange(false, "已成交");
			}
		}

		private void rdoAll_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.rdoAll.Checked)
			{
				this.rdoTradeChange(false, "全部单");
			}
		}

		private void rdoSuspend_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.rdoSuspend.Checked)
			{
				this.rdoTradeChange(false, "挂单");
			}
		}

		private void rdoRevoke_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.rdoRevoke.Checked)
			{
				this.rdoTradeChange(false, "已撤单");
			}
		}

		private void rdoError_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.rdoError.Checked)
			{
				this.rdoTradeChange(false, "错误单");
			}
		}

		private void rdoMainAll_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.rdoMainAll.Checked)
			{
				this.rdoTradeChange(true, "全部单");
			}
		}

		private void rdoMainSuspend_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.rdoMainSuspend.Checked)
			{
				this.rdoTradeChange(true, "挂单");
			}
		}

		private void rdoMainDeal_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.rdoMainDeal.Checked)
			{
				this.rdoTradeChange(true, "已成交");
			}
		}

		private void rdoMainRevoke_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.rdoMainRevoke.Checked)
			{
				this.rdoTradeChange(true, "已撤单");
			}
		}

		private void gvTrade_RowCellStyle(object sender, RowCellStyleEventArgs e)
		{
			if (this.gvTrade.GetRowCellValue(e.RowHandle, this.gvTrade.Columns["买卖"].ToString()).ToString() != "买" && e.Column.FieldName == "买卖")
			{
				e.Appearance.ForeColor = System.Drawing.Color.Green;
			}
			else if (this.gvTrade.GetRowCellValue(e.RowHandle, this.gvTrade.Columns["买卖"].ToString()).ToString() == "买" && e.Column.FieldName == "买卖")
			{
				e.Appearance.ForeColor = System.Drawing.Color.Red;
			}
			if ((this.gvTrade.GetRowCellValue(e.RowHandle, this.gvTrade.Columns["状态"].ToString()).ToString() == "未知" || this.gvTrade.GetRowCellValue(e.RowHandle, this.gvTrade.Columns["状态"].ToString()).ToString() == "错误") && e.Column.FieldName == "状态")
			{
				e.Appearance.ForeColor = System.Drawing.Color.Red;
			}
			else if (this.gvTrade.GetRowCellValue(e.RowHandle, this.gvTrade.Columns["状态"].ToString()).ToString() == "全部成交" && e.Column.FieldName == "状态")
			{
				e.Appearance.ForeColor = System.Drawing.Color.Green;
			}
			else if (this.gvTrade.GetRowCellValue(e.RowHandle, this.gvTrade.Columns["状态"].ToString()).ToString() != "未知" && this.gvTrade.GetRowCellValue(e.RowHandle, this.gvTrade.Columns["状态"].ToString()).ToString() != "全部成交" && e.Column.FieldName == "状态")
			{
				e.Appearance.ForeColor = System.Drawing.Color.DarkKhaki;
			}
		}

		private void gvDeal_RowCellStyle(object sender, RowCellStyleEventArgs e)
		{
			if (this.gvDeal.GetRowCellValue(e.RowHandle, this.gvDeal.Columns["买卖"].ToString()).ToString() != "买" && e.Column.FieldName == "买卖")
			{
				e.Appearance.ForeColor = System.Drawing.Color.Green;
			}
			if (this.gvDeal.GetRowCellValue(e.RowHandle, this.gvDeal.Columns["买卖"].ToString()).ToString() == "买" && e.Column.FieldName == "买卖")
			{
				e.Appearance.ForeColor = System.Drawing.Color.Red;
			}
		}

		private void gvMainTrade_RowCellStyle(object sender, RowCellStyleEventArgs e)
		{
			if (this.gvMainTrade.GetRowCellValue(e.RowHandle, this.gvMainTrade.Columns["买卖"].ToString()).ToString() != "买" && e.Column.FieldName == "买卖")
			{
				e.Appearance.ForeColor = System.Drawing.Color.Green;
			}
			if (this.gvMainTrade.GetRowCellValue(e.RowHandle, this.gvMainTrade.Columns["买卖"].ToString()).ToString() == "买" && e.Column.FieldName == "买卖")
			{
				e.Appearance.ForeColor = System.Drawing.Color.Red;
			}
			if ((this.gvMainTrade.GetRowCellValue(e.RowHandle, this.gvMainTrade.Columns["状态"].ToString()).ToString() == "未知" || this.gvMainTrade.GetRowCellValue(e.RowHandle, this.gvMainTrade.Columns["状态"].ToString()).ToString() == "错误") && e.Column.FieldName == "状态")
			{
				e.Appearance.ForeColor = System.Drawing.Color.Red;
			}
			if (this.gvMainTrade.GetRowCellValue(e.RowHandle, this.gvMainTrade.Columns["状态"].ToString()).ToString() == "全部成交" && e.Column.FieldName == "状态")
			{
				e.Appearance.ForeColor = System.Drawing.Color.Green;
			}
			if (this.gvMainTrade.GetRowCellValue(e.RowHandle, this.gvMainTrade.Columns["状态"].ToString()).ToString() != "未知" && this.gvMainTrade.GetRowCellValue(e.RowHandle, this.gvMainTrade.Columns["状态"].ToString()).ToString() != "全部成交" && e.Column.FieldName == "状态")
			{
				e.Appearance.ForeColor = System.Drawing.Color.DarkKhaki;
			}
		}

		private void gvMainDeal_RowCellStyle(object sender, RowCellStyleEventArgs e)
		{
			if (this.gvMainDeal.GetRowCellValue(e.RowHandle, this.gvMainDeal.Columns["买卖"].ToString()).ToString() != "买" && e.Column.FieldName == "买卖")
			{
				e.Appearance.ForeColor = System.Drawing.Color.Green;
			}
			if (this.gvMainDeal.GetRowCellValue(e.RowHandle, this.gvMainDeal.Columns["买卖"].ToString()).ToString() == "买" && e.Column.FieldName == "买卖")
			{
				e.Appearance.ForeColor = System.Drawing.Color.Red;
			}
		}

		private void bgvAccount_RowCellStyle(object sender, RowCellStyleEventArgs e)
		{
			if (System.Convert.ToDecimal(this.bgvAccount.GetRowCellValue(e.RowHandle, this.bgvAccount.Columns["平仓盈亏"].ToString())) < 0m && e.Column.FieldName == "平仓盈亏")
			{
				e.Appearance.ForeColor = System.Drawing.Color.Green;
			}
			if (System.Convert.ToDecimal(this.bgvAccount.GetRowCellValue(e.RowHandle, this.bgvAccount.Columns["持仓盈亏"].ToString())) < 0m && e.Column.FieldName == "持仓盈亏")
			{
				e.Appearance.ForeColor = System.Drawing.Color.Green;
			}
			if (System.Convert.ToDecimal(this.bgvAccount.GetRowCellValue(e.RowHandle, this.bgvAccount.Columns["平仓盈亏"].ToString())) > 0m && e.Column.FieldName == "平仓盈亏")
			{
				e.Appearance.ForeColor = System.Drawing.Color.Red;
			}
			if (System.Convert.ToDecimal(this.bgvAccount.GetRowCellValue(e.RowHandle, this.bgvAccount.Columns["持仓盈亏"].ToString())) > 0m && e.Column.FieldName == "持仓盈亏")
			{
				e.Appearance.ForeColor = System.Drawing.Color.Red;
			}
		}

		private void PositionTradeByAccountTSMI_Click(object sender, System.EventArgs e)
		{
			frmAccountTradeSet frmAccountTradeSet = new frmAccountTradeSet();
			frmAccountTradeSet expr_08 = frmAccountTradeSet;
			expr_08.cf = (frmAccountTradeSet.closeform)System.Delegate.Combine(expr_08.cf, new frmAccountTradeSet.closeform(this.accountset_close));
			GridControl gridControl = frmAccountTradeSet.Controls["xdgAccountTradeSet"] as GridControl;
			gridControl.DataSource = this.dt_AccountTradeSet;
			frmAccountTradeSet.TopMost = true;
			frmAccountTradeSet.Show();
		}

		private void accountset_close()
		{
			System.Data.DataRow[] array = this.dt_account_dg.Select("登录状态='已登录'");
			for (int i = 0; i < array.Length; i++)
			{
				string iD = array[i]["投资者帐户"].ToString();
				this.SelectCodeName(iD);
			}
			this.readttod.writetxt(this.dt_AccountTradeSet, "orderset.txt");
		}

		private void TSMI_BeginListen_Click(object sender, System.EventArgs e)
		{
			if (this.TSMI_BeginListen.Text == "启动监听")
			{
				this.listprintmsg.Add(new string[]
				{
					"0",
					"启动监听..."
				});
				this.TSMI_BeginListen.BackColor = System.Drawing.Color.Yellow;
				this.BeginListen = true;
				this.TSMI_BeginListen.Text = "停止监听";
			}
			else
			{
				this.listprintmsg.Add(new string[]
				{
					"0",
					"停止监听..."
				});
				this.TSMI_BeginListen.BackColor = System.Drawing.Color.Transparent;
				this.BeginListen = false;
				this.TSMI_BeginListen.Text = "启动监听";
			}
		}

		private void numericUpDownPrice_Click(object sender, System.EventArgs e)
		{
			this.numericUpDownPrice.Select(0, this.numericUpDownPrice.ToString().Length);
		}

		private void numericUpDownVolume_ValueChanged(object sender, System.EventArgs e)
		{
			this.numericUpDownVolume.Select(0, this.numericUpDownVolume.ToString().Length);
		}

		private void numericUpDownVolume_Click(object sender, System.EventArgs e)
		{
			this.numericUpDownVolume.Select(0, this.numericUpDownVolume.ToString().Length);
		}

		private void CodeSetTSMI_Click(object sender, System.EventArgs e)
		{
			frmCodeSet frmCodeSet = new frmCodeSet();
			frmCodeSet expr_08 = frmCodeSet;
			expr_08.cf = (frmCodeSet.closeform)System.Delegate.Combine(expr_08.cf, new frmCodeSet.closeform(this.codeset_close));
			GridControl gridControl = frmCodeSet.Controls["xdgCodeSet"] as GridControl;
			gridControl.DataSource = this.dt_CodeSet;
			frmCodeSet.TopMost = true;
			frmCodeSet.Show();
		}

		private void codeset_close()
		{
			this.readttod.writetxt(this.dt_CodeSet, "codeset.txt");
		}

		private void SubAccountSetTSMI_Click(object sender, System.EventArgs e)
		{
			frmSubAccountSet frmSubAccountSet = new frmSubAccountSet();
			frmSubAccountSet expr_08 = frmSubAccountSet;
			expr_08.cf = (frmSubAccountSet.closeform)System.Delegate.Combine(expr_08.cf, new frmSubAccountSet.closeform(this.SubAccountset_close));
			frmSubAccountSet expr_2A = frmSubAccountSet;
			expr_2A.subParams = (frmSubAccountSet.subaccountParams)System.Delegate.Combine(expr_2A.subParams, new frmSubAccountSet.subaccountParams(this.SubAccountsetParams));
			GridControl gridControl = frmSubAccountSet.Controls["xdgSubAccountSet"] as GridControl;
			gridControl.DataSource = this.dt_SubAccountSet;
			frmSubAccountSet.TopMost = true;
			frmSubAccountSet.Show();
		}

		private void SubAccountset_close()
		{
			this.readttod.writetxt(this.dt_SubAccountSet, "SubAccountset.txt");
		}

		private void SubAccountsetParams(string id, bool Listen, bool proof)
		{
			if (this.Dic_TradeApi.ContainsKey(id))
			{
				this.Dic_TradeApi[id].SubListenbegin = Listen;
				this.Dic_TradeApi[id].SubProofbegin = proof;
			}
		}

		private void chooseTreadORDeal(int num)
		{
			if (num == 0)
			{
				this.TradeORDeal = false;
			}
			else
			{
				this.TradeORDeal = true;
			}
		}

		private void radioButton2_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.radioButton2.Checked)
			{
				this.chooseTreadORDeal(1);
				this.ini.WriteValue("设置", "跟单方式", "1");
			}
		}

		private void radioButton1_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.radioButton1.Checked)
			{
				this.chooseTreadORDeal(0);
				this.ini.WriteValue("设置", "跟单方式", "0");
			}
		}

		protected override void WndProc(ref System.Windows.Forms.Message m)
		{
			this.hotkey.ProcessHotKey(m);
			base.WndProc(ref m);
		}

		public void focuscbbinstrument(int id)
		{
			if (id == 50000)
			{
				this.comboBoxInstrument.Focus();
			}
			else
			{
				frmMainFrame.hotkeyIDAndCode hotkeyIDAndCode = this.hotkeyidandcode.Find((frmMainFrame.hotkeyIDAndCode ic) => ic.keyID == id);
				if (hotkeyIDAndCode != null)
				{
					if (this.comboBoxInstrument.Text == "" || this.comboBoxInstrument.SelectionLength > 0)
					{
						this.comboBoxInstrument.Text = hotkeyIDAndCode.keyCode;
						this.comboBoxInstrument.SelectionStart = this.comboBoxInstrument.Text.Length;
					}
					else
					{
						ComboBoxEdit expr_CC = this.comboBoxInstrument;
						expr_CC.Text += hotkeyIDAndCode.keystr;
						this.comboBoxInstrument.SelectionStart = this.comboBoxInstrument.Text.Length;
					}
				}
			}
		}

		public void focusoffest(int id)
		{
			switch (id)
			{
			case 1:
				this.comboBoxDirector.SelectedIndex = 0;
				break;
			case 3:
				this.comboBoxDirector.SelectedIndex = 1;
				break;
			case 7:
				this.comboBoxOffset.SelectedIndex = 0;
				break;
			case 8:
				this.comboBoxOffset.SelectedIndex = 1;
				break;
			case 9:
				this.comboBoxOffset.SelectedIndex = 2;
				break;
			}
		}

		private void regedit(int num)
		{
			switch (num)
			{
			case 1:
			{
				int num2 = 10;
				for (int i = 0; i < this.dt_CodeSet.Rows.Count; i++)
				{
					string text = this.dt_CodeSet.Rows[i]["品种"].ToString().Trim();
					string text2 = this.dt_CodeSet.Rows[i]["快捷键代码"].ToString();
					string text3 = this.dt_CodeSet.Rows[i]["keys"].ToString();
					if (text != "" && text2 != "")
					{
						if (char.IsNumber(text2, 0))
						{
							System.Windows.Forms.Keys vk = (System.Windows.Forms.Keys)System.Enum.Parse(typeof(System.Windows.Forms.Keys), text3);
							this.hotkey.Regist(base.Handle, ++num2, 0, vk, new HotKeys.HotKeyCallBackHanlder(this.focuscbbinstrument));
							this.hotkeyidandcode.Add(new frmMainFrame.hotkeyIDAndCode
							{
								keyID = num2,
								keyCode = text,
								keystr = text2
							});
							string text4 = text3;
							switch (text4)
							{
							case "D0":
								this.hotkey.Regist(base.Handle, ++num2, 0, System.Windows.Forms.Keys.NumPad0, new HotKeys.HotKeyCallBackHanlder(this.focuscbbinstrument));
								break;
							case "D1":
								this.hotkey.Regist(base.Handle, ++num2, 0, System.Windows.Forms.Keys.NumPad1, new HotKeys.HotKeyCallBackHanlder(this.focuscbbinstrument));
								break;
							case "D2":
								this.hotkey.Regist(base.Handle, ++num2, 0, System.Windows.Forms.Keys.NumPad2, new HotKeys.HotKeyCallBackHanlder(this.focuscbbinstrument));
								break;
							case "D3":
								this.hotkey.Regist(base.Handle, ++num2, 0, System.Windows.Forms.Keys.NumPad3, new HotKeys.HotKeyCallBackHanlder(this.focuscbbinstrument));
								break;
							case "D4":
								this.hotkey.Regist(base.Handle, ++num2, 0, System.Windows.Forms.Keys.NumPad4, new HotKeys.HotKeyCallBackHanlder(this.focuscbbinstrument));
								break;
							case "D5":
								this.hotkey.Regist(base.Handle, ++num2, 0, System.Windows.Forms.Keys.NumPad5, new HotKeys.HotKeyCallBackHanlder(this.focuscbbinstrument));
								break;
							case "D6":
								this.hotkey.Regist(base.Handle, ++num2, 0, System.Windows.Forms.Keys.NumPad6, new HotKeys.HotKeyCallBackHanlder(this.focuscbbinstrument));
								break;
							case "D7":
								this.hotkey.Regist(base.Handle, ++num2, 0, System.Windows.Forms.Keys.NumPad7, new HotKeys.HotKeyCallBackHanlder(this.focuscbbinstrument));
								break;
							case "D8":
								this.hotkey.Regist(base.Handle, ++num2, 0, System.Windows.Forms.Keys.NumPad8, new HotKeys.HotKeyCallBackHanlder(this.focuscbbinstrument));
								break;
							case "D9":
								this.hotkey.Regist(base.Handle, ++num2, 0, System.Windows.Forms.Keys.NumPad9, new HotKeys.HotKeyCallBackHanlder(this.focuscbbinstrument));
								break;
							}
							this.hotkeyidandcode.Add(new frmMainFrame.hotkeyIDAndCode
							{
								keyID = num2,
								keyCode = text,
								keystr = text2
							});
						}
						else
						{
							System.Windows.Forms.Keys vk = (System.Windows.Forms.Keys)System.Enum.Parse(typeof(System.Windows.Forms.Keys), text2.ToUpper());
							this.hotkey.Regist(base.Handle, ++num2, 0, vk, new HotKeys.HotKeyCallBackHanlder(this.focuscbbinstrument));
							this.hotkeyidandcode.Add(new frmMainFrame.hotkeyIDAndCode
							{
								keyID = num2,
								keyCode = text,
								keystr = text2
							});
						}
					}
				}
				break;
			}
			case 3:
				this.hotkey.Regist(base.Handle, 1, 0, System.Windows.Forms.Keys.NumPad1, new HotKeys.HotKeyCallBackHanlder(this.focusoffest));
				this.hotkey.Regist(base.Handle, 3, 0, System.Windows.Forms.Keys.NumPad3, new HotKeys.HotKeyCallBackHanlder(this.focusoffest));
				this.hotkeyidandcode.Add(new frmMainFrame.hotkeyIDAndCode
				{
					keyID = 1,
					keyCode = "1",
					keystr = "1"
				});
				this.hotkeyidandcode.Add(new frmMainFrame.hotkeyIDAndCode
				{
					keyID = 3,
					keyCode = "3",
					keystr = "3"
				});
				break;
			case 4:
				this.hotkey.Regist(base.Handle, 7, 0, System.Windows.Forms.Keys.NumPad1, new HotKeys.HotKeyCallBackHanlder(this.focusoffest));
				this.hotkey.Regist(base.Handle, 8, 0, System.Windows.Forms.Keys.NumPad2, new HotKeys.HotKeyCallBackHanlder(this.focusoffest));
				this.hotkey.Regist(base.Handle, 9, 0, System.Windows.Forms.Keys.NumPad3, new HotKeys.HotKeyCallBackHanlder(this.focusoffest));
				this.hotkeyidandcode.Add(new frmMainFrame.hotkeyIDAndCode
				{
					keyID = 7,
					keyCode = "1",
					keystr = "1"
				});
				this.hotkeyidandcode.Add(new frmMainFrame.hotkeyIDAndCode
				{
					keyID = 8,
					keyCode = "2",
					keystr = "2"
				});
				this.hotkeyidandcode.Add(new frmMainFrame.hotkeyIDAndCode
				{
					keyID = 9,
					keyCode = "3",
					keystr = "3"
				});
				break;
			}
		}

		private void unregedit()
		{
			for (int i = 0; i < this.hotkeyidandcode.Count; i++)
			{
				this.hotkey.UnRegist(base.Handle, this.hotkeyidandcode[i].keyID);
			}
			this.hotkey.keymap.Clear();
			this.hotkeyidandcode.Clear();
		}

		private void regeditjudge()
		{
			if (this.has_regedit)
			{
				this.unregedit();
				if (this.comboBoxInstrument.IsEditorActive)
				{
					this.regedit(1);
				}
				if (this.comboBoxDirector.Focused)
				{
					this.regedit(3);
				}
				if (this.comboBoxOffset.Focused)
				{
					this.regedit(4);
				}
			}
		}

		private void frmMainFrame_Activated(object sender, System.EventArgs e)
		{
			this.has_regedit = true;
			this.regeditjudge();
		}

		private void frmMainFrame_Deactivate(object sender, System.EventArgs e)
		{
			this.has_regedit = false;
			this.unregedit();
		}

		private void comboBoxInstrument_Enter(object sender, System.EventArgs e)
		{
			this.unregedit();
			this.regedit(1);
		}

		private void comboBoxInstrument_Leave(object sender, System.EventArgs e)
		{
			this.unregedit();
		}

		private void comboBoxDirector_Enter(object sender, System.EventArgs e)
		{
			this.unregedit();
			this.regedit(3);
		}

		private void comboBoxDirector_Leave(object sender, System.EventArgs e)
		{
			this.unregedit();
		}

		private void comboBoxOffset_Enter(object sender, System.EventArgs e)
		{
			this.unregedit();
			this.regedit(4);
		}

		private void comboBoxOffset_Leave(object sender, System.EventArgs e)
		{
			this.unregedit();
		}

		private void frmMainFrame_Resize(object sender, System.EventArgs e)
		{
			if (base.WindowState == System.Windows.Forms.FormWindowState.Minimized)
			{
				this.has_regedit = false;
				this.unregedit();
			}
			else
			{
				this.has_regedit = true;
				this.regeditjudge();
			}
		}

		private void PositionCheckTSMI_Click(object sender, System.EventArgs e)
		{
			if (!this.beginHandCheck)
			{
				try
				{
					this.beginHandCheck = true;
					foreach (string current in this.dlchecktrans.Keys)
					{
						this.dlchecktrans[current].Clear();
					}
					foreach (string current in this.Dic_TradeApi.Keys)
					{
						if (this.Dic_TradeApi[current].IsLogin)
						{
							bool flag = this.Dic_TradeApi[current].Group == "主帐户";
							for (int i = 0; i < this.ds_GroupPosition.Tables[current].Rows.Count; i++)
							{
								System.Data.DataRow dataRow = this.ds_GroupPosition.Tables[current].Rows[i];
								string instrument = dataRow["合约"].ToString();
								string text = this.instrtovar(instrument);
								string direction = dataRow["买卖"].ToString();
								if (flag)
								{
									System.Data.DataRow[] array = this.dt_AccountTradeSet.Select(string.Concat(new string[]
									{
										"(品种='",
										instrument,
										"'or 品种='",
										text,
										"'or 品种='*') and 主帐户='",
										current,
										"'"
									}), "优先 asc");
									for (int j = 0; j < array.Length; j++)
									{
										string key = array[j]["子帐户"].ToString();
										if (this.Dic_TradeApi[key].IsLogin)
										{
											string[] array2 = this.Dic_SeparateCheck[key].Find((string[] s) => s[1] == instrument && s[2] == direction && s[6] == "手动");
											if (array2 == null || array2.Length == 0)
											{
												this.Dic_SeparateCheck[key].Insert(0, new string[]
												{
													current,
													instrument,
													direction,
													"0",
													System.DateTime.Now.ToString(),
													text,
													"手动",
													this.TradeORDeal ? "成交" : "委托"
												});
											}
										}
									}
								}
								else
								{
									string[] array2 = this.Dic_SeparateCheck[current].Find((string[] s) => s[1] == instrument && s[2] == direction && s[6] == "手动");
									if (array2 == null || array2.Length == 0)
									{
										this.Dic_SeparateCheck[current].Insert(0, new string[]
										{
											"",
											instrument,
											direction,
											"0",
											System.DateTime.Now.ToString(),
											text,
											"手动",
											this.TradeORDeal ? "成交" : "委托"
										});
									}
								}
							}
						}
					}
					this.frmcheckreturn();
				}
				catch (System.Exception ex)
				{
					this.beginHandCheck = false;
					System.Windows.Forms.MessageBox.Show(ex.ToString());
				}
				finally
				{
				}
			}
		}

		private void frmcheckreturn()
		{
			try
			{
				while (true)
				{
					int num = 0;
					for (int i = 0; i < this.dt_account_dg.Rows.Count; i++)
					{
						string key = this.dt_account_dg.Rows[i]["投资者帐户"].ToString();
						System.Collections.Generic.List<string[]> list = this.Dic_SeparateCheck[key].FindAll((string[] s) => s[6] == "手动");
						if (list != null && list.Count != 0)
						{
							num++;
						}
					}
					if (num == 0)
					{
						break;
					}
					System.Threading.Thread.Sleep(100);
				}
				FrmCheckPosition frmCheckPosition = new FrmCheckPosition();
				frmCheckPosition.Initialize(this.dlchecktrans);
				frmCheckPosition.checktrade = new FrmCheckPosition.checkpositionform(this.DoCheckTrade);
				frmCheckPosition.Show();
				this.beginHandCheck = false;
			}
			catch (System.Exception var_5_F9)
			{
			}
		}

		private void DoCheckTrade(System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string[]>> dlchecktrans)
		{
			foreach (string current in dlchecktrans.Keys)
			{
				for (int i = 0; i < dlchecktrans[current].Count; i++)
				{
					this.Dic_TradeApi[current].listTransDatas.Add(dlchecktrans[current][i]);
				}
			}
		}

		private void PositionColorCheck(string id)
		{
			try
			{
				while (true)
				{
					if (this.Dic_TradeApi[id].IsLogin && this.Dic_ColorCheck[id].Count != 0)
					{
						lock (this)
						{
							string[] array = this.Dic_ColorCheck[id][0];
							System.Data.DataRow dataRow = this.ds_GroupPosition.Tables[id].Select(string.Concat(new string[]
							{
								"合约='",
								array[0],
								"'and 买卖='",
								array[1],
								"'"
							})).FirstOrDefault<System.Data.DataRow>();
							bool error = false;
							if (dataRow != null)
							{
								if (array[2] == "0")
								{
									dataRow["校对"] = "0";
								}
								else
								{
									dataRow["校对"] = "1";
									error = true;
								}
							}
							else if (array[2] != "0")
							{
								error = true;
							}
							this.ShowTab(id, error);
							this.Dic_ColorCheck[id].RemoveAt(0);
						}
					}
					System.Threading.Thread.Sleep(100);
				}
			}
			catch (System.Exception var_4_162)
			{
			}
		}

		public void ShowTab(string id, bool error)
		{
			if (base.InvokeRequired)
			{
				frmMainFrame.InvokeCallback method = new frmMainFrame.InvokeCallback(this.ShowTab);
				base.Invoke(method, new object[]
				{
					id,
					error
				});
			}
			else if (error)
			{
				if (this.tab.TabPages.ContainsKey(id))
				{
					this.tab.TabPages[id].ImageIndex = 0;
				}
			}
			else if (this.tab.TabPages.ContainsKey(id))
			{
				this.tab.TabPages[id].ImageIndex = -1;
			}
		}

		private void textBox1_TextChanged(object sender, System.EventArgs e)
		{
			if (this.textBox1.Text.Trim() != "")
			{
				float num = 0f;
				if (float.TryParse(this.textBox1.Text.Trim(), out num))
				{
					this.prooftime = num;
					this.ini.WriteValue("设置", "延时时间", num.ToString());
				}
			}
		}

		private void comboBoxInstrument_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
		}

		private void 帮助ToolStripMenuItem_Click(object sender, System.EventArgs e)
		{
			Process.Start(new ProcessStartInfo
			{
				WorkingDirectory = System.Windows.Forms.Application.StartupPath,
				FileName = "help.chm",
				Arguments = ""
			});
		}

		private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			frmMsgTip frmMsgTip = new frmMsgTip();
			frmMsgTip.ShowDialog();
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			string text = this.comboBoxInstrument.Text;
			if (!string.IsNullOrWhiteSpace(text))
			{
				double num = (this.comboBoxDirector.SelectedIndex == 0) ? System.Convert.ToDouble(this.labelUpper.Text) : System.Convert.ToDouble(this.labelLower.Text);
				System.Data.DataRow dataRow = this.dtMarketData.Rows.Find(this.comboBoxInstrument.Text);
				if (dataRow != null)
				{
					num = (double)dataRow["卖价"];
				}
				int num2 = (int)this.numericUpDownVolume.Value;
				if (num2 > 0)
				{
					System.Data.DataRow[] array = this.dt_account_dg.Select("选择='True'");
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i]["登录状态"].ToString() == "已登录")
						{
							string key = array[i]["投资者帐户"].ToString();
							this.Dic_TradeApi[key].listTransDatas.Add(new string[]
							{
								text,
								"Buy",
								"open",
								num.ToString(),
								num2.ToString(),
								this.Dic_TradeApi[key].UpOrderRef().ToString()
							});
						}
					}
				}
			}
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			string text = this.comboBoxInstrument.Text;
			if (!string.IsNullOrWhiteSpace(text))
			{
				double num = (this.comboBoxDirector.SelectedIndex == 0) ? System.Convert.ToDouble(this.labelUpper.Text) : System.Convert.ToDouble(this.labelLower.Text);
				System.Data.DataRow dataRow = this.dtMarketData.Rows.Find(this.comboBoxInstrument.Text);
				if (dataRow != null)
				{
					num = (double)dataRow["买价"];
				}
				int num2 = (int)this.numericUpDownVolume.Value;
				if (num2 > 0)
				{
					System.Data.DataRow[] array = this.dt_account_dg.Select("选择='True'");
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i]["登录状态"].ToString() == "已登录")
						{
							string key = array[i]["投资者帐户"].ToString();
							this.Dic_TradeApi[key].listTransDatas.Add(new string[]
							{
								text,
								"Sell",
								"open",
								num.ToString(),
								num2.ToString(),
								this.Dic_TradeApi[key].UpOrderRef().ToString()
							});
						}
					}
				}
			}
		}

		private void button3_Click(object sender, System.EventArgs e)
		{
			string text = this.comboBoxInstrument.Text;
			if (!string.IsNullOrWhiteSpace(text))
			{
				double num = (this.comboBoxDirector.SelectedIndex == 0) ? System.Convert.ToDouble(this.labelUpper.Text) : System.Convert.ToDouble(this.labelLower.Text);
				System.Data.DataRow dataRow = this.dtMarketData.Rows.Find(this.comboBoxInstrument.Text);
				if (dataRow != null)
				{
					num = (double)dataRow["买价"];
				}
				int num2 = (int)this.numericUpDownVolume.Value;
				if (num2 > 0)
				{
					System.Data.DataRow[] array = this.dt_account_dg.Select("选择='True'");
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i]["登录状态"].ToString() == "已登录")
						{
							string key = array[i]["投资者帐户"].ToString();
							this.Dic_TradeApi[key].listTransDatas.Add(new string[]
							{
								text,
								"Sell",
								"close",
								num.ToString(),
								num2.ToString(),
								this.Dic_TradeApi[key].UpOrderRef().ToString()
							});
						}
					}
				}
			}
		}

		private void button4_Click(object sender, System.EventArgs e)
		{
			string text = this.comboBoxInstrument.Text;
			if (!string.IsNullOrWhiteSpace(text))
			{
				double num = (this.comboBoxDirector.SelectedIndex == 0) ? System.Convert.ToDouble(this.labelUpper.Text) : System.Convert.ToDouble(this.labelLower.Text);
				System.Data.DataRow dataRow = this.dtMarketData.Rows.Find(this.comboBoxInstrument.Text);
				if (dataRow != null)
				{
					num = (double)dataRow["卖价"];
				}
				int num2 = (int)this.numericUpDownVolume.Value;
				if (num2 > 0)
				{
					System.Data.DataRow[] array = this.dt_account_dg.Select("选择='True'");
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i]["登录状态"].ToString() == "已登录")
						{
							string key = array[i]["投资者帐户"].ToString();
							this.Dic_TradeApi[key].listTransDatas.Add(new string[]
							{
								text,
								"Buy",
								"close",
								num.ToString(),
								num2.ToString(),
								this.Dic_TradeApi[key].UpOrderRef().ToString()
							});
						}
					}
				}
			}
		}

		private void button5_Click(object sender, System.EventArgs e)
		{
			System.Data.DataView dataView = new System.Data.DataView(this.dt_AllTrade);
			dataView.RowFilter = "状态<> '全部成交' and 状态 <> '已撤单' and 状态 <> '错误'";
			for (int i = 0; i < dataView.Count; i++)
			{
				this.Dic_TradeApi[dataView[i]["投资者"].ToString()].ReqOrderAction(dataView[i]["合约"].ToString(), int.Parse(dataView[i]["前置编号"].ToString()), int.Parse(dataView[i]["会话编号"].ToString()), dataView[i]["序列号"].ToString());
			}
		}

		private void 结算单ToolStripMenuItem_Click(object sender, System.EventArgs e)
		{
			this.fcf.confirmInit(this.Dic_TradeApi.Keys.ToList<string>());
			this.fcf.Show();
		}

		private void SetComfirmmsg(string msg)
		{
			this.fcf.SetText(msg);
		}

		private void SelectConfirm(string account, string date)
		{
			if (this.Dic_TradeApi.ContainsKey(account) && this.Dic_TradeApi[account].IsLogin)
			{
				this.Dic_TradeApi[account].listQry.Add(new TradeApi.Order(TradeApi.EnumQryOrder.SettlementInfo, this.Dic_TradeApi[account], date, null));
			}
		}

		private void TSMI_Position_Click(object sender, System.EventArgs e)
		{
		}

		private void bgcMainAccount_RowCellStyle(object sender, RowCellStyleEventArgs e)
		{
			if (System.Convert.ToDecimal(this.bgcMainAccount.GetRowCellValue(e.RowHandle, this.bgcMainAccount.Columns["平仓盈亏"].ToString())) < 0m && e.Column.FieldName == "平仓盈亏")
			{
				e.Appearance.ForeColor = System.Drawing.Color.Green;
			}
			if (System.Convert.ToDecimal(this.bgcMainAccount.GetRowCellValue(e.RowHandle, this.bgcMainAccount.Columns["持仓盈亏"].ToString())) < 0m && e.Column.FieldName == "持仓盈亏")
			{
				e.Appearance.ForeColor = System.Drawing.Color.Green;
			}
			if (System.Convert.ToDecimal(this.bgcMainAccount.GetRowCellValue(e.RowHandle, this.bgcMainAccount.Columns["平仓盈亏"].ToString())) > 0m && e.Column.FieldName == "平仓盈亏")
			{
				e.Appearance.ForeColor = System.Drawing.Color.Red;
			}
			if (System.Convert.ToDecimal(this.bgcMainAccount.GetRowCellValue(e.RowHandle, this.bgcMainAccount.Columns["持仓盈亏"].ToString())) > 0m && e.Column.FieldName == "持仓盈亏")
			{
				e.Appearance.ForeColor = System.Drawing.Color.Red;
			}
		}

		private void 导出ToolStripMenuItem_Click(object sender, System.EventArgs e)
		{
			System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			folderBrowserDialog.ShowNewFolderButton = true;
			if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				this.writeAllTrade(this.dt_AllTrade, folderBrowserDialog.SelectedPath, "子账户委托");
			}
		}

		private void 导出数据ToolStripMenuItem_Click(object sender, System.EventArgs e)
		{
			System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			folderBrowserDialog.ShowNewFolderButton = true;
			if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				this.writeAllTrade(this.dt_MainTrade, folderBrowserDialog.SelectedPath, "主账户委托");
			}
		}

		private void writeAllTrade(System.Data.DataTable dt, string path, string title)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			string text = "";
			for (int i = 0; i < dt.Columns.Count; i++)
			{
				text = text + dt.Columns[i].ColumnName + ",";
			}
			stringBuilder.AppendLine(text);
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				string text2 = "";
				for (int j = 0; j < dt.Columns.Count; j++)
				{
					text2 = text2 + dt.Rows[i][j].ToString() + ",";
				}
				stringBuilder.AppendLine(text2);
			}
			using (System.IO.FileStream fileStream = new System.IO.FileStream(string.Concat(new string[]
			{
				path,
				"\\",
				System.DateTime.Now.ToString("yyyyMMdd"),
				title,
				".csv"
			}), System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.ReadWrite))
			{
				new System.IO.StreamWriter(fileStream, System.Text.Encoding.Default)
				{
					AutoFlush = true
				}.Write(stringBuilder.ToString());
			}
		}

		private void 导出数据ToolStripMenuItem2_Click(object sender, System.EventArgs e)
		{
			System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			folderBrowserDialog.ShowNewFolderButton = true;
			if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				this.writeAllTrade(this.dt_AllDeal, folderBrowserDialog.SelectedPath, "子账户成交");
			}
		}

		private void 导出数据ToolStripMenuItem1_Click(object sender, System.EventArgs e)
		{
			System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			folderBrowserDialog.ShowNewFolderButton = true;
			if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				this.writeAllTrade(this.dt_MainDeal, folderBrowserDialog.SelectedPath, "主账户成交");
			}
		}

		private void gvTrade_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
		{
			if (e.Info.IsRowIndicator && e.RowHandle >= 0)
			{
				e.Info.DisplayText = (e.RowHandle + 1).ToString();
			}
		}

		private void bgvAccount_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
		{
			int num = System.Convert.ToInt32((e.Item as GridSummaryItem).Tag);
			GridView gridView = sender as GridView;
			if (e.SummaryProcess == CustomSummaryProcess.Start)
			{
				this.saifbuy = 0;
				this.saifsell = 0;
				this.saihbuy = 0;
				this.saihsell = 0;
				this.saicbuy = 0;
				this.saicsell = 0;
				this.saaubuy = 0;
				this.saausell = 0;
				this.saagbuy = 0;
				this.saagsell = 0;
				this.sacubuy = 0;
				this.sacusell = 0;
				this.sarubuy = 0;
				this.sarusell = 0;
				this.sanibuy = 0;
				this.sanisell = 0;
				this.sarbbuy = 0;
				this.sarbsell = 0;
				this.saibuy = 0;
				this.saisell = 0;
				this.saybuy = 0;
				this.saysell = 0;
				this.sambuy = 0;
				this.samsell = 0;
				this.sappbuy = 0;
				this.sappsell = 0;
				this.salbuy = 0;
				this.salsell = 0;
				this.saMAbuy = 0;
				this.saMAsell = 0;
				this.saRMbuy = 0;
				this.saRMsell = 0;
				this.saSRbuy = 0;
				this.saSRsell = 0;
			}
			if (e.SummaryProcess == CustomSummaryProcess.Calculate)
			{
				string[] array = ((string)e.FieldValue).Split(new string[]
				{
					"/"
				}, System.StringSplitOptions.RemoveEmptyEntries);
				switch (num)
				{
				case 1:
					this.saifbuy += System.Convert.ToInt32(array[0]);
					this.saifsell += System.Convert.ToInt32(array[1]);
					break;
				case 2:
					this.saicbuy += System.Convert.ToInt32(array[0]);
					this.saicbuy += System.Convert.ToInt32(array[1]);
					break;
				case 3:
					this.saihbuy += System.Convert.ToInt32(array[0]);
					this.saihsell += System.Convert.ToInt32(array[1]);
					break;
				case 4:
					this.saaubuy += System.Convert.ToInt32(array[0]);
					this.saausell += System.Convert.ToInt32(array[1]);
					break;
				case 5:
					this.saagbuy += System.Convert.ToInt32(array[0]);
					this.saagsell += System.Convert.ToInt32(array[1]);
					break;
				case 6:
					this.sacubuy += System.Convert.ToInt32(array[0]);
					this.sacusell += System.Convert.ToInt32(array[1]);
					break;
				case 7:
					this.sarubuy += System.Convert.ToInt32(array[0]);
					this.sarusell += System.Convert.ToInt32(array[1]);
					break;
				case 8:
					this.sanibuy += System.Convert.ToInt32(array[0]);
					this.sanisell += System.Convert.ToInt32(array[1]);
					break;
				case 9:
					this.sarbbuy += System.Convert.ToInt32(array[0]);
					this.sarbsell += System.Convert.ToInt32(array[1]);
					break;
				case 10:
					this.saibuy += System.Convert.ToInt32(array[0]);
					this.saisell += System.Convert.ToInt32(array[1]);
					break;
				case 11:
					this.saybuy += System.Convert.ToInt32(array[0]);
					this.saysell += System.Convert.ToInt32(array[1]);
					break;
				case 12:
					this.sambuy += System.Convert.ToInt32(array[0]);
					this.samsell += System.Convert.ToInt32(array[1]);
					break;
				case 13:
					this.sappbuy += System.Convert.ToInt32(array[0]);
					this.sappsell += System.Convert.ToInt32(array[1]);
					break;
				case 14:
					this.salbuy += System.Convert.ToInt32(array[0]);
					this.salsell += System.Convert.ToInt32(array[1]);
					break;
				case 15:
					this.saMAbuy += System.Convert.ToInt32(array[0]);
					this.saMAsell += System.Convert.ToInt32(array[1]);
					break;
				case 16:
					this.saRMbuy += System.Convert.ToInt32(array[0]);
					this.saRMsell += System.Convert.ToInt32(array[1]);
					break;
				case 17:
					this.saSRbuy += System.Convert.ToInt32(array[0]);
					this.saSRsell += System.Convert.ToInt32(array[1]);
					break;
				}
			}
			if (e.SummaryProcess == CustomSummaryProcess.Finalize)
			{
				switch (num)
				{
				case 1:
					e.TotalValue = this.saifbuy + "/" + this.saifsell;
					break;
				case 2:
					e.TotalValue = this.saicbuy + "/" + this.saicsell;
					break;
				case 3:
					e.TotalValue = this.saihbuy + "/" + this.saihsell;
					break;
				case 4:
					e.TotalValue = this.saaubuy + "/" + this.saausell;
					break;
				case 5:
					e.TotalValue = this.saagbuy + "/" + this.saagsell;
					break;
				case 6:
					e.TotalValue = this.sacubuy + "/" + this.sacusell;
					break;
				case 7:
					e.TotalValue = this.sarubuy + "/" + this.sarusell;
					break;
				case 8:
					e.TotalValue = this.sanibuy + "/" + this.sanisell;
					break;
				case 9:
					e.TotalValue = this.sarbbuy + "/" + this.sarbsell;
					break;
				case 10:
					e.TotalValue = this.saibuy + "/" + this.saisell;
					break;
				case 11:
					e.TotalValue = this.saybuy + "/" + this.saysell;
					break;
				case 12:
					e.TotalValue = this.sambuy + "/" + this.samsell;
					break;
				case 13:
					e.TotalValue = this.sappbuy + "/" + this.sappsell;
					break;
				case 14:
					e.TotalValue = this.salbuy + "/" + this.salsell;
					break;
				case 15:
					e.TotalValue = this.saMAbuy + "/" + this.saMAsell;
					break;
				case 16:
					e.TotalValue = this.saRMbuy + "/" + this.saRMsell;
					break;
				case 17:
					e.TotalValue = this.saSRbuy + "/" + this.saSRsell;
					break;
				}
			}
		}

		private void bgcMainAccount_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
		{
			int num = System.Convert.ToInt32((e.Item as GridSummaryItem).Tag);
			GridView gridView = sender as GridView;
			if (e.SummaryProcess == CustomSummaryProcess.Start)
			{
				this.aifbuy = 0;
				this.aifsell = 0;
				this.aihbuy = 0;
				this.aihsell = 0;
				this.aicbuy = 0;
				this.aicsell = 0;
				this.aaubuy = 0;
				this.aausell = 0;
				this.aagbuy = 0;
				this.aagsell = 0;
				this.acubuy = 0;
				this.acusell = 0;
				this.arubuy = 0;
				this.arusell = 0;
				this.anibuy = 0;
				this.anisell = 0;
				this.arbbuy = 0;
				this.arbsell = 0;
				this.aibuy = 0;
				this.aisell = 0;
				this.aybuy = 0;
				this.aysell = 0;
				this.ambuy = 0;
				this.amsell = 0;
				this.appbuy = 0;
				this.appsell = 0;
				this.albuy = 0;
				this.alsell = 0;
				this.aMAbuy = 0;
				this.aMAsell = 0;
				this.aRMbuy = 0;
				this.aRMsell = 0;
				this.aSRbuy = 0;
				this.aSRsell = 0;
			}
			if (e.SummaryProcess == CustomSummaryProcess.Calculate)
			{
				string[] array = ((string)e.FieldValue).Split(new string[]
				{
					"/"
				}, System.StringSplitOptions.RemoveEmptyEntries);
				switch (num)
				{
				case 1:
					this.aifbuy += System.Convert.ToInt32(array[0]);
					this.aifsell += System.Convert.ToInt32(array[1]);
					break;
				case 2:
					this.aicbuy += System.Convert.ToInt32(array[0]);
					this.aicbuy += System.Convert.ToInt32(array[1]);
					break;
				case 3:
					this.aihbuy += System.Convert.ToInt32(array[0]);
					this.aihsell += System.Convert.ToInt32(array[1]);
					break;
				case 4:
					this.aaubuy += System.Convert.ToInt32(array[0]);
					this.aausell += System.Convert.ToInt32(array[1]);
					break;
				case 5:
					this.aagbuy += System.Convert.ToInt32(array[0]);
					this.aagsell += System.Convert.ToInt32(array[1]);
					break;
				case 6:
					this.acubuy += System.Convert.ToInt32(array[0]);
					this.acusell += System.Convert.ToInt32(array[1]);
					break;
				case 7:
					this.arubuy += System.Convert.ToInt32(array[0]);
					this.arusell += System.Convert.ToInt32(array[1]);
					break;
				case 8:
					this.anibuy += System.Convert.ToInt32(array[0]);
					this.anisell += System.Convert.ToInt32(array[1]);
					break;
				case 9:
					this.arbbuy += System.Convert.ToInt32(array[0]);
					this.arbsell += System.Convert.ToInt32(array[1]);
					break;
				case 10:
					this.aibuy += System.Convert.ToInt32(array[0]);
					this.aisell += System.Convert.ToInt32(array[1]);
					break;
				case 11:
					this.aybuy += System.Convert.ToInt32(array[0]);
					this.aysell += System.Convert.ToInt32(array[1]);
					break;
				case 12:
					this.ambuy += System.Convert.ToInt32(array[0]);
					this.amsell += System.Convert.ToInt32(array[1]);
					break;
				case 13:
					this.appbuy += System.Convert.ToInt32(array[0]);
					this.appsell += System.Convert.ToInt32(array[1]);
					break;
				case 14:
					this.albuy += System.Convert.ToInt32(array[0]);
					this.alsell += System.Convert.ToInt32(array[1]);
					break;
				case 15:
					this.aMAbuy += System.Convert.ToInt32(array[0]);
					this.aMAsell += System.Convert.ToInt32(array[1]);
					break;
				case 16:
					this.aRMbuy += System.Convert.ToInt32(array[0]);
					this.aRMsell += System.Convert.ToInt32(array[1]);
					break;
				case 17:
					this.aSRbuy += System.Convert.ToInt32(array[0]);
					this.aSRsell += System.Convert.ToInt32(array[1]);
					break;
				}
			}
			if (e.SummaryProcess == CustomSummaryProcess.Finalize)
			{
				switch (num)
				{
				case 1:
					e.TotalValue = this.aifbuy + "/" + this.aifsell;
					break;
				case 2:
					e.TotalValue = this.aicbuy + "/" + this.aicsell;
					break;
				case 3:
					e.TotalValue = this.aihbuy + "/" + this.aihsell;
					break;
				case 4:
					e.TotalValue = this.aaubuy + "/" + this.aausell;
					break;
				case 5:
					e.TotalValue = this.aagbuy + "/" + this.aagsell;
					break;
				case 6:
					e.TotalValue = this.acubuy + "/" + this.acusell;
					break;
				case 7:
					e.TotalValue = this.arubuy + "/" + this.arusell;
					break;
				case 8:
					e.TotalValue = this.anibuy + "/" + this.anisell;
					break;
				case 9:
					e.TotalValue = this.arbbuy + "/" + this.arbsell;
					break;
				case 10:
					e.TotalValue = this.aibuy + "/" + this.aisell;
					break;
				case 11:
					e.TotalValue = this.aybuy + "/" + this.aysell;
					break;
				case 12:
					e.TotalValue = this.ambuy + "/" + this.amsell;
					break;
				case 13:
					e.TotalValue = this.appbuy + "/" + this.appsell;
					break;
				case 14:
					e.TotalValue = this.albuy + "/" + this.alsell;
					break;
				case 15:
					e.TotalValue = this.aMAbuy + "/" + this.aMAsell;
					break;
				case 16:
					e.TotalValue = this.aRMbuy + "/" + this.aRMsell;
					break;
				case 17:
					e.TotalValue = this.aSRbuy + "/" + this.aSRsell;
					break;
				}
			}
		}

		private void button6_Click(object sender, System.EventArgs e)
		{
			string text = this.comboBoxInstrument.Text;
			if (!string.IsNullOrWhiteSpace(text))
			{
				double num = (this.comboBoxDirector.SelectedIndex == 0) ? System.Convert.ToDouble(this.labelUpper.Text) : System.Convert.ToDouble(this.labelLower.Text);
				System.Data.DataRow dataRow = this.dtMarketData.Rows.Find(this.comboBoxInstrument.Text);
				if (dataRow != null)
				{
					num = (double)dataRow["卖价"];
				}
				int num2 = (int)this.numericUpDownVolume.Value;
				if (num2 > 0)
				{
					System.Data.DataRow[] array = this.dt_account_dg.Select("选择='True'");
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i]["登录状态"].ToString() == "已登录")
						{
							string key = array[i]["投资者帐户"].ToString();
							this.Dic_TradeApi[key].listTransDatas.Add(new string[]
							{
								text,
								"Buy",
								"closetoday",
								num.ToString(),
								num2.ToString(),
								this.Dic_TradeApi[key].UpOrderRef().ToString()
							});
						}
					}
				}
			}
		}

		private void button7_Click(object sender, System.EventArgs e)
		{
			string text = this.comboBoxInstrument.Text;
			if (!string.IsNullOrWhiteSpace(text))
			{
				double num = (this.comboBoxDirector.SelectedIndex == 0) ? System.Convert.ToDouble(this.labelUpper.Text) : System.Convert.ToDouble(this.labelLower.Text);
				System.Data.DataRow dataRow = this.dtMarketData.Rows.Find(this.comboBoxInstrument.Text);
				if (dataRow != null)
				{
					num = (double)dataRow["买价"];
				}
				int num2 = (int)this.numericUpDownVolume.Value;
				if (num2 > 0)
				{
					System.Data.DataRow[] array = this.dt_account_dg.Select("选择='True'");
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i]["登录状态"].ToString() == "已登录")
						{
							string key = array[i]["投资者帐户"].ToString();
							this.Dic_TradeApi[key].listTransDatas.Add(new string[]
							{
								text,
								"Sell",
								"closetoday",
								num.ToString(),
								num2.ToString(),
								this.Dic_TradeApi[key].UpOrderRef().ToString()
							});
						}
					}
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.components = new Container();
			StyleFormatCondition styleFormatCondition = new StyleFormatCondition();
			StyleFormatCondition styleFormatCondition2 = new StyleFormatCondition();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(frmMainFrame));
			StyleFormatCondition styleFormatCondition3 = new StyleFormatCondition();
			StyleFormatCondition styleFormatCondition4 = new StyleFormatCondition();
			StyleFormatCondition styleFormatCondition5 = new StyleFormatCondition();
			StyleFormatCondition styleFormatCondition6 = new StyleFormatCondition();
			this.bandedGridColumn1 = new BandedGridColumn();
			this.repositoryItemCheckEdit3 = new RepositoryItemCheckEdit();
			this.choose = new BandedGridColumn();
			this.bandedGridColumn4 = new GridColumn();
			this.gridColumn12 = new GridColumn();
			this.gridColumn19 = new GridColumn();
			this.gridColumn40 = new GridColumn();
			this.repositoryItemSpinEdit1 = new RepositoryItemSpinEdit();
			this.panel1 = new System.Windows.Forms.Panel();
			this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
			this.domainUpDown1 = new System.Windows.Forms.DomainUpDown();
			this.butSelAll = new System.Windows.Forms.Button();
			this.butLoginOut = new System.Windows.Forms.Button();
			this.butLogin = new System.Windows.Forms.Button();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.Revoke = new System.Windows.Forms.ToolStripMenuItem();
			this.FullRevoke = new System.Windows.Forms.ToolStripMenuItem();
			this.导出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.columnHeader45 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader53 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader46 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader47 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader48 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader49 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader50 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader51 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader54 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader15 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader17 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader18 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader19 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader20 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader21 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader22 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader23 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader24 = new System.Windows.Forms.ColumnHeader();
			this.comboBoxErrMsg = new System.Windows.Forms.ComboBox();
			this.radioButtonMd = new System.Windows.Forms.RadioButton();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.buttonMarketPrice = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.AccountSetTSMI = new System.Windows.Forms.ToolStripMenuItem();
			this.PassWordTSMI = new System.Windows.Forms.ToolStripMenuItem();
			this.TradingAccountTSMI = new System.Windows.Forms.ToolStripMenuItem();
			this.TSMI_Position = new System.Windows.Forms.ToolStripMenuItem();
			this.PositionTradeByAccountTSMI = new System.Windows.Forms.ToolStripMenuItem();
			this.按帐户分组ToolStripMenuItem = new System.Windows.Forms.ToolStripSeparator();
			this.InvestorPositionTSMI = new System.Windows.Forms.ToolStripMenuItem();
			this.CodeSetTSMI = new System.Windows.Forms.ToolStripMenuItem();
			this.SubAccountSetTSMI = new System.Windows.Forms.ToolStripMenuItem();
			this.结算单ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.PositionCheckTSMI = new System.Windows.Forms.ToolStripMenuItem();
			this.TSMI_BeginListen = new System.Windows.Forms.ToolStripMenuItem();
			this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.xdgMainAccount = new GridControl();
			this.bgcMainAccount = new BandedGridView();
			this.gridBand3 = new GridBand();
			this.bandedGridColumn2 = new BandedGridColumn();
			this.bandedGridColumn3 = new BandedGridColumn();
			this.gridBand4 = new GridBand();
			this.bandedGridColumn16 = new BandedGridColumn();
			this.bandedGridColumn14 = new BandedGridColumn();
			this.bandedGridColumn13 = new BandedGridColumn();
			this.bandedGridColumn5 = new BandedGridColumn();
			this.bandedGridColumn9 = new BandedGridColumn();
			this.bandedGridColumn10 = new BandedGridColumn();
			this.bandedGridColumn18 = new BandedGridColumn();
			this.bandedGridColumn19 = new BandedGridColumn();
			this.bandedGridColumn17 = new BandedGridColumn();
			this.bandedGridColumn30 = new BandedGridColumn();
			this.bandedGridColumn29 = new BandedGridColumn();
			this.bandedGridColumn28 = new BandedGridColumn();
			this.bandedGridColumn27 = new BandedGridColumn();
			this.bandedGridColumn26 = new BandedGridColumn();
			this.bandedGridColumn25 = new BandedGridColumn();
			this.bandedGridColumn24 = new BandedGridColumn();
			this.bandedGridColumn23 = new BandedGridColumn();
			this.bandedGridColumn33 = new BandedGridColumn();
			this.bandedGridColumn32 = new BandedGridColumn();
			this.bandedGridColumn31 = new BandedGridColumn();
			this.bandedGridColumn34 = new BandedGridColumn();
			this.xdgAccount = new GridControl();
			this.bgvAccount = new BandedGridView();
			this.gridBand1 = new GridBand();
			this.colAccount = new BandedGridColumn();
			this.colInvestor = new BandedGridColumn();
			this.gridBand2 = new GridBand();
			this.colState = new BandedGridColumn();
			this.colBalance = new BandedGridColumn();
			this.colCloseProfit = new BandedGridColumn();
			this.colPositionProfit = new BandedGridColumn();
			this.bandedGridColumn12 = new BandedGridColumn();
			this.bandedGridColumn11 = new BandedGridColumn();
			this.bandedGridColumn22 = new BandedGridColumn();
			this.bandedGridColumn21 = new BandedGridColumn();
			this.bandedGridColumn20 = new BandedGridColumn();
			this.bandedGridColumn47 = new BandedGridColumn();
			this.bandedGridColumn46 = new BandedGridColumn();
			this.bandedGridColumn45 = new BandedGridColumn();
			this.bandedGridColumn44 = new BandedGridColumn();
			this.bandedGridColumn43 = new BandedGridColumn();
			this.bandedGridColumn42 = new BandedGridColumn();
			this.bandedGridColumn41 = new BandedGridColumn();
			this.bandedGridColumn40 = new BandedGridColumn();
			this.bandedGridColumn39 = new BandedGridColumn();
			this.bandedGridColumn38 = new BandedGridColumn();
			this.bandedGridColumn37 = new BandedGridColumn();
			this.bandedGridColumn36 = new BandedGridColumn();
			this.colPreBalance = new BandedGridColumn();
			this.colCommission = new BandedGridColumn();
			this.colCurrMargin = new BandedGridColumn();
			this.colFrozenCash = new BandedGridColumn();
			this.colAvailable = new BandedGridColumn();
			this.colDeposit = new BandedGridColumn();
			this.colWithdraw = new BandedGridColumn();
			this.colFrozenMargin = new BandedGridColumn();
			this.numericUpDownPrice = new System.Windows.Forms.NumericUpDown();
			this.buttonOrder = new System.Windows.Forms.Button();
			this.label7 = new System.Windows.Forms.Label();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.label15 = new System.Windows.Forms.Label();
			this.numericUpDownVolume = new System.Windows.Forms.NumericUpDown();
			this.labelVolumeMax = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.labelLower = new System.Windows.Forms.Label();
			this.buttonPrice = new System.Windows.Forms.Button();
			this.labelUpper = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.tab = new DraggableTabControl();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.columnHeader36 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader37 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader38 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader39 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader40 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader41 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader42 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader43 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader44 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader52 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader55 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader56 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader57 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader58 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader59 = new System.Windows.Forms.ColumnHeader();
			this.account = new System.Windows.Forms.ColumnHeader();
			this.num = new System.Windows.Forms.ColumnHeader();
			this.code = new System.Windows.Forms.ColumnHeader();
			this.directortype = new System.Windows.Forms.ColumnHeader();
			this.offsetflag = new System.Windows.Forms.ColumnHeader();
			this.status = new System.Windows.Forms.ColumnHeader();
			this.price = new System.Windows.Forms.ColumnHeader();
			this.orderhand = new System.Windows.Forms.ColumnHeader();
			this.notrade = new System.Windows.Forms.ColumnHeader();
			this.tradehand = new System.Windows.Forms.ColumnHeader();
			this.ordertime = new System.Windows.Forms.ColumnHeader();
			this.tradetime = new System.Windows.Forms.ColumnHeader();
			this.tradeprice = new System.Windows.Forms.ColumnHeader();
			this.detailstatus = new System.Windows.Forms.ColumnHeader();
			this.custome = new System.Windows.Forms.ColumnHeader();
			this.columnHeader34 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader25 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader26 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader27 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader28 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader29 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader30 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader31 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader32 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader33 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader35 = new System.Windows.Forms.ColumnHeader();
			this.tmMoneyRefresh = new System.Windows.Forms.Timer(this.components);
			this.spcAccount = new SplitContainerControl();
			this.button5 = new System.Windows.Forms.Button();
			this.comboBoxOffset = new ComboBoxEdit();
			this.comboBoxDirector = new ComboBoxEdit();
			this.comboBoxInstrument = new ComboBoxEdit();
			this.spcPosition = new SplitContainerControl();
			this.xtraTabControl1 = new XtraTabControl();
			this.xtabTrade = new XtraTabPage();
			this.rdoError = new System.Windows.Forms.RadioButton();
			this.rdoRevoke = new System.Windows.Forms.RadioButton();
			this.rdoDeal = new System.Windows.Forms.RadioButton();
			this.rdoSuspend = new System.Windows.Forms.RadioButton();
			this.rdoAll = new System.Windows.Forms.RadioButton();
			this.xdgTrade = new GridControl();
			this.gvTrade = new GridView();
			this.bandedGridColumn6 = new GridColumn();
			this.bandedGridColumn7 = new GridColumn();
			this.bandedGridColumn8 = new GridColumn();
			this.gridColumn1 = new GridColumn();
			this.gridColumn2 = new GridColumn();
			this.gridColumn3 = new GridColumn();
			this.gridColumn4 = new GridColumn();
			this.gridColumn5 = new GridColumn();
			this.gridColumn6 = new GridColumn();
			this.gridColumn7 = new GridColumn();
			this.gridColumn8 = new GridColumn();
			this.gridColumn9 = new GridColumn();
			this.gridColumn10 = new GridColumn();
			this.gridColumn11 = new GridColumn();
			this.bandedGridColumn15 = new GridColumn();
			this.xtabViewTrade = new XtraTabPage();
			this.xdgDeal = new GridControl();
			this.contextMenuStrip4 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.导出数据ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.gvDeal = new GridView();
			this.gridColumn13 = new GridColumn();
			this.gridColumn14 = new GridColumn();
			this.gridColumn15 = new GridColumn();
			this.gridColumn16 = new GridColumn();
			this.gridColumn18 = new GridColumn();
			this.gridColumn21 = new GridColumn();
			this.gridColumn23 = new GridColumn();
			this.gridColumn24 = new GridColumn();
			this.gridColumn25 = new GridColumn();
			this.gridColumn26 = new GridColumn();
			this.gridColumn17 = new GridColumn();
			this.spcMain = new SplitContainerControl();
			this.spcMainAccount = new SplitContainerControl();
			this.btnMainSelAll = new System.Windows.Forms.Button();
			this.butMainLoginOut = new System.Windows.Forms.Button();
			this.butMainLogin = new System.Windows.Forms.Button();
			this.spcMainPosition = new SplitContainerControl();
			this.tabMain = new DraggableTabControl();
			this.xtraTabControl2 = new XtraTabControl();
			this.xtraTabPage1 = new XtraTabPage();
			this.rdoMainRevoke = new System.Windows.Forms.RadioButton();
			this.rdoMainDeal = new System.Windows.Forms.RadioButton();
			this.rdoMainSuspend = new System.Windows.Forms.RadioButton();
			this.rdoMainAll = new System.Windows.Forms.RadioButton();
			this.xdgMianTrade = new GridControl();
			this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.导出数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.gvMainTrade = new GridView();
			this.gridColumn20 = new GridColumn();
			this.gridColumn22 = new GridColumn();
			this.gridColumn27 = new GridColumn();
			this.gridColumn28 = new GridColumn();
			this.gridColumn29 = new GridColumn();
			this.gridColumn30 = new GridColumn();
			this.gridColumn31 = new GridColumn();
			this.gridColumn32 = new GridColumn();
			this.gridColumn33 = new GridColumn();
			this.gridColumn34 = new GridColumn();
			this.gridColumn35 = new GridColumn();
			this.gridColumn36 = new GridColumn();
			this.gridColumn37 = new GridColumn();
			this.gridColumn38 = new GridColumn();
			this.gridColumn39 = new GridColumn();
			this.xtraTabPage2 = new XtraTabPage();
			this.xdgMainDeal = new GridControl();
			this.contextMenuStrip3 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.导出数据ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.gvMainDeal = new GridView();
			this.gridColumn41 = new GridColumn();
			this.gridColumn42 = new GridColumn();
			this.gridColumn43 = new GridColumn();
			this.gridColumn44 = new GridColumn();
			this.gridColumn45 = new GridColumn();
			this.gridColumn46 = new GridColumn();
			this.gridColumn47 = new GridColumn();
			this.gridColumn48 = new GridColumn();
			this.gridColumn49 = new GridColumn();
			this.gridColumn50 = new GridColumn();
			this.splitContainerControl2 = new SplitContainerControl();
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			this.radioButton2 = new System.Windows.Forms.RadioButton();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.button6 = new System.Windows.Forms.Button();
			this.button7 = new System.Windows.Forms.Button();
			((ISupportInitialize)this.repositoryItemCheckEdit3).BeginInit();
			((ISupportInitialize)this.repositoryItemSpinEdit1).BeginInit();
			this.panel1.SuspendLayout();
			this.contextMenuStrip1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			((ISupportInitialize)this.xdgMainAccount).BeginInit();
			((ISupportInitialize)this.bgcMainAccount).BeginInit();
			((ISupportInitialize)this.xdgAccount).BeginInit();
			((ISupportInitialize)this.bgvAccount).BeginInit();
			((ISupportInitialize)this.numericUpDownPrice).BeginInit();
			((ISupportInitialize)this.numericUpDownVolume).BeginInit();
			((ISupportInitialize)this.spcAccount).BeginInit();
			this.spcAccount.SuspendLayout();
			((ISupportInitialize)this.comboBoxOffset.Properties).BeginInit();
			((ISupportInitialize)this.comboBoxDirector.Properties).BeginInit();
			((ISupportInitialize)this.comboBoxInstrument.Properties).BeginInit();
			((ISupportInitialize)this.spcPosition).BeginInit();
			this.spcPosition.SuspendLayout();
			((ISupportInitialize)this.xtraTabControl1).BeginInit();
			this.xtraTabControl1.SuspendLayout();
			this.xtabTrade.SuspendLayout();
			((ISupportInitialize)this.xdgTrade).BeginInit();
			((ISupportInitialize)this.gvTrade).BeginInit();
			this.xtabViewTrade.SuspendLayout();
			((ISupportInitialize)this.xdgDeal).BeginInit();
			this.contextMenuStrip4.SuspendLayout();
			((ISupportInitialize)this.gvDeal).BeginInit();
			((ISupportInitialize)this.spcMain).BeginInit();
			this.spcMain.SuspendLayout();
			((ISupportInitialize)this.spcMainAccount).BeginInit();
			this.spcMainAccount.SuspendLayout();
			((ISupportInitialize)this.spcMainPosition).BeginInit();
			this.spcMainPosition.SuspendLayout();
			((ISupportInitialize)this.xtraTabControl2).BeginInit();
			this.xtraTabControl2.SuspendLayout();
			this.xtraTabPage1.SuspendLayout();
			((ISupportInitialize)this.xdgMianTrade).BeginInit();
			this.contextMenuStrip2.SuspendLayout();
			((ISupportInitialize)this.gvMainTrade).BeginInit();
			this.xtraTabPage2.SuspendLayout();
			((ISupportInitialize)this.xdgMainDeal).BeginInit();
			this.contextMenuStrip3.SuspendLayout();
			((ISupportInitialize)this.gvMainDeal).BeginInit();
			((ISupportInitialize)this.splitContainerControl2).BeginInit();
			this.splitContainerControl2.SuspendLayout();
			base.SuspendLayout();
			this.bandedGridColumn1.Caption = "选择";
			this.bandedGridColumn1.ColumnEdit = this.repositoryItemCheckEdit3;
			this.bandedGridColumn1.FieldName = "选择";
			this.bandedGridColumn1.Name = "bandedGridColumn1";
			this.bandedGridColumn1.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
			this.bandedGridColumn1.Visible = true;
			this.bandedGridColumn1.Width = 35;
			this.repositoryItemCheckEdit3.AllowFocused = false;
			this.repositoryItemCheckEdit3.AllowHtmlDraw = DefaultBoolean.False;
			this.repositoryItemCheckEdit3.AutoHeight = false;
			this.repositoryItemCheckEdit3.Name = "repositoryItemCheckEdit3";
			this.repositoryItemCheckEdit3.NullStyle = StyleIndeterminate.Unchecked;
			this.repositoryItemCheckEdit3.ValueUnchecked = null;
			this.choose.Caption = "选择";
			this.choose.ColumnEdit = this.repositoryItemCheckEdit3;
			this.choose.FieldName = "选择";
			this.choose.Name = "choose";
			this.choose.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
			this.choose.Visible = true;
			this.choose.Width = 41;
			this.bandedGridColumn4.AppearanceCell.ForeColor = System.Drawing.Color.DodgerBlue;
			this.bandedGridColumn4.AppearanceCell.Options.UseForeColor = true;
			this.bandedGridColumn4.Caption = "投资者";
			this.bandedGridColumn4.FieldName = "投资者";
			this.bandedGridColumn4.Name = "bandedGridColumn4";
			this.bandedGridColumn4.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn4.OptionsFilter.FilterPopupMode = FilterPopupMode.List;
			this.bandedGridColumn4.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
			this.bandedGridColumn4.Visible = true;
			this.bandedGridColumn4.VisibleIndex = 0;
			this.bandedGridColumn4.Width = 91;
			this.gridColumn12.AppearanceCell.ForeColor = System.Drawing.Color.DodgerBlue;
			this.gridColumn12.AppearanceCell.Options.UseForeColor = true;
			this.gridColumn12.Caption = "投资者";
			this.gridColumn12.FieldName = "投资者";
			this.gridColumn12.Name = "gridColumn12";
			this.gridColumn12.OptionsColumn.AllowEdit = false;
			this.gridColumn12.OptionsFilter.FilterPopupMode = FilterPopupMode.List;
			this.gridColumn12.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
			this.gridColumn12.Visible = true;
			this.gridColumn12.VisibleIndex = 0;
			this.gridColumn12.Width = 91;
			this.gridColumn19.AppearanceCell.ForeColor = System.Drawing.Color.DodgerBlue;
			this.gridColumn19.AppearanceCell.Options.UseForeColor = true;
			this.gridColumn19.Caption = "投资者";
			this.gridColumn19.FieldName = "投资者";
			this.gridColumn19.Name = "gridColumn19";
			this.gridColumn19.OptionsColumn.AllowEdit = false;
			this.gridColumn19.OptionsFilter.FilterPopupMode = FilterPopupMode.List;
			this.gridColumn19.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
			this.gridColumn19.Visible = true;
			this.gridColumn19.VisibleIndex = 0;
			this.gridColumn19.Width = 91;
			this.gridColumn40.AppearanceCell.ForeColor = System.Drawing.Color.DodgerBlue;
			this.gridColumn40.AppearanceCell.Options.UseForeColor = true;
			this.gridColumn40.Caption = "投资者";
			this.gridColumn40.FieldName = "投资者";
			this.gridColumn40.Name = "gridColumn40";
			this.gridColumn40.OptionsColumn.AllowEdit = false;
			this.gridColumn40.OptionsFilter.FilterPopupMode = FilterPopupMode.List;
			this.gridColumn40.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
			this.gridColumn40.Visible = true;
			this.gridColumn40.VisibleIndex = 0;
			this.gridColumn40.Width = 91;
			this.repositoryItemSpinEdit1.AutoHeight = false;
			this.repositoryItemSpinEdit1.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton()
			});
			this.repositoryItemSpinEdit1.Name = "repositoryItemSpinEdit1";
			this.panel1.Controls.Add(this.dateTimePicker1);
			this.panel1.Controls.Add(this.domainUpDown1);
			this.panel1.Location = new System.Drawing.Point(6, 39);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(200, 174);
			this.panel1.TabIndex = 0;
			this.dateTimePicker1.Location = new System.Drawing.Point(3, 103);
			this.dateTimePicker1.Name = "dateTimePicker1";
			this.dateTimePicker1.Size = new System.Drawing.Size(200, 21);
			this.dateTimePicker1.TabIndex = 2;
			this.domainUpDown1.Location = new System.Drawing.Point(33, 29);
			this.domainUpDown1.Name = "domainUpDown1";
			this.domainUpDown1.Size = new System.Drawing.Size(120, 21);
			this.domainUpDown1.TabIndex = 1;
			this.domainUpDown1.Text = "domainUpDown1";
			this.butSelAll.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.butSelAll.Location = new System.Drawing.Point(629, 3);
			this.butSelAll.Name = "butSelAll";
			this.butSelAll.Size = new System.Drawing.Size(62, 23);
			this.butSelAll.TabIndex = 15;
			this.butSelAll.Text = "全选";
			this.butSelAll.UseVisualStyleBackColor = true;
			this.butSelAll.Click += new System.EventHandler(this.butSelAll_Click);
			this.butLoginOut.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.butLoginOut.Location = new System.Drawing.Point(629, 64);
			this.butLoginOut.Name = "butLoginOut";
			this.butLoginOut.Size = new System.Drawing.Size(62, 23);
			this.butLoginOut.TabIndex = 12;
			this.butLoginOut.Text = "注销";
			this.butLoginOut.UseVisualStyleBackColor = true;
			this.butLoginOut.Click += new System.EventHandler(this.butLoginOut_Click);
			this.butLogin.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.butLogin.Location = new System.Drawing.Point(629, 33);
			this.butLogin.Name = "butLogin";
			this.butLogin.Size = new System.Drawing.Size(62, 23);
			this.butLogin.TabIndex = 11;
			this.butLogin.Text = "登录";
			this.butLogin.UseVisualStyleBackColor = true;
			this.butLogin.Click += new System.EventHandler(this.butLogin_Click);
			this.contextMenuStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
			{
				this.Revoke,
				this.FullRevoke,
				this.导出ToolStripMenuItem
			});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
			this.contextMenuStrip1.ShowImageMargin = false;
			this.contextMenuStrip1.Size = new System.Drawing.Size(100, 70);
			this.Revoke.Name = "Revoke";
			this.Revoke.Size = new System.Drawing.Size(99, 22);
			this.Revoke.Text = "撤单";
			this.Revoke.Click += new System.EventHandler(this.Revoke_Click);
			this.FullRevoke.Name = "FullRevoke";
			this.FullRevoke.Size = new System.Drawing.Size(99, 22);
			this.FullRevoke.Text = "全撤";
			this.FullRevoke.Click += new System.EventHandler(this.FullRevoke_Click);
			this.导出ToolStripMenuItem.Name = "导出ToolStripMenuItem";
			this.导出ToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
			this.导出ToolStripMenuItem.Text = "导出数据";
			this.导出ToolStripMenuItem.Click += new System.EventHandler(this.导出ToolStripMenuItem_Click);
			this.columnHeader45.Text = "合约";
			this.columnHeader45.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader45.Width = 80;
			this.columnHeader53.Text = "成交时间";
			this.columnHeader53.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader53.Width = 95;
			this.columnHeader46.Text = "交易所";
			this.columnHeader46.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader47.Text = "报单编号";
			this.columnHeader47.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader47.Width = 95;
			this.columnHeader48.Text = "买卖";
			this.columnHeader48.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader49.Text = "开平";
			this.columnHeader49.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader50.Text = "价格";
			this.columnHeader50.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader50.Width = 85;
			this.columnHeader51.Text = "数量";
			this.columnHeader51.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader54.Text = "经纪公司报单编号";
			this.columnHeader54.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader54.Width = 128;
			this.columnHeader15.Text = "合约";
			this.columnHeader15.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader15.Width = 80;
			this.columnHeader17.Text = "成交时间";
			this.columnHeader17.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader17.Width = 95;
			this.columnHeader18.Text = "交易所";
			this.columnHeader18.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader19.Text = "报单编号";
			this.columnHeader19.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader19.Width = 95;
			this.columnHeader20.Text = "买卖";
			this.columnHeader20.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader21.Text = "开平";
			this.columnHeader21.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader22.Text = "价格";
			this.columnHeader22.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader22.Width = 85;
			this.columnHeader23.Text = "数量";
			this.columnHeader23.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader24.Text = "经纪公司报单编号";
			this.columnHeader24.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader24.Width = 128;
			this.comboBoxErrMsg.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			this.comboBoxErrMsg.FormattingEnabled = true;
			this.comboBoxErrMsg.Location = new System.Drawing.Point(0, 854);
			this.comboBoxErrMsg.Name = "comboBoxErrMsg";
			this.comboBoxErrMsg.Size = new System.Drawing.Size(629, 20);
			this.comboBoxErrMsg.TabIndex = 10;
			this.radioButtonMd.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.radioButtonMd.AutoCheck = false;
			this.radioButtonMd.AutoSize = true;
			this.radioButtonMd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.radioButtonMd.ForeColor = System.Drawing.Color.Red;
			this.radioButtonMd.Location = new System.Drawing.Point(990, 856);
			this.radioButtonMd.Name = "radioButtonMd";
			this.radioButtonMd.Size = new System.Drawing.Size(46, 16);
			this.radioButtonMd.TabIndex = 11;
			this.radioButtonMd.Text = "行情";
			this.radioButtonMd.UseVisualStyleBackColor = true;
			this.toolTip1.AutoPopDelay = 2000;
			this.toolTip1.InitialDelay = 500;
			this.toolTip1.ReshowDelay = 100;
			this.buttonMarketPrice.Location = new System.Drawing.Point(272, 33);
			this.buttonMarketPrice.Name = "buttonMarketPrice";
			this.buttonMarketPrice.Size = new System.Drawing.Size(65, 23);
			this.buttonMarketPrice.TabIndex = 74;
			this.buttonMarketPrice.Text = "市价";
			this.toolTip1.SetToolTip(this.buttonMarketPrice, "以涨跌停下单");
			this.buttonMarketPrice.UseVisualStyleBackColor = true;
			this.buttonMarketPrice.Click += new System.EventHandler(this.buttonMarketPrice_Click);
			this.button1.ForeColor = System.Drawing.Color.FromArgb(192, 0, 0);
			this.button1.Location = new System.Drawing.Point(272, 64);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(71, 31);
			this.button1.TabIndex = 77;
			this.button1.Text = "买 开";
			this.toolTip1.SetToolTip(this.button1, "以涨跌停下单");
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			this.button2.ForeColor = System.Drawing.Color.FromArgb(0, 192, 0);
			this.button2.Location = new System.Drawing.Point(190, 64);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(71, 31);
			this.button2.TabIndex = 78;
			this.button2.Text = "卖 开";
			this.toolTip1.SetToolTip(this.button2, "以涨跌停下单");
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			this.button3.ForeColor = System.Drawing.Color.FromArgb(0, 192, 0);
			this.button3.Location = new System.Drawing.Point(273, 140);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(71, 31);
			this.button3.TabIndex = 79;
			this.button3.Text = "平 买";
			this.toolTip1.SetToolTip(this.button3, "以涨跌停下单");
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			this.button4.ForeColor = System.Drawing.Color.FromArgb(192, 0, 0);
			this.button4.Location = new System.Drawing.Point(190, 140);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(71, 31);
			this.button4.TabIndex = 80;
			this.button4.Text = "平 卖";
			this.toolTip1.SetToolTip(this.button4, "以涨跌停下单");
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.button4_Click);
			this.menuStrip1.BackColor = System.Drawing.Color.Transparent;
			this.menuStrip1.Font = new System.Drawing.Font("宋体", 9f);
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
			{
				this.ToolStripMenuItem,
				this.TSMI_Position,
				this.结算单ToolStripMenuItem,
				this.PositionCheckTSMI,
				this.TSMI_BeginListen,
				this.帮助ToolStripMenuItem
			});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.menuStrip1.Size = new System.Drawing.Size(1045, 40);
			this.menuStrip1.TabIndex = 12;
			this.menuStrip1.Text = "menuStrip1";
			this.ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[]
			{
				this.AccountSetTSMI,
				this.PassWordTSMI,
				this.TradingAccountTSMI
			});
			this.ToolStripMenuItem.Image = Resources.chinaz9;
			this.ToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.ToolStripMenuItem.Name = "ToolStripMenuItem";
			this.ToolStripMenuItem.Size = new System.Drawing.Size(97, 36);
			this.ToolStripMenuItem.Text = "账户编辑";
			this.AccountSetTSMI.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.AccountSetTSMI.Name = "AccountSetTSMI";
			this.AccountSetTSMI.Size = new System.Drawing.Size(142, 22);
			this.AccountSetTSMI.Text = "添加账户";
			this.AccountSetTSMI.Click += new System.EventHandler(this.AccountSetTSMI_Click);
			this.PassWordTSMI.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.PassWordTSMI.Name = "PassWordTSMI";
			this.PassWordTSMI.Size = new System.Drawing.Size(142, 22);
			this.PassWordTSMI.Text = "修改登录密码";
			this.PassWordTSMI.Click += new System.EventHandler(this.PassWordTSMI_Click);
			this.TradingAccountTSMI.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.TradingAccountTSMI.Name = "TradingAccountTSMI";
			this.TradingAccountTSMI.Size = new System.Drawing.Size(142, 22);
			this.TradingAccountTSMI.Text = "刷新账户资金";
			this.TradingAccountTSMI.Click += new System.EventHandler(this.TradingAccountTSMI_Click);
			this.TSMI_Position.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[]
			{
				this.PositionTradeByAccountTSMI,
				this.按帐户分组ToolStripMenuItem,
				this.InvestorPositionTSMI,
				this.CodeSetTSMI,
				this.SubAccountSetTSMI
			});
			this.TSMI_Position.Image = Resources.Database;
			this.TSMI_Position.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.TSMI_Position.Name = "TSMI_Position";
			this.TSMI_Position.Size = new System.Drawing.Size(97, 36);
			this.TSMI_Position.Text = "下单配置";
			this.TSMI_Position.Click += new System.EventHandler(this.TSMI_Position_Click);
			this.PositionTradeByAccountTSMI.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.PositionTradeByAccountTSMI.Name = "PositionTradeByAccountTSMI";
			this.PositionTradeByAccountTSMI.Size = new System.Drawing.Size(154, 22);
			this.PositionTradeByAccountTSMI.Text = "帐户下单配置";
			this.PositionTradeByAccountTSMI.Click += new System.EventHandler(this.PositionTradeByAccountTSMI_Click);
			this.按帐户分组ToolStripMenuItem.Name = "按帐户分组ToolStripMenuItem";
			this.按帐户分组ToolStripMenuItem.Size = new System.Drawing.Size(151, 6);
			this.InvestorPositionTSMI.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.InvestorPositionTSMI.Name = "InvestorPositionTSMI";
			this.InvestorPositionTSMI.Size = new System.Drawing.Size(154, 22);
			this.InvestorPositionTSMI.Text = "持仓刷新";
			this.InvestorPositionTSMI.Click += new System.EventHandler(this.InvestorPositionTSMI_Click);
			this.CodeSetTSMI.Name = "CodeSetTSMI";
			this.CodeSetTSMI.Size = new System.Drawing.Size(154, 22);
			this.CodeSetTSMI.Text = "合约设置";
			this.CodeSetTSMI.Click += new System.EventHandler(this.CodeSetTSMI_Click);
			this.SubAccountSetTSMI.Name = "SubAccountSetTSMI";
			this.SubAccountSetTSMI.Size = new System.Drawing.Size(154, 22);
			this.SubAccountSetTSMI.Text = "子帐户监听设置";
			this.SubAccountSetTSMI.Click += new System.EventHandler(this.SubAccountSetTSMI_Click);
			this.结算单ToolStripMenuItem.Image = Resources.简约贴纸风格系列图标66;
			this.结算单ToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.结算单ToolStripMenuItem.Name = "结算单ToolStripMenuItem";
			this.结算单ToolStripMenuItem.Size = new System.Drawing.Size(85, 36);
			this.结算单ToolStripMenuItem.Text = "结算单";
			this.结算单ToolStripMenuItem.Click += new System.EventHandler(this.结算单ToolStripMenuItem_Click);
			this.PositionCheckTSMI.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.PositionCheckTSMI.Name = "PositionCheckTSMI";
			this.PositionCheckTSMI.Size = new System.Drawing.Size(65, 36);
			this.PositionCheckTSMI.Text = "持仓核对";
			this.PositionCheckTSMI.Visible = false;
			this.PositionCheckTSMI.Click += new System.EventHandler(this.PositionCheckTSMI_Click);
			this.TSMI_BeginListen.Image = Resources._11;
			this.TSMI_BeginListen.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.TSMI_BeginListen.Name = "TSMI_BeginListen";
			this.TSMI_BeginListen.Size = new System.Drawing.Size(97, 36);
			this.TSMI_BeginListen.Text = "启动监听";
			this.TSMI_BeginListen.Click += new System.EventHandler(this.TSMI_BeginListen_Click);
			this.帮助ToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
			this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(41, 36);
			this.帮助ToolStripMenuItem.Text = "帮助";
			this.帮助ToolStripMenuItem.Visible = false;
			this.帮助ToolStripMenuItem.Click += new System.EventHandler(this.帮助ToolStripMenuItem_Click);
			this.xdgMainAccount.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			this.xdgMainAccount.Location = new System.Drawing.Point(0, 0);
			this.xdgMainAccount.MainView = this.bgcMainAccount;
			this.xdgMainAccount.Name = "xdgMainAccount";
			this.xdgMainAccount.Size = new System.Drawing.Size(767, 330);
			this.xdgMainAccount.TabIndex = 17;
			this.xdgMainAccount.ViewCollection.AddRange(new BaseView[]
			{
				this.bgcMainAccount
			});
			this.bgcMainAccount.Appearance.BandPanel.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.bgcMainAccount.Appearance.BandPanel.BackColor2 = System.Drawing.Color.LightSkyBlue;
			this.bgcMainAccount.Appearance.BandPanel.BorderColor = System.Drawing.Color.White;
			this.bgcMainAccount.Appearance.BandPanel.ForeColor = System.Drawing.Color.Black;
			this.bgcMainAccount.Appearance.BandPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.bgcMainAccount.Appearance.BandPanel.Options.UseBackColor = true;
			this.bgcMainAccount.Appearance.BandPanel.Options.UseBorderColor = true;
			this.bgcMainAccount.Appearance.BandPanel.Options.UseForeColor = true;
			this.bgcMainAccount.Appearance.BandPanelBackground.BackColor = System.Drawing.Color.FromArgb(236, 246, 255);
			this.bgcMainAccount.Appearance.BandPanelBackground.BackColor2 = System.Drawing.Color.White;
			this.bgcMainAccount.Appearance.BandPanelBackground.Options.UseBackColor = true;
			this.bgcMainAccount.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.bgcMainAccount.Appearance.ColumnFilterButton.BackColor2 = System.Drawing.Color.LightSkyBlue;
			this.bgcMainAccount.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.White;
			this.bgcMainAccount.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.White;
			this.bgcMainAccount.Appearance.ColumnFilterButton.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.bgcMainAccount.Appearance.ColumnFilterButton.Options.UseBackColor = true;
			this.bgcMainAccount.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
			this.bgcMainAccount.Appearance.ColumnFilterButton.Options.UseForeColor = true;
			this.bgcMainAccount.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(170, 216, 254);
			this.bgcMainAccount.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(170, 216, 254);
			this.bgcMainAccount.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Black;
			this.bgcMainAccount.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
			this.bgcMainAccount.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
			this.bgcMainAccount.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
			this.bgcMainAccount.Appearance.Empty.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.bgcMainAccount.Appearance.Empty.BackColor2 = System.Drawing.Color.FromArgb(224, 224, 224);
			this.bgcMainAccount.Appearance.Empty.Options.UseBackColor = true;
			this.bgcMainAccount.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.bgcMainAccount.Appearance.EvenRow.BackColor2 = System.Drawing.Color.WhiteSmoke;
			this.bgcMainAccount.Appearance.EvenRow.BorderColor = System.Drawing.Color.Transparent;
			this.bgcMainAccount.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
			this.bgcMainAccount.Appearance.EvenRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
			this.bgcMainAccount.Appearance.EvenRow.Options.UseBackColor = true;
			this.bgcMainAccount.Appearance.EvenRow.Options.UseBorderColor = true;
			this.bgcMainAccount.Appearance.EvenRow.Options.UseForeColor = true;
			this.bgcMainAccount.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(104, 184, 251);
			this.bgcMainAccount.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(104, 184, 251);
			this.bgcMainAccount.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.White;
			this.bgcMainAccount.Appearance.FilterCloseButton.Options.UseBackColor = true;
			this.bgcMainAccount.Appearance.FilterCloseButton.Options.UseBorderColor = true;
			this.bgcMainAccount.Appearance.FilterCloseButton.Options.UseForeColor = true;
			this.bgcMainAccount.Appearance.FilterPanel.BackColor = System.Drawing.Color.FromArgb(236, 246, 255);
			this.bgcMainAccount.Appearance.FilterPanel.BackColor2 = System.Drawing.Color.White;
			this.bgcMainAccount.Appearance.FilterPanel.ForeColor = System.Drawing.Color.Black;
			this.bgcMainAccount.Appearance.FilterPanel.Options.UseBackColor = true;
			this.bgcMainAccount.Appearance.FilterPanel.Options.UseForeColor = true;
			this.bgcMainAccount.Appearance.FixedLine.BackColor = System.Drawing.Color.White;
			this.bgcMainAccount.Appearance.FixedLine.Options.UseBackColor = true;
			this.bgcMainAccount.Appearance.FocusedCell.BackColor = System.Drawing.Color.Transparent;
			this.bgcMainAccount.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
			this.bgcMainAccount.Appearance.FocusedCell.Options.UseBackColor = true;
			this.bgcMainAccount.Appearance.FocusedCell.Options.UseForeColor = true;
			this.bgcMainAccount.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.bgcMainAccount.Appearance.FocusedRow.BackColor2 = System.Drawing.Color.FromArgb(224, 224, 224);
			this.bgcMainAccount.Appearance.FocusedRow.BorderColor = System.Drawing.Color.White;
			this.bgcMainAccount.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
			this.bgcMainAccount.Appearance.FocusedRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
			this.bgcMainAccount.Appearance.FocusedRow.Options.UseBackColor = true;
			this.bgcMainAccount.Appearance.FocusedRow.Options.UseBorderColor = true;
			this.bgcMainAccount.Appearance.FocusedRow.Options.UseForeColor = true;
			this.bgcMainAccount.Appearance.FooterPanel.BackColor = System.Drawing.Color.LightCyan;
			this.bgcMainAccount.Appearance.FooterPanel.BackColor2 = System.Drawing.Color.LightSkyBlue;
			this.bgcMainAccount.Appearance.FooterPanel.BorderColor = System.Drawing.Color.Transparent;
			this.bgcMainAccount.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
			this.bgcMainAccount.Appearance.FooterPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.bgcMainAccount.Appearance.FooterPanel.Options.UseBackColor = true;
			this.bgcMainAccount.Appearance.FooterPanel.Options.UseForeColor = true;
			this.bgcMainAccount.Appearance.FooterPanel.Options.UseTextOptions = true;
			this.bgcMainAccount.Appearance.FooterPanel.TextOptions.VAlignment = VertAlignment.Top;
			this.bgcMainAccount.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(104, 184, 251);
			this.bgcMainAccount.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(104, 184, 251);
			this.bgcMainAccount.Appearance.GroupButton.Options.UseBackColor = true;
			this.bgcMainAccount.Appearance.GroupButton.Options.UseBorderColor = true;
			this.bgcMainAccount.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.bgcMainAccount.Appearance.GroupFooter.BackColor2 = System.Drawing.Color.FromArgb(255, 255, 245);
			this.bgcMainAccount.Appearance.GroupFooter.BorderColor = System.Drawing.Color.White;
			this.bgcMainAccount.Appearance.GroupFooter.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.bgcMainAccount.Appearance.GroupFooter.Options.UseBackColor = true;
			this.bgcMainAccount.Appearance.GroupFooter.Options.UseBorderColor = true;
			this.bgcMainAccount.Appearance.GroupFooter.Options.UseForeColor = true;
			this.bgcMainAccount.Appearance.GroupFooter.Options.UseTextOptions = true;
			this.bgcMainAccount.Appearance.GroupFooter.TextOptions.VAlignment = VertAlignment.Top;
			this.bgcMainAccount.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(236, 246, 255);
			this.bgcMainAccount.Appearance.GroupPanel.BackColor2 = System.Drawing.Color.White;
			this.bgcMainAccount.Appearance.GroupPanel.ForeColor = System.Drawing.Color.Black;
			this.bgcMainAccount.Appearance.GroupPanel.Options.UseBackColor = true;
			this.bgcMainAccount.Appearance.GroupPanel.Options.UseForeColor = true;
			this.bgcMainAccount.Appearance.GroupRow.BackColor = System.Drawing.Color.WhiteSmoke;
			this.bgcMainAccount.Appearance.GroupRow.BackColor2 = System.Drawing.Color.Gainsboro;
			this.bgcMainAccount.Appearance.GroupRow.BorderColor = System.Drawing.Color.White;
			this.bgcMainAccount.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
			this.bgcMainAccount.Appearance.GroupRow.Options.UseBackColor = true;
			this.bgcMainAccount.Appearance.GroupRow.Options.UseBorderColor = true;
			this.bgcMainAccount.Appearance.GroupRow.Options.UseForeColor = true;
			this.bgcMainAccount.Appearance.HeaderPanelBackground.BackColor = System.Drawing.Color.FromArgb(236, 246, 255);
			this.bgcMainAccount.Appearance.HeaderPanelBackground.BackColor2 = System.Drawing.Color.White;
			this.bgcMainAccount.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.bgcMainAccount.Appearance.HideSelectionRow.BackColor2 = System.Drawing.Color.FromArgb(224, 224, 224);
			this.bgcMainAccount.Appearance.HideSelectionRow.BorderColor = System.Drawing.Color.Transparent;
			this.bgcMainAccount.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.Black;
			this.bgcMainAccount.Appearance.HideSelectionRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
			this.bgcMainAccount.Appearance.HideSelectionRow.Options.UseBackColor = true;
			this.bgcMainAccount.Appearance.HideSelectionRow.Options.UseBorderColor = true;
			this.bgcMainAccount.Appearance.HideSelectionRow.Options.UseForeColor = true;
			this.bgcMainAccount.Appearance.HorzLine.BackColor = System.Drawing.Color.White;
			this.bgcMainAccount.Appearance.HorzLine.Options.UseBackColor = true;
			this.bgcMainAccount.Appearance.OddRow.BackColor = System.Drawing.Color.Azure;
			this.bgcMainAccount.Appearance.OddRow.BorderColor = System.Drawing.Color.White;
			this.bgcMainAccount.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
			this.bgcMainAccount.Appearance.OddRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
			this.bgcMainAccount.Appearance.OddRow.Options.UseBackColor = true;
			this.bgcMainAccount.Appearance.OddRow.Options.UseBorderColor = true;
			this.bgcMainAccount.Appearance.OddRow.Options.UseForeColor = true;
			this.bgcMainAccount.Appearance.Preview.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.bgcMainAccount.Appearance.Preview.BackColor2 = System.Drawing.Color.FromArgb(255, 255, 245);
			this.bgcMainAccount.Appearance.Preview.Font = new System.Drawing.Font("Verdana", 7.5f);
			this.bgcMainAccount.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(83, 155, 215);
			this.bgcMainAccount.Appearance.Preview.Options.UseBackColor = true;
			this.bgcMainAccount.Appearance.Preview.Options.UseFont = true;
			this.bgcMainAccount.Appearance.Preview.Options.UseForeColor = true;
			this.bgcMainAccount.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.bgcMainAccount.Appearance.Row.ForeColor = System.Drawing.Color.Black;
			this.bgcMainAccount.Appearance.Row.Options.UseBackColor = true;
			this.bgcMainAccount.Appearance.Row.Options.UseForeColor = true;
			this.bgcMainAccount.Appearance.RowSeparator.BackColor = System.Drawing.Color.FromArgb(236, 246, 255);
			this.bgcMainAccount.Appearance.RowSeparator.BackColor2 = System.Drawing.Color.White;
			this.bgcMainAccount.Appearance.RowSeparator.ForeColor = System.Drawing.Color.Black;
			this.bgcMainAccount.Appearance.RowSeparator.Options.UseBackColor = true;
			this.bgcMainAccount.Appearance.RowSeparator.Options.UseForeColor = true;
			this.bgcMainAccount.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.bgcMainAccount.Appearance.SelectedRow.BackColor2 = System.Drawing.Color.FromArgb(224, 224, 224);
			this.bgcMainAccount.Appearance.SelectedRow.ForeColor = System.Drawing.Color.Black;
			this.bgcMainAccount.Appearance.SelectedRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
			this.bgcMainAccount.Appearance.SelectedRow.Options.UseBackColor = true;
			this.bgcMainAccount.Appearance.SelectedRow.Options.UseForeColor = true;
			this.bgcMainAccount.Appearance.TopNewRow.BackColor = System.Drawing.Color.White;
			this.bgcMainAccount.Appearance.TopNewRow.Options.UseBackColor = true;
			this.bgcMainAccount.Appearance.VertLine.BackColor = System.Drawing.Color.White;
			this.bgcMainAccount.Appearance.VertLine.Options.UseBackColor = true;
			this.bgcMainAccount.Bands.AddRange(new GridBand[]
			{
				this.gridBand3,
				this.gridBand4
			});
			this.bgcMainAccount.BorderStyle = BorderStyles.Simple;
			this.bgcMainAccount.Columns.AddRange(new BandedGridColumn[]
			{
				this.bandedGridColumn1,
				this.bandedGridColumn2,
				this.bandedGridColumn3,
				this.bandedGridColumn16,
				this.bandedGridColumn5,
				this.bandedGridColumn9,
				this.bandedGridColumn10,
				this.bandedGridColumn18,
				this.bandedGridColumn19,
				this.bandedGridColumn17,
				this.bandedGridColumn30,
				this.bandedGridColumn29,
				this.bandedGridColumn28,
				this.bandedGridColumn27,
				this.bandedGridColumn26,
				this.bandedGridColumn25,
				this.bandedGridColumn24,
				this.bandedGridColumn23,
				this.bandedGridColumn33,
				this.bandedGridColumn32,
				this.bandedGridColumn31,
				this.bandedGridColumn34,
				this.bandedGridColumn13,
				this.bandedGridColumn14
			});
			this.bgcMainAccount.FooterPanelHeight = 5;
			styleFormatCondition.ApplyToRow = true;
			styleFormatCondition.Column = this.bandedGridColumn1;
			styleFormatCondition.Condition = FormatConditionEnum.Equal;
			styleFormatCondition.Value1 = true;
			this.bgcMainAccount.FormatConditions.AddRange(new StyleFormatCondition[]
			{
				styleFormatCondition
			});
			this.bgcMainAccount.GridControl = this.xdgMainAccount;
			this.bgcMainAccount.GroupFooterShowMode = GroupFooterShowMode.Hidden;
			this.bgcMainAccount.HorzScrollVisibility = ScrollVisibility.Always;
			this.bgcMainAccount.Name = "bgcMainAccount";
			this.bgcMainAccount.OptionsBehavior.AutoExpandAllGroups = true;
			this.bgcMainAccount.OptionsCustomization.AllowFilter = false;
			this.bgcMainAccount.OptionsMenu.EnableColumnMenu = false;
			this.bgcMainAccount.OptionsMenu.EnableFooterMenu = false;
			this.bgcMainAccount.OptionsMenu.EnableGroupPanelMenu = false;
			this.bgcMainAccount.OptionsPrint.AutoWidth = false;
			this.bgcMainAccount.OptionsView.ColumnAutoWidth = false;
			this.bgcMainAccount.OptionsView.EnableAppearanceEvenRow = true;
			this.bgcMainAccount.OptionsView.EnableAppearanceOddRow = true;
			this.bgcMainAccount.OptionsView.ShowFooter = true;
			this.bgcMainAccount.OptionsView.ShowGroupPanel = false;
			this.bgcMainAccount.VertScrollVisibility = ScrollVisibility.Always;
			this.bgcMainAccount.RowCellStyle += new RowCellStyleEventHandler(this.bgcMainAccount_RowCellStyle);
			this.bgcMainAccount.CustomSummaryCalculate += new CustomSummaryEventHandler(this.bgcMainAccount_CustomSummaryCalculate);
			this.gridBand3.Caption = "主帐户";
			this.gridBand3.Columns.Add(this.bandedGridColumn1);
			this.gridBand3.Columns.Add(this.bandedGridColumn2);
			this.gridBand3.Columns.Add(this.bandedGridColumn3);
			this.gridBand3.Fixed = FixedStyle.Left;
			this.gridBand3.Name = "gridBand3";
			this.gridBand3.Width = 135;
			this.bandedGridColumn2.Caption = "投资者帐户";
			this.bandedGridColumn2.FieldName = "投资者帐户";
			this.bandedGridColumn2.Name = "bandedGridColumn2";
			this.bandedGridColumn2.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn2.Visible = true;
			this.bandedGridColumn2.Width = 100;
			this.bandedGridColumn3.Caption = "投资者";
			this.bandedGridColumn3.FieldName = "投资者";
			this.bandedGridColumn3.Name = "bandedGridColumn3";
			this.bandedGridColumn3.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn3.Width = 83;
			this.gridBand4.Caption = "账户详细";
			this.gridBand4.Columns.Add(this.bandedGridColumn16);
			this.gridBand4.Columns.Add(this.bandedGridColumn14);
			this.gridBand4.Columns.Add(this.bandedGridColumn13);
			this.gridBand4.Columns.Add(this.bandedGridColumn5);
			this.gridBand4.Columns.Add(this.bandedGridColumn9);
			this.gridBand4.Columns.Add(this.bandedGridColumn10);
			this.gridBand4.Columns.Add(this.bandedGridColumn18);
			this.gridBand4.Columns.Add(this.bandedGridColumn19);
			this.gridBand4.Columns.Add(this.bandedGridColumn17);
			this.gridBand4.Columns.Add(this.bandedGridColumn30);
			this.gridBand4.Columns.Add(this.bandedGridColumn29);
			this.gridBand4.Columns.Add(this.bandedGridColumn28);
			this.gridBand4.Columns.Add(this.bandedGridColumn27);
			this.gridBand4.Columns.Add(this.bandedGridColumn26);
			this.gridBand4.Columns.Add(this.bandedGridColumn25);
			this.gridBand4.Columns.Add(this.bandedGridColumn24);
			this.gridBand4.Columns.Add(this.bandedGridColumn23);
			this.gridBand4.Columns.Add(this.bandedGridColumn33);
			this.gridBand4.Columns.Add(this.bandedGridColumn32);
			this.gridBand4.Columns.Add(this.bandedGridColumn31);
			this.gridBand4.Columns.Add(this.bandedGridColumn34);
			this.gridBand4.Name = "gridBand4";
			this.gridBand4.Width = 1505;
			this.bandedGridColumn16.AppearanceCell.Options.UseTextOptions = true;
			this.bandedGridColumn16.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
			this.bandedGridColumn16.Caption = "动态权益";
			this.bandedGridColumn16.DisplayFormat.FormatType = FormatType.Numeric;
			this.bandedGridColumn16.FieldName = "动态权益";
			this.bandedGridColumn16.Name = "bandedGridColumn16";
			this.bandedGridColumn16.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn16.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Sum)
			});
			this.bandedGridColumn16.Visible = true;
			this.bandedGridColumn14.AppearanceCell.Options.UseTextOptions = true;
			this.bandedGridColumn14.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far;
			this.bandedGridColumn14.Caption = "持仓盈亏";
			this.bandedGridColumn14.FieldName = "持仓盈亏";
			this.bandedGridColumn14.GroupFormat.FormatType = FormatType.Numeric;
			this.bandedGridColumn14.Name = "bandedGridColumn14";
			this.bandedGridColumn14.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn14.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Sum)
			});
			this.bandedGridColumn14.Visible = true;
			this.bandedGridColumn13.AppearanceCell.Options.UseTextOptions = true;
			this.bandedGridColumn13.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far;
			this.bandedGridColumn13.Caption = "平仓盈亏";
			this.bandedGridColumn13.FieldName = "平仓盈亏";
			this.bandedGridColumn13.GroupFormat.FormatType = FormatType.Numeric;
			this.bandedGridColumn13.Name = "bandedGridColumn13";
			this.bandedGridColumn13.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn13.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Sum)
			});
			this.bandedGridColumn13.Visible = true;
			this.bandedGridColumn5.AppearanceCell.ForeColor = System.Drawing.Color.Red;
			this.bandedGridColumn5.AppearanceCell.Options.UseForeColor = true;
			this.bandedGridColumn5.AppearanceCell.Options.UseTextOptions = true;
			this.bandedGridColumn5.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
			this.bandedGridColumn5.Caption = "登录状态";
			this.bandedGridColumn5.FieldName = "登录状态";
			this.bandedGridColumn5.Name = "bandedGridColumn5";
			this.bandedGridColumn5.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn5.Visible = true;
			this.bandedGridColumn5.Width = 80;
			this.bandedGridColumn9.AppearanceCell.Options.UseTextOptions = true;
			this.bandedGridColumn9.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far;
			this.bandedGridColumn9.Caption = "IF持仓";
			this.bandedGridColumn9.FieldName = "IF持仓";
			this.bandedGridColumn9.Name = "bandedGridColumn9";
			this.bandedGridColumn9.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn9.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Custom, "IF持仓", "", "1")
			});
			this.bandedGridColumn9.Visible = true;
			this.bandedGridColumn9.Width = 60;
			this.bandedGridColumn10.AppearanceCell.Options.UseTextOptions = true;
			this.bandedGridColumn10.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far;
			this.bandedGridColumn10.AppearanceHeader.Options.UseTextOptions = true;
			this.bandedGridColumn10.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
			this.bandedGridColumn10.Caption = "IC持仓";
			this.bandedGridColumn10.FieldName = "IC持仓";
			this.bandedGridColumn10.Name = "bandedGridColumn10";
			this.bandedGridColumn10.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn10.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Custom, "IC持仓", "", "2")
			});
			this.bandedGridColumn10.Visible = true;
			this.bandedGridColumn10.Width = 60;
			this.bandedGridColumn18.AppearanceCell.Options.UseTextOptions = true;
			this.bandedGridColumn18.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far;
			this.bandedGridColumn18.AppearanceHeader.Options.UseTextOptions = true;
			this.bandedGridColumn18.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
			this.bandedGridColumn18.Caption = "IH持仓";
			this.bandedGridColumn18.FieldName = "IH持仓";
			this.bandedGridColumn18.Name = "bandedGridColumn18";
			this.bandedGridColumn18.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn18.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Custom, "IH持仓", "", "3")
			});
			this.bandedGridColumn18.Visible = true;
			this.bandedGridColumn18.Width = 60;
			this.bandedGridColumn19.AppearanceCell.Options.UseTextOptions = true;
			this.bandedGridColumn19.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far;
			this.bandedGridColumn19.AppearanceHeader.Options.UseTextOptions = true;
			this.bandedGridColumn19.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
			this.bandedGridColumn19.Caption = "au持仓";
			this.bandedGridColumn19.FieldName = "au持仓";
			this.bandedGridColumn19.Name = "bandedGridColumn19";
			this.bandedGridColumn19.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn19.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Custom, "au持仓", "", "4")
			});
			this.bandedGridColumn19.Visible = true;
			this.bandedGridColumn19.Width = 60;
			this.bandedGridColumn17.AppearanceCell.Options.UseTextOptions = true;
			this.bandedGridColumn17.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far;
			this.bandedGridColumn17.AppearanceHeader.Options.UseTextOptions = true;
			this.bandedGridColumn17.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
			this.bandedGridColumn17.Caption = "ag持仓";
			this.bandedGridColumn17.FieldName = "ag持仓";
			this.bandedGridColumn17.Name = "bandedGridColumn17";
			this.bandedGridColumn17.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn17.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Custom, "ag持仓", "", "5")
			});
			this.bandedGridColumn17.Visible = true;
			this.bandedGridColumn17.Width = 60;
			this.bandedGridColumn30.Caption = "cu持仓";
			this.bandedGridColumn30.FieldName = "cu持仓";
			this.bandedGridColumn30.Name = "bandedGridColumn30";
			this.bandedGridColumn30.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn30.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Custom, "cu持仓", "", "6")
			});
			this.bandedGridColumn30.Visible = true;
			this.bandedGridColumn29.Caption = "ru持仓";
			this.bandedGridColumn29.FieldName = "ru持仓";
			this.bandedGridColumn29.Name = "bandedGridColumn29";
			this.bandedGridColumn29.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn29.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Custom, "ru持仓", "", "7")
			});
			this.bandedGridColumn29.Visible = true;
			this.bandedGridColumn28.Caption = "ni持仓";
			this.bandedGridColumn28.FieldName = "ni持仓";
			this.bandedGridColumn28.Name = "bandedGridColumn28";
			this.bandedGridColumn28.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn28.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Custom, "ni持仓", "", "8")
			});
			this.bandedGridColumn28.Visible = true;
			this.bandedGridColumn27.Caption = "rb持仓";
			this.bandedGridColumn27.FieldName = "rb持仓";
			this.bandedGridColumn27.Name = "bandedGridColumn27";
			this.bandedGridColumn27.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn27.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Custom, "rb持仓", "", "9")
			});
			this.bandedGridColumn27.Visible = true;
			this.bandedGridColumn26.Caption = "i持仓";
			this.bandedGridColumn26.FieldName = "i持仓";
			this.bandedGridColumn26.Name = "bandedGridColumn26";
			this.bandedGridColumn26.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn26.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Custom, "i持仓", "", "10")
			});
			this.bandedGridColumn26.Visible = true;
			this.bandedGridColumn25.Caption = "y持仓";
			this.bandedGridColumn25.FieldName = "y持仓";
			this.bandedGridColumn25.Name = "bandedGridColumn25";
			this.bandedGridColumn25.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn25.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Custom, "y持仓", "", "11")
			});
			this.bandedGridColumn25.Visible = true;
			this.bandedGridColumn24.Caption = "m持仓";
			this.bandedGridColumn24.FieldName = "m持仓";
			this.bandedGridColumn24.Name = "bandedGridColumn24";
			this.bandedGridColumn24.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn24.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Custom, "m持仓", "", "12")
			});
			this.bandedGridColumn24.Visible = true;
			this.bandedGridColumn23.Caption = "pp持仓";
			this.bandedGridColumn23.FieldName = "pp持仓";
			this.bandedGridColumn23.Name = "bandedGridColumn23";
			this.bandedGridColumn23.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn23.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Custom, "pp持仓", "", "13")
			});
			this.bandedGridColumn23.Visible = true;
			this.bandedGridColumn33.Caption = "l持仓";
			this.bandedGridColumn33.FieldName = "l持仓";
			this.bandedGridColumn33.Name = "bandedGridColumn33";
			this.bandedGridColumn33.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn33.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Custom, "l持仓", "", "14")
			});
			this.bandedGridColumn33.Visible = true;
			this.bandedGridColumn32.Caption = "MA持仓";
			this.bandedGridColumn32.FieldName = "MA持仓";
			this.bandedGridColumn32.Name = "bandedGridColumn32";
			this.bandedGridColumn32.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn32.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Custom, "MA持仓", "", "15")
			});
			this.bandedGridColumn32.Visible = true;
			this.bandedGridColumn31.Caption = "RM持仓";
			this.bandedGridColumn31.FieldName = "RM持仓";
			this.bandedGridColumn31.Name = "bandedGridColumn31";
			this.bandedGridColumn31.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn31.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Custom, "RM持仓", "", "16")
			});
			this.bandedGridColumn31.Visible = true;
			this.bandedGridColumn34.Caption = "SR持仓";
			this.bandedGridColumn34.FieldName = "SR持仓";
			this.bandedGridColumn34.Name = "bandedGridColumn34";
			this.bandedGridColumn34.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn34.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Custom, "SR持仓", "", "17")
			});
			this.bandedGridColumn34.Visible = true;
			this.xdgAccount.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			this.xdgAccount.Location = new System.Drawing.Point(-1, -1);
			this.xdgAccount.MainView = this.bgvAccount;
			this.xdgAccount.Name = "xdgAccount";
			this.xdgAccount.Size = new System.Drawing.Size(624, 172);
			this.xdgAccount.TabIndex = 16;
			this.xdgAccount.ViewCollection.AddRange(new BaseView[]
			{
				this.bgvAccount
			});
			this.bgvAccount.Appearance.BandPanel.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.bgvAccount.Appearance.BandPanel.BackColor2 = System.Drawing.Color.LightSkyBlue;
			this.bgvAccount.Appearance.BandPanel.BorderColor = System.Drawing.Color.White;
			this.bgvAccount.Appearance.BandPanel.ForeColor = System.Drawing.Color.Black;
			this.bgvAccount.Appearance.BandPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.bgvAccount.Appearance.BandPanel.Options.UseBackColor = true;
			this.bgvAccount.Appearance.BandPanel.Options.UseBorderColor = true;
			this.bgvAccount.Appearance.BandPanel.Options.UseForeColor = true;
			this.bgvAccount.Appearance.BandPanelBackground.BackColor = System.Drawing.Color.FromArgb(236, 246, 255);
			this.bgvAccount.Appearance.BandPanelBackground.BackColor2 = System.Drawing.Color.White;
			this.bgvAccount.Appearance.BandPanelBackground.Options.UseBackColor = true;
			this.bgvAccount.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.bgvAccount.Appearance.ColumnFilterButton.BackColor2 = System.Drawing.Color.LightSkyBlue;
			this.bgvAccount.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.White;
			this.bgvAccount.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.White;
			this.bgvAccount.Appearance.ColumnFilterButton.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.bgvAccount.Appearance.ColumnFilterButton.Options.UseBackColor = true;
			this.bgvAccount.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
			this.bgvAccount.Appearance.ColumnFilterButton.Options.UseForeColor = true;
			this.bgvAccount.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(170, 216, 254);
			this.bgvAccount.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(170, 216, 254);
			this.bgvAccount.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Black;
			this.bgvAccount.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
			this.bgvAccount.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
			this.bgvAccount.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
			this.bgvAccount.Appearance.Empty.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.bgvAccount.Appearance.Empty.BackColor2 = System.Drawing.Color.FromArgb(224, 224, 224);
			this.bgvAccount.Appearance.Empty.Options.UseBackColor = true;
			this.bgvAccount.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.bgvAccount.Appearance.EvenRow.BackColor2 = System.Drawing.Color.WhiteSmoke;
			this.bgvAccount.Appearance.EvenRow.BorderColor = System.Drawing.Color.Transparent;
			this.bgvAccount.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
			this.bgvAccount.Appearance.EvenRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
			this.bgvAccount.Appearance.EvenRow.Options.UseBackColor = true;
			this.bgvAccount.Appearance.EvenRow.Options.UseBorderColor = true;
			this.bgvAccount.Appearance.EvenRow.Options.UseForeColor = true;
			this.bgvAccount.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(104, 184, 251);
			this.bgvAccount.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(104, 184, 251);
			this.bgvAccount.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.White;
			this.bgvAccount.Appearance.FilterCloseButton.Options.UseBackColor = true;
			this.bgvAccount.Appearance.FilterCloseButton.Options.UseBorderColor = true;
			this.bgvAccount.Appearance.FilterCloseButton.Options.UseForeColor = true;
			this.bgvAccount.Appearance.FilterPanel.BackColor = System.Drawing.Color.FromArgb(236, 246, 255);
			this.bgvAccount.Appearance.FilterPanel.BackColor2 = System.Drawing.Color.White;
			this.bgvAccount.Appearance.FilterPanel.ForeColor = System.Drawing.Color.Black;
			this.bgvAccount.Appearance.FilterPanel.Options.UseBackColor = true;
			this.bgvAccount.Appearance.FilterPanel.Options.UseForeColor = true;
			this.bgvAccount.Appearance.FixedLine.BackColor = System.Drawing.Color.White;
			this.bgvAccount.Appearance.FixedLine.Options.UseBackColor = true;
			this.bgvAccount.Appearance.FocusedCell.BackColor = System.Drawing.Color.Transparent;
			this.bgvAccount.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
			this.bgvAccount.Appearance.FocusedCell.Options.UseBackColor = true;
			this.bgvAccount.Appearance.FocusedCell.Options.UseForeColor = true;
			this.bgvAccount.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.bgvAccount.Appearance.FocusedRow.BackColor2 = System.Drawing.Color.FromArgb(224, 224, 224);
			this.bgvAccount.Appearance.FocusedRow.BorderColor = System.Drawing.Color.White;
			this.bgvAccount.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
			this.bgvAccount.Appearance.FocusedRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
			this.bgvAccount.Appearance.FocusedRow.Options.UseBackColor = true;
			this.bgvAccount.Appearance.FocusedRow.Options.UseBorderColor = true;
			this.bgvAccount.Appearance.FocusedRow.Options.UseForeColor = true;
			this.bgvAccount.Appearance.FooterPanel.BackColor = System.Drawing.Color.LightCyan;
			this.bgvAccount.Appearance.FooterPanel.BackColor2 = System.Drawing.Color.LightSkyBlue;
			this.bgvAccount.Appearance.FooterPanel.BorderColor = System.Drawing.Color.Transparent;
			this.bgvAccount.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
			this.bgvAccount.Appearance.FooterPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.bgvAccount.Appearance.FooterPanel.Options.UseBackColor = true;
			this.bgvAccount.Appearance.FooterPanel.Options.UseForeColor = true;
			this.bgvAccount.Appearance.FooterPanel.Options.UseTextOptions = true;
			this.bgvAccount.Appearance.FooterPanel.TextOptions.VAlignment = VertAlignment.Top;
			this.bgvAccount.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(104, 184, 251);
			this.bgvAccount.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(104, 184, 251);
			this.bgvAccount.Appearance.GroupButton.Options.UseBackColor = true;
			this.bgvAccount.Appearance.GroupButton.Options.UseBorderColor = true;
			this.bgvAccount.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.bgvAccount.Appearance.GroupFooter.BackColor2 = System.Drawing.Color.FromArgb(255, 255, 245);
			this.bgvAccount.Appearance.GroupFooter.BorderColor = System.Drawing.Color.White;
			this.bgvAccount.Appearance.GroupFooter.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.bgvAccount.Appearance.GroupFooter.Options.UseBackColor = true;
			this.bgvAccount.Appearance.GroupFooter.Options.UseBorderColor = true;
			this.bgvAccount.Appearance.GroupFooter.Options.UseForeColor = true;
			this.bgvAccount.Appearance.GroupFooter.Options.UseTextOptions = true;
			this.bgvAccount.Appearance.GroupFooter.TextOptions.VAlignment = VertAlignment.Top;
			this.bgvAccount.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(236, 246, 255);
			this.bgvAccount.Appearance.GroupPanel.BackColor2 = System.Drawing.Color.White;
			this.bgvAccount.Appearance.GroupPanel.ForeColor = System.Drawing.Color.Black;
			this.bgvAccount.Appearance.GroupPanel.Options.UseBackColor = true;
			this.bgvAccount.Appearance.GroupPanel.Options.UseForeColor = true;
			this.bgvAccount.Appearance.GroupRow.BackColor = System.Drawing.Color.WhiteSmoke;
			this.bgvAccount.Appearance.GroupRow.BackColor2 = System.Drawing.Color.Gainsboro;
			this.bgvAccount.Appearance.GroupRow.BorderColor = System.Drawing.Color.White;
			this.bgvAccount.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
			this.bgvAccount.Appearance.GroupRow.Options.UseBackColor = true;
			this.bgvAccount.Appearance.GroupRow.Options.UseBorderColor = true;
			this.bgvAccount.Appearance.GroupRow.Options.UseForeColor = true;
			this.bgvAccount.Appearance.HeaderPanelBackground.BackColor = System.Drawing.Color.FromArgb(236, 246, 255);
			this.bgvAccount.Appearance.HeaderPanelBackground.BackColor2 = System.Drawing.Color.White;
			this.bgvAccount.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.bgvAccount.Appearance.HideSelectionRow.BackColor2 = System.Drawing.Color.FromArgb(224, 224, 224);
			this.bgvAccount.Appearance.HideSelectionRow.BorderColor = System.Drawing.Color.Transparent;
			this.bgvAccount.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.Black;
			this.bgvAccount.Appearance.HideSelectionRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
			this.bgvAccount.Appearance.HideSelectionRow.Options.UseBackColor = true;
			this.bgvAccount.Appearance.HideSelectionRow.Options.UseBorderColor = true;
			this.bgvAccount.Appearance.HideSelectionRow.Options.UseForeColor = true;
			this.bgvAccount.Appearance.HorzLine.BackColor = System.Drawing.Color.White;
			this.bgvAccount.Appearance.HorzLine.Options.UseBackColor = true;
			this.bgvAccount.Appearance.OddRow.BackColor = System.Drawing.Color.Azure;
			this.bgvAccount.Appearance.OddRow.BorderColor = System.Drawing.Color.White;
			this.bgvAccount.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
			this.bgvAccount.Appearance.OddRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
			this.bgvAccount.Appearance.OddRow.Options.UseBackColor = true;
			this.bgvAccount.Appearance.OddRow.Options.UseBorderColor = true;
			this.bgvAccount.Appearance.OddRow.Options.UseForeColor = true;
			this.bgvAccount.Appearance.Preview.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.bgvAccount.Appearance.Preview.BackColor2 = System.Drawing.Color.FromArgb(255, 255, 245);
			this.bgvAccount.Appearance.Preview.Font = new System.Drawing.Font("Verdana", 7.5f);
			this.bgvAccount.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(83, 155, 215);
			this.bgvAccount.Appearance.Preview.Options.UseBackColor = true;
			this.bgvAccount.Appearance.Preview.Options.UseFont = true;
			this.bgvAccount.Appearance.Preview.Options.UseForeColor = true;
			this.bgvAccount.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.bgvAccount.Appearance.Row.ForeColor = System.Drawing.Color.Black;
			this.bgvAccount.Appearance.Row.Options.UseBackColor = true;
			this.bgvAccount.Appearance.Row.Options.UseForeColor = true;
			this.bgvAccount.Appearance.RowSeparator.BackColor = System.Drawing.Color.FromArgb(236, 246, 255);
			this.bgvAccount.Appearance.RowSeparator.BackColor2 = System.Drawing.Color.White;
			this.bgvAccount.Appearance.RowSeparator.ForeColor = System.Drawing.Color.Black;
			this.bgvAccount.Appearance.RowSeparator.Options.UseBackColor = true;
			this.bgvAccount.Appearance.RowSeparator.Options.UseForeColor = true;
			this.bgvAccount.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.bgvAccount.Appearance.SelectedRow.BackColor2 = System.Drawing.Color.FromArgb(224, 224, 224);
			this.bgvAccount.Appearance.SelectedRow.ForeColor = System.Drawing.Color.Black;
			this.bgvAccount.Appearance.SelectedRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
			this.bgvAccount.Appearance.SelectedRow.Options.UseBackColor = true;
			this.bgvAccount.Appearance.SelectedRow.Options.UseForeColor = true;
			this.bgvAccount.Appearance.TopNewRow.BackColor = System.Drawing.Color.White;
			this.bgvAccount.Appearance.TopNewRow.Options.UseBackColor = true;
			this.bgvAccount.Appearance.VertLine.BackColor = System.Drawing.Color.White;
			this.bgvAccount.Appearance.VertLine.Options.UseBackColor = true;
			this.bgvAccount.Bands.AddRange(new GridBand[]
			{
				this.gridBand1,
				this.gridBand2
			});
			this.bgvAccount.BorderStyle = BorderStyles.Simple;
			this.bgvAccount.Columns.AddRange(new BandedGridColumn[]
			{
				this.choose,
				this.colAccount,
				this.colInvestor,
				this.colState,
				this.colBalance,
				this.bandedGridColumn12,
				this.bandedGridColumn11,
				this.bandedGridColumn22,
				this.bandedGridColumn21,
				this.bandedGridColumn20,
				this.bandedGridColumn47,
				this.bandedGridColumn46,
				this.bandedGridColumn45,
				this.bandedGridColumn44,
				this.bandedGridColumn43,
				this.bandedGridColumn42,
				this.bandedGridColumn41,
				this.bandedGridColumn40,
				this.bandedGridColumn39,
				this.bandedGridColumn38,
				this.bandedGridColumn37,
				this.bandedGridColumn36,
				this.colPreBalance,
				this.colFrozenCash,
				this.colCurrMargin,
				this.colAvailable,
				this.colCloseProfit,
				this.colPositionProfit,
				this.colCommission,
				this.colDeposit,
				this.colWithdraw,
				this.colFrozenMargin
			});
			this.bgvAccount.FooterPanelHeight = 5;
			styleFormatCondition2.ApplyToRow = true;
			styleFormatCondition2.Column = this.choose;
			styleFormatCondition2.Condition = FormatConditionEnum.Equal;
			styleFormatCondition2.Value1 = true;
			this.bgvAccount.FormatConditions.AddRange(new StyleFormatCondition[]
			{
				styleFormatCondition2
			});
			this.bgvAccount.GridControl = this.xdgAccount;
			this.bgvAccount.GroupFooterShowMode = GroupFooterShowMode.Hidden;
			this.bgvAccount.GroupSummary.AddRange(new GridSummaryItem[]
			{
				new GridGroupSummaryItem(SummaryItemType.Sum, "动态权益", this.colBalance, ""),
				new GridGroupSummaryItem(SummaryItemType.Sum, "上次结算", this.colPreBalance, ""),
				new GridGroupSummaryItem(SummaryItemType.Sum, "冻结资金", this.colFrozenCash, ""),
				new GridGroupSummaryItem(SummaryItemType.Sum, "占用保证金", this.colCurrMargin, ""),
				new GridGroupSummaryItem(SummaryItemType.Sum, "可用资金", this.colAvailable, ""),
				new GridGroupSummaryItem(SummaryItemType.Sum, "平仓盈亏", this.colCloseProfit, ""),
				new GridGroupSummaryItem(SummaryItemType.Sum, "持仓盈亏", this.colPositionProfit, ""),
				new GridGroupSummaryItem(SummaryItemType.Sum, "手续费", this.colCommission, ""),
				new GridGroupSummaryItem(SummaryItemType.Sum, "入金", this.colDeposit, ""),
				new GridGroupSummaryItem(SummaryItemType.Sum, "出金", this.colWithdraw, ""),
				new GridGroupSummaryItem(SummaryItemType.Sum, "冻结保证金", this.colFrozenMargin, "")
			});
			this.bgvAccount.HorzScrollVisibility = ScrollVisibility.Always;
			this.bgvAccount.Name = "bgvAccount";
			this.bgvAccount.OptionsBehavior.AutoExpandAllGroups = true;
			this.bgvAccount.OptionsCustomization.AllowFilter = false;
			this.bgvAccount.OptionsMenu.EnableColumnMenu = false;
			this.bgvAccount.OptionsMenu.EnableFooterMenu = false;
			this.bgvAccount.OptionsPrint.AutoWidth = false;
			this.bgvAccount.OptionsView.ColumnAutoWidth = false;
			this.bgvAccount.OptionsView.EnableAppearanceEvenRow = true;
			this.bgvAccount.OptionsView.EnableAppearanceOddRow = true;
			this.bgvAccount.OptionsView.ShowFooter = true;
			this.bgvAccount.OptionsView.ShowGroupPanel = false;
			this.bgvAccount.VertScrollVisibility = ScrollVisibility.Always;
			this.bgvAccount.RowCellStyle += new RowCellStyleEventHandler(this.bgvAccount_RowCellStyle);
			this.bgvAccount.CustomSummaryCalculate += new CustomSummaryEventHandler(this.bgvAccount_CustomSummaryCalculate);
			this.gridBand1.Caption = "子帐户";
			this.gridBand1.Columns.Add(this.choose);
			this.gridBand1.Columns.Add(this.colAccount);
			this.gridBand1.Columns.Add(this.colInvestor);
			this.gridBand1.Fixed = FixedStyle.Left;
			this.gridBand1.Name = "gridBand1";
			this.gridBand1.Width = 229;
			this.colAccount.Caption = "投资者帐户";
			this.colAccount.FieldName = "投资者帐户";
			this.colAccount.Name = "colAccount";
			this.colAccount.OptionsColumn.AllowEdit = false;
			this.colAccount.Visible = true;
			this.colAccount.Width = 105;
			this.colInvestor.Caption = "投资者";
			this.colInvestor.FieldName = "投资者";
			this.colInvestor.Name = "colInvestor";
			this.colInvestor.OptionsColumn.AllowEdit = false;
			this.colInvestor.Visible = true;
			this.colInvestor.Width = 83;
			this.gridBand2.Caption = "资金详细";
			this.gridBand2.Columns.Add(this.colState);
			this.gridBand2.Columns.Add(this.colBalance);
			this.gridBand2.Columns.Add(this.colCloseProfit);
			this.gridBand2.Columns.Add(this.colPositionProfit);
			this.gridBand2.Columns.Add(this.bandedGridColumn12);
			this.gridBand2.Columns.Add(this.bandedGridColumn11);
			this.gridBand2.Columns.Add(this.bandedGridColumn22);
			this.gridBand2.Columns.Add(this.bandedGridColumn21);
			this.gridBand2.Columns.Add(this.bandedGridColumn20);
			this.gridBand2.Columns.Add(this.bandedGridColumn47);
			this.gridBand2.Columns.Add(this.bandedGridColumn46);
			this.gridBand2.Columns.Add(this.bandedGridColumn45);
			this.gridBand2.Columns.Add(this.bandedGridColumn44);
			this.gridBand2.Columns.Add(this.bandedGridColumn43);
			this.gridBand2.Columns.Add(this.bandedGridColumn42);
			this.gridBand2.Columns.Add(this.bandedGridColumn41);
			this.gridBand2.Columns.Add(this.bandedGridColumn40);
			this.gridBand2.Columns.Add(this.bandedGridColumn39);
			this.gridBand2.Columns.Add(this.bandedGridColumn38);
			this.gridBand2.Columns.Add(this.bandedGridColumn37);
			this.gridBand2.Columns.Add(this.bandedGridColumn36);
			this.gridBand2.Columns.Add(this.colPreBalance);
			this.gridBand2.Columns.Add(this.colCommission);
			this.gridBand2.Columns.Add(this.colCurrMargin);
			this.gridBand2.Columns.Add(this.colFrozenCash);
			this.gridBand2.Columns.Add(this.colAvailable);
			this.gridBand2.Columns.Add(this.colDeposit);
			this.gridBand2.Columns.Add(this.colWithdraw);
			this.gridBand2.Columns.Add(this.colFrozenMargin);
			this.gridBand2.Name = "gridBand2";
			this.gridBand2.Width = 2350;
			this.colState.AppearanceCell.ForeColor = System.Drawing.Color.Red;
			this.colState.AppearanceCell.Options.UseForeColor = true;
			this.colState.AppearanceCell.Options.UseTextOptions = true;
			this.colState.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
			this.colState.Caption = "登录状态";
			this.colState.FieldName = "登录状态";
			this.colState.Name = "colState";
			this.colState.OptionsColumn.AllowEdit = false;
			this.colState.Visible = true;
			this.colState.Width = 89;
			this.colBalance.AppearanceCell.Options.UseTextOptions = true;
			this.colBalance.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
			this.colBalance.Caption = "动态权益";
			this.colBalance.ColumnEdit = this.repositoryItemSpinEdit1;
			this.colBalance.DisplayFormat.FormatType = FormatType.Numeric;
			this.colBalance.FieldName = "动态权益";
			this.colBalance.Name = "colBalance";
			this.colBalance.OptionsColumn.AllowEdit = false;
			this.colBalance.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Sum)
			});
			this.colBalance.Visible = true;
			this.colBalance.Width = 108;
			this.colCloseProfit.AppearanceCell.Options.UseTextOptions = true;
			this.colCloseProfit.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
			this.colCloseProfit.Caption = "平仓盈亏";
			this.colCloseProfit.FieldName = "平仓盈亏";
			this.colCloseProfit.GroupFormat.FormatType = FormatType.Numeric;
			this.colCloseProfit.Name = "colCloseProfit";
			this.colCloseProfit.OptionsColumn.AllowEdit = false;
			this.colCloseProfit.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Sum)
			});
			this.colCloseProfit.Visible = true;
			this.colCloseProfit.Width = 90;
			this.colPositionProfit.AppearanceCell.Options.UseTextOptions = true;
			this.colPositionProfit.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
			this.colPositionProfit.Caption = "持仓盈亏";
			this.colPositionProfit.FieldName = "持仓盈亏";
			this.colPositionProfit.GroupFormat.FormatType = FormatType.Numeric;
			this.colPositionProfit.Name = "colPositionProfit";
			this.colPositionProfit.OptionsColumn.AllowEdit = false;
			this.colPositionProfit.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Sum)
			});
			this.colPositionProfit.Visible = true;
			this.colPositionProfit.Width = 89;
			this.bandedGridColumn12.AppearanceCell.Options.UseTextOptions = true;
			this.bandedGridColumn12.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far;
			this.bandedGridColumn12.Caption = "IF持仓";
			this.bandedGridColumn12.FieldName = "IF持仓";
			this.bandedGridColumn12.Name = "bandedGridColumn12";
			this.bandedGridColumn12.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn12.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Custom, "IF持仓", "", "1")
			});
			this.bandedGridColumn12.Visible = true;
			this.bandedGridColumn12.Width = 60;
			this.bandedGridColumn11.AppearanceCell.Options.UseTextOptions = true;
			this.bandedGridColumn11.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far;
			this.bandedGridColumn11.Caption = "IC持仓";
			this.bandedGridColumn11.FieldName = "IC持仓";
			this.bandedGridColumn11.Name = "bandedGridColumn11";
			this.bandedGridColumn11.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn11.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Custom, "IC持仓", "", "2")
			});
			this.bandedGridColumn11.Visible = true;
			this.bandedGridColumn11.Width = 60;
			this.bandedGridColumn22.AppearanceCell.Options.UseTextOptions = true;
			this.bandedGridColumn22.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far;
			this.bandedGridColumn22.Caption = "IH持仓";
			this.bandedGridColumn22.FieldName = "IH持仓";
			this.bandedGridColumn22.Name = "bandedGridColumn22";
			this.bandedGridColumn22.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn22.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Custom, "IH持仓", "", "3")
			});
			this.bandedGridColumn22.Visible = true;
			this.bandedGridColumn22.Width = 60;
			this.bandedGridColumn21.AppearanceCell.Options.UseTextOptions = true;
			this.bandedGridColumn21.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far;
			this.bandedGridColumn21.Caption = "au持仓";
			this.bandedGridColumn21.FieldName = "au持仓";
			this.bandedGridColumn21.Name = "bandedGridColumn21";
			this.bandedGridColumn21.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn21.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Custom, "au持仓", "", "4")
			});
			this.bandedGridColumn21.Visible = true;
			this.bandedGridColumn21.Width = 60;
			this.bandedGridColumn20.AppearanceCell.Options.UseTextOptions = true;
			this.bandedGridColumn20.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far;
			this.bandedGridColumn20.Caption = "ag持仓";
			this.bandedGridColumn20.FieldName = "ag持仓";
			this.bandedGridColumn20.Name = "bandedGridColumn20";
			this.bandedGridColumn20.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn20.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Custom, "ag持仓", "", "5")
			});
			this.bandedGridColumn20.Visible = true;
			this.bandedGridColumn20.Width = 60;
			this.bandedGridColumn47.Caption = "cu持仓";
			this.bandedGridColumn47.FieldName = "cu持仓";
			this.bandedGridColumn47.Name = "bandedGridColumn47";
			this.bandedGridColumn47.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn47.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Custom, "cu持仓", "", "6")
			});
			this.bandedGridColumn47.Visible = true;
			this.bandedGridColumn46.Caption = "ru持仓";
			this.bandedGridColumn46.FieldName = "ru持仓";
			this.bandedGridColumn46.Name = "bandedGridColumn46";
			this.bandedGridColumn46.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn46.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Custom, "ru持仓", "", "7")
			});
			this.bandedGridColumn46.Visible = true;
			this.bandedGridColumn45.Caption = "ni持仓";
			this.bandedGridColumn45.FieldName = "ni持仓";
			this.bandedGridColumn45.Name = "bandedGridColumn45";
			this.bandedGridColumn45.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn45.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Custom, "ni持仓", "", "8")
			});
			this.bandedGridColumn45.Visible = true;
			this.bandedGridColumn44.Caption = "rb持仓";
			this.bandedGridColumn44.FieldName = "rb持仓";
			this.bandedGridColumn44.Name = "bandedGridColumn44";
			this.bandedGridColumn44.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn44.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Custom, "rb持仓", "", "9")
			});
			this.bandedGridColumn44.Visible = true;
			this.bandedGridColumn43.Caption = "i持仓";
			this.bandedGridColumn43.FieldName = "i持仓";
			this.bandedGridColumn43.Name = "bandedGridColumn43";
			this.bandedGridColumn43.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn43.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Custom, "i持仓", "", "10")
			});
			this.bandedGridColumn43.Visible = true;
			this.bandedGridColumn42.Caption = "y持仓";
			this.bandedGridColumn42.FieldName = "y持仓";
			this.bandedGridColumn42.Name = "bandedGridColumn42";
			this.bandedGridColumn42.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn42.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Custom, "y持仓", "", "11")
			});
			this.bandedGridColumn42.Visible = true;
			this.bandedGridColumn41.Caption = "m持仓";
			this.bandedGridColumn41.FieldName = "m持仓";
			this.bandedGridColumn41.Name = "bandedGridColumn41";
			this.bandedGridColumn41.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn41.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Custom, "m持仓", "", "12")
			});
			this.bandedGridColumn41.Visible = true;
			this.bandedGridColumn40.Caption = "pp持仓";
			this.bandedGridColumn40.FieldName = "pp持仓";
			this.bandedGridColumn40.Name = "bandedGridColumn40";
			this.bandedGridColumn40.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn40.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Custom, "pp持仓", "", "13")
			});
			this.bandedGridColumn40.Visible = true;
			this.bandedGridColumn39.Caption = "l持仓";
			this.bandedGridColumn39.FieldName = "l持仓";
			this.bandedGridColumn39.Name = "bandedGridColumn39";
			this.bandedGridColumn39.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn39.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Custom, "l持仓", "", "14")
			});
			this.bandedGridColumn39.Visible = true;
			this.bandedGridColumn38.Caption = "MA持仓";
			this.bandedGridColumn38.FieldName = "MA持仓";
			this.bandedGridColumn38.Name = "bandedGridColumn38";
			this.bandedGridColumn38.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn38.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Custom, "MA持仓", "", "15")
			});
			this.bandedGridColumn38.Visible = true;
			this.bandedGridColumn37.Caption = "RM持仓";
			this.bandedGridColumn37.FieldName = "RM持仓";
			this.bandedGridColumn37.Name = "bandedGridColumn37";
			this.bandedGridColumn37.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn37.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Custom, "RM持仓", "", "16")
			});
			this.bandedGridColumn37.Visible = true;
			this.bandedGridColumn36.Caption = "SR持仓";
			this.bandedGridColumn36.FieldName = "SR持仓";
			this.bandedGridColumn36.Name = "bandedGridColumn36";
			this.bandedGridColumn36.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn36.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Custom, "SR持仓", "", "17")
			});
			this.bandedGridColumn36.Visible = true;
			this.colPreBalance.AppearanceCell.Options.UseTextOptions = true;
			this.colPreBalance.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
			this.colPreBalance.Caption = "上次结算";
			this.colPreBalance.FieldName = "上次结算";
			this.colPreBalance.GroupFormat.FormatType = FormatType.Numeric;
			this.colPreBalance.Name = "colPreBalance";
			this.colPreBalance.OptionsColumn.AllowEdit = false;
			this.colPreBalance.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Sum)
			});
			this.colPreBalance.Visible = true;
			this.colPreBalance.Width = 108;
			this.colCommission.AppearanceCell.Options.UseTextOptions = true;
			this.colCommission.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
			this.colCommission.Caption = "手续费";
			this.colCommission.FieldName = "手续费";
			this.colCommission.GroupFormat.FormatType = FormatType.Numeric;
			this.colCommission.Name = "colCommission";
			this.colCommission.OptionsColumn.AllowEdit = false;
			this.colCommission.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Sum)
			});
			this.colCommission.Visible = true;
			this.colCommission.Width = 78;
			this.colCurrMargin.AppearanceCell.Options.UseTextOptions = true;
			this.colCurrMargin.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
			this.colCurrMargin.Caption = "占用保证金";
			this.colCurrMargin.FieldName = "占用保证金";
			this.colCurrMargin.GroupFormat.FormatType = FormatType.Numeric;
			this.colCurrMargin.Name = "colCurrMargin";
			this.colCurrMargin.OptionsColumn.AllowEdit = false;
			this.colCurrMargin.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Sum)
			});
			this.colCurrMargin.Visible = true;
			this.colCurrMargin.Width = 104;
			this.colFrozenCash.AppearanceCell.Options.UseTextOptions = true;
			this.colFrozenCash.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
			this.colFrozenCash.Caption = "冻结资金";
			this.colFrozenCash.FieldName = "冻结资金";
			this.colFrozenCash.GroupFormat.FormatType = FormatType.Numeric;
			this.colFrozenCash.Name = "colFrozenCash";
			this.colFrozenCash.OptionsColumn.AllowEdit = false;
			this.colFrozenCash.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Sum)
			});
			this.colFrozenCash.Visible = true;
			this.colFrozenCash.Width = 104;
			this.colAvailable.AppearanceCell.Options.UseTextOptions = true;
			this.colAvailable.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
			this.colAvailable.Caption = "可用资金";
			this.colAvailable.FieldName = "可用资金";
			this.colAvailable.GroupFormat.FormatType = FormatType.Numeric;
			this.colAvailable.Name = "colAvailable";
			this.colAvailable.OptionsColumn.AllowEdit = false;
			this.colAvailable.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Sum)
			});
			this.colAvailable.Visible = true;
			this.colAvailable.Width = 101;
			this.colDeposit.AppearanceCell.Options.UseTextOptions = true;
			this.colDeposit.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
			this.colDeposit.Caption = "入金";
			this.colDeposit.FieldName = "入金";
			this.colDeposit.GroupFormat.FormatType = FormatType.Numeric;
			this.colDeposit.Name = "colDeposit";
			this.colDeposit.OptionsColumn.AllowEdit = false;
			this.colDeposit.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Sum)
			});
			this.colDeposit.Visible = true;
			this.colDeposit.Width = 77;
			this.colWithdraw.AppearanceCell.Options.UseTextOptions = true;
			this.colWithdraw.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
			this.colWithdraw.Caption = "出金";
			this.colWithdraw.FieldName = "出金";
			this.colWithdraw.GroupFormat.FormatType = FormatType.Numeric;
			this.colWithdraw.Name = "colWithdraw";
			this.colWithdraw.OptionsColumn.AllowEdit = false;
			this.colWithdraw.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Sum)
			});
			this.colWithdraw.Visible = true;
			this.colWithdraw.Width = 104;
			this.colFrozenMargin.AppearanceCell.Options.UseTextOptions = true;
			this.colFrozenMargin.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
			this.colFrozenMargin.Caption = "冻结保证金";
			this.colFrozenMargin.FieldName = "冻结保证金";
			this.colFrozenMargin.GroupFormat.FormatType = FormatType.Numeric;
			this.colFrozenMargin.Name = "colFrozenMargin";
			this.colFrozenMargin.OptionsColumn.AllowEdit = false;
			this.colFrozenMargin.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Sum)
			});
			this.colFrozenMargin.Visible = true;
			this.colFrozenMargin.Width = 98;
			this.numericUpDownPrice.Font = new System.Drawing.Font("微软雅黑", 10.5f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.numericUpDownPrice.Location = new System.Drawing.Point(59, 122);
			System.Windows.Forms.NumericUpDown arg_7759_0 = this.numericUpDownPrice;
			int[] array = new int[4];
			array[0] = 1000000;
			arg_7759_0.Maximum = new decimal(array);
			this.numericUpDownPrice.Name = "numericUpDownPrice";
			this.numericUpDownPrice.Size = new System.Drawing.Size(77, 26);
			this.numericUpDownPrice.TabIndex = 71;
			this.numericUpDownPrice.ValueChanged += new System.EventHandler(this.numericUpDownPrice_ValueChanged);
			this.numericUpDownPrice.Click += new System.EventHandler(this.numericUpDownPrice_Click);
			this.buttonOrder.Location = new System.Drawing.Point(190, 7);
			this.buttonOrder.Name = "buttonOrder";
			this.buttonOrder.Size = new System.Drawing.Size(77, 49);
			this.buttonOrder.TabIndex = 72;
			this.buttonOrder.Text = "下 单";
			this.buttonOrder.UseVisualStyleBackColor = true;
			this.buttonOrder.Click += new System.EventHandler(this.buttonOrder_Click);
			this.label7.Font = new System.Drawing.Font("微软雅黑", 10.5f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.label7.Location = new System.Drawing.Point(14, 57);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(39, 23);
			this.label7.TabIndex = 65;
			this.label7.Text = "开平";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.buttonCancel.Location = new System.Drawing.Point(273, 7);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(65, 23);
			this.buttonCancel.TabIndex = 73;
			this.buttonCancel.Text = "取 消";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			this.label15.AutoSize = true;
			this.label15.Location = new System.Drawing.Point(137, 94);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(17, 12);
			this.label15.TabIndex = 58;
			this.label15.Text = "≤";
			this.numericUpDownVolume.Font = new System.Drawing.Font("微软雅黑", 10.5f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.numericUpDownVolume.Location = new System.Drawing.Point(59, 89);
			System.Windows.Forms.NumericUpDown arg_7A0E_0 = this.numericUpDownVolume;
			array = new int[4];
			array[0] = 1000;
			arg_7A0E_0.Maximum = new decimal(array);
			this.numericUpDownVolume.Name = "numericUpDownVolume";
			this.numericUpDownVolume.Size = new System.Drawing.Size(77, 26);
			this.numericUpDownVolume.TabIndex = 70;
			System.Windows.Forms.NumericUpDown arg_7A62_0 = this.numericUpDownVolume;
			array = new int[4];
			array[0] = 1;
			arg_7A62_0.Value = new decimal(array);
			this.numericUpDownVolume.ValueChanged += new System.EventHandler(this.numericUpDownVolume_ValueChanged);
			this.numericUpDownVolume.Click += new System.EventHandler(this.numericUpDownVolume_Click);
			this.labelVolumeMax.AutoSize = true;
			this.labelVolumeMax.Font = new System.Drawing.Font("新宋体", 10.5f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.labelVolumeMax.Location = new System.Drawing.Point(141, 93);
			this.labelVolumeMax.Name = "labelVolumeMax";
			this.labelVolumeMax.Size = new System.Drawing.Size(14, 14);
			this.labelVolumeMax.TabIndex = 59;
			this.labelVolumeMax.Text = "-";
			this.label3.Font = new System.Drawing.Font("微软雅黑", 10.5f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.label3.Location = new System.Drawing.Point(10, 3);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(42, 23);
			this.label3.TabIndex = 62;
			this.label3.Text = "合约";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.label6.Font = new System.Drawing.Font("微软雅黑", 10.5f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.label6.Location = new System.Drawing.Point(5, 92);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(43, 23);
			this.label6.TabIndex = 63;
			this.label6.Text = "手 数";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.labelLower.AutoSize = true;
			this.labelLower.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			this.labelLower.ForeColor = System.Drawing.Color.Green;
			this.labelLower.Location = new System.Drawing.Point(141, 136);
			this.labelLower.Name = "labelLower";
			this.labelLower.Size = new System.Drawing.Size(11, 14);
			this.labelLower.TabIndex = 60;
			this.labelLower.Text = "-";
			this.buttonPrice.BackColor = System.Drawing.Color.Transparent;
			this.buttonPrice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.buttonPrice.Font = new System.Drawing.Font("新宋体", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.buttonPrice.Location = new System.Drawing.Point(1, 125);
			this.buttonPrice.Name = "buttonPrice";
			this.buttonPrice.Size = new System.Drawing.Size(56, 23);
			this.buttonPrice.TabIndex = 66;
			this.buttonPrice.Text = "指定价";
			this.buttonPrice.UseVisualStyleBackColor = false;
			this.buttonPrice.Click += new System.EventHandler(this.buttonPrice_Click);
			this.labelUpper.AutoSize = true;
			this.labelUpper.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			this.labelUpper.ForeColor = System.Drawing.Color.Red;
			this.labelUpper.Location = new System.Drawing.Point(141, 122);
			this.labelUpper.Name = "labelUpper";
			this.labelUpper.Size = new System.Drawing.Size(11, 14);
			this.labelUpper.TabIndex = 61;
			this.labelUpper.Text = "-";
			this.label5.Font = new System.Drawing.Font("微软雅黑", 10.5f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
			this.label5.Location = new System.Drawing.Point(14, 29);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(39, 23);
			this.label5.TabIndex = 64;
			this.label5.Text = "买卖";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.tab.AllowDrop = true;
			this.tab.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tab.Font = new System.Drawing.Font("宋体", 10f);
			this.tab.ImageList = this.imageList1;
			this.tab.Location = new System.Drawing.Point(0, 0);
			this.tab.Multiline = true;
			this.tab.Name = "tab";
			this.tab.SelectedIndex = 0;
			this.tab.Size = new System.Drawing.Size(522, 293);
			this.tab.TabIndex = 19;
			this.tab.DoubleClick += new System.EventHandler(this.tab_DoubleClick);
			this.imageList1.ImageStream = (System.Windows.Forms.ImageListStreamer)componentResourceManager.GetObject("imageList1.ImageStream");
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "13.ico");
			this.columnHeader36.Text = "投资者";
			this.columnHeader36.Width = 130;
			this.columnHeader37.Text = "编号";
			this.columnHeader37.Width = 85;
			this.columnHeader38.Text = "合约";
			this.columnHeader38.Width = 85;
			this.columnHeader39.Text = "买卖";
			this.columnHeader39.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader39.Width = 85;
			this.columnHeader40.Text = "开平";
			this.columnHeader40.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader40.Width = 85;
			this.columnHeader41.Text = "状态";
			this.columnHeader41.Width = 95;
			this.columnHeader42.Text = "价格";
			this.columnHeader42.Width = 85;
			this.columnHeader43.Text = "报单手数";
			this.columnHeader43.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader43.Width = 95;
			this.columnHeader44.Text = "未成交";
			this.columnHeader44.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader44.Width = 85;
			this.columnHeader52.Text = "成交手数";
			this.columnHeader52.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader52.Width = 95;
			this.columnHeader55.Text = "报单时间";
			this.columnHeader55.Width = 95;
			this.columnHeader56.Text = "成交时间";
			this.columnHeader56.Width = 95;
			this.columnHeader57.Text = "成交均价";
			this.columnHeader57.Width = 95;
			this.columnHeader58.Text = "详细状态";
			this.columnHeader58.Width = 255;
			this.columnHeader59.Text = "客户信息";
			this.columnHeader59.Width = 95;
			this.account.Text = "投资者帐户";
			this.account.Width = 130;
			this.num.Text = "编号";
			this.num.Width = 85;
			this.code.Text = "合约";
			this.code.Width = 85;
			this.directortype.Text = "买卖";
			this.directortype.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.directortype.Width = 85;
			this.offsetflag.Text = "开平";
			this.offsetflag.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.offsetflag.Width = 85;
			this.status.Text = "状态";
			this.status.Width = 95;
			this.price.Text = "价格";
			this.price.Width = 85;
			this.orderhand.Text = "报单手数";
			this.orderhand.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.orderhand.Width = 95;
			this.notrade.Text = "未成交";
			this.notrade.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.notrade.Width = 85;
			this.tradehand.Text = "成交手数";
			this.tradehand.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.tradehand.Width = 95;
			this.ordertime.Text = "报单时间";
			this.ordertime.Width = 95;
			this.tradetime.Text = "成交时间";
			this.tradetime.Width = 95;
			this.tradeprice.Text = "成交均价";
			this.tradeprice.Width = 95;
			this.detailstatus.Text = "详细状态";
			this.detailstatus.Width = 255;
			this.custome.Text = "客户信息";
			this.custome.Width = 95;
			this.columnHeader34.Text = "投资者";
			this.columnHeader34.Width = 130;
			this.columnHeader25.Text = "编号";
			this.columnHeader25.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader25.Width = 80;
			this.columnHeader26.Text = "合约";
			this.columnHeader26.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader26.Width = 85;
			this.columnHeader27.Text = "买卖";
			this.columnHeader27.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader27.Width = 85;
			this.columnHeader28.Text = "开平";
			this.columnHeader28.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader28.Width = 85;
			this.columnHeader29.Text = "成交价格";
			this.columnHeader29.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader29.Width = 95;
			this.columnHeader30.Text = "成交手数";
			this.columnHeader30.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader30.Width = 95;
			this.columnHeader31.Text = "成交时间";
			this.columnHeader31.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader31.Width = 95;
			this.columnHeader32.Text = "报单编号";
			this.columnHeader32.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader32.Width = 95;
			this.columnHeader33.Text = "成交类型";
			this.columnHeader33.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader33.Width = 95;
			this.columnHeader35.Text = "交易所";
			this.columnHeader35.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.columnHeader35.Width = 95;
			this.tmMoneyRefresh.Interval = 60000;
			this.tmMoneyRefresh.Tick += new System.EventHandler(this.tmMoneyRefresh_Tick);
			this.spcAccount.CollapsePanel = SplitCollapsePanel.Panel2;
			this.spcAccount.Dock = System.Windows.Forms.DockStyle.Fill;
			this.spcAccount.FixedPanel = SplitFixedPanel.Panel2;
			this.spcAccount.Location = new System.Drawing.Point(0, 0);
			this.spcAccount.LookAndFeel.Style = LookAndFeelStyle.Flat;
			this.spcAccount.LookAndFeel.UseDefaultLookAndFeel = false;
			this.spcAccount.Name = "spcAccount";
			this.spcAccount.Panel1.Controls.Add(this.button5);
			this.spcAccount.Panel1.Controls.Add(this.xdgAccount);
			this.spcAccount.Panel1.Controls.Add(this.butSelAll);
			this.spcAccount.Panel1.Controls.Add(this.butLoginOut);
			this.spcAccount.Panel1.Controls.Add(this.butLogin);
			this.spcAccount.Panel1.Text = "Panel1";
			this.spcAccount.Panel2.Controls.Add(this.button7);
			this.spcAccount.Panel2.Controls.Add(this.button6);
			this.spcAccount.Panel2.Controls.Add(this.button4);
			this.spcAccount.Panel2.Controls.Add(this.button3);
			this.spcAccount.Panel2.Controls.Add(this.button2);
			this.spcAccount.Panel2.Controls.Add(this.button1);
			this.spcAccount.Panel2.Controls.Add(this.comboBoxOffset);
			this.spcAccount.Panel2.Controls.Add(this.comboBoxDirector);
			this.spcAccount.Panel2.Controls.Add(this.buttonPrice);
			this.spcAccount.Panel2.Controls.Add(this.comboBoxInstrument);
			this.spcAccount.Panel2.Controls.Add(this.buttonMarketPrice);
			this.spcAccount.Panel2.Controls.Add(this.buttonOrder);
			this.spcAccount.Panel2.Controls.Add(this.numericUpDownPrice);
			this.spcAccount.Panel2.Controls.Add(this.label6);
			this.spcAccount.Panel2.Controls.Add(this.label3);
			this.spcAccount.Panel2.Controls.Add(this.label7);
			this.spcAccount.Panel2.Controls.Add(this.labelLower);
			this.spcAccount.Panel2.Controls.Add(this.buttonCancel);
			this.spcAccount.Panel2.Controls.Add(this.labelVolumeMax);
			this.spcAccount.Panel2.Controls.Add(this.label5);
			this.spcAccount.Panel2.Controls.Add(this.numericUpDownVolume);
			this.spcAccount.Panel2.Controls.Add(this.labelUpper);
			this.spcAccount.Panel2.Controls.Add(this.label15);
			this.spcAccount.Panel2.MinSize = 190;
			this.spcAccount.Panel2.Text = "Panel2";
			this.spcAccount.Size = new System.Drawing.Size(1044, 176);
			this.spcAccount.SplitterPosition = 346;
			this.spcAccount.TabIndex = 0;
			this.spcAccount.Text = "splitContainerControl2";
			this.button5.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.button5.Location = new System.Drawing.Point(629, 148);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(62, 23);
			this.button5.TabIndex = 17;
			this.button5.Text = "撤单";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += new System.EventHandler(this.button5_Click);
			this.comboBoxOffset.EditValue = "开仓";
			this.comboBoxOffset.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.comboBoxOffset.Location = new System.Drawing.Point(59, 59);
			this.comboBoxOffset.Name = "comboBoxOffset";
			this.comboBoxOffset.Properties.AppearanceFocused.BackColor = System.Drawing.Color.DodgerBlue;
			this.comboBoxOffset.Properties.AppearanceFocused.Options.UseBackColor = true;
			this.comboBoxOffset.Properties.AutoComplete = false;
			this.comboBoxOffset.Properties.BorderStyle = BorderStyles.Office2003;
			this.comboBoxOffset.Properties.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton(ButtonPredefines.DropDown)
			});
			this.comboBoxOffset.Properties.ButtonsStyle = BorderStyles.Office2003;
			this.comboBoxOffset.Properties.Items.AddRange(new object[]
			{
				"开仓",
				"平今",
				"平仓"
			});
			this.comboBoxOffset.Properties.ShowPopupShadow = false;
			this.comboBoxOffset.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.comboBoxOffset.Size = new System.Drawing.Size(117, 20);
			this.comboBoxOffset.TabIndex = 76;
			this.comboBoxOffset.Enter += new System.EventHandler(this.comboBoxOffset_Enter);
			this.comboBoxOffset.Leave += new System.EventHandler(this.comboBoxOffset_Leave);
			this.comboBoxDirector.EditValue = "买入";
			this.comboBoxDirector.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.comboBoxDirector.Location = new System.Drawing.Point(59, 31);
			this.comboBoxDirector.Name = "comboBoxDirector";
			this.comboBoxDirector.Properties.AppearanceFocused.BackColor = System.Drawing.Color.DodgerBlue;
			this.comboBoxDirector.Properties.AppearanceFocused.Options.UseBackColor = true;
			this.comboBoxDirector.Properties.AutoComplete = false;
			this.comboBoxDirector.Properties.BorderStyle = BorderStyles.Office2003;
			this.comboBoxDirector.Properties.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton(ButtonPredefines.DropDown)
			});
			this.comboBoxDirector.Properties.ButtonsStyle = BorderStyles.Office2003;
			this.comboBoxDirector.Properties.Items.AddRange(new object[]
			{
				"买入",
				"卖出"
			});
			this.comboBoxDirector.Properties.ShowPopupShadow = false;
			this.comboBoxDirector.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.comboBoxDirector.Size = new System.Drawing.Size(117, 20);
			this.comboBoxDirector.TabIndex = 20;
			this.comboBoxDirector.SelectedIndexChanged += new System.EventHandler(this.comboBoxDirector_SelectedIndexChanged);
			this.comboBoxDirector.Enter += new System.EventHandler(this.comboBoxDirector_Enter);
			this.comboBoxDirector.Leave += new System.EventHandler(this.comboBoxDirector_Leave);
			this.comboBoxInstrument.Location = new System.Drawing.Point(59, 4);
			this.comboBoxInstrument.Name = "comboBoxInstrument";
			this.comboBoxInstrument.Properties.AutoComplete = false;
			this.comboBoxInstrument.Properties.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton(ButtonPredefines.Combo)
			});
			this.comboBoxInstrument.Properties.ImmediatePopup = true;
			this.comboBoxInstrument.Properties.Sorted = true;
			this.comboBoxInstrument.Size = new System.Drawing.Size(117, 20);
			this.comboBoxInstrument.TabIndex = 75;
			this.comboBoxInstrument.SelectedIndexChanged += new System.EventHandler(this.comboBoxInstrument_SelectedIndexChanged);
			this.comboBoxInstrument.Enter += new System.EventHandler(this.comboBoxInstrument_Enter);
			this.comboBoxInstrument.Leave += new System.EventHandler(this.comboBoxInstrument_Leave);
			this.comboBoxInstrument.MouseDown += new System.Windows.Forms.MouseEventHandler(this.comboBoxInstrument_MouseDown);
			this.spcPosition.Appearance.BackColor = System.Drawing.Color.FromArgb(235, 236, 239);
			this.spcPosition.Appearance.Options.UseBackColor = true;
			this.spcPosition.CollapsePanel = SplitCollapsePanel.Panel1;
			this.spcPosition.Dock = System.Windows.Forms.DockStyle.Fill;
			this.spcPosition.FixedPanel = SplitFixedPanel.None;
			this.spcPosition.Location = new System.Drawing.Point(0, 0);
			this.spcPosition.LookAndFeel.Style = LookAndFeelStyle.Flat;
			this.spcPosition.LookAndFeel.UseDefaultLookAndFeel = false;
			this.spcPosition.Name = "spcPosition";
			this.spcPosition.Panel1.Controls.Add(this.tab);
			this.spcPosition.Panel1.Text = "Panel1";
			this.spcPosition.Panel2.Controls.Add(this.xtraTabControl1);
			this.spcPosition.Panel2.Text = "Panel2";
			this.spcPosition.Size = new System.Drawing.Size(1044, 293);
			this.spcPosition.SplitterPosition = 522;
			this.spcPosition.TabIndex = 0;
			this.spcPosition.Text = "splitContainerControl2";
			this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
			this.xtraTabControl1.Name = "xtraTabControl1";
			this.xtraTabControl1.SelectedTabPage = this.xtabTrade;
			this.xtraTabControl1.Size = new System.Drawing.Size(518, 293);
			this.xtraTabControl1.TabIndex = 18;
			this.xtraTabControl1.TabPages.AddRange(new XtraTabPage[]
			{
				this.xtabTrade,
				this.xtabViewTrade
			});
			this.xtabTrade.Appearance.PageClient.BackColor = System.Drawing.Color.Red;
			this.xtabTrade.Appearance.PageClient.Options.UseBackColor = true;
			this.xtabTrade.Controls.Add(this.rdoError);
			this.xtabTrade.Controls.Add(this.rdoRevoke);
			this.xtabTrade.Controls.Add(this.rdoDeal);
			this.xtabTrade.Controls.Add(this.rdoSuspend);
			this.xtabTrade.Controls.Add(this.rdoAll);
			this.xtabTrade.Controls.Add(this.xdgTrade);
			this.xtabTrade.Name = "xtabTrade";
			this.xtabTrade.Size = new System.Drawing.Size(512, 264);
			this.xtabTrade.Text = "所有委托单";
			this.rdoError.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			this.rdoError.AutoSize = true;
			this.rdoError.Location = new System.Drawing.Point(255, 243);
			this.rdoError.Name = "rdoError";
			this.rdoError.Size = new System.Drawing.Size(59, 16);
			this.rdoError.TabIndex = 23;
			this.rdoError.Text = "错误单";
			this.rdoError.UseVisualStyleBackColor = true;
			this.rdoError.CheckedChanged += new System.EventHandler(this.rdoError_CheckedChanged);
			this.rdoRevoke.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			this.rdoRevoke.AutoSize = true;
			this.rdoRevoke.Location = new System.Drawing.Point(190, 243);
			this.rdoRevoke.Name = "rdoRevoke";
			this.rdoRevoke.Size = new System.Drawing.Size(59, 16);
			this.rdoRevoke.TabIndex = 22;
			this.rdoRevoke.Text = "已撤单";
			this.rdoRevoke.UseVisualStyleBackColor = true;
			this.rdoRevoke.CheckedChanged += new System.EventHandler(this.rdoRevoke_CheckedChanged);
			this.rdoDeal.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			this.rdoDeal.AutoSize = true;
			this.rdoDeal.Location = new System.Drawing.Point(125, 243);
			this.rdoDeal.Name = "rdoDeal";
			this.rdoDeal.Size = new System.Drawing.Size(59, 16);
			this.rdoDeal.TabIndex = 21;
			this.rdoDeal.Text = "已成交";
			this.rdoDeal.UseVisualStyleBackColor = true;
			this.rdoDeal.CheckedChanged += new System.EventHandler(this.rdoDeal_CheckedChanged);
			this.rdoSuspend.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			this.rdoSuspend.AutoSize = true;
			this.rdoSuspend.Location = new System.Drawing.Point(72, 243);
			this.rdoSuspend.Name = "rdoSuspend";
			this.rdoSuspend.Size = new System.Drawing.Size(47, 16);
			this.rdoSuspend.TabIndex = 20;
			this.rdoSuspend.Text = "挂单";
			this.rdoSuspend.UseVisualStyleBackColor = true;
			this.rdoSuspend.CheckedChanged += new System.EventHandler(this.rdoSuspend_CheckedChanged);
			this.rdoAll.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			this.rdoAll.AutoSize = true;
			this.rdoAll.Checked = true;
			this.rdoAll.Location = new System.Drawing.Point(7, 243);
			this.rdoAll.Name = "rdoAll";
			this.rdoAll.Size = new System.Drawing.Size(59, 16);
			this.rdoAll.TabIndex = 19;
			this.rdoAll.TabStop = true;
			this.rdoAll.Text = "全部单";
			this.rdoAll.UseVisualStyleBackColor = true;
			this.rdoAll.CheckedChanged += new System.EventHandler(this.rdoAll_CheckedChanged);
			this.xdgTrade.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			this.xdgTrade.ContextMenuStrip = this.contextMenuStrip1;
			this.xdgTrade.Location = new System.Drawing.Point(0, 0);
			this.xdgTrade.MainView = this.gvTrade;
			this.xdgTrade.Name = "xdgTrade";
			this.xdgTrade.Size = new System.Drawing.Size(512, 237);
			this.xdgTrade.TabIndex = 18;
			this.xdgTrade.ViewCollection.AddRange(new BaseView[]
			{
				this.gvTrade
			});
			this.gvTrade.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.gvTrade.Appearance.ColumnFilterButton.BackColor2 = System.Drawing.Color.LightSkyBlue;
			this.gvTrade.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.White;
			this.gvTrade.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.White;
			this.gvTrade.Appearance.ColumnFilterButton.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.gvTrade.Appearance.ColumnFilterButton.Options.UseBackColor = true;
			this.gvTrade.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
			this.gvTrade.Appearance.ColumnFilterButton.Options.UseForeColor = true;
			this.gvTrade.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(170, 216, 254);
			this.gvTrade.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(170, 216, 254);
			this.gvTrade.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Black;
			this.gvTrade.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
			this.gvTrade.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
			this.gvTrade.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
			this.gvTrade.Appearance.Empty.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.gvTrade.Appearance.Empty.BackColor2 = System.Drawing.Color.FromArgb(224, 224, 224);
			this.gvTrade.Appearance.Empty.Options.UseBackColor = true;
			this.gvTrade.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.gvTrade.Appearance.EvenRow.BackColor2 = System.Drawing.Color.WhiteSmoke;
			this.gvTrade.Appearance.EvenRow.BorderColor = System.Drawing.Color.Transparent;
			this.gvTrade.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
			this.gvTrade.Appearance.EvenRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
			this.gvTrade.Appearance.EvenRow.Options.UseBackColor = true;
			this.gvTrade.Appearance.EvenRow.Options.UseBorderColor = true;
			this.gvTrade.Appearance.EvenRow.Options.UseForeColor = true;
			this.gvTrade.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(104, 184, 251);
			this.gvTrade.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(104, 184, 251);
			this.gvTrade.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.White;
			this.gvTrade.Appearance.FilterCloseButton.Options.UseBackColor = true;
			this.gvTrade.Appearance.FilterCloseButton.Options.UseBorderColor = true;
			this.gvTrade.Appearance.FilterCloseButton.Options.UseForeColor = true;
			this.gvTrade.Appearance.FilterPanel.BackColor = System.Drawing.Color.FromArgb(236, 246, 255);
			this.gvTrade.Appearance.FilterPanel.BackColor2 = System.Drawing.Color.White;
			this.gvTrade.Appearance.FilterPanel.ForeColor = System.Drawing.Color.Black;
			this.gvTrade.Appearance.FilterPanel.Options.UseBackColor = true;
			this.gvTrade.Appearance.FilterPanel.Options.UseForeColor = true;
			this.gvTrade.Appearance.FixedLine.BackColor = System.Drawing.Color.White;
			this.gvTrade.Appearance.FixedLine.Options.UseBackColor = true;
			this.gvTrade.Appearance.FocusedCell.BackColor = System.Drawing.Color.Transparent;
			this.gvTrade.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
			this.gvTrade.Appearance.FocusedCell.Options.UseBackColor = true;
			this.gvTrade.Appearance.FocusedCell.Options.UseForeColor = true;
			this.gvTrade.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.gvTrade.Appearance.FocusedRow.BackColor2 = System.Drawing.Color.FromArgb(224, 224, 224);
			this.gvTrade.Appearance.FocusedRow.BorderColor = System.Drawing.Color.White;
			this.gvTrade.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
			this.gvTrade.Appearance.FocusedRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
			this.gvTrade.Appearance.FocusedRow.Options.UseBackColor = true;
			this.gvTrade.Appearance.FocusedRow.Options.UseBorderColor = true;
			this.gvTrade.Appearance.FocusedRow.Options.UseForeColor = true;
			this.gvTrade.Appearance.FooterPanel.BackColor = System.Drawing.Color.LightCyan;
			this.gvTrade.Appearance.FooterPanel.BackColor2 = System.Drawing.Color.LightSkyBlue;
			this.gvTrade.Appearance.FooterPanel.BorderColor = System.Drawing.Color.Transparent;
			this.gvTrade.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
			this.gvTrade.Appearance.FooterPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.gvTrade.Appearance.FooterPanel.Options.UseBackColor = true;
			this.gvTrade.Appearance.FooterPanel.Options.UseBorderColor = true;
			this.gvTrade.Appearance.FooterPanel.Options.UseForeColor = true;
			this.gvTrade.Appearance.FooterPanel.Options.UseTextOptions = true;
			this.gvTrade.Appearance.FooterPanel.TextOptions.VAlignment = VertAlignment.Top;
			this.gvTrade.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(104, 184, 251);
			this.gvTrade.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(104, 184, 251);
			this.gvTrade.Appearance.GroupButton.Options.UseBackColor = true;
			this.gvTrade.Appearance.GroupButton.Options.UseBorderColor = true;
			this.gvTrade.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.gvTrade.Appearance.GroupFooter.BackColor2 = System.Drawing.Color.FromArgb(255, 255, 245);
			this.gvTrade.Appearance.GroupFooter.BorderColor = System.Drawing.Color.White;
			this.gvTrade.Appearance.GroupFooter.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.gvTrade.Appearance.GroupFooter.Options.UseBackColor = true;
			this.gvTrade.Appearance.GroupFooter.Options.UseBorderColor = true;
			this.gvTrade.Appearance.GroupFooter.Options.UseForeColor = true;
			this.gvTrade.Appearance.GroupFooter.Options.UseTextOptions = true;
			this.gvTrade.Appearance.GroupFooter.TextOptions.VAlignment = VertAlignment.Top;
			this.gvTrade.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(236, 246, 255);
			this.gvTrade.Appearance.GroupPanel.BackColor2 = System.Drawing.Color.White;
			this.gvTrade.Appearance.GroupPanel.ForeColor = System.Drawing.Color.Black;
			this.gvTrade.Appearance.GroupPanel.Options.UseBackColor = true;
			this.gvTrade.Appearance.GroupPanel.Options.UseForeColor = true;
			this.gvTrade.Appearance.GroupRow.BackColor = System.Drawing.Color.WhiteSmoke;
			this.gvTrade.Appearance.GroupRow.BackColor2 = System.Drawing.Color.Gainsboro;
			this.gvTrade.Appearance.GroupRow.BorderColor = System.Drawing.Color.White;
			this.gvTrade.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
			this.gvTrade.Appearance.GroupRow.Options.UseBackColor = true;
			this.gvTrade.Appearance.GroupRow.Options.UseBorderColor = true;
			this.gvTrade.Appearance.GroupRow.Options.UseForeColor = true;
			this.gvTrade.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.gvTrade.Appearance.HideSelectionRow.BackColor2 = System.Drawing.Color.FromArgb(224, 224, 224);
			this.gvTrade.Appearance.HideSelectionRow.BorderColor = System.Drawing.Color.Transparent;
			this.gvTrade.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.Black;
			this.gvTrade.Appearance.HideSelectionRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
			this.gvTrade.Appearance.HideSelectionRow.Options.UseBackColor = true;
			this.gvTrade.Appearance.HideSelectionRow.Options.UseBorderColor = true;
			this.gvTrade.Appearance.HideSelectionRow.Options.UseForeColor = true;
			this.gvTrade.Appearance.HorzLine.BackColor = System.Drawing.Color.White;
			this.gvTrade.Appearance.HorzLine.Options.UseBackColor = true;
			this.gvTrade.Appearance.OddRow.BackColor = System.Drawing.Color.Azure;
			this.gvTrade.Appearance.OddRow.BorderColor = System.Drawing.Color.White;
			this.gvTrade.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
			this.gvTrade.Appearance.OddRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
			this.gvTrade.Appearance.OddRow.Options.UseBackColor = true;
			this.gvTrade.Appearance.OddRow.Options.UseBorderColor = true;
			this.gvTrade.Appearance.OddRow.Options.UseForeColor = true;
			this.gvTrade.Appearance.Preview.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.gvTrade.Appearance.Preview.BackColor2 = System.Drawing.Color.FromArgb(255, 255, 245);
			this.gvTrade.Appearance.Preview.Font = new System.Drawing.Font("Verdana", 7.5f);
			this.gvTrade.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(83, 155, 215);
			this.gvTrade.Appearance.Preview.Options.UseBackColor = true;
			this.gvTrade.Appearance.Preview.Options.UseFont = true;
			this.gvTrade.Appearance.Preview.Options.UseForeColor = true;
			this.gvTrade.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.gvTrade.Appearance.Row.ForeColor = System.Drawing.Color.Black;
			this.gvTrade.Appearance.Row.Options.UseBackColor = true;
			this.gvTrade.Appearance.Row.Options.UseForeColor = true;
			this.gvTrade.Appearance.RowSeparator.BackColor = System.Drawing.Color.FromArgb(236, 246, 255);
			this.gvTrade.Appearance.RowSeparator.BackColor2 = System.Drawing.Color.White;
			this.gvTrade.Appearance.RowSeparator.ForeColor = System.Drawing.Color.Black;
			this.gvTrade.Appearance.RowSeparator.Options.UseBackColor = true;
			this.gvTrade.Appearance.RowSeparator.Options.UseForeColor = true;
			this.gvTrade.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.gvTrade.Appearance.SelectedRow.BackColor2 = System.Drawing.Color.FromArgb(224, 224, 224);
			this.gvTrade.Appearance.SelectedRow.ForeColor = System.Drawing.Color.Black;
			this.gvTrade.Appearance.SelectedRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
			this.gvTrade.Appearance.SelectedRow.Options.UseBackColor = true;
			this.gvTrade.Appearance.SelectedRow.Options.UseForeColor = true;
			this.gvTrade.Appearance.TopNewRow.BackColor = System.Drawing.Color.White;
			this.gvTrade.Appearance.TopNewRow.Options.UseBackColor = true;
			this.gvTrade.Appearance.VertLine.BackColor = System.Drawing.Color.White;
			this.gvTrade.Appearance.VertLine.Options.UseBackColor = true;
			this.gvTrade.BorderStyle = BorderStyles.Simple;
			this.gvTrade.Columns.AddRange(new GridColumn[]
			{
				this.bandedGridColumn4,
				this.bandedGridColumn6,
				this.bandedGridColumn7,
				this.bandedGridColumn8,
				this.gridColumn1,
				this.gridColumn2,
				this.gridColumn3,
				this.gridColumn4,
				this.gridColumn5,
				this.gridColumn6,
				this.gridColumn7,
				this.gridColumn8,
				this.gridColumn9,
				this.gridColumn10,
				this.gridColumn11,
				this.bandedGridColumn15
			});
			this.gvTrade.FocusRectStyle = DrawFocusRectStyle.RowFocus;
			styleFormatCondition3.ApplyToRow = true;
			styleFormatCondition3.Column = this.bandedGridColumn4;
			styleFormatCondition3.Condition = FormatConditionEnum.Equal;
			styleFormatCondition3.Value1 = true;
			this.gvTrade.FormatConditions.AddRange(new StyleFormatCondition[]
			{
				styleFormatCondition3
			});
			this.gvTrade.GridControl = this.xdgTrade;
			this.gvTrade.GroupFooterShowMode = GroupFooterShowMode.Hidden;
			this.gvTrade.GroupSummary.AddRange(new GridSummaryItem[]
			{
				new GridGroupSummaryItem(SummaryItemType.Sum, "成交手数", this.gridColumn6, ""),
				new GridGroupSummaryItem(SummaryItemType.Sum, "报单手数", this.gridColumn4, "")
			});
			this.gvTrade.HorzScrollVisibility = ScrollVisibility.Always;
			this.gvTrade.IndicatorWidth = 40;
			this.gvTrade.Name = "gvTrade";
			this.gvTrade.OptionsBehavior.AutoExpandAllGroups = true;
			this.gvTrade.OptionsLayout.StoreDataSettings = false;
			this.gvTrade.OptionsMenu.EnableColumnMenu = false;
			this.gvTrade.OptionsMenu.EnableFooterMenu = false;
			this.gvTrade.OptionsNavigation.AutoFocusNewRow = true;
			this.gvTrade.OptionsPrint.AutoWidth = false;
			this.gvTrade.OptionsView.ColumnAutoWidth = false;
			this.gvTrade.OptionsView.EnableAppearanceEvenRow = true;
			this.gvTrade.OptionsView.EnableAppearanceOddRow = true;
			this.gvTrade.OptionsView.HeaderFilterButtonShowMode = FilterButtonShowMode.Button;
			this.gvTrade.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;
			this.gvTrade.OptionsView.ShowFooter = true;
			this.gvTrade.OptionsView.ShowGroupPanel = false;
			this.gvTrade.SortInfo.AddRange(new GridColumnSortInfo[]
			{
				new GridColumnSortInfo(this.gridColumn7, ColumnSortOrder.Descending)
			});
			this.gvTrade.VertScrollVisibility = ScrollVisibility.Always;
			this.gvTrade.CustomDrawRowIndicator += new RowIndicatorCustomDrawEventHandler(this.gvTrade_CustomDrawRowIndicator);
			this.gvTrade.RowCellStyle += new RowCellStyleEventHandler(this.gvTrade_RowCellStyle);
			this.bandedGridColumn6.Caption = "编号";
			this.bandedGridColumn6.FieldName = "编号";
			this.bandedGridColumn6.Name = "bandedGridColumn6";
			this.bandedGridColumn6.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn6.OptionsFilter.AllowFilter = false;
			this.bandedGridColumn6.Visible = true;
			this.bandedGridColumn6.VisibleIndex = 1;
			this.bandedGridColumn6.Width = 70;
			this.bandedGridColumn7.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
			this.bandedGridColumn7.AppearanceCell.Options.UseFont = true;
			this.bandedGridColumn7.Caption = "合约";
			this.bandedGridColumn7.FieldName = "合约";
			this.bandedGridColumn7.Name = "bandedGridColumn7";
			this.bandedGridColumn7.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn7.Visible = true;
			this.bandedGridColumn7.VisibleIndex = 2;
			this.bandedGridColumn7.Width = 69;
			this.bandedGridColumn8.Caption = "买卖";
			this.bandedGridColumn8.FieldName = "买卖";
			this.bandedGridColumn8.Name = "bandedGridColumn8";
			this.bandedGridColumn8.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn8.Visible = true;
			this.bandedGridColumn8.VisibleIndex = 3;
			this.bandedGridColumn8.Width = 71;
			this.gridColumn1.Caption = "开平";
			this.gridColumn1.FieldName = "开平";
			this.gridColumn1.Name = "gridColumn1";
			this.gridColumn1.OptionsColumn.AllowEdit = false;
			this.gridColumn1.Visible = true;
			this.gridColumn1.VisibleIndex = 4;
			this.gridColumn1.Width = 50;
			this.gridColumn2.Caption = "状态";
			this.gridColumn2.FieldName = "状态";
			this.gridColumn2.Name = "gridColumn2";
			this.gridColumn2.OptionsColumn.AllowEdit = false;
			this.gridColumn2.OptionsFilter.AllowFilter = false;
			this.gridColumn2.Visible = true;
			this.gridColumn2.VisibleIndex = 5;
			this.gridColumn3.Caption = "价格";
			this.gridColumn3.FieldName = "价格";
			this.gridColumn3.Name = "gridColumn3";
			this.gridColumn3.OptionsColumn.AllowEdit = false;
			this.gridColumn3.OptionsFilter.AllowFilter = false;
			this.gridColumn3.Visible = true;
			this.gridColumn3.VisibleIndex = 6;
			this.gridColumn4.Caption = "报单手数";
			this.gridColumn4.FieldName = "报单手数";
			this.gridColumn4.GroupFormat.FormatType = FormatType.Numeric;
			this.gridColumn4.Name = "gridColumn4";
			this.gridColumn4.OptionsColumn.AllowEdit = false;
			this.gridColumn4.OptionsFilter.AllowFilter = false;
			this.gridColumn4.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Sum)
			});
			this.gridColumn4.Visible = true;
			this.gridColumn4.VisibleIndex = 7;
			this.gridColumn5.Caption = "未成交";
			this.gridColumn5.FieldName = "未成交";
			this.gridColumn5.Name = "gridColumn5";
			this.gridColumn5.OptionsColumn.AllowEdit = false;
			this.gridColumn5.OptionsFilter.AllowFilter = false;
			this.gridColumn5.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Sum)
			});
			this.gridColumn5.Visible = true;
			this.gridColumn5.VisibleIndex = 8;
			this.gridColumn6.Caption = "成交手数";
			this.gridColumn6.FieldName = "成交手数";
			this.gridColumn6.GroupFormat.FormatType = FormatType.Numeric;
			this.gridColumn6.Name = "gridColumn6";
			this.gridColumn6.OptionsColumn.AllowEdit = false;
			this.gridColumn6.OptionsFilter.AllowFilter = false;
			this.gridColumn6.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Sum)
			});
			this.gridColumn6.Visible = true;
			this.gridColumn6.VisibleIndex = 9;
			this.gridColumn7.Caption = "报单时间";
			this.gridColumn7.DisplayFormat.FormatString = "HH:mm:ss";
			this.gridColumn7.DisplayFormat.FormatType = FormatType.DateTime;
			this.gridColumn7.FieldName = "报单时间";
			this.gridColumn7.Name = "gridColumn7";
			this.gridColumn7.OptionsColumn.AllowEdit = false;
			this.gridColumn7.OptionsFilter.AllowFilter = false;
			this.gridColumn7.SortMode = ColumnSortMode.Value;
			this.gridColumn7.Visible = true;
			this.gridColumn7.VisibleIndex = 10;
			this.gridColumn7.Width = 90;
			this.gridColumn8.Caption = "成交时间";
			this.gridColumn8.FieldName = "成交时间";
			this.gridColumn8.Name = "gridColumn8";
			this.gridColumn8.OptionsColumn.AllowEdit = false;
			this.gridColumn8.OptionsFilter.AllowFilter = false;
			this.gridColumn8.Visible = true;
			this.gridColumn8.VisibleIndex = 11;
			this.gridColumn8.Width = 110;
			this.gridColumn9.Caption = "成交均价";
			this.gridColumn9.FieldName = "成交均价";
			this.gridColumn9.Name = "gridColumn9";
			this.gridColumn9.OptionsColumn.AllowEdit = false;
			this.gridColumn9.OptionsFilter.AllowFilter = false;
			this.gridColumn9.Visible = true;
			this.gridColumn9.VisibleIndex = 12;
			this.gridColumn10.Caption = "详细状态";
			this.gridColumn10.FieldName = "详细状态";
			this.gridColumn10.Name = "gridColumn10";
			this.gridColumn10.OptionsColumn.AllowEdit = false;
			this.gridColumn10.OptionsFilter.AllowFilter = false;
			this.gridColumn10.Visible = true;
			this.gridColumn10.VisibleIndex = 13;
			this.gridColumn10.Width = 200;
			this.gridColumn11.Caption = "客户信息";
			this.gridColumn11.FieldName = "客户信息";
			this.gridColumn11.Name = "gridColumn11";
			this.gridColumn11.OptionsColumn.AllowEdit = false;
			this.gridColumn11.Visible = true;
			this.gridColumn11.VisibleIndex = 14;
			this.gridColumn11.Width = 106;
			this.bandedGridColumn15.Caption = "序列号";
			this.bandedGridColumn15.FieldName = "序列号";
			this.bandedGridColumn15.Name = "bandedGridColumn15";
			this.bandedGridColumn15.OptionsColumn.AllowEdit = false;
			this.bandedGridColumn15.OptionsFilter.AllowFilter = false;
			this.bandedGridColumn15.Visible = true;
			this.bandedGridColumn15.VisibleIndex = 15;
			this.xtabViewTrade.Controls.Add(this.xdgDeal);
			this.xtabViewTrade.Name = "xtabViewTrade";
			this.xtabViewTrade.Size = new System.Drawing.Size(512, 264);
			this.xtabViewTrade.Text = "成交记录";
			this.xdgDeal.ContextMenuStrip = this.contextMenuStrip4;
			this.xdgDeal.Dock = System.Windows.Forms.DockStyle.Fill;
			this.xdgDeal.Location = new System.Drawing.Point(0, 0);
			this.xdgDeal.MainView = this.gvDeal;
			this.xdgDeal.Name = "xdgDeal";
			this.xdgDeal.Size = new System.Drawing.Size(512, 264);
			this.xdgDeal.TabIndex = 19;
			this.xdgDeal.ViewCollection.AddRange(new BaseView[]
			{
				this.gvDeal
			});
			this.contextMenuStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
			{
				this.导出数据ToolStripMenuItem2
			});
			this.contextMenuStrip4.Name = "contextMenuStrip4";
			this.contextMenuStrip4.Size = new System.Drawing.Size(125, 26);
			this.导出数据ToolStripMenuItem2.Name = "导出数据ToolStripMenuItem2";
			this.导出数据ToolStripMenuItem2.Size = new System.Drawing.Size(124, 22);
			this.导出数据ToolStripMenuItem2.Text = "导出数据";
			this.导出数据ToolStripMenuItem2.Click += new System.EventHandler(this.导出数据ToolStripMenuItem2_Click);
			this.gvDeal.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.gvDeal.Appearance.ColumnFilterButton.BackColor2 = System.Drawing.Color.LightSkyBlue;
			this.gvDeal.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.White;
			this.gvDeal.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.White;
			this.gvDeal.Appearance.ColumnFilterButton.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.gvDeal.Appearance.ColumnFilterButton.Options.UseBackColor = true;
			this.gvDeal.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
			this.gvDeal.Appearance.ColumnFilterButton.Options.UseForeColor = true;
			this.gvDeal.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(170, 216, 254);
			this.gvDeal.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(170, 216, 254);
			this.gvDeal.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Black;
			this.gvDeal.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
			this.gvDeal.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
			this.gvDeal.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
			this.gvDeal.Appearance.Empty.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.gvDeal.Appearance.Empty.BackColor2 = System.Drawing.Color.FromArgb(224, 224, 224);
			this.gvDeal.Appearance.Empty.Options.UseBackColor = true;
			this.gvDeal.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.gvDeal.Appearance.EvenRow.BackColor2 = System.Drawing.Color.WhiteSmoke;
			this.gvDeal.Appearance.EvenRow.BorderColor = System.Drawing.Color.Transparent;
			this.gvDeal.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
			this.gvDeal.Appearance.EvenRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
			this.gvDeal.Appearance.EvenRow.Options.UseBackColor = true;
			this.gvDeal.Appearance.EvenRow.Options.UseBorderColor = true;
			this.gvDeal.Appearance.EvenRow.Options.UseForeColor = true;
			this.gvDeal.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(104, 184, 251);
			this.gvDeal.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(104, 184, 251);
			this.gvDeal.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.White;
			this.gvDeal.Appearance.FilterCloseButton.Options.UseBackColor = true;
			this.gvDeal.Appearance.FilterCloseButton.Options.UseBorderColor = true;
			this.gvDeal.Appearance.FilterCloseButton.Options.UseForeColor = true;
			this.gvDeal.Appearance.FilterPanel.BackColor = System.Drawing.Color.FromArgb(236, 246, 255);
			this.gvDeal.Appearance.FilterPanel.BackColor2 = System.Drawing.Color.White;
			this.gvDeal.Appearance.FilterPanel.ForeColor = System.Drawing.Color.Black;
			this.gvDeal.Appearance.FilterPanel.Options.UseBackColor = true;
			this.gvDeal.Appearance.FilterPanel.Options.UseForeColor = true;
			this.gvDeal.Appearance.FixedLine.BackColor = System.Drawing.Color.White;
			this.gvDeal.Appearance.FixedLine.Options.UseBackColor = true;
			this.gvDeal.Appearance.FocusedCell.BackColor = System.Drawing.Color.Transparent;
			this.gvDeal.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
			this.gvDeal.Appearance.FocusedCell.Options.UseBackColor = true;
			this.gvDeal.Appearance.FocusedCell.Options.UseForeColor = true;
			this.gvDeal.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.gvDeal.Appearance.FocusedRow.BackColor2 = System.Drawing.Color.FromArgb(224, 224, 224);
			this.gvDeal.Appearance.FocusedRow.BorderColor = System.Drawing.Color.White;
			this.gvDeal.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
			this.gvDeal.Appearance.FocusedRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
			this.gvDeal.Appearance.FocusedRow.Options.UseBackColor = true;
			this.gvDeal.Appearance.FocusedRow.Options.UseBorderColor = true;
			this.gvDeal.Appearance.FocusedRow.Options.UseForeColor = true;
			this.gvDeal.Appearance.FooterPanel.BackColor = System.Drawing.Color.LightCyan;
			this.gvDeal.Appearance.FooterPanel.BackColor2 = System.Drawing.Color.LightSkyBlue;
			this.gvDeal.Appearance.FooterPanel.BorderColor = System.Drawing.Color.Transparent;
			this.gvDeal.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
			this.gvDeal.Appearance.FooterPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.gvDeal.Appearance.FooterPanel.Options.UseBackColor = true;
			this.gvDeal.Appearance.FooterPanel.Options.UseForeColor = true;
			this.gvDeal.Appearance.FooterPanel.Options.UseTextOptions = true;
			this.gvDeal.Appearance.FooterPanel.TextOptions.VAlignment = VertAlignment.Top;
			this.gvDeal.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(104, 184, 251);
			this.gvDeal.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(104, 184, 251);
			this.gvDeal.Appearance.GroupButton.Options.UseBackColor = true;
			this.gvDeal.Appearance.GroupButton.Options.UseBorderColor = true;
			this.gvDeal.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.gvDeal.Appearance.GroupFooter.BackColor2 = System.Drawing.Color.FromArgb(255, 255, 245);
			this.gvDeal.Appearance.GroupFooter.BorderColor = System.Drawing.Color.White;
			this.gvDeal.Appearance.GroupFooter.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.gvDeal.Appearance.GroupFooter.Options.UseBackColor = true;
			this.gvDeal.Appearance.GroupFooter.Options.UseBorderColor = true;
			this.gvDeal.Appearance.GroupFooter.Options.UseForeColor = true;
			this.gvDeal.Appearance.GroupFooter.Options.UseTextOptions = true;
			this.gvDeal.Appearance.GroupFooter.TextOptions.VAlignment = VertAlignment.Top;
			this.gvDeal.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(236, 246, 255);
			this.gvDeal.Appearance.GroupPanel.BackColor2 = System.Drawing.Color.White;
			this.gvDeal.Appearance.GroupPanel.ForeColor = System.Drawing.Color.Black;
			this.gvDeal.Appearance.GroupPanel.Options.UseBackColor = true;
			this.gvDeal.Appearance.GroupPanel.Options.UseForeColor = true;
			this.gvDeal.Appearance.GroupRow.BackColor = System.Drawing.Color.WhiteSmoke;
			this.gvDeal.Appearance.GroupRow.BackColor2 = System.Drawing.Color.Gainsboro;
			this.gvDeal.Appearance.GroupRow.BorderColor = System.Drawing.Color.White;
			this.gvDeal.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
			this.gvDeal.Appearance.GroupRow.Options.UseBackColor = true;
			this.gvDeal.Appearance.GroupRow.Options.UseBorderColor = true;
			this.gvDeal.Appearance.GroupRow.Options.UseForeColor = true;
			this.gvDeal.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.gvDeal.Appearance.HideSelectionRow.BackColor2 = System.Drawing.Color.FromArgb(224, 224, 224);
			this.gvDeal.Appearance.HideSelectionRow.BorderColor = System.Drawing.Color.Transparent;
			this.gvDeal.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.Black;
			this.gvDeal.Appearance.HideSelectionRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
			this.gvDeal.Appearance.HideSelectionRow.Options.UseBackColor = true;
			this.gvDeal.Appearance.HideSelectionRow.Options.UseBorderColor = true;
			this.gvDeal.Appearance.HideSelectionRow.Options.UseForeColor = true;
			this.gvDeal.Appearance.HorzLine.BackColor = System.Drawing.Color.White;
			this.gvDeal.Appearance.HorzLine.Options.UseBackColor = true;
			this.gvDeal.Appearance.OddRow.BackColor = System.Drawing.Color.Azure;
			this.gvDeal.Appearance.OddRow.BorderColor = System.Drawing.Color.White;
			this.gvDeal.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
			this.gvDeal.Appearance.OddRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
			this.gvDeal.Appearance.OddRow.Options.UseBackColor = true;
			this.gvDeal.Appearance.OddRow.Options.UseBorderColor = true;
			this.gvDeal.Appearance.OddRow.Options.UseForeColor = true;
			this.gvDeal.Appearance.Preview.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.gvDeal.Appearance.Preview.BackColor2 = System.Drawing.Color.FromArgb(255, 255, 245);
			this.gvDeal.Appearance.Preview.Font = new System.Drawing.Font("Verdana", 7.5f);
			this.gvDeal.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(83, 155, 215);
			this.gvDeal.Appearance.Preview.Options.UseBackColor = true;
			this.gvDeal.Appearance.Preview.Options.UseFont = true;
			this.gvDeal.Appearance.Preview.Options.UseForeColor = true;
			this.gvDeal.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.gvDeal.Appearance.Row.ForeColor = System.Drawing.Color.Black;
			this.gvDeal.Appearance.Row.Options.UseBackColor = true;
			this.gvDeal.Appearance.Row.Options.UseForeColor = true;
			this.gvDeal.Appearance.RowSeparator.BackColor = System.Drawing.Color.FromArgb(236, 246, 255);
			this.gvDeal.Appearance.RowSeparator.BackColor2 = System.Drawing.Color.White;
			this.gvDeal.Appearance.RowSeparator.ForeColor = System.Drawing.Color.Black;
			this.gvDeal.Appearance.RowSeparator.Options.UseBackColor = true;
			this.gvDeal.Appearance.RowSeparator.Options.UseForeColor = true;
			this.gvDeal.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.gvDeal.Appearance.SelectedRow.BackColor2 = System.Drawing.Color.FromArgb(224, 224, 224);
			this.gvDeal.Appearance.SelectedRow.ForeColor = System.Drawing.Color.Black;
			this.gvDeal.Appearance.SelectedRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
			this.gvDeal.Appearance.SelectedRow.Options.UseBackColor = true;
			this.gvDeal.Appearance.SelectedRow.Options.UseForeColor = true;
			this.gvDeal.Appearance.TopNewRow.BackColor = System.Drawing.Color.White;
			this.gvDeal.Appearance.TopNewRow.Options.UseBackColor = true;
			this.gvDeal.Appearance.VertLine.BackColor = System.Drawing.Color.White;
			this.gvDeal.Appearance.VertLine.Options.UseBackColor = true;
			this.gvDeal.BorderStyle = BorderStyles.Simple;
			this.gvDeal.Columns.AddRange(new GridColumn[]
			{
				this.gridColumn12,
				this.gridColumn13,
				this.gridColumn14,
				this.gridColumn15,
				this.gridColumn16,
				this.gridColumn18,
				this.gridColumn21,
				this.gridColumn23,
				this.gridColumn24,
				this.gridColumn25,
				this.gridColumn26,
				this.gridColumn17
			});
			this.gvDeal.FooterPanelHeight = 5;
			styleFormatCondition4.ApplyToRow = true;
			styleFormatCondition4.Column = this.gridColumn12;
			styleFormatCondition4.Condition = FormatConditionEnum.Equal;
			styleFormatCondition4.Value1 = true;
			this.gvDeal.FormatConditions.AddRange(new StyleFormatCondition[]
			{
				styleFormatCondition4
			});
			this.gvDeal.GridControl = this.xdgDeal;
			this.gvDeal.GroupFooterShowMode = GroupFooterShowMode.Hidden;
			this.gvDeal.HorzScrollVisibility = ScrollVisibility.Always;
			this.gvDeal.Name = "gvDeal";
			this.gvDeal.OptionsBehavior.AutoExpandAllGroups = true;
			this.gvDeal.OptionsMenu.EnableColumnMenu = false;
			this.gvDeal.OptionsMenu.EnableFooterMenu = false;
			this.gvDeal.OptionsMenu.EnableGroupPanelMenu = false;
			this.gvDeal.OptionsPrint.AutoWidth = false;
			this.gvDeal.OptionsView.ColumnAutoWidth = false;
			this.gvDeal.OptionsView.EnableAppearanceEvenRow = true;
			this.gvDeal.OptionsView.EnableAppearanceOddRow = true;
			this.gvDeal.OptionsView.HeaderFilterButtonShowMode = FilterButtonShowMode.Button;
			this.gvDeal.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;
			this.gvDeal.OptionsView.ShowFooter = true;
			this.gvDeal.OptionsView.ShowGroupPanel = false;
			this.gvDeal.SortInfo.AddRange(new GridColumnSortInfo[]
			{
				new GridColumnSortInfo(this.gridColumn23, ColumnSortOrder.Descending)
			});
			this.gvDeal.VertScrollVisibility = ScrollVisibility.Always;
			this.gvDeal.RowCellStyle += new RowCellStyleEventHandler(this.gvDeal_RowCellStyle);
			this.gridColumn13.Caption = "编号";
			this.gridColumn13.FieldName = "编号";
			this.gridColumn13.Name = "gridColumn13";
			this.gridColumn13.OptionsColumn.AllowEdit = false;
			this.gridColumn13.OptionsFilter.AllowFilter = false;
			this.gridColumn13.Visible = true;
			this.gridColumn13.VisibleIndex = 1;
			this.gridColumn13.Width = 79;
			this.gridColumn14.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
			this.gridColumn14.AppearanceCell.Options.UseFont = true;
			this.gridColumn14.Caption = "合约";
			this.gridColumn14.FieldName = "合约";
			this.gridColumn14.Name = "gridColumn14";
			this.gridColumn14.OptionsColumn.AllowEdit = false;
			this.gridColumn14.Visible = true;
			this.gridColumn14.VisibleIndex = 2;
			this.gridColumn14.Width = 79;
			this.gridColumn15.Caption = "买卖";
			this.gridColumn15.FieldName = "买卖";
			this.gridColumn15.Name = "gridColumn15";
			this.gridColumn15.OptionsColumn.AllowEdit = false;
			this.gridColumn15.Visible = true;
			this.gridColumn15.VisibleIndex = 3;
			this.gridColumn15.Width = 71;
			this.gridColumn16.Caption = "开平";
			this.gridColumn16.FieldName = "开平";
			this.gridColumn16.Name = "gridColumn16";
			this.gridColumn16.OptionsColumn.AllowEdit = false;
			this.gridColumn16.Visible = true;
			this.gridColumn16.VisibleIndex = 4;
			this.gridColumn16.Width = 55;
			this.gridColumn18.Caption = "成交价格";
			this.gridColumn18.FieldName = "成交价格";
			this.gridColumn18.Name = "gridColumn18";
			this.gridColumn18.OptionsColumn.AllowEdit = false;
			this.gridColumn18.OptionsFilter.AllowFilter = false;
			this.gridColumn18.Visible = true;
			this.gridColumn18.VisibleIndex = 5;
			this.gridColumn21.Caption = "成交手数";
			this.gridColumn21.FieldName = "成交手数";
			this.gridColumn21.Name = "gridColumn21";
			this.gridColumn21.OptionsColumn.AllowEdit = false;
			this.gridColumn21.OptionsFilter.AllowFilter = false;
			this.gridColumn21.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Sum)
			});
			this.gridColumn21.Visible = true;
			this.gridColumn21.VisibleIndex = 6;
			this.gridColumn23.Caption = "成交时间";
			this.gridColumn23.FieldName = "成交时间";
			this.gridColumn23.Name = "gridColumn23";
			this.gridColumn23.OptionsColumn.AllowEdit = false;
			this.gridColumn23.OptionsFilter.AllowFilter = false;
			this.gridColumn23.Visible = true;
			this.gridColumn23.VisibleIndex = 7;
			this.gridColumn23.Width = 110;
			this.gridColumn24.Caption = "报单编号";
			this.gridColumn24.FieldName = "报单编号";
			this.gridColumn24.Name = "gridColumn24";
			this.gridColumn24.OptionsColumn.AllowEdit = false;
			this.gridColumn24.OptionsFilter.AllowFilter = false;
			this.gridColumn24.Visible = true;
			this.gridColumn24.VisibleIndex = 8;
			this.gridColumn24.Width = 83;
			this.gridColumn25.Caption = "成交类型";
			this.gridColumn25.FieldName = "成交类型";
			this.gridColumn25.Name = "gridColumn25";
			this.gridColumn25.OptionsColumn.AllowEdit = false;
			this.gridColumn25.OptionsFilter.AllowFilter = false;
			this.gridColumn25.Visible = true;
			this.gridColumn25.VisibleIndex = 9;
			this.gridColumn25.Width = 63;
			this.gridColumn26.Caption = "交易所";
			this.gridColumn26.FieldName = "交易所";
			this.gridColumn26.Name = "gridColumn26";
			this.gridColumn26.OptionsColumn.AllowEdit = false;
			this.gridColumn26.OptionsFilter.AllowFilter = false;
			this.gridColumn26.Visible = true;
			this.gridColumn26.VisibleIndex = 10;
			this.gridColumn26.Width = 69;
			this.gridColumn17.Caption = "滑点";
			this.gridColumn17.FieldName = "滑点";
			this.gridColumn17.Name = "gridColumn17";
			this.gridColumn17.OptionsColumn.AllowEdit = false;
			this.gridColumn17.OptionsFilter.AllowFilter = false;
			this.gridColumn17.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Sum)
			});
			this.gridColumn17.Visible = true;
			this.gridColumn17.VisibleIndex = 11;
			this.spcMain.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			this.spcMain.Appearance.BackColor = System.Drawing.Color.FromArgb(235, 236, 239);
			this.spcMain.Appearance.Options.UseBackColor = true;
			this.spcMain.CollapsePanel = SplitCollapsePanel.Panel1;
			this.spcMain.Horizontal = false;
			this.spcMain.Location = new System.Drawing.Point(1, 43);
			this.spcMain.LookAndFeel.Style = LookAndFeelStyle.Flat;
			this.spcMain.LookAndFeel.UseDefaultLookAndFeel = false;
			this.spcMain.Name = "spcMain";
			this.spcMain.Panel1.Controls.Add(this.spcMainAccount);
			this.spcMain.Panel1.Text = "Panel1";
			this.spcMain.Panel2.Controls.Add(this.splitContainerControl2);
			this.spcMain.Panel2.Text = "Panel2";
			this.spcMain.Size = new System.Drawing.Size(1044, 807);
			this.spcMain.SplitterPosition = 330;
			this.spcMain.TabIndex = 18;
			this.spcMain.Text = "splitContainerControl1";
			this.spcMainAccount.Appearance.BackColor = System.Drawing.Color.FromArgb(235, 236, 239);
			this.spcMainAccount.Appearance.Options.UseBackColor = true;
			this.spcMainAccount.CollapsePanel = SplitCollapsePanel.Panel1;
			this.spcMainAccount.Dock = System.Windows.Forms.DockStyle.Fill;
			this.spcMainAccount.Location = new System.Drawing.Point(0, 0);
			this.spcMainAccount.LookAndFeel.Style = LookAndFeelStyle.Flat;
			this.spcMainAccount.LookAndFeel.UseDefaultLookAndFeel = false;
			this.spcMainAccount.Name = "spcMainAccount";
			this.spcMainAccount.Panel1.Controls.Add(this.btnMainSelAll);
			this.spcMainAccount.Panel1.Controls.Add(this.butMainLoginOut);
			this.spcMainAccount.Panel1.Controls.Add(this.butMainLogin);
			this.spcMainAccount.Panel1.Controls.Add(this.xdgMainAccount);
			this.spcMainAccount.Panel1.MinSize = 420;
			this.spcMainAccount.Panel1.Text = "Panel1";
			this.spcMainAccount.Panel2.Controls.Add(this.spcMainPosition);
			this.spcMainAccount.Panel2.Text = "Panel2";
			this.spcMainAccount.Size = new System.Drawing.Size(1044, 330);
			this.spcMainAccount.SplitterPosition = 825;
			this.spcMainAccount.TabIndex = 0;
			this.spcMainAccount.Text = "splitContainerControl1";
			this.btnMainSelAll.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.btnMainSelAll.Location = new System.Drawing.Point(773, 3);
			this.btnMainSelAll.Name = "btnMainSelAll";
			this.btnMainSelAll.Size = new System.Drawing.Size(50, 23);
			this.btnMainSelAll.TabIndex = 20;
			this.btnMainSelAll.Text = "全选";
			this.btnMainSelAll.UseVisualStyleBackColor = true;
			this.btnMainSelAll.Click += new System.EventHandler(this.btnMainSelAll_Click);
			this.butMainLoginOut.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.butMainLoginOut.Location = new System.Drawing.Point(773, 64);
			this.butMainLoginOut.Name = "butMainLoginOut";
			this.butMainLoginOut.Size = new System.Drawing.Size(50, 23);
			this.butMainLoginOut.TabIndex = 19;
			this.butMainLoginOut.Text = "注销";
			this.butMainLoginOut.UseVisualStyleBackColor = true;
			this.butMainLoginOut.Click += new System.EventHandler(this.butMainLoginOut_Click);
			this.butMainLogin.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.butMainLogin.Enabled = false;
			this.butMainLogin.Location = new System.Drawing.Point(773, 33);
			this.butMainLogin.Name = "butMainLogin";
			this.butMainLogin.Size = new System.Drawing.Size(50, 23);
			this.butMainLogin.TabIndex = 18;
			this.butMainLogin.Text = "登录";
			this.butMainLogin.UseVisualStyleBackColor = true;
			this.butMainLogin.Click += new System.EventHandler(this.butMainLogin_Click);
			this.spcMainPosition.Appearance.BackColor = System.Drawing.Color.FromArgb(235, 236, 239);
			this.spcMainPosition.Appearance.Options.UseBackColor = true;
			this.spcMainPosition.CollapsePanel = SplitCollapsePanel.Panel1;
			this.spcMainPosition.Dock = System.Windows.Forms.DockStyle.Fill;
			this.spcMainPosition.FixedPanel = SplitFixedPanel.None;
			this.spcMainPosition.Location = new System.Drawing.Point(0, 0);
			this.spcMainPosition.LookAndFeel.Style = LookAndFeelStyle.Flat;
			this.spcMainPosition.LookAndFeel.UseDefaultLookAndFeel = false;
			this.spcMainPosition.Name = "spcMainPosition";
			this.spcMainPosition.Panel1.Controls.Add(this.tabMain);
			this.spcMainPosition.Panel1.Text = "Panel1";
			this.spcMainPosition.Panel2.Controls.Add(this.xtraTabControl2);
			this.spcMainPosition.Panel2.Text = "Panel2";
			this.spcMainPosition.Size = new System.Drawing.Size(215, 330);
			this.spcMainPosition.SplitterPosition = 98;
			this.spcMainPosition.TabIndex = 1;
			this.spcMainPosition.Text = "splitContainerControl2";
			this.tabMain.AllowDrop = true;
			this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabMain.Font = new System.Drawing.Font("宋体", 10f);
			this.tabMain.Location = new System.Drawing.Point(0, 0);
			this.tabMain.Multiline = true;
			this.tabMain.Name = "tabMain";
			this.tabMain.SelectedIndex = 0;
			this.tabMain.Size = new System.Drawing.Size(98, 330);
			this.tabMain.TabIndex = 19;
			this.tabMain.DoubleClick += new System.EventHandler(this.tabMain_DoubleClick);
			this.xtraTabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.xtraTabControl2.Location = new System.Drawing.Point(0, 0);
			this.xtraTabControl2.Name = "xtraTabControl2";
			this.xtraTabControl2.SelectedTabPage = this.xtraTabPage1;
			this.xtraTabControl2.Size = new System.Drawing.Size(113, 330);
			this.xtraTabControl2.TabIndex = 19;
			this.xtraTabControl2.TabPages.AddRange(new XtraTabPage[]
			{
				this.xtraTabPage1,
				this.xtraTabPage2
			});
			this.xtraTabPage1.Controls.Add(this.rdoMainRevoke);
			this.xtraTabPage1.Controls.Add(this.rdoMainDeal);
			this.xtraTabPage1.Controls.Add(this.rdoMainSuspend);
			this.xtraTabPage1.Controls.Add(this.rdoMainAll);
			this.xtraTabPage1.Controls.Add(this.xdgMianTrade);
			this.xtraTabPage1.Name = "xtraTabPage1";
			this.xtraTabPage1.Size = new System.Drawing.Size(107, 301);
			this.xtraTabPage1.Text = "所有委托单";
			this.rdoMainRevoke.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			this.rdoMainRevoke.AutoSize = true;
			this.rdoMainRevoke.Location = new System.Drawing.Point(185, 282);
			this.rdoMainRevoke.Name = "rdoMainRevoke";
			this.rdoMainRevoke.Size = new System.Drawing.Size(59, 16);
			this.rdoMainRevoke.TabIndex = 22;
			this.rdoMainRevoke.Text = "已撤单";
			this.rdoMainRevoke.UseVisualStyleBackColor = true;
			this.rdoMainRevoke.CheckedChanged += new System.EventHandler(this.rdoMainRevoke_CheckedChanged);
			this.rdoMainDeal.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			this.rdoMainDeal.AutoSize = true;
			this.rdoMainDeal.Location = new System.Drawing.Point(120, 282);
			this.rdoMainDeal.Name = "rdoMainDeal";
			this.rdoMainDeal.Size = new System.Drawing.Size(59, 16);
			this.rdoMainDeal.TabIndex = 21;
			this.rdoMainDeal.Text = "已成交";
			this.rdoMainDeal.UseVisualStyleBackColor = true;
			this.rdoMainDeal.CheckedChanged += new System.EventHandler(this.rdoMainDeal_CheckedChanged);
			this.rdoMainSuspend.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			this.rdoMainSuspend.AutoSize = true;
			this.rdoMainSuspend.Location = new System.Drawing.Point(67, 282);
			this.rdoMainSuspend.Name = "rdoMainSuspend";
			this.rdoMainSuspend.Size = new System.Drawing.Size(47, 16);
			this.rdoMainSuspend.TabIndex = 20;
			this.rdoMainSuspend.Text = "挂单";
			this.rdoMainSuspend.UseVisualStyleBackColor = true;
			this.rdoMainSuspend.CheckedChanged += new System.EventHandler(this.rdoMainSuspend_CheckedChanged);
			this.rdoMainAll.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			this.rdoMainAll.AutoSize = true;
			this.rdoMainAll.Checked = true;
			this.rdoMainAll.Location = new System.Drawing.Point(2, 282);
			this.rdoMainAll.Name = "rdoMainAll";
			this.rdoMainAll.Size = new System.Drawing.Size(59, 16);
			this.rdoMainAll.TabIndex = 19;
			this.rdoMainAll.TabStop = true;
			this.rdoMainAll.Text = "全部单";
			this.rdoMainAll.UseVisualStyleBackColor = true;
			this.rdoMainAll.CheckedChanged += new System.EventHandler(this.rdoMainAll_CheckedChanged);
			this.xdgMianTrade.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			this.xdgMianTrade.ContextMenuStrip = this.contextMenuStrip2;
			this.xdgMianTrade.Location = new System.Drawing.Point(3, 1);
			this.xdgMianTrade.MainView = this.gvMainTrade;
			this.xdgMianTrade.Name = "xdgMianTrade";
			this.xdgMianTrade.Size = new System.Drawing.Size(106, 275);
			this.xdgMianTrade.TabIndex = 18;
			this.xdgMianTrade.ViewCollection.AddRange(new BaseView[]
			{
				this.gvMainTrade
			});
			this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
			{
				this.导出数据ToolStripMenuItem
			});
			this.contextMenuStrip2.Name = "contextMenuStrip2";
			this.contextMenuStrip2.Size = new System.Drawing.Size(125, 26);
			this.导出数据ToolStripMenuItem.Name = "导出数据ToolStripMenuItem";
			this.导出数据ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
			this.导出数据ToolStripMenuItem.Text = "导出数据";
			this.导出数据ToolStripMenuItem.Click += new System.EventHandler(this.导出数据ToolStripMenuItem_Click);
			this.gvMainTrade.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.gvMainTrade.Appearance.ColumnFilterButton.BackColor2 = System.Drawing.Color.LightSkyBlue;
			this.gvMainTrade.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.White;
			this.gvMainTrade.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.White;
			this.gvMainTrade.Appearance.ColumnFilterButton.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.gvMainTrade.Appearance.ColumnFilterButton.Options.UseBackColor = true;
			this.gvMainTrade.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
			this.gvMainTrade.Appearance.ColumnFilterButton.Options.UseForeColor = true;
			this.gvMainTrade.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(170, 216, 254);
			this.gvMainTrade.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(170, 216, 254);
			this.gvMainTrade.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Black;
			this.gvMainTrade.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
			this.gvMainTrade.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
			this.gvMainTrade.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
			this.gvMainTrade.Appearance.Empty.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.gvMainTrade.Appearance.Empty.BackColor2 = System.Drawing.Color.FromArgb(224, 224, 224);
			this.gvMainTrade.Appearance.Empty.Options.UseBackColor = true;
			this.gvMainTrade.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.gvMainTrade.Appearance.EvenRow.BackColor2 = System.Drawing.Color.WhiteSmoke;
			this.gvMainTrade.Appearance.EvenRow.BorderColor = System.Drawing.Color.Transparent;
			this.gvMainTrade.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
			this.gvMainTrade.Appearance.EvenRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
			this.gvMainTrade.Appearance.EvenRow.Options.UseBackColor = true;
			this.gvMainTrade.Appearance.EvenRow.Options.UseBorderColor = true;
			this.gvMainTrade.Appearance.EvenRow.Options.UseForeColor = true;
			this.gvMainTrade.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(104, 184, 251);
			this.gvMainTrade.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(104, 184, 251);
			this.gvMainTrade.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.White;
			this.gvMainTrade.Appearance.FilterCloseButton.Options.UseBackColor = true;
			this.gvMainTrade.Appearance.FilterCloseButton.Options.UseBorderColor = true;
			this.gvMainTrade.Appearance.FilterCloseButton.Options.UseForeColor = true;
			this.gvMainTrade.Appearance.FilterPanel.BackColor = System.Drawing.Color.FromArgb(236, 246, 255);
			this.gvMainTrade.Appearance.FilterPanel.BackColor2 = System.Drawing.Color.White;
			this.gvMainTrade.Appearance.FilterPanel.ForeColor = System.Drawing.Color.Black;
			this.gvMainTrade.Appearance.FilterPanel.Options.UseBackColor = true;
			this.gvMainTrade.Appearance.FilterPanel.Options.UseForeColor = true;
			this.gvMainTrade.Appearance.FixedLine.BackColor = System.Drawing.Color.White;
			this.gvMainTrade.Appearance.FixedLine.Options.UseBackColor = true;
			this.gvMainTrade.Appearance.FocusedCell.BackColor = System.Drawing.Color.Transparent;
			this.gvMainTrade.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
			this.gvMainTrade.Appearance.FocusedCell.Options.UseBackColor = true;
			this.gvMainTrade.Appearance.FocusedCell.Options.UseForeColor = true;
			this.gvMainTrade.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.gvMainTrade.Appearance.FocusedRow.BackColor2 = System.Drawing.Color.FromArgb(224, 224, 224);
			this.gvMainTrade.Appearance.FocusedRow.BorderColor = System.Drawing.Color.White;
			this.gvMainTrade.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
			this.gvMainTrade.Appearance.FocusedRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
			this.gvMainTrade.Appearance.FocusedRow.Options.UseBackColor = true;
			this.gvMainTrade.Appearance.FocusedRow.Options.UseBorderColor = true;
			this.gvMainTrade.Appearance.FocusedRow.Options.UseForeColor = true;
			this.gvMainTrade.Appearance.FooterPanel.BackColor = System.Drawing.Color.LightCyan;
			this.gvMainTrade.Appearance.FooterPanel.BackColor2 = System.Drawing.Color.LightSkyBlue;
			this.gvMainTrade.Appearance.FooterPanel.BorderColor = System.Drawing.Color.Transparent;
			this.gvMainTrade.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
			this.gvMainTrade.Appearance.FooterPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.gvMainTrade.Appearance.FooterPanel.Options.UseBackColor = true;
			this.gvMainTrade.Appearance.FooterPanel.Options.UseForeColor = true;
			this.gvMainTrade.Appearance.FooterPanel.Options.UseTextOptions = true;
			this.gvMainTrade.Appearance.FooterPanel.TextOptions.VAlignment = VertAlignment.Top;
			this.gvMainTrade.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(104, 184, 251);
			this.gvMainTrade.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(104, 184, 251);
			this.gvMainTrade.Appearance.GroupButton.Options.UseBackColor = true;
			this.gvMainTrade.Appearance.GroupButton.Options.UseBorderColor = true;
			this.gvMainTrade.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.gvMainTrade.Appearance.GroupFooter.BackColor2 = System.Drawing.Color.FromArgb(255, 255, 245);
			this.gvMainTrade.Appearance.GroupFooter.BorderColor = System.Drawing.Color.White;
			this.gvMainTrade.Appearance.GroupFooter.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.gvMainTrade.Appearance.GroupFooter.Options.UseBackColor = true;
			this.gvMainTrade.Appearance.GroupFooter.Options.UseBorderColor = true;
			this.gvMainTrade.Appearance.GroupFooter.Options.UseForeColor = true;
			this.gvMainTrade.Appearance.GroupFooter.Options.UseTextOptions = true;
			this.gvMainTrade.Appearance.GroupFooter.TextOptions.VAlignment = VertAlignment.Top;
			this.gvMainTrade.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(236, 246, 255);
			this.gvMainTrade.Appearance.GroupPanel.BackColor2 = System.Drawing.Color.White;
			this.gvMainTrade.Appearance.GroupPanel.ForeColor = System.Drawing.Color.Black;
			this.gvMainTrade.Appearance.GroupPanel.Options.UseBackColor = true;
			this.gvMainTrade.Appearance.GroupPanel.Options.UseForeColor = true;
			this.gvMainTrade.Appearance.GroupRow.BackColor = System.Drawing.Color.WhiteSmoke;
			this.gvMainTrade.Appearance.GroupRow.BackColor2 = System.Drawing.Color.Gainsboro;
			this.gvMainTrade.Appearance.GroupRow.BorderColor = System.Drawing.Color.White;
			this.gvMainTrade.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
			this.gvMainTrade.Appearance.GroupRow.Options.UseBackColor = true;
			this.gvMainTrade.Appearance.GroupRow.Options.UseBorderColor = true;
			this.gvMainTrade.Appearance.GroupRow.Options.UseForeColor = true;
			this.gvMainTrade.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.gvMainTrade.Appearance.HideSelectionRow.BackColor2 = System.Drawing.Color.FromArgb(224, 224, 224);
			this.gvMainTrade.Appearance.HideSelectionRow.BorderColor = System.Drawing.Color.Transparent;
			this.gvMainTrade.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.Black;
			this.gvMainTrade.Appearance.HideSelectionRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
			this.gvMainTrade.Appearance.HideSelectionRow.Options.UseBackColor = true;
			this.gvMainTrade.Appearance.HideSelectionRow.Options.UseBorderColor = true;
			this.gvMainTrade.Appearance.HideSelectionRow.Options.UseForeColor = true;
			this.gvMainTrade.Appearance.HorzLine.BackColor = System.Drawing.Color.White;
			this.gvMainTrade.Appearance.HorzLine.Options.UseBackColor = true;
			this.gvMainTrade.Appearance.OddRow.BackColor = System.Drawing.Color.Azure;
			this.gvMainTrade.Appearance.OddRow.BorderColor = System.Drawing.Color.White;
			this.gvMainTrade.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
			this.gvMainTrade.Appearance.OddRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
			this.gvMainTrade.Appearance.OddRow.Options.UseBackColor = true;
			this.gvMainTrade.Appearance.OddRow.Options.UseBorderColor = true;
			this.gvMainTrade.Appearance.OddRow.Options.UseForeColor = true;
			this.gvMainTrade.Appearance.Preview.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.gvMainTrade.Appearance.Preview.BackColor2 = System.Drawing.Color.FromArgb(255, 255, 245);
			this.gvMainTrade.Appearance.Preview.Font = new System.Drawing.Font("Verdana", 7.5f);
			this.gvMainTrade.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(83, 155, 215);
			this.gvMainTrade.Appearance.Preview.Options.UseBackColor = true;
			this.gvMainTrade.Appearance.Preview.Options.UseFont = true;
			this.gvMainTrade.Appearance.Preview.Options.UseForeColor = true;
			this.gvMainTrade.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.gvMainTrade.Appearance.Row.ForeColor = System.Drawing.Color.Black;
			this.gvMainTrade.Appearance.Row.Options.UseBackColor = true;
			this.gvMainTrade.Appearance.Row.Options.UseForeColor = true;
			this.gvMainTrade.Appearance.RowSeparator.BackColor = System.Drawing.Color.FromArgb(236, 246, 255);
			this.gvMainTrade.Appearance.RowSeparator.BackColor2 = System.Drawing.Color.White;
			this.gvMainTrade.Appearance.RowSeparator.ForeColor = System.Drawing.Color.Black;
			this.gvMainTrade.Appearance.RowSeparator.Options.UseBackColor = true;
			this.gvMainTrade.Appearance.RowSeparator.Options.UseForeColor = true;
			this.gvMainTrade.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.gvMainTrade.Appearance.SelectedRow.BackColor2 = System.Drawing.Color.FromArgb(224, 224, 224);
			this.gvMainTrade.Appearance.SelectedRow.ForeColor = System.Drawing.Color.Black;
			this.gvMainTrade.Appearance.SelectedRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
			this.gvMainTrade.Appearance.SelectedRow.Options.UseBackColor = true;
			this.gvMainTrade.Appearance.SelectedRow.Options.UseForeColor = true;
			this.gvMainTrade.Appearance.TopNewRow.BackColor = System.Drawing.Color.White;
			this.gvMainTrade.Appearance.TopNewRow.Options.UseBackColor = true;
			this.gvMainTrade.Appearance.VertLine.BackColor = System.Drawing.Color.White;
			this.gvMainTrade.Appearance.VertLine.Options.UseBackColor = true;
			this.gvMainTrade.BorderStyle = BorderStyles.Simple;
			this.gvMainTrade.Columns.AddRange(new GridColumn[]
			{
				this.gridColumn19,
				this.gridColumn20,
				this.gridColumn22,
				this.gridColumn27,
				this.gridColumn28,
				this.gridColumn29,
				this.gridColumn30,
				this.gridColumn31,
				this.gridColumn32,
				this.gridColumn33,
				this.gridColumn34,
				this.gridColumn35,
				this.gridColumn36,
				this.gridColumn37,
				this.gridColumn38,
				this.gridColumn39
			});
			this.gvMainTrade.FooterPanelHeight = 5;
			styleFormatCondition5.ApplyToRow = true;
			styleFormatCondition5.Column = this.gridColumn19;
			styleFormatCondition5.Condition = FormatConditionEnum.Equal;
			styleFormatCondition5.Value1 = true;
			this.gvMainTrade.FormatConditions.AddRange(new StyleFormatCondition[]
			{
				styleFormatCondition5
			});
			this.gvMainTrade.GridControl = this.xdgMianTrade;
			this.gvMainTrade.GroupFooterShowMode = GroupFooterShowMode.Hidden;
			this.gvMainTrade.HorzScrollVisibility = ScrollVisibility.Always;
			this.gvMainTrade.Name = "gvMainTrade";
			this.gvMainTrade.OptionsBehavior.AutoExpandAllGroups = true;
			this.gvMainTrade.OptionsLayout.StoreDataSettings = false;
			this.gvMainTrade.OptionsMenu.EnableColumnMenu = false;
			this.gvMainTrade.OptionsMenu.EnableFooterMenu = false;
			this.gvMainTrade.OptionsMenu.EnableGroupPanelMenu = false;
			this.gvMainTrade.OptionsNavigation.AutoFocusNewRow = true;
			this.gvMainTrade.OptionsPrint.AutoWidth = false;
			this.gvMainTrade.OptionsView.ColumnAutoWidth = false;
			this.gvMainTrade.OptionsView.EnableAppearanceEvenRow = true;
			this.gvMainTrade.OptionsView.EnableAppearanceOddRow = true;
			this.gvMainTrade.OptionsView.HeaderFilterButtonShowMode = FilterButtonShowMode.Button;
			this.gvMainTrade.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;
			this.gvMainTrade.OptionsView.ShowFooter = true;
			this.gvMainTrade.OptionsView.ShowGroupPanel = false;
			this.gvMainTrade.SortInfo.AddRange(new GridColumnSortInfo[]
			{
				new GridColumnSortInfo(this.gridColumn34, ColumnSortOrder.Descending)
			});
			this.gvMainTrade.VertScrollVisibility = ScrollVisibility.Always;
			this.gvMainTrade.RowCellStyle += new RowCellStyleEventHandler(this.gvMainTrade_RowCellStyle);
			this.gridColumn20.Caption = "编号";
			this.gridColumn20.FieldName = "编号";
			this.gridColumn20.Name = "gridColumn20";
			this.gridColumn20.OptionsColumn.AllowEdit = false;
			this.gridColumn20.OptionsFilter.AllowFilter = false;
			this.gridColumn20.Visible = true;
			this.gridColumn20.VisibleIndex = 1;
			this.gridColumn20.Width = 70;
			this.gridColumn22.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
			this.gridColumn22.AppearanceCell.Options.UseFont = true;
			this.gridColumn22.Caption = "合约";
			this.gridColumn22.FieldName = "合约";
			this.gridColumn22.Name = "gridColumn22";
			this.gridColumn22.OptionsColumn.AllowEdit = false;
			this.gridColumn22.Visible = true;
			this.gridColumn22.VisibleIndex = 2;
			this.gridColumn22.Width = 69;
			this.gridColumn27.Caption = "买卖";
			this.gridColumn27.FieldName = "买卖";
			this.gridColumn27.Name = "gridColumn27";
			this.gridColumn27.OptionsColumn.AllowEdit = false;
			this.gridColumn27.Visible = true;
			this.gridColumn27.VisibleIndex = 3;
			this.gridColumn27.Width = 71;
			this.gridColumn28.Caption = "开平";
			this.gridColumn28.FieldName = "开平";
			this.gridColumn28.Name = "gridColumn28";
			this.gridColumn28.OptionsColumn.AllowEdit = false;
			this.gridColumn28.Visible = true;
			this.gridColumn28.VisibleIndex = 4;
			this.gridColumn28.Width = 50;
			this.gridColumn29.Caption = "状态";
			this.gridColumn29.FieldName = "状态";
			this.gridColumn29.Name = "gridColumn29";
			this.gridColumn29.OptionsColumn.AllowEdit = false;
			this.gridColumn29.OptionsFilter.AllowFilter = false;
			this.gridColumn29.Visible = true;
			this.gridColumn29.VisibleIndex = 5;
			this.gridColumn30.Caption = "价格";
			this.gridColumn30.FieldName = "价格";
			this.gridColumn30.Name = "gridColumn30";
			this.gridColumn30.OptionsColumn.AllowEdit = false;
			this.gridColumn30.OptionsFilter.AllowFilter = false;
			this.gridColumn30.Visible = true;
			this.gridColumn30.VisibleIndex = 6;
			this.gridColumn31.Caption = "报单手数";
			this.gridColumn31.FieldName = "报单手数";
			this.gridColumn31.Name = "gridColumn31";
			this.gridColumn31.OptionsColumn.AllowEdit = false;
			this.gridColumn31.OptionsFilter.AllowFilter = false;
			this.gridColumn31.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Sum)
			});
			this.gridColumn31.Visible = true;
			this.gridColumn31.VisibleIndex = 7;
			this.gridColumn32.Caption = "未成交";
			this.gridColumn32.FieldName = "未成交";
			this.gridColumn32.Name = "gridColumn32";
			this.gridColumn32.OptionsColumn.AllowEdit = false;
			this.gridColumn32.OptionsFilter.AllowFilter = false;
			this.gridColumn32.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Sum)
			});
			this.gridColumn32.Visible = true;
			this.gridColumn32.VisibleIndex = 8;
			this.gridColumn33.Caption = "成交手数";
			this.gridColumn33.FieldName = "成交手数";
			this.gridColumn33.Name = "gridColumn33";
			this.gridColumn33.OptionsColumn.AllowEdit = false;
			this.gridColumn33.OptionsFilter.AllowFilter = false;
			this.gridColumn33.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Sum)
			});
			this.gridColumn33.Visible = true;
			this.gridColumn33.VisibleIndex = 9;
			this.gridColumn34.Caption = "报单时间";
			this.gridColumn34.DisplayFormat.FormatString = "HH:mm:ss";
			this.gridColumn34.DisplayFormat.FormatType = FormatType.DateTime;
			this.gridColumn34.FieldName = "报单时间";
			this.gridColumn34.Name = "gridColumn34";
			this.gridColumn34.OptionsColumn.AllowEdit = false;
			this.gridColumn34.OptionsFilter.AllowFilter = false;
			this.gridColumn34.SortMode = ColumnSortMode.Value;
			this.gridColumn34.Visible = true;
			this.gridColumn34.VisibleIndex = 10;
			this.gridColumn34.Width = 90;
			this.gridColumn35.Caption = "成交时间";
			this.gridColumn35.FieldName = "成交时间";
			this.gridColumn35.Name = "gridColumn35";
			this.gridColumn35.OptionsColumn.AllowEdit = false;
			this.gridColumn35.OptionsFilter.AllowFilter = false;
			this.gridColumn35.Visible = true;
			this.gridColumn35.VisibleIndex = 11;
			this.gridColumn35.Width = 110;
			this.gridColumn36.Caption = "成交均价";
			this.gridColumn36.FieldName = "成交均价";
			this.gridColumn36.Name = "gridColumn36";
			this.gridColumn36.OptionsColumn.AllowEdit = false;
			this.gridColumn36.OptionsFilter.AllowFilter = false;
			this.gridColumn36.Visible = true;
			this.gridColumn36.VisibleIndex = 12;
			this.gridColumn37.Caption = "详细状态";
			this.gridColumn37.FieldName = "详细状态";
			this.gridColumn37.Name = "gridColumn37";
			this.gridColumn37.OptionsColumn.AllowEdit = false;
			this.gridColumn37.OptionsFilter.AllowFilter = false;
			this.gridColumn37.Visible = true;
			this.gridColumn37.VisibleIndex = 13;
			this.gridColumn37.Width = 200;
			this.gridColumn38.Caption = "客户信息";
			this.gridColumn38.FieldName = "客户信息";
			this.gridColumn38.Name = "gridColumn38";
			this.gridColumn38.OptionsColumn.AllowEdit = false;
			this.gridColumn38.Visible = true;
			this.gridColumn38.VisibleIndex = 14;
			this.gridColumn38.Width = 106;
			this.gridColumn39.Caption = "序列号";
			this.gridColumn39.FieldName = "序列号";
			this.gridColumn39.Name = "gridColumn39";
			this.gridColumn39.OptionsColumn.AllowEdit = false;
			this.gridColumn39.OptionsFilter.AllowFilter = false;
			this.gridColumn39.Visible = true;
			this.gridColumn39.VisibleIndex = 15;
			this.xtraTabPage2.Controls.Add(this.xdgMainDeal);
			this.xtraTabPage2.Name = "xtraTabPage2";
			this.xtraTabPage2.Size = new System.Drawing.Size(107, 301);
			this.xtraTabPage2.Text = "成交记录";
			this.xdgMainDeal.ContextMenuStrip = this.contextMenuStrip3;
			this.xdgMainDeal.Dock = System.Windows.Forms.DockStyle.Fill;
			this.xdgMainDeal.Location = new System.Drawing.Point(0, 0);
			this.xdgMainDeal.MainView = this.gvMainDeal;
			this.xdgMainDeal.Name = "xdgMainDeal";
			this.xdgMainDeal.Size = new System.Drawing.Size(107, 301);
			this.xdgMainDeal.TabIndex = 19;
			this.xdgMainDeal.ViewCollection.AddRange(new BaseView[]
			{
				this.gvMainDeal
			});
			this.contextMenuStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
			{
				this.导出数据ToolStripMenuItem1
			});
			this.contextMenuStrip3.Name = "contextMenuStrip3";
			this.contextMenuStrip3.Size = new System.Drawing.Size(125, 26);
			this.导出数据ToolStripMenuItem1.Name = "导出数据ToolStripMenuItem1";
			this.导出数据ToolStripMenuItem1.Size = new System.Drawing.Size(124, 22);
			this.导出数据ToolStripMenuItem1.Text = "导出数据";
			this.导出数据ToolStripMenuItem1.Click += new System.EventHandler(this.导出数据ToolStripMenuItem1_Click);
			this.gvMainDeal.Appearance.ColumnFilterButton.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.gvMainDeal.Appearance.ColumnFilterButton.BackColor2 = System.Drawing.Color.LightSkyBlue;
			this.gvMainDeal.Appearance.ColumnFilterButton.BorderColor = System.Drawing.Color.White;
			this.gvMainDeal.Appearance.ColumnFilterButton.ForeColor = System.Drawing.Color.White;
			this.gvMainDeal.Appearance.ColumnFilterButton.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.gvMainDeal.Appearance.ColumnFilterButton.Options.UseBackColor = true;
			this.gvMainDeal.Appearance.ColumnFilterButton.Options.UseBorderColor = true;
			this.gvMainDeal.Appearance.ColumnFilterButton.Options.UseForeColor = true;
			this.gvMainDeal.Appearance.ColumnFilterButtonActive.BackColor = System.Drawing.Color.FromArgb(170, 216, 254);
			this.gvMainDeal.Appearance.ColumnFilterButtonActive.BorderColor = System.Drawing.Color.FromArgb(170, 216, 254);
			this.gvMainDeal.Appearance.ColumnFilterButtonActive.ForeColor = System.Drawing.Color.Black;
			this.gvMainDeal.Appearance.ColumnFilterButtonActive.Options.UseBackColor = true;
			this.gvMainDeal.Appearance.ColumnFilterButtonActive.Options.UseBorderColor = true;
			this.gvMainDeal.Appearance.ColumnFilterButtonActive.Options.UseForeColor = true;
			this.gvMainDeal.Appearance.Empty.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.gvMainDeal.Appearance.Empty.BackColor2 = System.Drawing.Color.FromArgb(224, 224, 224);
			this.gvMainDeal.Appearance.Empty.Options.UseBackColor = true;
			this.gvMainDeal.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.gvMainDeal.Appearance.EvenRow.BackColor2 = System.Drawing.Color.WhiteSmoke;
			this.gvMainDeal.Appearance.EvenRow.BorderColor = System.Drawing.Color.Transparent;
			this.gvMainDeal.Appearance.EvenRow.ForeColor = System.Drawing.Color.Black;
			this.gvMainDeal.Appearance.EvenRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
			this.gvMainDeal.Appearance.EvenRow.Options.UseBackColor = true;
			this.gvMainDeal.Appearance.EvenRow.Options.UseBorderColor = true;
			this.gvMainDeal.Appearance.EvenRow.Options.UseForeColor = true;
			this.gvMainDeal.Appearance.FilterCloseButton.BackColor = System.Drawing.Color.FromArgb(104, 184, 251);
			this.gvMainDeal.Appearance.FilterCloseButton.BorderColor = System.Drawing.Color.FromArgb(104, 184, 251);
			this.gvMainDeal.Appearance.FilterCloseButton.ForeColor = System.Drawing.Color.White;
			this.gvMainDeal.Appearance.FilterCloseButton.Options.UseBackColor = true;
			this.gvMainDeal.Appearance.FilterCloseButton.Options.UseBorderColor = true;
			this.gvMainDeal.Appearance.FilterCloseButton.Options.UseForeColor = true;
			this.gvMainDeal.Appearance.FilterPanel.BackColor = System.Drawing.Color.FromArgb(236, 246, 255);
			this.gvMainDeal.Appearance.FilterPanel.BackColor2 = System.Drawing.Color.White;
			this.gvMainDeal.Appearance.FilterPanel.ForeColor = System.Drawing.Color.Black;
			this.gvMainDeal.Appearance.FilterPanel.Options.UseBackColor = true;
			this.gvMainDeal.Appearance.FilterPanel.Options.UseForeColor = true;
			this.gvMainDeal.Appearance.FixedLine.BackColor = System.Drawing.Color.White;
			this.gvMainDeal.Appearance.FixedLine.Options.UseBackColor = true;
			this.gvMainDeal.Appearance.FocusedCell.BackColor = System.Drawing.Color.Transparent;
			this.gvMainDeal.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
			this.gvMainDeal.Appearance.FocusedCell.Options.UseBackColor = true;
			this.gvMainDeal.Appearance.FocusedCell.Options.UseForeColor = true;
			this.gvMainDeal.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.gvMainDeal.Appearance.FocusedRow.BackColor2 = System.Drawing.Color.FromArgb(224, 224, 224);
			this.gvMainDeal.Appearance.FocusedRow.BorderColor = System.Drawing.Color.White;
			this.gvMainDeal.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
			this.gvMainDeal.Appearance.FocusedRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
			this.gvMainDeal.Appearance.FocusedRow.Options.UseBackColor = true;
			this.gvMainDeal.Appearance.FocusedRow.Options.UseBorderColor = true;
			this.gvMainDeal.Appearance.FocusedRow.Options.UseForeColor = true;
			this.gvMainDeal.Appearance.FooterPanel.BackColor = System.Drawing.Color.LightCyan;
			this.gvMainDeal.Appearance.FooterPanel.BackColor2 = System.Drawing.Color.LightSkyBlue;
			this.gvMainDeal.Appearance.FooterPanel.BorderColor = System.Drawing.Color.Transparent;
			this.gvMainDeal.Appearance.FooterPanel.ForeColor = System.Drawing.Color.Black;
			this.gvMainDeal.Appearance.FooterPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.gvMainDeal.Appearance.FooterPanel.Options.UseBackColor = true;
			this.gvMainDeal.Appearance.FooterPanel.Options.UseForeColor = true;
			this.gvMainDeal.Appearance.FooterPanel.Options.UseTextOptions = true;
			this.gvMainDeal.Appearance.FooterPanel.TextOptions.VAlignment = VertAlignment.Top;
			this.gvMainDeal.Appearance.GroupButton.BackColor = System.Drawing.Color.FromArgb(104, 184, 251);
			this.gvMainDeal.Appearance.GroupButton.BorderColor = System.Drawing.Color.FromArgb(104, 184, 251);
			this.gvMainDeal.Appearance.GroupButton.Options.UseBackColor = true;
			this.gvMainDeal.Appearance.GroupButton.Options.UseBorderColor = true;
			this.gvMainDeal.Appearance.GroupFooter.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.gvMainDeal.Appearance.GroupFooter.BackColor2 = System.Drawing.Color.FromArgb(255, 255, 245);
			this.gvMainDeal.Appearance.GroupFooter.BorderColor = System.Drawing.Color.White;
			this.gvMainDeal.Appearance.GroupFooter.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.gvMainDeal.Appearance.GroupFooter.Options.UseBackColor = true;
			this.gvMainDeal.Appearance.GroupFooter.Options.UseBorderColor = true;
			this.gvMainDeal.Appearance.GroupFooter.Options.UseForeColor = true;
			this.gvMainDeal.Appearance.GroupFooter.Options.UseTextOptions = true;
			this.gvMainDeal.Appearance.GroupFooter.TextOptions.VAlignment = VertAlignment.Top;
			this.gvMainDeal.Appearance.GroupPanel.BackColor = System.Drawing.Color.FromArgb(236, 246, 255);
			this.gvMainDeal.Appearance.GroupPanel.BackColor2 = System.Drawing.Color.White;
			this.gvMainDeal.Appearance.GroupPanel.ForeColor = System.Drawing.Color.Black;
			this.gvMainDeal.Appearance.GroupPanel.Options.UseBackColor = true;
			this.gvMainDeal.Appearance.GroupPanel.Options.UseForeColor = true;
			this.gvMainDeal.Appearance.GroupRow.BackColor = System.Drawing.Color.WhiteSmoke;
			this.gvMainDeal.Appearance.GroupRow.BackColor2 = System.Drawing.Color.Gainsboro;
			this.gvMainDeal.Appearance.GroupRow.BorderColor = System.Drawing.Color.White;
			this.gvMainDeal.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
			this.gvMainDeal.Appearance.GroupRow.Options.UseBackColor = true;
			this.gvMainDeal.Appearance.GroupRow.Options.UseBorderColor = true;
			this.gvMainDeal.Appearance.GroupRow.Options.UseForeColor = true;
			this.gvMainDeal.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.gvMainDeal.Appearance.HideSelectionRow.BackColor2 = System.Drawing.Color.FromArgb(224, 224, 224);
			this.gvMainDeal.Appearance.HideSelectionRow.BorderColor = System.Drawing.Color.Transparent;
			this.gvMainDeal.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.Black;
			this.gvMainDeal.Appearance.HideSelectionRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
			this.gvMainDeal.Appearance.HideSelectionRow.Options.UseBackColor = true;
			this.gvMainDeal.Appearance.HideSelectionRow.Options.UseBorderColor = true;
			this.gvMainDeal.Appearance.HideSelectionRow.Options.UseForeColor = true;
			this.gvMainDeal.Appearance.HorzLine.BackColor = System.Drawing.Color.White;
			this.gvMainDeal.Appearance.HorzLine.Options.UseBackColor = true;
			this.gvMainDeal.Appearance.OddRow.BackColor = System.Drawing.Color.Azure;
			this.gvMainDeal.Appearance.OddRow.BorderColor = System.Drawing.Color.White;
			this.gvMainDeal.Appearance.OddRow.ForeColor = System.Drawing.Color.Black;
			this.gvMainDeal.Appearance.OddRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
			this.gvMainDeal.Appearance.OddRow.Options.UseBackColor = true;
			this.gvMainDeal.Appearance.OddRow.Options.UseBorderColor = true;
			this.gvMainDeal.Appearance.OddRow.Options.UseForeColor = true;
			this.gvMainDeal.Appearance.Preview.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.gvMainDeal.Appearance.Preview.BackColor2 = System.Drawing.Color.FromArgb(255, 255, 245);
			this.gvMainDeal.Appearance.Preview.Font = new System.Drawing.Font("Verdana", 7.5f);
			this.gvMainDeal.Appearance.Preview.ForeColor = System.Drawing.Color.FromArgb(83, 155, 215);
			this.gvMainDeal.Appearance.Preview.Options.UseBackColor = true;
			this.gvMainDeal.Appearance.Preview.Options.UseFont = true;
			this.gvMainDeal.Appearance.Preview.Options.UseForeColor = true;
			this.gvMainDeal.Appearance.Row.BackColor = System.Drawing.Color.FromArgb(255, 255, 245);
			this.gvMainDeal.Appearance.Row.ForeColor = System.Drawing.Color.Black;
			this.gvMainDeal.Appearance.Row.Options.UseBackColor = true;
			this.gvMainDeal.Appearance.Row.Options.UseForeColor = true;
			this.gvMainDeal.Appearance.RowSeparator.BackColor = System.Drawing.Color.FromArgb(236, 246, 255);
			this.gvMainDeal.Appearance.RowSeparator.BackColor2 = System.Drawing.Color.White;
			this.gvMainDeal.Appearance.RowSeparator.ForeColor = System.Drawing.Color.Black;
			this.gvMainDeal.Appearance.RowSeparator.Options.UseBackColor = true;
			this.gvMainDeal.Appearance.RowSeparator.Options.UseForeColor = true;
			this.gvMainDeal.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(181, 241, 231);
			this.gvMainDeal.Appearance.SelectedRow.BackColor2 = System.Drawing.Color.FromArgb(224, 224, 224);
			this.gvMainDeal.Appearance.SelectedRow.ForeColor = System.Drawing.Color.Black;
			this.gvMainDeal.Appearance.SelectedRow.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
			this.gvMainDeal.Appearance.SelectedRow.Options.UseBackColor = true;
			this.gvMainDeal.Appearance.SelectedRow.Options.UseForeColor = true;
			this.gvMainDeal.Appearance.TopNewRow.BackColor = System.Drawing.Color.White;
			this.gvMainDeal.Appearance.TopNewRow.Options.UseBackColor = true;
			this.gvMainDeal.Appearance.VertLine.BackColor = System.Drawing.Color.White;
			this.gvMainDeal.Appearance.VertLine.Options.UseBackColor = true;
			this.gvMainDeal.BorderStyle = BorderStyles.Simple;
			this.gvMainDeal.Columns.AddRange(new GridColumn[]
			{
				this.gridColumn40,
				this.gridColumn41,
				this.gridColumn42,
				this.gridColumn43,
				this.gridColumn44,
				this.gridColumn45,
				this.gridColumn46,
				this.gridColumn47,
				this.gridColumn48,
				this.gridColumn49,
				this.gridColumn50
			});
			this.gvMainDeal.FooterPanelHeight = 5;
			styleFormatCondition6.ApplyToRow = true;
			styleFormatCondition6.Column = this.gridColumn40;
			styleFormatCondition6.Condition = FormatConditionEnum.Equal;
			styleFormatCondition6.Value1 = true;
			this.gvMainDeal.FormatConditions.AddRange(new StyleFormatCondition[]
			{
				styleFormatCondition6
			});
			this.gvMainDeal.GridControl = this.xdgMainDeal;
			this.gvMainDeal.GroupFooterShowMode = GroupFooterShowMode.Hidden;
			this.gvMainDeal.HorzScrollVisibility = ScrollVisibility.Always;
			this.gvMainDeal.Name = "gvMainDeal";
			this.gvMainDeal.OptionsBehavior.AutoExpandAllGroups = true;
			this.gvMainDeal.OptionsMenu.EnableColumnMenu = false;
			this.gvMainDeal.OptionsMenu.EnableFooterMenu = false;
			this.gvMainDeal.OptionsMenu.EnableGroupPanelMenu = false;
			this.gvMainDeal.OptionsPrint.AutoWidth = false;
			this.gvMainDeal.OptionsView.ColumnAutoWidth = false;
			this.gvMainDeal.OptionsView.EnableAppearanceEvenRow = true;
			this.gvMainDeal.OptionsView.EnableAppearanceOddRow = true;
			this.gvMainDeal.OptionsView.HeaderFilterButtonShowMode = FilterButtonShowMode.Button;
			this.gvMainDeal.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;
			this.gvMainDeal.OptionsView.ShowFooter = true;
			this.gvMainDeal.OptionsView.ShowGroupPanel = false;
			this.gvMainDeal.SortInfo.AddRange(new GridColumnSortInfo[]
			{
				new GridColumnSortInfo(this.gridColumn47, ColumnSortOrder.Descending)
			});
			this.gvMainDeal.VertScrollVisibility = ScrollVisibility.Always;
			this.gvMainDeal.RowCellStyle += new RowCellStyleEventHandler(this.gvMainDeal_RowCellStyle);
			this.gridColumn41.Caption = "编号";
			this.gridColumn41.FieldName = "编号";
			this.gridColumn41.Name = "gridColumn41";
			this.gridColumn41.OptionsColumn.AllowEdit = false;
			this.gridColumn41.OptionsFilter.AllowFilter = false;
			this.gridColumn41.Visible = true;
			this.gridColumn41.VisibleIndex = 1;
			this.gridColumn41.Width = 79;
			this.gridColumn42.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
			this.gridColumn42.AppearanceCell.Options.UseFont = true;
			this.gridColumn42.Caption = "合约";
			this.gridColumn42.FieldName = "合约";
			this.gridColumn42.Name = "gridColumn42";
			this.gridColumn42.OptionsColumn.AllowEdit = false;
			this.gridColumn42.Visible = true;
			this.gridColumn42.VisibleIndex = 2;
			this.gridColumn42.Width = 79;
			this.gridColumn43.Caption = "买卖";
			this.gridColumn43.FieldName = "买卖";
			this.gridColumn43.Name = "gridColumn43";
			this.gridColumn43.OptionsColumn.AllowEdit = false;
			this.gridColumn43.Visible = true;
			this.gridColumn43.VisibleIndex = 3;
			this.gridColumn43.Width = 71;
			this.gridColumn44.Caption = "开平";
			this.gridColumn44.FieldName = "开平";
			this.gridColumn44.Name = "gridColumn44";
			this.gridColumn44.OptionsColumn.AllowEdit = false;
			this.gridColumn44.Visible = true;
			this.gridColumn44.VisibleIndex = 4;
			this.gridColumn44.Width = 55;
			this.gridColumn45.Caption = "成交价格";
			this.gridColumn45.FieldName = "成交价格";
			this.gridColumn45.Name = "gridColumn45";
			this.gridColumn45.OptionsColumn.AllowEdit = false;
			this.gridColumn45.OptionsFilter.AllowFilter = false;
			this.gridColumn45.Visible = true;
			this.gridColumn45.VisibleIndex = 5;
			this.gridColumn46.Caption = "成交手数";
			this.gridColumn46.FieldName = "成交手数";
			this.gridColumn46.Name = "gridColumn46";
			this.gridColumn46.OptionsColumn.AllowEdit = false;
			this.gridColumn46.OptionsFilter.AllowFilter = false;
			this.gridColumn46.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem(SummaryItemType.Sum)
			});
			this.gridColumn46.Visible = true;
			this.gridColumn46.VisibleIndex = 6;
			this.gridColumn47.Caption = "成交时间";
			this.gridColumn47.FieldName = "成交时间";
			this.gridColumn47.Name = "gridColumn47";
			this.gridColumn47.OptionsColumn.AllowEdit = false;
			this.gridColumn47.OptionsFilter.AllowFilter = false;
			this.gridColumn47.Visible = true;
			this.gridColumn47.VisibleIndex = 7;
			this.gridColumn47.Width = 110;
			this.gridColumn48.Caption = "报单编号";
			this.gridColumn48.FieldName = "报单编号";
			this.gridColumn48.Name = "gridColumn48";
			this.gridColumn48.OptionsColumn.AllowEdit = false;
			this.gridColumn48.OptionsFilter.AllowFilter = false;
			this.gridColumn48.Visible = true;
			this.gridColumn48.VisibleIndex = 8;
			this.gridColumn48.Width = 83;
			this.gridColumn49.Caption = "成交类型";
			this.gridColumn49.FieldName = "成交类型";
			this.gridColumn49.Name = "gridColumn49";
			this.gridColumn49.OptionsColumn.AllowEdit = false;
			this.gridColumn49.OptionsFilter.AllowFilter = false;
			this.gridColumn49.Visible = true;
			this.gridColumn49.VisibleIndex = 9;
			this.gridColumn49.Width = 63;
			this.gridColumn50.Caption = "交易所";
			this.gridColumn50.FieldName = "交易所";
			this.gridColumn50.Name = "gridColumn50";
			this.gridColumn50.OptionsColumn.AllowEdit = false;
			this.gridColumn50.OptionsFilter.AllowFilter = false;
			this.gridColumn50.Visible = true;
			this.gridColumn50.VisibleIndex = 10;
			this.gridColumn50.Width = 69;
			this.splitContainerControl2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainerControl2.Horizontal = false;
			this.splitContainerControl2.Location = new System.Drawing.Point(0, 0);
			this.splitContainerControl2.LookAndFeel.Style = LookAndFeelStyle.Flat;
			this.splitContainerControl2.LookAndFeel.UseDefaultLookAndFeel = false;
			this.splitContainerControl2.Name = "splitContainerControl2";
			this.splitContainerControl2.Panel1.Controls.Add(this.spcAccount);
			this.splitContainerControl2.Panel1.Text = "Panel1";
			this.splitContainerControl2.Panel2.Controls.Add(this.spcPosition);
			this.splitContainerControl2.Panel2.Text = "Panel2";
			this.splitContainerControl2.Size = new System.Drawing.Size(1044, 473);
			this.splitContainerControl2.SplitterPosition = 176;
			this.splitContainerControl2.TabIndex = 0;
			this.splitContainerControl2.Text = "splitContainerControl2";
			this.radioButton1.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.radioButton1.AutoSize = true;
			this.radioButton1.BackColor = System.Drawing.Color.Transparent;
			this.radioButton1.Enabled = false;
			this.radioButton1.Location = new System.Drawing.Point(850, 10);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.Size = new System.Drawing.Size(47, 16);
			this.radioButton1.TabIndex = 2;
			this.radioButton1.Text = "委托";
			this.radioButton1.UseVisualStyleBackColor = false;
			this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
			this.radioButton2.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.radioButton2.AutoSize = true;
			this.radioButton2.BackColor = System.Drawing.Color.Transparent;
			this.radioButton2.Location = new System.Drawing.Point(903, 9);
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.Size = new System.Drawing.Size(47, 16);
			this.radioButton2.TabIndex = 3;
			this.radioButton2.Text = "成交";
			this.radioButton2.UseVisualStyleBackColor = false;
			this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
			this.textBox1.Location = new System.Drawing.Point(624, 9);
			this.textBox1.Name = "textBox1";
			this.textBox1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.textBox1.Size = new System.Drawing.Size(32, 21);
			this.textBox1.TabIndex = 20;
			this.textBox1.Text = "3";
			this.textBox1.Visible = false;
			this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Location = new System.Drawing.Point(487, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(131, 12);
			this.label1.TabIndex = 19;
			this.label1.Text = "校对延时时间单位(秒):";
			this.label1.Visible = false;
			this.label2.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.label2.AutoSize = true;
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.Location = new System.Drawing.Point(809, 12);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(35, 12);
			this.label2.TabIndex = 4;
			this.label2.Text = "模式:";
			this.linkLabel1.Anchor = (System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.linkLabel1.AutoSize = true;
			this.linkLabel1.BackColor = System.Drawing.SystemColors.ActiveBorder;
			this.linkLabel1.LinkColor = System.Drawing.Color.Red;
			this.linkLabel1.Location = new System.Drawing.Point(956, 13);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(77, 12);
			this.linkLabel1.TabIndex = 23;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "免责条款声明";
			this.linkLabel1.Visible = false;
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			this.button6.ForeColor = System.Drawing.Color.FromArgb(192, 0, 0);
			this.button6.Location = new System.Drawing.Point(190, 101);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(71, 31);
			this.button6.TabIndex = 81;
			this.button6.Text = "平今卖";
			this.toolTip1.SetToolTip(this.button6, "以涨跌停下单");
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Click += new System.EventHandler(this.button6_Click);
			this.button7.ForeColor = System.Drawing.Color.FromArgb(0, 192, 0);
			this.button7.Location = new System.Drawing.Point(272, 101);
			this.button7.Name = "button7";
			this.button7.Size = new System.Drawing.Size(71, 31);
			this.button7.TabIndex = 82;
			this.button7.Text = "平今买";
			this.toolTip1.SetToolTip(this.button7, "以涨跌停下单");
			this.button7.UseVisualStyleBackColor = true;
			this.button7.Click += new System.EventHandler(this.button7_Click);
			this.AllowDrop = true;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(248, 248, 248);
			base.ClientSize = new System.Drawing.Size(1045, 874);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.radioButton2);
			base.Controls.Add(this.radioButton1);
			base.Controls.Add(this.linkLabel1);
			base.Controls.Add(this.textBox1);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.spcMain);
			base.Controls.Add(this.comboBoxErrMsg);
			base.Controls.Add(this.radioButtonMd);
			base.Controls.Add(this.menuStrip1);
			this.DoubleBuffered = true;
			base.Icon = (System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
			base.KeyPreview = true;
			base.MainMenuStrip = this.menuStrip1;
			base.Name = "frmMainFrame";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "壹玖交易系统V8";
			base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			base.Activated += new System.EventHandler(this.frmMainFrame_Activated);
			base.Deactivate += new System.EventHandler(this.frmMainFrame_Deactivate);
			base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMainFrame_FormClosing);
			base.Load += new System.EventHandler(this.frmMainFrame_Load);
			base.Resize += new System.EventHandler(this.frmMainFrame_Resize);
			((ISupportInitialize)this.repositoryItemCheckEdit3).EndInit();
			((ISupportInitialize)this.repositoryItemSpinEdit1).EndInit();
			this.panel1.ResumeLayout(false);
			this.contextMenuStrip1.ResumeLayout(false);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			((ISupportInitialize)this.xdgMainAccount).EndInit();
			((ISupportInitialize)this.bgcMainAccount).EndInit();
			((ISupportInitialize)this.xdgAccount).EndInit();
			((ISupportInitialize)this.bgvAccount).EndInit();
			((ISupportInitialize)this.numericUpDownPrice).EndInit();
			((ISupportInitialize)this.numericUpDownVolume).EndInit();
			((ISupportInitialize)this.spcAccount).EndInit();
			this.spcAccount.ResumeLayout(false);
			((ISupportInitialize)this.comboBoxOffset.Properties).EndInit();
			((ISupportInitialize)this.comboBoxDirector.Properties).EndInit();
			((ISupportInitialize)this.comboBoxInstrument.Properties).EndInit();
			((ISupportInitialize)this.spcPosition).EndInit();
			this.spcPosition.ResumeLayout(false);
			((ISupportInitialize)this.xtraTabControl1).EndInit();
			this.xtraTabControl1.ResumeLayout(false);
			this.xtabTrade.ResumeLayout(false);
			this.xtabTrade.PerformLayout();
			((ISupportInitialize)this.xdgTrade).EndInit();
			((ISupportInitialize)this.gvTrade).EndInit();
			this.xtabViewTrade.ResumeLayout(false);
			((ISupportInitialize)this.xdgDeal).EndInit();
			this.contextMenuStrip4.ResumeLayout(false);
			((ISupportInitialize)this.gvDeal).EndInit();
			((ISupportInitialize)this.spcMain).EndInit();
			this.spcMain.ResumeLayout(false);
			((ISupportInitialize)this.spcMainAccount).EndInit();
			this.spcMainAccount.ResumeLayout(false);
			((ISupportInitialize)this.spcMainPosition).EndInit();
			this.spcMainPosition.ResumeLayout(false);
			((ISupportInitialize)this.xtraTabControl2).EndInit();
			this.xtraTabControl2.ResumeLayout(false);
			this.xtraTabPage1.ResumeLayout(false);
			this.xtraTabPage1.PerformLayout();
			((ISupportInitialize)this.xdgMianTrade).EndInit();
			this.contextMenuStrip2.ResumeLayout(false);
			((ISupportInitialize)this.gvMainTrade).EndInit();
			this.xtraTabPage2.ResumeLayout(false);
			((ISupportInitialize)this.xdgMainDeal).EndInit();
			this.contextMenuStrip3.ResumeLayout(false);
			((ISupportInitialize)this.gvMainDeal).EndInit();
			((ISupportInitialize)this.splitContainerControl2).EndInit();
			this.splitContainerControl2.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
