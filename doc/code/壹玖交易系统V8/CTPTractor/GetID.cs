using System;
using System.IO;
using System.Management;
using System.Windows.Forms;

namespace CTPTractor
{
	internal class GetID
	{
		public int[] intCode = new int[127];

		public int[] intNumber = new int[25];

		public char[] Charcode = new char[25];

		public static string getinfo()
		{
			string result = "";
			ManagementClass managementClass = new ManagementClass("Win32_Processor");
			ManagementObjectCollection instances = managementClass.GetInstances();
			using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = instances.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ManagementObject managementObject = (ManagementObject)enumerator.Current;
					string text = managementObject.Properties["ProcessorId"].Value.ToString();
					result = text;
				}
			}
			return result;
		}

		public string GetDiskVolumeSerialNumber()
		{
			ManagementClass managementClass = new ManagementClass("Win32_NetworkAdapterConfiguration");
			ManagementObject managementObject = new ManagementObject("win32_logicaldisk.deviceid=\"c:\"");
			managementObject.Get();
			return managementObject.GetPropertyValue("VolumeSerialNumber").ToString();
		}

		public string getMNum()
		{
			string text = GetID.getinfo() + this.GetDiskVolumeSerialNumber();
			return text.Substring(0, 24);
		}

		public void setIntCode()
		{
			for (int i = 1; i < this.intCode.Length; i++)
			{
				this.intCode[i] = i % 9;
			}
		}

		public string getRNum()
		{
			this.setIntCode();
			string mNum = this.getMNum();
			for (int i = 1; i < this.Charcode.Length; i++)
			{
				this.Charcode[i] = System.Convert.ToChar(mNum.Substring(i - 1, 1));
			}
			for (int j = 1; j < this.intNumber.Length; j++)
			{
				this.intNumber[j] = this.intCode[System.Convert.ToInt32(this.Charcode[j])] + System.Convert.ToInt32(this.Charcode[j]);
			}
			string text = "";
			for (int j = 1; j < this.intNumber.Length; j++)
			{
				if (this.intNumber[j] >= 48 && this.intNumber[j] <= 57)
				{
					text += System.Convert.ToChar(this.intNumber[j]).ToString();
				}
				else if (this.intNumber[j] >= 65 && this.intNumber[j] <= 90)
				{
					text += System.Convert.ToChar(this.intNumber[j]).ToString();
				}
				else if (this.intNumber[j] >= 97 && this.intNumber[j] <= 122)
				{
					text += System.Convert.ToChar(this.intNumber[j]).ToString();
				}
				else if (this.intNumber[j] > 122)
				{
					text += System.Convert.ToChar(this.intNumber[j] - 10).ToString();
				}
				else
				{
					text += System.Convert.ToChar(this.intNumber[j] - 9).ToString();
				}
			}
			return text;
		}

		public string getPW(string num)
		{
			string text = num + this.GetDiskVolumeSerialNumber();
			this.setIntCode();
			char[] array = new char[text.Length];
			int[] array2 = new int[text.Length];
			for (int i = 1; i < array.Length; i++)
			{
				array[i] = System.Convert.ToChar(text.Substring(i - 1, 1));
			}
			for (int j = 1; j < array2.Length; j++)
			{
				array2[j] = this.intCode[System.Convert.ToInt32(array[j])] + System.Convert.ToInt32(array[j]);
			}
			string text2 = "";
			for (int j = 1; j < array2.Length; j++)
			{
				if (array2[j] >= 48 && array2[j] <= 57)
				{
					text2 += System.Convert.ToChar(array2[j]).ToString();
				}
				else if (array2[j] >= 65 && array2[j] <= 90)
				{
					text2 += System.Convert.ToChar(array2[j]).ToString();
				}
				else if (array2[j] >= 97 && array2[j] <= 122)
				{
					text2 += System.Convert.ToChar(array2[j]).ToString();
				}
				else if (array2[j] > 122)
				{
					text2 += System.Convert.ToChar(array2[j] - 10).ToString();
				}
				else
				{
					text2 += System.Convert.ToChar(array2[j] - 9).ToString();
				}
			}
			return text2;
		}

		public string[] ValidateProgram()
		{
			string[] array = new string[]
			{
				"",
				"",
				""
			};
			string text = "";
			string text2 = System.Windows.Forms.Application.ExecutablePath.Substring(0, System.Windows.Forms.Application.ExecutablePath.LastIndexOf("\\")) + "\\License.dat";
			string[] result;
			try
			{
				text = this.getMNum();
			}
			catch (System.Exception ex)
			{
				array[0] = "序列号读取出错";
				result = array;
				return result;
			}
			try
			{
				while (text.Length < 15)
				{
					text += "S";
				}
				if (System.IO.File.Exists(text2))
				{
					IniOP iniOP = new IniOP(text2);
					string text3 = iniOP.ReadValue("CTP", "SN");
					Encrypt encrypt = new Encrypt();
					if (text3 != "")
					{
						string text4 = encrypt.ToDecrypt("HyIv", text3);
						int num = text4.IndexOf(",");
						string text5 = text4.Substring(0, num);
						if (text5.Substring(1, 10) != text.Substring(3, 10))
						{
							array[0] = "注册机硬件更改";
							array[1] = text;
						}
						int num2 = int.Parse(text4.Substring(17 + num, text4.Length - 17 - num));
						if (num2 < 0)
						{
							array[0] = "软件已过期";
							array[1] = text;
						}
						string text6 = text4.Substring(num + 1, 8);
						string s = string.Concat(new string[]
						{
							text6.Substring(0, 4),
							"-",
							text6.Substring(4, 2),
							"-",
							text6.Substring(6, 2)
						});
						System.DateTime now = System.DateTime.Now;
						if (now > System.DateTime.Parse(s))
						{
							array[0] = "软件已过期";
							array[1] = text;
						}
					}
					else
					{
						array[0] = "软件注册码异常";
						array[1] = text;
					}
				}
				else
				{
					array[0] = "软件未注册";
					array[1] = text;
				}
				result = array;
			}
			catch (System.Exception ex)
			{
				array[0] = "出错";
				array[1] = text;
				array[2] = ex.StackTrace;
				result = array;
			}
			return result;
		}
	}
}
