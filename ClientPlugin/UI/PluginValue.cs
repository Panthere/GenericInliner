using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClientPlugin.UI
{
    [Serializable]
    
    public class PluginValue
    {
        /// <summary>
        /// The name of the value, matches <see cref="ClientPlugin.UI.PluginControl.Name"/>
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The value that was given as an object
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// The <see cref="ClientPlugin.UI.PluginControl.Name"/> that owns the name
        /// </summary>
        public PluginControl Control { get; set; }
    }
}
