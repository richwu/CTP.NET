using System.Collections.Generic;

namespace WinCtp
{
    public class Rsp
    {
        public static Rsp This { get; }

        static Rsp()
        {
            This = new Rsp();
        }

        private readonly IDictionary<int, string> _dirRsp;

        private Rsp()
        {
            _dirRsp = new Dictionary<int, string>()
            {
                {  0,"发送成功" },
                { -1,"因网络原因发送失败" },
                { -2,"未处理请求队列总数量超限" },
                { -3,"每秒发送请求数量超限" }
            };
        }

        public string this[int rsp]
        {
            get
            {
                if (rsp > 0 || rsp < -3)
                    return null;
                return _dirRsp[rsp];
            }
        }
    }
}