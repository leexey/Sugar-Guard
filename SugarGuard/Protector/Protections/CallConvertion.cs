using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using SugarGuard.Protector.Class;

namespace SugarGuard.Protector.Protections
{
	// Token: 0x02000008 RID: 8
	public class CallConvertion
	{
		// Token: 0x0600001B RID: 27 RVA: 0x00003B0C File Offset: 0x00001D0C
		public CallConvertion(SugarLib lib)
		{
			this.Main(lib);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00003B20 File Offset: 0x00001D20
		private void Main(SugarLib lib)
		{
			Local local = new Local(lib.ctor.Module.ImportAsTypeSig(typeof(Module)));
			lib.ctor.Body.Variables.Add(local);
			FieldDef fieldDef = new FieldDefUser(Renamer.InvisibleName, new FieldSig(lib.moduleDef.ImportAsTypeSig(typeof(IntPtr[]))), dnlib.DotNet.FieldAttributes.FamANDAssem | dnlib.DotNet.FieldAttributes.Family | dnlib.DotNet.FieldAttributes.Static);
			lib.moduleDef.GlobalType.Fields.Add(fieldDef);
			List<Instruction> list = new List<Instruction>
			{
				OpCodes.Ldtoken.ToInstruction(lib.moduleDef.GlobalType),
				OpCodes.Call.ToInstruction(lib.moduleDef.Import(typeof(Type).GetMethod("GetTypeFromHandle", new Type[]
				{
					typeof(RuntimeTypeHandle)
				}))),
				OpCodes.Callvirt.ToInstruction(lib.moduleDef.Import(typeof(Type).GetMethod("get_Module"))),
				OpCodes.Stloc.ToInstruction(local),
				OpCodes.Ldc_I4.ToInstruction(666),
				OpCodes.Newarr.ToInstruction(lib.moduleDef.CorLibTypes.IntPtr),
				OpCodes.Stsfld.ToInstruction(fieldDef)
			};
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			int num = 0;
			foreach (TypeDef typeDef in lib.moduleDef.Types.ToArray<TypeDef>())
			{
				foreach (MethodDef methodDef in typeDef.Methods.ToArray<MethodDef>())
				{
					bool isConstructor = methodDef.IsConstructor;
					if (!isConstructor)
					{
						bool flag = methodDef.Body == null;
						if (!flag)
						{
							bool flag2 = methodDef.HasBody && methodDef.Body.HasInstructions && !methodDef.IsConstructor && !methodDef.DeclaringType.IsGlobalModuleType;
							if (flag2)
							{
								IList<Instruction> instructions = methodDef.Body.Instructions;
								int k = 0;
								while (k < instructions.Count<Instruction>())
								{
									MemberRef memberRef;
									bool flag3;
									if (instructions[k].OpCode == OpCodes.Call || instructions[k].OpCode == OpCodes.Callvirt)
									{
										memberRef = (instructions[k].Operand as MemberRef);
										flag3 = (memberRef != null);
									}
									else
									{
										flag3 = false;
									}
									bool flag4 = flag3;
									if (flag4)
									{
										bool hasThis = memberRef.HasThis;
										if (!hasThis)
										{
											int key = memberRef.MDToken.ToInt32();
											bool flag5 = !dictionary.ContainsKey(key);
											if (flag5)
											{
												list.Add(OpCodes.Ldsfld.ToInstruction(fieldDef));
												list.Add(OpCodes.Ldc_I4.ToInstruction(num));
												list.Add(OpCodes.Ldftn.ToInstruction(memberRef));
												list.Add(OpCodes.Stelem_I.ToInstruction());
												list.Add(OpCodes.Nop.ToInstruction());
												instructions[k].OpCode = OpCodes.Ldsfld;
												instructions[k].Operand = fieldDef;
												instructions.Insert(++k, Instruction.Create(OpCodes.Ldc_I4, num));
												instructions.Insert(++k, Instruction.Create(OpCodes.Ldelem_I));
												instructions.Insert(++k, Instruction.Create(OpCodes.Calli, memberRef.MethodSig));
												dictionary.Add(key, num);
												num++;
											}
											else
											{
												int value;
												dictionary.TryGetValue(key, out value);
												instructions[k].OpCode = OpCodes.Ldsfld;
												instructions[k].Operand = fieldDef;
												instructions.Insert(++k, Instruction.Create(OpCodes.Ldc_I4, value));
												instructions.Insert(++k, Instruction.Create(OpCodes.Ldelem_I));
												instructions.Insert(++k, Instruction.Create(OpCodes.Calli, memberRef.MethodSig));
											}
										}
									}
									IL_430:
									k++;
									continue;
									goto IL_430;
								}
							}
						}
					}
				}
			}
			list[4].OpCode = OpCodes.Ldc_I4;
			list[4].Operand = num;
			for (int l = 0; l < list.Count; l++)
			{
				lib.ctor.Body.Instructions.Insert(l, list[l]);
			}
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00003FFC File Offset: 0x000021FC
		public static bool IsDelegate(TypeDef type)
		{
			bool flag = type.BaseType == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				string fullName = type.BaseType.FullName;
				result = (fullName == "System.Delegate" || fullName == "System.MulticastDelegate");
			}
			return result;
		}
	}
}
