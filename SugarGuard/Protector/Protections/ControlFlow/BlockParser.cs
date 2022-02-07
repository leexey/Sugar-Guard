using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using dnlib.DotNet.Emit;

namespace SugarGuard.Protector.Protections.ControlFlow
{
	// Token: 0x02000021 RID: 33
	internal static class BlockParser
	{
		// Token: 0x0600009A RID: 154 RVA: 0x00008798 File Offset: 0x00006998
		public static BlockParser.ScopeBlock ParseBody(CilBody body)
		{
			Dictionary<ExceptionHandler, Tuple<BlockParser.ScopeBlock, BlockParser.ScopeBlock, BlockParser.ScopeBlock>> dictionary = new Dictionary<ExceptionHandler, Tuple<BlockParser.ScopeBlock, BlockParser.ScopeBlock, BlockParser.ScopeBlock>>();
			foreach (ExceptionHandler exceptionHandler in body.ExceptionHandlers)
			{
				BlockParser.ScopeBlock item = new BlockParser.ScopeBlock(BlockParser.BlockType.Try, exceptionHandler);
				BlockParser.BlockType type = BlockParser.BlockType.Handler;
				bool flag = exceptionHandler.HandlerType == ExceptionHandlerType.Finally;
				if (flag)
				{
					type = BlockParser.BlockType.Finally;
				}
				else
				{
					bool flag2 = exceptionHandler.HandlerType == ExceptionHandlerType.Fault;
					if (flag2)
					{
						type = BlockParser.BlockType.Fault;
					}
				}
				BlockParser.ScopeBlock item2 = new BlockParser.ScopeBlock(type, exceptionHandler);
				bool flag3 = exceptionHandler.FilterStart != null;
				if (flag3)
				{
					BlockParser.ScopeBlock item3 = new BlockParser.ScopeBlock(BlockParser.BlockType.Filter, exceptionHandler);
					dictionary[exceptionHandler] = Tuple.Create<BlockParser.ScopeBlock, BlockParser.ScopeBlock, BlockParser.ScopeBlock>(item, item2, item3);
				}
				else
				{
					dictionary[exceptionHandler] = Tuple.Create<BlockParser.ScopeBlock, BlockParser.ScopeBlock, BlockParser.ScopeBlock>(item, item2, null);
				}
			}
			BlockParser.ScopeBlock scopeBlock = new BlockParser.ScopeBlock(BlockParser.BlockType.Normal, null);
			Stack<BlockParser.ScopeBlock> stack = new Stack<BlockParser.ScopeBlock>();
			stack.Push(scopeBlock);
			foreach (Instruction instruction in body.Instructions)
			{
				foreach (ExceptionHandler exceptionHandler2 in body.ExceptionHandlers)
				{
					Tuple<BlockParser.ScopeBlock, BlockParser.ScopeBlock, BlockParser.ScopeBlock> tuple = dictionary[exceptionHandler2];
					bool flag4 = instruction == exceptionHandler2.TryEnd;
					if (flag4)
					{
						stack.Pop();
					}
					bool flag5 = instruction == exceptionHandler2.HandlerEnd;
					if (flag5)
					{
						stack.Pop();
					}
					bool flag6 = exceptionHandler2.FilterStart != null && instruction == exceptionHandler2.HandlerStart;
					if (flag6)
					{
						Debug.Assert(stack.Peek().Type == BlockParser.BlockType.Filter);
						stack.Pop();
					}
				}
				foreach (ExceptionHandler exceptionHandler3 in body.ExceptionHandlers.Reverse<ExceptionHandler>())
				{
					Tuple<BlockParser.ScopeBlock, BlockParser.ScopeBlock, BlockParser.ScopeBlock> tuple2 = dictionary[exceptionHandler3];
					BlockParser.ScopeBlock scopeBlock2 = (stack.Count > 0) ? stack.Peek() : null;
					bool flag7 = instruction == exceptionHandler3.TryStart;
					if (flag7)
					{
						bool flag8 = scopeBlock2 != null;
						if (flag8)
						{
							scopeBlock2.Children.Add(tuple2.Item1);
						}
						stack.Push(tuple2.Item1);
					}
					bool flag9 = instruction == exceptionHandler3.HandlerStart;
					if (flag9)
					{
						bool flag10 = scopeBlock2 != null;
						if (flag10)
						{
							scopeBlock2.Children.Add(tuple2.Item2);
						}
						stack.Push(tuple2.Item2);
					}
					bool flag11 = instruction == exceptionHandler3.FilterStart;
					if (flag11)
					{
						bool flag12 = scopeBlock2 != null;
						if (flag12)
						{
							scopeBlock2.Children.Add(tuple2.Item3);
						}
						stack.Push(tuple2.Item3);
					}
				}
				BlockParser.ScopeBlock scopeBlock3 = stack.Peek();
				BlockParser.InstrBlock instrBlock = scopeBlock3.Children.LastOrDefault<BlockParser.BlockBase>() as BlockParser.InstrBlock;
				bool flag13 = instrBlock == null;
				if (flag13)
				{
					scopeBlock3.Children.Add(instrBlock = new BlockParser.InstrBlock());
				}
				instrBlock.Instructions.Add(instruction);
			}
			foreach (ExceptionHandler exceptionHandler4 in body.ExceptionHandlers)
			{
				bool flag14 = exceptionHandler4.TryEnd == null;
				if (flag14)
				{
					stack.Pop();
				}
				bool flag15 = exceptionHandler4.HandlerEnd == null;
				if (flag15)
				{
					stack.Pop();
				}
			}
			Debug.Assert(stack.Count == 1);
			return scopeBlock;
		}

		// Token: 0x02000043 RID: 67
		internal abstract class BlockBase
		{
			// Token: 0x0600014C RID: 332 RVA: 0x0000E46B File Offset: 0x0000C66B
			public BlockBase(BlockParser.BlockType type)
			{
				this.Type = type;
			}

			// Token: 0x17000027 RID: 39
			// (get) Token: 0x0600014D RID: 333 RVA: 0x0000E47D File Offset: 0x0000C67D
			// (set) Token: 0x0600014E RID: 334 RVA: 0x0000E485 File Offset: 0x0000C685
			public BlockParser.BlockType Type { get; private set; }

			// Token: 0x0600014F RID: 335
			public abstract void ToBody(CilBody body);
		}

		// Token: 0x02000044 RID: 68
		internal enum BlockType
		{
			// Token: 0x04000088 RID: 136
			Normal,
			// Token: 0x04000089 RID: 137
			Try,
			// Token: 0x0400008A RID: 138
			Handler,
			// Token: 0x0400008B RID: 139
			Finally,
			// Token: 0x0400008C RID: 140
			Filter,
			// Token: 0x0400008D RID: 141
			Fault
		}

		// Token: 0x02000045 RID: 69
		internal class ScopeBlock : BlockParser.BlockBase
		{
			// Token: 0x06000150 RID: 336 RVA: 0x0000E48E File Offset: 0x0000C68E
			public ScopeBlock(BlockParser.BlockType type, ExceptionHandler handler) : base(type)
			{
				this.Handler = handler;
				this.Children = new List<BlockParser.BlockBase>();
			}

			// Token: 0x17000028 RID: 40
			// (get) Token: 0x06000151 RID: 337 RVA: 0x0000E4AD File Offset: 0x0000C6AD
			// (set) Token: 0x06000152 RID: 338 RVA: 0x0000E4B5 File Offset: 0x0000C6B5
			public ExceptionHandler Handler { get; private set; }

			// Token: 0x17000029 RID: 41
			// (get) Token: 0x06000153 RID: 339 RVA: 0x0000E4BE File Offset: 0x0000C6BE
			// (set) Token: 0x06000154 RID: 340 RVA: 0x0000E4C6 File Offset: 0x0000C6C6
			public List<BlockParser.BlockBase> Children { get; set; }

			// Token: 0x06000155 RID: 341 RVA: 0x0000E4D0 File Offset: 0x0000C6D0
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				bool flag = base.Type == BlockParser.BlockType.Try;
				if (flag)
				{
					stringBuilder.Append("try ");
				}
				else
				{
					bool flag2 = base.Type == BlockParser.BlockType.Handler;
					if (flag2)
					{
						stringBuilder.Append("handler ");
					}
					else
					{
						bool flag3 = base.Type == BlockParser.BlockType.Finally;
						if (flag3)
						{
							stringBuilder.Append("finally ");
						}
						else
						{
							bool flag4 = base.Type == BlockParser.BlockType.Fault;
							if (flag4)
							{
								stringBuilder.Append("fault ");
							}
						}
					}
				}
				stringBuilder.AppendLine("{");
				foreach (BlockParser.BlockBase value in this.Children)
				{
					stringBuilder.Append(value);
				}
				stringBuilder.AppendLine("}");
				return stringBuilder.ToString();
			}

			// Token: 0x06000156 RID: 342 RVA: 0x0000E5C0 File Offset: 0x0000C7C0
			public Instruction GetFirstInstr()
			{
				BlockParser.BlockBase blockBase = this.Children.First<BlockParser.BlockBase>();
				bool flag = blockBase is BlockParser.ScopeBlock;
				Instruction result;
				if (flag)
				{
					result = ((BlockParser.ScopeBlock)blockBase).GetFirstInstr();
				}
				else
				{
					result = ((BlockParser.InstrBlock)blockBase).Instructions.First<Instruction>();
				}
				return result;
			}

			// Token: 0x06000157 RID: 343 RVA: 0x0000E60C File Offset: 0x0000C80C
			public Instruction GetLastInstr()
			{
				BlockParser.BlockBase blockBase = this.Children.Last<BlockParser.BlockBase>();
				bool flag = blockBase is BlockParser.ScopeBlock;
				Instruction result;
				if (flag)
				{
					result = ((BlockParser.ScopeBlock)blockBase).GetLastInstr();
				}
				else
				{
					result = ((BlockParser.InstrBlock)blockBase).Instructions.Last<Instruction>();
				}
				return result;
			}

			// Token: 0x06000158 RID: 344 RVA: 0x0000E658 File Offset: 0x0000C858
			public override void ToBody(CilBody body)
			{
				bool flag = base.Type > BlockParser.BlockType.Normal;
				if (flag)
				{
					bool flag2 = base.Type == BlockParser.BlockType.Try;
					if (flag2)
					{
						this.Handler.TryStart = this.GetFirstInstr();
						this.Handler.TryEnd = this.GetLastInstr();
					}
					else
					{
						bool flag3 = base.Type == BlockParser.BlockType.Filter;
						if (flag3)
						{
							this.Handler.FilterStart = this.GetFirstInstr();
						}
						else
						{
							this.Handler.HandlerStart = this.GetFirstInstr();
							this.Handler.HandlerEnd = this.GetLastInstr();
						}
					}
				}
				foreach (BlockParser.BlockBase blockBase in this.Children)
				{
					blockBase.ToBody(body);
				}
			}
		}

		// Token: 0x02000046 RID: 70
		internal class InstrBlock : BlockParser.BlockBase
		{
			// Token: 0x06000159 RID: 345 RVA: 0x0000E73C File Offset: 0x0000C93C
			public InstrBlock() : base(BlockParser.BlockType.Normal)
			{
				this.Instructions = new List<Instruction>();
			}

			// Token: 0x1700002A RID: 42
			// (get) Token: 0x0600015A RID: 346 RVA: 0x0000E753 File Offset: 0x0000C953
			// (set) Token: 0x0600015B RID: 347 RVA: 0x0000E75B File Offset: 0x0000C95B
			public List<Instruction> Instructions { get; set; }

			// Token: 0x0600015C RID: 348 RVA: 0x0000E764 File Offset: 0x0000C964
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (Instruction instruction in this.Instructions)
				{
					stringBuilder.AppendLine(instruction.ToString());
				}
				return stringBuilder.ToString();
			}

			// Token: 0x0600015D RID: 349 RVA: 0x0000E7D0 File Offset: 0x0000C9D0
			public override void ToBody(CilBody body)
			{
				foreach (Instruction item in this.Instructions)
				{
					body.Instructions.Add(item);
				}
			}
		}
	}
}
