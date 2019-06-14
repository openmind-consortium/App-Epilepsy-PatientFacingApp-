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
    partial class ProgramControlForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.Analytics_groupBox = new System.Windows.Forms.GroupBox();
            this.checkBox_Analytics = new System.Windows.Forms.CheckBox();
            this.INS_groupBox = new System.Windows.Forms.GroupBox();
            this.btnStart_INS = new System.Windows.Forms.Button();
            this.btnStop_INS = new System.Windows.Forms.Button();
            this.App_groupBox = new System.Windows.Forms.GroupBox();
            this.btnStop_App = new System.Windows.Forms.Button();
            this.btnStart_App = new System.Windows.Forms.Button();
            this.Notes_groupBox = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnSave_Notes = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.Analytics_groupBox.SuspendLayout();
            this.INS_groupBox.SuspendLayout();
            this.App_groupBox.SuspendLayout();
            this.Notes_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Analytics_groupBox);
            this.panel1.Controls.Add(this.INS_groupBox);
            this.panel1.Controls.Add(this.App_groupBox);
            this.panel1.Controls.Add(this.Notes_groupBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(834, 637);
            this.panel1.TabIndex = 0;
            // 
            // Analytics_groupBox
            // 
            this.Analytics_groupBox.Controls.Add(this.checkBox_Analytics);
            this.Analytics_groupBox.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.Analytics_groupBox.Location = new System.Drawing.Point(491, 12);
            this.Analytics_groupBox.Name = "Analytics_groupBox";
            this.Analytics_groupBox.Size = new System.Drawing.Size(300, 100);
            this.Analytics_groupBox.TabIndex = 31;
            this.Analytics_groupBox.TabStop = false;
            this.Analytics_groupBox.Text = "Analytics Control";
            // 
            // checkBox_Analytics
            // 
            this.checkBox_Analytics.AutoSize = true;
            this.checkBox_Analytics.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_Analytics.Location = new System.Drawing.Point(18, 38);
            this.checkBox_Analytics.Name = "checkBox_Analytics";
            this.checkBox_Analytics.Size = new System.Drawing.Size(216, 42);
            this.checkBox_Analytics.TabIndex = 0;
            this.checkBox_Analytics.Text = "Run Seizure\r\nPrediction and Detection";
            this.checkBox_Analytics.UseVisualStyleBackColor = true;
            this.checkBox_Analytics.CheckedChanged += new System.EventHandler(this.checkBox_Analytics_CheckedChanged);
            // 
            // INS_groupBox
            // 
            this.INS_groupBox.Controls.Add(this.btnStart_INS);
            this.INS_groupBox.Controls.Add(this.btnStop_INS);
            this.INS_groupBox.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.INS_groupBox.Location = new System.Drawing.Point(491, 144);
            this.INS_groupBox.Name = "INS_groupBox";
            this.INS_groupBox.Size = new System.Drawing.Size(300, 100);
            this.INS_groupBox.TabIndex = 30;
            this.INS_groupBox.TabStop = false;
            this.INS_groupBox.Text = "INS Control";
            // 
            // btnStart_INS
            // 
            this.btnStart_INS.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.btnStart_INS.Location = new System.Drawing.Point(162, 28);
            this.btnStart_INS.Name = "btnStart_INS";
            this.btnStart_INS.Size = new System.Drawing.Size(132, 54);
            this.btnStart_INS.TabIndex = 21;
            this.btnStart_INS.Text = "Connect";
            this.btnStart_INS.UseVisualStyleBackColor = true;
            this.btnStart_INS.Click += new System.EventHandler(this.btnStart_INS_Click);
            // 
            // btnStop_INS
            // 
            this.btnStop_INS.Enabled = false;
            this.btnStop_INS.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.btnStop_INS.Location = new System.Drawing.Point(18, 28);
            this.btnStop_INS.Name = "btnStop_INS";
            this.btnStop_INS.Size = new System.Drawing.Size(132, 54);
            this.btnStop_INS.TabIndex = 20;
            this.btnStop_INS.Text = "Disconnect";
            this.btnStop_INS.UseVisualStyleBackColor = true;
            this.btnStop_INS.Click += new System.EventHandler(this.btnStop_INS_Click);
            // 
            // App_groupBox
            // 
            this.App_groupBox.Controls.Add(this.btnStop_App);
            this.App_groupBox.Controls.Add(this.btnStart_App);
            this.App_groupBox.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.App_groupBox.Location = new System.Drawing.Point(491, 301);
            this.App_groupBox.Name = "App_groupBox";
            this.App_groupBox.Size = new System.Drawing.Size(300, 100);
            this.App_groupBox.TabIndex = 24;
            this.App_groupBox.TabStop = false;
            this.App_groupBox.Text = "RC+S Data Management Platform Control";
            // 
            // btnStop_App
            // 
            this.btnStop_App.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.btnStop_App.Location = new System.Drawing.Point(162, 38);
            this.btnStop_App.Name = "btnStop_App";
            this.btnStop_App.Size = new System.Drawing.Size(132, 54);
            this.btnStop_App.TabIndex = 21;
            this.btnStop_App.Text = "Close Application";
            this.btnStop_App.UseVisualStyleBackColor = true;
            this.btnStop_App.Click += new System.EventHandler(this.btnStop_App_Click);
            // 
            // btnStart_App
            // 
            this.btnStart_App.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.btnStart_App.Location = new System.Drawing.Point(18, 38);
            this.btnStart_App.Name = "btnStart_App";
            this.btnStart_App.Size = new System.Drawing.Size(132, 54);
            this.btnStart_App.TabIndex = 20;
            this.btnStart_App.Text = "Restart Application";
            this.btnStart_App.UseVisualStyleBackColor = true;
            this.btnStart_App.Click += new System.EventHandler(this.btnStart_App_Click);
            // 
            // Notes_groupBox
            // 
            this.Notes_groupBox.Controls.Add(this.textBox1);
            this.Notes_groupBox.Controls.Add(this.btnSave_Notes);
            this.Notes_groupBox.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.Notes_groupBox.Location = new System.Drawing.Point(12, 12);
            this.Notes_groupBox.Name = "Notes_groupBox";
            this.Notes_groupBox.Size = new System.Drawing.Size(473, 457);
            this.Notes_groupBox.TabIndex = 22;
            this.Notes_groupBox.TabStop = false;
            this.Notes_groupBox.Text = "Physician Notes";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Arial", 12F);
            this.textBox1.ForeColor = System.Drawing.Color.Gray;
            this.textBox1.Location = new System.Drawing.Point(6, 38);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(452, 351);
            this.textBox1.TabIndex = 21;
            this.textBox1.Text = "Physician Note";
            this.textBox1.Click += new System.EventHandler(this.textBox1_Click);
            this.textBox1.Enter += new System.EventHandler(this.textBox1_Enter);
            this.textBox1.Leave += new System.EventHandler(this.textBox1_Leave);
            this.textBox1.MouseEnter += new System.EventHandler(this.textBox1_Enter);
            // 
            // btnSave_Notes
            // 
            this.btnSave_Notes.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.btnSave_Notes.Location = new System.Drawing.Point(163, 395);
            this.btnSave_Notes.Name = "btnSave_Notes";
            this.btnSave_Notes.Size = new System.Drawing.Size(132, 54);
            this.btnSave_Notes.TabIndex = 20;
            this.btnSave_Notes.Text = "Save Note";
            this.btnSave_Notes.UseVisualStyleBackColor = true;
            this.btnSave_Notes.Click += new System.EventHandler(this.btnSave_Notes_Click);
            // 
            // ProgramControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 637);
            this.Controls.Add(this.panel1);
            this.Name = "ProgramControlForm";
            this.Text = "Program Control";
            this.panel1.ResumeLayout(false);
            this.Analytics_groupBox.ResumeLayout(false);
            this.Analytics_groupBox.PerformLayout();
            this.INS_groupBox.ResumeLayout(false);
            this.App_groupBox.ResumeLayout(false);
            this.Notes_groupBox.ResumeLayout(false);
            this.Notes_groupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        protected internal System.Windows.Forms.GroupBox INS_groupBox;
        protected internal System.Windows.Forms.Button btnStart_INS;
        protected internal System.Windows.Forms.Button btnStop_INS;
        protected internal System.Windows.Forms.GroupBox Analytics_groupBox;
        protected internal System.Windows.Forms.Panel panel1;
        protected internal System.Windows.Forms.GroupBox Notes_groupBox;
        protected internal System.Windows.Forms.Button btnSave_Notes;
        protected internal System.Windows.Forms.TextBox textBox1;
        protected internal System.Windows.Forms.GroupBox App_groupBox;
        protected internal System.Windows.Forms.Button btnStop_App;
        protected internal System.Windows.Forms.Button btnStart_App;
        protected internal System.Windows.Forms.CheckBox checkBox_Analytics;
    }
}