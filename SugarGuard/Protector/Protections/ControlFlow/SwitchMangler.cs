using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using SugarGuard.Protector.Class;

namespace SugarGuard.Protector.Protections.ControlFlow
{
	// Token: 0x02000028 RID: 40
	internal class SwitchMangler : ManglerBase
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x060000B7 RID: 183 RVA: 0x00009522 File Offset: 0x00007722
		// (set) Token: 0x060000B8 RID: 184 RVA: 0x0000952A File Offset: 0x0000772A
		public SugarLib ctx { get; set; }

		// Token: 0x060000B9 RID: 185 RVA: 0x00009534 File Offset: 0x00007734
		private static OpCode InverseBranch(OpCode opCode)
		{
			OpCode result;
			switch (opCode.Code)
			{
			case Code.Brfalse:
				result = OpCodes.Brtrue;
				break;
			case Code.Brtrue:
				result = OpCodes.Brfalse;
				break;
			case Code.Beq:
				result = OpCodes.Bne_Un;
				break;
			case Code.Bge:
				result = OpCodes.Blt;
				break;
			case Code.Bgt:
				result = OpCodes.Ble;
				break;
			case Code.Ble:
				result = OpCodes.Bgt;
				break;
			case Code.Blt:
				result = OpCodes.Bge;
				break;
			case Code.Bne_Un:
				result = OpCodes.Beq;
				break;
			case Code.Bge_Un:
				result = OpCodes.Blt_Un;
				break;
			case Code.Bgt_Un:
				result = OpCodes.Ble_Un;
				break;
			case Code.Ble_Un:
				result = OpCodes.Bgt_Un;
				break;
			case Code.Blt_Un:
				result = OpCodes.Bge_Un;
				break;
			default:
				throw new NotSupportedException();
			}
			return result;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x000095F0 File Offset: 0x000077F0
		public override void Mangle(CilBody body, BlockParser.ScopeBlock root, SugarLib ctx, MethodDef Method, TypeSig retType)
		{
			SwitchMangler.<>c__DisplayClass7_0 CS$<>8__locals1 = new SwitchMangler.<>c__DisplayClass7_0();
			this.ctx = ctx;
			CS$<>8__locals1.trace = new SwitchMangler.Trace(body, retType.RemoveModifiers().ElementType != ElementType.Void);
			Local local = new Local(Method.Module.CorLibTypes.UInt32);
			Local local2 = new Local(Method.Module.ImportAsTypeSig(typeof(uint[])));
			body.Variables.Add(local2);
			body.Variables.Add(local);
			body.InitLocals = true;
			body.MaxStack += 2;
			IPredicate predicate = new Predicate(ctx);
			using (IEnumerator<BlockParser.InstrBlock> enumerator = ManglerBase.GetAllBlocks(root).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SwitchMangler.<>c__DisplayClass7_1 CS$<>8__locals2 = new SwitchMangler.<>c__DisplayClass7_1();
					CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
					CS$<>8__locals2.block = enumerator.Current;
					LinkedList<Instruction[]> statements = this.SplitStatements(CS$<>8__locals2.block, CS$<>8__locals2.CS$<>8__locals1.trace, ctx);
					bool isInstanceConstructor = Method.IsInstanceConstructor;
					if (isInstanceConstructor)
					{
						List<Instruction> list = new List<Instruction>();
						while (statements.First != null)
						{
							list.AddRange(statements.First.Value);
							Instruction instruction = statements.First.Value.Last<Instruction>();
							statements.RemoveFirst();
							bool flag = instruction.OpCode == OpCodes.Call && ((IMethod)instruction.Operand).Name == ".ctor";
							if (flag)
							{
								break;
							}
						}
						statements.AddFirst(list.ToArray());
					}
					bool flag2 = statements.Count < 3;
					if (!flag2)
					{
						int[] array = Enumerable.Range(0, statements.Count).ToArray<int>();
						this.Shuffle<int>(array);
						int[] array2 = new int[array.Length];
						int i;
						for (i = 0; i < array2.Length; i++)
						{
							int num = new Random().Next() & int.MaxValue;
							array2[i] = num - num % statements.Count + array[i];
						}
						Dictionary<Instruction, int> dictionary = new Dictionary<Instruction, int>();
						LinkedListNode<Instruction[]> linkedListNode = statements.First;
						i = 0;
						while (linkedListNode != null)
						{
							bool flag3 = i != 0;
							if (flag3)
							{
								dictionary[linkedListNode.Value[0]] = array2[i];
							}
							i++;
							linkedListNode = linkedListNode.Next;
						}
						HashSet<Instruction> statementLast = new HashSet<Instruction>(from st in statements
						select st.Last<Instruction>());
						Func<Instruction, bool> <>9__4;
						Func<Instruction, bool> <>9__5;
						Func<Instruction, bool> <>9__2;
						Func<IList<Instruction>, bool> func = delegate(IList<Instruction> instrs)
						{
							Func<Instruction, bool> predicate2;
							if ((predicate2 = <>9__2) == null)
							{
								predicate2 = (<>9__2 = delegate(Instruction instr)
								{
									bool flag19 = CS$<>8__locals2.CS$<>8__locals1.trace.HasMultipleSources(instr.Offset);
									bool result;
									if (flag19)
									{
										result = true;
									}
									else
									{
										List<Instruction> list5;
										bool flag20 = CS$<>8__locals2.CS$<>8__locals1.trace.BrRefs.TryGetValue(instr.Offset, out list5);
										if (flag20)
										{
											bool flag21 = list5.Any((Instruction src) => src.Operand is Instruction[]);
											if (flag21)
											{
												return true;
											}
											IEnumerable<Instruction> source = list5;
											Func<Instruction, bool> predicate3;
											if ((predicate3 = <>9__4) == null)
											{
												predicate3 = (<>9__4 = ((Instruction src) => src.Offset <= statements.First.Value.Last<Instruction>().Offset || src.Offset >= CS$<>8__locals2.block.Instructions.Last<Instruction>().Offset));
											}
											bool flag22 = source.Any(predicate3);
											if (flag22)
											{
												return true;
											}
											IEnumerable<Instruction> source2 = list5;
											Func<Instruction, bool> predicate4;
											if ((predicate4 = <>9__5) == null)
											{
												predicate4 = (<>9__5 = ((Instruction src) => statementLast.Contains(src)));
											}
											bool flag23 = source2.Any(predicate4);
											if (flag23)
											{
												return true;
											}
										}
										result = false;
									}
									return result;
								});
							}
							return instrs.Any(predicate2);
						};
						Instruction instruction2 = new Instruction(OpCodes.Switch);
						List<Instruction> list2 = new List<Instruction>();
						bool flag4 = predicate != null;
						if (flag4)
						{
							predicate.Init(body);
							list2.Add(Instruction.CreateLdcI4(predicate.GetSwitchKey(array2[1])));
							predicate.EmitSwitchLoad(list2);
						}
						else
						{
							list2.Add(Instruction.CreateLdcI4(array2[1]));
						}
						list2.Add(Instruction.Create(OpCodes.Dup));
						list2.Add(Instruction.Create(OpCodes.Stloc, local));
						list2.Add(Instruction.Create(OpCodes.Ldc_I4, statements.Count));
						list2.Add(Instruction.Create(OpCodes.Rem_Un));
						list2.Add(instruction2);
						this.AddJump(list2, statements.Last.Value[0], Method);
						this.AddJunk(list2, Method);
						Instruction[] array3 = new Instruction[statements.Count];
						linkedListNode = statements.First;
						i = 0;
						while (linkedListNode.Next != null)
						{
							List<Instruction> list3 = new List<Instruction>(linkedListNode.Value);
							bool flag5 = i != 0;
							if (flag5)
							{
								bool flag6 = false;
								bool flag7 = list3.Last<Instruction>().IsBr();
								if (flag7)
								{
									Instruction key = (Instruction)list3.Last<Instruction>().Operand;
									int num2;
									bool flag8 = !CS$<>8__locals2.CS$<>8__locals1.trace.IsBranchTarget(list3.Last<Instruction>().Offset) && dictionary.TryGetValue(key, out num2);
									if (flag8)
									{
										int num3 = (predicate != null) ? predicate.GetSwitchKey(num2) : num2;
										bool flag9 = func(list3);
										list3.RemoveAt(list3.Count - 1);
										bool flag10 = flag9;
										if (flag10)
										{
											list3.Add(Instruction.Create(OpCodes.Ldc_I4, num3));
										}
										else
										{
											int num4 = array2[i];
											int num5 = SwitchMangler.rnd.Next(1000, 2000);
											Local local3 = new Local(Method.Module.CorLibTypes.UInt32);
											Local local4 = new Local(Method.Module.CorLibTypes.UInt32);
											body.Variables.Add(local3);
											list3.Add(Instruction.Create(OpCodes.Ldloc, local));
											list3.Add(Instruction.Create(OpCodes.Ldc_I4, num5));
											list3.Add(Instruction.Create(OpCodes.Div));
											list3.Add(Instruction.Create(OpCodes.Stloc, local3));
											list3.Add(Instruction.Create(OpCodes.Ldloc, local3));
											list3.Add(Instruction.Create(OpCodes.Ldc_I4, num4 / num5 - num3));
											list3.Add(Instruction.Create(OpCodes.Sub));
										}
										this.AddJump(list3, list2[1], Method);
										this.AddJunk(list3, Method);
										array3[array[i]] = list3[0];
										flag6 = true;
									}
								}
								else
								{
									bool flag11 = list3.Last<Instruction>().IsConditionalBranch();
									if (flag11)
									{
										Instruction key2 = (Instruction)list3.Last<Instruction>().Operand;
										int num6;
										bool flag12 = !CS$<>8__locals2.CS$<>8__locals1.trace.IsBranchTarget(list3.Last<Instruction>().Offset) && dictionary.TryGetValue(key2, out num6);
										if (flag12)
										{
											bool flag13 = func(list3);
											int num7 = array2[i + 1];
											OpCode opCode = list3.Last<Instruction>().OpCode;
											list3.RemoveAt(list3.Count - 1);
											bool flag14 = Convert.ToBoolean(SwitchMangler.rnd.Next(0, 2));
											if (flag14)
											{
												opCode = SwitchMangler.InverseBranch(opCode);
												int num8 = num6;
												num6 = num7;
												num7 = num8;
											}
											int num9 = array2[i];
											int num10 = 0;
											int num11 = 0;
											bool flag15 = !flag13;
											if (flag15)
											{
												num10 = SwitchMangler.rnd.Next(1000, 2000);
												num11 = num9 / num10;
											}
											Instruction instruction3 = Instruction.CreateLdcI4(num11 ^ ((predicate != null) ? predicate.GetSwitchKey(num6) : num6));
											Instruction item = Instruction.CreateLdcI4(num11 ^ ((predicate != null) ? predicate.GetSwitchKey(num7) : num7));
											Instruction instruction4 = Instruction.Create(OpCodes.Pop);
											list3.Add(Instruction.Create(opCode, instruction3));
											list3.Add(item);
											list3.Add(Instruction.Create(OpCodes.Dup));
											list3.Add(Instruction.Create(OpCodes.Br, instruction4));
											list3.Add(instruction3);
											list3.Add(Instruction.Create(OpCodes.Dup));
											list3.Add(instruction4);
											bool flag16 = !flag13;
											if (flag16)
											{
												list3.Add(Instruction.Create(OpCodes.Ldloc, local));
												list3.Add(Instruction.Create(OpCodes.Ldc_I4, num10));
												list3.Add(Instruction.Create(OpCodes.Div));
												list3.Add(Instruction.Create(OpCodes.Xor));
											}
											this.AddJump(list3, list2[1], Method);
											this.AddJunk(list3, Method);
											array3[array[i]] = list3[0];
											flag6 = true;
										}
									}
								}
								bool flag17 = !flag6;
								if (flag17)
								{
									int num12 = (predicate != null) ? predicate.GetSwitchKey(array2[i + 1]) : array2[i + 1];
									bool flag18 = !func(list3);
									if (flag18)
									{
										int num13 = array2[i];
										int[] array4 = SwitchMangler.GenerateArray();
										int num14 = array4[array4.Length - 1];
										Local local5 = new Local(Method.Module.CorLibTypes.UInt32);
										Local local6 = new Local(Method.Module.CorLibTypes.UInt32);
										body.Variables.Add(local5);
										body.Variables.Add(local6);
										list3.Add(Instruction.Create(OpCodes.Ldloc, local));
										list3.Add(Instruction.Create(OpCodes.Stloc, local6));
										SwitchMangler.InjectArray(Method, array4, ref list3, local2);
										list3.Add(Instruction.Create(OpCodes.Ldloc, local6));
										list3.Add(OpCodes.Ldloc_S.ToInstruction(local2));
										list3.Add(OpCodes.Ldc_I4.ToInstruction(array4.Length - 1));
										list3.Add(OpCodes.Ldelem_I4.ToInstruction());
										list3.Add(Instruction.Create(OpCodes.Div));
										list3.Add(Instruction.Create(OpCodes.Stloc, local5));
										list3.Add(Instruction.Create(OpCodes.Ldloc, local5));
										list3.Add(Instruction.Create(OpCodes.Ldc_I4, num13 / num14 - num12));
										list3.Add(Instruction.Create(OpCodes.Sub));
									}
									else
									{
										list3.Add(Instruction.Create(OpCodes.Ldc_I4, num12));
									}
									this.AddJump(list3, list2[1], Method);
									this.AddJunk(list3, Method);
									array3[array[i]] = list3[0];
								}
							}
							else
							{
								array3[array[i]] = list2[0];
							}
							linkedListNode.Value = list3.ToArray();
							linkedListNode = linkedListNode.Next;
							i++;
						}
						array3[array[i]] = linkedListNode.Value[0];
						instruction2.Operand = array3;
						Instruction[] value = statements.First.Value;
						statements.RemoveFirst();
						Instruction[] value2 = statements.Last.Value;
						statements.RemoveLast();
						List<Instruction[]> list4 = statements.ToList<Instruction[]>();
						this.Shuffle<Instruction[]>(list4);
						CS$<>8__locals2.block.Instructions.Clear();
						CS$<>8__locals2.block.Instructions.AddRange(value);
						CS$<>8__locals2.block.Instructions.AddRange(list2);
						foreach (Instruction[] collection in list4)
						{
							CS$<>8__locals2.block.Instructions.AddRange(collection);
						}
						CS$<>8__locals2.block.Instructions.AddRange(value2);
					}
				}
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x0000A1A8 File Offset: 0x000083A8
		private static int[] GenerateArray()
		{
			int[] array = new int[SwitchMangler.rnd.Next(3, 6)];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = SwitchMangler.rnd.Next(100, 500);
			}
			return array;
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0000A1F8 File Offset: 0x000083F8
		private static void InjectArray(MethodDef method, int[] array, ref List<Instruction> toInject, Local local)
		{
			List<Instruction> list = new List<Instruction>
			{
				OpCodes.Ldc_I4.ToInstruction(array.Length),
				OpCodes.Newarr.ToInstruction(method.Module.CorLibTypes.UInt32),
				OpCodes.Stloc_S.ToInstruction(local)
			};
			for (int i = 0; i < array.Length; i++)
			{
				bool flag = i == 0;
				if (flag)
				{
					list.Add(OpCodes.Ldloc_S.ToInstruction(local));
					list.Add(OpCodes.Ldc_I4.ToInstruction(i));
					list.Add(OpCodes.Ldc_I4.ToInstruction(array[i]));
					list.Add(OpCodes.Stelem_I4.ToInstruction());
					list.Add(OpCodes.Nop.ToInstruction());
				}
				else
				{
					int num = array[i];
					list.Add(OpCodes.Ldloc_S.ToInstruction(local));
					list.Add(OpCodes.Ldc_I4.ToInstruction(i));
					list.Add(OpCodes.Ldc_I4.ToInstruction(num));
					int index = list.Count - 1;
					for (int j = i - 1; j >= 0; j--)
					{
						OpCode opCode = null;
						int num2 = SwitchMangler.rnd.Next(0, 2);
						int num3 = num2;
						if (num3 != 0)
						{
							if (num3 == 1)
							{
								num -= array[j];
								opCode = OpCodes.Add;
							}
						}
						else
						{
							num += array[j];
							opCode = OpCodes.Sub;
						}
						list.Add(OpCodes.Ldloc_S.ToInstruction(local));
						list.Add(OpCodes.Ldc_I4.ToInstruction(j));
						list.Add(OpCodes.Ldelem_I4.ToInstruction());
						list.Add(opCode.ToInstruction());
					}
					list[index].OpCode = OpCodes.Ldc_I4;
					list[index].Operand = num;
					list.Add(OpCodes.Stelem_I4.ToInstruction());
					list.Add(OpCodes.Nop.ToInstruction());
				}
			}
			for (int k = 0; k < list.Count; k++)
			{
				toInject.Add(list[k]);
			}
		}

		// Token: 0x060000BD RID: 189 RVA: 0x0000A438 File Offset: 0x00008638
		private LinkedList<Instruction[]> SplitStatements(BlockParser.InstrBlock block, SwitchMangler.Trace trace, SugarLib ctx)
		{
			LinkedList<Instruction[]> linkedList = new LinkedList<Instruction[]>();
			List<Instruction> list = new List<Instruction>();
			HashSet<Instruction> hashSet = new HashSet<Instruction>();
			for (int i = 0; i < block.Instructions.Count; i++)
			{
				Instruction instruction = block.Instructions[i];
				list.Add(instruction);
				bool flag = i + 1 < block.Instructions.Count && trace.HasMultipleSources(block.Instructions[i + 1].Offset);
				FlowControl flowControl = instruction.OpCode.FlowControl;
				FlowControl flowControl2 = flowControl;
				if (flowControl2 == FlowControl.Branch || flowControl2 == FlowControl.Cond_Branch || flowControl2 - FlowControl.Return <= 1)
				{
					flag = true;
					bool flag2 = trace.AfterStack[instruction.Offset] != 0;
					if (flag2)
					{
						Instruction instruction2 = instruction.Operand as Instruction;
						bool flag3 = instruction2 != null;
						if (flag3)
						{
							hashSet.Add(instruction2);
						}
						else
						{
							Instruction[] array = instruction.Operand as Instruction[];
							bool flag4 = array != null;
							if (flag4)
							{
								foreach (Instruction item in array)
								{
									hashSet.Add(item);
								}
							}
						}
					}
				}
				hashSet.Remove(instruction);
				bool flag5 = instruction.OpCode.OpCodeType != OpCodeType.Prefix && trace.AfterStack[instruction.Offset] == 0 && hashSet.Count == 0 && (flag || 90.0 > new Random().NextDouble()) && (i == 0 || block.Instructions[i - 1].OpCode.Code != Code.Tailcall);
				if (flag5)
				{
					linkedList.AddLast(list.ToArray());
					list.Clear();
				}
			}
			bool flag6 = list.Count > 0;
			if (flag6)
			{
				linkedList.AddLast(list.ToArray());
			}
			return linkedList;
		}

		// Token: 0x060000BE RID: 190 RVA: 0x0000A630 File Offset: 0x00008830
		public void Shuffle<T>(IList<T> list)
		{
			for (int i = list.Count - 1; i > 1; i--)
			{
				int index = new Random().Next(i + 1);
				T value = list[index];
				list[index] = list[i];
				list[i] = value;
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x0000A688 File Offset: 0x00008888
		public void AddJump(IList<Instruction> instrs, Instruction target, MethodDef Method)
		{
			bool flag = !Method.Module.IsClr40 && !Method.DeclaringType.HasGenericParameters && !Method.HasGenericParameters && (instrs[0].OpCode.FlowControl == FlowControl.Call || instrs[0].OpCode.FlowControl == FlowControl.Next);
			if (flag)
			{
				bool flag2 = false;
				bool flag3 = Convert.ToBoolean(new Random().Next(0, 2));
				if (flag3)
				{
					TypeDef typeDef = Method.Module.Types[new Random().Next(Method.Module.Types.Count)];
					bool hasMethods = typeDef.HasMethods;
					if (hasMethods)
					{
						instrs.Add(Instruction.Create(OpCodes.Ldtoken, typeDef.Methods[new Random().Next(typeDef.Methods.Count)]));
						instrs.Add(Instruction.Create(OpCodes.Box, Method.Module.CorLibTypes.GetTypeRef("System", "RuntimeMethodHandle")));
						flag2 = true;
					}
				}
				bool flag4 = !flag2;
				if (flag4)
				{
					instrs.Add(Instruction.Create(OpCodes.Ldc_I4, Convert.ToBoolean(new Random().Next(0, 2)) ? 0 : 1));
					instrs.Add(Instruction.Create(OpCodes.Box, Method.Module.CorLibTypes.Int32.TypeDefOrRef));
				}
				Instruction item = Instruction.Create(OpCodes.Pop);
				instrs.Add(Instruction.Create(OpCodes.Brfalse, instrs[0]));
				instrs.Add(Instruction.Create(OpCodes.Ldc_I4, Convert.ToBoolean(new Random().Next(0, 2)) ? 0 : 1));
				instrs.Add(item);
			}
			instrs.Add(Instruction.Create(OpCodes.Br, target));
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x0000A868 File Offset: 0x00008A68
		public void AddJunk(IList<Instruction> instrs, MethodDef Method)
		{
			bool isClr = Method.Module.IsClr40;
			if (!isClr)
			{
				instrs.Add(Instruction.Create(OpCodes.Pop));
				instrs.Add(Instruction.Create(OpCodes.Dup));
				instrs.Add(Instruction.Create(OpCodes.Throw));
				instrs.Add(Instruction.Create(OpCodes.Ldarg, new Parameter(255)));
				instrs.Add(Instruction.Create(OpCodes.Ldloc, new Local(null, null, 255)));
				instrs.Add(Instruction.Create(OpCodes.Ldtoken, Method));
			}
		}

		// Token: 0x04000043 RID: 67
		private static Random rnd = new Random();

		// Token: 0x02000049 RID: 73
		private struct Trace
		{
			// Token: 0x0600016B RID: 363 RVA: 0x0000EB00 File Offset: 0x0000CD00
			private static void Increment(Dictionary<uint, int> counts, uint key)
			{
				int num;
				bool flag = !counts.TryGetValue(key, out num);
				if (flag)
				{
					num = 0;
				}
				counts[key] = num + 1;
			}

			// Token: 0x0600016C RID: 364 RVA: 0x0000EB2C File Offset: 0x0000CD2C
			public Trace(CilBody body, bool hasReturnValue)
			{
				this.RefCount = new Dictionary<uint, int>();
				this.BrRefs = new Dictionary<uint, List<Instruction>>();
				this.BeforeStack = new Dictionary<uint, int>();
				this.AfterStack = new Dictionary<uint, int>();
				body.UpdateInstructionOffsets();
				foreach (ExceptionHandler exceptionHandler in body.ExceptionHandlers)
				{
					this.BeforeStack[exceptionHandler.TryStart.Offset] = 0;
					this.BeforeStack[exceptionHandler.HandlerStart.Offset] = ((exceptionHandler.HandlerType != ExceptionHandlerType.Finally) ? 1 : 0);
					bool flag = exceptionHandler.FilterStart != null;
					if (flag)
					{
						this.BeforeStack[exceptionHandler.FilterStart.Offset] = 1;
					}
				}
				int value = 0;
				int i = 0;
				while (i < body.Instructions.Count)
				{
					Instruction instruction = body.Instructions[i];
					bool flag2 = this.BeforeStack.ContainsKey(instruction.Offset);
					if (flag2)
					{
						value = this.BeforeStack[instruction.Offset];
					}
					this.BeforeStack[instruction.Offset] = value;
					instruction.UpdateStack(ref value, hasReturnValue);
					this.AfterStack[instruction.Offset] = value;
					switch (instruction.OpCode.FlowControl)
					{
					case FlowControl.Branch:
					{
						uint offset = ((Instruction)instruction.Operand).Offset;
						bool flag3 = !this.BeforeStack.ContainsKey(offset);
						if (flag3)
						{
							this.BeforeStack[offset] = value;
						}
						SwitchMangler.Trace.Increment(this.RefCount, offset);
						this.BrRefs.AddListEntry(offset, instruction);
						value = 0;
						break;
					}
					case FlowControl.Break:
					case FlowControl.Meta:
					case FlowControl.Next:
						goto IL_2F9;
					case FlowControl.Call:
					{
						bool flag4 = instruction.OpCode.Code == Code.Jmp;
						if (flag4)
						{
							value = 0;
						}
						goto IL_2F9;
					}
					case FlowControl.Cond_Branch:
					{
						bool flag5 = instruction.OpCode.Code == Code.Switch;
						if (flag5)
						{
							foreach (Instruction instruction2 in (Instruction[])instruction.Operand)
							{
								bool flag6 = !this.BeforeStack.ContainsKey(instruction2.Offset);
								if (flag6)
								{
									this.BeforeStack[instruction2.Offset] = value;
								}
								SwitchMangler.Trace.Increment(this.RefCount, instruction2.Offset);
								this.BrRefs.AddListEntry(instruction2.Offset, instruction);
							}
						}
						else
						{
							uint offset = ((Instruction)instruction.Operand).Offset;
							bool flag7 = !this.BeforeStack.ContainsKey(offset);
							if (flag7)
							{
								this.BeforeStack[offset] = value;
							}
							SwitchMangler.Trace.Increment(this.RefCount, offset);
							this.BrRefs.AddListEntry(offset, instruction);
						}
						goto IL_2F9;
					}
					case FlowControl.Phi:
						goto IL_2F3;
					case FlowControl.Return:
					case FlowControl.Throw:
						break;
					default:
						goto IL_2F3;
					}
					IL_337:
					i++;
					continue;
					IL_2F9:
					bool flag8 = i + 1 < body.Instructions.Count;
					if (flag8)
					{
						uint offset = body.Instructions[i + 1].Offset;
						SwitchMangler.Trace.Increment(this.RefCount, offset);
					}
					goto IL_337;
					IL_2F3:
					throw new Exception();
				}
			}

			// Token: 0x0600016D RID: 365 RVA: 0x0000EEA0 File Offset: 0x0000D0A0
			public bool IsBranchTarget(uint offset)
			{
				List<Instruction> list;
				bool flag = this.BrRefs.TryGetValue(offset, out list);
				return flag && list.Count > 0;
			}

			// Token: 0x0600016E RID: 366 RVA: 0x0000EED4 File Offset: 0x0000D0D4
			public bool HasMultipleSources(uint offset)
			{
				int num;
				bool flag = this.RefCount.TryGetValue(offset, out num);
				return flag && num > 1;
			}

			// Token: 0x0400009C RID: 156
			public Dictionary<uint, int> RefCount;

			// Token: 0x0400009D RID: 157
			public Dictionary<uint, List<Instruction>> BrRefs;

			// Token: 0x0400009E RID: 158
			public Dictionary<uint, int> BeforeStack;

			// Token: 0x0400009F RID: 159
			public Dictionary<uint, int> AfterStack;
		}
	}
}
