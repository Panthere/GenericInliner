using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using ClientPlugin.Providers;
using ClientPlugin.Enums;
using ClientPlugin.UI;
using ClientPlugin.Models;

namespace CallInliner
{
    public class CallInliner : IGenericInliner
    {
        public IAssemblyHelper asm;
        public List<string> OpCodeRegex = new List<string>();
        public List<string> OperandRegex = new List<string>();
        public List<string> AttributeRegex = new List<string>();
        public List<string> MethodNameRegex = new List<string>();
        public List<string> MethodTokenRegex = new List<string>();

        public bool MatchAll;
        public bool RemoveInlined;
        public bool UseFullName;
        public bool UseHexToken;

        private List<PluginControl> BuildPluginControls()
        {
            List<PluginControl> Controls = new List<PluginControl>();

            Controls.Add(new PluginControl()
            {
                Description = "OpCode Regexes",
                Index = 2,
                Name = "OpCodeRegex",
                Type = ControlType.Listview
            });
            Controls.Add(new PluginControl()
            {
                Description = "Operand Regexes",
                Index = 1,
                Name = "OperandRegex",
                Type = ControlType.Listview
            });
            Controls.Add(new PluginControl()
            {
                Description = "Attribute Regexes",
                Index = 3,
                Name = "AttributeRegex",
                Type = ControlType.Listview
            });
            Controls.Add(new PluginControl()
            {
                Description = "Method Name Regexes",
                Index = 4,
                Name = "MethodNameRegex",
                Type = ControlType.Listview
            });
            Controls.Add(new PluginControl()
            {
                Description = "Method Token Regexes",
                Index = 5,
                Name = "MethodTokenRegex",
                Type = ControlType.Listview
            });
            Controls.Add(new PluginControl()
            {
                Description = "Remove Inlined",
                Index = 2,
                Name = "RemoveInlined",
                Type = ControlType.Checkbox
            });
            Controls.Add(new PluginControl()
            {
                Description = "Match All",
                Index = 1,
                Name = "MatchAll",
                Type = ControlType.Checkbox
            });
            Controls.Add(new PluginControl()
            {
                Description = "Use FullName Matching",
                Index = 4,
                Name = "UseFullName",
                Type = ControlType.Checkbox
            });
            Controls.Add(new PluginControl()
            {
                Description = "Use Hex Token Matching",
                Index = 3,
                Name = "UseHexToken",
                Type = ControlType.Checkbox
            });


           
            return Controls;
        }

        public void Process(IAssemblyHelper asmHelper)
        {
            asm = asmHelper;

            Logger.Log(this, string.Format("Processing"));

            // Set regex/bool
            foreach (PluginValue ps in asm.Settings)
            {
                switch (ps.Name)
                {
                    case "OpCodeRegex": OpCodeRegex = (List<string>)ps.Value; break;
                    case "OperandRegex": OperandRegex = (List<string>)ps.Value; break;
                    case "AttributeRegex": AttributeRegex = (List<string>)ps.Value; break;
                    case "MethodNameRegex": MethodNameRegex = (List<string>)ps.Value; break;
                    case "MethodTokenRegex": MethodTokenRegex = (List<string>)ps.Value; break;
                    case "MatchAll": MatchAll = (bool)ps.Value; break;
                    case "RemoveInlined": RemoveInlined = (bool)ps.Value; break;
                    case "UseHexToken": UseHexToken = (bool)ps.Value; break;
                    case "UseFullName": UseFullName = (bool)ps.Value; break;
                }
            }

            Logger.Log(this, string.Format("Loaded {0} OpCodeRegex", OpCodeRegex.Count));
            Logger.Log(this, string.Format("Loaded {0} OperandRegex", OperandRegex.Count));
            Logger.Log(this, string.Format("Loaded {0} AttributeRegex", AttributeRegex.Count));
            Logger.Log(this, string.Format("Loaded {0} TokenRegex", MethodTokenRegex.Count));
            Logger.Log(this, string.Format("Loaded {0} NameRegex", MethodNameRegex.Count));


            List<CallInfo> toPatch = new List<CallInfo>();

            foreach (TypeDef td in asm.Module.GetTypes())
            {

                foreach (MethodDef md in td.Methods)
                {
                    if (!md.HasBody)
                        continue;
                    md.Body.SimplifyMacros(md.Parameters);
                }
            }

            List<MethodDef> allMatches = new List<MethodDef>();
            foreach (TypeDef td in asm.Module.GetTypes())
            {

                List<MethodDef> methodMatches = GetMatching(td).Distinct().ToList();
                Logger.Log(this, string.Format("Found {0} Matches for {1}", methodMatches.Count, td.Name));
                allMatches.AddRange(methodMatches);
                

            }
            Logger.Log(this, string.Format("Found {0} Matches Total", allMatches.Count));
            foreach (TypeDef td in asm.Module.GetTypes())
            {
                foreach (MethodDef md in td.Methods)
                {
                    if (!md.HasBody)
                        continue;

                    for (int i = 0; i < md.Body.Instructions.Count; i++)
                    {
                        Instruction inst = md.Body.Instructions[i];
                        if ((inst.OpCode == OpCodes.Call || inst.OpCode == OpCodes.Callvirt) && inst.Operand is MethodDef)
                        {

                            // MethodDef
                            MethodDef mDef = inst.Operand as MethodDef;

                            if (allMatches.Any(x => x.MDToken == mDef.MDToken))
                            {
                                // Patch this call
                                toPatch.Add(new CallInfo() { CallingInst = inst, ParentMethod = md, TargetMethod = mDef });
                            }
                        }
                    }
                }
            }


            Logger.Log(this, string.Format("Found {0} calls to inline", toPatch.Count));

            List<CallInfo> toPatch2 = new List<CallInfo>(toPatch);

            // Have to do priority, if a method is already in the topatch and another has it as a target method then there needs to be priority for the latter.

            List<CallInfo> cInfos = new List<CallInfo>();
            
            foreach (CallInfo cInfo in toPatch)
            {
                if (cInfos.Any(x => x.TargetMethod == cInfo.ParentMethod))
                {
                    // if it already exists in the list, insert it before
                    CallInfo cMatch = cInfos.Where(x => x.TargetMethod == cInfo.ParentMethod).FirstOrDefault();

                    if (cMatch == null)
                    {
                        // what the fuck
                        cInfos.Add(cInfo);
                        continue;
                    }

                    int index = cInfos.IndexOf(cMatch);
                    if (index >= 0)
                    {
                        Logger.Log(this, string.Format("Prioritizing {0} over {1}", cInfo.TargetMethod.Name, cMatch.TargetMethod.Name));
                        cInfos.Insert(index != 0 ? index - 1 : index, cInfo);
                    }
                    else
                    {
                        cInfos.Add(cInfo);
                    }

                }
                else
                {
                    cInfos.Add(cInfo);
                }
            }

            foreach (CallInfo cInfo in cInfos)
            {
                if (cInfo.CallingInst == null || cInfo.ParentMethod == null || cInfo.ParentMethod.Body == null)
                {
                    continue;
                }
                int instIndex = cInfo.ParentMethod.Body.Instructions.IndexOf(cInfo.CallingInst);
                if (!cInfo.TargetMethod.IsStatic)
                {
                    if (cInfo.ParentMethod.Body.Instructions[instIndex - 1].OpCode == OpCodes.Ldarg)
                    {
                        cInfo.ParentMethod.Body.Instructions[instIndex - 1].OpCode = OpCodes.Nop;
                        cInfo.ParentMethod.Body.Instructions[instIndex - 1].Operand = null;
                    }
                }
                List<Instruction> toCopy = cInfo.TargetMethod.Body.Instructions.Where(x =>
                    x.OpCode != OpCodes.Ldarg &&
                    x.OpCode != OpCodes.Starg &&
                    x.OpCode != OpCodes.Nop &&
                    x.OpCode != OpCodes.Ret &&
                    x.OpCode != OpCodes.Br && 
                    x.OpCode != OpCodes.Switch
                    ).ToList();

                for (int i = 0; i < toCopy.Count; i++)
                {
                    toCopy[i] = toCopy[i].Clone();
                }

                Dictionary<int, Local> OldLocals = new Dictionary<int,Local>();


                if (cInfo.TargetMethod.Body.HasVariables)
                {
                    foreach (var loc in cInfo.TargetMethod.Body.Variables)
                    {
                        Local l = new Local(loc.Type) { Name = loc.Name, PdbAttributes = loc.PdbAttributes };
                        l = cInfo.ParentMethod.Body.Variables.Add(l);

                        OldLocals.Add(loc.Index, l);

                    }
                    Logger.Log(this, string.Format("Copied {0} Locals", OldLocals.Count));
                }
                for (int i = 0; i < toCopy.Count; i++)
                {
                    if (toCopy[i].OpCode == OpCodes.Stloc || toCopy[i].OpCode == OpCodes.Ldloc)
                    {
                        if (OldLocals.ContainsKey((toCopy[i].Operand as Local).Index))
                        {
                            toCopy[i].Operand = OldLocals[(toCopy[i].Operand as Local).Index];
                           
                        }
                    }
                    Instruction tReplace = toCopy[i].Clone();
                    if (i == 0)
                    {
                        
                        cInfo.ParentMethod.Body.Instructions[instIndex].OpCode = tReplace.OpCode;
                        cInfo.ParentMethod.Body.Instructions[instIndex].Operand = tReplace.Operand;
                    }
                    else
                    {
                        cInfo.ParentMethod.Body.Instructions.Insert(instIndex + i, tReplace);
                        cInfo.ParentMethod.Body.UpdateInstructionOffsets();
                    }
                }

                Logger.Log(this, string.Format("Inlined {0} Instructions", toCopy.Count));
                toPatch2.Remove(cInfo);

                
                if (RemoveInlined && !toPatch2.Any(x => x.TargetMethod == cInfo.TargetMethod))
                {
                    cInfo.TargetMethod.Body.Variables.Clear();
                    cInfo.TargetMethod.Body.Instructions.Clear();
                    cInfo.TargetMethod.Body = null;

                    cInfo.TargetMethod.DeclaringType.Methods.Remove(cInfo.TargetMethod);

                    Logger.Log(this, string.Format("Removed Method {0}", cInfo.TargetMethod.Name));
                }
            }

            Logger.Log(this, string.Format("Processing has finished"));
        }

        private List<MethodDef> GetMatching(TypeDef td)
        {
            Identifier id = new Identifier(td);

            List<MethodDef> AttribMethods = new List<MethodDef>();
            List<MethodDef> OpCodeMethods = new List<MethodDef>();
            List<MethodDef> OperMethods = new List<MethodDef>();
            List<MethodDef> NameMethods = new List<MethodDef>();
            List<MethodDef> TokenMethods = new List<MethodDef>();

            if (AttributeRegex.Count != 0)
            {
                AttributeRegex.ForEach(x => AttribMethods.AddRange(id.FindWhereAttributes(x)));
            }
            if (OperandRegex.Count != 0)
            {
                OperandRegex.ForEach(x => OperMethods.AddRange(id.FindWhereOperands(x)));
            }
            if (OpCodeRegex.Count != 0)
            {
                OpCodeRegex.ForEach(x => OpCodeMethods.AddRange(id.FindWhereOpCodes(x)));
            }
            if (MethodNameRegex.Count != 0)
            {
                MethodNameRegex.ForEach(x => NameMethods.AddRange(id.FindWhereName(x, UseFullName)));
            }
            if (MethodTokenRegex.Count != 0)
            {
                MethodTokenRegex.ForEach(x => TokenMethods.AddRange(id.FindWhereToken(x, UseHexToken)));
            }

            List<MethodDef> MatchingMethods = new List<MethodDef>();
            if (MatchAll)
            {
                foreach (var mtd in td.Methods)
                {

                    if((AttribMethods.Count > 0 || OpCodeMethods.Count > 0 || OperMethods.Count > 0 || NameMethods.Count > 0 || TokenMethods.Count > 0) &&
                        (AttribMethods.Count == 0 || AttribMethods.Contains(mtd)) &&
                        (OpCodeMethods.Count == 0 || OpCodeMethods.Contains(mtd)) &&
                        (OperMethods.Count == 0 || OperMethods.Contains(mtd)) &&
                        (NameMethods.Count == 0 || NameMethods.Contains(mtd)) &&
                        (TokenMethods.Count == 0 || TokenMethods.Contains(mtd))
                        )
                        MatchingMethods.Add(mtd);


                }
            }
            else
            {
                MatchingMethods.AddRange(AttribMethods);
                MatchingMethods.AddRange(OpCodeMethods);
                MatchingMethods.AddRange(OperMethods);
                MatchingMethods.AddRange(NameMethods);
                MatchingMethods.AddRange(TokenMethods);
            }

            return MatchingMethods;
        }

        
        public string Name
        {
            get { return "Call Inliner"; }
        }

        public string Author
        {
            get { return "Pan"; }
        }

        public string Description
        {
            get { return "Inlines calls based on regex"; }
        }

        public int Priority
        {
            get { return 2; }
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
