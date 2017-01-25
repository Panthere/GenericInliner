using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GenericInliner.Core.Providers;
using ClientPlugin.Models;
using ClientPlugin.UI;
using ClientPlugin.Enums;
using GenericInliner.Core.UI.Controls;
using GenericInliner.Core.Settings;
using GenericInliner.Core.Extensions;
using System.IO;
using ClientPlugin.Providers;
using System.Threading.Tasks;
using dnlib.DotNet;
using dnlib.DotNet.Writer;
using System.Threading;

namespace GenericInliner
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void Logger_MessageReceived(object sender, LogReceivedEventArgs e)
        {
            if (!this.IsHandleCreated)
            {
                return;
            }
            try
            {
                Invoke(new MethodInvoker(() =>
                {
                    try
                    {
                        if (txtLog.Text.Length > 15000)
                        {
                            txtLog.Clear();
                        }
                        if (!string.IsNullOrEmpty(e.AltSender))
                        {
                            txtLog.AppendText(string.Format("{2}{0} - {1}", e.AltSender, e.Message, Environment.NewLine));
                        }
                        else
                        {
                            txtLog.AppendText(string.Format("{2}[{0} - {3}] - {1}", e.Sender.Name, e.Message, Environment.NewLine, e.Sender.Version));
                        }
                    }
                    catch (Exception)
                    {


                    }
                }));
            }
            catch (Exception)
            {
                // Invalid operation stuff
            }
        }

        #region Triggers

        private void txtFile_TextChanged(object sender, EventArgs e)
        {
            ofd.FileName = Path.GetFileName(txtFile.Text);
            ofd.InitialDirectory = Path.GetDirectoryName(txtFile.Text);

            string outFile = txtFile.Text;

            int index = outFile.LastIndexOf('.');
            if (index != -1)
            {
                outFile = outFile.Insert(index, "_inlined");
            }

            txtOutput.Text = outFile;

            sfd.FileName = Path.GetFileName(outFile);
            sfd.InitialDirectory = Path.GetDirectoryName(outFile);

        }

        #endregion

        #region Drag Drop
        private void txtFile_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                e.Effect = DragDropEffects.All;
            }
        }

        private void txtFile_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length != 0)
            {
                txtFile.Text = files[0];
            }
        }

        private void txtFile_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                e.Effect = DragDropEffects.All;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        #endregion

        #region Plugins
        public void LoadPlugins()
        {
            Logger.Log("Plugins", "Loading Plugins");

            Plugins.LoadAll();
            foreach (TabPage tp in tabControl1.TabPages)
            {
                tp.Show();
            }
            // Visit tab page 3
            //tabControl1.SelectTab(2);
            //tabControl1.SelectTab(0);
            foreach (IGenericInliner inliner in Plugins.LoadedPlugins)
            {
                
                lvPlugins.AddPluginRow(inliner);
                clbEnabledPlugins.Items.Add(inliner.Name, true);

                if (inliner.Controls.Count == 0)
                    continue;

                List<PluginControl> txtBoxes = inliner.Controls.Where(x => x.Type == ControlType.Textbox).OrderBy(x => x.Index).ToList();
                List<PluginControl> chkBoxes = inliner.Controls.Where(x => x.Type == ControlType.Checkbox).OrderBy(x => x.Index).ToList();
                List<PluginControl> listViews = inliner.Controls.Where(x => x.Type == ControlType.Listview).OrderBy(x => x.Index).ToList();

                

                txtBoxes.ForEach(x => pTxtLayout.Controls.Add(new PluginTextBox(x)));
                chkBoxes.ForEach(x => pChkLayout.Controls.Add(new PluginCheckbox(x)));
                listViews.ForEach(x => pLVLayout.Controls.Add(new PluginListView(x)));

                Logger.Log("Plugins", string.Format("Adding Settings for {0}", inliner.Name));
            }

            Logger.Log("Plugins", string.Format("Loaded {0} Plugins", Plugins.LoadedPlugins.Count));

        }
        public List<IGenericInliner> GetEnabledPlugins()
        {
            return Plugins.LoadedPlugins.Where(x => clbEnabledPlugins.CheckedItems.Contains(x.Name)).ToList();
        }
        public List<PluginValue> GetPluginValues()
        {
            List<PluginValue> pVals = new List<PluginValue>();
            
            List<PluginControl> pControls = pTxtLayout.Controls.Cast<PluginTextBox>().Select(x => x.PluginCtrl).ToList();
            pControls.AddRange(pChkLayout.Controls.Cast<PluginCheckbox>().Select(x => x.PluginCtrl).ToList());
            pControls.AddRange(pLVLayout.Controls.Cast<PluginListView>().Select(x => x.PluginCtrl).ToList());

            pControls.ForEach(x => pVals.Add(new PluginValue() { 
                Name = x.Name,
                Control = x,
                Value = x.Value
            }));

            return pVals;
        }

        private void btnResetSaved_Click(object sender, EventArgs e)
        {
            SettingsHelper.Reset();
            pTxtLayout.Controls.Cast<PluginTextBox>().ToList().ForEach(x => x.ResetControl());
            pChkLayout.Controls.Cast<PluginCheckbox>().ToList().ForEach(x => x.ResetControl());
            pLVLayout.Controls.Cast<PluginListView>().ToList().ForEach(x => x.ResetControl());
            Logger.Log("Main", string.Format("Reset Plugin Settings to defaults"));
        }
        #endregion

        #region Form Handlers
        private void FrmMain_Load(object sender, EventArgs e)
        {
            this.Text += Application.ProductVersion;
            Logger.MessageReceived += new Logger.LogReceived(Logger_MessageReceived);
            LoadPlugins();
            Logger.Log("Main", "Initialized Form");
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Save settings for controls
            List<PluginControl> pControls = pTxtLayout.Controls.Cast<PluginTextBox>().Select(x => x.PluginCtrl).ToList();
            pControls.AddRange(pChkLayout.Controls.Cast<PluginCheckbox>().Select(x => x.PluginCtrl).ToList());
            pControls.AddRange(pLVLayout.Controls.Cast<PluginListView>().Select(x => x.PluginCtrl).ToList());

            SettingsHelper.Save(pControls);
        }
        #endregion

        #region Main Buttons
        private void btnBrowseOpen_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtFile.Text = ofd.FileName;
            }
        }

        private void btnBrowseSave_Click(object sender, EventArgs e)
        {
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtOutput.Text = sfd.FileName;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                string inFile = txtFile.Text;
                string outFile = txtOutput.Text;

                if (string.IsNullOrEmpty(inFile) || string.IsNullOrEmpty(outFile))
                {
                    MessageBox.Show("Select an input file and output location before continuing...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bool preserve = chkPreserve.Checked;
                bool useDummy = chkDummyLog.Checked;

                btnStart.Enabled = false;
                btnBrowseOpen.Enabled = false;
                btnBrowseSave.Enabled = false;

                List<IGenericInliner> pluginsToRun = GetEnabledPlugins();

                pbMain.Value = 0;
                pbMain.Maximum = pluginsToRun.Count;
                pbMain.Visible = true;

              

                Task.Factory.StartNew(() =>
                {
                    AssemblyHelper asm = new AssemblyHelper();

                    asm.Module = ModuleDefMD.Load(inFile);
                    asm.Settings = GetPluginValues();


                    foreach (IGenericInliner inliner in pluginsToRun)
                    {
                        try
                        {
                            inliner.Process(asm);

                            Invoke((MethodInvoker)(() => {
                                pbMain.Increment(1);
                            }));
                        }
                        catch (Exception ex)
                        {
                            Logger.Log("Processor", "Plugin failed to execute: " + ex.ToString());
                        }
                    }

                    // Write

                    Logger.Log("Processor", string.Format("Preparing to write to {0}", outFile));
                    ModuleWriterOptions opts = new ModuleWriterOptions(asm.Module);

                    if (preserve)
                    {
                        opts.MetaDataOptions.Flags = MetaDataFlags.PreserveAll;
                    }

                    if (useDummy)
                    {
                        opts.Logger = DummyLogger.NoThrowInstance;
                    }

                    try
                    {
                        asm.Module.Write(outFile, opts);
                        Logger.Log("Processor", string.Format("Written output to {0}", outFile));
                    }
                    catch (Exception ex)
                    {
                        Logger.Log("Processor", "Failed to write to output: " + ex.ToString());
                    }

                    Thread.Sleep(600);
                    Invoke((MethodInvoker)(() =>
                    {
                        pbMain.Visible = false;
                        btnStart.Enabled = true;
                        btnBrowseOpen.Enabled = true;
                        btnBrowseSave.Enabled = true;
                    }));
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Start Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

    }
}
