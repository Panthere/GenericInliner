using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dnlib.DotNet;
using ClientPlugin.Models;
using ClientPlugin.UI;

namespace ClientPlugin.Providers
{
    /// <summary>
    /// The assembly helper provides the list of <see cref="ClientPlugin.UI.PluginValue"/> and ModuleDef for plugins to work with.
    /// </summary>
    public class AssemblyHelper : IAssemblyHelper
    {
        /// <summary>
        /// Module currently being processed
        /// </summary>
        public ModuleDefMD Module { get; set; }

        /// <summary>
        /// Plugin values returned from the user input.
        /// </summary>
        public List<PluginValue> Settings { get; set; }

    }
}
