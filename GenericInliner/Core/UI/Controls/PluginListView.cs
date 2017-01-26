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
using System.Text.RegularExpressions;

namespace GenericInliner.Core.UI.Controls
{
    public partial class PluginListView : UserControl
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
        public List<string> Value
        {
            get
            {
                return lbMain.Items.OfType<string>().ToList();
            }
        }

        public void LoadSettings()
        {
            PluginControl savedCtrl = SettingsHelper.Load(PluginCtrl);
            if (savedCtrl.Value == null)
                return;

            List<string> value = (List<string>)savedCtrl.Value;
            value.ForEach(x => lbMain.Items.Add(x));

            // Set main ctrl to saved
            PluginCtrl = savedCtrl;
        }
        public bool ResetControl()
        {
            lbMain.Items.Clear();
            return true;
        }
        public PluginListView(PluginControl pControl)
        {
            PluginCtrl = pControl;

            InitializeComponent();

            lblDesc.Text = pControl.Description;
            lbMain.Items.Clear();

        }

        private bool IsRegexValid(string test)
        {
            try
            {
                new Regex(test);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbMain.SelectedItems.Count == 0)
                return;

            List<object> toRem = new List<object>();
            foreach (object item in lbMain.SelectedItems)
            {
                toRem.Add(item);
            }
            toRem.ForEach(x => lbMain.Items.Remove(x));

        }

        private void deleteAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lbMain.Items.Clear();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAdd.Text) || !IsRegexValid(txtAdd.Text))
                return;

            lbMain.Items.Add(txtAdd.Text);
            txtAdd.Clear();
        }

        private void PluginListView_Load(object sender, EventArgs e)
        {
            Size = new System.Drawing.Size(Size.Width - 100, Size.Height - 50);
            Invalidate();

            LoadSettings();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string clip = Clipboard.GetText();
                if (string.IsNullOrEmpty(clip))
                {
                    return;
                }
                foreach (string line in clip.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                {
                    lbMain.Items.Add(line);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error on Paste", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
