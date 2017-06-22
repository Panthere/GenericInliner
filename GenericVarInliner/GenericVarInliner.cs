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

namespace GenericVarInliner
{
    public class GenericVarInliner : IGenericInliner
    {
        public IAssemblyHelper asm;
        public List<MethodDef> Processed = new List<MethodDef>();
        public void Process(IAssemblyHelper asmHelper)
        {
            // WORK IN PROGRESS SHOULD NOT BE USED.

            return;

            asm = asmHelper;

            Logger.Log(this, string.Format("Processing"));
            foreach (var t in asm.Module.Types)
            {
                foreach (var m in t.Methods)
                {
                    foreach (var v in m.Body.Variables)
                    {
                        v.Type = asm.Module.CorLibTypes.Int32;
                    }
                }
            }
            return;
            foreach (TypeDef td in asm.Module.GetTypes())
            {

                for (int i = 0; i < td.Methods.Count; i++)//foreach (MethodDef md in td.Methods)
                {

                    MethodDef md = td.Methods[i];

                    if (!md.HasBody)
                        continue;

                    md.Body.SimplifyMacros(md.Parameters);
                    List<GenInfo> toPatch = new List<GenInfo>();

                    if (!md.HasGenericParameters)
                    {
                    }


                    for (int x = 0; x < md.Body.Variables.Count; x++)
                    {
                        Local l = md.Body.Variables[x];

                        td.Methods[i].Body.Variables[x].Type = md.Module.CorLibTypes.Int32;


                    }
                    td.Methods[i].GenericParameters.Clear();
                    td.Methods[i].Body.Variables.Clear();
                    /*int procMeth = -1;
                    int count = 0;
                    //while (procMeth != 0 && count++ < 15)
                    {
                        procMeth = ProcessMethod( md);
                        if (procMeth != 0)
                        {
                            Logger.Log(this, string.Format("Solved {0} Variables", procMeth));
                        }
                    }*/
                }
            }
            Logger.Log(this, string.Format("Processing has finished"));
        }

        private int ProcessMethod(MethodDef md)
        {
            List<GenInfo> toPatch = new List<GenInfo>();

            if (!md.HasGenericParameters)
            {
                return 0;
            }

            List<MethodSpec> callers = FindCallingSpecs(md);

            if (callers.Count == 0)
                return 0;

            Dictionary<int, TypeSig> locals = new Dictionary<int, TypeSig>();

            for (int i = 0; i < callers.Count; i++)
            {
                MethodSpec spec = callers[i];

                for (int x = 0; x < md.Body.Variables.Count; x++)
                {
                    Local l = md.Body.Variables[x];

                    if (!l.Type.IsGenericMethodParameter)
                    {
                        continue;
                    }
                    if (!(l.Type is GenericSig))
                    {
                        continue;
                    }


                    GenericSig gSig = l.Type as GenericSig;

                    GenericParam gParam = md.GenericParameters.Where(y => y.Number == gSig.Number).FirstOrDefault();

                    int indexOf = md.GenericParameters.IndexOf(gParam);

                    if (gSig.Number > spec.GenericInstMethodSig.GenericArguments.Count - 1 || gSig.Number < 0)
                        continue;

                    TypeSig tSig = spec.GenericInstMethodSig.GenericArguments[(int)gSig.Number];

                    
                    
                    ITypeDefOrRef cunt = tSig.ToTypeDefOrRef();

                    ITypeDefOrRef tRef = cunt.ScopeType;

                    if (tSig.IsSZArray)
                    {
                        
                    }
                    l.Type = tRef.ToTypeSig();
                    
                    if (locals.ContainsKey(l.Index))
                    {
                        if (locals[l.Index ] == tRef.ToTypeSig())
                            continue;
                        else
                            locals[l.Index] = tRef.ToTypeSig();
                    }
                    else
                    {
                        locals.Add(l.Index, tRef.ToTypeSig());
                    }
                    
                    //md.GenericParameters.Remove(gParam);
                }
            }

            if (locals.Count == 0)
                return 0;


            Dictionary<int, Local> newLocals = new Dictionary<int, Local>();
            foreach (var pair in locals)
            {
                int index = pair.Key;
                TypeSig tSig = pair.Value;

                Local newLocal = new Local(tSig);
                //newLocal.Name = "TESTING!!!" + Guid.NewGuid().ToString();
                newLocal.Index = index;

                newLocals.Add(index, newLocal);
                
            }

            for (int i = 0; i < md.Body.Instructions.Count; i++)
            {
                Instruction inst = md.Body.Instructions[i];
                if (inst.OpCode == OpCodes.Stloc || inst.OpCode == OpCodes.Ldloc)
                {
                    Local l = inst.Operand as Local;
                    if (newLocals.ContainsKey(l.Index))
                    {
                        md.Body.Instructions[i].Operand = newLocals[l.Index];
                    }

                }
            }

            md.Body.Variables.Clear();
            foreach (var pair in newLocals)
            {
                md.Body.Variables.Add(pair.Value);
            }

            // New local is not added at all, it is simply fucked.
            
            if (md.Name == "")
            {

            }
            /*for (int i = 0; i < md.Body.Variables.Count; i++)
            {
                Local l = md.Body.Variables[i];

                if (!l.Type.IsGenericMethodParameter)
                {
                    continue;
                }
                if (!(l.Type is GenericSig))
                {
                    continue;
                }

                if (locals.ContainsKey(l))
                {
                    md.Body.Variables[i].Type = locals[l];
                }
            }*/

            /*for (int i = 0; i < md.Body.Instructions.Count; i++)
            {
                Instruction inst = md.Body.Instructions[i];

                // If it is a generic method
                if (inst.Operand != null && inst.Operand is MethodSpec)
                {
                    // Investigate the method body to see if all locals are outlined, use gen param count vs local count...?
                    // If it is the same then we know that there are variables to outline. Next is trying to identify when a legitimate generic type is there...
                    // We need to collate them into a list so we can link how many have the same generic sig types listed 
                    // (a generic should technically have several different types).
                    
                    
                    MethodSpec mSpec = inst.Operand as MethodSpec;
                    if (mSpec == null)
                    {
                        // Failsafe check
                        continue;
                    }

                    MethodDef mDef = mSpec.ResolveMethodDef();
                    if (mDef == null)
                    {
                        Logger.Log(this, string.Format("Resolved MethodDef is null for {0}", mSpec));
                        continue;
                    }

                    if (mDef.Body == null)
                    {
                        continue;
                    }

                    int lCount = mDef.Body.Variables.Count;
                    int gpCount = mSpec.GenericInstMethodSig.GenericArguments.Count;

                    

                    toPatch.Add(new GenInfo() { CallingInst = inst, GenCount = gpCount, LocalCount = lCount, RawMethod = mDef, TargetGeneric = mSpec, 
                        GenericArgs = mSpec.GenericInstMethodSig.GenericArguments.ToList(),
                    CallIndex = i,
                    ParentMethod = md});

                }
            }*/


            List<GenInfo> Completed = new List<GenInfo>();
            //Completed.AddRange(toPatch);
            
            foreach (GenInfo inf in toPatch)
            {
                // Get count of other similar generic instances
                // ... should put in a dictionary the link between the method, and numbers.
                // Let's get it working without that for now


                // get generic param from generic arg

                // Go to local, go to type, cast type to genericsig.
                // You can then get the 'number' which corresponds to the generic parameter on the raw method
                // Relate that back to the index on the locals maybe?
                bool comp = false;

                List<GenericParam> toDelete = new List<GenericParam>();
                List<Local> toAdd = new List<Local>();
                for (int i = 0; i < inf.RawMethod.Body.Variables.Count; i++)
                {
                    Local l = inf.RawMethod.Body.Variables[i];

                    if (!l.Type.IsGenericMethodParameter)
                    {
                        continue;
                    }
                    if (!(l.Type is GenericSig))
                    {
                        continue;
                    }

                    GenericSig gSig = l.Type as GenericSig;

                    if (gSig.Number > inf.TargetGeneric.GenericInstMethodSig.GenericArguments.Count - 1 || gSig.Number < 0)
                        continue;
                    TypeSig tSig = inf.TargetGeneric.GenericInstMethodSig.GenericArguments[(int)gSig.Number];

                    if (inf.RawMethod.Name == "method_1")
                    {

                    }
                    if (tSig.IsGenericParameter)
                    {

                    }
                    if (tSig == null)
                        continue;

                    ((MethodDef)inf.TargetGeneric.Method).Body.Variables[i].Type = tSig;
                    ((MethodDef)inf.TargetGeneric.Method).Body.Variables[i].Name = "CANCER_" + Guid.NewGuid().ToString();
                    //toAdd.Add(new Local(tSig));
                   

                }

                //toAdd.ForEach(x => ((MethodDef)inf.TargetGeneric.Method).Body.Variables.Add(x));
                /*for (int i = 0; i < inf.RawMethod.Parameters.Count; i++)
                {
                    Parameter param = inf.RawMethod.Parameters[i];

                    if (!param.Type.IsGenericParameter)
                        continue;


                    GenericSig gSig = param.Type.ToGenericSig();

                    TypeSig tSig = inf.TargetGeneric.GenericInstMethodSig.GenericArguments[(int)gSig.Number];

                    if (tSig == null)
                        continue;

                    if (inf.RawMethod.Name == "method_4")
                    {

                    }
                    if (tSig.IsGenericParameter)
                        continue;
                    param.Type = tSig;

                    //toDelete.Add(inf.RawMethod.GenericParameters.Where(x => x.Number == gSig.Number).FirstOrDefault());


                }*/

  

                /*for (int i = 0; i < inf.RawMethod.GenericParameters.Count; i++)
                {
                    GenericParam gParam = inf.RawMethod.GenericParameters[i];

                    
                    TypeSig tSig = inf.TargetGeneric.GenericInstMethodSig.GenericArguments[i];

                    if (tSig == null)
                        continue;

                    MethodDef mDef = inf.TargetGeneric.Method as MethodDef;

                    Parameter p = mDef.Parameters.Where(x => !x.IsHiddenThisParameter && x.Type.FullName == gParam.FullName).FirstOrDefault();
                    

                    if (p == null)
                    {
                        continue;
                    }
                  
                    p.Type = tSig;

                    toDelete.Add(gParam);


                }*/
                toDelete.ForEach(x => inf.RawMethod.GenericParameters.Remove(x));
                /*for (int i = 0; i < inf.RawMethod.Body.Variables.Count; i++)
                {
                    Local l = inf.RawMethod.Body.Variables[i];

                    if (!l.Type.IsGenericMethodParameter)
                    {
                        continue;
                    }
                    if (!(l.Type is GenericSig))
                    {
                        continue;
                    }
                    
                    GenericSig gSig = l.Type as GenericSig;
                    //if (gSig.Number > inf.TargetGeneric.GenericInstMethodSig.GenericArguments.Count - 1)
                    //    continue;
                    TypeSig tSig = inf.TargetGeneric.GenericInstMethodSig.GenericArguments.Where(
                        (x) =>
                        {
                            if (!x.IsGenericMethodParameter)
                                return false;
                                
                            GenericSig gSig2 = x.ToGenericSig();
                            if (gSig2 == null)
                                return false;

                            return gSig2.Number == gSig.Number;
                            //return true;
                        }
                        ).FirstOrDefault();

                    if (tSig == null)
                        continue;

                    inf.RawMethod.Body.Variables[i].Type = tSig;

                    comp = true;
                }

                if (comp)
                    Completed.Add(inf);*/
            }
           

            return Completed.Count;
        }

        private List<MethodSpec> FindCallingSpecs(MethodDef md)
        {
            List<MethodSpec> mSpecs = new List<MethodSpec>();

            foreach (TypeDef td in md.Module.GetTypes())
            {
                foreach (MethodDef mDef in td.Methods)
                {
                    if (!mDef.HasBody)
                        continue;

                    foreach (Instruction inst in mDef.Body.Instructions)
                    {
                        if (inst.Operand is MethodSpec)
                        {
                            MethodSpec spec = inst.Operand as MethodSpec;

                            if (spec.Method.MDToken == md.MDToken)
                            {
                                mSpecs.Add(spec);
                            }
                        }
                    }
                }
            }
            return mSpecs;
        }

        public string Name
        {
            get { return "GenericParam Inliner"; }
        }

        public string Author
        {
            get { return "Pan"; }
        }

        public string Description
        {
            get { return "Attempts to inline Generic Methods that outlines vars to Generic Parameters"; }
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
