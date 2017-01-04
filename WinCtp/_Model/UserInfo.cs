using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using GalaxyFutures.Sfit.Api;

namespace WinCtp
{
    /// <summary>
    /// 账户信息。
    /// </summary>
    public class UserInfo
    {
        public static IList<UserInfo> GetAll()
        {
            var arr = new List<UserInfo>();
            DataTable table;
            using (var con = SQLiteHelper.NewConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();
                var sql = new SQLiteHelper(cmd);
                table = sql.Select("select UserID,UserName,Password,BrokerID,IsSub from CtpUser");
                con.Close();
            }
            foreach (DataRow r in table.Rows)
            {
                var a = new UserInfo();
                a.UserId = r["UserID"].ToString();
                a.UserName = r["UserName"].ToString();
                a.Password = r["Password"].ToString();
                a.BrokerId = r["BrokerID"].ToString();
                a.IsSub = Convert.ToBoolean(r["IsSub"]);
                arr.Add(a);
            }
            return arr;
        }

        /// <summary>
        /// 账户ID。
        /// </summary>
        public string UserId { get; set; }

        public string UserName { get; set; }

        /// <summary>
        /// 账户密码。
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 期货公司。
        /// </summary>
        public string BrokerId { get; set; }

        /// <summary>
        /// 是否子账户。
        /// </summary>
        public bool IsSub { get; set; }

        public bool Exists()
        {
            object o;
            using (var con = SQLiteHelper.NewConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();
                var sql = new SQLiteHelper(cmd);
                o = sql.ExecuteScalar("SELECT UserID from CtpUser WHERE UserID=@vuserid",
                    new Dictionary<string, object> { { "@vuserid", UserId } });
                con.Close();
            }
            return o != null;
        }

        public void Save()
        {
            using (var con = SQLiteHelper.NewConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();
                var sql = new SQLiteHelper(cmd);
                var ps = new Dictionary<string, object>();
                ps["UserID"] = UserId;
                ps["UserName"] = UserName;
                ps["Password"] = Password;
                ps["BrokerID"] = BrokerId;
                ps["IsSub"] = IsSub;
                sql.Insert("CtpUser", ps);
                con.Close();
            }
        }

        public void Update()
        {
            using (var con = SQLiteHelper.NewConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();
                var sql = new SQLiteHelper(cmd);
                var ps = new Dictionary<string, object>();
                ps["UserName"] = UserName;
                ps["BrokerID"] = BrokerId;
                ps["Password"] = Password;
                ps["IsSub"] = IsSub;
                sql.Update("CtpUser", ps, "UserID", UserId);
                con.Close();
            }
        }

        public void Delete()
        {
            using (var con = SQLiteHelper.NewConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();
                var sql = new SQLiteHelper(cmd);
                sql.Execute("delete from CtpUser where userid=@vuserid",
                    new Dictionary<string, object> { { "@vuserid", UserId } });
                con.Close();
            }
        }

        public UserInfo Clone()
        {
            var obj = new UserInfo
            {
                UserId = UserId,
                UserName = UserName,
                Password = Password,
                BrokerId = BrokerId,
                IsSub = IsSub
            };
            return obj;
        }
    }

    public abstract class CtpUserInfo
    {
        public bool IsChecked { get; set; }

        public string BrokerId { get; set; }

        public BrokerInfo Broker { get; set; }

        /// <summary>
        /// 账户ID。
        /// </summary>
        public string UserId { get; set; }

        public string UserName { get; set; }

        /// <summary>
        /// 账户密码。
        /// </summary>
        public string Password { get; set; }

        public bool IsLogin { get; set; }

        public int ReqId { get; set; }
    }

    public class CtpMstUser : CtpUserInfo
    {
        
    }

    public class CtpSubUser : CtpUserInfo
    {
        public int MaxOrderRef { get; set; }

        public int FrontId { get; set; }

        public int SessionId { get; set; }

        public string GetOrderRef()
        {
            var r = MaxOrderRef;
            MaxOrderRef++;
            return r.ToString("d12");
        }
        
        /// <summary>
        /// 结算信息确认时间。
        /// </summary>
        public DateTime? SettlementInfoConfirmTime { get; set; }

        public IDictionary<string, UserInserOrderConfig> Config { get; private set; }

        /// <summary>
        /// 加载跟单配置。
        /// </summary>
        public void LoadConfig()
        {
            if (Config != null)
                return;
            Config = new Dictionary<string, UserInserOrderConfig>();
            var cfgs = UserInserOrderConfig.Get(UserId);
            foreach (var c in cfgs)
            {
                Config[c.MstUserId] = c;
            }
        }
    }

    public static class CtpUserInfoEx
    {
        public static CtpTraderApi TraderApi(this CtpUserInfo user)
        {
            UserApi userApi;
            return !UserApi.This.TryGetValue(user.UserId, out userApi) ? null : userApi.TraderApi;
        }

        public static CtpInputOrder FollowOrder(this CtpSubUser user,CtpTrade ctpTrade)
        {
            if(user.Config == null || user.Config.Count == 0)
                return null;
            UserInserOrderConfig cfg;
            if (!user.Config.TryGetValue(ctpTrade.InvestorID, out cfg))
                return null;
            if (!string.IsNullOrWhiteSpace(cfg.Instrument))
            {
                var ins = ctpTrade.InstrumentID.ToLower();
                var arr = cfg.Instrument.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var b = arr.Any(s => ins.StartsWith(s.ToLower()));
                if(b)
                    return null;
            }
            var req = new CtpInputOrder();
            req.BrokerID = user.BrokerId;
            req.InvestorID = user.UserId;
            req.UserID = user.UserId;
            req.CombOffsetFlag = ((char)ctpTrade.OffsetFlag).ToString();
            if (ctpTrade.Direction == CtpDirectionType.Buy)
                req.Direction = cfg.IsInverse ? CtpDirectionType.Sell : CtpDirectionType.Buy;
            else
                req.Direction = cfg.IsInverse ? CtpDirectionType.Buy : CtpDirectionType.Sell;
            req.InstrumentID = ctpTrade.InstrumentID;
            req.BusinessUnit = ctpTrade.BusinessUnit;
            req.VolumeTotalOriginal = (int)Math.Ceiling(ctpTrade.Volume * cfg.Volume);
            req.VolumeCondition = CtpVolumeConditionType.AV;
            req.ContingentCondition = CtpContingentConditionType.Immediately;
            if (cfg.Price <= 0) //市价
            {
                req.OrderPriceType = CtpOrderPriceTypeType.AnyPrice;
                //req.LimitPrice = 0;
            }
            else
            {
                req.OrderPriceType = CtpOrderPriceTypeType.LimitPrice;
                req.LimitPrice = ctpTrade.Price;
            }

            return req;
        }
    }
}
