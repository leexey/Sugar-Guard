using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace SugarGuard.Protector.Class.Constants
{
	// Token: 0x02000034 RID: 52
	public class DynamicContext : ImportContext
	{
		// Token: 0x0600010E RID: 270 RVA: 0x0000DA39 File Offset: 0x0000BC39
		public DynamicContext(ModuleDef module) : base(module)
		{
			this.LocalRefs = new Dictionary<Local, Local>();
			this.BranchRefs = new Dictionary<Instruction, Local>();
			this.ExceptionHandlers = new List<dnlib.DotNet.Emit.ExceptionHandler>();
		}

		// Token: 0x0600010F RID: 271 RVA: 0x0000DA68 File Offset: 0x0000BC68
		public override void Initialize()
		{
			base.AddTypeImport(base.Module.ImportAsTypeSig(typeof(ILGenerator)));
			base.AddTypeImport(base.Module.ImportAsTypeSig(typeof(DynamicMethod)));
			base.AddTypeImport(base.Module.ImportAsTypeSig(typeof(LocalBuilder)));
			base.AddTypeImport(base.Module.ImportAsTypeSig(typeof(Type)));
			base.AddTypeImport(base.Module.ImportAsTypeSig(typeof(MethodInfo)));
			base.AddTypeImport(base.Module.ImportAsTypeSig(typeof(Label)));
			base.AddTypeImport(base.Module.ImportAsTypeSig(typeof(object)));
			base.AddRefImport(base.Module.Import(typeof(Type).GetMethod("GetTypeFromHandle", new Type[]
			{
				typeof(RuntimeTypeHandle)
			})));
			base.AddRefImport(base.Module.Import(typeof(DynamicMethod).GetConstructor(new Type[]
			{
				typeof(string),
				typeof(System.Reflection.MethodAttributes),
				typeof(CallingConventions),
				typeof(Type),
				typeof(Type[]),
				typeof(Type),
				typeof(bool)
			})));
			base.AddRefImport(base.Module.Import(typeof(DynamicMethod).GetMethod("GetILGenerator", new Type[0])));
			base.AddRefImport(base.Module.Import(typeof(ILGenerator).GetMethod("DeclareLocal", new Type[]
			{
				typeof(Type)
			})));
			base.AddRefImport(base.Module.Import(typeof(Type).GetMethod("get_Module")));
			base.AddRefImport(base.Module.Import(typeof(MethodBase).GetMethod("Invoke", new Type[]
			{
				typeof(object),
				typeof(object[])
			})));
			base.AddRefImport(base.Module.Import(typeof(ILGenerator).GetMethod("DefineLabel")));
			base.AddRefImport(base.Module.Import(typeof(ILGenerator).GetMethod("MarkLabel", new Type[]
			{
				typeof(Label)
			})));
			base.AddRefImport(base.Module.Import(typeof(MethodInfo).GetMethod("GetBaseDefinition")));
			base.AddRefImport(base.Module.Import(typeof(Type).GetMethod("GetField", new Type[]
			{
				typeof(string),
				typeof(BindingFlags)
			})));
			base.AddRefImport(base.Module.Import(typeof(ILGenerator).GetMethod("BeginExceptionBlock")));
			base.AddRefImport(base.Module.Import(typeof(ILGenerator).GetMethod("BeginCatchBlock", new Type[]
			{
				typeof(Type)
			})));
			base.AddRefImport(base.Module.Import(typeof(ILGenerator).GetMethod("BeginExceptFilterBlock")));
			base.AddRefImport(base.Module.Import(typeof(ILGenerator).GetMethod("BeginFinallyBlock")));
			base.AddRefImport(base.Module.Import(typeof(ILGenerator).GetMethod("BeginFaultBlock")));
			base.AddRefImport(base.Module.Import(typeof(ILGenerator).GetMethod("EndExceptionBlock")));
		}

		// Token: 0x06000110 RID: 272 RVA: 0x0000DE88 File Offset: 0x0000C088
		public bool IsBranchTarget(Instruction instruction, out Local label)
		{
			label = this.BranchRefs.FirstOrDefault((KeyValuePair<Instruction, Local> x) => (Instruction)x.Key.Operand == instruction).Value;
			return label != null;
		}

		// Token: 0x06000111 RID: 273 RVA: 0x0000DED0 File Offset: 0x0000C0D0
		public bool IsExceptionStart(Instruction instruction)
		{
			return this.ExceptionHandlers.FirstOrDefault((dnlib.DotNet.Emit.ExceptionHandler x) => x.TryStart == instruction) != null;
		}

		// Token: 0x06000112 RID: 274 RVA: 0x0000DF0C File Offset: 0x0000C10C
		public bool IsExceptionEnd(Instruction instruction)
		{
			return this.ExceptionHandlers.FirstOrDefault((dnlib.DotNet.Emit.ExceptionHandler x) => x.HandlerEnd == instruction) != null;
		}

		// Token: 0x06000113 RID: 275 RVA: 0x0000DF48 File Offset: 0x0000C148
		public bool IsHandlerStart(Instruction instruction, out MemberRef beginMethod, out dnlib.DotNet.Emit.ExceptionHandler ex)
		{
			ex = this.ExceptionHandlers.FirstOrDefault((dnlib.DotNet.Emit.ExceptionHandler x) => x.HandlerStart == instruction);
			beginMethod = null;
			bool flag = ex == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				string name = (ex.HandlerType == ExceptionHandlerType.Filter) ? this.FormatName("ExceptFilter") : this.FormatName(ex.HandlerType.ToString());
				beginMethod = base.GetRefImport<MemberRef>(name);
				result = true;
			}
			return result;
		}

		// Token: 0x06000114 RID: 276 RVA: 0x0000DFCC File Offset: 0x0000C1CC
		private string FormatName(string handlerName)
		{
			return "Begin" + handlerName + "Block";
		}

		// Token: 0x0400006C RID: 108
		public Dictionary<Instruction, Local> BranchRefs;

		// Token: 0x0400006D RID: 109
		public Dictionary<Local, Local> LocalRefs;

		// Token: 0x0400006E RID: 110
		public List<dnlib.DotNet.Emit.ExceptionHandler> ExceptionHandlers;

		// Token: 0x0400006F RID: 111
		public Instruction Branch;

		// Token: 0x04000070 RID: 112
		public Local ILGenerator;

		// Token: 0x04000071 RID: 113
		public Local DynamicMethod;

		// Token: 0x04000072 RID: 114
		public Local MethodInfo;
	}
}
