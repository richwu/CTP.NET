using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace CTPTractor
{
	public class IniOP
	{
		private string m_strFilePath = "";

		private string m_strKey = "";

		private string m_strSubKey = "";

		private int m_intSize = 255;

		private string _strFilePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\user.ini";

		private string _strKey = "初始设置";

		private string _strSubKey = "软件名称";

		public string FilePath
		{
			get
			{
				return this.m_strFilePath;
			}
			set
			{
				this.m_strFilePath = value;
			}
		}

		public string Key
		{
			get
			{
				return this.m_strKey;
			}
			set
			{
				this.m_strKey = value;
			}
		}

		public int Size
		{
			get
			{
				return this.m_intSize;
			}
			set
			{
				this.m_intSize = value;
			}
		}

		public string SubKey
		{
			get
			{
				return this.m_strSubKey;
			}
			set
			{
				this.m_strSubKey = value;
			}
		}

		public static string Version
		{
			get
			{
				return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
			}
		}

		[System.Runtime.InteropServices.DllImport("kernel32")]
		private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

		[System.Runtime.InteropServices.DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section, string key, string def, System.Text.StringBuilder retVal, int size, string filePath);

		public IniOP()
		{
		}

		public IniOP(string strPath)
		{
			this.m_strFilePath = strPath;
		}

		public IniOP(string strPath, string strKey)
		{
			this.m_strFilePath = strPath;
			this.m_strKey = strKey;
		}

		public IniOP(string strPath, string strKey, int intSize)
		{
			this.m_strFilePath = strPath;
			this.m_strKey = strKey;
			this.m_intSize = intSize;
		}

		public IniOP(string strPath, string strKey, int intSize, string strSubKey)
		{
			this.m_strFilePath = strPath;
			this.m_strKey = strKey;
			this.m_intSize = intSize;
			this.m_strSubKey = strSubKey;
		}

		public void IniCheckDefaultValue()
		{
			if (this.m_strFilePath == "" || !System.IO.File.Exists(this.m_strFilePath))
			{
				this._strFilePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\user.ini";
				this.WriteValue("设置", "文件路径", "");
				this.WriteValue("设置", "校对码", "");
			}
			else
			{
				this._strFilePath = this.m_strFilePath;
			}
			if (this.m_strKey == "")
			{
				this._strKey = "ErrorKeyIsNull_" + System.DateTime.Now.ToString("yyyyMMddHHmmss");
			}
			else
			{
				this._strKey = this.m_strKey;
			}
			if (this.m_strSubKey == "")
			{
				this._strSubKey = "ErrorSubKeyIsNull_" + System.DateTime.Now.ToString("yyyyMMddHHmmss");
			}
			else
			{
				this._strSubKey = this.m_strSubKey;
			}
		}

		public void WriteValue(string strValue)
		{
			this.IniCheckDefaultValue();
			IniOP.WritePrivateProfileString(this._strKey, this._strSubKey, strValue, this._strFilePath);
		}

		public void WriteValue(string strSubKey, string strValue)
		{
			this.IniCheckDefaultValue();
			IniOP.WritePrivateProfileString(this._strKey, strSubKey, strValue, this._strFilePath);
		}

		public void WriteValue(string strKey, string strSubKey, string strValue)
		{
			IniOP.WritePrivateProfileString(strKey, strSubKey, strValue, this._strFilePath);
		}

		public void WriteValue(string strKey, string strSubKey, string strPath, string strValue)
		{
			IniOP.WritePrivateProfileString(strKey, strSubKey, strValue, strPath);
		}

		public static void staticWriteValue(string strKey, string strSubKey, string strPath, string strValue)
		{
			IniOP.WritePrivateProfileString(strKey, strSubKey, strValue, strPath);
		}

		public string ReadValue()
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder(this.m_intSize);
			int privateProfileString = IniOP.GetPrivateProfileString(this.m_strKey, this.m_strSubKey, "", stringBuilder, this.m_intSize, this.m_strFilePath);
			return stringBuilder.ToString();
		}

		public string ReadValue(string strSubKey)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder(this.m_intSize);
			int privateProfileString = IniOP.GetPrivateProfileString(this.m_strKey, strSubKey, "", stringBuilder, this.m_intSize, this.m_strFilePath);
			return stringBuilder.ToString();
		}

		public string ReadValue(string strKey, string strSubKey)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder(this.m_intSize);
			int privateProfileString = IniOP.GetPrivateProfileString(strKey, strSubKey, "", stringBuilder, this.m_intSize, this.m_strFilePath);
			return stringBuilder.ToString();
		}

		public string ReadValue(string strKey, string strSubKey, string strPath)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder(this.m_intSize);
			int privateProfileString = IniOP.GetPrivateProfileString(strKey, strSubKey, "", stringBuilder, this.m_intSize, strPath);
			return stringBuilder.ToString();
		}

		public string ReadValue(string strKey, string strSubKey, int intSize)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder(intSize);
			int privateProfileString = IniOP.GetPrivateProfileString(strKey, strSubKey, "", stringBuilder, intSize, this.m_strFilePath);
			return stringBuilder.ToString();
		}

		public string ReadValue(string strKey, string strSubKey, string strPath, int intSize)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder(intSize);
			int privateProfileString = IniOP.GetPrivateProfileString(strKey, strSubKey, "", stringBuilder, intSize, strPath);
			return stringBuilder.ToString();
		}

		public static string staticReadValue(string strKey, string strSubKey, string strPath, int intSize)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder(intSize);
			int privateProfileString = IniOP.GetPrivateProfileString(strKey, strSubKey, "", stringBuilder, intSize, strPath);
			return stringBuilder.ToString();
		}
	}
}
