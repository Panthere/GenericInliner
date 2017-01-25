using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClientPlugin.UI;

namespace GenericInliner.Core.Settings
{
    /// <summary>
    /// Helper for accessing the settings for the application
    /// </summary>
    public static class SettingsHelper
    {
        /// <summary>
        /// The raw settings class
        /// </summary>
        public static PluginControlSettings pSettings;

        /// <summary>
        /// Get the stored control that matches the supplied if it exists
        /// </summary>
        /// <param name="ctrl">The control to match against</param>
        /// <returns>If no match is found it simply returns the original control</returns>
        public static PluginControl Load(PluginControl ctrl)
        {
            try
            {
                
                if (pSettings == null)
                {
                    pSettings = new PluginControlSettings();

                    if (pSettings.UpgradeRequired)
                    {
                        pSettings.Reload();
                    }
                    else
                    {
                        pSettings.Upgrade();
                        pSettings.Reload();
                        pSettings.UpgradeRequired = true;
                        pSettings.Save();
                    }
                }

                

                if (pSettings.PluginControls == null)
                    return ctrl;
                if (pSettings.PluginControls.Count == 0)
                    return ctrl;

                foreach (PluginControl savedCtrl in pSettings.PluginControls)
                {
                    if (savedCtrl == ctrl)
                    {
                        return savedCtrl;
                    }
                }

                // None found
                return ctrl;
            }
            catch (Exception ex)
            {
                return ctrl;
            }
        }
        /// <summary>
        /// Save the plugin controls to the settings
        /// </summary>
        /// <param name="ctrls">Controls to save</param>
        public static void Save(List<PluginControl> ctrls)
        {
            try
            {
                if (pSettings == null)
                {
                    pSettings = new PluginControlSettings();
                    pSettings.Reload();
                }

                if (pSettings.PluginControls == null)
                {
                    pSettings.PluginControls = new List<PluginControl>();
                }

                // Remove all saved?
                pSettings.PluginControls.Clear();

                // Add controls
                pSettings.PluginControls.AddRange(ctrls);
                pSettings.Save();
            }
            catch (Exception ex)
            {

            }
        }
        public static void Reset()
        {
            pSettings.PluginControls = new List<PluginControl>();
            pSettings.Save();
        }
    }
}
