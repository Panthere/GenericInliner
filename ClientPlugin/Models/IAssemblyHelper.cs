using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dnlib.DotNet;
using ClientPlugin.UI;

namespace ClientPlugin.Models
{
    /// <summary>
    /// The assembly helper provides the list of <see cref="ClientPlugin.UI.PluginValue"/> and ModuleDef for plugins to work with.
    /// </summary>
    public interface IAssemblyHelper
    {
        /// <summary>
        /// Module that is currently being processed
        /// </summary>
        ModuleDefMD Module { get; set; }

        /// <summary>
        /// Plugin values returned from the user input.
        /// </summary>
        List<PluginValue> Settings { get; set; }
    }
}
