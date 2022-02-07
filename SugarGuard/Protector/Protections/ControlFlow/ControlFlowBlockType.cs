using System;

namespace SugarGuard.Protector.Protections.ControlFlow
{
	// Token: 0x02000023 RID: 35
	[Flags]
	public enum ControlFlowBlockType
	{
		// Token: 0x04000036 RID: 54
		Normal = 0,
		// Token: 0x04000037 RID: 55
		Entry = 1,
		// Token: 0x04000038 RID: 56
		Exit = 2
	}
}
