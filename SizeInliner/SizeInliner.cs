using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using SR = System.Reflection;
using SRE = System.Reflection.Emit;
using System.Collections;
using ClientPlugin;
using ClientPlugin.Providers;
using ClientPlugin.Models;
using ClientPlugin.UI;
using ClientPlugin.Enums;
using System.Runtime.InteropServices;

namespace SizeInliner
{
    public class SizeInliner : IGenericInliner
    {
        public IAssemblyHelper asm;
        private List<PluginControl> BuildPluginControls()
        {
            return new List<PluginControl>();
        }

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
                            Logger.Log(this, string.Format("Processed {0} Size Inlines", procMeth));
                        }
                    }
                }
            }

            Logger.Log(this, string.Format("Processing has finished"));
        }

        private int ProcessMethod(MethodDef md)
        {

            int patched = 0;
            for (int i = 0; i < md.Body.Instructions.Count; i++)
            {
                Instruction inst = md.Body.Instructions[i];

                if (inst.OpCode == OpCodes.Sizeof)
                {
                    ITypeDefOrRef sizeTargetRef = (ITypeDefOrRef)inst.Operand;
                    if (sizeTargetRef is TypeSpec)
                    {
                        continue;
                    }
                    md.Body.Instructions[i].OpCode = OpCodes.Ldc_I4;
                    md.Body.Instructions[i].Operand = GetSize(sizeTargetRef);
                    patched++;
                }
            }

            return patched;
        }

        private int GetSize(ITypeDefOrRef refOrDef, bool topmost = true)
        {
            int ret = 0;
            TypeDef target = refOrDef.ResolveTypeDef();

            if (target == null)
            {
                return GetSize(refOrDef.ReflectionFullName);
            }

            if (!topmost && target.BaseType != null && target.BaseType.Name == "ValueType")
            {
                //ret += 1;
            }

            foreach (FieldDef fd in target.Fields)
            {
                if (fd.FieldType.TryGetTypeDef() != null)
                {
                    int size = GetSize(fd.FieldType.ToTypeDefOrRef(), false);
                    ret += size;
                }
                else
                {
                    int size = GetSize(fd.FieldType.ReflectionFullName);
                    ret += size;
                }
            }

            if (ret % 4 != 0)
            {
                int rem = ret % 4;
                ret -= rem;
                ret += 4;
            }

            return ret;
        }

        private int GetSizeOfficialMSDNStruct(ITypeDefOrRef refOrDef, bool topmost = true)
        {
            int ret = 0;
            TypeDef target = refOrDef.ResolveTypeDef();

            if (target == null)
            {
                return GetSize(refOrDef.ReflectionFullName);
            }

            if (target.BaseType != null && target.BaseType.Name == "ValueType")
            {
                ret += 1;
            }

            foreach (FieldDef fd in target.Fields)
            {
                if (fd.FieldType.TryGetTypeDef() != null)
                {
                    int size = GetSize(fd.FieldType.ToTypeDefOrRef(), false);
                    int alignment = 0;

                    alignment = 8;

                    if (ret % alignment != 0)
                    {
                        int rem = ret % alignment;
                        ret -= rem;
                        ret += alignment;
                    }
                    ret += size;
                }
                else
                {
                    int size = GetSize(fd.FieldType.ReflectionFullName);
                    int alignment = 0;

                    if (size == 4)
                        alignment = 4;
                    else if (size == 2)
                        alignment = 2;
                    else if (size == 1)
                        alignment = 1;
                    else if (size == 8)
                        alignment = 4;
                    else if (size == 16)
                        alignment = 4; // TODO: Check this

                    if (ret % alignment != 0)
                    {
                        int rem = ret % alignment;
                        ret -= rem;
                        ret += alignment;
                    }

                    ret += size;
                }
            }


            return ret;
        }

        private delegate int SizeDM();
        
        private int GetSize(string target)
        {
            Type targetType = Type.GetType(target, false);
            if (targetType == null)
            {
                return -1;
            }

            SRE.DynamicMethod dm = new SRE.DynamicMethod("", typeof(int), null);

            SRE.ILGenerator gen = dm.GetILGenerator();

            gen.Emit(SRE.OpCodes.Sizeof, targetType);
            gen.Emit(SRE.OpCodes.Ret);

            SizeDM del = (SizeDM)dm.CreateDelegate(typeof(SizeDM));

            return del();
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
        

        public string Name
        {
            get { return "Size Inliner"; }
        }

        public string Author
        {
            get { return "Pan"; }
        }

        public string Description
        {
            get { return "Resolves all possible sizeof instructions. Includes custom data types (inheriting from ValueType) and regular data types."; }
        }

        public int Priority
        {
            get { return 5; }
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
