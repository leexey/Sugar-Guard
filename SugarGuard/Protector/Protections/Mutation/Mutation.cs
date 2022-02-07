using System;
using dnlib.DotNet;
using SugarGuard.Protector.Class;
using SugarGuard.Protector.Protections.Mutation.Stages;

namespace SugarGuard.Protector.Protections.Mutation
{
	// Token: 0x02000013 RID: 19
	public class Mutation
	{
		// Token: 0x06000044 RID: 68 RVA: 0x00005A82 File Offset: 0x00003C82
		public Mutation(SugarLib lib)
		{
			this.Phase(lib);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00005A94 File Offset: 0x00003C94
		public void Phase(SugarLib lib)
		{
			ModuleDef moduleDef = lib.moduleDef;
			foreach (TypeDef typeDef in moduleDef.GetTypes())
			{
				bool isGlobalModuleType = typeDef.IsGlobalModuleType;
				if (!isGlobalModuleType)
				{
					foreach (MethodDef methodDef in typeDef.Methods)
					{
						bool flag = !this.canMutateMethod(methodDef);
						if (!flag)
						{
							new MethodPreparation(methodDef).Execute();
							IntsToInliner intsToInliner = new IntsToInliner(methodDef);
							IntsToArray intsToArray = new IntsToArray(methodDef);
							IntsToStackalloc intsToStackalloc = new IntsToStackalloc(methodDef);
							IntsToMath intsToMath = new IntsToMath(methodDef);
							LocalsToCustomLocal localsToCustomLocal = new LocalsToCustomLocal(methodDef);
							IntsToRandom intsToRandom = new IntsToRandom(methodDef);
							for (int i = 0; i < methodDef.Body.Instructions.Count; i++)
							{
								bool flag2 = methodDef.Body.Instructions[i].IsLdcI4() && MutationHelper.CanObfuscate(methodDef.Body.Instructions, i);
								if (flag2)
								{
									switch (Mutation.rnd.Next(0, 5))
									{
									case 1:
										intsToMath.Execute(ref i);
										break;
									case 2:
										localsToCustomLocal.Execute(ref i);
										break;
									case 3:
										intsToRandom.Execute(ref i);
										break;
									case 4:
										intsToStackalloc.Execute(ref i);
										break;
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00005C74 File Offset: 0x00003E74
		public bool canMutateMethod(MethodDef method)
		{
			return method.HasBody && method.Body.HasInstructions;
		}

		// Token: 0x04000019 RID: 25
		private static Random rnd = new Random();
	}
}
