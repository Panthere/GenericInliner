using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using ClientPlugin.Models;
using ClientPlugin.Providers;
using ClientPlugin.UI;
using dnlib.DotNet;
using dnlib.DotNet.Emit;

namespace DecimalCompareInliner
{
    public class CompareInliner:IGenericInliner
	{
		public IAssemblyHelper asm;

		public void Process(IAssemblyHelper asmHelper)
		{
			decimalCompare(asmHelper.Module);
		}
		public void decimalCompare(ModuleDefMD module)
		{
			foreach (var types in module.GetTypes())
			foreach (var methods in types.Methods)
			{
				if (!methods.HasBody) continue;
				for (var i = 0; i < methods.Body.Instructions.Count; i++)
					if (methods.Body.Instructions[i].OpCode == OpCodes.Call)
						if (methods.Body.Instructions[i].OpCode == OpCodes.Call &&
						    methods.Body.Instructions[i].Operand.ToString().Contains("Compare"))
							if (methods.Body.Instructions[i - 1].OpCode == OpCodes.Newobj)
								if (methods.Body.Instructions[i - 2].IsLdcI4() && methods.Body.Instructions[i - 4].IsLdcI4())
								{
									var val1 = methods.Body.Instructions[i - 4].GetLdcI4Value();
									var val2 = methods.Body.Instructions[i - 2].GetLdcI4Value();
									var newValue = decimal.Compare(val1, val2);
									
									Logger.Log(this, string.Format("[!] Decimal.Compare({0},{1}) is {2}", val1, val2, newValue));
									methods.Body.Instructions[i].OpCode = OpCodes.Nop;
									methods.Body.Instructions[i - 1].OpCode = OpCodes.Nop;
									methods.Body.Instructions[i - 2].OpCode = OpCodes.Nop;
									methods.Body.Instructions[i - 3].OpCode = OpCodes.Nop;
									methods.Body.Instructions[i - 4].OpCode = OpCodes.Ldc_I4;
									methods.Body.Instructions[i - 4].Operand = newValue;
								}
			}
		}

		private List<PluginControl> BuildPluginControls()
		{
			return new List<PluginControl>();
		}
		public string Name
		{
			get { return "Decimal.Compare Inliner"; }
		}

		public string Author
		{
			get { return "cawk"; }
		}

		public string Description
		{
			get { return "Resolves all Decimal.Compare(x,y) Cases."; }
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
