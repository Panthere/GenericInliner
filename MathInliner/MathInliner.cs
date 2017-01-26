using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using SR = System.Reflection;
using System.Collections;
using ClientPlugin;
using ClientPlugin.Providers;
using ClientPlugin.Models;
using ClientPlugin.UI;
using ClientPlugin.Enums;

namespace MathInliner
{
    public class MathInliner : IGenericInliner
    {
        public IAssemblyHelper asm;
        public int DecimalPlaces = 2;
        public bool Round;
        public List<string> IgnoredMethods = new List<string>();
        private List<PluginControl> BuildPluginControls()
        {
            List<PluginControl> Controls = new List<PluginControl>();

            Controls.Add(new PluginControl()
            {
                Description = "Round Math Results",
                Index = 3,
                Name = "MathRound",
                Type = ControlType.Checkbox
            });

            Controls.Add(new PluginControl()
            {
                Description = "Ignore Math Methods",
                Index = 6,
                Name = "IgnoredMethods",
                Type = ControlType.Listview
            });


            return Controls;
        }


        public void Process(IAssemblyHelper asmHelper)
        {
            asm = asmHelper;


            Logger.Log(this, string.Format("Processing"));

            foreach (PluginValue ps in asm.Settings)
            {
                switch (ps.Name)
                {
                    case "MathRound": Round = (bool)ps.Value; break;
                    case "IgnoredMethods": IgnoredMethods = (List<string>)ps.Value; break;
                }
            }
            Logger.Log(this, string.Format("Ignored Methods: {0}", IgnoredMethods.Count));
            Logger.Log(this, string.Format("Math Round: {0}", Round));

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
                            Logger.Log(this, string.Format("Processed {0} Math Inlines", procMeth));
                        }
                    }
                }


            }

            Logger.Log(this, string.Format("Processing has finished"));
        }
        
        private int ProcessMethod(MethodDef md)
        {

            List<MathInfo> Patched = new List<MathInfo>();
            
            for (int i = 0; i < md.Body.Instructions.Count; i++)
            {
                Instruction inst = md.Body.Instructions[i];

                if (inst.OpCode == OpCodes.Call && inst.Operand is MemberRef)
                {
                    MemberRef mRef = inst.Operand as MemberRef;

                    if (mRef.DeclaringType.FullName != "System.Math")
                    {
                        continue;
                    }

                    if (IgnoredMethods.Any(x => mRef.Name == x))
                    {
                        Logger.Log(this, string.Format("Skipping {0} since it is ignored", mRef.Name));
                        continue;
                    }
                    // Find insts to remove
                    // Find args
                    MathInfo mInf = new MathInfo();

                    mInf.Method = mRef.Name;
                    mInf.CallingInst = inst;
                    mInf.ParentMethod = md;

                    List<Instruction> paramVals = FindParamValues(i, md, mRef.MethodSig.Params.Count);
                    paramVals.Reverse();

                    if (paramVals.Count == 0)
                    {
                        // Do something
                        continue;
                    }
                    mInf.Param1 = (double)paramVals[0].Operand;
                    if (paramVals.Count > 1)
                    {
                        mInf.Param2 = (double)paramVals[1].Operand;
                        
                    }

                    if (paramVals.Count < mRef.MethodSig.Params.Count)
                    {

                    }

                    if (paramVals.Contains(inst))
                    {
                        paramVals.Remove(inst);
                    }

                    mInf.Remove.AddRange(paramVals);

                    Logger.Log(this, string.Format("Solving {0}({1}{2})", mInf.Method, mInf.Param1, !double.IsNaN(mInf.Param2) ? ", " + mInf.Param2.ToString() : ""));
                    // I have to do this immediately after so I can solve for the next call if they are packed together.
                    if (Solve(mInf, md))
                    {
                        Logger.Log(this, string.Format("Solved {0}({1}{2})", mInf.Method, mInf.Param1, !double.IsNaN(mInf.Param2) ? ", " + mInf.Param2.ToString() : ""));
                        Patched.Add(mInf);
                    }
                    else
                    {
                        Logger.Log(this, string.Format("Failed to solve {0}({1}{2})", mInf.Method, mInf.Param1, !double.IsNaN(mInf.Param2) ? ", " + mInf.Param2.ToString() : ""));
                    }
                }
            }

            return Patched.Count;
        }


        private double GetMathResult(double originalNumber, string type)
        {

            SR.MethodInfo mInfo = typeof(Math).GetMethod(type, new Type[] { typeof(Double) });
            if (mInfo == null)
                return double.NaN;

            return (double)mInfo.Invoke(null, new object[] { originalNumber });
        }

        private double GetMathResult(double originalNumber, double secondNumber, string type)
        {
            SR.MethodInfo mInfo = typeof(Math).GetMethod(type, new Type[] { typeof(Double), typeof(Double) });
            if (mInfo == null)
                return double.NaN;

            return (double)mInfo.Invoke(null, new object[] { originalNumber, secondNumber });
        }

        private bool Solve(MathInfo inf, MethodDef md)
        {

            if (double.IsNaN(inf.Param1) && double.IsNaN(inf.Param2))
                return false;


            double mResult = double.NaN;
            bool usedTwo = false;
            try
            {
                if (!double.IsNaN(inf.Param2) && !double.IsNaN(inf.Param1))
                {
                    if (inf.Method == "Pow")
                        mResult = Pow(inf.Param1, inf.Param2);
                    else
                        mResult = GetMathResult(inf.Param1, inf.Param2, inf.Method);
                    usedTwo = true;
                }
                else if (!double.IsNaN(inf.Param1) && double.IsNaN (inf.Param2))
                {
                    mResult = GetMathResult(inf.Param1, inf.Method);
                }

            }
            catch (Exception)
            {

            }

            if (double.IsNaN(mResult) || double.IsInfinity(mResult))
            {
                return false;
            }

            int cIndex = md.Body.Instructions.IndexOf(inf.CallingInst);
            if (cIndex == -1)
                return false;

            if (md.Body.Instructions[cIndex].OpCode != inf.CallingInst.OpCode)
            {
                Logger.Log(this, string.Format("Original Instruction was not {0} but {1}", inf.CallingInst.OpCode, md.Body.Instructions[cIndex].OpCode));
                return false;
            }
            double rounded = Math.Round(mResult, DecimalPlaces);

            md.Body.Instructions[cIndex].OpCode = GetNumberType(inf.Converter); //OpCodes.Ldc_R8;
            md.Body.Instructions[cIndex].Operand = Round ? rounded : mResult;
            
            md.Body.UpdateInstructionOffsets();

            foreach (Instruction rem in inf.Remove)
            {

                int index = md.Body.Instructions.IndexOf(rem);
                if (index == -1)
                    continue;
                md.Body.Instructions[index].OpCode = OpCodes.Nop;
                md.Body.Instructions[index].Operand = null;
                md.Body.UpdateInstructionOffsets();
            }
            return true;
        }

        private OpCode GetNumberType(Instruction convInst)
        {
            OpCode numType = OpCodes.Ldc_R8;
            if (convInst == null)
                return numType;
            switch (convInst.OpCode.Code)
            {
                case Code.Conv_I4:
                    numType = OpCodes.Ldc_I4;
                    break;
                case Code.Conv_I8:
                    numType = OpCodes.Ldc_I8;
                    break;
                case Code.Conv_R4:
                    numType = OpCodes.Ldc_R4;
                    break;
            }
            return numType;
        }
        private List<Instruction> FindParamValues(int start, MethodDef md, int numParams)
        {
            List<Instruction> paramValues = new List<Instruction>();
            int offset = start;
            if (offset == -1)
                return null;

            OpCode tOpCode = OpCodes.Ldc_R8;

            for (int i = offset; i > ((offset - numParams * 4) > 0 ? (offset - numParams * 4) : 0); i--)
            {

                if (i < 0 || i >= md.Body.Instructions.Count)
                    break;

                Instruction inst = md.Body.Instructions[i];
                if (paramValues.Count == numParams)
                    break;
                if (inst.OpCode == tOpCode)
                {
                    
                    if (!paramValues.Contains(inst))
                        paramValues.Add(inst);
                }  
            }

            
            return paramValues;

        }
        public static double Pow(double x, double y)
        {
            if (x == -1 && y == 0.2)
                return -1;
            else
                return Math.Pow(x, y);
        }


        public string Name
        {
            get { return "Math Inliner"; }
        }

        public string Author
        {
            get { return "Pan"; }
        }

        public string Description
        {
            get { return "Solves Math.* functions both single and double parameter."; }
        }

        public int Priority
        {
            get { return 3; }
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
