using Algorithm;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CTPTractor
{
	internal class UserKey
	{
		public bool Key(int type)
		{
			string text = this.ReadKey();
			bool result;
			if (text == null)
			{
				if (type == 0)
				{
					System.Windows.Forms.MessageBox.Show("没有授权，请注册！");
				}
				result = false;
			}
			else
			{
				AlgClass algClass = new AlgClass();
				text = algClass.Psw_Change_Public(text);
				char[] array = text.ToArray<char>();
				text = null;
				Regex regex = new Regex("^[a-z]+$");
				for (int i = 0; i < array.Length; i++)
				{
					if (regex.IsMatch(array[i].ToString()))
					{
						array[i] = System.Convert.ToChar(System.Convert.ToInt32(array[i]) - 32);
					}
					if (array[i] == ' ')
					{
						text += "0";
					}
					else
					{
						text += array[i].ToString();
					}
				}
				string sMac = text.Substring(8);
				if (!this.PCID(sMac, type))
				{
					result = false;
				}
				else
				{
					string sTime = text.Substring(0, 8);
					result = this.Valid(sTime, type);
				}
			}
			return result;
		}

		private string ReadKey()
		{
			string path = System.Windows.Forms.Application.StartupPath + "//Keys.key";
			string result;
			if (!System.IO.File.Exists(path))
			{
				result = null;
			}
			else
			{
				System.IO.FileStream fileStream = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
				System.IO.BinaryReader binaryReader = new System.IO.BinaryReader(fileStream);
				string text = binaryReader.ReadString();
				binaryReader.Close();
				fileStream.Close();
				result = text;
			}
			return result;
		}

		private bool Valid(string sTime, int type)
		{
			string text = sTime.Substring(0, 4);
			string text2 = sTime.Substring(4, 2);
			string text3 = sTime.Substring(6, 2);
			System.DateTime d = UserKey.request();
			if (d.ToString("yyyy-MM-dd") == "2999-01-01")
			{
				d = UserKey.GetBaiduTime();
			}
			System.TimeSpan timeSpan = System.Convert.ToDateTime(string.Concat(new string[]
			{
				text,
				"-",
				text2,
				"-",
				text3
			})) - d;
			bool result;
			if (timeSpan.Days <= 10)
			{
				if (timeSpan.Days < 0)
				{
					if (type == 0)
					{
						System.Windows.Forms.MessageBox.Show("秘钥失效，请联系管理员获取最新秘钥。");
					}
					result = false;
					return result;
				}
				if (type == 0 && timeSpan.Days <= 3)
				{
					System.Windows.Forms.MessageBox.Show("客户端有效期还剩：" + timeSpan.Days + "天，请联系管理员获取最新秘钥。");
				}
			}
			result = true;
			return result;
		}

		public static System.DateTime request()
		{
			System.DateTime result;
			try
			{
				string arg = "http://apis.baidu.com/3023/time/time";
				string arg2 = "";
				string requestUriString = arg + '?' + arg2;
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
				httpWebRequest.Method = "GET";
				httpWebRequest.Headers.Add("apikey", "e15c285b4b71cc76a5271b8db63af71c");
				HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				System.IO.Stream responseStream = httpWebResponse.GetResponseStream();
				string text = "";
				System.IO.StreamReader streamReader = new System.IO.StreamReader(responseStream, System.Text.Encoding.UTF8);
				string str;
				while ((str = streamReader.ReadLine()) != null)
				{
					text += str;
				}
				string[] array = text.Substring(1, text.Length - 2).Split(new string[]
				{
					":"
				}, System.StringSplitOptions.RemoveEmptyEntries);
				System.DateTime dateTime = System.Convert.ToDateTime("1970-01-01 08:00:00").AddSeconds((double)System.Convert.ToInt32(array[1]));
				result = dateTime;
			}
			catch (System.Exception var_11_F3)
			{
				result = System.DateTime.Parse("2999-1-1");
			}
			finally
			{
			}
			return result;
		}

		public static System.DateTime GetBeijingTime()
		{
			WebRequest webRequest = null;
			WebResponse webResponse = null;
			System.DateTime dateTime;
			System.DateTime result;
			try
			{
				webRequest = WebRequest.Create("http://www.beijing-time.org/time.asp");
				webResponse = webRequest.GetResponse();
				string text = string.Empty;
				using (System.IO.Stream responseStream = webResponse.GetResponseStream())
				{
					using (System.IO.StreamReader streamReader = new System.IO.StreamReader(responseStream, System.Text.Encoding.UTF8))
					{
						text = streamReader.ReadToEnd();
					}
				}
				string[] array = text.Split(new char[]
				{
					';'
				});
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = array[i].Replace("\r\n", "");
				}
				string text2 = array[1].Substring(array[1].IndexOf("nyear=") + 6);
				string text3 = array[2].Substring(array[2].IndexOf("nmonth=") + 7);
				string text4 = array[3].Substring(array[3].IndexOf("nday=") + 5);
				string text5 = array[5].Substring(array[5].IndexOf("nhrs=") + 5);
				string text6 = array[6].Substring(array[6].IndexOf("nmin=") + 5);
				string text7 = array[7].Substring(array[7].IndexOf("nsec=") + 5);
				dateTime = System.DateTime.Parse(string.Concat(new string[]
				{
					text2,
					"-",
					text3,
					"-",
					text4,
					" ",
					text5,
					":",
					text6,
					":",
					text7
				}));
			}
			catch (WebException)
			{
				result = System.DateTime.Parse("2999-1-1");
				return result;
			}
			catch (System.Exception)
			{
				result = System.DateTime.Parse("2999-1-1");
				return result;
			}
			finally
			{
				if (webResponse != null)
				{
					webResponse.Close();
				}
				if (webRequest != null)
				{
					webRequest.Abort();
				}
			}
			result = dateTime;
			return result;
		}

		public static System.DateTime GetBaiduTime()
		{
			WebRequest webRequest = null;
			WebResponse webResponse = null;
			System.DateTime dateTime;
			System.DateTime result;
			try
			{
				webRequest = WebRequest.Create("http://123.125.114.102/special/time/");
				webResponse = webRequest.GetResponse();
				string text = string.Empty;
				using (System.IO.Stream responseStream = webResponse.GetResponseStream())
				{
					using (System.IO.StreamReader streamReader = new System.IO.StreamReader(responseStream, System.Text.Encoding.UTF8))
					{
						text = streamReader.ReadToEnd();
					}
				}
				string text2 = "window.baidu_time(";
				int num = text.IndexOf(text2);
				if (num != -1)
				{
					string value = text.Substring(num + text2.Length, 13);
					dateTime = UserKey.ConvertIntDateTime(System.Convert.ToDouble(value));
				}
				else
				{
					dateTime = System.DateTime.Parse("2999-1-1");
				}
			}
			catch (WebException)
			{
				result = System.DateTime.Parse("2999-1-1");
				return result;
			}
			catch (System.Exception)
			{
				result = System.DateTime.Parse("2999-1-1");
				return result;
			}
			finally
			{
				if (webResponse != null)
				{
					webResponse.Close();
				}
				if (webRequest != null)
				{
					webRequest.Abort();
				}
			}
			result = dateTime;
			return result;
		}

		public static System.DateTime ConvertIntDateTime(double d)
		{
			System.DateTime minValue = System.DateTime.MinValue;
			return System.TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)).AddMilliseconds(d);
		}

		private bool PCID(string sMac, int type)
		{
			HardIDKey hardIDKey = new HardIDKey();
			char[] array = sMac.ToArray<char>();
			sMac = null;
			Regex regex = new Regex("^[a-z]+$");
			for (int i = 0; i < array.Length; i++)
			{
				if (regex.IsMatch(array[i].ToString()))
				{
					array[i] = System.Convert.ToChar(System.Convert.ToInt32(array[i]) - 32);
				}
				sMac += array[i].ToString();
			}
			bool result;
			if (hardIDKey.GetCPUInfo() + hardIDKey.GetWebInfo() + hardIDKey.GetHardInfo() + "DT" != sMac)
			{
				if (type == 0)
				{
					System.Windows.Forms.MessageBox.Show("电脑未获得授权！");
					result = false;
					return result;
				}
			}
			result = true;
			return result;
		}
	}
}
