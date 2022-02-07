using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace SugarGuard.Protector.Protections.Runtime
{
	// Token: 0x0200000E RID: 14
	internal static class AntiDump
	{
		// Token: 0x06000038 RID: 56
		[DllImport("kernel32.dll")]
		private unsafe static extern bool VirtualProtect(byte* lpAddress, int dwSize, uint flNewProtect, out uint lpflOldProtect);

		// Token: 0x06000039 RID: 57 RVA: 0x00004EA0 File Offset: 0x000030A0
		private unsafe static void AntiDumpInj()
		{
			Module module = typeof(AntiDump).Module;
			byte* ptr = (byte*)((void*)Marshal.GetHINSTANCE(module));
			byte* ptr2 = ptr + 60;
			ptr2 = ptr + *(uint*)ptr2;
			ptr2 += 6;
			ushort num = *(ushort*)ptr2;
			ptr2 += 14;
			ushort num2 = *(ushort*)ptr2;
			ptr2 = ptr2 + 4 + num2;
			byte* ptr3 = stackalloc byte[(UIntPtr)11];
			uint num3;
			AntiDump.VirtualProtect(ptr2 - 16, 8, 64U, out num3);
			*(int*)(ptr2 - 12) = 0;
			byte* ptr4 = ptr + *(uint*)(ptr2 - 16);
			*(int*)(ptr2 - 16) = 0;
			AntiDump.VirtualProtect(ptr4, 72, 64U, out num3);
			byte* ptr5 = ptr + *(uint*)(ptr4 + 8);
			*(int*)ptr4 = 0;
			*(int*)(ptr4 + 4) = 0;
			*(int*)(ptr4 + (IntPtr)2 * 4) = 0;
			*(int*)(ptr4 + (IntPtr)3 * 4) = 0;
			AntiDump.VirtualProtect(ptr5, 4, 64U, out num3);
			*(int*)ptr5 = 0;
			for (int i = 0; i < (int)num; i++)
			{
				AntiDump.VirtualProtect(ptr2, 8, 64U, out num3);
				Marshal.Copy(new byte[8], 0, (IntPtr)((void*)ptr2), 8);
				ptr2 += 40;
			}
		}
	}
}
