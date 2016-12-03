using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using GalaxyFutures.Sfit.Api;

namespace WinCtp
{
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
            var obj = new UserInfo();
            obj.UserId = UserId;
            obj.UserName = UserName;
            obj.Password = Password;
            obj.BrokerId = BrokerId;
            obj.IsSub = IsSub;
            return obj;
        }
    }

    public abstract class CtpUserInfo
    {
        public bool IsChecked { get; set; }

        public string BrokerId { get; set; }

        public BrokerInfo Broker { get; set; }

        public CtpTraderApi TraderApi => Broker.TraderApi;

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

        public int MaxOrderRef { get; set; }
    }

    public class CtpMstUser : CtpUserInfo
    {
        
    }

    public class CtpSubUser : CtpUserInfo
    {
        private IDictionary<string, UserInserOrderConfig> _cfg;

        private void LoadConfig()
        {
            if (_cfg != null)
                return;
            _cfg = new Dictionary<string, UserInserOrderConfig>();
            var cfgs = UserInserOrderConfig.Get(UserId);
            foreach (var c in cfgs)
            {
                _cfg[c.MstUserId] = c;
            }
        }

        public bool InsertOrder(CtpTrade ctpTrade)
        {
            LoadConfig();
            UserInserOrderConfig cfg;
            if (!_cfg.TryGetValue(ctpTrade.InvestorID, out cfg))
                return false;
            if(!string.IsNullOrEmpty(cfg.Instrument) && ctpTrade.InstrumentID.StartsWith(cfg.Instrument))
                return false;
            var req = new CtpInputOrder();
            //req.CombOffsetFlag = ctpTrade.OffsetFlag.ToString();//==
            if (ctpTrade.Direction == CtpDirectionType.Buy)
                req.Direction = cfg.IsInverse ? CtpDirectionType.Sell : CtpDirectionType.Buy;
            else
                req.Direction = cfg.IsInverse ? CtpDirectionType.Buy : CtpDirectionType.Sell;
            req.InstrumentID = ctpTrade.InstrumentID;
            req.LimitPrice = ctpTrade.Price;
            req.BusinessUnit = ctpTrade.BusinessUnit;
            req.VolumeTotalOriginal = (int)Math.Ceiling(ctpTrade.Volume * cfg.Volume);
            req.ContingentCondition = CtpContingentConditionType.Immediately;
            req.OrderPriceType = CtpOrderPriceTypeType.LimitPrice;
            req.VolumeCondition = CtpVolumeConditionType.AV;
            var rsp = TraderApi.ReqOrderInsert(req, RequestId.OrderInsertId());
            return rsp == 0;
        }
    }
}
