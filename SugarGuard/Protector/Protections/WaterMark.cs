using System;
using SugarGuard.Protector.Class;

namespace SugarGuard.Protector.Protections
{
	// Token: 0x0200000C RID: 12
	public class WaterMark
	{
		// Token: 0x0600002B RID: 43 RVA: 0x00004CD8 File Offset: 0x00002ED8
		public WaterMark(SugarLib lib)
		{
			this.Main(lib);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00004CE9 File Offset: 0x00002EE9
		private void Main(SugarLib lib)
		{
			lib.assembly.Name = "[Ϩ] Sugar Guard";
			lib.moduleDef.Name = "[ッ] Sugary";
		}
	}
}
