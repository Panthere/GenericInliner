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

namespace EmptyTypesInliner
{
    public class EmptyTypesInliner:IGenericInliner
    {
	    public IAssemblyHelper asm;

	    public void Process(IAssemblyHelper asmHelper)
	    {
		    EmptyTypesCleaner(asmHelper.Module);
	    }
		public void EmptyTypesCleaner(ModuleDefMD module)
		{
			foreach (var type in module.GetTypes())
			foreach (var methods in type.Methods)
			{
				if (!methods.HasBody) continue;
				for (var i = 0; i < methods.Body.Instructions.Count; i++)
					if (methods.Body.Instructions[i].OpCode == OpCodes.Ldsfld && methods.Body.Instructions[i]
						    .Operand.ToString().Contains("::EmptyTypes") && methods.Body.Instructions[i + 1].OpCode == OpCodes.Ldlen)
					{
						methods.Body.Instructions[i].OpCode = OpCodes.Ldc_I4_0;
						methods.Body.Instructions[i + 1].OpCode = OpCodes.Nop;
							Logger.Log(this,"Found and replaced empty types");
					}
			}
		}

		private List<PluginControl> BuildPluginControls()
	    {
		    return new List<PluginControl>();
	    }
	    public string Name
	    {
		    get { return "Empty Types Inliner"; }
	    }

	    public string Author
	    {
		    get { return "cawk"; }
	    }

	    public string Description
	    {
		    get { return "Replaces EmptyTypes.Length with Ldc_I4_0."; }
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
