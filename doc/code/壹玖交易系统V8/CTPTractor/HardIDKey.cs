using ClsDiskInfo;
using System;
using System.Management;

namespace CTPTractor
{
	internal class HardIDKey
	{
		private static byte nHardIndex = 0;

		private static string[] sHardKeys = new string[]
		{
			"",
			"",
			""
		};

		public string GetCPUInfo()
		{
			string result = "";
			ManagementClass managementClass = new ManagementClass("Win32_Processor");
			ManagementObjectCollection instances = managementClass.GetInstances();
			using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = instances.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ManagementObject managementObject = (ManagementObject)enumerator.Current;
					result = managementObject.Properties["ProcessorId"].Value.ToString();
				}
			}
			return result;
		}

		public string GetHardInfo()
		{
			string text = "";
			string result;
			try
			{
				HardDiskInfo hddInfo = ClsDiskInfo.GetHddInfo(HardIDKey.nHardIndex);
				HardIDKey.sHardKeys[(int)HardIDKey.nHardIndex] = hddInfo.SerialNumber;
				HardIDKey.nHardIndex += 1;
				this.GetHardInfo();
			}
			catch
			{
				if (HardIDKey.nHardIndex == 2)
				{
					HardIDKey.sHardKeys[(int)HardIDKey.nHardIndex] = "UNKNOW";
					result = "UNKNOW";
					return result;
				}
				HardIDKey.nHardIndex += 1;
				this.GetHardInfo();
				result = "UNKNOW";
				return result;
			}
			for (int i = 0; i < HardIDKey.sHardKeys.Length; i++)
			{
				if (text.Length < HardIDKey.sHardKeys[i].Length)
				{
					text = HardIDKey.sHardKeys[i];
				}
			}
			string[] array = text.ToString().Split(new char[]
			{
				' ',
				'-'
			}, System.StringSplitOptions.RemoveEmptyEntries);
			text = null;
			string[] array2 = array;
			for (int j = 0; j < array2.Length; j++)
			{
				string str = array2[j];
				text += str;
			}
			HardIDKey.nHardIndex = 0;
			result = text;
			return result;
		}

		public string GetWebInfo()
		{
			ManagementClass managementClass = new ManagementClass("Win32_NetworkAdapterConfiguration");
			ManagementObjectCollection instances = managementClass.GetInstances();
			string text = null;
			using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = instances.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ManagementObject managementObject = (ManagementObject)enumerator.Current;
					if ((bool)managementObject["IPEnabled"])
					{
						string[] array = managementObject["MacAddress"].ToString().Split(new char[]
						{
							':'
						}, System.StringSplitOptions.RemoveEmptyEntries);
						string[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							string str = array2[i];
							text += str;
						}
					}
					managementObject.Dispose();
				}
			}
			return text;
		}

		public string GetOtherInfo()
		{
			string result = string.Empty;
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("select * from Win32_baseboard");
			using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = managementObjectSearcher.Get().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					ManagementObject managementObject = (ManagementObject)enumerator.Current;
					result = managementObject["SerialNumber"].ToString();
				}
			}
			return result;
		}
	}
}
