using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;

namespace CTPTractor
{
	public class dld
	{
		public enum ModePass
		{
			ByValue = 1,
			ByRef
		}

		public System.IntPtr hModule = System.IntPtr.Zero;

		public System.IntPtr farProc = System.IntPtr.Zero;

		private string _file;

		[System.Runtime.InteropServices.DllImport("kernel32.dll")]
		private static extern System.IntPtr LoadLibrary(string lpFileName);

		[System.Runtime.InteropServices.DllImport("kernel32.dll")]
		private static extern System.IntPtr GetProcAddress(System.IntPtr hModule, string lpProcName);

		[System.Runtime.InteropServices.DllImport("kernel32", SetLastError = true)]
		private static extern bool FreeLibrary(System.IntPtr hModule);

		public void LoadDll(string lpFileName)
		{
			this.hModule = dld.LoadLibrary(lpFileName);
			if (this.hModule == System.IntPtr.Zero)
			{
				throw new System.Exception(" 没有找到 :" + lpFileName + ".");
			}
		}

		public void LoadDll(System.IntPtr HMODULE)
		{
			if (HMODULE == System.IntPtr.Zero)
			{
				throw new System.Exception(" 所传入的函数库模块的句柄 HMODULE 为空 .");
			}
			this.hModule = HMODULE;
		}

		public System.Delegate Invoke(string lpProcName, System.Type t)
		{
			if (this.hModule == System.IntPtr.Zero)
			{
				throw new System.Exception(" 函数库模块的句柄为空 , 请确保已进行 LoadDll 操作 !");
			}
			this.farProc = dld.GetProcAddress(this.hModule, lpProcName);
			if (this.farProc == System.IntPtr.Zero)
			{
				throw new System.Exception(" 没有找到 :" + lpProcName + " 这个函数的入口点 ");
			}
			return System.Runtime.InteropServices.Marshal.GetDelegateForFunctionPointer(this.farProc, t);
		}

		public void LoadFun(string lpProcName)
		{
			if (this.hModule == System.IntPtr.Zero)
			{
				throw new System.Exception(" 函数库模块的句柄为空 , 请确保已进行 LoadDll 操作 !");
			}
			this.farProc = dld.GetProcAddress(this.hModule, lpProcName);
			if (this.farProc == System.IntPtr.Zero)
			{
				throw new System.Exception(" 没有找到 :" + lpProcName + " 这个函数的入口点 ");
			}
		}

		public void LoadFun(string lpFileName, string lpProcName)
		{
			this.hModule = dld.LoadLibrary(lpFileName);
			if (this.hModule == System.IntPtr.Zero)
			{
				throw new System.Exception(" 没有找到 :" + lpFileName + ".");
			}
			this.farProc = dld.GetProcAddress(this.hModule, lpProcName);
			if (this.farProc == System.IntPtr.Zero)
			{
				throw new System.Exception(" 没有找到 :" + lpProcName + " 这个函数的入口点 ");
			}
		}

		public void UnLoadDll()
		{
			try
			{
				dld.FreeLibrary(this.hModule);
				this.hModule = System.IntPtr.Zero;
				this.farProc = System.IntPtr.Zero;
			}
			catch
			{
			}
		}

		public object Invoke(object[] ObjArray_Parameter, System.Type[] TypeArray_ParameterType, dld.ModePass[] ModePassArray_Parameter, System.Type Type_Return)
		{
			if (this.hModule == System.IntPtr.Zero)
			{
				throw new System.Exception(" 函数库模块的句柄为空 , 请确保已进行 LoadDll 操作 !");
			}
			if (this.farProc == System.IntPtr.Zero)
			{
				throw new System.Exception(" 函数指针为空 , 请确保已进行 LoadFun 操作 !");
			}
			if (ObjArray_Parameter.Length != ModePassArray_Parameter.Length)
			{
				throw new System.Exception(" 参数个数及其传递方式的个数不匹配 .");
			}
			System.Reflection.AssemblyName assemblyName = new System.Reflection.AssemblyName();
			assemblyName.Name = "InvokeFun";
			System.Reflection.Emit.AssemblyBuilder assemblyBuilder = System.AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, System.Reflection.Emit.AssemblyBuilderAccess.Run);
			System.Reflection.Emit.ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("InvokeDll");
			System.Reflection.Emit.MethodBuilder methodBuilder = moduleBuilder.DefineGlobalMethod("MyFun", System.Reflection.MethodAttributes.FamANDAssem | System.Reflection.MethodAttributes.Family | System.Reflection.MethodAttributes.Static, Type_Return, TypeArray_ParameterType);
			System.Reflection.Emit.ILGenerator iLGenerator = methodBuilder.GetILGenerator();
			System.Reflection.Emit.LocalBuilder[] array = new System.Reflection.Emit.LocalBuilder[TypeArray_ParameterType.Length];
			for (int i = 0; i < TypeArray_ParameterType.Length; i++)
			{
				array[i] = iLGenerator.DeclareLocal(TypeArray_ParameterType[i], true);
			}
			for (int i = 0; i < ObjArray_Parameter.Length; i++)
			{
				switch (ModePassArray_Parameter[i])
				{
				case dld.ModePass.ByValue:
					iLGenerator.Emit(System.Reflection.Emit.OpCodes.Ldarg, i);
					break;
				case dld.ModePass.ByRef:
					iLGenerator.Emit(System.Reflection.Emit.OpCodes.Ldarga, i);
					break;
				default:
					throw new System.Exception(" 第 " + (i + 1).ToString() + " 个参数没有给定正确的传递方式 .");
				}
			}
			if (System.IntPtr.Size == 4)
			{
				iLGenerator.Emit(System.Reflection.Emit.OpCodes.Ldc_I4, this.farProc.ToInt32());
			}
			else
			{
				if (System.IntPtr.Size != 8)
				{
					throw new System.PlatformNotSupportedException();
				}
				iLGenerator.Emit(System.Reflection.Emit.OpCodes.Ldc_I8, this.farProc.ToInt64());
			}
			iLGenerator.EmitCalli(System.Reflection.Emit.OpCodes.Calli, System.Runtime.InteropServices.CallingConvention.StdCall, Type_Return, TypeArray_ParameterType);
			iLGenerator.Emit(System.Reflection.Emit.OpCodes.Ret);
			moduleBuilder.CreateGlobalFunctions();
			System.Reflection.MethodInfo method = moduleBuilder.GetMethod("MyFun");
			return method.Invoke(null, ObjArray_Parameter);
		}

		public object Invoke(System.IntPtr IntPtr_Function, object[] ObjArray_Parameter, System.Type[] TypeArray_ParameterType, dld.ModePass[] ModePassArray_Parameter, System.Type Type_Return)
		{
			this.hModule == System.IntPtr.Zero;
			bool flag = 1 == 0;
			throw new System.Exception(" 函数库模块的句柄为空 , 请确保已进行 LoadDll 操作 !");
		}

		private void EmitFastInt(System.Reflection.Emit.ILGenerator il, int value)
		{
			switch (value)
			{
			case -1:
				il.Emit(System.Reflection.Emit.OpCodes.Ldc_I4_M1);
				break;
			case 0:
				il.Emit(System.Reflection.Emit.OpCodes.Ldc_I4_0);
				break;
			case 1:
				il.Emit(System.Reflection.Emit.OpCodes.Ldc_I4_1);
				break;
			case 2:
				il.Emit(System.Reflection.Emit.OpCodes.Ldc_I4_2);
				break;
			case 3:
				il.Emit(System.Reflection.Emit.OpCodes.Ldc_I4_3);
				break;
			case 4:
				il.Emit(System.Reflection.Emit.OpCodes.Ldc_I4_4);
				break;
			case 5:
				il.Emit(System.Reflection.Emit.OpCodes.Ldc_I4_5);
				break;
			case 6:
				il.Emit(System.Reflection.Emit.OpCodes.Ldc_I4_6);
				break;
			case 7:
				il.Emit(System.Reflection.Emit.OpCodes.Ldc_I4_7);
				break;
			case 8:
				il.Emit(System.Reflection.Emit.OpCodes.Ldc_I4_8);
				break;
			}
		}
	}
}
