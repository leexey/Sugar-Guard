using System;
using System.Collections.Generic;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using SugarGuard.Protector.Class;

namespace SugarGuard.Protector.Protections.ControlFlow
{
	// Token: 0x02000025 RID: 37
	internal abstract class ManglerBase
	{
		// Token: 0x060000AD RID: 173 RVA: 0x0000947F File Offset: 0x0000767F
		protected static IEnumerable<BlockParser.InstrBlock> GetAllBlocks(BlockParser.ScopeBlock scope)
		{
			foreach (BlockParser.BlockBase child in scope.Children)
			{
				bool flag = child is BlockParser.InstrBlock;
				if (flag)
				{
					yield return (BlockParser.InstrBlock)child;
				}
				else
				{
					foreach (BlockParser.InstrBlock block in ManglerBase.GetAllBlocks((BlockParser.ScopeBlock)child))
					{
						yield return block;
						block = null;
					}
					IEnumerator<BlockParser.InstrBlock> enumerator2 = null;
				}
				child = null;
			}
			List<BlockParser.BlockBase>.Enumerator enumerator = default(List<BlockParser.BlockBase>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060000AE RID: 174
		public abstract void Mangle(CilBody body, BlockParser.ScopeBlock root, SugarLib ctx, MethodDef method, TypeSig retType);
	}
}
