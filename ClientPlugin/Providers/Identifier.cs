using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System.Text.RegularExpressions;

namespace ClientPlugin.Providers
{
    /// <summary>
    /// A class used to identify certain methods inside a type.
    /// </summary>
    public class Identifier
    {
        private TypeDef _tDef;
        /// <summary>
        /// Create a new instance of identifier with the TypeDef specified.
        /// </summary>
        /// <param name="targetType">TypeDef to identify on</param>
        public Identifier(TypeDef targetType)
        {
            _tDef = targetType;

        }

      
        /// <summary>
        /// Find a list of methods that have opcodes that match
        /// </summary>
        /// <param name="opRegexes">OpCode Regexes, each string represents a regex for an opcode (in order)</param>
        /// <returns>List of matches</returns>
        public List<MethodDef> FindWhereOpCodes(params string[] opRegexes)
        {
            List<MethodDef> matchMethods = new List<MethodDef>();
            foreach (MethodDef md in _tDef.Methods)
            {
                if (!md.HasBody)
                    continue;

                int matching = 0;
                int index = 0;

                foreach (Instruction inst in md.Body.Instructions)
                {
                    if (index >= opRegexes.Length)
                    {
                        index = 0;
                    }

                    Regex curRegex = new Regex(opRegexes[index], RegexOptions.IgnoreCase);

                    if (curRegex.IsMatch(inst.OpCode.ToString()))
                    {
                        matching++;
                    }
                    index++;
                }
                if (matching == opRegexes.Length)
                {
                    matchMethods.Add(md);
                }
            }

            return matchMethods;

        }

        /// <summary>
        /// Find a list of methods that have operands that match
        /// </summary>
        /// <param name="opRegexes">Operand Regexes, each string represents a regex for an Operand (in order)</param>
        /// <returns>List of matches</returns>
        public List<MethodDef> FindWhereOperands(params string[] opRegexes)
        {
            List<MethodDef> matchMethods = new List<MethodDef>();
            foreach (MethodDef md in _tDef.Methods)
            {
                if (!md.HasBody)
                    continue;

                int matching = 0;
                int index = 0;

                foreach (Instruction inst in md.Body.Instructions)
                {
                    if (index >= opRegexes.Length)
                    {
                        index = 0;
                    }

                    Regex curRegex = new Regex(opRegexes[index], RegexOptions.IgnoreCase);

                    if (curRegex.IsMatch(inst.Operand.ToString()))
                    {
                        matching++;
                    }
                    index++;
                }
                if (matching == opRegexes.Length)
                {
                    matchMethods.Add(md);
                }
            }

            return matchMethods;

        }

        /// <summary>
        /// Find a list of methods that have opcodes that match the supplied regex
        /// </summary>
        /// <param name="opCodeRegex">Regex for all opcodes in the body. Seperator is a space.</param>
        /// <returns>List of matches</returns>
        public List<MethodDef> FindWhereOpCodes(string opCodeRegex)
        {
            List<MethodDef> matchMethods = new List<MethodDef>();
            foreach (MethodDef md in _tDef.Methods)
            {
                if (!md.HasBody)
                    continue;

                string opcodes = GetOpCodes(md);

                if (md.Name == "CopyMethodDef")
                {

                }
                if (Regex.IsMatch(opcodes, opCodeRegex, RegexOptions.IgnoreCase))
                {
                    matchMethods.Add(md);
                }
            }

            return matchMethods;

        }

        /// <summary>
        /// Find a list of methods that have operands that match the supplied regex
        /// </summary>
        /// <param name="operandRegex">Regex for all operands in the body. Seperator is a space.</param>
        /// <returns>List of matches</returns>
        public List<MethodDef> FindWhereOperands(string operandRegex)
        {
            List<MethodDef> matchMethods = new List<MethodDef>();
            foreach (MethodDef md in _tDef.Methods)
            {
                if (!md.HasBody)
                    continue;

                string operands = GetOperands(md);

                if (Regex.IsMatch(operands, operandRegex, RegexOptions.IgnoreCase))
                {
                    matchMethods.Add(md);
                }
            }

            return matchMethods;

        }

        /// <summary>
        /// Find methods where the attributes match
        /// </summary>
        /// <param name="attribRegex">Attribute regex</param>
        /// <returns>List of matches</returns>
        public List<MethodDef> FindWhereAttributes(string attribRegex)
        {
            List<MethodDef> matchMethods = new List<MethodDef>();
            foreach (MethodDef md in _tDef.Methods)
            {
                if (!md.HasBody)
                    continue;

                string attributes = GetAttributes(md);

                if (Regex.IsMatch(attributes, attribRegex, RegexOptions.IgnoreCase))
                {
                    matchMethods.Add(md);
                }
            }

            return matchMethods;
        }

        private string GetOpCodes(MethodDef md)
        {
            List<string> opcodes = new List<string>();
            foreach (Instruction inst in md.Body.Instructions)
            {
                if (inst.OpCode == OpCodes.Nop)
                    continue;
                opcodes.Add(string.Format("{0}", inst.OpCode));
            }
            return string.Join(" ", opcodes.ToArray()); 
        }

        private string GetOperands(MethodDef md)
        {
            List<string> operands = new List<string>();
            foreach (Instruction inst in md.Body.Instructions)
            {
                
                if (inst.Operand != null)
                    operands.Add(string.Format("{0}", inst.Operand));
            }
            return string.Join(" ", operands.ToArray()); 
        }

        private string GetAttributes(MethodDef md)
        {
            return GetMatchingEnum<MethodAttributes>(md);
        }

        private string GetMatchingEnum<T>(MethodDef md)
        {
            List<string> attributes = new List<string>();
            string[] possibleVals = Enum.GetNames(typeof(T));

            foreach (string possible in possibleVals)
            {
                if (md.Attributes.HasFlag((Enum)Enum.Parse(typeof(T), possible)))
                {
                    attributes.Add(possible);
                }
            }

            return string.Join(" ", attributes.ToArray()); 
        }
    }
}
