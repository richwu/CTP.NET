using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace WinCtp
{
    /// <summary>
    /// DataGridView.
    /// </summary>
    public class DataGridViewEx : DataGridView
    {
        public DataGridViewEx()
        {
            RowPostPaint += (sender, e) =>
            {
                var rectangle = new Rectangle(e.RowBounds.Location.X,
                                                    e.RowBounds.Location.Y,
                                                    RowHeadersWidth - 4,
                                                    e.RowBounds.Height);
                TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(CultureInfo.InvariantCulture),
                                      RowHeadersDefaultCellStyle.Font, rectangle,
                                      RowHeadersDefaultCellStyle.ForeColor,
                                      TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
            };
            BackgroundColor = SystemColors.Control;
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        protected override void OnDataError(bool displayErrorDialogIfNoHandler, DataGridViewDataErrorEventArgs e)
        {
        }
    }
}