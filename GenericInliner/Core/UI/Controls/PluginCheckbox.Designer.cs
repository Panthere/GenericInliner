namespace GenericInliner.Core.UI.Controls
{
    partial class PluginCheckbox
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.chkMain = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // chkMain
            // 
            this.chkMain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.chkMain.AutoSize = true;
            this.chkMain.Location = new System.Drawing.Point(29, 33);
            this.chkMain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkMain.Name = "chkMain";
            this.chkMain.Size = new System.Drawing.Size(161, 18);
            this.chkMain.TabIndex = 1;
            this.chkMain.Text = "Checkbox Description";
            this.chkMain.UseVisualStyleBackColor = true;
            // 
            // PluginCheckbox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.chkMain);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "PluginCheckbox";
            this.Size = new System.Drawing.Size(288, 87);
            this.Load += new System.EventHandler(this.PluginCheckbox_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkMain;
    }
}
