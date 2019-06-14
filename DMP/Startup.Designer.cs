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
    partial class Startup
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.verNum_software_label = new System.Windows.Forms.Label();
            this.buildNum_software_label = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.verNum_software = new System.Windows.Forms.Label();
            this.buildNum_software = new System.Windows.Forms.Label();
            this.Mode = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.date_software = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.IDNum_stim = new System.Windows.Forms.Label();
            this.verNum_stim_label = new System.Windows.Forms.Label();
            this.verNum_stim = new System.Windows.Forms.Label();
            this.label_starting = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox1.Image = global::DMP.Properties.Resources.Mayo_clinic_logo;
            this.pictureBox1.Location = new System.Drawing.Point(556, 160);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(233, 252);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(18, 704);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(909, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Black", 36F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(88)))), ((int)(((byte)(90)))));
            this.label1.Location = new System.Drawing.Point(12, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(902, 68);
            this.label1.TabIndex = 2;
            this.label1.Text = "RC+S Data Management Platform";
            // 
            // verNum_software_label
            // 
            this.verNum_software_label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.verNum_software_label.AutoSize = true;
            this.verNum_software_label.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.verNum_software_label.Location = new System.Drawing.Point(46, 32);
            this.verNum_software_label.Name = "verNum_software_label";
            this.verNum_software_label.Padding = new System.Windows.Forms.Padding(3);
            this.verNum_software_label.Size = new System.Drawing.Size(116, 30);
            this.verNum_software_label.TabIndex = 5;
            this.verNum_software_label.Text = "Version # :";
            // 
            // buildNum_software_label
            // 
            this.buildNum_software_label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.buildNum_software_label.AutoSize = true;
            this.buildNum_software_label.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buildNum_software_label.Location = new System.Drawing.Point(46, 62);
            this.buildNum_software_label.Name = "buildNum_software_label";
            this.buildNum_software_label.Padding = new System.Windows.Forms.Padding(3);
            this.buildNum_software_label.Size = new System.Drawing.Size(91, 30);
            this.buildNum_software_label.TabIndex = 6;
            this.buildNum_software_label.Text = "Build # :";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BackgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker1_RunWorkerCompleted);
            // 
            // verNum_software
            // 
            this.verNum_software.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.verNum_software.AutoSize = true;
            this.verNum_software.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.verNum_software.Location = new System.Drawing.Point(168, 35);
            this.verNum_software.Name = "verNum_software";
            this.verNum_software.Size = new System.Drawing.Size(20, 22);
            this.verNum_software.TabIndex = 17;
            this.verNum_software.Text = " .";
            // 
            // buildNum_software
            // 
            this.buildNum_software.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.buildNum_software.AutoSize = true;
            this.buildNum_software.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buildNum_software.Location = new System.Drawing.Point(168, 65);
            this.buildNum_software.Name = "buildNum_software";
            this.buildNum_software.Size = new System.Drawing.Size(20, 22);
            this.buildNum_software.TabIndex = 18;
            this.buildNum_software.Text = " .";
            // 
            // Mode
            // 
            this.Mode.AutoSize = true;
            this.Mode.Font = new System.Drawing.Font("Arial Black", 16F, System.Drawing.FontStyle.Bold);
            this.Mode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(88)))), ((int)(((byte)(90)))));
            this.Mode.Location = new System.Drawing.Point(12, 9);
            this.Mode.Name = "Mode";
            this.Mode.Size = new System.Drawing.Size(88, 31);
            this.Mode.TabIndex = 22;
            this.Mode.Text = "label2";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBox2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox2.Controls.Add(this.date_software);
            this.groupBox2.Controls.Add(this.verNum_software_label);
            this.groupBox2.Controls.Add(this.verNum_software);
            this.groupBox2.Controls.Add(this.buildNum_software_label);
            this.groupBox2.Controls.Add(this.buildNum_software);
            this.groupBox2.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold);
            this.groupBox2.Location = new System.Drawing.Point(262, 150);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(288, 146);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Software";
            // 
            // date_software
            // 
            this.date_software.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.date_software.AutoSize = true;
            this.date_software.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.date_software.Location = new System.Drawing.Point(46, 101);
            this.date_software.Name = "date_software";
            this.date_software.Size = new System.Drawing.Size(15, 22);
            this.date_software.TabIndex = 19;
            this.date_software.Text = ".";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBox3.BackColor = System.Drawing.SystemColors.ControlLight;
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.IDNum_stim);
            this.groupBox3.Controls.Add(this.verNum_stim_label);
            this.groupBox3.Controls.Add(this.verNum_stim);
            this.groupBox3.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold);
            this.groupBox3.Location = new System.Drawing.Point(258, 314);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(292, 69);
            this.groupBox3.TabIndex = 27;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Summit RDK";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(50, 28);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(3);
            this.label2.Size = new System.Drawing.Size(116, 30);
            this.label2.TabIndex = 21;
            this.label2.Text = "Version # :";
            // 
            // IDNum_stim
            // 
            this.IDNum_stim.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.IDNum_stim.AutoSize = true;
            this.IDNum_stim.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IDNum_stim.Location = new System.Drawing.Point(170, 38);
            this.IDNum_stim.Name = "IDNum_stim";
            this.IDNum_stim.Size = new System.Drawing.Size(0, 22);
            this.IDNum_stim.TabIndex = 19;
            // 
            // verNum_stim_label
            // 
            this.verNum_stim_label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.verNum_stim_label.AutoSize = true;
            this.verNum_stim_label.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.verNum_stim_label.Location = new System.Drawing.Point(73, 18);
            this.verNum_stim_label.Name = "verNum_stim_label";
            this.verNum_stim_label.Padding = new System.Windows.Forms.Padding(3);
            this.verNum_stim_label.Size = new System.Drawing.Size(6, 22);
            this.verNum_stim_label.TabIndex = 8;
            // 
            // verNum_stim
            // 
            this.verNum_stim.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.verNum_stim.AutoSize = true;
            this.verNum_stim.Font = new System.Drawing.Font("Arial", 15.75F);
            this.verNum_stim.Location = new System.Drawing.Point(172, 28);
            this.verNum_stim.Name = "verNum_stim";
            this.verNum_stim.Size = new System.Drawing.Size(16, 24);
            this.verNum_stim.TabIndex = 20;
            this.verNum_stim.Text = ".";
            // 
            // label_starting
            // 
            this.label_starting.AutoSize = true;
            this.label_starting.BackColor = System.Drawing.Color.Transparent;
            this.label_starting.Font = new System.Drawing.Font("Arial Black", 18F, System.Drawing.FontStyle.Bold);
            this.label_starting.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(88)))), ((int)(((byte)(90)))));
            this.label_starting.Location = new System.Drawing.Point(6, 16);
            this.label_starting.Name = "label_starting";
            this.label_starting.Size = new System.Drawing.Size(399, 33);
            this.label_starting.TabIndex = 22;
            this.label_starting.Text = "DMP Starting... Please Wait...";
            this.label_starting.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBox1.Controls.Add(this.label_starting);
            this.groupBox1.Location = new System.Drawing.Point(262, 646);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(424, 52);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Location = new System.Drawing.Point(97, 418);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(775, 210);
            this.groupBox4.TabIndex = 29;
            this.groupBox4.TabStop = false;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Arial Black", 20F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(3, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(769, 191);
            this.label3.TabIndex = 31;
            this.label3.Text = "CAUTION - Investigational device!\r\nLimited by Federal (or United States) law to i" +
    "nvestigational use.\r\nThis device is intended to be operated by personnel trained" +
    " in its proper use.";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Startup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(934, 739);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.Mode);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Startup";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Startup";
            this.Load += new System.EventHandler(this.Startup_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        /// <summary>
        /// Logo
        /// </summary>

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label Mode;

        /// <summary>
        /// Requirement 2.1.2: Version, and Build
        /// The Startup-screen shall include identification of the software version and build number.
        /// </summary>
        private System.Windows.Forms.Label verNum_software_label;
        private System.Windows.Forms.Label buildNum_software_label;
        private System.Windows.Forms.Label verNum_software;
        private System.Windows.Forms.Label buildNum_software;


        /// <summary>
        /// Requirement 4.1.1: Initialization
        /// Following application start, the software shall initialize the Log File, load saved settings, verify access to the internal Bluetooth system,
        /// verify access and writability of data storage, and check for a Bluetooth-connected Medtronic Summit device.
        /// If connected the program shall read the INS battery level and display a warning if levels are below a threshold specified in the saved settings.
        /// Following initialization of the system, the software shall enter the Home Mode.
        /// </summary>
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label date_software;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label IDNum_stim;
        private System.Windows.Forms.Label verNum_stim_label;
        private System.Windows.Forms.Label verNum_stim;
        private System.Windows.Forms.Label label_starting;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox4;
        protected internal System.Windows.Forms.Label label3;
    }
}

