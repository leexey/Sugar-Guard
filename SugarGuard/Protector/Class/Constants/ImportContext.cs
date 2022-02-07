using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;

namespace SugarGuard.Protector.Class.Constants
{
	// Token: 0x02000035 RID: 53
	public abstract class ImportContext
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000115 RID: 277 RVA: 0x0000DFEE File Offset: 0x0000C1EE
		// (set) Token: 0x06000116 RID: 278 RVA: 0x0000DFF6 File Offset: 0x0000C1F6
		public ModuleDef Module { get; private set; }

		// Token: 0x06000117 RID: 279 RVA: 0x0000DFFF File Offset: 0x0000C1FF
		public ImportContext(ModuleDef module)
		{
			this.Module = module;
		}

		// Token: 0x06000118 RID: 280
		public abstract void Initialize();

		// Token: 0x06000119 RID: 281 RVA: 0x0000E027 File Offset: 0x0000C227
		public void AddRefImport(IMemberRef mRef)
		{
			this.importedRefs.Add(mRef);
		}

		// Token: 0x0600011A RID: 282 RVA: 0x0000E037 File Offset: 0x0000C237
		public void AddTypeImport(IType type)
		{
			this.importedTypes.Add(type);
		}

		// Token: 0x0600011B RID: 283 RVA: 0x0000E048 File Offset: 0x0000C248
		public T GetRefImport<T>(string name)
		{
			return (T)((object)this.importedRefs.FirstOrDefault((IMemberRef x) => x.Name == name));
		}

		// Token: 0x0600011C RID: 284 RVA: 0x0000E084 File Offset: 0x0000C284
		public T GetTypeImport<T>(string name)
		{
			return (T)((object)this.importedTypes.FirstOrDefault((IType x) => x.Name == name));
		}

		// Token: 0x04000073 RID: 115
		private List<IMemberRef> importedRefs = new List<IMemberRef>();

		// Token: 0x04000074 RID: 116
		private List<IType> importedTypes = new List<IType>();
	}
}
