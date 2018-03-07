using ClientPlugin.Models;
using dnlib.DotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dnlib.DotNet.Emit;
using ClientPlugin.Providers;
using ClientPlugin.UI;

namespace StringLengthInliner
{
    public class StringLengthInliner: IGenericInliner
	{
		public IAssemblyHelper asm;

		public void Process(IAssemblyHelper asmHelper)
		{
			stringLengthFixer(asmHelper.Module);
		}
		public void stringLengthFixer(ModuleDefMD module)
		{
			foreach (TypeDef typeDef in module.GetTypes())
			{
				foreach (MethodDef methods in typeDef.Methods)
				{
					if (!methods.HasBody) continue;
					for (int i = 0; i < methods.Body.Instructions.Count; i++)
					{
						if (methods.Body.Instructions[i].OpCode == OpCodes.Call &&
						    methods.Body.Instructions[i].Operand.ToString().Contains("get_Length") &&
						    methods.Body.Instructions[i - 1].OpCode == OpCodes.Ldstr)
						{
							string val = methods.Body.Instructions[i - 1].Operand.ToString();
							int len = val.Length;
							Logger.Log(this,string.Format("[!] {0} Length is {1}", val, len));
							methods.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
							methods.Body.Instructions[i].Operand = len;
							methods.Body.Instructions[i - 1].OpCode = OpCodes.Nop;
						}
					}
				}
			}
		}

		private List<PluginControl> BuildPluginControls()
		{
			return new List<PluginControl>();
		}
		public string Name
		{
			get { return "String Length Inliner"; }
		}

		public string Author
		{
			get { return "cawk"; }
		}

		public string Description
		{
			get { return "Fixes String.Length and replaces with correct value."; }
		}

		public int Priority
		{
			get { return 5; }
		}

		public List<PluginControl> Controls
		{
			get { return BuildPluginControls(); }
		}


		public Version Version
		{
			get { return new Version(1, 0, 0, 0); }
		}
	}
}
