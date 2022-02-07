using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using SugarGuard.Protector.Class;
using SugarGuard.Protector.Protections.Runtime;

namespace SugarGuard.Protector.Protections
{
	// Token: 0x02000005 RID: 5
	public class AntiDump
	{
		// Token: 0x06000015 RID: 21 RVA: 0x0000375C File Offset: 0x0000195C
		public AntiDump(SugarLib lib)
		{
			this.Main(lib);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00003770 File Offset: 0x00001970
		private void Main(SugarLib lib)
		{
			ModuleDef moduleDef = lib.moduleDef;
			ModuleDefMD moduleDefMD = ModuleDefMD.Load(typeof(AntiDump).Module);
			TypeDef typeDef = moduleDefMD.ResolveTypeDef(MDToken.ToRID(typeof(AntiDump).MetadataToken));
			IEnumerable<IDnlibDef> source = InjectHelper.Inject(typeDef, moduleDef.GlobalType, moduleDef);
			MethodDef method2 = (MethodDef)source.Single((IDnlibDef method) => method.Name == "AntiDumpInj");
			MethodDef methodDef = moduleDef.GlobalType.FindStaticConstructor();
			methodDef.Body.Instructions.Insert(0, OpCodes.Call.ToInstruction(method2));
		}
	}
}
