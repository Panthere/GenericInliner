using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClientPlugin.Enums;

namespace ClientPlugin.UI
{
    [Serializable]
    public class PluginControl
    {
        /// <summary>
        /// Control order index in it's type group
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Control Type
        /// </summary>
        public ControlType Type { get; set; }

        /// <summary>
        /// Control Name, is not unique
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Control description, keep it short.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Used to save/load a control and keep it's values, do not use!
        /// </summary>
        public object Value { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1} ({2})", Name, Description, Type); 
        }

        public static bool operator ==(PluginControl a, PluginControl b)
        {
            if (a.Description == b.Description &&
                a.Index == b.Index &&
                a.Name == b.Name &&
                a.Type == b.Type)
                return true;
            else
                return false;
                
        }
        public static bool operator !=(PluginControl a, PluginControl b)
        {
            if (a.Description != b.Description &&
                a.Index != b.Index &&
                a.Name != b.Name &&
                a.Type != b.Type)
                return true;
            else
                return false;

        }
    }
}
