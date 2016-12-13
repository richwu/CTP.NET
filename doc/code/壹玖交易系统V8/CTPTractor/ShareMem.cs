using System;
using System.Runtime.InteropServices;
using System.Text;

namespace CTPTractor
{
	public class ShareMem
	{
		private const int ERROR_ALREADY_EXISTS = 183;

		private const int FILE_MAP_COPY = 1;

		private const int FILE_MAP_WRITE = 2;

		private const int FILE_MAP_READ = 4;

		private const int FILE_MAP_ALL_ACCESS = 6;

		private const int PAGE_READONLY = 2;

		private const int PAGE_READWRITE = 4;

		private const int PAGE_WRITECOPY = 8;

		private const int PAGE_EXECUTE = 16;

		private const int PAGE_EXECUTE_READ = 32;

		private const int PAGE_EXECUTE_READWRITE = 64;

		private const int SEC_COMMIT = 134217728;

		private const int SEC_IMAGE = 16777216;

		private const int SEC_NOCACHE = 268435456;

		private const int SEC_RESERVE = 67108864;

		private const int INVALID_HANDLE_VALUE = -1;

		private System.IntPtr m_hSharedMemoryFile = System.IntPtr.Zero;

		private System.IntPtr m_pwData = System.IntPtr.Zero;

		private bool m_bAlreadyExist = false;

		private bool m_bInit = false;

		private long m_MemSize = 0L;

		[System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern System.IntPtr SendMessage(System.IntPtr hWnd, int Msg, int wParam, System.IntPtr lParam);

		[System.Runtime.InteropServices.DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
		public static extern System.IntPtr CreateFileMapping(int hFile, System.IntPtr lpAttributes, uint flProtect, uint dwMaxSizeHi, uint dwMaxSizeLow, string lpName);

		[System.Runtime.InteropServices.DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
		public static extern System.IntPtr OpenFileMapping(int dwDesiredAccess, [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)] bool bInheritHandle, string lpName);

		[System.Runtime.InteropServices.DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
		public static extern System.IntPtr MapViewOfFile(System.IntPtr hFileMapping, uint dwDesiredAccess, uint dwFileOffsetHigh, uint dwFileOffsetLow, uint dwNumberOfBytesToMap);

		[System.Runtime.InteropServices.DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
		public static extern bool UnmapViewOfFile(System.IntPtr pvBaseAddress);

		[System.Runtime.InteropServices.DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
		public static extern bool CloseHandle(System.IntPtr handle);

		[System.Runtime.InteropServices.DllImport("kernel32")]
		public static extern int GetLastError();

		public ShareMem(string strName, int lngSize)
		{
			this.Init(strName, lngSize);
		}

		~ShareMem()
		{
			this.Close();
		}

		private int Init(string strName, int lngSize)
		{
			if (lngSize <= 0 || lngSize > 8388608)
			{
				lngSize = 8388608;
			}
			this.m_MemSize = (long)lngSize;
			int result;
			if (strName.Length > 0)
			{
				this.m_hSharedMemoryFile = ShareMem.CreateFileMapping(-1, System.IntPtr.Zero, 4u, 0u, (uint)lngSize, strName);
				if (this.m_hSharedMemoryFile == System.IntPtr.Zero)
				{
					this.m_bAlreadyExist = false;
					this.m_bInit = false;
					result = 2;
				}
				else
				{
					if (ShareMem.GetLastError() == 183)
					{
						this.m_bAlreadyExist = true;
					}
					else
					{
						this.m_bAlreadyExist = false;
					}
					this.m_pwData = ShareMem.MapViewOfFile(this.m_hSharedMemoryFile, 2u, 0u, 0u, (uint)lngSize);
					if (this.m_pwData == System.IntPtr.Zero)
					{
						this.m_bInit = false;
						ShareMem.CloseHandle(this.m_hSharedMemoryFile);
						result = 3;
					}
					else
					{
						this.m_bInit = true;
						if (!this.m_bAlreadyExist)
						{
						}
						result = 0;
					}
				}
			}
			else
			{
				result = 1;
			}
			return result;
		}

		public void Close()
		{
			if (this.m_bInit)
			{
				this.Write(new byte[this.m_MemSize], 0, (int)this.m_MemSize);
				ShareMem.UnmapViewOfFile(this.m_pwData);
				ShareMem.CloseHandle(this.m_hSharedMemoryFile);
			}
		}

		public int Read(ref byte[] bytData, int lngAddr, int lngSize)
		{
			int result;
			if ((long)(lngAddr + lngSize) > this.m_MemSize)
			{
				result = 2;
			}
			else if (this.m_bInit)
			{
				System.Runtime.InteropServices.Marshal.Copy(this.m_pwData, bytData, lngAddr, lngSize);
				result = 0;
			}
			else
			{
				result = 1;
			}
			return result;
		}

		public string Read()
		{
			byte[] bytes = new byte[this.m_MemSize];
			this.Read(ref bytes, 0, (int)this.m_MemSize);
			string arg_32_0 = System.Text.Encoding.ASCII.GetString(bytes);
			char[] trimChars = new char[1];
			return arg_32_0.TrimEnd(trimChars);
		}

		public int Write(byte[] bytData, int lngAddr, int lngSize)
		{
			int result;
			if ((long)(lngAddr + lngSize) > this.m_MemSize)
			{
				result = 2;
			}
			else if (this.m_bInit)
			{
				System.Runtime.InteropServices.Marshal.Copy(bytData, lngAddr, this.m_pwData, lngSize);
				result = 0;
			}
			else
			{
				result = 1;
			}
			return result;
		}

		public int Write(byte[] btyData)
		{
			return this.Write(btyData, 0, btyData.Length);
		}

		public int Write(string strData)
		{
			byte[] bytes = System.Text.Encoding.ASCII.GetBytes(strData);
			this.Write(new byte[this.m_MemSize], 0, (int)this.m_MemSize);
			return this.Write(bytes, 0, bytes.Length);
		}
	}
}
