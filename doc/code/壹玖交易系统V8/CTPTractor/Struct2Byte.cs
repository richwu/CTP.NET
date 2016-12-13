using System;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public class Struct2Byte
	{
		public static byte[] StructToBytes(object structObj)
		{
			int num = System.Runtime.InteropServices.Marshal.SizeOf(structObj);
			System.IntPtr intPtr = System.Runtime.InteropServices.Marshal.AllocHGlobal(num);
			byte[] result;
			try
			{
				System.Runtime.InteropServices.Marshal.StructureToPtr(structObj, intPtr, false);
				byte[] array = new byte[num];
				System.Runtime.InteropServices.Marshal.Copy(intPtr, array, 0, num);
				result = array;
			}
			finally
			{
				System.Runtime.InteropServices.Marshal.FreeHGlobal(intPtr);
			}
			return result;
		}

		public static object BytesToStruct(byte[] bytes, System.Type strcutType)
		{
			int num = System.Runtime.InteropServices.Marshal.SizeOf(strcutType);
			System.IntPtr intPtr = System.Runtime.InteropServices.Marshal.AllocHGlobal(num);
			object result;
			try
			{
				System.Runtime.InteropServices.Marshal.Copy(bytes, 0, intPtr, num);
				result = System.Runtime.InteropServices.Marshal.PtrToStructure(intPtr, strcutType);
			}
			finally
			{
				System.Runtime.InteropServices.Marshal.FreeHGlobal(intPtr);
			}
			return result;
		}
	}
}
