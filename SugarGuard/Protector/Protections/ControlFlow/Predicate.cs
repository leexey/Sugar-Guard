using System;
using System.Collections.Generic;
using dnlib.DotNet.Emit;
using SugarGuard.Protector.Class;

namespace SugarGuard.Protector.Protections.ControlFlow
{
	// Token: 0x02000027 RID: 39
	internal class Predicate : IPredicate
	{
		// Token: 0x060000B3 RID: 179 RVA: 0x00009498 File Offset: 0x00007698
		public Predicate(SugarLib ctx)
		{
			this.ctx = ctx;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x000094AC File Offset: 0x000076AC
		public void Init(CilBody body)
		{
			bool flag = this.inited;
			if (!flag)
			{
				this.xorKey = new Random().Next();
				this.inited = true;
			}
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000094DD File Offset: 0x000076DD
		public void EmitSwitchLoad(IList<Instruction> instrs)
		{
			instrs.Add(Instruction.Create(OpCodes.Ldc_I4, this.xorKey));
			instrs.Add(Instruction.Create(OpCodes.Xor));
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00009508 File Offset: 0x00007708
		public int GetSwitchKey(int key)
		{
			return key ^ this.xorKey;
		}

		// Token: 0x0400003F RID: 63
		private readonly SugarLib ctx;

		// Token: 0x04000040 RID: 64
		private bool inited;

		// Token: 0x04000041 RID: 65
		private int xorKey;
	}
}
