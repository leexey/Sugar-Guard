using System;
using dnlib.DotNet;

namespace SugarGuard.Protector.Class.Constants
{
	// Token: 0x02000037 RID: 55
	public class EncodedMethod
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000122 RID: 290 RVA: 0x0000E29C File Offset: 0x0000C49C
		// (set) Token: 0x06000123 RID: 291 RVA: 0x0000E2A4 File Offset: 0x0000C4A4
		public MethodDef eMethod { get; private set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000124 RID: 292 RVA: 0x0000E2AD File Offset: 0x0000C4AD
		// (set) Token: 0x06000125 RID: 293 RVA: 0x0000E2B5 File Offset: 0x0000C4B5
		public int eNum { get; private set; }

		// Token: 0x06000126 RID: 294 RVA: 0x0000E2BE File Offset: 0x0000C4BE
		public EncodedMethod(MethodDef method, int num)
		{
			this.eMethod = method;
			this.eNum = num;
		}
	}
}
