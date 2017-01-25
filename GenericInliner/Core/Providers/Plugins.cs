using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClientPlugin;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using ClientPlugin.Models;

namespace GenericInliner.Core.Providers
{
    /// <summary>
    /// Helper class for loading/managing plugins
    /// </summary>
    public static class Plugins
    {
        /// <summary>
        /// The list of loaded plugins currently available to access
        /// </summary>
        public static List<IGenericInliner> LoadedPlugins;

        /// <summary>
        /// Load all plugins in the 'Plugins' folder
        /// </summary>
        /// <returns>If the method hits an exception with loading plugins it will return false</returns>
        public static bool LoadAll()
        {
            LoadedPlugins = new List<IGenericInliner>();
            try
            {
                foreach (string module in Directory.GetFiles(Environment.CurrentDirectory + "\\Plugins", "*.dll"))
                {
                    if (module.EndsWith("ClientPlugin.dll") || module.EndsWith("dnlib.dll"))
                        continue;
                    if (LoadFrom(Path.GetFileNameWithoutExtension(module)) == null)
                    {
                        // Warn users
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

            // Order by priority
            LoadedPlugins = LoadedPlugins.OrderBy(x => x.Priority).ToList();
            
            return true;
        }
        /// <summary>
        /// Load a specific plugin from the 'Plugins' folder
        /// </summary>
        /// <param name="module">Name of the dll file (without extension)</param>
        /// <returns>An instance of the plugin</returns>
        public static IGenericInliner LoadFrom(string module)
        {
            try
            {
                string filePath = Environment.CurrentDirectory + "\\Plugins\\" + module + ".dll";

                Assembly asm = Assembly.LoadFrom(filePath);
                Type pluginType = typeof(IGenericInliner);

                ICollection<Type> pluginTypes = new List<Type>();

                if (asm != null)
                {
                    Type[] types = asm.GetTypes();
                    foreach (Type type in types)
                    {
                        if (type.IsInterface || type.IsAbstract)
                        {
                            continue;
                        }
                        else
                        {
                            if (type.GetInterface(pluginType.FullName) != null)
                            {
                                pluginTypes.Add(type);
                            }
                        }
                    }
                }

                IGenericInliner loadedPlugin = null;
                foreach (Type type in pluginTypes)
                {
                    IGenericInliner plugin = (IGenericInliner)Activator.CreateInstance(type);
                    loadedPlugin = plugin;
                    LoadedPlugins.Add(plugin);
                }

                return loadedPlugin;
            }
            catch (Exception)
            {
                
                return null;
            }
        }
        
    }
}
