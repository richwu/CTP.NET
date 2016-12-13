using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace CTPTractor
{
	public class NistTime
	{
		public static System.DateTime GetChineseDateTime()
		{
			System.DateTime result = System.DateTime.MinValue;
			try
			{
				string input = "http://www.time.ac.cn/stime.asp";
				string pattern = "\\d{4}年\\d{1,2}月\\d{1,2}日";
				string pattern2 = "hrs\\s+=\\s+\\d{1,2}";
				string pattern3 = "min\\s+=\\s+\\d{1,2}";
				string pattern4 = "sec\\s+=\\s+\\d{1,2}";
				Regex regex = new Regex(pattern);
				Regex regex2 = new Regex(pattern2);
				Regex regex3 = new Regex(pattern3);
				Regex regex4 = new Regex(pattern4);
				result = System.DateTime.Parse(regex.Match(input).Value);
				int @int = NistTime.GetInt(regex2.Match(input).Value, false);
				int int2 = NistTime.GetInt(regex3.Match(input).Value, false);
				int int3 = NistTime.GetInt(regex4.Match(input).Value, false);
				result = result.AddHours((double)@int).AddMinutes((double)int2).AddSeconds((double)int3);
			}
			catch
			{
			}
			return result;
		}

		private static int GetInt(string origin, bool fullMatch)
		{
			int result;
			if (string.IsNullOrEmpty(origin))
			{
				result = 0;
			}
			else
			{
				origin = origin.Trim();
				if (!fullMatch)
				{
					string pattern = "-?\\d+";
					Regex regex = new Regex(pattern);
					origin = regex.Match(origin.Trim()).Value;
				}
				int num = 0;
				int.TryParse(origin, out num);
				result = num;
			}
			return result;
		}

		public static System.DateTime GetStandardTime()
		{
			WebRequest webRequest = WebRequest.Create("http://www.time.ac.cn/timeflash.asp?user=flash");
			webRequest.Credentials = CredentialCache.DefaultCredentials;
			WebResponse response = webRequest.GetResponse();
			System.IO.StreamReader streamReader = new System.IO.StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);
			string text = streamReader.ReadToEnd();
			streamReader.Close();
			response.Close();
			int num = text.IndexOf("<year>") + 6;
			int num2 = text.IndexOf("<month>") + 7;
			int num3 = text.IndexOf("<day>") + 5;
			int num4 = text.IndexOf("<hour>") + 6;
			int num5 = text.IndexOf("<minite>") + 8;
			int num6 = text.IndexOf("<second>") + 8;
			string text2 = text.Substring(num, text.IndexOf("</year>") - num);
			string text3 = text.Substring(num2, text.IndexOf("</month>") - num2);
			string text4 = text.Substring(num3, text.IndexOf("</day>") - num3);
			string text5 = text.Substring(num4, text.IndexOf("</hour>") - num4);
			string text6 = text.Substring(num5, text.IndexOf("</minite>") - num5);
			string text7 = text.Substring(num6, text.IndexOf("</second>") - num6);
			return System.DateTime.Parse(string.Concat(new string[]
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

		public static System.DateTime GetBeijingTime()
		{
			WebRequest webRequest = WebRequest.Create("http://www.beijing-time.org/time.asp");
			WebResponse response = webRequest.GetResponse();
			string text = string.Empty;
			using (System.IO.Stream responseStream = response.GetResponseStream())
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
			return System.DateTime.Parse(string.Concat(new string[]
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

		public static System.DateTime DataStandardTime()
		{
			string[,] array = new string[14, 2];
			int[] array2 = new int[]
			{
				3,
				2,
				4,
				8,
				9,
				6,
				11,
				5,
				10,
				0,
				1,
				7,
				12
			};
			array[0, 0] = "time-a.nist.gov";
			array[0, 1] = "129.6.15.28";
			array[1, 0] = "time-b.nist.gov";
			array[1, 1] = "129.6.15.29";
			array[2, 0] = "time-a.timefreq.bldrdoc.gov";
			array[2, 1] = "132.163.4.101";
			array[3, 0] = "time-b.timefreq.bldrdoc.gov";
			array[3, 1] = "132.163.4.102";
			array[4, 0] = "time-c.timefreq.bldrdoc.gov";
			array[4, 1] = "132.163.4.103";
			array[5, 0] = "utcnist.colorado.edu";
			array[5, 1] = "128.138.140.44";
			array[6, 0] = "time.nist.gov";
			array[6, 1] = "192.43.244.18";
			array[7, 0] = "time-nw.nist.gov";
			array[7, 1] = "131.107.1.10";
			array[8, 0] = "nist1.symmetricom.com";
			array[8, 1] = "69.25.96.13";
			array[9, 0] = "nist1-dc.glassey.com";
			array[9, 1] = "216.200.93.8";
			array[10, 0] = "nist1-ny.glassey.com";
			array[10, 1] = "208.184.49.9";
			array[11, 0] = "nist1-sj.glassey.com";
			array[11, 1] = "207.126.98.204";
			array[12, 0] = "nist1.aol-ca.truetime.com";
			array[12, 1] = "207.200.81.113";
			array[13, 0] = "nist1.aol-va.truetime.com";
			array[13, 1] = "64.236.96.53";
			int port = 13;
			byte[] array3 = new byte[1024];
			int count = 0;
			TcpClient tcpClient = new TcpClient();
			for (int i = 0; i < 13; i++)
			{
				string hostname = array[array2[i], 1];
				try
				{
					tcpClient.Connect(hostname, port);
					NetworkStream stream = tcpClient.GetStream();
					count = stream.Read(array3, 0, array3.Length);
					tcpClient.Close();
					break;
				}
				catch (System.Exception)
				{
				}
			}
			char[] separator = new char[]
			{
				' '
			};
			System.DateTime result = default(System.DateTime);
			string @string = System.Text.Encoding.ASCII.GetString(array3, 0, count);
			string[] array4 = @string.Split(separator);
			if (array4.Length >= 2)
			{
				result = System.DateTime.Parse(array4[1] + array4[2]).AddHours(8.0);
			}
			return result;
		}
	}
}
