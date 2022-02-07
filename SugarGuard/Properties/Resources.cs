using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace SugarGuard.Properties
{
	// Token: 0x02000038 RID: 56
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Resources
	{
		// Token: 0x06000127 RID: 295 RVA: 0x0000E2D8 File Offset: 0x0000C4D8
		internal Resources()
		{
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000128 RID: 296 RVA: 0x0000E2E4 File Offset: 0x0000C4E4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				bool flag = Resources.resourceMan == null;
				if (flag)
				{
					ResourceManager resourceManager = new ResourceManager("SugarGuard.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000129 RID: 297 RVA: 0x0000E32C File Offset: 0x0000C52C
		// (set) Token: 0x0600012A RID: 298 RVA: 0x0000E343 File Offset: 0x0000C543
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600012B RID: 299 RVA: 0x0000E34C File Offset: 0x0000C54C
		internal static Bitmap Delete
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("Delete", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600012C RID: 300 RVA: 0x0000E37C File Offset: 0x0000C57C
		internal static Bitmap Logo
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("Logo", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}

		// Token: 0x04000079 RID: 121
		private static ResourceManager resourceMan;

		// Token: 0x0400007A RID: 122
		private static CultureInfo resourceCulture;
	}
}
