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
    public partial class PluginTextBox : UserControl
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

            string value = (string)savedCtrl.Value;
            txtMain.Text = value;

            // Set main ctrl to saved
            PluginCtrl = savedCtrl;
        }
        public bool ResetControl()
        {
            txtMain.Clear();
            return true;
        }
        public string Value
        {
            get
            {
                return txtMain.Text;
            }
        }

        public PluginTextBox(PluginControl pControl)
        {
            PluginCtrl = pControl;

            InitializeComponent();

            lblDesc.Text = pControl.Description;
            txtMain.Clear();

            LoadSettings();
        }

        private void PluginTextBox_Load(object sender, EventArgs e)
        {
            Size = new Size(txtMain.Width, lblDesc.Height + txtMain.Height + 5);
            Invalidate();
        }
    }
}
