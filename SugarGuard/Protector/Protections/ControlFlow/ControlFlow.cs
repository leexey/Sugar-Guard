using System;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using dnlib.DotNet.Pdb;
using SugarGuard.Protector.Class;

namespace SugarGuard.Protector.Protections.ControlFlow
{
	// Token: 0x02000020 RID: 32
	public class ControlFlow
	{
		// Token: 0x06000098 RID: 152 RVA: 0x0000850E File Offset: 0x0000670E
		public ControlFlow(SugarLib lib)
		{
			this.Main(lib);
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00008520 File Offset: 0x00006720
		private void Main(SugarLib lib)
		{
			foreach (TypeDef typeDef in lib.moduleDef.Types)
			{
				foreach (MethodDef methodDef in typeDef.Methods)
				{
					bool flag = !methodDef.HasBody || !methodDef.Body.HasInstructions;
					if (!flag)
					{
						bool flag2 = methodDef.ReturnType != null;
						if (flag2)
						{
							CilBody body = methodDef.Body;
							body.SimplifyBranches();
							BlockParser.ScopeBlock scopeBlock = BlockParser.ParseBody(body);
							new SwitchMangler().Mangle(body, scopeBlock, lib, methodDef, methodDef.ReturnType);
							body.Instructions.Clear();
							scopeBlock.ToBody(body);
							bool flag3 = body.PdbMethod != null;
							if (flag3)
							{
								body.PdbMethod = new PdbMethod
								{
									Scope = new PdbScope
									{
										Start = body.Instructions.First<Instruction>(),
										End = body.Instructions.Last<Instruction>()
									}
								};
							}
							methodDef.CustomDebugInfos.RemoveWhere((PdbCustomDebugInfo cdi) => cdi is PdbStateMachineHoistedLocalScopesCustomDebugInfo);
							foreach (ExceptionHandler exceptionHandler in body.ExceptionHandlers)
							{
								int num = body.Instructions.IndexOf(exceptionHandler.TryEnd) + 1;
								exceptionHandler.TryEnd = ((num < body.Instructions.Count) ? body.Instructions[num] : null);
								num = body.Instructions.IndexOf(exceptionHandler.HandlerEnd) + 1;
								exceptionHandler.HandlerEnd = ((num < body.Instructions.Count) ? body.Instructions[num] : null);
							}
						}
					}
				}
			}
		}
	}
}
