using System;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace SugarGuard.Protector.Protections.ReferenceProxy
{
	// Token: 0x02000012 RID: 18
	internal class Helper
	{
		// Token: 0x06000040 RID: 64 RVA: 0x000056F4 File Offset: 0x000038F4
		public MethodDef GenerateMethod(TypeDef declaringType, object targetMethod, bool hasThis = false, bool isVoid = false)
		{
			MemberRef memberRef = (MemberRef)targetMethod;
			MethodDef methodDef = new MethodDefUser(Renamer.InvisibleName, MethodSig.CreateStatic(memberRef.ReturnType), MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static);
			methodDef.Body = new CilBody();
			if (hasThis)
			{
				methodDef.MethodSig.Params.Add(declaringType.Module.Import(declaringType.ToTypeSig(true)));
			}
			foreach (TypeSig item in memberRef.MethodSig.Params)
			{
				methodDef.MethodSig.Params.Add(item);
			}
			methodDef.Parameters.UpdateParameterTypes();
			foreach (Parameter parameter in methodDef.Parameters)
			{
				methodDef.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg, parameter));
			}
			methodDef.Body.Instructions.Add(Instruction.Create(OpCodes.Call, memberRef));
			methodDef.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
			return methodDef;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00005854 File Offset: 0x00003A54
		public MethodDef GenerateMethod(IMethod targetMethod, MethodDef md)
		{
			MethodDef methodDef = new MethodDefUser(Renamer.InvisibleName, MethodSig.CreateStatic(md.Module.Import(targetMethod.DeclaringType.ToTypeSig(true))), MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static);
			methodDef.ImplAttributes = MethodImplAttributes.IL;
			methodDef.IsHideBySig = true;
			methodDef.Body = new CilBody();
			for (int i = 0; i < targetMethod.MethodSig.Params.Count; i++)
			{
				methodDef.ParamDefs.Add(new ParamDefUser(Renamer.InvisibleName, (ushort)(i + 1)));
				methodDef.MethodSig.Params.Add(targetMethod.MethodSig.Params[i]);
			}
			methodDef.Parameters.UpdateParameterTypes();
			for (int j = 0; j < methodDef.Parameters.Count; j++)
			{
				Parameter operand = methodDef.Parameters[j];
				methodDef.Body.Instructions.Add(new Instruction(OpCodes.Ldarg, operand));
			}
			methodDef.Body.Instructions.Add(new Instruction(OpCodes.Newobj, targetMethod));
			methodDef.Body.Instructions.Add(new Instruction(OpCodes.Ret));
			return methodDef;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000059A4 File Offset: 0x00003BA4
		public MethodDef GenerateMethod(FieldDef targetField, MethodDef md)
		{
			MethodDef methodDef = new MethodDefUser(Renamer.InvisibleName, MethodSig.CreateStatic(md.Module.Import(targetField.FieldType)), MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static);
			methodDef.Body = new CilBody();
			TypeDef declaringType = md.DeclaringType;
			methodDef.MethodSig.Params.Add(md.Module.Import(declaringType).ToTypeSig(true));
			methodDef.Body.Instructions.Add(Instruction.Create(OpCodes.Ldarg_0));
			methodDef.Body.Instructions.Add(Instruction.Create(OpCodes.Ldfld, targetField));
			methodDef.Body.Instructions.Add(Instruction.Create(OpCodes.Ret));
			md.DeclaringType.Methods.Add(methodDef);
			return methodDef;
		}
	}
}
