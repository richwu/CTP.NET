using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Text;

namespace CTPTractor
{
	internal class ReadTxtToDatatable
	{
		public void readtxt(ref System.Data.DataTable dt, string filename, int num)
		{
			string text = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" + filename;
			if (!System.IO.File.Exists(text))
			{
				System.IO.File.CreateText(text);
			}
			else
			{
				this.TextToDatatable(ref dt, text, num);
			}
		}

		public void writetxt(System.Data.DataTable dt, string filename)
		{
			string text = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" + filename;
			if (!System.IO.File.Exists(text))
			{
				System.IO.File.CreateText(text);
			}
			this.DatatableToTxt(dt, text);
		}

		private void TextToDatatable(ref System.Data.DataTable dt, string filepath, int num)
		{
			System.Collections.Generic.List<StockTransSet> list = new System.Collections.Generic.List<StockTransSet>();
			System.IO.FileStream fileStream = System.IO.File.Open(filepath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
			System.IO.StreamReader streamReader = new System.IO.StreamReader(fileStream, System.Text.Encoding.Default);
			bool flag = true;
			while (flag)
			{
				string text = streamReader.ReadLine();
				if (!string.IsNullOrEmpty(text))
				{
					string[] array = text.Split(new char[]
					{
						'\t'
					});
					StockTransSet stockTransSet = new StockTransSet();
					int num2 = 0;
					if (num == 0 && array.Length >= 16)
					{
						num2 = 16;
					}
					else if (num == 0 && array.Length >= 11)
					{
						num2 = 11;
					}
					else if (num == 0 && array.Length >= 10)
					{
						num2 = 10;
					}
					else if (num == 1 && array.Length >= 3)
					{
						num2 = 3;
					}
					else if (num == 2 && array.Length >= 3)
					{
						num2 = 3;
					}
					System.Data.DataRow dataRow = dt.NewRow();
					for (int i = 0; i < num2; i++)
					{
						System.Type dataType = dt.Columns[i].DataType;
						if (dataType == typeof(int))
						{
							dataRow[i] = System.Convert.ToInt32((array[i] == "") ? "10" : array[i]);
						}
						else
						{
							dataRow[i] = array[i];
						}
					}
					dt.Rows.Add(dataRow);
				}
				else
				{
					flag = false;
				}
			}
			fileStream.Close();
		}

		public void DatatableToTxt(System.Data.DataTable dt, string filepath)
		{
			try
			{
				System.IO.FileStream fileStream = System.IO.File.Open(filepath, System.IO.FileMode.Truncate, System.IO.FileAccess.Write);
				using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(fileStream, System.Text.Encoding.Default))
				{
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						string text = "";
						for (int j = 0; j < dt.Columns.Count; j++)
						{
							text = text + dt.Rows[i][j].ToString() + "\t";
						}
						streamWriter.WriteLine(text);
					}
				}
				fileStream.Close();
			}
			catch (System.Exception var_5_AA)
			{
			}
		}
	}
}
