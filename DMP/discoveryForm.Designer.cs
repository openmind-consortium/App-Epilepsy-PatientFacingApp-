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
    partial class discoveryForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(discoveryForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel_INSStatus = new System.Windows.Forms.Panel();
            this.INSname = new System.Windows.Forms.Label();
            this.INSStatusText = new System.Windows.Forms.Label();
            this.INSBattery_circularProgressBar = new CircularProgressBar.CircularProgressBar();
            this.INSBatteryText = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(809, 612);
            this.panel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel_INSStatus);
            this.groupBox1.Controls.Add(this.INSname);
            this.groupBox1.Controls.Add(this.INSStatusText);
            this.groupBox1.Controls.Add(this.INSBattery_circularProgressBar);
            this.groupBox1.Controls.Add(this.INSBatteryText);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(263, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(277, 142);
            this.groupBox1.TabIndex = 45;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connected INS";
            // 
            // panel_INSStatus
            // 
            this.panel_INSStatus.BackColor = System.Drawing.Color.Silver;
            this.panel_INSStatus.Location = new System.Drawing.Point(59, 63);
            this.panel_INSStatus.Name = "panel_INSStatus";
            this.panel_INSStatus.Size = new System.Drawing.Size(30, 30);
            this.panel_INSStatus.TabIndex = 49;
            // 
            // INSname
            // 
            this.INSname.AutoSize = true;
            this.INSname.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.INSname.Location = new System.Drawing.Point(30, 38);
            this.INSname.Name = "INSname";
            this.INSname.Size = new System.Drawing.Size(56, 19);
            this.INSname.TabIndex = 43;
            this.INSname.Text = "INS ID";
            // 
            // INSStatusText
            // 
            this.INSStatusText.AutoSize = true;
            this.INSStatusText.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.INSStatusText.Location = new System.Drawing.Point(31, 101);
            this.INSStatusText.Name = "INSStatusText";
            this.INSStatusText.Size = new System.Drawing.Size(88, 14);
            this.INSStatusText.TabIndex = 42;
            this.INSStatusText.Text = "Not Connected";
            this.INSStatusText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // INSBattery_circularProgressBar
            // 
            this.INSBattery_circularProgressBar.AnimationFunction = WinFormAnimation.KnownAnimationFunctions.Liner;
            this.INSBattery_circularProgressBar.AnimationSpeed = 500;
            this.INSBattery_circularProgressBar.BackColor = System.Drawing.Color.Transparent;
            this.INSBattery_circularProgressBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Bold);
            this.INSBattery_circularProgressBar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.INSBattery_circularProgressBar.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.INSBattery_circularProgressBar.InnerMargin = 2;
            this.INSBattery_circularProgressBar.InnerWidth = -1;
            this.INSBattery_circularProgressBar.Location = new System.Drawing.Point(156, 38);
            this.INSBattery_circularProgressBar.MarqueeAnimationSpeed = 2000;
            this.INSBattery_circularProgressBar.Name = "INSBattery_circularProgressBar";
            this.INSBattery_circularProgressBar.OuterColor = System.Drawing.Color.Firebrick;
            this.INSBattery_circularProgressBar.OuterMargin = -25;
            this.INSBattery_circularProgressBar.OuterWidth = 26;
            this.INSBattery_circularProgressBar.ProgressColor = System.Drawing.Color.MediumBlue;
            this.INSBattery_circularProgressBar.ProgressWidth = 25;
            this.INSBattery_circularProgressBar.SecondaryFont = new System.Drawing.Font("Microsoft Sans Serif", 36F);
            this.INSBattery_circularProgressBar.Size = new System.Drawing.Size(75, 75);
            this.INSBattery_circularProgressBar.StartAngle = 270;
            this.INSBattery_circularProgressBar.SubscriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.INSBattery_circularProgressBar.SubscriptMargin = new System.Windows.Forms.Padding(10, -35, 0, 0);
            this.INSBattery_circularProgressBar.SubscriptText = ".23";
            this.INSBattery_circularProgressBar.SuperscriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.INSBattery_circularProgressBar.SuperscriptMargin = new System.Windows.Forms.Padding(10, 35, 0, 0);
            this.INSBattery_circularProgressBar.SuperscriptText = "°C";
            this.INSBattery_circularProgressBar.TabIndex = 39;
            this.INSBattery_circularProgressBar.TextMargin = new System.Windows.Forms.Padding(8, 8, 0, 0);
            this.INSBattery_circularProgressBar.Value = 68;
            // 
            // INSBatteryText
            // 
            this.INSBatteryText.AutoSize = true;
            this.INSBatteryText.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.INSBatteryText.Location = new System.Drawing.Point(183, 116);
            this.INSBatteryText.Name = "INSBatteryText";
            this.INSBatteryText.Size = new System.Drawing.Size(25, 14);
            this.INSBatteryText.TabIndex = 41;
            this.INSBatteryText.Text = "N\\A";
            this.INSBatteryText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Enabled = false;
            this.textBox1.Font = new System.Drawing.Font("Arial Black", 13F, System.Drawing.FontStyle.Bold);
            this.textBox1.ForeColor = System.Drawing.Color.Black;
            this.textBox1.Location = new System.Drawing.Point(9, 179);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(788, 430);
            this.textBox1.TabIndex = 19;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // discoveryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(809, 612);
            this.Controls.Add(this.panel1);
            this.Name = "discoveryForm";
            this.ShowInTaskbar = false;
            this.Text = "Connect to Device";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        protected internal System.Windows.Forms.Label INSStatusText;
        protected internal System.Windows.Forms.Label INSBatteryText;
        protected internal CircularProgressBar.CircularProgressBar INSBattery_circularProgressBar;
        protected internal System.Windows.Forms.Panel panel1;
        protected internal System.Windows.Forms.TextBox textBox1;
        protected internal System.Windows.Forms.Label INSname;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel_INSStatus;
    }
}