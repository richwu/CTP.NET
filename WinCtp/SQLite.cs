using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinCtp
{
    internal static class SQLite
    {
        private static readonly string _dbfile;

        static SQLite()
        {
            _dbfile = $"Data Source={Path.Combine(Application.StartupPath, "ctp.db3")}";
        }

        public static void Initializer()
        {
        }

        public static SQLiteConnection NewConnection()
        {
            var con = new SQLiteConnection(_dbfile);
            return con;
        }
    }
}
