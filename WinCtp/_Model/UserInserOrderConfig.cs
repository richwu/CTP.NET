using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace WinCtp
{
    /// <summary>
    /// 用户跟单配置。
    /// </summary>
    public class UserInserOrderConfig
    {
        public static IList<UserInserOrderConfig> GetAll()
        {
            var arr = new List<UserInserOrderConfig>();
            DataTable table;
            using (var con = SQLiteHelper.NewConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();
                var sql = new SQLiteHelper(cmd);
                table = sql.Select("select SubUserID,MstUserID,Instrument,Volume,Price,IsInverse from CfgUserInserOrder");
                con.Close();
            }
            foreach (DataRow r in table.Rows)
            {
                var a = new UserInserOrderConfig();
                a.SubUserId = r["SubUserID"].ToString();
                a.MstUserId = r["MstUserID"].ToString();
                a.Instrument = r["Instrument"].ToString();
                a.Volume = Convert.ToDouble(r["Volume"]);
                a.Price = Convert.ToDouble(r["Price"]);
                a.IsInverse = Convert.ToBoolean(r["IsInverse"]);
                arr.Add(a);
            }
            return arr;
        }

        public static IList<UserInserOrderConfig> Get(string subUserId)
        {
            var arr = new List<UserInserOrderConfig>();
            DataTable table;
            using (var con = SQLiteHelper.NewConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();
                var sql = new SQLiteHelper(cmd);
                table = sql.Select("select SubUserID,MstUserID,Instrument,Volume,Price,IsInverse from CfgUserInserOrder where SubUserID=@p_SubUserID",
                    new Dictionary<string, object> { { "@p_SubUserID", subUserId }});
                con.Close();
            }
            foreach (DataRow r in table.Rows)
            {
                var a = new UserInserOrderConfig();
                a.SubUserId = r["SubUserID"].ToString();
                a.MstUserId = r["MstUserID"].ToString();
                a.Instrument = r["Instrument"].ToString();
                a.Volume = Convert.ToDouble(r["Volume"]);
                a.Price = Convert.ToDouble(r["Price"]);
                a.IsInverse = Convert.ToBoolean(r["IsInverse"]);
                arr.Add(a);
            }
            return arr;
        }

        /// <summary>
        /// 子账户。
        /// </summary>
        public string SubUserId { get; set; }

        /// <summary>
        /// 主账户。
        /// </summary>
        public string MstUserId { get; set; }

        /// <summary>
        /// 不跟单合约品种。
        /// </summary>
        /// <remarks>按英文逗号分隔。</remarks>
        /// <example>if,gb</example>
        public string Instrument { get; set; }

        /// <summary>
        /// 手数倍率。
        /// </summary>
        public double Volume { get; set; }

        /// <summary>
        /// 价格。
        /// </summary>
        /// <remarks>&gt;0限价，&lt;=0市价</remarks>
        public double Price { get; set; }

        /// <summary>
        /// 是否反向。
        /// </summary>
        public bool IsInverse { get; set; }

        public UserInserOrderConfig Clone()
        {
            var obj = new UserInserOrderConfig();
            obj.SubUserId = SubUserId;
            obj.MstUserId = MstUserId;
            obj.Instrument = Instrument;
            obj.Volume = Volume;
            obj.IsInverse = IsInverse;
            return obj;
        }

        public void Save()
        {
            using (var con = SQLiteHelper.NewConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();
                var sql = new SQLiteHelper(cmd);
                var ps = new Dictionary<string, object>();
                ps["SubUserID"] = SubUserId;
                ps["MstUserID"] = MstUserId;
                ps["Instrument"] = Instrument;
                ps["Volume"] = Volume;
                ps["Price"] = Price;
                ps["IsInverse"] = IsInverse;
                sql.Insert("CfgUserInserOrder", ps);
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
                ps["Instrument"] = Instrument;
                ps["Volume"] = Volume;
                ps["Price"] = Price;
                ps["IsInverse"] = IsInverse;
                sql.Update("CfgUserInserOrder", ps, new Dictionary<string, object>()
                {
                    {"SubUserID", SubUserId }, {"MstUserID", MstUserId }
                });
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
                sql.Execute("delete from CfgUserInserOrder where SubUserID=@vsubuserid AND MstUserID=@vmstuserid",
                    new Dictionary<string, object> { { "@vsubuserid", SubUserId }, { "@vmstuserid", MstUserId } });
                con.Close();
            }
        }
    }
}