using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace SugarGuard.Protector.Class.Constants
{
	// Token: 0x02000032 RID: 50
	internal class DynamicHelper
	{
		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000F5 RID: 245 RVA: 0x0000C733 File Offset: 0x0000A933
		// (set) Token: 0x060000F6 RID: 246 RVA: 0x0000C73B File Offset: 0x0000A93B
		public DynamicContext Context { get; set; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000F7 RID: 247 RVA: 0x0000C744 File Offset: 0x0000A944
		// (set) Token: 0x060000F8 RID: 248 RVA: 0x0000C74C File Offset: 0x0000A94C
		private MethodDef[] methodDef { get; set; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000F9 RID: 249 RVA: 0x0000C755 File Offset: 0x0000A955
		// (set) Token: 0x060000FA RID: 250 RVA: 0x0000C75D File Offset: 0x0000A95D
		private MethodDef Method { get; set; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000FB RID: 251 RVA: 0x0000C766 File Offset: 0x0000A966
		// (set) Token: 0x060000FC RID: 252 RVA: 0x0000C76E File Offset: 0x0000A96E
		private CilBody Body { get; set; }

		// Token: 0x060000FD RID: 253 RVA: 0x0000C777 File Offset: 0x0000A977
		public DynamicHelper(params MethodDef[] methodDef)
		{
			this.methodDef = methodDef;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0000C78C File Offset: 0x0000A98C
		public void Execute(SugarLib lib)
		{
			this.Context = new Inject().Execute(lib, lib.moduleDef);
			foreach (MethodDef methodDef2 in this.methodDef)
			{
				this.Method = methodDef2;
				this.Serialize();
				this.InitializeLocals();
				this.InitializeBranches();
				this.InitializeExceptionHandlers();
				IList<Instruction> instructions = this.Body.Instructions;
				foreach (Instruction instruction in methodDef2.Body.Instructions)
				{
					this.TryTransform(instruction);
					List<Type> list = new List<Type>();
					list.Add(typeof(System.Reflection.Emit.OpCode));
					string name = "Emit";
					MemberRef mr = this.OpCodeToRef(instruction.OpCode.Name.Replace(".", "_"));
					instructions.Add(dnlib.DotNet.Emit.OpCodes.Ldloc.ToInstruction(this.Context.ILGenerator));
					instructions.Add(dnlib.DotNet.Emit.OpCodes.Ldsfld.ToInstruction(mr));
					switch (instruction.OpCode.OperandType)
					{
					case dnlib.DotNet.Emit.OperandType.InlineBrTarget:
					{
						Local local = this.Context.BranchRefs[instruction];
						list.Add(typeof(Label));
						instructions.Add(dnlib.DotNet.Emit.OpCodes.Ldloc.ToInstruction(local));
						break;
					}
					case dnlib.DotNet.Emit.OperandType.InlineField:
					{
						list.Add(typeof(FieldInfo));
						IField field = (IField)instruction.Operand;
						this.EmitTypeof(field.DeclaringType);
						instructions.Add(dnlib.DotNet.Emit.OpCodes.Ldstr.ToInstruction(field.Name));
						instructions.Add(dnlib.DotNet.Emit.OpCodes.Ldc_I4.ToInstruction(61));
						instructions.Add(dnlib.DotNet.Emit.OpCodes.Callvirt.ToInstruction(this.Context.GetRefImport<MemberRef>("GetField")));
						break;
					}
					case dnlib.DotNet.Emit.OperandType.InlineI:
						list.Add(typeof(int));
						instructions.Add(dnlib.DotNet.Emit.OpCodes.Ldc_I4.ToInstruction((int)instruction.Operand));
						break;
					case dnlib.DotNet.Emit.OperandType.InlineI8:
						list.Add(typeof(long));
						instructions.Add(dnlib.DotNet.Emit.OpCodes.Ldc_I8.ToInstruction((long)instruction.Operand));
						break;
					case dnlib.DotNet.Emit.OperandType.InlineMethod:
					{
						bool flag = false;
						IMethod method = (IMethod)instruction.Operand;
						bool flag2 = method.Name == ".ctor" && instruction.OpCode != dnlib.DotNet.Emit.OpCodes.Call;
						MethodDef method2;
						if (flag2)
						{
							list.Add(typeof(ConstructorInfo));
							method2 = Inject.allMethods["GetConstructor"];
							method2 = this.Context.GetRefImport<MethodDef>("GetConstructor");
						}
						else
						{
							bool flag3 = instruction.OpCode == dnlib.DotNet.Emit.OpCodes.Call || instruction.OpCode == dnlib.DotNet.Emit.OpCodes.Callvirt;
							if (flag3)
							{
								list.AddRange(new Type[]
								{
									typeof(MethodInfo),
									typeof(Type[])
								});
								name = "EmitCall";
								flag = true;
							}
							else
							{
								list.Add(typeof(MethodInfo));
							}
							method2 = Inject.allMethods["GetMethod"];
							this.EmitTypeof(method.DeclaringType);
						}
						instructions.Add(dnlib.DotNet.Emit.OpCodes.Ldstr.ToInstruction(method.Name));
						this.EmitTypeArray(method.MethodSig.Params);
						instructions.Add(dnlib.DotNet.Emit.OpCodes.Call.ToInstruction(method2));
						bool flag4 = flag;
						if (flag4)
						{
							instructions.Add(dnlib.DotNet.Emit.OpCodes.Ldnull.ToInstruction());
						}
						break;
					}
					case dnlib.DotNet.Emit.OperandType.InlineR:
						list.Add(typeof(double));
						instructions.Add(dnlib.DotNet.Emit.OpCodes.Ldc_R8.ToInstruction((double)instruction.Operand));
						break;
					case dnlib.DotNet.Emit.OperandType.InlineString:
						list.Add(typeof(string));
						instructions.Add(dnlib.DotNet.Emit.OpCodes.Ldstr.ToInstruction((string)instruction.Operand));
						break;
					case dnlib.DotNet.Emit.OperandType.InlineTok:
					case dnlib.DotNet.Emit.OperandType.InlineType:
						list.Add(typeof(Type));
						this.EmitTypeof((ITypeDefOrRef)instruction.Operand);
						instructions.Add(dnlib.DotNet.Emit.OpCodes.Call.ToInstruction(this.Context.GetRefImport<IMethod>("GetTypeFromHandle")));
						break;
					case dnlib.DotNet.Emit.OperandType.InlineVar:
					{
						Local local2 = instruction.Operand as Local;
						bool flag5 = local2 != null;
						if (flag5)
						{
							Local local3 = this.Context.LocalRefs[local2];
							list.Add(typeof(LocalBuilder));
							instructions.Add(dnlib.DotNet.Emit.OpCodes.Ldloc.ToInstruction(local3));
						}
						else
						{
							list.Add(typeof(int));
							instructions.Add(Instruction.CreateLdcI4(instruction.GetParameterIndex()));
						}
						break;
					}
					case dnlib.DotNet.Emit.OperandType.ShortInlineR:
						list.Add(typeof(float));
						instructions.Add(dnlib.DotNet.Emit.OpCodes.Ldc_R4.ToInstruction((float)instruction.Operand));
						break;
					}
					instructions.Add(Instruction.Create(dnlib.DotNet.Emit.OpCodes.Callvirt, methodDef2.Module.Import(typeof(ILGenerator).GetMethod(name, list.ToArray()))));
				}
				this.Deserialize();
			}
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0000CD78 File Offset: 0x0000AF78
		private void Serialize()
		{
			this.Method.Body.SimplifyMacros(this.Method.Parameters);
			this.Body = new CilBody();
			this.Context.ILGenerator = new Local(this.Context.GetTypeImport<TypeSig>("ILGenerator"));
			this.Body.Variables.Add(this.Context.ILGenerator);
			this.Context.MethodInfo = new Local(this.Context.GetTypeImport<TypeSig>("MethodInfo"));
			this.Body.Variables.Add(this.Context.MethodInfo);
			this.Context.DynamicMethod = new Local(this.Context.GetTypeImport<TypeSig>("DynamicMethod"));
			this.Body.Variables.Add(this.Context.DynamicMethod);
			IList<Instruction> instructions = this.Body.Instructions;
			instructions.Add(dnlib.DotNet.Emit.OpCodes.Ldstr.ToInstruction("Virtual Machine"));
			instructions.Add(dnlib.DotNet.Emit.OpCodes.Ldc_I4.ToInstruction(22));
			instructions.Add(dnlib.DotNet.Emit.OpCodes.Ldc_I4_1.ToInstruction());
			this.EmitTypeof(this.Method.ReturnType.ToTypeDefOrRef());
			this.EmitTypeArray(this.GetSigs(this.Method.Parameters));
			this.EmitTypeof(this.Method.DeclaringType);
			instructions.Add(dnlib.DotNet.Emit.OpCodes.Ldc_I4_0.ToInstruction());
			instructions.Add(dnlib.DotNet.Emit.OpCodes.Newobj.ToInstruction(this.Context.GetRefImport<MemberRef>(".ctor")));
			instructions.Add(dnlib.DotNet.Emit.OpCodes.Stloc.ToInstruction(this.Context.DynamicMethod));
			instructions.Add(dnlib.DotNet.Emit.OpCodes.Ldloc.ToInstruction(this.Context.DynamicMethod));
			instructions.Add(dnlib.DotNet.Emit.OpCodes.Call.ToInstruction(Inject.allMethods["GetCreatedMethodInfo"]));
			instructions.Add(dnlib.DotNet.Emit.OpCodes.Stloc.ToInstruction(this.Context.MethodInfo));
			instructions.Add(dnlib.DotNet.Emit.OpCodes.Ldloc.ToInstruction(this.Context.MethodInfo));
			instructions.Add(dnlib.DotNet.Emit.OpCodes.Ldnull.ToInstruction());
			instructions.Add(dnlib.DotNet.Emit.OpCodes.Ceq.ToInstruction());
			this.Context.Branch = dnlib.DotNet.Emit.OpCodes.Brfalse.ToInstruction(this.Body.Instructions.Last<Instruction>());
			instructions.Add(this.Context.Branch);
			instructions.Add(dnlib.DotNet.Emit.OpCodes.Ldloc.ToInstruction(this.Context.DynamicMethod));
			instructions.Add(dnlib.DotNet.Emit.OpCodes.Callvirt.ToInstruction(this.Context.GetRefImport<MemberRef>("GetILGenerator")));
			instructions.Add(dnlib.DotNet.Emit.OpCodes.Stloc.ToInstruction(this.Context.ILGenerator));
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0000D060 File Offset: 0x0000B260
		private void TryTransform(Instruction instruction)
		{
			IList<Instruction> instructions = this.Body.Instructions;
			Local local;
			bool flag = this.Context.IsBranchTarget(instruction, out local);
			if (flag)
			{
				instructions.Add(dnlib.DotNet.Emit.OpCodes.Ldloc.ToInstruction(this.Context.ILGenerator));
				instructions.Add(dnlib.DotNet.Emit.OpCodes.Ldloc.ToInstruction(local));
				instructions.Add(dnlib.DotNet.Emit.OpCodes.Callvirt.ToInstruction(this.Context.GetRefImport<MemberRef>("MarkLabel")));
			}
			bool flag2 = this.Context.IsExceptionStart(instruction);
			if (flag2)
			{
				instructions.Add(dnlib.DotNet.Emit.OpCodes.Ldloc.ToInstruction(this.Context.ILGenerator));
				instructions.Add(dnlib.DotNet.Emit.OpCodes.Callvirt.ToInstruction(this.Context.GetRefImport<MemberRef>("BeginExceptionBlock")));
				instructions.Add(dnlib.DotNet.Emit.OpCodes.Pop.ToInstruction());
			}
			else
			{
				MemberRef mr;
				dnlib.DotNet.Emit.ExceptionHandler exceptionHandler;
				bool flag3 = this.Context.IsHandlerStart(instruction, out mr, out exceptionHandler);
				if (flag3)
				{
					instructions.Add(dnlib.DotNet.Emit.OpCodes.Ldloc.ToInstruction(this.Context.ILGenerator));
					bool flag4 = exceptionHandler.HandlerType == ExceptionHandlerType.Catch;
					if (flag4)
					{
						this.EmitTypeof(exceptionHandler.CatchType);
					}
					instructions.Add(dnlib.DotNet.Emit.OpCodes.Callvirt.ToInstruction(mr));
				}
				else
				{
					bool flag5 = this.Context.IsExceptionEnd(instruction);
					if (flag5)
					{
						instructions.Add(dnlib.DotNet.Emit.OpCodes.Ldloc.ToInstruction(this.Context.ILGenerator));
						instructions.Add(dnlib.DotNet.Emit.OpCodes.Callvirt.ToInstruction(this.Context.GetRefImport<MemberRef>("EndExceptionBlock")));
					}
				}
			}
		}

		// Token: 0x06000101 RID: 257 RVA: 0x0000D1FC File Offset: 0x0000B3FC
		private void InitializeExceptionHandlers()
		{
			bool flag = !this.Method.Body.HasExceptionHandlers;
			if (!flag)
			{
				foreach (dnlib.DotNet.Emit.ExceptionHandler item in this.Method.Body.ExceptionHandlers)
				{
					this.Context.ExceptionHandlers.Add(item);
				}
			}
		}

		// Token: 0x06000102 RID: 258 RVA: 0x0000D27C File Offset: 0x0000B47C
		private void InitializeLocals()
		{
			bool flag = !this.Method.Body.HasVariables;
			if (!flag)
			{
				IList<Instruction> instructions = this.Body.Instructions;
				foreach (Local local in this.Method.Body.Variables)
				{
					Local local2 = new Local(this.Context.GetTypeImport<TypeSig>("LocalBuilder"));
					this.Body.Variables.Add(local2);
					instructions.Add(dnlib.DotNet.Emit.OpCodes.Ldloc.ToInstruction(this.Context.ILGenerator));
					this.EmitTypeof(local.Type.ToTypeDefOrRef());
					instructions.Add(dnlib.DotNet.Emit.OpCodes.Callvirt.ToInstruction(this.Context.GetRefImport<MemberRef>("DeclareLocal")));
					instructions.Add(dnlib.DotNet.Emit.OpCodes.Stloc.ToInstruction(local2));
					this.Context.LocalRefs.Add(local, local2);
				}
			}
		}

		// Token: 0x06000103 RID: 259 RVA: 0x0000D3A8 File Offset: 0x0000B5A8
		private void InitializeBranches()
		{
			IList<Instruction> instructions = this.Body.Instructions;
			Dictionary<Instruction, Local> dictionary = new Dictionary<Instruction, Local>();
			foreach (Instruction instruction in from x in this.Method.Body.Instructions
			where x.OpCode.OperandType == dnlib.DotNet.Emit.OperandType.InlineBrTarget
			select x)
			{
				Local local;
				bool flag = this.Context.IsBranchTarget((Instruction)instruction.Operand, out local);
				Local local2;
				if (flag)
				{
					local2 = local;
				}
				else
				{
					local2 = new Local(this.Context.GetTypeImport<TypeSig>("Label"));
					this.Body.Variables.Add(local2);
					instructions.Add(dnlib.DotNet.Emit.OpCodes.Ldloc.ToInstruction(this.Context.ILGenerator));
					instructions.Add(dnlib.DotNet.Emit.OpCodes.Callvirt.ToInstruction(this.Context.GetRefImport<MemberRef>("DefineLabel")));
					instructions.Add(dnlib.DotNet.Emit.OpCodes.Stloc.ToInstruction(local2));
				}
				this.Context.BranchRefs.Add(instruction, local2);
			}
		}

		// Token: 0x06000104 RID: 260 RVA: 0x0000D4F0 File Offset: 0x0000B6F0
		private void Deserialize()
		{
			IList<Instruction> instructions = this.Body.Instructions;
			instructions.Add(dnlib.DotNet.Emit.OpCodes.Ldloc.ToInstruction(this.Context.DynamicMethod));
			instructions.Add(dnlib.DotNet.Emit.OpCodes.Callvirt.ToInstruction(this.Context.GetRefImport<MemberRef>("GetBaseDefinition")));
			instructions.Add(dnlib.DotNet.Emit.OpCodes.Stloc.ToInstruction(this.Context.MethodInfo));
			instructions.Add(dnlib.DotNet.Emit.OpCodes.Ldloc.ToInstruction(this.Context.DynamicMethod));
			instructions.Add(dnlib.DotNet.Emit.OpCodes.Ldloc.ToInstruction(this.Context.MethodInfo));
			instructions.Add(dnlib.DotNet.Emit.OpCodes.Call.ToInstruction(Inject.allMethods["SetMethodInfo"]));
			Instruction instruction = dnlib.DotNet.Emit.OpCodes.Ldloc.ToInstruction(this.Context.MethodInfo);
			this.Context.Branch.Operand = instruction;
			instructions.Add(instruction);
			instructions.Add(dnlib.DotNet.Emit.OpCodes.Ldnull.ToInstruction());
			int count = this.Method.MethodSig.Params.Count;
			this.EmitArray(this.Context.GetTypeImport<TypeSig>("Object"), this.GetSigs(this.Method.Parameters), delegate(int index, IList<TypeSig> s, IList<Instruction> instrs)
			{
				instrs.Add(dnlib.DotNet.Emit.OpCodes.Dup.ToInstruction());
				instrs.Add(dnlib.DotNet.Emit.OpCodes.Ldc_I4.ToInstruction(index));
				instrs.Add(dnlib.DotNet.Emit.OpCodes.Ldarg.ToInstruction(this.Method.Parameters.ElementAt(index)));
				instrs.Add(dnlib.DotNet.Emit.OpCodes.Box.ToInstruction(this.Method.Parameters[index].Type.ToTypeDefOrRef()));
				instrs.Add(dnlib.DotNet.Emit.OpCodes.Stelem_Ref.ToInstruction());
			});
			instructions.Add(dnlib.DotNet.Emit.OpCodes.Callvirt.ToInstruction(this.Context.GetRefImport<MemberRef>("Invoke")));
			bool hasReturnType = this.Method.HasReturnType;
			if (hasReturnType)
			{
				instructions.Add(dnlib.DotNet.Emit.OpCodes.Unbox_Any.ToInstruction(this.Method.ReturnType.ToTypeDefOrRef()));
			}
			else
			{
				instructions.Add(dnlib.DotNet.Emit.OpCodes.Pop.ToInstruction());
			}
			instructions.Add(dnlib.DotNet.Emit.OpCodes.Ret.ToInstruction());
			this.Method.FreeMethodBody();
			this.Method.Body = this.Body;
			this.Body.OptimizeMacros();
		}

		// Token: 0x06000105 RID: 261 RVA: 0x0000D6E4 File Offset: 0x0000B8E4
		private MemberRef OpCodeToRef(string opName)
		{
			return this.Context.Module.Import(typeof(System.Reflection.Emit.OpCodes).GetField(opName, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public));
		}

		// Token: 0x06000106 RID: 262 RVA: 0x0000D718 File Offset: 0x0000B918
		private void EmitArray(TypeSig type, IList<TypeSig> sigs, Action<int, IList<TypeSig>, IList<Instruction>> emit)
		{
			IList<Instruction> instructions = this.Body.Instructions;
			int count = sigs.Count;
			instructions.Add(dnlib.DotNet.Emit.OpCodes.Ldc_I4.ToInstruction(count));
			instructions.Add(dnlib.DotNet.Emit.OpCodes.Newarr.ToInstruction(type.ToTypeDefOrRef()));
			for (int i = 0; i < sigs.Count; i++)
			{
				emit(i, sigs, instructions);
			}
		}

		// Token: 0x06000107 RID: 263 RVA: 0x0000D784 File Offset: 0x0000B984
		private void EmitTypeof(ITypeDefOrRef type)
		{
			this.Body.Instructions.Add(dnlib.DotNet.Emit.OpCodes.Ldtoken.ToInstruction(type));
			this.Body.Instructions.Add(dnlib.DotNet.Emit.OpCodes.Call.ToInstruction(this.Context.GetRefImport<MemberRef>("GetTypeFromHandle")));
		}

		// Token: 0x06000108 RID: 264 RVA: 0x0000D7D9 File Offset: 0x0000B9D9
		private void EmitTypeArray(IList<TypeSig> sigs)
		{
			this.EmitArray(this.Context.GetTypeImport<TypeSig>("Type"), sigs, delegate(int index, IList<TypeSig> s, IList<Instruction> instrs)
			{
				instrs.Add(dnlib.DotNet.Emit.OpCodes.Dup.ToInstruction());
				instrs.Add(dnlib.DotNet.Emit.OpCodes.Ldc_I4.ToInstruction(index));
				this.EmitTypeof(s[index].ToTypeDefOrRef());
				instrs.Add(dnlib.DotNet.Emit.OpCodes.Stelem_Ref.ToInstruction());
			});
		}

		// Token: 0x06000109 RID: 265 RVA: 0x0000D800 File Offset: 0x0000BA00
		private List<TypeSig> GetSigs(ParameterList list)
		{
			List<TypeSig> list2 = new List<TypeSig>();
			foreach (Parameter parameter in list)
			{
				list2.Add(parameter.Type);
			}
			return list2;
		}
	}
}
