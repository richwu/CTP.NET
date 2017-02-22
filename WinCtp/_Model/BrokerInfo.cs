using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace WinCtp
{
    /// <summary>
    /// 期货公司。
    /// </summary>
    public class BrokerInfo : ILookupObject
    {
        /// <summary>
        /// 公司代码。
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 公司名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 交易前置地址。
        /// </summary>
        public string TraderFrontAddress { get; set; }

        /// <summary>
        /// 行情前置地址。
        /// </summary>
        public string MarketFrontAddress { get; set; }

        public static IList<BrokerInfo> GetAll()
        {
            var arr = new List<BrokerInfo>();
            DataTable table;
            using (var con = SQLiteHelper.NewConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();
                var sql = new SQLiteHelper(cmd);
                table = sql.Select("select BrokerID,BrokerName,TraderFrontAddress,MarketFrontAddress from CtpBroker");
                con.Close();
            }
            if (table == null || table.Rows.Count == 0)
                return arr;
            foreach (DataRow r in table.Rows)
            {
                var a = new BrokerInfo();
                a.Id = r["BrokerID"].ToString();
                a.Name = r["BrokerName"].ToString();
                a.TraderFrontAddress = r["TraderFrontAddress"].ToString().Unprotect();
                a.MarketFrontAddress = r["MarketFrontAddress"].ToString().Unprotect();
                arr.Add(a);
            }
            return arr;
        }

        /// <summary>
        /// 深度复制对象。
        /// </summary>
        /// <returns></returns>
        public BrokerInfo Clone()
        {
            return new BrokerInfo
            {
                Id = Id,
                Name = Name,
                TraderFrontAddress = TraderFrontAddress,
                MarketFrontAddress = MarketFrontAddress
            };
        }

        public void Save()
        {
            using (var con = SQLiteHelper.NewConnection())
            {
                con.Open();
                var cmd = con.CreateCommand();
                var sql = new SQLiteHelper(cmd);
                var ps = new Dictionary<string, object>();
                ps["BrokerID"] = Id;
                ps["BrokerName"] = Name;
                ps["TraderFrontAddress"] = TraderFrontAddress.Protect();
                ps["MarketFrontAddress"] = MarketFrontAddress.Protect();
                sql.Insert("CtpBroker", ps);
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
                ps["BrokerName"] = Name;
                ps["TraderFrontAddress"] = TraderFrontAddress.Protect();
                ps["MarketFrontAddress"] = MarketFrontAddress.Protect();
                sql.Update("CtpBroker", ps, new Dictionary<string, object>()
                {
                    {"BrokerID", Id }
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
                sql.Execute("delete from CtpBroker where BrokerID=@p_BrokerID",
                    new Dictionary<string, object> { { "@p_BrokerID", Id } });
                con.Close();
            }
        }

        public object Value => Id;

        public string Display => Name;

        public int Sn => 0;
    }
}