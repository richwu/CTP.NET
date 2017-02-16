using System.Collections.Generic;
using System.Windows.Forms;
using Amib.Threading;
using GalaxyFutures.Sfit.Api;

namespace WinCtp
{
    public class MainViewImpl
    {
        private IMainView _view;

        private readonly IList<LookupObject> _luPosiDirection;

        public MainViewImpl(IMainView view)
        {
            _view = view;

            _luPosiDirection = new List<LookupObject>
            {
                new LookupObject(CtpPosiDirectionType.Net, "净"),//1
                new LookupObject(CtpPosiDirectionType.Long, "多头"),//2
                new LookupObject(CtpPosiDirectionType.Short, "空头")//3
            };
        }

        /// <summary>
        /// 获取买卖
        /// </summary>
        /// <returns></returns>
        internal IList<LookupObject> GetDirectionLookup()
        {
            return new List<LookupObject>
            {
                new LookupObject(CtpDirectionType.Buy, "买"),
                new LookupObject(CtpDirectionType.Sell, "卖")
            };
        }

        /// <summary>
        /// 报单状态
        /// </summary>
        /// <returns></returns>
        internal IList<LookupObject> GetOrderStatusLookup()
        {
            return new List<LookupObject>
            {
                new LookupObject(CtpOrderStatusType.Unknown, "未知"),//表示Thost已经接受用户的委托指令，还没有转发到交易所
                new LookupObject(CtpOrderStatusType.AllTraded, "全部成交"),
                new LookupObject(CtpOrderStatusType.Canceled, "撤单"),
                new LookupObject(CtpOrderStatusType.NoTradeNotQueueing, "未成交O队"),//未成交不在队列中
                new LookupObject(CtpOrderStatusType.NoTradeQueueing, "未成交I队"),//未成交还在队列中
                new LookupObject(CtpOrderStatusType.NotTouched, "未触发"),
                new LookupObject(CtpOrderStatusType.PartTradedNotQueueing, "部分成交O队"),//部分成交不在队列中
                new LookupObject(CtpOrderStatusType.PartTradedQueueing, "部分成交I队"),//部分成交还在队列中
                new LookupObject(CtpOrderStatusType.Touched, "已触发")
            };
        }

        /// <summary>
        /// 获取组合开平标志
        /// </summary>
        /// <returns></returns>
        internal IList<LookupObject> GetCombOffsetFlagLookup()
        {
            return new List<LookupObject>
            {
                new LookupObject(((char)CtpOffsetFlagType.Open).ToString(), "开仓"),
                new LookupObject(((char)CtpOffsetFlagType.Close).ToString(), "平仓"),
                new LookupObject(((char)CtpOffsetFlagType.ForceClose).ToString(), "强平"),
                new LookupObject(((char)CtpOffsetFlagType.CloseToday).ToString(), "平今"),
                new LookupObject(((char)CtpOffsetFlagType.CloseYesterday).ToString(), "平昨"),
                new LookupObject(((char)CtpOffsetFlagType.ForceOff).ToString(), "强减"),
                new LookupObject(((char)CtpOffsetFlagType.LocalForceClose).ToString(), "本地强平")
            };
        }

        /// <summary>
        /// 获取开平标志
        /// </summary>
        /// <returns></returns>
        internal IList<LookupObject> GetOffsetFlagLookup()
        {
            return new List<LookupObject>
            {
                new LookupObject(CtpOffsetFlagType.Open, "开仓"),//0
                new LookupObject(CtpOffsetFlagType.Close, "平仓"),//1
                new LookupObject(CtpOffsetFlagType.ForceClose, "强平"),//2
                new LookupObject(CtpOffsetFlagType.CloseToday, "平今"),//3
                new LookupObject(CtpOffsetFlagType.CloseYesterday, "平昨"),//4
                new LookupObject(CtpOffsetFlagType.ForceOff, "强减"),//5
                new LookupObject(CtpOffsetFlagType.LocalForceClose, "本地强平")//6
            };
        }

        internal DataGridViewEx CreatePositionView(string userId)
        {
            var gv = new DataGridViewEx();
            gv.AllowUserToAddRows = false;
            gv.AllowUserToDeleteRows = false;
            gv.ReadOnly = true;
            gv.Visible = true;
            gv.Name = $"gvp{userId}";
            gv.Dock = DockStyle.Fill;
            //
            var gcInvestorId = new DataGridViewTextBoxColumn
            {
                Name = $"gcInvestorId{userId}",
                HeaderText = "投资者",
                DataPropertyName = "InvestorId",
                ReadOnly = true,
                Width = 78
            };
            var gcInstrumentId = new DataGridViewTextBoxColumn
            {
                Name = $"gcInstrumentId{userId}",
                HeaderText = "合约",
                DataPropertyName = "InstrumentId",
                ReadOnly = true,
                Width = 78
            };
            var gcOpenVolume = new DataGridViewTextBoxColumn
            {
                Name = $"gcOpenVolume{userId}",
                HeaderText = "开仓量",
                DataPropertyName = "OpenVolume",
                ReadOnly = true,
                Width = 78
            };
            var gcPositionCost = new DataGridViewTextBoxColumn
            {
                Name = $"gcPositionCost{userId}",
                HeaderText = "持仓成本",
                DataPropertyName = "PositionCost",
                ReadOnly = true,
                Width = 78
            };
            var gcPositionProfit = new DataGridViewTextBoxColumn
            {
                Name = $"gcPositionProfit{userId}",
                HeaderText = "持仓盈亏",
                DataPropertyName = "PositionProfit",
                ReadOnly = true,
                Width = 78
            };
            var gcPosiDirection = new DataGridViewComboBoxColumn()
            {
                Name = $"gcPosiDirection{userId}",
                HeaderText = "持仓多空方向",
                DataPropertyName = "PosiDirection",
                ReadOnly = true,
                Width = 78,
                DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing
            };
            gcPosiDirection.Bind(_luPosiDirection);
            gv.Columns.AddRange(gcInvestorId, gcInstrumentId, gcOpenVolume, gcPositionCost, gcPositionProfit, gcPosiDirection);
            return gv;
        }

        internal IWorkItemsGroup InitThreedPool()
        {
            var stp = new STPStartInfo();//线程详细配置参数
            stp.CallToPostExecute = CallToPostExecute.Always;//工作项执行完成后是否调用回调方法
            stp.DisposeOfStateObjects = true;//当工作项执行完成后,是否释放工作项的参数,如果释放,参数对象必须实现IDisposable接口
            //当线程池中没有工作项时,闲置的线程等待时间,超过这个时间后,会释放掉这个闲置的线程,默认为60秒
            stp.IdleTimeout = 30;//30s
                                 //最大线程数,默认为25,
                                 //注意,由于windows的机制,所以一般最大线程最大设置成25,
                                 //如果设置成0的话,那么线程池将停止运行
            stp.MaxWorkerThreads = 10;//15 thread
                                      //只在STP执行Action<...>与Func<...>两种任务时有效
                                      //在执行工作项的过程中,是否把参数传递到WorkItem中去,用做IWorkItemResult接口取State时使用,
                                      //如果设置为false那么IWorkItemResult.State是取不到值的
                                      //如果设置为true可以取到传入参数的数组
            stp.FillStateWithArgs = true;
            //最小线程数,默认为0,当没有工作项时,线程池最多剩余的线程数
            stp.MinWorkerThreads = 5;//5 thread
                                     //当工作项执行完毕后,默认的回调方法
            stp.PostExecuteWorkItemCallback = delegate (IWorkItemResult wir) { };
            //是否需要等待start方法后再执行工作项,?默认为true,当true状态时,STP必须执行Start方法,才会为线程分配工作项
            stp.StartSuspended = false;
            return new SmartThreadPool(stp);
        }
    }
}