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
    partial class Home
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Home));
            this.label1 = new System.Windows.Forms.Label();
            this.Mode = new System.Windows.Forms.Label();
            this.physicianMode = new System.Windows.Forms.Button();
            this.CloseApp = new System.Windows.Forms.Button();
            this.Eventbutton = new System.Windows.Forms.Button();
            this.Mode_groupBox = new System.Windows.Forms.GroupBox();
            this.patientMode = new System.Windows.Forms.Button();
            this.Annotations_groupBox = new System.Windows.Forms.GroupBox();
            this.label_recTime = new System.Windows.Forms.Label();
            this.Medbutton = new System.Windows.Forms.Button();
            this.Indicators_groupBox = new System.Windows.Forms.GroupBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.INSIDText = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.panel_Privacy = new System.Windows.Forms.Panel();
            this.privacyStatusText = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.DataStatusText = new System.Windows.Forms.Label();
            this.panel_DataStatus = new System.Windows.Forms.Panel();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.panel_StimStatus = new System.Windows.Forms.Panel();
            this.StimStatusText = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.panel_Network = new System.Windows.Forms.Panel();
            this.TabletBattery_circularProgressBar = new CircularProgressBar.CircularProgressBar();
            this.TabBatteryText = new System.Windows.Forms.Label();
            this.NetworkStatusText = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.CTMBatteryText = new System.Windows.Forms.Label();
            this.CTMBattery_circularProgressBar = new CircularProgressBar.CircularProgressBar();
            this.panel_CTMStatus = new System.Windows.Forms.Panel();
            this.CTMStatusText = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.panel_INSStatus = new System.Windows.Forms.Panel();
            this.INSStatusText = new System.Windows.Forms.Label();
            this.INSBatteryText = new System.Windows.Forms.Label();
            this.INSBattery_circularProgressBar = new CircularProgressBar.CircularProgressBar();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.stopStim = new System.Windows.Forms.Button();
            this.safeMode = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Mode_groupBox.SuspendLayout();
            this.Annotations_groupBox.SuspendLayout();
            this.Indicators_groupBox.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Black", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(88)))), ((int)(((byte)(90)))));
            this.label1.Location = new System.Drawing.Point(27, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(419, 31);
            this.label1.TabIndex = 3;
            this.label1.Text = "RC+S Data Management Platform";
            // 
            // Mode
            // 
            this.Mode.AutoSize = true;
            this.Mode.Font = new System.Drawing.Font("Arial Black", 16F, System.Drawing.FontStyle.Bold);
            this.Mode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(88)))), ((int)(((byte)(90)))));
            this.Mode.Location = new System.Drawing.Point(27, 47);
            this.Mode.Name = "Mode";
            this.Mode.Size = new System.Drawing.Size(88, 31);
            this.Mode.TabIndex = 4;
            this.Mode.Text = "label2";
            // 
            // physicianMode
            // 
            this.physicianMode.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.physicianMode.Font = new System.Drawing.Font("Arial Black", 16F);
            this.physicianMode.Location = new System.Drawing.Point(41, 52);
            this.physicianMode.Name = "physicianMode";
            this.physicianMode.Size = new System.Drawing.Size(165, 70);
            this.physicianMode.TabIndex = 6;
            this.physicianMode.Text = "Physician";
            this.physicianMode.UseVisualStyleBackColor = true;
            this.physicianMode.Click += new System.EventHandler(this.PhysicianMode_Click);
            // 
            // CloseApp
            // 
            this.CloseApp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseApp.Font = new System.Drawing.Font("Arial Black", 16F);
            this.CloseApp.Location = new System.Drawing.Point(605, 12);
            this.CloseApp.Name = "CloseApp";
            this.CloseApp.Size = new System.Drawing.Size(275, 50);
            this.CloseApp.TabIndex = 7;
            this.CloseApp.Text = "Close Application";
            this.CloseApp.UseVisualStyleBackColor = true;
            this.CloseApp.Click += new System.EventHandler(this.CloseApp_Click);
            // 
            // Eventbutton
            // 
            this.Eventbutton.BackColor = System.Drawing.Color.Yellow;
            this.Eventbutton.Font = new System.Drawing.Font("Arial Black", 24F);
            this.Eventbutton.Location = new System.Drawing.Point(41, 109);
            this.Eventbutton.Name = "Eventbutton";
            this.Eventbutton.Size = new System.Drawing.Size(270, 150);
            this.Eventbutton.TabIndex = 8;
            this.Eventbutton.Text = "Event";
            this.Eventbutton.UseVisualStyleBackColor = false;
            this.Eventbutton.Click += new System.EventHandler(this.Eventbutton_Click);
            // 
            // Mode_groupBox
            // 
            this.Mode_groupBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Mode_groupBox.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.Mode_groupBox.Controls.Add(this.patientMode);
            this.Mode_groupBox.Controls.Add(this.physicianMode);
            this.Mode_groupBox.Font = new System.Drawing.Font("Arial Black", 24F, System.Drawing.FontStyle.Bold);
            this.Mode_groupBox.Location = new System.Drawing.Point(33, 583);
            this.Mode_groupBox.Name = "Mode_groupBox";
            this.Mode_groupBox.Size = new System.Drawing.Size(420, 135);
            this.Mode_groupBox.TabIndex = 11;
            this.Mode_groupBox.TabStop = false;
            this.Mode_groupBox.Text = "Mode";
            // 
            // patientMode
            // 
            this.patientMode.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.patientMode.Font = new System.Drawing.Font("Arial Black", 16F);
            this.patientMode.Location = new System.Drawing.Point(232, 52);
            this.patientMode.Name = "patientMode";
            this.patientMode.Size = new System.Drawing.Size(165, 70);
            this.patientMode.TabIndex = 6;
            this.patientMode.Text = "Patient";
            this.patientMode.UseVisualStyleBackColor = true;
            this.patientMode.Click += new System.EventHandler(this.PatientMode_Click);
            // 
            // Annotations_groupBox
            // 
            this.Annotations_groupBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Annotations_groupBox.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.Annotations_groupBox.Controls.Add(this.label_recTime);
            this.Annotations_groupBox.Controls.Add(this.Medbutton);
            this.Annotations_groupBox.Controls.Add(this.Eventbutton);
            this.Annotations_groupBox.Font = new System.Drawing.Font("Arial Black", 24F, System.Drawing.FontStyle.Bold);
            this.Annotations_groupBox.Location = new System.Drawing.Point(33, 117);
            this.Annotations_groupBox.Name = "Annotations_groupBox";
            this.Annotations_groupBox.Size = new System.Drawing.Size(352, 460);
            this.Annotations_groupBox.TabIndex = 12;
            this.Annotations_groupBox.TabStop = false;
            this.Annotations_groupBox.Text = "User Annotations";
            // 
            // label_recTime
            // 
            this.label_recTime.AutoSize = true;
            this.label_recTime.BackColor = System.Drawing.Color.Transparent;
            this.label_recTime.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_recTime.ForeColor = System.Drawing.Color.Red;
            this.label_recTime.Location = new System.Drawing.Point(37, 54);
            this.label_recTime.Name = "label_recTime";
            this.label_recTime.Size = new System.Drawing.Size(20, 22);
            this.label_recTime.TabIndex = 11;
            this.label_recTime.Text = "  ";
            // 
            // Medbutton
            // 
            this.Medbutton.BackColor = System.Drawing.Color.Green;
            this.Medbutton.Font = new System.Drawing.Font("Arial Black", 24F);
            this.Medbutton.Location = new System.Drawing.Point(41, 278);
            this.Medbutton.Name = "Medbutton";
            this.Medbutton.Size = new System.Drawing.Size(270, 150);
            this.Medbutton.TabIndex = 10;
            this.Medbutton.Text = "Medication";
            this.Medbutton.UseVisualStyleBackColor = false;
            this.Medbutton.Click += new System.EventHandler(this.Medbutton_Click);
            // 
            // Indicators_groupBox
            // 
            this.Indicators_groupBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Indicators_groupBox.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.Indicators_groupBox.Controls.Add(this.groupBox9);
            this.Indicators_groupBox.Controls.Add(this.groupBox8);
            this.Indicators_groupBox.Controls.Add(this.groupBox6);
            this.Indicators_groupBox.Controls.Add(this.groupBox7);
            this.Indicators_groupBox.Controls.Add(this.groupBox5);
            this.Indicators_groupBox.Controls.Add(this.groupBox4);
            this.Indicators_groupBox.Controls.Add(this.groupBox3);
            this.Indicators_groupBox.Font = new System.Drawing.Font("Arial Black", 24F, System.Drawing.FontStyle.Bold);
            this.Indicators_groupBox.Location = new System.Drawing.Point(391, 117);
            this.Indicators_groupBox.Name = "Indicators_groupBox";
            this.Indicators_groupBox.Size = new System.Drawing.Size(489, 460);
            this.Indicators_groupBox.TabIndex = 13;
            this.Indicators_groupBox.TabStop = false;
            this.Indicators_groupBox.Text = "Status";
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.INSIDText);
            this.groupBox9.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold);
            this.groupBox9.Location = new System.Drawing.Point(39, 54);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(160, 70);
            this.groupBox9.TabIndex = 58;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Device ID";
            // 
            // INSIDText
            // 
            this.INSIDText.AutoSize = true;
            this.INSIDText.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.INSIDText.Location = new System.Drawing.Point(18, 35);
            this.INSIDText.Name = "INSIDText";
            this.INSIDText.Size = new System.Drawing.Size(34, 18);
            this.INSIDText.TabIndex = 41;
            this.INSIDText.Text = "N\\A";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.panel_Privacy);
            this.groupBox8.Controls.Add(this.privacyStatusText);
            this.groupBox8.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold);
            this.groupBox8.Location = new System.Drawing.Point(294, 54);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(160, 70);
            this.groupBox8.TabIndex = 57;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Privacy Mode";
            // 
            // panel_Privacy
            // 
            this.panel_Privacy.BackColor = System.Drawing.Color.Silver;
            this.panel_Privacy.Location = new System.Drawing.Point(49, 31);
            this.panel_Privacy.Name = "panel_Privacy";
            this.panel_Privacy.Size = new System.Drawing.Size(30, 30);
            this.panel_Privacy.TabIndex = 47;
            // 
            // privacyStatusText
            // 
            this.privacyStatusText.AutoSize = true;
            this.privacyStatusText.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.privacyStatusText.Location = new System.Drawing.Point(85, 39);
            this.privacyStatusText.Name = "privacyStatusText";
            this.privacyStatusText.Size = new System.Drawing.Size(23, 14);
            this.privacyStatusText.TabIndex = 52;
            this.privacyStatusText.Text = "Off";
            this.privacyStatusText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.DataStatusText);
            this.groupBox6.Controls.Add(this.panel_DataStatus);
            this.groupBox6.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold);
            this.groupBox6.Location = new System.Drawing.Point(39, 358);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(200, 70);
            this.groupBox6.TabIndex = 55;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "EEG Data";
            // 
            // DataStatusText
            // 
            this.DataStatusText.AutoSize = true;
            this.DataStatusText.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.DataStatusText.Location = new System.Drawing.Point(82, 39);
            this.DataStatusText.Name = "DataStatusText";
            this.DataStatusText.Size = new System.Drawing.Size(23, 14);
            this.DataStatusText.TabIndex = 38;
            this.DataStatusText.Text = "Off";
            this.DataStatusText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel_DataStatus
            // 
            this.panel_DataStatus.BackColor = System.Drawing.Color.Silver;
            this.panel_DataStatus.Location = new System.Drawing.Point(46, 31);
            this.panel_DataStatus.Name = "panel_DataStatus";
            this.panel_DataStatus.Size = new System.Drawing.Size(30, 30);
            this.panel_DataStatus.TabIndex = 50;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.panel_StimStatus);
            this.groupBox7.Controls.Add(this.StimStatusText);
            this.groupBox7.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold);
            this.groupBox7.Location = new System.Drawing.Point(254, 358);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(200, 70);
            this.groupBox7.TabIndex = 56;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Stimulation";
            // 
            // panel_StimStatus
            // 
            this.panel_StimStatus.BackColor = System.Drawing.Color.Silver;
            this.panel_StimStatus.Location = new System.Drawing.Point(53, 31);
            this.panel_StimStatus.Name = "panel_StimStatus";
            this.panel_StimStatus.Size = new System.Drawing.Size(30, 30);
            this.panel_StimStatus.TabIndex = 53;
            // 
            // StimStatusText
            // 
            this.StimStatusText.AutoSize = true;
            this.StimStatusText.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.StimStatusText.Location = new System.Drawing.Point(89, 39);
            this.StimStatusText.Name = "StimStatusText";
            this.StimStatusText.Size = new System.Drawing.Size(23, 14);
            this.StimStatusText.TabIndex = 52;
            this.StimStatusText.Text = "Off";
            this.StimStatusText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.panel_Network);
            this.groupBox5.Controls.Add(this.TabletBattery_circularProgressBar);
            this.groupBox5.Controls.Add(this.TabBatteryText);
            this.groupBox5.Controls.Add(this.NetworkStatusText);
            this.groupBox5.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.Location = new System.Drawing.Point(344, 138);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(110, 200);
            this.groupBox5.TabIndex = 55;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Tablet";
            // 
            // panel_Network
            // 
            this.panel_Network.BackColor = System.Drawing.Color.Silver;
            this.panel_Network.Location = new System.Drawing.Point(36, 31);
            this.panel_Network.Name = "panel_Network";
            this.panel_Network.Size = new System.Drawing.Size(30, 30);
            this.panel_Network.TabIndex = 46;
            // 
            // TabletBattery_circularProgressBar
            // 
            this.TabletBattery_circularProgressBar.AnimationFunction = WinFormAnimation.KnownAnimationFunctions.Liner;
            this.TabletBattery_circularProgressBar.AnimationSpeed = 500;
            this.TabletBattery_circularProgressBar.BackColor = System.Drawing.Color.Transparent;
            this.TabletBattery_circularProgressBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Bold);
            this.TabletBattery_circularProgressBar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.TabletBattery_circularProgressBar.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.TabletBattery_circularProgressBar.InnerMargin = 2;
            this.TabletBattery_circularProgressBar.InnerWidth = -1;
            this.TabletBattery_circularProgressBar.Location = new System.Drawing.Point(15, 100);
            this.TabletBattery_circularProgressBar.MarqueeAnimationSpeed = 2000;
            this.TabletBattery_circularProgressBar.Name = "TabletBattery_circularProgressBar";
            this.TabletBattery_circularProgressBar.OuterColor = System.Drawing.Color.Firebrick;
            this.TabletBattery_circularProgressBar.OuterMargin = -25;
            this.TabletBattery_circularProgressBar.OuterWidth = 26;
            this.TabletBattery_circularProgressBar.ProgressColor = System.Drawing.Color.MediumBlue;
            this.TabletBattery_circularProgressBar.ProgressWidth = 25;
            this.TabletBattery_circularProgressBar.SecondaryFont = new System.Drawing.Font("Microsoft Sans Serif", 36F);
            this.TabletBattery_circularProgressBar.Size = new System.Drawing.Size(75, 75);
            this.TabletBattery_circularProgressBar.StartAngle = 270;
            this.TabletBattery_circularProgressBar.SubscriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.TabletBattery_circularProgressBar.SubscriptMargin = new System.Windows.Forms.Padding(10, -35, 0, 0);
            this.TabletBattery_circularProgressBar.SubscriptText = ".23";
            this.TabletBattery_circularProgressBar.SuperscriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.TabletBattery_circularProgressBar.SuperscriptMargin = new System.Windows.Forms.Padding(10, 35, 0, 0);
            this.TabletBattery_circularProgressBar.SuperscriptText = "°C";
            this.TabletBattery_circularProgressBar.TabIndex = 28;
            this.TabletBattery_circularProgressBar.TextMargin = new System.Windows.Forms.Padding(8, 8, 0, 0);
            this.TabletBattery_circularProgressBar.Value = 68;
            // 
            // TabBatteryText
            // 
            this.TabBatteryText.AutoSize = true;
            this.TabBatteryText.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.TabBatteryText.Location = new System.Drawing.Point(39, 176);
            this.TabBatteryText.Name = "TabBatteryText";
            this.TabBatteryText.Size = new System.Drawing.Size(25, 14);
            this.TabBatteryText.TabIndex = 35;
            this.TabBatteryText.Text = "N\\A";
            this.TabBatteryText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NetworkStatusText
            // 
            this.NetworkStatusText.AutoEllipsis = true;
            this.NetworkStatusText.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.NetworkStatusText.ForeColor = System.Drawing.SystemColors.ControlText;
            this.NetworkStatusText.Location = new System.Drawing.Point(9, 66);
            this.NetworkStatusText.Name = "NetworkStatusText";
            this.NetworkStatusText.Size = new System.Drawing.Size(95, 31);
            this.NetworkStatusText.TabIndex = 45;
            this.NetworkStatusText.Text = "Network\r\nStatus";
            this.NetworkStatusText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.CTMBatteryText);
            this.groupBox4.Controls.Add(this.CTMBattery_circularProgressBar);
            this.groupBox4.Controls.Add(this.panel_CTMStatus);
            this.groupBox4.Controls.Add(this.CTMStatusText);
            this.groupBox4.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(192, 138);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(110, 200);
            this.groupBox4.TabIndex = 54;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "CTM";
            // 
            // CTMBatteryText
            // 
            this.CTMBatteryText.AutoSize = true;
            this.CTMBatteryText.Font = new System.Drawing.Font("Arial Black", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CTMBatteryText.Location = new System.Drawing.Point(25, 130);
            this.CTMBatteryText.Name = "CTMBatteryText";
            this.CTMBatteryText.Size = new System.Drawing.Size(53, 30);
            this.CTMBatteryText.TabIndex = 34;
            this.CTMBatteryText.Text = "N\\A";
            this.CTMBatteryText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CTMBattery_circularProgressBar
            // 
            this.CTMBattery_circularProgressBar.AnimationFunction = WinFormAnimation.KnownAnimationFunctions.Liner;
            this.CTMBattery_circularProgressBar.AnimationSpeed = 500;
            this.CTMBattery_circularProgressBar.BackColor = System.Drawing.Color.Transparent;
            this.CTMBattery_circularProgressBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Bold);
            this.CTMBattery_circularProgressBar.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.CTMBattery_circularProgressBar.InnerColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.CTMBattery_circularProgressBar.InnerMargin = 2;
            this.CTMBattery_circularProgressBar.InnerWidth = -1;
            this.CTMBattery_circularProgressBar.Location = new System.Drawing.Point(15, 98);
            this.CTMBattery_circularProgressBar.MarqueeAnimationSpeed = 2000;
            this.CTMBattery_circularProgressBar.Name = "CTMBattery_circularProgressBar";
            this.CTMBattery_circularProgressBar.OuterColor = System.Drawing.Color.Firebrick;
            this.CTMBattery_circularProgressBar.OuterMargin = -25;
            this.CTMBattery_circularProgressBar.OuterWidth = 26;
            this.CTMBattery_circularProgressBar.ProgressColor = System.Drawing.Color.MediumBlue;
            this.CTMBattery_circularProgressBar.ProgressWidth = 25;
            this.CTMBattery_circularProgressBar.SecondaryFont = new System.Drawing.Font("Microsoft Sans Serif", 36F);
            this.CTMBattery_circularProgressBar.Size = new System.Drawing.Size(75, 75);
            this.CTMBattery_circularProgressBar.StartAngle = 270;
            this.CTMBattery_circularProgressBar.SubscriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.CTMBattery_circularProgressBar.SubscriptMargin = new System.Windows.Forms.Padding(10, -35, 0, 0);
            this.CTMBattery_circularProgressBar.SubscriptText = ".23";
            this.CTMBattery_circularProgressBar.SuperscriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.CTMBattery_circularProgressBar.SuperscriptMargin = new System.Windows.Forms.Padding(10, 35, 0, 0);
            this.CTMBattery_circularProgressBar.SuperscriptText = "°C";
            this.CTMBattery_circularProgressBar.TabIndex = 27;
            this.CTMBattery_circularProgressBar.TextMargin = new System.Windows.Forms.Padding(8, 8, 0, 0);
            this.CTMBattery_circularProgressBar.Value = 68;
            this.CTMBattery_circularProgressBar.Visible = false;
            // 
            // panel_CTMStatus
            // 
            this.panel_CTMStatus.BackColor = System.Drawing.Color.Silver;
            this.panel_CTMStatus.Location = new System.Drawing.Point(40, 31);
            this.panel_CTMStatus.Name = "panel_CTMStatus";
            this.panel_CTMStatus.Size = new System.Drawing.Size(30, 30);
            this.panel_CTMStatus.TabIndex = 49;
            // 
            // CTMStatusText
            // 
            this.CTMStatusText.AutoEllipsis = true;
            this.CTMStatusText.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.CTMStatusText.Location = new System.Drawing.Point(12, 64);
            this.CTMStatusText.Name = "CTMStatusText";
            this.CTMStatusText.Size = new System.Drawing.Size(88, 31);
            this.CTMStatusText.TabIndex = 37;
            this.CTMStatusText.Text = "Not\r\nConnected";
            this.CTMStatusText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.panel_INSStatus);
            this.groupBox3.Controls.Add(this.INSStatusText);
            this.groupBox3.Controls.Add(this.INSBatteryText);
            this.groupBox3.Controls.Add(this.INSBattery_circularProgressBar);
            this.groupBox3.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(39, 138);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(110, 200);
            this.groupBox3.TabIndex = 53;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "INS";
            // 
            // panel_INSStatus
            // 
            this.panel_INSStatus.BackColor = System.Drawing.Color.Silver;
            this.panel_INSStatus.Location = new System.Drawing.Point(41, 31);
            this.panel_INSStatus.Name = "panel_INSStatus";
            this.panel_INSStatus.Size = new System.Drawing.Size(30, 30);
            this.panel_INSStatus.TabIndex = 53;
            // 
            // INSStatusText
            // 
            this.INSStatusText.AutoEllipsis = true;
            this.INSStatusText.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.INSStatusText.Location = new System.Drawing.Point(11, 64);
            this.INSStatusText.Name = "INSStatusText";
            this.INSStatusText.Size = new System.Drawing.Size(88, 31);
            this.INSStatusText.TabIndex = 52;
            this.INSStatusText.Text = "Not\r\nConnected";
            this.INSStatusText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // INSBatteryText
            // 
            this.INSBatteryText.AutoSize = true;
            this.INSBatteryText.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.INSBatteryText.Location = new System.Drawing.Point(38, 176);
            this.INSBatteryText.Name = "INSBatteryText";
            this.INSBatteryText.Size = new System.Drawing.Size(25, 14);
            this.INSBatteryText.TabIndex = 51;
            this.INSBatteryText.Text = "N\\A";
            this.INSBatteryText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.INSBattery_circularProgressBar.Location = new System.Drawing.Point(14, 98);
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
            this.INSBattery_circularProgressBar.TabIndex = 50;
            this.INSBattery_circularProgressBar.TextMargin = new System.Windows.Forms.Padding(8, 8, 0, 0);
            this.INSBattery_circularProgressBar.Value = 68;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBox1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.groupBox1.Controls.Add(this.stopStim);
            this.groupBox1.Controls.Add(this.safeMode);
            this.groupBox1.Font = new System.Drawing.Font("Arial Black", 24F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(451, 583);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(429, 135);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            // 
            // stopStim
            // 
            this.stopStim.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.stopStim.Font = new System.Drawing.Font("Arial Black", 16F);
            this.stopStim.Location = new System.Drawing.Point(222, 52);
            this.stopStim.Name = "stopStim";
            this.stopStim.Size = new System.Drawing.Size(165, 70);
            this.stopStim.TabIndex = 6;
            this.stopStim.Text = "Stop Stimulation";
            this.stopStim.UseVisualStyleBackColor = true;
            this.stopStim.Click += new System.EventHandler(this.stopStim_Click);
            // 
            // safeMode
            // 
            this.safeMode.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.safeMode.Font = new System.Drawing.Font("Arial Black", 16F);
            this.safeMode.Location = new System.Drawing.Point(35, 52);
            this.safeMode.Name = "safeMode";
            this.safeMode.Size = new System.Drawing.Size(165, 70);
            this.safeMode.TabIndex = 6;
            this.safeMode.Text = "Safe Mode";
            this.safeMode.UseVisualStyleBackColor = true;
            this.safeMode.Click += new System.EventHandler(this.safeMode_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.groupBox2.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Location = new System.Drawing.Point(30, 726);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(850, 121);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            // 
            // textBox1
            // 
            this.textBox1.AcceptsReturn = true;
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Font = new System.Drawing.Font("Arial Black", 13F);
            this.textBox1.ForeColor = System.Drawing.Color.Red;
            this.textBox1.Location = new System.Drawing.Point(3, 16);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(844, 102);
            this.textBox1.TabIndex = 19;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(19, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 14);
            this.label2.TabIndex = 60;
            this.label2.Text = "Battery Level";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(934, 863);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.Mode_groupBox);
            this.Controls.Add(this.Indicators_groupBox);
            this.Controls.Add(this.Annotations_groupBox);
            this.Controls.Add(this.CloseApp);
            this.Controls.Add(this.Mode);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Home";
            this.ShowInTaskbar = false;
            this.Text = "Home";
            this.Activated += new System.EventHandler(this.Home_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Home_FormClosing);
            this.Load += new System.EventHandler(this.Home_Load);
            this.Shown += new System.EventHandler(this.Home_Load);
            this.Enter += new System.EventHandler(this.Home_Load);
            this.Mode_groupBox.ResumeLayout(false);
            this.Annotations_groupBox.ResumeLayout(false);
            this.Annotations_groupBox.PerformLayout();
            this.Indicators_groupBox.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        protected internal System.Windows.Forms.Label label1;
        protected internal System.Windows.Forms.Label Mode;
        protected internal System.Windows.Forms.Button physicianMode;
        protected internal System.Windows.Forms.Button CloseApp;
        protected internal System.Windows.Forms.Button Eventbutton;
        protected internal System.Windows.Forms.GroupBox Mode_groupBox;
        protected internal System.Windows.Forms.Button patientMode;
        protected internal System.Windows.Forms.GroupBox Annotations_groupBox;
        protected internal System.Windows.Forms.GroupBox Indicators_groupBox;
        protected internal System.Windows.Forms.Button Medbutton;
        protected internal System.Windows.Forms.GroupBox groupBox1;
        protected internal System.Windows.Forms.Button stopStim;
        protected internal System.Windows.Forms.Button safeMode;
        protected internal CircularProgressBar.CircularProgressBar TabletBattery_circularProgressBar;
        protected internal CircularProgressBar.CircularProgressBar CTMBattery_circularProgressBar;
        protected internal System.Windows.Forms.Label CTMBatteryText;
        protected internal System.Windows.Forms.Label TabBatteryText;
        protected internal System.Windows.Forms.Label DataStatusText;
        protected internal System.Windows.Forms.Label CTMStatusText;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox1;
        protected internal System.Windows.Forms.Label INSIDText;
        protected internal System.Windows.Forms.Label NetworkStatusText;
        private System.Windows.Forms.Panel panel_Privacy;
        private System.Windows.Forms.Panel panel_Network;
        private System.Windows.Forms.Panel panel_DataStatus;
        private System.Windows.Forms.Panel panel_CTMStatus;
        protected internal System.Windows.Forms.Label privacyStatusText;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Panel panel_INSStatus;
        protected internal System.Windows.Forms.Label INSStatusText;
        protected internal System.Windows.Forms.Label INSBatteryText;
        protected internal CircularProgressBar.CircularProgressBar INSBattery_circularProgressBar;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Panel panel_StimStatus;
        protected internal System.Windows.Forms.Label StimStatusText;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Label label_recTime;
        protected internal System.Windows.Forms.Label label2;
    }
}