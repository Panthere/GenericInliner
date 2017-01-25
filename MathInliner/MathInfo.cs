using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dnlib.DotNet.Emit;
using dnlib.DotNet;

namespace MathInliner
{
    /// <summary>
    /// Provides the details needed to patch a Math call
    /// </summary>
    public class MathInfo
    {
        /// <summary>
        /// The Math.* Instruction
        /// </summary>
        public Instruction CallingInst;

        /// <summary>
        /// Not used currently, but the convert instruction (conv_i4 or similar) for the call
        /// </summary>
        public Instruction Converter;

        /// <summary>
        /// List of Instructions to remove from the Parent Method
        /// </summary>
        public List<Instruction> Remove = new List<Instruction>();

        /// <summary>
        /// The Math.* Method, eg: Abs, Sin
        /// </summary>
        public string Method;

        /// <summary>
        /// Parameter 1 for the Math.* Method
        /// </summary>
        public double Param1 = double.NaN;
        /// <summary>
        /// Parameter 2 for the Math.* Method (may be NaN)
        /// </summary>
        public double Param2 = double.NaN;

        /// <summary>
        /// Parent method that holds the CallingInst
        /// </summary>
        public MethodDef ParentMethod;
    }
}
