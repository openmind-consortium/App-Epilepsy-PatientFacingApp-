/* Copyright (c) 2017-2018, Mayo Foundation for Medical Education and Research (MFMER), All rights reserved. 
Academic non-commercial use of this software is allowed with expressed permission of the developers. 
MFMER disclaims all implied warranties of merchantability and fitness for a particular purpose with 
respect to this software, its application, and any verbal or written statements regarding its use. 
The software may not be distributed to third parties without consent of MFMER. Use of this software 
constitutes acceptance of these terms. 
Contributors: Daniel Crepeau, Tal Pal Attia, Jan Cimbalnik, Hari Guragain, Mona Nasseri, Vaclav Kremen, 
Benjamin Brinkmann, Matt Stead, Gregory Worrell.  */

namespace DMP
{
    partial class DMP_Main_MDIParent
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DMP_Main_MDIParent));
            this.BatterytoolStrip = new SafeControls.SafeStatusStrip();
            this.toolStripLabel1 = new SafeControls.SafeToolStripLabel();
            this.INSbatteryLevelText = new SafeControls.SafeToolStripLabel();
            this.toolStripLabel3 = new SafeControls.SafeToolStripLabel();
            this.ctmBatteryLevelText = new SafeControls.SafeToolStripLabel();
            this.toolStripLabel5 = new SafeControls.SafeToolStripLabel();
            this.tabletBatteryLevelText = new SafeControls.SafeToolStripLabel();
            this.BatterytoolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // BatterytoolStrip
            // 
            this.BatterytoolStrip.Dock = System.Windows.Forms.DockStyle.Top;
            this.BatterytoolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.INSbatteryLevelText,
            this.toolStripLabel3,
            this.ctmBatteryLevelText,
            this.toolStripLabel5,
            this.tabletBatteryLevelText});
            this.BatterytoolStrip.Location = new System.Drawing.Point(0, 0);
            this.BatterytoolStrip.Name = "BatterytoolStrip";
            this.BatterytoolStrip.Size = new System.Drawing.Size(984, 22);
            this.BatterytoolStrip.TabIndex = 0;
            this.BatterytoolStrip.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(95, 17);
            this.toolStripLabel1.Text = "INS Battery Level";
            // 
            // INSbatteryLevelText
            // 
            this.INSbatteryLevelText.Name = "INSbatteryLevelText";
            this.INSbatteryLevelText.Size = new System.Drawing.Size(29, 17);
            this.INSbatteryLevelText.Text = "N\\A";
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(103, 17);
            this.toolStripLabel3.Text = "CTM Battery Level";
            // 
            // ctmBatteryLevelText
            // 
            this.ctmBatteryLevelText.Name = "ctmBatteryLevelText";
            this.ctmBatteryLevelText.Size = new System.Drawing.Size(29, 17);
            this.ctmBatteryLevelText.Text = "N\\A";
            // 
            // toolStripLabel5
            // 
            this.toolStripLabel5.Name = "toolStripLabel5";
            this.toolStripLabel5.Size = new System.Drawing.Size(110, 17);
            this.toolStripLabel5.Text = "Tablet Battery Level";
            // 
            // tabletBatteryLevelText
            // 
            this.tabletBatteryLevelText.Name = "tabletBatteryLevelText";
            this.tabletBatteryLevelText.Size = new System.Drawing.Size(29, 17);
            this.tabletBatteryLevelText.Text = "N\\A";
            // 
            // DMP_Main_MDIParent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(984, 962);
            this.ControlBox = false;
            this.Controls.Add(this.BatterytoolStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.KeyPreview = true;
            this.Name = "DMP_Main_MDIParent";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DMP";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.MdiChildActivate += new System.EventHandler(this.DMP_Main_MDIParent_MdiChildActivate);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DMP_Main_MDIParent_KeyDown);
            this.BatterytoolStrip.ResumeLayout(false);
            this.BatterytoolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        protected internal SafeControls.SafeStatusStrip BatterytoolStrip;
        protected internal SafeControls.SafeToolStripLabel toolStripLabel1;
        protected internal SafeControls.SafeToolStripLabel INSbatteryLevelText;
        protected internal SafeControls.SafeToolStripLabel toolStripLabel3;
        protected internal SafeControls.SafeToolStripLabel ctmBatteryLevelText;
        protected internal SafeControls.SafeToolStripLabel toolStripLabel5;
        protected internal SafeControls.SafeToolStripLabel tabletBatteryLevelText;
        #endregion
        /*
        protected internal SafeControls.SafeStatusStrip BatterytoolStrip;
        protected internal SafeControls.SafeToolStripLabel toolStripLabel1;
        protected internal SafeControls.SafeToolStripLabel INSbatteryLevelText;
        protected internal SafeControls.SafeToolStripLabel toolStripLabel3;
        protected internal SafeControls.SafeToolStripLabel ctmBatteryLevelText;
        protected internal SafeControls.SafeToolStripLabel toolStripLabel5;
        protected internal SafeControls.SafeToolStripLabel tabletBatteryLevelText;
        */
    }
}



