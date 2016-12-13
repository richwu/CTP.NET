using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ClsDiskInfo
{
	public class ClsDiskInfo
	{
		private const uint DFP_GET_VERSION = 475264u;

		private const uint DFP_SEND_DRIVE_COMMAND = 508036u;

		private const uint DFP_RECEIVE_DRIVE_DATA = 508040u;

		private const uint GENERIC_READ = 2147483648u;

		private const uint GENERIC_WRITE = 1073741824u;

		private const uint FILE_SHARE_READ = 1u;

		private const uint FILE_SHARE_WRITE = 2u;

		private const uint CREATE_NEW = 1u;

		private const uint OPEN_EXISTING = 3u;

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern int CloseHandle(IntPtr hObject);

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

		[DllImport("kernel32.dll")]
		private static extern int DeviceIoControl(IntPtr hDevice, uint dwIoControlCode, IntPtr lpInBuffer, uint nInBufferSize, ref GetVersionOutParams lpOutBuffer, uint nOutBufferSize, ref uint lpBytesReturned, [Out] IntPtr lpOverlapped);

		[DllImport("kernel32.dll")]
		private static extern int DeviceIoControl(IntPtr hDevice, uint dwIoControlCode, ref SendCmdInParams lpInBuffer, uint nInBufferSize, ref SendCmdOutParams lpOutBuffer, uint nOutBufferSize, ref uint lpBytesReturned, [Out] IntPtr lpOverlapped);

		public static HardDiskInfo GetHddInfo(byte driveIndex)
		{
			switch (Environment.OSVersion.Platform)
			{
			case PlatformID.Win32S:
				throw new NotSupportedException("Win32s   is   not   supported.");
			case PlatformID.Win32Windows:
				return ClsDiskInfo.GetHddInfo9x(driveIndex);
			case PlatformID.Win32NT:
				return ClsDiskInfo.GetHddInfoNT(driveIndex);
			case PlatformID.WinCE:
				throw new NotSupportedException("WinCE   is   not   supported.");
			default:
				throw new NotSupportedException("Unknown   Platform.");
			}
		}

		private static HardDiskInfo GetHddInfo9x(byte driveIndex)
		{
			GetVersionOutParams getVersionOutParams = default(GetVersionOutParams);
			SendCmdInParams sendCmdInParams = default(SendCmdInParams);
			SendCmdOutParams sendCmdOutParams = default(SendCmdOutParams);
			uint num = 0u;
			IntPtr intPtr = ClsDiskInfo.CreateFile("\\\\.\\Smartvsd", 0u, 0u, IntPtr.Zero, 1u, 0u, IntPtr.Zero);
			if (intPtr == IntPtr.Zero)
			{
				throw new Exception("Open   smartvsd.vxd   failed.");
			}
			if (ClsDiskInfo.DeviceIoControl(intPtr, 475264u, IntPtr.Zero, 0u, ref getVersionOutParams, (uint)Marshal.SizeOf(getVersionOutParams), ref num, IntPtr.Zero) == 0)
			{
				ClsDiskInfo.CloseHandle(intPtr);
				throw new Exception("DeviceIoControl   failed:DFP_GET_VERSION");
			}
			if ((getVersionOutParams.fCapabilities & 1u) == 0u)
			{
				ClsDiskInfo.CloseHandle(intPtr);
				throw new Exception("Error:   IDE   identify   command   not   supported.");
			}
			if ((driveIndex & 1) != 0)
			{
				sendCmdInParams.irDriveRegs.bDriveHeadReg = 176;
			}
			else
			{
				sendCmdInParams.irDriveRegs.bDriveHeadReg = 160;
			}
			if (0uL != ((ulong)getVersionOutParams.fCapabilities & (ulong)((long)(16 >> (int)driveIndex))))
			{
				ClsDiskInfo.CloseHandle(intPtr);
				throw new Exception(string.Format("Drive   {0}   is   a   ATAPI   device,   we   don't   detect   it", (int)(driveIndex + 1)));
			}
			sendCmdInParams.irDriveRegs.bCommandReg = 236;
			sendCmdInParams.bDriveNumber = driveIndex;
			sendCmdInParams.irDriveRegs.bSectorCountReg = 1;
			sendCmdInParams.irDriveRegs.bSectorNumberReg = 1;
			sendCmdInParams.cBufferSize = 512u;
			if (ClsDiskInfo.DeviceIoControl(intPtr, 508040u, ref sendCmdInParams, (uint)Marshal.SizeOf(sendCmdInParams), ref sendCmdOutParams, (uint)Marshal.SizeOf(sendCmdOutParams), ref num, IntPtr.Zero) == 0)
			{
				ClsDiskInfo.CloseHandle(intPtr);
				throw new Exception("DeviceIoControl   failed:   DFP_RECEIVE_DRIVE_DATA");
			}
			ClsDiskInfo.CloseHandle(intPtr);
			return ClsDiskInfo.GetHardDiskInfo(sendCmdOutParams.bBuffer);
		}

		private static HardDiskInfo GetHddInfoNT(byte driveIndex)
		{
			GetVersionOutParams getVersionOutParams = default(GetVersionOutParams);
			SendCmdInParams sendCmdInParams = default(SendCmdInParams);
			SendCmdOutParams sendCmdOutParams = default(SendCmdOutParams);
			uint num = 0u;
			IntPtr intPtr = ClsDiskInfo.CreateFile(string.Format("\\\\.\\PhysicalDrive{0}", driveIndex), 3221225472u, 3u, IntPtr.Zero, 3u, 0u, IntPtr.Zero);
			if (intPtr == IntPtr.Zero)
			{
				throw new Exception("CreateFile   faild.");
			}
			if (ClsDiskInfo.DeviceIoControl(intPtr, 475264u, IntPtr.Zero, 0u, ref getVersionOutParams, (uint)Marshal.SizeOf(getVersionOutParams), ref num, IntPtr.Zero) == 0)
			{
				ClsDiskInfo.CloseHandle(intPtr);
				throw new Exception(string.Format("Drive   {0}   may   not   exists.", (int)(driveIndex + 1)));
			}
			if ((getVersionOutParams.fCapabilities & 1u) == 0u)
			{
				ClsDiskInfo.CloseHandle(intPtr);
				throw new Exception("Error:   IDE   identify   command   not   supported.");
			}
			if ((driveIndex & 1) != 0)
			{
				sendCmdInParams.irDriveRegs.bDriveHeadReg = 176;
			}
			else
			{
				sendCmdInParams.irDriveRegs.bDriveHeadReg = 160;
			}
			if (0uL != ((ulong)getVersionOutParams.fCapabilities & (ulong)((long)(16 >> (int)driveIndex))))
			{
				ClsDiskInfo.CloseHandle(intPtr);
				throw new Exception(string.Format("Drive   {0}   is   a   ATAPI   device,   we   don't   detect   it.", (int)(driveIndex + 1)));
			}
			sendCmdInParams.irDriveRegs.bCommandReg = 236;
			sendCmdInParams.bDriveNumber = driveIndex;
			sendCmdInParams.irDriveRegs.bSectorCountReg = 1;
			sendCmdInParams.irDriveRegs.bSectorNumberReg = 1;
			sendCmdInParams.cBufferSize = 512u;
			if (ClsDiskInfo.DeviceIoControl(intPtr, 508040u, ref sendCmdInParams, (uint)Marshal.SizeOf(sendCmdInParams), ref sendCmdOutParams, (uint)Marshal.SizeOf(sendCmdOutParams), ref num, IntPtr.Zero) == 0)
			{
				ClsDiskInfo.CloseHandle(intPtr);
				throw new Exception("DeviceIoControl   failed:   DFP_RECEIVE_DRIVE_DATA");
			}
			ClsDiskInfo.CloseHandle(intPtr);
			return ClsDiskInfo.GetHardDiskInfo(sendCmdOutParams.bBuffer);
		}

		private static HardDiskInfo GetHardDiskInfo(IdSector phdinfo)
		{
			HardDiskInfo result = default(HardDiskInfo);
			ClsDiskInfo.ChangeByteOrder(phdinfo.sModelNumber);
			result.ModuleNumber = Encoding.ASCII.GetString(phdinfo.sModelNumber).Trim();
			ClsDiskInfo.ChangeByteOrder(phdinfo.sFirmwareRev);
			result.Firmware = Encoding.ASCII.GetString(phdinfo.sFirmwareRev).Trim();
			ClsDiskInfo.ChangeByteOrder(phdinfo.sSerialNumber);
			result.SerialNumber = Encoding.ASCII.GetString(phdinfo.sSerialNumber).Trim();
			result.Capacity = phdinfo.ulTotalAddressableSectors / 2u / 1024u;
			return result;
		}

		private static void ChangeByteOrder(byte[] charArray)
		{
			for (int i = 0; i < charArray.Length; i += 2)
			{
				byte b = charArray[i];
				charArray[i] = charArray[i + 1];
				charArray[i + 1] = b;
			}
		}
	}
}
