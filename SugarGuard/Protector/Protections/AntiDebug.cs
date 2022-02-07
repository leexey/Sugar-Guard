using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using SugarGuard.Protector.Class;
using SugarGuard.Protector.Protections.Runtime;

namespace SugarGuard.Protector.Protections
{
	// Token: 0x02000006 RID: 6
	public class AntiDebug
	{
		// Token: 0x06000017 RID: 23 RVA: 0x0000381B File Offset: 0x00001A1B
		public AntiDebug(SugarLib lib)
		{
			this.Main(lib);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x0000382C File Offset: 0x00001A2C
		private void Main(SugarLib lib)
		{
			ModuleDef moduleDef = lib.moduleDef;
			ModuleDefMD moduleDefMD = ModuleDefMD.Load(typeof(AntiDebug).Module);
			TypeDef typeDef = moduleDefMD.ResolveTypeDef(MDToken.ToRID(typeof(AntiDebug).MetadataToken));
			IEnumerable<IDnlibDef> source = InjectHelper.Inject(typeDef, moduleDef.EntryPoint.DeclaringType, moduleDef);
			MethodDef method2 = (MethodDef)source.Single((IDnlibDef method) => method.Name == "Initialize");
			MethodDef entryPoint = moduleDef.EntryPoint;
			entryPoint.Body.Instructions.Insert(0, OpCodes.Call.ToInstruction(method2));
		}
	}
}
