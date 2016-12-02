using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace WinCtp
{
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
                table = sql.Select("select SubUserID,MstUserID,Instrument,Volume,IsInverse from CfgUserInserOrder");
                con.Close();
            }
            foreach (DataRow r in table.Rows)
            {
                var a = new UserInserOrderConfig();
                a.SubUserId = r["SubUserID"].ToString();
                a.MstUserId = r["MstUserID"].ToString();
                a.Instrument = r["Instrument"].ToString();
                a.Volume = Convert.ToInt32(r["Volume"]);
                a.IsInverse = Convert.ToBoolean(r["IsInverse"]);
                arr.Add(a);
            }
            return arr;
        }

        public string SubUserId { get; set; }

        public string MstUserId { get; set; }

        public string Instrument { get; set; }

        public int Volume { get; set; }

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