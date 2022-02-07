using System;
using System.Collections.Generic;
using System.Linq;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace SugarGuard.Protector.Class
{
	// Token: 0x0200002E RID: 46
	public static class InjectHelper
	{
		// Token: 0x060000D1 RID: 209 RVA: 0x0000B544 File Offset: 0x00009744
		private static TypeDefUser Clone(TypeDef origin)
		{
			TypeDefUser typeDefUser = new TypeDefUser(origin.Namespace, origin.Name);
			typeDefUser.Attributes = origin.Attributes;
			bool flag = origin.ClassLayout != null;
			if (flag)
			{
				typeDefUser.ClassLayout = new ClassLayoutUser(origin.ClassLayout.PackingSize, origin.ClassSize);
			}
			foreach (GenericParam genericParam in origin.GenericParameters)
			{
				typeDefUser.GenericParameters.Add(new GenericParamUser(genericParam.Number, genericParam.Flags, "-"));
			}
			return typeDefUser;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x0000B604 File Offset: 0x00009804
		private static MethodDefUser Clone(MethodDef origin)
		{
			MethodDefUser methodDefUser = new MethodDefUser(origin.Name, null, origin.ImplAttributes, origin.Attributes);
			foreach (GenericParam genericParam in origin.GenericParameters)
			{
				methodDefUser.GenericParameters.Add(new GenericParamUser(genericParam.Number, genericParam.Flags, "-"));
			}
			return methodDefUser;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x0000B694 File Offset: 0x00009894
		private static FieldDefUser Clone(FieldDef origin)
		{
			return new FieldDefUser(origin.Name, null, origin.Attributes);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x0000B6BC File Offset: 0x000098BC
		private static TypeDef PopulateContext(TypeDef typeDef, InjectHelper.InjectContext ctx)
		{
			IDnlibDef dnlibDef;
			bool flag = !ctx.map.TryGetValue(typeDef, out dnlibDef);
			TypeDef typeDef2;
			if (flag)
			{
				typeDef2 = InjectHelper.Clone(typeDef);
				ctx.map[typeDef] = typeDef2;
			}
			else
			{
				typeDef2 = (TypeDef)dnlibDef;
			}
			foreach (TypeDef typeDef3 in typeDef.NestedTypes)
			{
				typeDef2.NestedTypes.Add(InjectHelper.PopulateContext(typeDef3, ctx));
			}
			foreach (MethodDef methodDef in typeDef.Methods)
			{
				typeDef2.Methods.Add((MethodDef)(ctx.map[methodDef] = InjectHelper.Clone(methodDef)));
			}
			foreach (FieldDef fieldDef in typeDef.Fields)
			{
				typeDef2.Fields.Add((FieldDef)(ctx.map[fieldDef] = InjectHelper.Clone(fieldDef)));
			}
			return typeDef2;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x0000B828 File Offset: 0x00009A28
		private static void CopyTypeDef(TypeDef typeDef, InjectHelper.InjectContext ctx)
		{
			TypeDef typeDef2 = (TypeDef)ctx.map[typeDef];
			typeDef2.BaseType = ctx.Importer.Import(typeDef.BaseType);
			foreach (InterfaceImpl interfaceImpl in typeDef.Interfaces)
			{
				typeDef2.Interfaces.Add(new InterfaceImplUser(ctx.Importer.Import(interfaceImpl.Interface)));
			}
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x0000B8C4 File Offset: 0x00009AC4
		private static void CopyMethodDef(MethodDef methodDef, InjectHelper.InjectContext ctx)
		{
			MethodDef methodDef2 = (MethodDef)ctx.map[methodDef];
			methodDef2.Signature = ctx.Importer.Import(methodDef.Signature);
			methodDef2.Parameters.UpdateParameterTypes();
			bool flag = methodDef.ImplMap != null;
			if (flag)
			{
				methodDef2.ImplMap = new ImplMapUser(new ModuleRefUser(ctx.TargetModule, methodDef.ImplMap.Module.Name), methodDef.ImplMap.Name, methodDef.ImplMap.Attributes);
			}
			foreach (CustomAttribute customAttribute in methodDef.CustomAttributes)
			{
				methodDef2.CustomAttributes.Add(new CustomAttribute((ICustomAttributeType)ctx.Importer.Import(customAttribute.Constructor)));
			}
			bool hasBody = methodDef.HasBody;
			if (hasBody)
			{
				methodDef2.Body = new CilBody(methodDef.Body.InitLocals, new List<Instruction>(), new List<ExceptionHandler>(), new List<Local>());
				methodDef2.Body.MaxStack = methodDef.Body.MaxStack;
				Dictionary<object, object> bodyMap = new Dictionary<object, object>();
				foreach (Local local in methodDef.Body.Variables)
				{
					Local local2 = new Local(ctx.Importer.Import(local.Type));
					methodDef2.Body.Variables.Add(local2);
					local2.Name = local.Name;
					bodyMap[local] = local2;
				}
				foreach (Instruction instruction in methodDef.Body.Instructions)
				{
					Instruction instruction2 = new Instruction(instruction.OpCode, instruction.Operand);
					instruction2.SequencePoint = instruction.SequencePoint;
					bool flag2 = instruction2.Operand is IType;
					if (flag2)
					{
						instruction2.Operand = ctx.Importer.Import((IType)instruction2.Operand);
					}
					else
					{
						bool flag3 = instruction2.Operand is IMethod;
						if (flag3)
						{
							instruction2.Operand = ctx.Importer.Import((IMethod)instruction2.Operand);
						}
						else
						{
							bool flag4 = instruction2.Operand is IField;
							if (flag4)
							{
								instruction2.Operand = ctx.Importer.Import((IField)instruction2.Operand);
							}
						}
					}
					methodDef2.Body.Instructions.Add(instruction2);
					bodyMap[instruction] = instruction2;
				}
				Func<Instruction, Instruction> <>9__0;
				foreach (Instruction instruction3 in methodDef2.Body.Instructions)
				{
					bool flag5 = instruction3.Operand != null && bodyMap.ContainsKey(instruction3.Operand);
					if (flag5)
					{
						instruction3.Operand = bodyMap[instruction3.Operand];
					}
					else
					{
						bool flag6 = instruction3.Operand is Instruction[];
						if (flag6)
						{
							Instruction instruction4 = instruction3;
							IEnumerable<Instruction> source = (Instruction[])instruction3.Operand;
							Func<Instruction, Instruction> selector;
							if ((selector = <>9__0) == null)
							{
								selector = (<>9__0 = ((Instruction target) => (Instruction)bodyMap[target]));
							}
							instruction4.Operand = source.Select(selector).ToArray<Instruction>();
						}
					}
				}
				foreach (ExceptionHandler exceptionHandler in methodDef.Body.ExceptionHandlers)
				{
					methodDef2.Body.ExceptionHandlers.Add(new ExceptionHandler(exceptionHandler.HandlerType)
					{
						CatchType = ((exceptionHandler.CatchType == null) ? null : ctx.Importer.Import(exceptionHandler.CatchType)),
						TryStart = (Instruction)bodyMap[exceptionHandler.TryStart],
						TryEnd = (Instruction)bodyMap[exceptionHandler.TryEnd],
						HandlerStart = (Instruction)bodyMap[exceptionHandler.HandlerStart],
						HandlerEnd = (Instruction)bodyMap[exceptionHandler.HandlerEnd],
						FilterStart = ((exceptionHandler.FilterStart == null) ? null : ((Instruction)bodyMap[exceptionHandler.FilterStart]))
					});
				}
				methodDef2.Body.SimplifyMacros(methodDef2.Parameters);
			}
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x0000BE4C File Offset: 0x0000A04C
		private static void CopyFieldDef(FieldDef fieldDef, InjectHelper.InjectContext ctx)
		{
			FieldDef fieldDef2 = (FieldDef)ctx.map[fieldDef];
			fieldDef2.Signature = ctx.Importer.Import(fieldDef.Signature);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x0000BE88 File Offset: 0x0000A088
		private static void Copy(TypeDef typeDef, InjectHelper.InjectContext ctx, bool copySelf)
		{
			if (copySelf)
			{
				InjectHelper.CopyTypeDef(typeDef, ctx);
			}
			foreach (TypeDef typeDef2 in typeDef.NestedTypes)
			{
				InjectHelper.Copy(typeDef2, ctx, true);
			}
			foreach (MethodDef methodDef in typeDef.Methods)
			{
				InjectHelper.CopyMethodDef(methodDef, ctx);
			}
			foreach (FieldDef fieldDef in typeDef.Fields)
			{
				InjectHelper.CopyFieldDef(fieldDef, ctx);
			}
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x0000BF70 File Offset: 0x0000A170
		public static TypeDef Inject(TypeDef typeDef, ModuleDef target)
		{
			InjectHelper.InjectContext injectContext = new InjectHelper.InjectContext(typeDef.Module, target);
			InjectHelper.PopulateContext(typeDef, injectContext);
			InjectHelper.Copy(typeDef, injectContext, true);
			return (TypeDef)injectContext.map[typeDef];
		}

		// Token: 0x060000DA RID: 218 RVA: 0x0000BFB4 File Offset: 0x0000A1B4
		public static MethodDef Inject(MethodDef methodDef, ModuleDef target)
		{
			InjectHelper.InjectContext injectContext = new InjectHelper.InjectContext(methodDef.Module, target);
			injectContext.map[methodDef] = InjectHelper.Clone(methodDef);
			InjectHelper.CopyMethodDef(methodDef, injectContext);
			return (MethodDef)injectContext.map[methodDef];
		}

		// Token: 0x060000DB RID: 219 RVA: 0x0000C000 File Offset: 0x0000A200
		public static IEnumerable<IDnlibDef> Inject(TypeDef typeDef, TypeDef newType, ModuleDef target)
		{
			InjectHelper.InjectContext injectContext = new InjectHelper.InjectContext(typeDef.Module, target);
			injectContext.map[typeDef] = newType;
			InjectHelper.PopulateContext(typeDef, injectContext);
			InjectHelper.Copy(typeDef, injectContext, false);
			return injectContext.map.Values.Except(new TypeDef[]
			{
				newType
			});
		}

		// Token: 0x0200004F RID: 79
		private class InjectContext : ImportMapper
		{
			// Token: 0x0600017E RID: 382 RVA: 0x0000F11C File Offset: 0x0000D31C
			public InjectContext(ModuleDef module, ModuleDef target)
			{
				this.OriginModule = module;
				this.TargetModule = target;
				this.importer = new Importer(target, ImporterOptions.TryToUseTypeDefs, default(GenericParamContext), this);
			}

			// Token: 0x1700002D RID: 45
			// (get) Token: 0x0600017F RID: 383 RVA: 0x0000F164 File Offset: 0x0000D364
			public Importer Importer
			{
				get
				{
					return this.importer;
				}
			}

			// Token: 0x06000180 RID: 384 RVA: 0x0000F17C File Offset: 0x0000D37C
			public override ITypeDefOrRef Map(ITypeDefOrRef typeDefOrRef)
			{
				TypeDef typeDef = typeDefOrRef as TypeDef;
				bool flag = typeDef != null;
				if (flag)
				{
					bool flag2 = this.map.ContainsKey(typeDef);
					if (flag2)
					{
						return (TypeDef)this.map[typeDef];
					}
				}
				return null;
			}

			// Token: 0x06000181 RID: 385 RVA: 0x0000F1C4 File Offset: 0x0000D3C4
			public override IMethod Map(MethodDef methodDef)
			{
				bool flag = this.map.ContainsKey(methodDef);
				IMethod result;
				if (flag)
				{
					result = (MethodDef)this.map[methodDef];
				}
				else
				{
					result = null;
				}
				return result;
			}

			// Token: 0x06000182 RID: 386 RVA: 0x0000F1FC File Offset: 0x0000D3FC
			public override IField Map(FieldDef fieldDef)
			{
				bool flag = this.map.ContainsKey(fieldDef);
				IField result;
				if (flag)
				{
					result = (FieldDef)this.map[fieldDef];
				}
				else
				{
					result = null;
				}
				return result;
			}

			// Token: 0x040000AF RID: 175
			public readonly Dictionary<IDnlibDef, IDnlibDef> map = new Dictionary<IDnlibDef, IDnlibDef>();

			// Token: 0x040000B0 RID: 176
			public readonly ModuleDef OriginModule;

			// Token: 0x040000B1 RID: 177
			public readonly ModuleDef TargetModule;

			// Token: 0x040000B2 RID: 178
			private readonly Importer importer;
		}
	}
}
