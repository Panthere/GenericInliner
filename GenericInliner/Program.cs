using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using GenericInliner.Core.Providers;
using dnlib.DotNet;
using dnlib.DotNet.Writer;
using ClientPlugin;
using ClientPlugin.Providers;
using ClientPlugin.Models;
using ClientPlugin.UI;

namespace GenericInliner
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            /*AssemblyHelper asm = new AssemblyHelper();


            Plugins.LoadAll();

            List<IGenericInliner> procLine = Plugins.LoadedPlugins;

            asm.Module = ModuleDefMD.Load(@"D:\Projects\TurboPatch\TurboPatch\bin\Release\Unpackme.exe");
            asm.Settings = new List<PluginValue>();

            /*asm.AttributeRegex.Add("(public|private) static");
            asm.OpCodeRegex.Add("^(call|callvirt) ret$");
            asm.OpCodeRegex.Add("^[ldarg ]+ (call|callvirt) ret$");
            asm.OpCodeRegex.Add("^newobj ret$");
            asm.OpCodeRegex.Add("^[ldarg ]+ newobj ret$");

            asm.OpCodeRegex.Add("ldstr ret");
            asm.OpCodeRegex.Add("ldc.r8.*?call.*?sizeof sub ldc.r8.*?ldloc ret");
            asm.OpCodeRegex.Add("call call.*?ldloc stloc br ldloc ret");

            asm.CallMatchAll = true;
            asm.CallRemoveMethods = true;
            asm.MathRound = true;

            procLine.Add(new VariableInliner());
            procLine.Add(new CallInliner());
            procLine.Add(new MathInliner());
            //procLine.Add(new MathInliner());

            foreach (IGenericInliner inliner in procLine)
            {
                inliner.Process(asm);
            }

            var opts = new ModuleWriterOptions(asm.Module);
            opts.MetaDataOptions.Flags = MetaDataFlags.PreserveAll;
            opts.Logger = DummyLogger.NoThrowInstance;


            asm.Module.Write(@"D:\Projects\TurboPatch\TurboPatch\bin\Release\Unpackme_test.exe", opts);*/
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMain());
        }
    }
}
