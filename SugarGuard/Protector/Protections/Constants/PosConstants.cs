using System;
using System.Collections.Generic;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using SugarGuard.Protector.Class;
using SugarGuard.Protector.Class.Constants;

namespace SugarGuard.Protector.Protections.Constants
{
	// Token: 0x0200002B RID: 43
	public class PosConstants
	{
		// Token: 0x060000CF RID: 207 RVA: 0x0000B3E4 File Offset: 0x000095E4
		public PosConstants(SugarLib lib)
		{
			this.Main(lib);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x0000B3F8 File Offset: 0x000095F8
		private void Main(SugarLib lib)
		{
			foreach (TypeDef typeDef in lib.moduleDef.Types)
			{
				foreach (MethodDef methodDef in typeDef.Methods)
				{
					List<EncodedMethod> encodedMethods = Constants.encodedMethods;
					foreach (EncodedMethod encodedMethod in encodedMethods)
					{
						bool flag = encodedMethod.eMethod == methodDef;
						if (flag)
						{
							methodDef.Body.Instructions.Add(OpCodes.Ldstr.ToInstruction(encodedMethod.eNum.ToString()));
							methodDef.Body.Instructions.Add(OpCodes.Pop.ToInstruction());
							methodDef.Body.Instructions.Add(OpCodes.Ret.ToInstruction());
						}
					}
				}
			}
		}
	}
}
