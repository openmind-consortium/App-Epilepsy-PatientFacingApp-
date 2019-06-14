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
    partial class ConfigureLDAForm
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
            this.lbl_LD_ConfigStatus = new System.Windows.Forms.Label();
            this.numericUpDownTerminationDuration = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownOnsetDuration = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownInput2HighBand2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownInput2LowBand2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownInput2HighBand1 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownInput2LowBand1 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownInput1HighBand2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownInput1LowBand2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownInput1HighBand1 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownInput1LowBand1 = new System.Windows.Forms.NumericUpDown();
            this.chkBox_SaveFftData = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.numeric_UpDown_Threshold = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.cmb_Input2_channels = new System.Windows.Forms.ComboBox();
            this.cmb_Input1_channels = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_ChannelEmbedded = new System.Windows.Forms.Label();
            this.lbl_Input2 = new System.Windows.Forms.Label();
            this.lbl_Input1 = new System.Windows.Forms.Label();
            this.btn_ConfigurationLDA = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTerminationDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownOnsetDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInput2HighBand2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInput2LowBand2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInput2HighBand1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInput2LowBand1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInput1HighBand2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInput1LowBand2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInput1HighBand1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInput1LowBand1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_UpDown_Threshold)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbl_LD_ConfigStatus);
            this.panel1.Controls.Add(this.numericUpDownTerminationDuration);
            this.panel1.Controls.Add(this.numericUpDownOnsetDuration);
            this.panel1.Controls.Add(this.numericUpDownInput2HighBand2);
            this.panel1.Controls.Add(this.numericUpDownInput2LowBand2);
            this.panel1.Controls.Add(this.numericUpDownInput2HighBand1);
            this.panel1.Controls.Add(this.numericUpDownInput2LowBand1);
            this.panel1.Controls.Add(this.numericUpDownInput1HighBand2);
            this.panel1.Controls.Add(this.numericUpDownInput1LowBand2);
            this.panel1.Controls.Add(this.numericUpDownInput1HighBand1);
            this.panel1.Controls.Add(this.numericUpDownInput1LowBand1);
            this.panel1.Controls.Add(this.chkBox_SaveFftData);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.numeric_UpDown_Threshold);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.cmb_Input2_channels);
            this.panel1.Controls.Add(this.cmb_Input1_channels);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.lbl_ChannelEmbedded);
            this.panel1.Controls.Add(this.lbl_Input2);
            this.panel1.Controls.Add(this.lbl_Input1);
            this.panel1.Controls.Add(this.btn_ConfigurationLDA);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(809, 612);
            this.panel1.TabIndex = 1;
            // 
            // lbl_LD_ConfigStatus
            // 
            this.lbl_LD_ConfigStatus.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_LD_ConfigStatus.AutoSize = true;
            this.lbl_LD_ConfigStatus.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.lbl_LD_ConfigStatus.Location = new System.Drawing.Point(263, 497);
            this.lbl_LD_ConfigStatus.Name = "lbl_LD_ConfigStatus";
            this.lbl_LD_ConfigStatus.Size = new System.Drawing.Size(58, 19);
            this.lbl_LD_ConfigStatus.TabIndex = 42;
            this.lbl_LD_ConfigStatus.Text = "Status";
            // 
            // numericUpDownTerminationDuration
            // 
            this.numericUpDownTerminationDuration.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericUpDownTerminationDuration.BackColor = System.Drawing.SystemColors.HighlightText;
            this.numericUpDownTerminationDuration.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownTerminationDuration.Location = new System.Drawing.Point(562, 317);
            this.numericUpDownTerminationDuration.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownTerminationDuration.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownTerminationDuration.Name = "numericUpDownTerminationDuration";
            this.numericUpDownTerminationDuration.ReadOnly = true;
            this.numericUpDownTerminationDuration.Size = new System.Drawing.Size(62, 22);
            this.numericUpDownTerminationDuration.TabIndex = 40;
            this.numericUpDownTerminationDuration.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownTerminationDuration.Validating += new System.ComponentModel.CancelEventHandler(this.numericUpDown_Validating);
            // 
            // numericUpDownOnsetDuration
            // 
            this.numericUpDownOnsetDuration.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericUpDownOnsetDuration.BackColor = System.Drawing.SystemColors.HighlightText;
            this.numericUpDownOnsetDuration.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownOnsetDuration.Location = new System.Drawing.Point(267, 315);
            this.numericUpDownOnsetDuration.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownOnsetDuration.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownOnsetDuration.Name = "numericUpDownOnsetDuration";
            this.numericUpDownOnsetDuration.ReadOnly = true;
            this.numericUpDownOnsetDuration.Size = new System.Drawing.Size(62, 22);
            this.numericUpDownOnsetDuration.TabIndex = 39;
            this.numericUpDownOnsetDuration.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numericUpDownOnsetDuration.Validating += new System.ComponentModel.CancelEventHandler(this.numericUpDown_Validating);
            // 
            // numericUpDownInput2HighBand2
            // 
            this.numericUpDownInput2HighBand2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericUpDownInput2HighBand2.BackColor = System.Drawing.SystemColors.HighlightText;
            this.numericUpDownInput2HighBand2.DecimalPlaces = 3;
            this.numericUpDownInput2HighBand2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownInput2HighBand2.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDownInput2HighBand2.Location = new System.Drawing.Point(562, 245);
            this.numericUpDownInput2HighBand2.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.numericUpDownInput2HighBand2.Name = "numericUpDownInput2HighBand2";
            this.numericUpDownInput2HighBand2.ReadOnly = true;
            this.numericUpDownInput2HighBand2.Size = new System.Drawing.Size(62, 22);
            this.numericUpDownInput2HighBand2.TabIndex = 38;
            this.numericUpDownInput2HighBand2.Value = new decimal(new int[] {
            11,
            0,
            0,
            0});
            this.numericUpDownInput2HighBand2.Validating += new System.ComponentModel.CancelEventHandler(this.numericUpDown_Validating);
            // 
            // numericUpDownInput2LowBand2
            // 
            this.numericUpDownInput2LowBand2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericUpDownInput2LowBand2.BackColor = System.Drawing.SystemColors.HighlightText;
            this.numericUpDownInput2LowBand2.DecimalPlaces = 3;
            this.numericUpDownInput2LowBand2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownInput2LowBand2.Location = new System.Drawing.Point(472, 245);
            this.numericUpDownInput2LowBand2.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.numericUpDownInput2LowBand2.Name = "numericUpDownInput2LowBand2";
            this.numericUpDownInput2LowBand2.ReadOnly = true;
            this.numericUpDownInput2LowBand2.Size = new System.Drawing.Size(62, 22);
            this.numericUpDownInput2LowBand2.TabIndex = 37;
            this.numericUpDownInput2LowBand2.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.numericUpDownInput2LowBand2.Validating += new System.ComponentModel.CancelEventHandler(this.numericUpDown_Validating);
            // 
            // numericUpDownInput2HighBand1
            // 
            this.numericUpDownInput2HighBand1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericUpDownInput2HighBand1.BackColor = System.Drawing.SystemColors.HighlightText;
            this.numericUpDownInput2HighBand1.DecimalPlaces = 3;
            this.numericUpDownInput2HighBand1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownInput2HighBand1.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDownInput2HighBand1.Location = new System.Drawing.Point(562, 203);
            this.numericUpDownInput2HighBand1.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.numericUpDownInput2HighBand1.Name = "numericUpDownInput2HighBand1";
            this.numericUpDownInput2HighBand1.ReadOnly = true;
            this.numericUpDownInput2HighBand1.Size = new System.Drawing.Size(62, 22);
            this.numericUpDownInput2HighBand1.TabIndex = 36;
            this.numericUpDownInput2HighBand1.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.numericUpDownInput2HighBand1.Validating += new System.ComponentModel.CancelEventHandler(this.numericUpDown_Validating);
            // 
            // numericUpDownInput2LowBand1
            // 
            this.numericUpDownInput2LowBand1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericUpDownInput2LowBand1.BackColor = System.Drawing.SystemColors.HighlightText;
            this.numericUpDownInput2LowBand1.DecimalPlaces = 3;
            this.numericUpDownInput2LowBand1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownInput2LowBand1.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDownInput2LowBand1.Location = new System.Drawing.Point(472, 203);
            this.numericUpDownInput2LowBand1.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.numericUpDownInput2LowBand1.Name = "numericUpDownInput2LowBand1";
            this.numericUpDownInput2LowBand1.ReadOnly = true;
            this.numericUpDownInput2LowBand1.Size = new System.Drawing.Size(62, 22);
            this.numericUpDownInput2LowBand1.TabIndex = 35;
            this.numericUpDownInput2LowBand1.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numericUpDownInput2LowBand1.Validating += new System.ComponentModel.CancelEventHandler(this.numericUpDown_Validating);
            // 
            // numericUpDownInput1HighBand2
            // 
            this.numericUpDownInput1HighBand2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericUpDownInput1HighBand2.BackColor = System.Drawing.SystemColors.HighlightText;
            this.numericUpDownInput1HighBand2.DecimalPlaces = 3;
            this.numericUpDownInput1HighBand2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownInput1HighBand2.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDownInput1HighBand2.Location = new System.Drawing.Point(562, 130);
            this.numericUpDownInput1HighBand2.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.numericUpDownInput1HighBand2.Name = "numericUpDownInput1HighBand2";
            this.numericUpDownInput1HighBand2.ReadOnly = true;
            this.numericUpDownInput1HighBand2.Size = new System.Drawing.Size(62, 22);
            this.numericUpDownInput1HighBand2.TabIndex = 34;
            this.numericUpDownInput1HighBand2.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownInput1HighBand2.Validating += new System.ComponentModel.CancelEventHandler(this.numericUpDown_Validating);
            // 
            // numericUpDownInput1LowBand2
            // 
            this.numericUpDownInput1LowBand2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericUpDownInput1LowBand2.BackColor = System.Drawing.SystemColors.HighlightText;
            this.numericUpDownInput1LowBand2.DecimalPlaces = 3;
            this.numericUpDownInput1LowBand2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownInput1LowBand2.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDownInput1LowBand2.Location = new System.Drawing.Point(472, 130);
            this.numericUpDownInput1LowBand2.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.numericUpDownInput1LowBand2.Name = "numericUpDownInput1LowBand2";
            this.numericUpDownInput1LowBand2.ReadOnly = true;
            this.numericUpDownInput1LowBand2.Size = new System.Drawing.Size(62, 22);
            this.numericUpDownInput1LowBand2.TabIndex = 33;
            this.numericUpDownInput1LowBand2.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.numericUpDownInput1LowBand2.Validating += new System.ComponentModel.CancelEventHandler(this.numericUpDown_Validating);
            // 
            // numericUpDownInput1HighBand1
            // 
            this.numericUpDownInput1HighBand1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericUpDownInput1HighBand1.BackColor = System.Drawing.SystemColors.HighlightText;
            this.numericUpDownInput1HighBand1.DecimalPlaces = 3;
            this.numericUpDownInput1HighBand1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownInput1HighBand1.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDownInput1HighBand1.Location = new System.Drawing.Point(562, 81);
            this.numericUpDownInput1HighBand1.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.numericUpDownInput1HighBand1.Name = "numericUpDownInput1HighBand1";
            this.numericUpDownInput1HighBand1.ReadOnly = true;
            this.numericUpDownInput1HighBand1.Size = new System.Drawing.Size(62, 22);
            this.numericUpDownInput1HighBand1.TabIndex = 32;
            this.numericUpDownInput1HighBand1.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownInput1HighBand1.Validating += new System.ComponentModel.CancelEventHandler(this.numericUpDown_Validating);
            // 
            // numericUpDownInput1LowBand1
            // 
            this.numericUpDownInput1LowBand1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numericUpDownInput1LowBand1.BackColor = System.Drawing.SystemColors.HighlightText;
            this.numericUpDownInput1LowBand1.DecimalPlaces = 3;
            this.numericUpDownInput1LowBand1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownInput1LowBand1.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDownInput1LowBand1.Location = new System.Drawing.Point(472, 81);
            this.numericUpDownInput1LowBand1.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.numericUpDownInput1LowBand1.Name = "numericUpDownInput1LowBand1";
            this.numericUpDownInput1LowBand1.ReadOnly = true;
            this.numericUpDownInput1LowBand1.Size = new System.Drawing.Size(62, 22);
            this.numericUpDownInput1LowBand1.TabIndex = 31;
            this.numericUpDownInput1LowBand1.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDownInput1LowBand1.Validating += new System.ComponentModel.CancelEventHandler(this.numericUpDown_Validating);
            // 
            // chkBox_SaveFftData
            // 
            this.chkBox_SaveFftData.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkBox_SaveFftData.AutoSize = true;
            this.chkBox_SaveFftData.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.chkBox_SaveFftData.Location = new System.Drawing.Point(484, 360);
            this.chkBox_SaveFftData.Name = "chkBox_SaveFftData";
            this.chkBox_SaveFftData.Size = new System.Drawing.Size(139, 23);
            this.chkBox_SaveFftData.TabIndex = 30;
            this.chkBox_SaveFftData.Text = "Save FFT Data";
            this.chkBox_SaveFftData.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.label9.Location = new System.Drawing.Point(385, 252);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(63, 19);
            this.label9.TabIndex = 29;
            this.label9.Text = "Band 2";
            // 
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.label10.Location = new System.Drawing.Point(385, 210);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(63, 19);
            this.label10.TabIndex = 28;
            this.label10.Text = "Band 1";
            // 
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.label8.Location = new System.Drawing.Point(385, 137);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 19);
            this.label8.TabIndex = 27;
            this.label8.Text = "Band 2";
            // 
            // label7
            // 
            this.label7.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(385, 88);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 19);
            this.label7.TabIndex = 26;
            this.label7.Text = "Band 1";
            // 
            // numeric_UpDown_Threshold
            // 
            this.numeric_UpDown_Threshold.AllowDrop = true;
            this.numeric_UpDown_Threshold.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numeric_UpDown_Threshold.BackColor = System.Drawing.SystemColors.HighlightText;
            this.numeric_UpDown_Threshold.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numeric_UpDown_Threshold.Location = new System.Drawing.Point(267, 360);
            this.numeric_UpDown_Threshold.Maximum = new decimal(new int[] {
            -1294967296,
            0,
            0,
            0});
            this.numeric_UpDown_Threshold.Name = "numeric_UpDown_Threshold";
            this.numeric_UpDown_Threshold.ReadOnly = true;
            this.numeric_UpDown_Threshold.Size = new System.Drawing.Size(102, 22);
            this.numeric_UpDown_Threshold.TabIndex = 25;
            this.numeric_UpDown_Threshold.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numeric_UpDown_Threshold.Validating += new System.ComponentModel.CancelEventHandler(this.numericUpDown_Validating);
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(92, 364);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(123, 19);
            this.label6.TabIndex = 22;
            this.label6.Text = "LDA Threshold";
            // 
            // cmb_Input2_channels
            // 
            this.cmb_Input2_channels.AllowDrop = true;
            this.cmb_Input2_channels.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cmb_Input2_channels.AutoCompleteCustomSource.AddRange(new string[] {
            "E0-E1",
            "E2-E3",
            "E8-E9",
            "E10-E11"});
            this.cmb_Input2_channels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Input2_channels.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.cmb_Input2_channels.FormattingEnabled = true;
            this.cmb_Input2_channels.Location = new System.Drawing.Point(207, 212);
            this.cmb_Input2_channels.Name = "cmb_Input2_channels";
            this.cmb_Input2_channels.Size = new System.Drawing.Size(101, 24);
            this.cmb_Input2_channels.TabIndex = 19;
            // 
            // cmb_Input1_channels
            // 
            this.cmb_Input1_channels.AllowDrop = true;
            this.cmb_Input1_channels.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cmb_Input1_channels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Input1_channels.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.cmb_Input1_channels.FormattingEnabled = true;
            this.cmb_Input1_channels.ItemHeight = 16;
            this.cmb_Input1_channels.Location = new System.Drawing.Point(207, 80);
            this.cmb_Input1_channels.Name = "cmb_Input1_channels";
            this.cmb_Input1_channels.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cmb_Input1_channels.Size = new System.Drawing.Size(101, 24);
            this.cmb_Input1_channels.TabIndex = 18;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(346, 317);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(210, 19);
            this.label5.TabIndex = 17;
            this.label5.Text = "Termination Duration (sec)";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(92, 315);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(165, 19);
            this.label4.TabIndex = 16;
            this.label4.Text = "Onset Duration (sec)";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(567, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 19);
            this.label3.TabIndex = 15;
            this.label3.Text = "High";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(485, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 19);
            this.label2.TabIndex = 14;
            this.label2.Text = "Low";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(453, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(171, 19);
            this.label1.TabIndex = 13;
            this.label1.Text = "Frequency Band (Hz)";
            // 
            // lbl_ChannelEmbedded
            // 
            this.lbl_ChannelEmbedded.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_ChannelEmbedded.AutoSize = true;
            this.lbl_ChannelEmbedded.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.lbl_ChannelEmbedded.Location = new System.Drawing.Point(216, 18);
            this.lbl_ChannelEmbedded.Name = "lbl_ChannelEmbedded";
            this.lbl_ChannelEmbedded.Size = new System.Drawing.Size(82, 19);
            this.lbl_ChannelEmbedded.TabIndex = 12;
            this.lbl_ChannelEmbedded.Text = "Channels";
            // 
            // lbl_Input2
            // 
            this.lbl_Input2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_Input2.AutoSize = true;
            this.lbl_Input2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.lbl_Input2.Location = new System.Drawing.Point(110, 213);
            this.lbl_Input2.Name = "lbl_Input2";
            this.lbl_Input2.Size = new System.Drawing.Size(61, 19);
            this.lbl_Input2.TabIndex = 5;
            this.lbl_Input2.Text = "Input 2";
            // 
            // lbl_Input1
            // 
            this.lbl_Input1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_Input1.AutoSize = true;
            this.lbl_Input1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.lbl_Input1.Location = new System.Drawing.Point(110, 81);
            this.lbl_Input1.Name = "lbl_Input1";
            this.lbl_Input1.Size = new System.Drawing.Size(61, 19);
            this.lbl_Input1.TabIndex = 4;
            this.lbl_Input1.Text = "Input 1";
            // 
            // btn_ConfigurationLDA
            // 
            this.btn_ConfigurationLDA.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btn_ConfigurationLDA.BackColor = System.Drawing.SystemColors.Control;
            this.btn_ConfigurationLDA.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.btn_ConfigurationLDA.Location = new System.Drawing.Point(267, 427);
            this.btn_ConfigurationLDA.Name = "btn_ConfigurationLDA";
            this.btn_ConfigurationLDA.Size = new System.Drawing.Size(199, 55);
            this.btn_ConfigurationLDA.TabIndex = 1;
            this.btn_ConfigurationLDA.Text = "Apply Settings";
            this.btn_ConfigurationLDA.UseVisualStyleBackColor = true;
            this.btn_ConfigurationLDA.Click += new System.EventHandler(this.btn_ConfigurationLDA_Click);
            // 
            // ConfigureLDAForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(809, 612);
            this.Controls.Add(this.panel1);
            this.Name = "ConfigureLDAForm";
            this.Text = "Embedded Detector";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTerminationDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownOnsetDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInput2HighBand2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInput2LowBand2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInput2HighBand1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInput2LowBand1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInput1HighBand2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInput1LowBand2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInput1HighBand1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInput1LowBand1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numeric_UpDown_Threshold)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbl_Input2;
        private System.Windows.Forms.Label lbl_Input1;
        protected internal System.Windows.Forms.Button btn_ConfigurationLDA;
        private System.Windows.Forms.Label lbl_ChannelEmbedded;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        protected internal System.Windows.Forms.ComboBox cmb_Input1_channels;
        protected internal System.Windows.Forms.ComboBox cmb_Input2_channels;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        protected internal System.Windows.Forms.NumericUpDown numericUpDownInput1LowBand1;
        protected internal System.Windows.Forms.NumericUpDown numericUpDownInput1HighBand1;
        protected internal System.Windows.Forms.NumericUpDown numeric_UpDown_Threshold;
        protected internal System.Windows.Forms.NumericUpDown numericUpDownInput2HighBand2;
        protected internal System.Windows.Forms.NumericUpDown numericUpDownInput2LowBand2;
        protected internal System.Windows.Forms.NumericUpDown numericUpDownInput2HighBand1;
        protected internal System.Windows.Forms.NumericUpDown numericUpDownInput2LowBand1;
        protected internal System.Windows.Forms.NumericUpDown numericUpDownInput1HighBand2;
        protected internal System.Windows.Forms.NumericUpDown numericUpDownInput1LowBand2;
        protected internal System.Windows.Forms.NumericUpDown numericUpDownOnsetDuration;
        protected internal System.Windows.Forms.NumericUpDown numericUpDownTerminationDuration;
        protected internal System.Windows.Forms.CheckBox chkBox_SaveFftData;
        private System.Windows.Forms.Label lbl_LD_ConfigStatus;
    }
}