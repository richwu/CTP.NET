using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WinCtp
{
    /// <summary>
    /// 有关控件的一些帮助方法。
    /// </summary>
    public static class ControlUtil
    {
        public static void SetStatus(this ToolStrip toolbar, string tag)
        {
            var theTag = (tag ?? string.Empty).Trim().ToLower();
            foreach (ToolStripItem item in toolbar.Items)
            {
                var itemTag = (item.Tag ?? string.Empty).ToString().Trim().ToLower();
                item.Enabled = string.IsNullOrWhiteSpace(itemTag) || itemTag.Contains(theTag);
            }
        }

        /// <summary>
        /// 设置控件可编辑性
        /// </summary>
        /// <param name="container">控件容器</param>
        /// <param name="editable">可编辑性</param>
        /// <param name="flag">控件tag标识</param>
        public static void SetEditable(this Control container, bool editable, string flag = null)
        {
            var fg = (flag ?? string.Empty).Trim().ToLower();
            foreach (Control c in container.Controls)
            {
                var tag = (c.Tag ?? string.Empty).ToString().Trim().ToLower();
                if (tag.Length > 0 && (fg == string.Empty || tag.Contains(fg)))
                {
                    if (c is TextBoxBase)
                        ((TextBoxBase)c).ReadOnly = !editable;
                    else if (c is UpDownBase)
                        ((UpDownBase)c).ReadOnly = !editable;
                    else if(c is ToolStrip)
                        SetEditable((ToolStrip)c, editable);
                    else if (c is ComboBox ||
                        c is DateTimePicker ||
                        c is CheckBox ||
                        c is CheckedListBox)
                        c.Enabled = editable;
                }
                else
                {
                    SetEditable(c, editable,flag);
                }
                // ReSharper restore CanBeReplacedWithTryCastAndCheckForNull
            }
        }
        
        private static void SetEditable(ToolStrip ts, bool editable)
        {
            foreach (ToolStripItem item in ts.Items)
            {
                var tag = (item.Tag ?? string.Empty).ToString().ToLower();
                if(!tag.Contains("edit"))
                    continue;
                item.Enabled = editable;
            }
        }

        /// <summary>
        /// 只显示给定Tab页。
        /// </summary>
        /// <param name="tc"></param>
        /// <param name="tp">要显示的Tab页。</param>
        public static void ShowPage(this TabControl tc,TabPage tp)
        {
            if (tc == null || tc.TabPages.Count == 0)
                return;
            foreach (TabPage p in tc.TabPages)
            {
                p.Parent = null;
            }
            tp.Parent = tc;
        }

        public static void Bind(this ComboBox cmb, IEnumerable<ILookupObject> ds, bool includeEmpty = false)
        {
            if (ds == null)
            {
                cmb.DataSource = null;
                return;
            }
            var list = new List<LookupObject>();
            if (includeEmpty)
            {
                list.Add(new LookupObject(null, String.Empty, Int32.MinValue));
            }
            list.AddRange(ds.Select(item => new LookupObject(item.Value, item.Display)));
            cmb.ValueMember = "Value";
            cmb.DisplayMember = "Display";
            cmb.DataSource = list.OrderBy(o => o.Sn).ToList();
        }

        public static void Bind(this DataGridViewComboBoxColumn cmb, IEnumerable<ILookupObject> ds)
        {
            if (ds == null)
            {
                cmb.DataSource = null;
                return;
            }
            var list = new List<LookupObject>();
            list.AddRange(ds.Select(item => new LookupObject(item.Value, item.Display)));
            cmb.ValueMember = "Value";
            cmb.DisplayMember = "Display";
            cmb.DataSource = list.OrderBy(o => o.Sn).ToList();
        }
    }
}