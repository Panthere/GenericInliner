using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dnlib.DotNet.Emit;
using dnlib.DotNet;

namespace CallInliner
{
    /// <summary>
    /// Provides the needed details to inline a call
    /// </summary>
    public class CallInfo
    {
        /// <summary>
        /// The instruction that calls the target method
        /// </summary>
        public Instruction CallingInst;
        
        /// <summary>
        /// The target method to inline
        /// </summary>
        public MethodDef TargetMethod;

        /// <summary>
        /// The parent method that owns the CallingInst
        /// </summary>
        public MethodDef ParentMethod;

    }
}
