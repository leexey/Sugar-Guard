using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet.Emit;

namespace SugarGuard.Protector.Protections.ControlFlow
{
	// Token: 0x02000024 RID: 36
	public class ControlFlowBlock
	{
		// Token: 0x060000A7 RID: 167 RVA: 0x000093B0 File Offset: 0x000075B0
		internal ControlFlowBlock(int id, ControlFlowBlockType type, Instruction header, Instruction footer)
		{
			this.Id = id;
			this.Type = type;
			this.Header = header;
			this.Footer = footer;
			this.Sources = new List<ControlFlowBlock>();
			this.Targets = new List<ControlFlowBlock>();
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x000093EF File Offset: 0x000075EF
		// (set) Token: 0x060000A9 RID: 169 RVA: 0x000093F7 File Offset: 0x000075F7
		public IList<ControlFlowBlock> Sources { get; private set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00009400 File Offset: 0x00007600
		// (set) Token: 0x060000AB RID: 171 RVA: 0x00009408 File Offset: 0x00007608
		public IList<ControlFlowBlock> Targets { get; private set; }

		// Token: 0x060000AC RID: 172 RVA: 0x00009414 File Offset: 0x00007614
		public override string ToString()
		{
			return string.Format("Block {0} => {1} {2}", this.Id, this.Type, string.Join(", ", (from block in this.Targets
			select block.Id.ToString()).ToArray<string>()));
		}

		// Token: 0x04000039 RID: 57
		public readonly Instruction Footer;

		// Token: 0x0400003A RID: 58
		public readonly Instruction Header;

		// Token: 0x0400003B RID: 59
		public readonly int Id;

		// Token: 0x0400003C RID: 60
		public readonly ControlFlowBlockType Type;
	}
}
