using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using SugarGuard.Protector.Class;
using SugarGuard.Protector.Class.Constants;
using SugarGuard.Protector.Protections.Runtime;

namespace SugarGuard.Protector.Protections.Constants
{
	// Token: 0x0200002A RID: 42
	public class Constants
	{
		// Token: 0x060000C5 RID: 197 RVA: 0x0000A9B4 File Offset: 0x00008BB4
		public Constants(SugarLib lib)
		{
			Constants.encodedMethods = new List<EncodedMethod>();
			this.Main(lib);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x0000A9D0 File Offset: 0x00008BD0
		private void Main(SugarLib lib)
		{
			ModuleDef moduleDef = lib.moduleDef;
			int[] array = this.GenerateArray();
			string invisibleName = Renamer.InvisibleName;
			FieldDefUser fieldDefUser = new FieldDefUser(invisibleName, new FieldSig(moduleDef.ImportAsTypeSig(typeof(int[]))), FieldAttributes.FamANDAssem | FieldAttributes.Family | FieldAttributes.Static);
			moduleDef.GlobalType.Fields.Add(fieldDefUser);
			this.InjectArray(array, lib.ctor, fieldDefUser);
			MethodDef methodDef = this.InjectDecryptor(lib);
			methodDef = this.ModifyDecryptor(methodDef, fieldDefUser);
			foreach (TypeDef typeDef in moduleDef.Types)
			{
				bool isGlobalModuleType = typeDef.IsGlobalModuleType;
				if (!isGlobalModuleType)
				{
					foreach (MethodDef methodDef2 in typeDef.Methods)
					{
						bool flag = !methodDef2.HasBody || !methodDef2.Body.HasInstructions;
						if (!flag)
						{
							bool flag2 = !this.hasStrings(methodDef2);
							if (!flag2)
							{
								for (int i = 0; i < methodDef2.Body.Instructions.Count; i++)
								{
									bool flag3 = methodDef2.Body.Instructions[i].OpCode == OpCodes.Ldstr;
									if (flag3)
									{
										Local local = new Local(methodDef2.Module.ImportAsTypeSig(typeof(string)));
										methodDef2.Body.Variables.Add(local);
										string s = methodDef2.Body.Instructions[i].Operand.ToString();
										methodDef2.Body.Instructions[i].OpCode = OpCodes.Ldloc;
										methodDef2.Body.Instructions[i].Operand = local;
										methodDef2.Body.Instructions.Insert(0, OpCodes.Ldstr.ToInstruction(s));
										methodDef2.Body.Instructions.Insert(1, OpCodes.Stloc_S.ToInstruction(local));
										i += 2;
									}
								}
								int num = Constants.rnd.Next(100, 500);
								Constants.encodedMethods.Add(new EncodedMethod(methodDef2, num));
								MethodDef methodDef3 = moduleDef.GlobalType.FindOrCreateStaticConstructor();
								Local local2 = new Local(moduleDef.ImportAsTypeSig(typeof(RuntimeMethodHandle)));
								methodDef3.Body.Variables.Add(local2);
								methodDef3.Body.Instructions.Insert(0, OpCodes.Ldtoken.ToInstruction(methodDef2));
								methodDef3.Body.Instructions.Insert(1, OpCodes.Stloc.ToInstruction(local2));
								for (int j = 0; j < methodDef2.Body.Instructions.Count; j++)
								{
									bool flag4 = methodDef2.Body.Instructions[j].OpCode == OpCodes.Ldstr;
									if (flag4)
									{
										string s2 = methodDef2.Body.Instructions[j].Operand.ToString();
										int num2 = Constants.rnd.Next(10, 50);
										string s3 = this.EncodeString(s2, num2 + num, array);
										FieldDefUser fieldDefUser2 = new FieldDefUser(Renamer.GenerateName(), new FieldSig(moduleDef.ImportAsTypeSig(typeof(string))), FieldAttributes.FamANDAssem | FieldAttributes.Family | FieldAttributes.Static);
										moduleDef.GlobalType.Fields.Add(fieldDefUser2);
										int num3 = methodDef3.Body.Instructions.Count - 1;
										methodDef3.Body.Instructions.Insert(num3, OpCodes.Ldstr.ToInstruction(s3));
										methodDef3.Body.Instructions.Insert(++num3, OpCodes.Ldc_I4.ToInstruction(num2));
										methodDef3.Body.Instructions.Insert(++num3, OpCodes.Ldloc.ToInstruction(local2));
										methodDef3.Body.Instructions.Insert(++num3, OpCodes.Call.ToInstruction(methodDef));
										methodDef3.Body.Instructions.Insert(num3 + 1, OpCodes.Stsfld.ToInstruction(fieldDefUser2));
										methodDef2.Body.Instructions[j] = OpCodes.Ldsfld.ToInstruction(fieldDefUser2);
										methodDef2.Body.SimplifyBranches();
										methodDef2.Body.OptimizeBranches();
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x0000AED4 File Offset: 0x000090D4
		private string EncodeString(string s, int realkey, int[] array)
		{
			for (int i = 0; i < array.Length; i++)
			{
				realkey += array[i];
			}
			char[] array2 = new char[s.Length];
			for (int j = 0; j < s.Length; j++)
			{
				array2[j] = (char)((int)s[j] ^ realkey);
			}
			return new string(array2);
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x0000AF3C File Offset: 0x0000913C
		private int[] GenerateArray()
		{
			int[] array = new int[Constants.rnd.Next(10, 50)];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Constants.rnd.Next(100, 500);
			}
			return array;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x0000AF8C File Offset: 0x0000918C
		private void InjectArray(int[] array, MethodDef cctor, FieldDef arrayField)
		{
			bool flag = Constants.rnd.Next(0, 3) == 1;
			if (flag)
			{
				List<Instruction> list = new List<Instruction>
				{
					OpCodes.Ldc_I4.ToInstruction(array.Length),
					OpCodes.Newarr.ToInstruction(cctor.Module.CorLibTypes.Int32),
					OpCodes.Stsfld.ToInstruction(arrayField)
				};
				for (int i = 0; i < array.Length; i++)
				{
					list.Add(OpCodes.Ldsfld.ToInstruction(arrayField));
					list.Add(OpCodes.Ldc_I4.ToInstruction(i));
					list.Add(OpCodes.Ldc_I4.ToInstruction(array[i]));
					list.Add(OpCodes.Stelem_I4.ToInstruction());
					list.Add(OpCodes.Nop.ToInstruction());
				}
				for (int j = 0; j < list.Count; j++)
				{
					cctor.Body.Instructions.Insert(j, list[j]);
				}
			}
			else
			{
				List<Instruction> list2 = new List<Instruction>
				{
					OpCodes.Ldc_I4.ToInstruction(array.Length),
					OpCodes.Newarr.ToInstruction(cctor.Module.CorLibTypes.Int32),
					OpCodes.Dup.ToInstruction()
				};
				for (int k = 0; k < array.Length; k++)
				{
					list2.Add(OpCodes.Ldc_I4.ToInstruction(k));
					list2.Add(OpCodes.Ldc_I4.ToInstruction(array[k]));
					list2.Add(OpCodes.Stelem_I4.ToInstruction());
					bool flag2 = k != array.Length - 1;
					if (flag2)
					{
						list2.Add(OpCodes.Dup.ToInstruction());
					}
				}
				list2.Add(OpCodes.Stsfld.ToInstruction(arrayField));
				for (int l = 0; l < list2.Count; l++)
				{
					cctor.Body.Instructions.Insert(l, list2[l]);
				}
			}
		}

		// Token: 0x060000CA RID: 202 RVA: 0x0000B1C0 File Offset: 0x000093C0
		private MethodDef InjectDecryptor(SugarLib lib)
		{
			ModuleDefMD moduleDefMD = ModuleDefMD.Load(typeof(ConstantsRuntime).Module);
			TypeDef typeDef = moduleDefMD.ResolveTypeDef(MDToken.ToRID(typeof(ConstantsRuntime).MetadataToken));
			IEnumerable<IDnlibDef> enumerable = InjectHelper.Inject(typeDef, lib.globalType, lib.moduleDef);
			MethodDef result = (MethodDef)enumerable.Single((IDnlibDef method) => method.Name == "Decrypt");
			foreach (IDnlibDef dnlibDef in enumerable)
			{
				dnlibDef.Name = Constants.GenerateName();
			}
			return result;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x0000B294 File Offset: 0x00009494
		public static string GenerateName()
		{
			return new string((from s in Enumerable.Repeat<string>("あいうえおかきくけこがぎぐげごさしすせそざじずぜアイウエオクザタツワルムパリピンプペヲポ", 10)
			select s[Constants.rnd.Next(s.Length)]).ToArray<char>());
		}

		// Token: 0x060000CC RID: 204 RVA: 0x0000B2E0 File Offset: 0x000094E0
		private MethodDef ModifyDecryptor(MethodDef dec, FieldDef field)
		{
			for (int i = 0; i < dec.Body.Instructions.Count; i++)
			{
				bool flag = dec.Body.Instructions[i].OpCode == OpCodes.Ldsfld;
				if (flag)
				{
					dec.Body.Instructions[i].OpCode = OpCodes.Ldsfld;
					dec.Body.Instructions[i].Operand = field;
				}
			}
			return dec;
		}

		// Token: 0x060000CD RID: 205 RVA: 0x0000B36C File Offset: 0x0000956C
		private bool hasStrings(MethodDef method)
		{
			foreach (Instruction instruction in method.Body.Instructions)
			{
				bool flag = instruction.OpCode == OpCodes.Ldstr;
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000044 RID: 68
		public static List<EncodedMethod> encodedMethods;

		// Token: 0x04000045 RID: 69
		public static Random rnd = new Random();
	}
}
