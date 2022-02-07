using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace SugarGuard.Protector.Protections.Runtime
{
	// Token: 0x0200000D RID: 13
	internal static class AntiDebug
	{
		// Token: 0x0600002D RID: 45
		[DllImport("kernel32.dll")]
		internal static extern int CloseHandle(IntPtr hModule);

		// Token: 0x0600002E RID: 46
		[DllImport("kernel32.dll")]
		internal static extern IntPtr OpenProcess(uint hModule, int procName, uint procId);

		// Token: 0x0600002F RID: 47
		[DllImport("kernel32.dll")]
		internal static extern uint GetCurrentProcessId();

		// Token: 0x06000030 RID: 48
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern IntPtr LoadLibrary(string hModule);

		// Token: 0x06000031 RID: 49
		[DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
		internal static extern AntiDebug.GetProcA GetProcAddress(IntPtr hModule, string procName);

		// Token: 0x06000032 RID: 50
		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "GetProcAddress")]
		internal static extern AntiDebug.GetProcA2 GetProcAddress_2(IntPtr hModule, string procName);

		// Token: 0x06000033 RID: 51
		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, EntryPoint = "GetProcAddress")]
		internal static extern AntiDebug.GetProcA3 GetProcAddress_3(IntPtr hModule, string procName);

		// Token: 0x06000034 RID: 52
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		internal static extern int GetClassName(IntPtr hModule, StringBuilder procName, int procId);

		// Token: 0x06000035 RID: 53 RVA: 0x00004D18 File Offset: 0x00002F18
		internal static string SplitMenuItem(IntPtr hModule)
		{
			StringBuilder stringBuilder = new StringBuilder(260);
			AntiDebug.GetClassName(hModule, stringBuilder, stringBuilder.Capacity);
			return stringBuilder.ToString();
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00004D4C File Offset: 0x00002F4C
		private static void Initialize()
		{
			bool flag = AntiDebug.Detected();
			if (flag)
			{
				throw new Exception(string.Format("Debugger was found! - This software cannot be executed under the debugger.", Array.Empty<object>()));
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00004D78 File Offset: 0x00002F78
		internal static bool Detected()
		{
			try
			{
				IntPtr hModule = AntiDebug.LoadLibrary("kernel32.dll");
				bool isAttached = Debugger.IsAttached;
				if (isAttached)
				{
					return true;
				}
				AntiDebug.GetProcA procAddress = AntiDebug.GetProcAddress(hModule, "IsDebuggerPresent");
				bool flag = procAddress != null && procAddress() != 0;
				if (flag)
				{
					return true;
				}
				IntPtr intPtr = AntiDebug.OpenProcess(1024U, 0, AntiDebug.GetCurrentProcessId());
				bool flag2 = intPtr != IntPtr.Zero;
				if (flag2)
				{
					try
					{
						AntiDebug.GetProcA2 procAddress_ = AntiDebug.GetProcAddress_2(hModule, "CheckRemoteDebuggerPresent");
						bool flag3 = procAddress_ != null;
						if (flag3)
						{
							int num = 0;
							bool flag4 = procAddress_(intPtr, ref num) != 0;
							if (flag4)
							{
								bool flag5 = num != 0;
								if (flag5)
								{
									return true;
								}
							}
						}
					}
					finally
					{
						AntiDebug.CloseHandle(intPtr);
					}
				}
				bool flag6 = false;
				try
				{
					AntiDebug.CloseHandle(new IntPtr(305419896));
				}
				catch
				{
					flag6 = true;
				}
				bool flag7 = flag6;
				if (flag7)
				{
					return true;
				}
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x0200003E RID: 62
		// (Invoke) Token: 0x0600013A RID: 314
		internal delegate int GetProcA();

		// Token: 0x0200003F RID: 63
		// (Invoke) Token: 0x0600013E RID: 318
		internal delegate int GetProcA2(IntPtr hProcess, ref int pbDebuggerPresent);

		// Token: 0x02000040 RID: 64
		// (Invoke) Token: 0x06000142 RID: 322
		internal delegate int WL(IntPtr wnd, IntPtr lParam);

		// Token: 0x02000041 RID: 65
		// (Invoke) Token: 0x06000146 RID: 326
		internal delegate int GetProcA3(AntiDebug.WL lpEnumFunc, IntPtr lParam);
	}
}
