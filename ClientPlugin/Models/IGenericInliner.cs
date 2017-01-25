using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClientPlugin.UI;

namespace ClientPlugin.Models
{
    /// <summary>
    /// The base class for all inliners. This is generally the main class for the inlining code.
    /// </summary>
    public interface IGenericInliner
    {
        /// <summary>
        /// This function is called when the inlining process is being executed. It is the only function that is executed by the plugin loader.
        /// </summary>
        /// <param name="asmHelper"></param>
        void Process(IAssemblyHelper asmHelper);

        /// <summary>
        /// The name of the plugin
        /// </summary>
        string Name { get; }
        /// <summary>
        /// The author of the plugin
        /// </summary>
        string Author { get; }
        /// <summary>
        /// The description of the plugin, keep it short!
        /// </summary>
        string Description { get; }
        /// <summary>
        /// The priority of the plugin, it will determine which plugin should be executed before another.
        /// </summary>
        int Priority { get; }
        /// <summary>
        /// The version of the plugin
        /// </summary>
        Version Version { get; }
        /// <summary>
        /// The associated <see cref="ClientPlugin.UI.PluginControl"/> list that will be displayed on the 'Plugin Settings' page.
        /// </summary>
        List<PluginControl> Controls { get; }
    }
}
