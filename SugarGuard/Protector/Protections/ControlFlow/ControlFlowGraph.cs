using System;
using System.Collections;
using System.Collections.Generic;
using dnlib.DotNet.Emit;

namespace SugarGuard.Protector.Protections.ControlFlow
{
	// Token: 0x02000022 RID: 34
	public class ControlFlowGraph : IEnumerable<ControlFlowBlock>, IEnumerable
	{
		// Token: 0x0600009B RID: 155 RVA: 0x00008BB8 File Offset: 0x00006DB8
		private ControlFlowGraph(CilBody body)
		{
			this.body = body;
			this.instrBlocks = new int[body.Instructions.Count];
			this.blocks = new List<ControlFlowBlock>();
			this.indexMap = new Dictionary<Instruction, int>();
			for (int i = 0; i < body.Instructions.Count; i++)
			{
				this.indexMap.Add(body.Instructions[i], i);
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600009C RID: 156 RVA: 0x00008C34 File Offset: 0x00006E34
		public int Count
		{
			get
			{
				return this.blocks.Count;
			}
		}

		// Token: 0x1700000F RID: 15
		public ControlFlowBlock this[int id]
		{
			get
			{
				return this.blocks[id];
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600009E RID: 158 RVA: 0x00008C74 File Offset: 0x00006E74
		public CilBody Body
		{
			get
			{
				return this.body;
			}
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00008C8C File Offset: 0x00006E8C
		IEnumerator<ControlFlowBlock> IEnumerable<ControlFlowBlock>.GetEnumerator()
		{
			return this.blocks.GetEnumerator();
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00008CB0 File Offset: 0x00006EB0
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.blocks.GetEnumerator();
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00008CD4 File Offset: 0x00006ED4
		public ControlFlowBlock GetContainingBlock(int instrIndex)
		{
			return this.blocks[this.instrBlocks[instrIndex]];
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00008CFC File Offset: 0x00006EFC
		public int IndexOf(Instruction instr)
		{
			return this.indexMap[instr];
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00008D1C File Offset: 0x00006F1C
		private void PopulateBlockHeaders(HashSet<Instruction> blockHeaders, HashSet<Instruction> entryHeaders)
		{
			for (int i = 0; i < this.body.Instructions.Count; i++)
			{
				Instruction instruction = this.body.Instructions[i];
				bool flag = instruction.Operand is Instruction;
				if (flag)
				{
					blockHeaders.Add((Instruction)instruction.Operand);
					bool flag2 = i + 1 < this.body.Instructions.Count;
					if (flag2)
					{
						blockHeaders.Add(this.body.Instructions[i + 1]);
					}
				}
				else
				{
					bool flag3 = instruction.Operand is Instruction[];
					if (flag3)
					{
						foreach (Instruction item in (Instruction[])instruction.Operand)
						{
							blockHeaders.Add(item);
						}
						bool flag4 = i + 1 < this.body.Instructions.Count;
						if (flag4)
						{
							blockHeaders.Add(this.body.Instructions[i + 1]);
						}
					}
					else
					{
						bool flag5 = (instruction.OpCode.FlowControl == FlowControl.Throw || instruction.OpCode.FlowControl == FlowControl.Return) && i + 1 < this.body.Instructions.Count;
						if (flag5)
						{
							blockHeaders.Add(this.body.Instructions[i + 1]);
						}
					}
				}
			}
			blockHeaders.Add(this.body.Instructions[0]);
			foreach (ExceptionHandler exceptionHandler in this.body.ExceptionHandlers)
			{
				blockHeaders.Add(exceptionHandler.TryStart);
				blockHeaders.Add(exceptionHandler.HandlerStart);
				blockHeaders.Add(exceptionHandler.FilterStart);
				entryHeaders.Add(exceptionHandler.HandlerStart);
				entryHeaders.Add(exceptionHandler.FilterStart);
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00008F40 File Offset: 0x00007140
		private void SplitBlocks(HashSet<Instruction> blockHeaders, HashSet<Instruction> entryHeaders)
		{
			int num = 0;
			int num2 = -1;
			Instruction instruction = null;
			for (int i = 0; i < this.body.Instructions.Count; i++)
			{
				Instruction instruction2 = this.body.Instructions[i];
				bool flag = blockHeaders.Contains(instruction2);
				if (flag)
				{
					bool flag2 = instruction != null;
					if (flag2)
					{
						Instruction instruction3 = this.body.Instructions[i - 1];
						ControlFlowBlockType controlFlowBlockType = ControlFlowBlockType.Normal;
						bool flag3 = entryHeaders.Contains(instruction) || instruction == this.body.Instructions[0];
						if (flag3)
						{
							controlFlowBlockType |= ControlFlowBlockType.Entry;
						}
						bool flag4 = instruction3.OpCode.FlowControl == FlowControl.Return || instruction3.OpCode.FlowControl == FlowControl.Throw;
						if (flag4)
						{
							controlFlowBlockType |= ControlFlowBlockType.Exit;
						}
						this.blocks.Add(new ControlFlowBlock(num2, controlFlowBlockType, instruction, instruction3));
					}
					num2 = num++;
					instruction = instruction2;
				}
				this.instrBlocks[i] = num2;
			}
			bool flag5 = this.blocks.Count == 0 || this.blocks[this.blocks.Count - 1].Id != num2;
			if (flag5)
			{
				Instruction instruction4 = this.body.Instructions[this.body.Instructions.Count - 1];
				ControlFlowBlockType controlFlowBlockType2 = ControlFlowBlockType.Normal;
				bool flag6 = entryHeaders.Contains(instruction) || instruction == this.body.Instructions[0];
				if (flag6)
				{
					controlFlowBlockType2 |= ControlFlowBlockType.Entry;
				}
				bool flag7 = instruction4.OpCode.FlowControl == FlowControl.Return || instruction4.OpCode.FlowControl == FlowControl.Throw;
				if (flag7)
				{
					controlFlowBlockType2 |= ControlFlowBlockType.Exit;
				}
				this.blocks.Add(new ControlFlowBlock(num2, controlFlowBlockType2, instruction, instruction4));
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00009124 File Offset: 0x00007324
		private void LinkBlocks()
		{
			for (int i = 0; i < this.body.Instructions.Count; i++)
			{
				Instruction instruction = this.body.Instructions[i];
				bool flag = instruction.Operand is Instruction;
				if (flag)
				{
					ControlFlowBlock controlFlowBlock = this.blocks[this.instrBlocks[i]];
					ControlFlowBlock controlFlowBlock2 = this.blocks[this.instrBlocks[this.indexMap[(Instruction)instruction.Operand]]];
					controlFlowBlock2.Sources.Add(controlFlowBlock);
					controlFlowBlock.Targets.Add(controlFlowBlock2);
				}
				else
				{
					bool flag2 = instruction.Operand is Instruction[];
					if (flag2)
					{
						foreach (Instruction key in (Instruction[])instruction.Operand)
						{
							ControlFlowBlock controlFlowBlock3 = this.blocks[this.instrBlocks[i]];
							ControlFlowBlock controlFlowBlock4 = this.blocks[this.instrBlocks[this.indexMap[key]]];
							controlFlowBlock4.Sources.Add(controlFlowBlock3);
							controlFlowBlock3.Targets.Add(controlFlowBlock4);
						}
					}
				}
			}
			for (int k = 0; k < this.blocks.Count; k++)
			{
				bool flag3 = this.blocks[k].Footer.OpCode.FlowControl != FlowControl.Branch && this.blocks[k].Footer.OpCode.FlowControl != FlowControl.Return && this.blocks[k].Footer.OpCode.FlowControl != FlowControl.Throw;
				if (flag3)
				{
					this.blocks[k].Targets.Add(this.blocks[k + 1]);
					this.blocks[k + 1].Sources.Add(this.blocks[k]);
				}
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00009358 File Offset: 0x00007558
		public static ControlFlowGraph Construct(CilBody body)
		{
			ControlFlowGraph controlFlowGraph = new ControlFlowGraph(body);
			bool flag = body.Instructions.Count == 0;
			ControlFlowGraph result;
			if (flag)
			{
				result = controlFlowGraph;
			}
			else
			{
				HashSet<Instruction> blockHeaders = new HashSet<Instruction>();
				HashSet<Instruction> entryHeaders = new HashSet<Instruction>();
				controlFlowGraph.PopulateBlockHeaders(blockHeaders, entryHeaders);
				controlFlowGraph.SplitBlocks(blockHeaders, entryHeaders);
				controlFlowGraph.LinkBlocks();
				result = controlFlowGraph;
			}
			return result;
		}

		// Token: 0x04000031 RID: 49
		private readonly List<ControlFlowBlock> blocks;

		// Token: 0x04000032 RID: 50
		private readonly CilBody body;

		// Token: 0x04000033 RID: 51
		private readonly int[] instrBlocks;

		// Token: 0x04000034 RID: 52
		private readonly Dictionary<Instruction, int> indexMap;
	}
}
