using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClientPlugin.Enums
{
    [Serializable]
    
    public enum ControlType
    {
        /// <summary>
        /// Create a CheckBox control with the text being <see cref="ClientPlugin.UI.PluginControl.Description"/>
        /// </summary>
        Checkbox,
        /// <summary>
        /// Create a ListBox control with default add/delete/delete all abilities
        /// </summary>
        Listview,
        /// <summary>
        /// Create a TextBox with a label for <see cref="ClientPlugin.UI.PluginControl.Description"/>
        /// </summary>
        Textbox
    }
}
