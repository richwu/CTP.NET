using System.Collections.Generic;
using System.Windows.Forms;
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
    }
}