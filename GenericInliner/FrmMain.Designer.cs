namespace GenericInliner
{
    partial class FrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.clbEnabledPlugins = new System.Windows.Forms.CheckedListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lvPlugins = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnResetSaved = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.pLVLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.pTxtLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.pChkLayout = new System.Windows.Forms.FlowLayoutPanel();
            this.btnStart = new System.Windows.Forms.Button();
            this.pbMain = new System.Windows.Forms.ProgressBar();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.btnBrowseOpen = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.btnBrowseSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkPreserve = new System.Windows.Forms.CheckBox();
            this.chkDummyLog = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(10, 8);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(717, 554);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.AllowDrop = true;
            this.tabPage1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.txtLog);
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.pbMain);
            this.tabPage1.Location = new System.Drawing.Point(4, 33);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(709, 517);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Main";
            this.tabPage1.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtFile_DragDrop);
            this.tabPage1.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtFile_DragEnter);
            this.tabPage1.DragOver += new System.Windows.Forms.DragEventHandler(this.txtFile_DragOver);
            // 
            // clbEnabledPlugins
            // 
            this.clbEnabledPlugins.BackColor = System.Drawing.SystemColors.Control;
            this.clbEnabledPlugins.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.clbEnabledPlugins.CheckOnClick = true;
            this.clbEnabledPlugins.FormattingEnabled = true;
            this.clbEnabledPlugins.Location = new System.Drawing.Point(24, 34);
            this.clbEnabledPlugins.Name = "clbEnabledPlugins";
            this.clbEnabledPlugins.Size = new System.Drawing.Size(286, 119);
            this.clbEnabledPlugins.TabIndex = 4;
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage2.Controls.Add(this.lvPlugins);
            this.tabPage2.Controls.Add(this.btnResetSaved);
            this.tabPage2.Location = new System.Drawing.Point(4, 33);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(709, 517);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Plugins";
            // 
            // lvPlugins
            // 
            this.lvPlugins.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5});
            this.lvPlugins.FullRowSelect = true;
            this.lvPlugins.GridLines = true;
            this.lvPlugins.Location = new System.Drawing.Point(17, 18);
            this.lvPlugins.Name = "lvPlugins";
            this.lvPlugins.Size = new System.Drawing.Size(673, 245);
            this.lvPlugins.TabIndex = 1;
            this.lvPlugins.UseCompatibleStateImageBehavior = false;
            this.lvPlugins.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 159;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Description";
            this.columnHeader2.Width = 184;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Author";
            this.columnHeader3.Width = 135;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Version";
            this.columnHeader4.Width = 93;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Settings";
            this.columnHeader5.Width = 75;
            // 
            // btnResetSaved
            // 
            this.btnResetSaved.Location = new System.Drawing.Point(570, 473);
            this.btnResetSaved.Name = "btnResetSaved";
            this.btnResetSaved.Size = new System.Drawing.Size(109, 23);
            this.btnResetSaved.TabIndex = 0;
            this.btnResetSaved.Text = "Reset Settings";
            this.btnResetSaved.UseVisualStyleBackColor = true;
            this.btnResetSaved.Click += new System.EventHandler(this.btnResetSaved_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage3.Controls.Add(this.pLVLayout);
            this.tabPage3.Controls.Add(this.pTxtLayout);
            this.tabPage3.Controls.Add(this.pChkLayout);
            this.tabPage3.Location = new System.Drawing.Point(4, 33);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(709, 517);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Plugin Settings";
            // 
            // pLVLayout
            // 
            this.pLVLayout.AutoScroll = true;
            this.pLVLayout.Location = new System.Drawing.Point(19, 252);
            this.pLVLayout.Name = "pLVLayout";
            this.pLVLayout.Size = new System.Drawing.Size(644, 208);
            this.pLVLayout.TabIndex = 2;
            // 
            // pTxtLayout
            // 
            this.pTxtLayout.AutoScroll = true;
            this.pTxtLayout.Location = new System.Drawing.Point(386, 25);
            this.pTxtLayout.Name = "pTxtLayout";
            this.pTxtLayout.Size = new System.Drawing.Size(277, 221);
            this.pTxtLayout.TabIndex = 1;
            // 
            // pChkLayout
            // 
            this.pChkLayout.AutoScroll = true;
            this.pChkLayout.Location = new System.Drawing.Point(19, 25);
            this.pChkLayout.Name = "pChkLayout";
            this.pChkLayout.Size = new System.Drawing.Size(274, 221);
            this.pChkLayout.TabIndex = 0;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(128, 65);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 33);
            this.btnStart.TabIndex = 9;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // pbMain
            // 
            this.pbMain.Location = new System.Drawing.Point(-4, 500);
            this.pbMain.Name = "pbMain";
            this.pbMain.Size = new System.Drawing.Size(717, 21);
            this.pbMain.TabIndex = 10;
            this.pbMain.Visible = false;
            // 
            // txtFile
            // 
            this.txtFile.AllowDrop = true;
            this.txtFile.Location = new System.Drawing.Point(24, 46);
            this.txtFile.Name = "txtFile";
            this.txtFile.ReadOnly = true;
            this.txtFile.Size = new System.Drawing.Size(216, 22);
            this.txtFile.TabIndex = 7;
            this.txtFile.TextChanged += new System.EventHandler(this.txtFile_TextChanged);
            this.txtFile.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtFile_DragDrop);
            this.txtFile.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtFile_DragEnter);
            this.txtFile.DragOver += new System.Windows.Forms.DragEventHandler(this.txtFile_DragOver);
            // 
            // btnBrowseOpen
            // 
            this.btnBrowseOpen.Location = new System.Drawing.Point(246, 45);
            this.btnBrowseOpen.Name = "btnBrowseOpen";
            this.btnBrowseOpen.Size = new System.Drawing.Size(64, 23);
            this.btnBrowseOpen.TabIndex = 8;
            this.btnBrowseOpen.Text = "...";
            this.btnBrowseOpen.UseVisualStyleBackColor = true;
            this.btnBrowseOpen.Click += new System.EventHandler(this.btnBrowseOpen_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnBrowseSave);
            this.groupBox1.Controls.Add(this.txtOutput);
            this.groupBox1.Controls.Add(this.txtFile);
            this.groupBox1.Controls.Add(this.btnBrowseOpen);
            this.groupBox1.Location = new System.Drawing.Point(19, 19);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(331, 153);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "File Selection";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.clbEnabledPlugins);
            this.groupBox2.Location = new System.Drawing.Point(19, 178);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(331, 163);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Enabled Plugins";
            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(24, 101);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.ReadOnly = true;
            this.txtOutput.Size = new System.Drawing.Size(216, 22);
            this.txtOutput.TabIndex = 9;
            // 
            // btnBrowseSave
            // 
            this.btnBrowseSave.Location = new System.Drawing.Point(246, 100);
            this.btnBrowseSave.Name = "btnBrowseSave";
            this.btnBrowseSave.Size = new System.Drawing.Size(64, 23);
            this.btnBrowseSave.TabIndex = 10;
            this.btnBrowseSave.Text = "...";
            this.btnBrowseSave.UseVisualStyleBackColor = true;
            this.btnBrowseSave.Click += new System.EventHandler(this.btnBrowseSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 14);
            this.label1.TabIndex = 11;
            this.label1.Text = "Output";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 14);
            this.label2.TabIndex = 12;
            this.label2.Text = "Input";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkDummyLog);
            this.groupBox3.Controls.Add(this.chkPreserve);
            this.groupBox3.Location = new System.Drawing.Point(356, 19);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(331, 153);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Output Settings";
            // 
            // chkPreserve
            // 
            this.chkPreserve.AutoSize = true;
            this.chkPreserve.Checked = true;
            this.chkPreserve.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPreserve.Location = new System.Drawing.Point(19, 29);
            this.chkPreserve.Name = "chkPreserve";
            this.chkPreserve.Size = new System.Drawing.Size(149, 18);
            this.chkPreserve.TabIndex = 0;
            this.chkPreserve.Text = "Preserve All Tokens";
            this.chkPreserve.UseVisualStyleBackColor = true;
            // 
            // chkDummyLog
            // 
            this.chkDummyLog.AutoSize = true;
            this.chkDummyLog.Checked = true;
            this.chkDummyLog.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDummyLog.Location = new System.Drawing.Point(19, 53);
            this.chkDummyLog.Name = "chkDummyLog";
            this.chkDummyLog.Size = new System.Drawing.Size(120, 18);
            this.chkDummyLog.TabIndex = 1;
            this.chkDummyLog.Text = "Dummy Logger";
            this.chkDummyLog.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.btnStart);
            this.groupBox4.Location = new System.Drawing.Point(356, 178);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(331, 163);
            this.groupBox4.TabIndex = 14;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Controls";
            // 
            // txtLog
            // 
            this.txtLog.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.Location = new System.Drawing.Point(19, 347);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.Size = new System.Drawing.Size(458, 147);
            this.txtLog.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(499, 350);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 14);
            this.label3.TabIndex = 16;
            this.label3.Text = "Credits";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(499, 377);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(175, 98);
            this.label4.TabIndex = 17;
            this.label4.Text = "Created by Pan\r\nUses dnlib by 0xd4d\r\n\r\nThis tool was created\r\nfor rtn-team.cc mem" +
    "bers\r\nRedistribution on other\r\nforums/websites isn\'t cool.";
            // 
            // ofd
            // 
            this.ofd.SupportMultiDottedExtensions = true;
            this.ofd.Title = "Select .NET PE to process";
            // 
            // sfd
            // 
            this.sfd.SupportMultiDottedExtensions = true;
            this.sfd.Title = "Select save location for output";
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(717, 554);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Generic Inliner - v";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.FlowLayoutPanel pChkLayout;
        private System.Windows.Forms.FlowLayoutPanel pLVLayout;
        private System.Windows.Forms.FlowLayoutPanel pTxtLayout;
        private System.Windows.Forms.Button btnResetSaved;
        private System.Windows.Forms.ListView lvPlugins;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.CheckedListBox clbEnabledPlugins;
        private System.Windows.Forms.ProgressBar pbMain;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnBrowseOpen;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnBrowseSave;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkDummyLog;
        private System.Windows.Forms.CheckBox chkPreserve;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.SaveFileDialog sfd;
    }
}

