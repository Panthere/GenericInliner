using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ClientPlugin.UI;
using GenericInliner.Core.Settings;

namespace GenericInliner.Core.UI.Controls
{
    public partial class PluginCheckbox : UserControl
    {
        public PluginControl pControl;

        public PluginControl PluginCtrl
        {
            get
            {
                // Save value to pControl, return
                pControl.Value = this.Value;
                return pControl;
            }
            private set
            {
                pControl = value;
            }
        }
        public void LoadSettings()
        {
            PluginControl savedCtrl = SettingsHelper.Load(PluginCtrl);
            if (savedCtrl.Value == null)
                return;

            bool value = (bool)savedCtrl.Value;
            chkMain.Checked = value;

            // Set main ctrl to saved
            PluginCtrl = savedCtrl;
        }
        public bool ResetControl()
        {
            chkMain.Checked = false;
            return true;
        }
        public bool Value
        {
            get
            {
                return chkMain.Checked;
            }
        }

        public PluginCheckbox(PluginControl pControl)
        {
            PluginCtrl = pControl;

            InitializeComponent();

            chkMain.Text = pControl.Description;

            LoadSettings();
        }

        private void PluginCheckbox_Load(object sender, EventArgs e)
        {
            Size = new Size(chkMain.Width + 50, chkMain.Height + 5);
            Invalidate();
        }
    }
}
