using System;
using System.Collections.Generic;
using dnlib.DotNet;
using SugarGuard.Protector.Class;

namespace SugarGuard.Protector.Protections
{
	// Token: 0x02000007 RID: 7
	public class FakeAttributes
	{
		// Token: 0x06000019 RID: 25 RVA: 0x000038D7 File Offset: 0x00001AD7
		public FakeAttributes(SugarLib lib)
		{
			this.Main(lib);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000038E8 File Offset: 0x00001AE8
		private void Main(SugarLib lib)
		{
			ModuleDef moduleDef = lib.moduleDef;
			List<string> list = new List<string>
			{
				"Borland.Vcl.Types",
				"Borland.Eco.Interfaces",
				"();\t",
				"BabelObfuscatorAttribute",
				"ZYXDNGuarder",
				"DotfuscatorAttribute",
				"YanoAttribute",
				"Reactor",
				"EMyPID_8234_",
				"ObfuscatedByAgileDotNetAttribute",
				"ObfuscatedByGoliath",
				"CheckRuntime",
				"ObfuscatedByCliSecureAttribute",
				"____KILL",
				"CodeWallTrialVersion",
				"Sixxpack",
				"Microsoft.VisualBasic",
				"nsnet",
				"ConfusedByAttribute",
				"Macrobject.Obfuscator",
				"Protected_By_Attribute'00'NETSpider.Attribute",
				"CryptoObfuscator.ProtectedWithCryptoObfuscatorAttribute",
				"Xenocode.Client.Attributes.AssemblyAttributes.ProcessedByXenocode",
				"NineRays.Obfuscator.Evaluation",
				"SecureTeam.Attributes.ObfuscatedByAgileDotNetAttribute",
				"SmartAssembly.Attributes.PoweredByAttribute",
				"Sugary",
				"Form",
				"Program"
			};
			foreach (string s in list)
			{
				TypeDef typeDef = new TypeDefUser("SugarGuard.Attributes", s, moduleDef.Import(typeof(Attribute)));
				typeDef.Attributes = TypeAttributes.NotPublic;
				lib.moduleDef.Types.Add(typeDef);
			}
			TypeDef typeDef2 = lib.moduleDef.Types[new Random().Next(0, moduleDef.Types.Count)];
		}
	}
}
