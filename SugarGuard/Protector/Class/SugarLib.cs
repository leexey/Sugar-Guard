using System;
using System.IO;
using dnlib.DotNet;
using dnlib.DotNet.Writer;
using SugarGuard.Protector.Enums;

namespace SugarGuard.Protector.Class
{
	// Token: 0x02000031 RID: 49
	public class SugarLib
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x0000C50E File Offset: 0x0000A70E
		// (set) Token: 0x060000E2 RID: 226 RVA: 0x0000C516 File Offset: 0x0000A716
		public string filePath { get; private set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x0000C51F File Offset: 0x0000A71F
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x0000C527 File Offset: 0x0000A727
		public AssemblyDef assembly { get; private set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x0000C530 File Offset: 0x0000A730
		// (set) Token: 0x060000E6 RID: 230 RVA: 0x0000C538 File Offset: 0x0000A738
		public ModuleDef moduleDef { get; private set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x0000C541 File Offset: 0x0000A741
		// (set) Token: 0x060000E8 RID: 232 RVA: 0x0000C549 File Offset: 0x0000A749
		public TypeDef globalType { get; private set; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x0000C552 File Offset: 0x0000A752
		// (set) Token: 0x060000EA RID: 234 RVA: 0x0000C55A File Offset: 0x0000A75A
		public MethodDef ctor { get; private set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000EB RID: 235 RVA: 0x0000C563 File Offset: 0x0000A763
		// (set) Token: 0x060000EC RID: 236 RVA: 0x0000C56B File Offset: 0x0000A76B
		public NativeModuleWriterOptions nativeModuleWriterOptions { get; private set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000ED RID: 237 RVA: 0x0000C574 File Offset: 0x0000A774
		// (set) Token: 0x060000EE RID: 238 RVA: 0x0000C57C File Offset: 0x0000A77C
		public ModuleWriterOptions moduleWriterOptions { get; private set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x060000EF RID: 239 RVA: 0x0000C585 File Offset: 0x0000A785
		// (set) Token: 0x060000F0 RID: 240 RVA: 0x0000C58D File Offset: 0x0000A78D
		public bool noThrowInstance { get; set; }

		// Token: 0x060000F1 RID: 241 RVA: 0x0000C598 File Offset: 0x0000A798
		public SugarLib(string file)
		{
			this.filePath = file;
			this.assembly = AssemblyDef.Load(file, null);
			this.moduleDef = this.assembly.ManifestModule;
			this.globalType = this.assembly.ManifestModule.GlobalType;
			this.ctor = this.assembly.ManifestModule.GlobalType.FindOrCreateStaticConstructor();
			this.noThrowInstance = false;
			this.nativeModuleWriterOptions = new NativeModuleWriterOptions(this.moduleDef as ModuleDefMD, true)
			{
				MetadataLogger = DummyLogger.NoThrowInstance
			};
			this.moduleWriterOptions = new ModuleWriterOptions(this.moduleDef)
			{
				MetadataLogger = DummyLogger.NoThrowInstance
			};
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x0000C652 File Offset: 0x0000A852
		private void Listener()
		{
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000C658 File Offset: 0x0000A858
		private string NewName()
		{
			return string.Concat(new string[]
			{
				Path.GetDirectoryName(this.filePath),
				"//",
				Path.GetFileNameWithoutExtension(this.filePath),
				"_Sugary",
				Path.GetExtension(this.filePath)
			});
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x0000C6B0 File Offset: 0x0000A8B0
		public void buildASM(saveMode mode)
		{
			bool flag = mode == saveMode.Normal;
			if (flag)
			{
				this.moduleWriterOptions.MetadataOptions.Flags = (MetadataFlags.AlwaysCreateGuidHeap | MetadataFlags.AlwaysCreateStringsHeap | MetadataFlags.AlwaysCreateUSHeap | MetadataFlags.AlwaysCreateBlobHeap);
				this.moduleDef.Write(this.NewName(), this.moduleWriterOptions);
			}
			else
			{
				bool flag2 = mode == saveMode.x86;
				if (flag2)
				{
					this.nativeModuleWriterOptions.MetadataOptions.Flags = (MetadataFlags.AlwaysCreateGuidHeap | MetadataFlags.AlwaysCreateStringsHeap | MetadataFlags.AlwaysCreateUSHeap | MetadataFlags.AlwaysCreateBlobHeap);
					(this.moduleDef as ModuleDefMD).NativeWrite(this.NewName(), this.nativeModuleWriterOptions);
				}
			}
		}
	}
}
