using System;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using SugarGuard.Protector.Class;

namespace SugarGuard.Protector.Protections
{
	// Token: 0x02000009 RID: 9
	public class InvalidOpcodes
	{
		// Token: 0x0600001E RID: 30 RVA: 0x00004046 File Offset: 0x00002246
		public InvalidOpcodes(SugarLib lib)
		{
			this.Main(lib);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00004058 File Offset: 0x00002258
		private void Main(SugarLib lib)
		{
			foreach (TypeDef typeDef in lib.moduleDef.GetTypes())
			{
				foreach (MethodDef methodDef in typeDef.Methods)
				{
					bool flag = !methodDef.HasBody || !methodDef.Body.HasInstructions;
					if (!flag)
					{
						methodDef.Body.Instructions.Insert(0, Instruction.Create(OpCodes.Box, methodDef.Module.Import(typeof(Math))));
					}
				}
			}
		}
	}
}
