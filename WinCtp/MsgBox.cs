using System.Windows.Forms;

namespace WinCtp
{
    /// <summary>
    /// 通用消息框。
    /// </summary>
    public static class MsgBox
    {
        /// <summary>
        /// 弹出提示消息框。
        /// </summary>
        /// <param name="msg">提示消息。</param>
        public static void Info(string msg)
        {
            MessageBox.Show(msg, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 弹出错误消息框。
        /// </summary>
        /// <param name="msg">错误消息。</param>
        public static void Error(string msg)
        {
            MessageBox.Show(msg, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 弹出问题消息框。
        /// </summary>
        /// <param name="msg">问题消息。</param>
        /// <returns><c>true</c>用户选择YES，<c>false</c>用户选择NO。</returns>
        public static bool Ask(string msg)
        {
            return MessageBox.Show(msg, "问题", MessageBoxButtons.YesNo, MessageBoxIcon.Question,MessageBoxDefaultButton.Button2) == DialogResult.Yes;
        }

        /// <summary>
        /// 弹出问题消息框。
        /// </summary>
        /// <param name="format">问题消息。</param>
        /// <param name="args">格式化参数值。</param>
        /// <returns><c>true</c>用户选择YES，<c>false</c>用户选择NO。</returns>
        public static bool Ask(string format, params object[] args)
        {
            return MessageBox.Show(string.Format(format, args), "问题", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
        }
    }
}