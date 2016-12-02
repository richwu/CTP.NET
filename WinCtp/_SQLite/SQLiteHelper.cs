using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace System.Data.SQLite
{
    public enum ColType
    {
        Text,
        DateTime,
        Integer,
        Decimal,
        BLOB
    }

    public class SQLiteHelper
    {
        private static readonly string Dbfile = $"Data Source={Path.Combine(Application.StartupPath, "ctp.db3")}";

        public static SQLiteConnection NewConnection()
        {
            var con = new SQLiteConnection(Dbfile);
            return con;
        }

        private readonly SQLiteCommand _cmd;

        public SQLiteHelper(SQLiteCommand command)
        {
            _cmd = command;
        }

        #region DB Info

        public DataTable GetTableStatus()
        {
            return Select("SELECT * FROM sqlite_master;");
        }

        public DataTable GetTableList()
        {
            var dt = GetTableStatus();
            var dt2 = new DataTable();
            dt2.Columns.Add("Tables");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string t = dt.Rows[i]["name"] + "";
                if (t != "sqlite_sequence")
                    dt2.Rows.Add(t);
            }
            return dt2;
        }

        public DataTable GetColumnStatus(string tableName)
        {
            return Select($"PRAGMA table_info(`{tableName}`);");
        }

        public DataTable ShowDatabase()
        {
            return Select("PRAGMA database_list;");
        }

        #endregion

        #region Query

        public void BeginTransaction()
        {
            _cmd.CommandText = "begin transaction;";
            _cmd.ExecuteNonQuery();
        }

        public void Commit()
        {
            _cmd.CommandText = "commit;";
            _cmd.ExecuteNonQuery();
        }

        public void Rollback()
        {
            _cmd.CommandText = "rollback";
            _cmd.ExecuteNonQuery();
        }

        public DataTable Select(string sql)
        {
            return Select(sql, new List<SQLiteParameter>());
        }

        public DataTable Select(string sql, Dictionary<string, object> dicParameters = null)
        {
            IEnumerable<SQLiteParameter> lst = GetParametersList(dicParameters);
            return Select(sql, lst);
        }

        public DataTable Select(string sql, IEnumerable<SQLiteParameter> parameters = null)
        {
            _cmd.Parameters.Clear();
            _cmd.CommandText = sql;
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    _cmd.Parameters.Add(param);
                }
            }
            SQLiteDataAdapter da = new SQLiteDataAdapter(_cmd);
            da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            da.MissingMappingAction = MissingMappingAction.Passthrough;
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public int Execute(string sql)
        {
            return Execute(sql, new List<SQLiteParameter>());
        }

        public int Execute(string sql, Dictionary<string, object> dicParameters = null)
        {
            IEnumerable<SQLiteParameter> lst = GetParametersList(dicParameters);
            return Execute(sql, lst);
        }

        public int Execute(string sql, IEnumerable<SQLiteParameter> parameters = null)
        {
            _cmd.Parameters.Clear();
            _cmd.CommandText = sql;
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    _cmd.Parameters.Add(param);
                }
            }
            return _cmd.ExecuteNonQuery();
        }

        public object ExecuteScalar(string sql)
        {
            _cmd.CommandText = sql;
            return _cmd.ExecuteScalar();
        }

        public object ExecuteScalar(string sql, Dictionary<string, object> dicParameters = null)
        {
            IEnumerable<SQLiteParameter> lst = GetParametersList(dicParameters);
            return ExecuteScalar(sql, lst);
        }

        public object ExecuteScalar(string sql, IEnumerable<SQLiteParameter> parameters = null)
        {
            _cmd.CommandText = sql;
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    _cmd.Parameters.Add(parameter);
                }
            }
            return _cmd.ExecuteScalar();
        }

        public dataType ExecuteScalar<dataType>(string sql, Dictionary<string, object> dicParameters = null)
        {
            List<SQLiteParameter> lst = null;
            if (dicParameters != null)
            {
                lst = new List<SQLiteParameter>();
                foreach (KeyValuePair<string, object> kv in dicParameters)
                {
                    lst.Add(new SQLiteParameter(kv.Key, kv.Value));
                }
            }
            return ExecuteScalar<dataType>(sql, lst);
        }

        public T ExecuteScalar<T>(string sql, IEnumerable<SQLiteParameter> parameters = null)
        {
            _cmd.CommandText = sql;
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    _cmd.Parameters.Add(parameter);
                }
            }
            return (T)Convert.ChangeType(_cmd.ExecuteScalar(), typeof(T));
        }

        public T ExecuteScalar<T>(string sql)
        {
            _cmd.CommandText = sql;
            return (T)Convert.ChangeType(_cmd.ExecuteScalar(), typeof(T));
        }

        private static IEnumerable<SQLiteParameter> GetParametersList(Dictionary<string, object> dicParameters)
        {
            var lst = new List<SQLiteParameter>();
            if (dicParameters == null)
                return lst;
            foreach (var kv in dicParameters)
            {
                lst.Add(new SQLiteParameter(kv.Key, kv.Value));
            }
            return lst;
        }

        public string Escape(string data)
        {
            data = data.Replace("'", "''");
            data = data.Replace("\\", "\\\\");
            return data;
        }

        public int Insert(string tableName, Dictionary<string, object> dic)
        {
            var sbCol = new StringBuilder();
            var sbVal = new StringBuilder();

            foreach (KeyValuePair<string, object> kv in dic)
            {
                if (sbCol.Length == 0)
                {
                    sbCol.Append("insert into ");
                    sbCol.Append(tableName);
                    sbCol.Append("(");
                }
                else
                {
                    sbCol.Append(",");
                }

                sbCol.Append("`");
                sbCol.Append(kv.Key);
                sbCol.Append("`");

                if (sbVal.Length == 0)
                {
                    sbVal.Append(" values(");
                }
                else
                {
                    sbVal.Append(", ");
                }

                sbVal.Append("@v");
                sbVal.Append(kv.Key);
            }

            sbCol.Append(") ");
            sbVal.Append(");");

            _cmd.CommandText = sbCol.ToString() + sbVal.ToString();
            _cmd.Parameters.Clear();
            foreach (KeyValuePair<string, object> kv in dic)
            {
                _cmd.Parameters.AddWithValue("@v" + kv.Key, kv.Value);
            }

            return _cmd.ExecuteNonQuery();
        }

        public int Update(string tableName, Dictionary<string, object> dicData, string colCond, object varCond)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic[colCond] = varCond;
            return Update(tableName, dicData, dic);
        }

        public int Update(string tableName, Dictionary<string, object> dicData, Dictionary<string, object> dicCond)
        {
            if (dicData.Count == 0)
                throw new Exception("dicData is empty.");

            StringBuilder sbData = new System.Text.StringBuilder();

            Dictionary<string, object> _dicTypeSource = new Dictionary<string, object>();

            foreach (KeyValuePair<string, object> kv1 in dicData)
            {
                _dicTypeSource[kv1.Key] = null;
            }

            foreach (KeyValuePair<string, object> kv2 in dicCond)
            {
                if (!_dicTypeSource.ContainsKey(kv2.Key))
                    _dicTypeSource[kv2.Key] = null;
            }

            sbData.Append("update `");
            sbData.Append(tableName);
            sbData.Append("` set ");

            bool firstRecord = true;

            foreach (KeyValuePair<string, object> kv in dicData)
            {
                if (firstRecord)
                    firstRecord = false;
                else
                    sbData.Append(",");

                sbData.Append("`");
                sbData.Append(kv.Key);
                sbData.Append("` = ");

                sbData.Append("@v");
                sbData.Append(kv.Key);
            }

            sbData.Append(" where ");

            firstRecord = true;

            foreach (KeyValuePair<string, object> kv in dicCond)
            {
                if (firstRecord)
                    firstRecord = false;
                else
                {
                    sbData.Append(" and ");
                }

                sbData.Append("`");
                sbData.Append(kv.Key);
                sbData.Append("` = ");

                sbData.Append("@c");
                sbData.Append(kv.Key);
            }

            sbData.Append(";");

            _cmd.CommandText = sbData.ToString();
            _cmd.Parameters.Clear();
            foreach (KeyValuePair<string, object> kv in dicData)
            {
                _cmd.Parameters.AddWithValue("@v" + kv.Key, kv.Value);
            }

            foreach (KeyValuePair<string, object> kv in dicCond)
            {
                _cmd.Parameters.AddWithValue("@c" + kv.Key, kv.Value);
            }

            return _cmd.ExecuteNonQuery();
        }

        public long LastInsertRowId()
        {
            return ExecuteScalar<long>("select last_insert_rowid();");
        }

        #endregion

        #region Utilities

        public void CreateTable(SQLiteTable table)
        {
            StringBuilder sb = new Text.StringBuilder();
            sb.Append("create table if not exists `");
            sb.Append(table.TableName);
            sb.AppendLine("`(");

            bool firstRecord = true;

            foreach (SQLiteColumn col in table.Columns)
            {
                if (col.ColumnName.Trim().Length == 0)
                {
                    throw new Exception("Column name cannot be blank.");
                }

                if (firstRecord)
                    firstRecord = false;
                else
                    sb.AppendLine(",");

                sb.Append(col.ColumnName);
                sb.Append(" ");

                if (col.AutoIncrement)
                {

                    sb.Append("integer primary key autoincrement");
                    continue;
                }

                switch (col.ColDataType)
                {
                    case ColType.Text:
                        sb.Append("text"); break;
                    case ColType.Integer:
                        sb.Append("integer"); break;
                    case ColType.Decimal:
                        sb.Append("decimal"); break;
                    case ColType.DateTime:
                        sb.Append("datetime"); break;
                    case ColType.BLOB:
                        sb.Append("blob"); break;
                }

                if (col.PrimaryKey)
                    sb.Append(" primary key");
                else if (col.NotNull)
                    sb.Append(" not null");
                else if (col.DefaultValue.Length > 0)
                {
                    sb.Append(" default ");

                    if (col.DefaultValue.Contains(" ") || col.ColDataType == ColType.Text || col.ColDataType == ColType.DateTime)
                    {
                        sb.Append("'");
                        sb.Append(col.DefaultValue);
                        sb.Append("'");
                    }
                    else
                    {
                        sb.Append(col.DefaultValue);
                    }
                }
            }

            sb.AppendLine(");");

            _cmd.CommandText = sb.ToString();
            _cmd.ExecuteNonQuery();
        }

        public void RenameTable(string tableFrom, string tableTo)
        {
            _cmd.CommandText = $"alter table `{tableFrom}` rename to `{tableTo}`;";
            _cmd.ExecuteNonQuery();
        }

        public void CopyAllData(string tableFrom, string tableTo)
        {
            DataTable dt1 = Select($"select * from `{tableFrom}` where 1 = 2;");
            DataTable dt2 = Select($"select * from `{tableTo}` where 1 = 2;");

            Dictionary<string, bool> dic = new Dictionary<string, bool>();

            foreach (DataColumn dc in dt1.Columns)
            {
                if (dt2.Columns.Contains(dc.ColumnName))
                {
                    if (!dic.ContainsKey(dc.ColumnName))
                    {
                        dic[dc.ColumnName] = true;
                    }
                }
            }

            foreach (DataColumn dc in dt2.Columns)
            {
                if (dt1.Columns.Contains(dc.ColumnName))
                {
                    if (!dic.ContainsKey(dc.ColumnName))
                    {
                        dic[dc.ColumnName] = true;
                    }
                }
            }

            StringBuilder sb = new Text.StringBuilder();

            foreach (KeyValuePair<string, bool> kv in dic)
            {
                if (sb.Length > 0)
                    sb.Append(",");

                sb.Append("`");
                sb.Append(kv.Key);
                sb.Append("`");
            }

            StringBuilder sb2 = new Text.StringBuilder();
            sb2.Append("insert into `");
            sb2.Append(tableTo);
            sb2.Append("`(");
            sb2.Append(sb.ToString());
            sb2.Append(") select ");
            sb2.Append(sb.ToString());
            sb2.Append(" from `");
            sb2.Append(tableFrom);
            sb2.Append("`;");

            _cmd.CommandText = sb2.ToString();
            _cmd.ExecuteNonQuery();
        }

        public void DropTable(string table)
        {
            _cmd.CommandText = $"drop table if exists `{table}`";
            _cmd.ExecuteNonQuery();
        }

        public void UpdateTableStructure(string targetTable, SQLiteTable newStructure)
        {
            newStructure.TableName = targetTable + "_temp";

            CreateTable(newStructure);

            CopyAllData(targetTable, newStructure.TableName);

            DropTable(targetTable);

            RenameTable(newStructure.TableName, targetTable);
        }

        public void AttachDatabase(string database, string alias)
        {
            Execute($"attach '{database}' as {alias};");
        }

        public void DetachDatabase(string alias)
        {
            Execute($"detach {alias};");
        }

        #endregion

        public void Post(DataTable table)
        {
            foreach (DataRow dr in table.Rows)
            {
                if (dr.RowState == DataRowState.Added)
                {
                    var ps = new Dictionary<string,object>();
                    foreach (DataColumn c in table.Columns)
                    {
                        if(c.AutoIncrement)
                            continue;
                        ps[c.ColumnName] = dr[c.ColumnName];
                    }
                    Insert(table.TableName, ps);
                }
                else if (dr.RowState == DataRowState.Modified)
                {
                    var dicPk = new Dictionary<string, DataColumn>();
                    foreach (var dc in table.PrimaryKey)
                    {
                        dicPk[dc.ColumnName] = dc;
                    }
                    var dicCond = new Dictionary<string, object>();
                    foreach (var c in table.PrimaryKey)
                    {
                        dicCond[c.ColumnName] = dr[c.ColumnName];
                    }
                    var dicData = new Dictionary<string, object>();
                    foreach (DataColumn c in table.Columns)
                    {
                        if(dicPk.ContainsKey(c.ColumnName))
                            continue;
                        if(dr[c.ColumnName,DataRowVersion.Original] == dr[c.ColumnName])
                            continue;
                        dicData[c.ColumnName] = dr[c.ColumnName];
                    }
                    Update(table.TableName, dicData, dicCond);
                }
                else if (dr.RowState == DataRowState.Deleted)
                {
                    var ps = new Dictionary<string, object>();
                    var where = "";
                    var b = true;
                    foreach (var c in table.PrimaryKey)
                    {
                        ps[$"@v{c.ColumnName}"] = dr[c.ColumnName,DataRowVersion.Original];
                        if (b) b = false;
                        else where += " and ";
                        where += $"{c.ColumnName}=@v{c.ColumnName}";
                    }
                    Execute($"delete from {table.TableName} where {where}", ps);
                }
            }
        }
    }
}