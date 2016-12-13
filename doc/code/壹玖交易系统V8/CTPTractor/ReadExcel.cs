using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

namespace CTPTractor
{
	public class ReadExcel
	{
		public System.Data.DataSet ReadExcelToDataSet(string sExcelFile)
		{
			System.Data.OleDb.OleDbConnection oleDbConnection = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + sExcelFile + ";Extended Properties=Excel 8.0;");
			oleDbConnection.Open();
			System.Data.DataTable oleDbSchemaTable = oleDbConnection.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
			System.Collections.Generic.List<System.Data.DataSet> list = new System.Collections.Generic.List<System.Data.DataSet>();
			System.Data.DataSet dataSet = new System.Data.DataSet();
			for (int i = 0; i < oleDbSchemaTable.Rows.Count; i++)
			{
				string text = oleDbSchemaTable.Rows[i][2].ToString().Trim();
				string text2 = "select * from [" + text + "]";
				System.Data.OleDb.OleDbCommand oleDbCommand = new System.Data.OleDb.OleDbCommand(text2, oleDbConnection);
				System.Data.OleDb.OleDbDataAdapter oleDbDataAdapter = new System.Data.OleDb.OleDbDataAdapter(text2, oleDbConnection);
				oleDbDataAdapter.Fill(dataSet, text);
			}
			oleDbConnection.Close();
			return dataSet;
		}
	}
}
