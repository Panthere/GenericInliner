using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dnlib.DotNet.Emit;

namespace VariableInliner
{
    /// <summary>
    /// Provides the details needed to patch a variable
    /// </summary>
    public class VarInfo
    {
        /// <summary>
        /// The value instruction that references the variable value
        /// </summary>
        public Instruction Value;
        /// <summary>
        /// The Instructions that load the variable
        /// </summary>
        public List<Instruction> Loads;
        /// <summary>
        /// Instructions to remove 
        /// </summary>
        public List<Instruction> Remove;
        /// <summary>
        /// The target variable
        /// </summary>
        public Local TheLocal;
    }
}
