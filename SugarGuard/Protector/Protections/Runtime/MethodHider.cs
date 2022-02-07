using System;
using System.Linq;
using System.Reflection;

namespace SugarGuard.Protector.Protections.Runtime
{
	// Token: 0x02000010 RID: 16
	internal static class MethodHider
	{
		// Token: 0x0600003B RID: 59 RVA: 0x00005140 File Offset: 0x00003340
		public static void MethodHiderInj(object[] parameters, int token)
		{
			parameters.Reverse<object>();
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			Assembly callingAssembly = Assembly.GetCallingAssembly();
			bool flag = executingAssembly == callingAssembly;
			if (flag)
			{
				Module manifestModule = executingAssembly.ManifestModule;
				MethodInfo methodInfo = (MethodInfo)manifestModule.ResolveMethod(token);
				methodInfo.Invoke(null, parameters);
			}
		}
	}
}
