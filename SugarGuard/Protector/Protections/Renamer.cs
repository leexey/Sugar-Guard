using System;
using System.Linq;
using dnlib.DotNet;
using SugarGuard.Protector.Class;

namespace SugarGuard.Protector.Protections
{
	// Token: 0x0200000B RID: 11
	public class Renamer
	{
		// Token: 0x06000022 RID: 34 RVA: 0x000046A8 File Offset: 0x000028A8
		public Renamer(SugarLib lib)
		{
			this.Main(lib);
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000023 RID: 35 RVA: 0x000046BC File Offset: 0x000028BC
		public static string InvisibleName
		{
			get
			{
				return string.Format("<{0}>Sugar{1}", Renamer.GenerateName(), "Guard.ツ");
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000046E4 File Offset: 0x000028E4
		private void Main(SugarLib lib)
		{
			string text = null;
			foreach (ModuleDef moduleDef in lib.assembly.Modules)
			{
				foreach (TypeDef typeDef in moduleDef.Types)
				{
					bool isPublic = typeDef.IsPublic;
					if (isPublic)
					{
						text = typeDef.Name;
					}
					bool flag = Renamer.CanRename(typeDef);
					if (flag)
					{
						foreach (MethodDef methodDef in typeDef.Methods)
						{
							bool flag2 = Renamer.CanRename(methodDef);
							if (flag2)
							{
								TypeRef typeRef = lib.moduleDef.CorLibTypes.GetTypeRef("System.Runtime.CompilerServices", "CompilerGeneratedAttribute");
								MemberRefUser ctor = new MemberRefUser(lib.moduleDef, ".ctor", MethodSig.CreateInstance(lib.moduleDef.Import(typeof(void)).ToTypeSig(true)), typeRef);
								CustomAttribute item = new CustomAttribute(ctor);
								methodDef.CustomAttributes.Add(item);
								methodDef.Name = Renamer.InvisibleName;
							}
							foreach (Parameter parameter in methodDef.Parameters)
							{
								parameter.Name = Renamer.InvisibleName;
							}
						}
					}
					foreach (FieldDef fieldDef in typeDef.Fields)
					{
						bool flag3 = Renamer.CanRename(fieldDef);
						if (flag3)
						{
							fieldDef.Name = Renamer.InvisibleName;
						}
					}
					foreach (EventDef eventDef in typeDef.Events)
					{
						bool flag4 = Renamer.CanRename(eventDef);
						if (flag4)
						{
							eventDef.Name = Renamer.InvisibleName;
						}
					}
					bool isPublic2 = typeDef.IsPublic;
					if (isPublic2)
					{
						foreach (Resource resource in lib.moduleDef.Resources)
						{
							bool flag5 = resource.Name.Contains(text);
							if (flag5)
							{
								resource.Name = resource.Name.Replace(text, typeDef.Name);
							}
						}
					}
				}
			}
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00004A64 File Offset: 0x00002C64
		public static string GenerateName()
		{
			return new string((from s in Enumerable.Repeat<string>("あいうえおかきくけこがぎぐげごさしすせそざじずぜアイウエオクザタツワルムパリピンプペヲポ", 10)
			select s[Renamer.rnd.Next(s.Length)]).ToArray<char>());
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00004AB0 File Offset: 0x00002CB0
		private static bool CanRename(TypeDef type)
		{
			bool isGlobalModuleType = type.IsGlobalModuleType;
			bool result;
			if (isGlobalModuleType)
			{
				result = false;
			}
			else
			{
				try
				{
					bool flag = type.Namespace.Contains("My");
					if (flag)
					{
						return false;
					}
				}
				catch
				{
				}
				bool flag2 = type.Interfaces.Count > 0;
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool isSpecialName = type.IsSpecialName;
					if (isSpecialName)
					{
						result = false;
					}
					else
					{
						bool isRuntimeSpecialName = type.IsRuntimeSpecialName;
						if (isRuntimeSpecialName)
						{
							result = false;
						}
						else
						{
							bool flag3 = type.Name.Contains("Sugar");
							result = !flag3;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00004B54 File Offset: 0x00002D54
		private static bool CanRename(MethodDef method)
		{
			bool isConstructor = method.IsConstructor;
			bool result;
			if (isConstructor)
			{
				result = false;
			}
			else
			{
				bool isForwarder = method.DeclaringType.IsForwarder;
				if (isForwarder)
				{
					result = false;
				}
				else
				{
					bool isFamily = method.IsFamily;
					if (isFamily)
					{
						result = false;
					}
					else
					{
						bool flag = method.IsConstructor || method.IsStaticConstructor;
						if (flag)
						{
							result = false;
						}
						else
						{
							bool isRuntimeSpecialName = method.IsRuntimeSpecialName;
							if (isRuntimeSpecialName)
							{
								result = false;
							}
							else
							{
								bool isForwarder2 = method.DeclaringType.IsForwarder;
								if (isForwarder2)
								{
									result = false;
								}
								else
								{
									bool isGlobalModuleType = method.DeclaringType.IsGlobalModuleType;
									if (isGlobalModuleType)
									{
										result = false;
									}
									else
									{
										bool flag2 = method.Name.Contains("Sugar");
										result = !flag2;
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00004C0C File Offset: 0x00002E0C
		private static bool CanRename(FieldDef field)
		{
			bool flag = field.IsLiteral && field.DeclaringType.IsEnum;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool isForwarder = field.DeclaringType.IsForwarder;
				if (isForwarder)
				{
					result = false;
				}
				else
				{
					bool isRuntimeSpecialName = field.IsRuntimeSpecialName;
					if (isRuntimeSpecialName)
					{
						result = false;
					}
					else
					{
						bool flag2 = field.IsLiteral && field.DeclaringType.IsEnum;
						if (flag2)
						{
							result = false;
						}
						else
						{
							bool flag3 = field.Name.Contains("Sugar");
							result = !flag3;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00004C98 File Offset: 0x00002E98
		private static bool CanRename(EventDef ev)
		{
			bool isForwarder = ev.DeclaringType.IsForwarder;
			bool result;
			if (isForwarder)
			{
				result = false;
			}
			else
			{
				bool isRuntimeSpecialName = ev.IsRuntimeSpecialName;
				result = !isRuntimeSpecialName;
			}
			return result;
		}

		// Token: 0x04000016 RID: 22
		private static Random rnd = new Random();
	}
}
