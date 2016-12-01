using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace WinCtp
{
    public class UserInserOrderConfig
    {
        public static IList<UserInserOrderConfig> GetAll()
        {
            return new List<UserInserOrderConfig>();
        }

        public string SubUserId { get; set; }

        public string MstUserId { get; set; }

        public string Instrument { get; set; }

        public int Volume { get; set; }

        public bool IsInverse { get; set; }

        public int Version { get; set; }

        public UserInserOrderConfig Clone()
        {
            var obj = new UserInserOrderConfig();
            obj.SubUserId = SubUserId;
            obj.MstUserId = MstUserId;
            obj.Instrument = Instrument;
            obj.Volume = Volume;
            obj.IsInverse = IsInverse;
            obj.Version = Version;
            return obj;
        }

        public void Save()
        {
            using (var con = SQLite.NewConnection())
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
                ps["Version"] = Version + 1;
                sql.Insert("CfgUserInserOrder", ps);
                con.Close();
            }
        }

        public void Update()
        {
            using (var con = SQLite.NewConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();
                var sql = new SQLiteHelper(cmd);
                var ps = new Dictionary<string, object>();
                ps["Instrument"] = Instrument;
                ps["Volume"] = Volume;
                ps["IsInverse"] = IsInverse;
                ps["Version"] = Version + 1;
                sql.Update("CfgUserInserOrder", ps, new Dictionary<string, object>()
                {
                    {"SubUserID", SubUserId }, {"MstUserID", MstUserId }, {"Version", Version }
                });
                con.Close();
            }
        }

        public void Delete()
        {
            using (var con = SQLite.NewConnection())
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