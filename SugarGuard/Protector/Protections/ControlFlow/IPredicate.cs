using System;
using System.Collections.Generic;
using dnlib.DotNet.Emit;

namespace SugarGuard.Protector.Protections.ControlFlow
{
	// Token: 0x02000026 RID: 38
	internal interface IPredicate
	{
		// Token: 0x060000B0 RID: 176
		void Init(CilBody body);

		// Token: 0x060000B1 RID: 177
		void EmitSwitchLoad(IList<Instruction> instrs);

		// Token: 0x060000B2 RID: 178
		int GetSwitchKey(int key);
	}
}
