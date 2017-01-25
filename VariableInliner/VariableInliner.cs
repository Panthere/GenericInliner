using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using ClientPlugin;
using ClientPlugin.Models;
using ClientPlugin.Providers;
using ClientPlugin.UI;

namespace VariableInliner
{
    public class VariableInliner : IGenericInliner
    {
        public IAssemblyHelper asm;
        public void Process(IAssemblyHelper asmHelper)
        {
            asm = asmHelper;

            Logger.Log(this, string.Format("Processing"));
            foreach (TypeDef td in asm.Module.GetTypes())
            {

                foreach (MethodDef md in td.Methods)
                {
                    
                    if (!md.HasBody)
                        continue;

                    md.Body.SimplifyMacros(md.Parameters);

                    int procMeth = -1;
                    while (procMeth != 0)
                    {
                        procMeth = ProcessMethod(md);
                        if (procMeth != 0)
                        {
                            Logger.Log(this, string.Format("Solved {0} Variables", procMeth));
                        }
                    }
                }
            }
            Logger.Log(this, string.Format("Processing has finished"));
        }

        private int ProcessMethod(MethodDef md)
        {
            List<VarInfo> toPatch = new List<VarInfo>();
            for (int i = 0; i < md.Body.Instructions.Count; i++)
            {
                Instruction inst = md.Body.Instructions[i];

                if (inst.OpCode == OpCodes.Ldloc && inst.Operand is Local)
                {
                    List<Instruction> localUsed = FindWhereLocalUsed(inst.Operand as Local, md);

                    if (toPatch.Where(x => x.TheLocal == inst.Operand as Local).Count() > 0)
                    {
                        continue;
                    }
                    if (localUsed.Where(x => x.OpCode == OpCodes.Ldloc).Count() > 1 && localUsed.Where(x => x.OpCode == OpCodes.Stloc).Count() > 1)
                    {
                        continue;
                    }


                    foreach (Instruction lInst in localUsed)
                    {
                        if (lInst.OpCode == OpCodes.Stloc)
                        {
                            Instruction val = FindLocalValue(lInst, inst.Operand as Local, md);
                            if (val == null)
                                continue;
                            VarInfo vInf = new VarInfo();
                            vInf.Loads = localUsed.Where(x => x.OpCode == OpCodes.Ldloc).ToList();
                            vInf.Remove = localUsed.Where(x => x.OpCode == OpCodes.Stloc).ToList();
                            vInf.Remove.Add(val);
                            vInf.Value = val.Clone();
                            vInf.TheLocal = inst.Operand as Local;

                            toPatch.Add(vInf);
                        }
                    }
                }
            }

            foreach (VarInfo inf in toPatch)
            {
                foreach (Instruction rem in inf.Remove)
                {

                    int index = md.Body.Instructions.IndexOf(rem);
                    if (index == -1)
                        continue;
                    md.Body.Instructions[index].OpCode = OpCodes.Nop;
                    md.Body.Instructions[index].Operand = null;
                }
                foreach (Instruction rep in inf.Loads)
                {
                    int index = md.Body.Instructions.IndexOf(rep);
                    if (index == -1)
                        continue;
                    md.Body.Instructions[index] = inf.Value;
                    Logger.Log(this, string.Format("Resolved variable {0} to {1}", inf.TheLocal, inf.Value));
                }
            }
            return toPatch.Count;
        }

        private List<Instruction> FindWhereLocalUsed(Local loc, MethodDef md)
        {
            List<Instruction> ret = new List<Instruction>();

            foreach (Instruction inst in md.Body.Instructions)
            {
                if ((inst.OpCode == OpCodes.Ldloc || inst.OpCode == OpCodes.Stloc) && inst.Operand == loc)
                {
                    ret.Add(inst);
                }
            }
            return ret;

        }

        private Instruction FindLocalValue(Instruction stLoc, Local loc, MethodDef md)
        {
            int offset = md.Body.Instructions.IndexOf(stLoc);
            TypeSig locType = loc.Type;

            if (offset == 0)
                return null;

            OpCode tOpCode = OpCodes.UNKNOWN1;
            switch (locType.ElementType)
            {
                case ElementType.I4:
                    tOpCode = OpCodes.Ldc_I4;
                    break;
                case ElementType.R4:
                    tOpCode = OpCodes.Ldc_R4;
                    break;
                case ElementType.R8:
                    tOpCode = OpCodes.Ldc_R8;
                    break;
                case ElementType.I8:
                    tOpCode = OpCodes.Ldc_I8;
                    break;
                case ElementType.String:
                    tOpCode = OpCodes.Ldstr;
                    break;
            }
            if (tOpCode == OpCodes.UNKNOWN1)
            {
                return null;
            }

            // Have to restrict to - 2 of the offset, otherwise it'll grab values it isn't meant to.
            // Easiest way is to check shitty stack modifications it performs

            for (int i = offset; i >= (offset - 1); i--)
            {
                Instruction inst = md.Body.Instructions[i];
                if (inst.OpCode == tOpCode)
                {
                    return inst;
                }
            }
            return null;

        }


        public string Name
        {
            get { return "Variable Inliner"; }
        }

        public string Author
        {
            get { return "Pan"; }
        }

        public string Description
        {
            get { return "Attempts to inline variables that are outlined into other variables";  }
        }

        public int Priority
        {
            get { return 1; }
        }

        public List<PluginControl> Controls
        {
            get { return new List<PluginControl>(); }
        }

        public Version Version
        {
            get { return new Version(1, 0, 0, 0); }
        }
    }
}
