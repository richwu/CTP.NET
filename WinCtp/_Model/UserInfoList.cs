using System.Collections.Generic;

namespace WinCtp
{
    public class UserInfoList
    {
        public static UserInfoList Load()
        {
            return new UserInfoList(UserInfo.GetAll());
        }

        private readonly IList<UserInfo> _users;

        public UserInfoList(IList<UserInfo> users)
        {
            _users = users;
        }

        public IList<CtpMstUser> GetMst()
        {
            IList<CtpMstUser> arr = new List<CtpMstUser>();
            if (_users == null || _users.Count == 0)
                return arr;
            var id = 1000;
            foreach (var u in _users)
            {
                if (!u.IsSub)
                    continue;
                var obj = new CtpMstUser();
                obj.UserId = u.UserId;
                obj.UserName = u.UserName;
                obj.BrokerId = u.BrokerId;
                obj.Password = u.Password;
                obj.ReqId = id;
                arr.Add(obj);
                id++;
            }
            return arr;
        }

        public IList<CtpSubUser> GetSub()
        {
            IList<CtpSubUser> arr = new List<CtpSubUser>();
            if (_users == null || _users.Count == 0)
                return arr;
            var id = 2000;
            foreach (var u in _users)
            {
                if (u.IsSub)
                    continue;
                var obj = new CtpSubUser();
                obj.UserId = u.UserId;
                obj.UserName = u.UserName;
                obj.BrokerId = u.BrokerId;
                obj.Password = u.Password;
                obj.ReqId = id;
                obj.LoadConfig();
                arr.Add(obj);
                id++;
            }
            return arr;
        }
    }
}