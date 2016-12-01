
namespace WinCtp
{
    /// <summary>
    /// 键值对接口。
    /// </summary>
    public interface ILookupObject
    {
        /// <summary>
        /// 实际值。
        /// </summary>
        object Value { get; }

        /// <summary>
        /// 显示文本。
        /// </summary>
        string Display { get; }

        /// <summary>
        /// 显示顺序。
        /// </summary>
        int Sn { get; }
    }

    public class LookupObject : ILookupObject
    {
        public LookupObject(object value, string display)
            : this(value, display, 0)
        {
        }

        public LookupObject(object value, string display, int sn)
        {
            Value = value;
            Display = display;
            Sn = sn;
        }

        /// <summary>
        /// 实际值。
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// 显示文本。
        /// </summary>
        public string Display { get; }

        /// <summary>
        /// 显示顺序。
        /// </summary>
        public int Sn { get;  }
    }

}