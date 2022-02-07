using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace SugarGuard.Protector.Class.Constants
{
	// Token: 0x02000036 RID: 54
	internal static class DynamicMethods
	{
		// Token: 0x0600011D RID: 285 RVA: 0x0000E0C0 File Offset: 0x0000C2C0
		internal static MethodInfo GetCreatedMethodInfo(DynamicMethod method)
		{
			bool flag = DynamicMethods._methods == null;
			if (flag)
			{
				DynamicMethods._methods = new Dictionary<string, MethodInfo>();
			}
			bool flag2 = DynamicMethods._methods.ContainsKey(method.Name);
			MethodInfo result;
			if (flag2)
			{
				result = DynamicMethods._methods[method.Name];
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600011E RID: 286 RVA: 0x0000E110 File Offset: 0x0000C310
		internal static void SetMethodInfo(DynamicMethod method, MethodInfo methodInfo)
		{
			bool flag = !DynamicMethods._methods.ContainsKey(method.Name);
			if (flag)
			{
				DynamicMethods._methods.Add(method.Name, methodInfo);
			}
			else
			{
				DynamicMethods._methods[method.Name] = methodInfo;
			}
		}

		// Token: 0x0600011F RID: 287 RVA: 0x0000E15C File Offset: 0x0000C35C
		internal static MethodInfo GetMethod(Type ownerType, string name, Type[] parameters)
		{
			MethodInfo methodInfo = ownerType.GetMethod(name, parameters);
			bool flag = methodInfo == null;
			if (flag)
			{
				foreach (MethodInfo methodInfo2 in ownerType.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
				{
					bool flag2 = methodInfo2.Name == name && DynamicMethods.HasSameParamSig(parameters, methodInfo2.GetParameters());
					if (flag2)
					{
						methodInfo = methodInfo2;
						break;
					}
				}
			}
			bool isGenericMethod = methodInfo.IsGenericMethod;
			MethodInfo result;
			if (isGenericMethod)
			{
				result = methodInfo.GetGenericMethodDefinition();
			}
			else
			{
				result = methodInfo;
			}
			return result;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x0000E1E8 File Offset: 0x0000C3E8
		internal static ConstructorInfo GetConstructor(Type ownerType, Type[] parameters)
		{
			ConstructorInfo constructorInfo = ownerType.GetConstructor(parameters);
			bool flag = constructorInfo == null;
			if (flag)
			{
				foreach (ConstructorInfo constructorInfo2 in ownerType.GetConstructors())
				{
					bool flag2 = DynamicMethods.HasSameParamSig(parameters, constructorInfo2.GetParameters());
					if (flag2)
					{
						constructorInfo = constructorInfo2;
						break;
					}
				}
			}
			return constructorInfo;
		}

		// Token: 0x06000121 RID: 289 RVA: 0x0000E248 File Offset: 0x0000C448
		private static bool HasSameParamSig(Type[] fParameters, ParameterInfo[] sParameters)
		{
			bool flag = fParameters.Length != sParameters.Length;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < fParameters.Length; i++)
				{
					bool flag2 = fParameters[i] != sParameters[i].ParameterType;
					if (flag2)
					{
						return false;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x04000076 RID: 118
		private static Dictionary<string, MethodInfo> _methods;
	}
}
