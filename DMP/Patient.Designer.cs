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
    partial class Patient
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
            this.components = new System.ComponentModel.Container();
            this.Mode = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.patientTabs = new System.Windows.Forms.TabControl();
            this.Home_button = new System.Windows.Forms.Button();
            this.IndicatorLabel = new System.Windows.Forms.Label();
            this.InactivityTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // Mode
            // 
            this.Mode.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Mode.AutoSize = true;
            this.Mode.Font = new System.Drawing.Font("Arial Black", 16F, System.Drawing.FontStyle.Bold);
            this.Mode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(88)))), ((int)(((byte)(90)))));
            this.Mode.Location = new System.Drawing.Point(50, 58);
            this.Mode.Name = "Mode";
            this.Mode.Size = new System.Drawing.Size(88, 31);
            this.Mode.TabIndex = 7;
            this.Mode.Text = "label2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Black", 16F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(88)))), ((int)(((byte)(90)))));
            this.label1.Location = new System.Drawing.Point(50, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(296, 31);
            this.label1.TabIndex = 6;
            this.label1.Text = "RC+S Data Management Platform";
            // 
            // patientTabs
            // 
            this.patientTabs.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.patientTabs.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.patientTabs.Location = new System.Drawing.Point(56, 115);
            this.patientTabs.Multiline = true;
            this.patientTabs.Name = "patientTabs";
            this.patientTabs.SelectedIndex = 0;
            this.patientTabs.Size = new System.Drawing.Size(850, 650);
            this.patientTabs.TabIndex = 13;
            this.patientTabs.SelectedIndexChanged += new System.EventHandler(this.patientTabs_SelectedIndexChanged);
            // 
            // Home_button
            // 
            this.Home_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Home_button.Font = new System.Drawing.Font("Arial Black", 16F);
            this.Home_button.Location = new System.Drawing.Point(756, 30);
            this.Home_button.Name = "Home_button";
            this.Home_button.Size = new System.Drawing.Size(150, 52);
            this.Home_button.TabIndex = 21;
            this.Home_button.Text = "Home";
            this.Home_button.UseVisualStyleBackColor = true;
            this.Home_button.Click += new System.EventHandler(this.Home_button_Click);
            // 
            // IndicatorLabel
            // 
            this.IndicatorLabel.AutoSize = true;
            this.IndicatorLabel.Font = new System.Drawing.Font("Arial", 16F);
            this.IndicatorLabel.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.IndicatorLabel.Location = new System.Drawing.Point(56, 772);
            this.IndicatorLabel.Name = "IndicatorLabel";
            this.IndicatorLabel.Size = new System.Drawing.Size(18, 25);
            this.IndicatorLabel.TabIndex = 22;
            this.IndicatorLabel.Text = " ";
            // 
            // InactivityTimer
            // 
            this.InactivityTimer.Enabled = true;
            this.InactivityTimer.Tick += new System.EventHandler(this.InactivityTimer_Tick);
            // 
            // Patient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(934, 812);
            this.ControlBox = false;
            this.Controls.Add(this.IndicatorLabel);
            this.Controls.Add(this.Home_button);
            this.Controls.Add(this.patientTabs);
            this.Controls.Add(this.Mode);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Patient";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Patient";
            this.Activated += new System.EventHandler(this.Patient_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Patient_FormClosing);
            this.Load += new System.EventHandler(this.Patient_Load);
            this.Shown += new System.EventHandler(this.Patient_Load);
            this.Enter += new System.EventHandler(this.Patient_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected internal System.Windows.Forms.Label Mode;
        protected internal System.Windows.Forms.Label label1;
        protected internal System.Windows.Forms.TabControl patientTabs;
        protected internal System.Windows.Forms.Button Home_button;
        protected internal System.Windows.Forms.Label IndicatorLabel;
        protected internal System.Windows.Forms.Timer InactivityTimer;
    }
}