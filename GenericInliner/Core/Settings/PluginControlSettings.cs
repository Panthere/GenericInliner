using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using ClientPlugin.UI;

namespace GenericInliner.Core.Settings
{
    public class PluginControlSettings : ApplicationSettingsBase 
    {
        [DefaultSettingValue(""), SettingsSerializeAs(SettingsSerializeAs.Binary), UserScopedSetting]
        public bool UpgradeRequired
        {
            get
            {
                return ((bool)this["UpgradeRequired"]);
            }
            set
            {
                this["UpgradeRequired"] = (bool)value;
            }
        }
        [DefaultSettingValue(""), SettingsSerializeAs(SettingsSerializeAs.Binary), UserScopedSetting]
        public List<PluginControl> PluginControls
        {
            get
            {
                return ((List<PluginControl>)this["PluginControls"]);
            }
            set
            {
                this["PluginControls"] = (List<PluginControl>)value;
            }
        }
       
    }
}
