using System;
using System.Diagnostics;
using System.Reflection;

namespace SugarGuard.Protector.Protections.Runtime
{
	// Token: 0x0200000F RID: 15
	public static class ConstantsRuntime
	{
		// Token: 0x0600003A RID: 58 RVA: 0x00004FA4 File Offset: 0x000031A4
		public static string Decrypt(string s, int key, RuntimeMethodHandle runtimeMethodHandle)
		{
			StackTrace stackTrace = new StackTrace();
			MethodBase method = ((stackTrace != null) ? stackTrace.GetFrame(1) : null).GetMethod();
			bool flag = method != null;
			if (flag)
			{
				bool flag2 = method.Name == "InvokeMethod";
				if (!flag2)
				{
					byte[] array = new byte[4];
					bool flag3 = method.MethodHandle == runtimeMethodHandle || method.Name == ".cctor";
					if (flag3)
					{
						array = MethodBase.GetMethodFromHandle(runtimeMethodHandle).GetMethodBody().GetILAsByteArray();
					}
					bool value = runtimeMethodHandle.Value == method.MethodHandle.Value || method.Name == ".cctor";
					int num = array.Length - Convert.ToInt32(value) * 6;
					int metadataToken = (int)array[num++] | (int)array[num++] << 8 | (int)array[num++] << 16 | (int)array[num++] << 24;
					int num2 = Convert.ToInt32(method.Module.ResolveString(metadataToken));
					key += num2;
					int[] array2 = ConstantsRuntime.tempField;
					for (int i = 0; i < array2.Length; i++)
					{
						key += array2[i];
					}
					char[] array3 = new char[s.Length];
					for (int j = 0; j < s.Length; j++)
					{
						array3[j] = (char)((int)s[j] ^ key);
					}
					return new string(array3);
				}
			}
			return method.Name;
		}

		// Token: 0x04000017 RID: 23
		public static int[] tempField;
	}
}
