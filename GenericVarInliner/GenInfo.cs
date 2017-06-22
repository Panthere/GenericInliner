using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dnlib.DotNet.Emit;
using dnlib.DotNet;

namespace GenericVarInliner
{
    /// <summary>
    /// Provides the details needed to patch a variable
    /// </summary>
    public class GenInfo
    {
        public MethodDef ParentMethod;
        public Instruction CallingInst;
        public MethodDef RawMethod;
        public MethodSpec TargetGeneric;
        public List<TypeSig> GenericArgs;
        public int LocalCount;
        public int GenCount;
        public int CallIndex;
    }
}
