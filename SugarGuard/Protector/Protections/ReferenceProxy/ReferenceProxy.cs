using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using SugarGuard.Protector.Class;

namespace SugarGuard.Protector.Protections.ReferenceProxy
{
	// Token: 0x02000011 RID: 17
	public class ReferenceProxy
	{
		// Token: 0x0600003C RID: 60 RVA: 0x0000518D File Offset: 0x0000338D
		public ReferenceProxy(SugarLib lib)
		{
			this.Main(lib);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x000051AC File Offset: 0x000033AC
		private static bool canObfuscate(MethodDef methodDef)
		{
			bool flag = !methodDef.HasBody;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !methodDef.Body.HasInstructions;
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool isGlobalModuleType = methodDef.DeclaringType.IsGlobalModuleType;
					result = !isGlobalModuleType;
				}
			}
			return result;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000051FC File Offset: 0x000033FC
		private void Main(SugarLib lib)
		{
			Helper helper = new Helper();
			ReferenceProxy.<Main>g__fixProxy|3_0(lib.moduleDef);
			foreach (TypeDef typeDef in lib.moduleDef.Types.ToArray<TypeDef>())
			{
				foreach (MethodDef methodDef in typeDef.Methods.ToArray<MethodDef>())
				{
					bool flag = this.usedMethods.Contains(methodDef);
					if (!flag)
					{
						bool flag2 = ReferenceProxy.canObfuscate(methodDef);
						if (flag2)
						{
							Instruction[] array3 = methodDef.Body.Instructions.ToArray<Instruction>();
							int k = 0;
							while (k < array3.Length)
							{
								Instruction instruction = array3[k];
								bool flag3 = instruction.OpCode == OpCodes.Newobj;
								if (flag3)
								{
									IMethodDefOrRef methodDefOrRef = instruction.Operand as IMethodDefOrRef;
									bool isMethodSpec = methodDefOrRef.IsMethodSpec;
									if (!isMethodSpec)
									{
										bool flag4 = methodDefOrRef == null;
										if (!flag4)
										{
											MethodDef methodDef2 = helper.GenerateMethod(methodDefOrRef, methodDef);
											bool flag5 = methodDef2 == null;
											if (!flag5)
											{
												methodDef.DeclaringType.Methods.Add(methodDef2);
												this.usedMethods.Add(methodDef2);
												instruction.OpCode = OpCodes.Call;
												instruction.Operand = methodDef2;
												this.usedMethods.Add(methodDef2);
											}
										}
									}
								}
								else
								{
									bool flag6 = instruction.OpCode == OpCodes.Stfld;
									if (flag6)
									{
										FieldDef fieldDef = instruction.Operand as FieldDef;
										bool flag7 = fieldDef == null;
										if (!flag7)
										{
											CilBody cilBody = new CilBody();
											cilBody.Instructions.Add(OpCodes.Nop.ToInstruction());
											cilBody.Instructions.Add(OpCodes.Ldarg_0.ToInstruction());
											cilBody.Instructions.Add(OpCodes.Ldarg_1.ToInstruction());
											cilBody.Instructions.Add(OpCodes.Stfld.ToInstruction(fieldDef));
											cilBody.Instructions.Add(OpCodes.Ret.ToInstruction());
											MethodSig methodSig = MethodSig.CreateInstance(lib.moduleDef.CorLibTypes.Void, fieldDef.FieldSig.GetFieldType());
											methodSig.HasThis = true;
											MethodDefUser methodDefUser = new MethodDefUser(Renamer.InvisibleName, methodSig)
											{
												Body = cilBody,
												IsHideBySig = true
											};
											this.usedMethods.Add(methodDefUser);
											methodDef.DeclaringType.Methods.Add(methodDefUser);
											instruction.Operand = methodDefUser;
											instruction.OpCode = OpCodes.Call;
										}
									}
									else
									{
										bool flag8 = instruction.OpCode == OpCodes.Ldfld;
										if (flag8)
										{
											FieldDef fieldDef2 = instruction.Operand as FieldDef;
											bool flag9 = fieldDef2 == null;
											if (!flag9)
											{
												MethodDef methodDef3 = helper.GenerateMethod(fieldDef2, methodDef);
												instruction.OpCode = OpCodes.Call;
												instruction.Operand = methodDef3;
												this.usedMethods.Add(methodDef3);
											}
										}
										else
										{
											bool flag10 = instruction.OpCode == OpCodes.Call;
											if (flag10)
											{
												bool flag11 = instruction.Operand is MemberRef;
												if (flag11)
												{
													MemberRef memberRef = (MemberRef)instruction.Operand;
													bool flag12 = !memberRef.FullName.Contains("Collections.Generic") && !memberRef.Name.Contains("ToString") && !memberRef.FullName.Contains("Thread::Start");
													if (flag12)
													{
														MethodDef methodDef4 = helper.GenerateMethod(typeDef, memberRef, memberRef.HasThis, memberRef.FullName.StartsWith("System.Void"));
														bool flag13 = methodDef4 != null;
														if (flag13)
														{
															this.usedMethods.Add(methodDef4);
															typeDef.Methods.Add(methodDef4);
															instruction.Operand = methodDef4;
															methodDef4.Body.Instructions.Add(new Instruction(OpCodes.Ret));
														}
													}
												}
											}
										}
									}
								}
								IL_3EB:
								k++;
								continue;
								goto IL_3EB;
							}
						}
					}
				}
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00005628 File Offset: 0x00003828
		[CompilerGenerated]
		internal static void <Main>g__fixProxy|3_0(ModuleDef moduleDef)
		{
			AssemblyResolver assemblyResolver = new AssemblyResolver();
			ModuleContext moduleContext = new ModuleContext(assemblyResolver);
			assemblyResolver.DefaultModuleContext = moduleContext;
			assemblyResolver.EnableTypeDefCache = true;
			List<AssemblyRef> list = moduleDef.GetAssemblyRefs().ToList<AssemblyRef>();
			moduleDef.Context = moduleContext;
			foreach (AssemblyRef assemblyRef in list)
			{
				bool flag = assemblyRef == null;
				bool flag2 = !flag;
				if (flag2)
				{
					AssemblyDef assemblyDef = assemblyResolver.Resolve(assemblyRef.FullName, moduleDef);
					bool flag3 = assemblyDef == null;
					bool flag4 = !flag3;
					if (flag4)
					{
						((AssemblyResolver)moduleDef.Context.AssemblyResolver).AddToCache(assemblyDef);
					}
				}
			}
		}

		// Token: 0x04000018 RID: 24
		private List<MethodDef> usedMethods = new List<MethodDef>();
	}
}
