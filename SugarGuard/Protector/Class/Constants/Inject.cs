using System;
using System.Collections.Generic;
using dnlib.DotNet;

namespace SugarGuard.Protector.Class.Constants
{
	// Token: 0x02000033 RID: 51
	internal class Inject
	{
		// Token: 0x0600010C RID: 268 RVA: 0x0000D94C File Offset: 0x0000BB4C
		public DynamicContext Execute(SugarLib context, ModuleDef moduleDef)
		{
			Inject.allMethods = new Dictionary<string, MethodDef>();
			ModuleDefMD moduleDefMD = ModuleDefMD.Load(typeof(DynamicMethods).Module);
			TypeDef typeDef = moduleDefMD.ResolveTypeDef(MDToken.ToRID(typeof(DynamicMethods).MetadataToken));
			IEnumerable<IDnlibDef> enumerable = InjectHelper.Inject(typeDef, moduleDef.GlobalType, moduleDef);
			DynamicContext dynamicContext = new DynamicContext(moduleDef);
			foreach (IDnlibDef dnlibDef in enumerable)
			{
				MethodDef methodDef = dnlibDef as MethodDef;
				bool flag = methodDef != null;
				if (flag)
				{
					dynamicContext.AddRefImport(methodDef);
					Inject.allMethods.Add(methodDef.Name, methodDef);
				}
			}
			dynamicContext.Initialize();
			return dynamicContext;
		}

		// Token: 0x0400006B RID: 107
		public static Dictionary<string, MethodDef> allMethods;
	}
}
