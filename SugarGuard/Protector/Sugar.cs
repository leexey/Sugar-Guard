using System;
using System.Collections.Generic;
using SugarGuard.Protector.Class;
using SugarGuard.Protector.Enums;
using SugarGuard.Protector.Protections;
using SugarGuard.Protector.Protections.Constants;
using SugarGuard.Protector.Protections.ControlFlow;
using SugarGuard.Protector.Protections.Mutation;
using SugarGuard.Protector.Protections.ReferenceProxy;

namespace SugarGuard.Protector
{
	// Token: 0x02000004 RID: 4
	public class Sugar
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000010 RID: 16 RVA: 0x00003566 File Offset: 0x00001766
		// (set) Token: 0x06000011 RID: 17 RVA: 0x0000356E File Offset: 0x0000176E
		private SugarLib lib { get; set; }

		// Token: 0x06000012 RID: 18 RVA: 0x00003577 File Offset: 0x00001777
		public Sugar(string filePath)
		{
			this.lib = new SugarLib(filePath);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000359C File Offset: 0x0000179C
		public void Protect()
		{
			foreach (Protections protections in this.protections)
			{
				bool flag = protections == Protections.CallConvertion;
				if (flag)
				{
					new CallConvertion(this.lib);
				}
				bool flag2 = protections == Protections.Constants;
				if (flag2)
				{
					new Constants(this.lib);
				}
				bool flag3 = protections == Protections.VM;
				if (flag3)
				{
					bool flag4 = protections == Protections.ReferenceProxy;
					if (flag4)
					{
						new ReferenceProxy(this.lib);
					}
				}
				bool flag5 = protections == Protections.ControlFlow;
				if (flag5)
				{
					new ControlFlow(this.lib);
				}
				bool flag6 = protections == Protections.InvalidOpcodes;
				if (flag6)
				{
					new InvalidOpcodes(this.lib);
				}
				bool flag7 = protections == Protections.AntiDump;
				if (flag7)
				{
					new AntiDump(this.lib);
				}
				bool flag8 = protections == Protections.AntiDebug;
				if (flag8)
				{
					new AntiDebug(this.lib);
				}
				bool flag9 = protections == Protections.Mutation;
				if (flag9)
				{
					new Mutation(this.lib);
				}
			}
			foreach (Protections protections2 in this.protections)
			{
				bool flag10 = protections2 == Protections.PosConstants;
				if (flag10)
				{
					new PosConstants(this.lib);
				}
				bool flag11 = protections2 == Protections.Renamer;
				if (flag11)
				{
					new Renamer(this.lib);
				}
				bool flag12 = protections2 == Protections.FakeAttributes;
				if (flag12)
				{
					new FakeAttributes(this.lib);
				}
				bool flag13 = protections2 == Protections.WaterMark;
				if (flag13)
				{
					new WaterMark(this.lib);
				}
			}
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000374C File Offset: 0x0000194C
		public void Save()
		{
			this.lib.buildASM(saveMode.x86);
		}

		// Token: 0x04000014 RID: 20
		public List<Protections> protections = new List<Protections>();
	}
}
