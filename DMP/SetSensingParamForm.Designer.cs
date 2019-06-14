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
    partial class SetSensingParamForm
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
            this.label_settingsTime = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.samplingRateText = new System.Windows.Forms.Label();
            this.samplingRateBox = new System.Windows.Forms.ComboBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.loopBreakInputBox = new System.Windows.Forms.TextBox();
            this.loopBreakText = new System.Windows.Forms.Label();
            this.loopRecordingInputBox = new System.Windows.Forms.TextBox();
            this.loopRecordingText = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.periodicButton = new System.Windows.Forms.RadioButton();
            this.continuousButton = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.channelPlusBox3 = new System.Windows.Forms.ComboBox();
            this.channelMinusBox3 = new System.Windows.Forms.ComboBox();
            this.channelText1 = new System.Windows.Forms.Label();
            this.channelPlusBox1 = new System.Windows.Forms.ComboBox();
            this.channelText0 = new System.Windows.Forms.Label();
            this.channelText2 = new System.Windows.Forms.Label();
            this.channelPlusBox2 = new System.Windows.Forms.ComboBox();
            this.channelText3 = new System.Windows.Forms.Label();
            this.channelMinusBox0 = new System.Windows.Forms.ComboBox();
            this.channelPlusBox0 = new System.Windows.Forms.ComboBox();
            this.channelMinusBox2 = new System.Windows.Forms.ComboBox();
            this.channelMinusBox1 = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnStart_Sensing = new System.Windows.Forms.Button();
            this.btnStop_Sensing = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label_settingsTime);
            this.panel1.Controls.Add(this.groupBox5);
            this.panel1.Controls.Add(this.groupBox4);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.btnApply);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(809, 612);
            this.panel1.TabIndex = 0;
            // 
            // label_settingsTime
            // 
            this.label_settingsTime.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label_settingsTime.AutoSize = true;
            this.label_settingsTime.Font = new System.Drawing.Font("Arial", 9.75F);
            this.label_settingsTime.Location = new System.Drawing.Point(212, 457);
            this.label_settingsTime.Name = "label_settingsTime";
            this.label_settingsTime.Size = new System.Drawing.Size(32, 16);
            this.label_settingsTime.TabIndex = 75;
            this.label_settingsTime.Text = " . . .";
            this.label_settingsTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.samplingRateText);
            this.groupBox5.Controls.Add(this.samplingRateBox);
            this.groupBox5.Location = new System.Drawing.Point(214, 18);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(300, 54);
            this.groupBox5.TabIndex = 34;
            this.groupBox5.TabStop = false;
            // 
            // samplingRateText
            // 
            this.samplingRateText.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.samplingRateText.AutoSize = true;
            this.samplingRateText.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.samplingRateText.Location = new System.Drawing.Point(6, 21);
            this.samplingRateText.Name = "samplingRateText";
            this.samplingRateText.Size = new System.Drawing.Size(120, 19);
            this.samplingRateText.TabIndex = 16;
            this.samplingRateText.Text = "Sampling Rate";
            // 
            // samplingRateBox
            // 
            this.samplingRateBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.samplingRateBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.samplingRateBox.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.samplingRateBox.FormattingEnabled = true;
            this.samplingRateBox.Items.AddRange(new object[] {
            "250 Hz",
            "500 Hz",
            "1000 Hz"});
            this.samplingRateBox.Location = new System.Drawing.Point(162, 21);
            this.samplingRateBox.Name = "samplingRateBox";
            this.samplingRateBox.Size = new System.Drawing.Size(132, 24);
            this.samplingRateBox.TabIndex = 17;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.loopBreakInputBox);
            this.groupBox4.Controls.Add(this.loopBreakText);
            this.groupBox4.Controls.Add(this.loopRecordingInputBox);
            this.groupBox4.Controls.Add(this.loopRecordingText);
            this.groupBox4.Location = new System.Drawing.Point(214, 286);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(300, 100);
            this.groupBox4.TabIndex = 33;
            this.groupBox4.TabStop = false;
            // 
            // loopBreakInputBox
            // 
            this.loopBreakInputBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.loopBreakInputBox.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loopBreakInputBox.Location = new System.Drawing.Point(252, 53);
            this.loopBreakInputBox.Name = "loopBreakInputBox";
            this.loopBreakInputBox.Size = new System.Drawing.Size(42, 22);
            this.loopBreakInputBox.TabIndex = 17;
            this.loopBreakInputBox.Text = "3";
            this.loopBreakInputBox.Enter += new System.EventHandler(this.InputBox_Enter);
            this.loopBreakInputBox.Leave += new System.EventHandler(this.InputBox_Leave);
            this.loopBreakInputBox.MouseEnter += new System.EventHandler(this.InputBox_Enter);
            this.loopBreakInputBox.MouseHover += new System.EventHandler(this.InputBox_Enter);
            this.loopBreakInputBox.Validating += new System.ComponentModel.CancelEventHandler(this.InputBox_Validating);
            // 
            // loopBreakText
            // 
            this.loopBreakText.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.loopBreakText.AutoSize = true;
            this.loopBreakText.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loopBreakText.Location = new System.Drawing.Point(6, 54);
            this.loopBreakText.Name = "loopBreakText";
            this.loopBreakText.Size = new System.Drawing.Size(212, 19);
            this.loopBreakText.TabIndex = 16;
            this.loopBreakText.Text = "Break between loops (min)";
            // 
            // loopRecordingInputBox
            // 
            this.loopRecordingInputBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.loopRecordingInputBox.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loopRecordingInputBox.Location = new System.Drawing.Point(252, 25);
            this.loopRecordingInputBox.Name = "loopRecordingInputBox";
            this.loopRecordingInputBox.Size = new System.Drawing.Size(42, 22);
            this.loopRecordingInputBox.TabIndex = 15;
            this.loopRecordingInputBox.Text = "1";
            this.loopRecordingInputBox.Enter += new System.EventHandler(this.InputBox_Enter);
            this.loopRecordingInputBox.Leave += new System.EventHandler(this.InputBox_Leave);
            this.loopRecordingInputBox.MouseEnter += new System.EventHandler(this.InputBox_Enter);
            this.loopRecordingInputBox.Validating += new System.ComponentModel.CancelEventHandler(this.InputBox_Validating);
            // 
            // loopRecordingText
            // 
            this.loopRecordingText.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.loopRecordingText.AutoSize = true;
            this.loopRecordingText.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loopRecordingText.Location = new System.Drawing.Point(6, 28);
            this.loopRecordingText.Name = "loopRecordingText";
            this.loopRecordingText.Size = new System.Drawing.Size(240, 19);
            this.loopRecordingText.TabIndex = 14;
            this.loopRecordingText.Text = "Length of recording loop (min)";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.periodicButton);
            this.groupBox3.Controls.Add(this.continuousButton);
            this.groupBox3.Location = new System.Drawing.Point(215, 225);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(299, 54);
            this.groupBox3.TabIndex = 32;
            this.groupBox3.TabStop = false;
            // 
            // periodicButton
            // 
            this.periodicButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.periodicButton.AutoSize = true;
            this.periodicButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.periodicButton.Location = new System.Drawing.Point(203, 21);
            this.periodicButton.Name = "periodicButton";
            this.periodicButton.Size = new System.Drawing.Size(90, 23);
            this.periodicButton.TabIndex = 9;
            this.periodicButton.TabStop = true;
            this.periodicButton.Text = "Periodic";
            this.periodicButton.UseVisualStyleBackColor = true;
            // 
            // continuousButton
            // 
            this.continuousButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.continuousButton.AutoSize = true;
            this.continuousButton.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.continuousButton.Location = new System.Drawing.Point(9, 21);
            this.continuousButton.Name = "continuousButton";
            this.continuousButton.Size = new System.Drawing.Size(117, 23);
            this.continuousButton.TabIndex = 8;
            this.continuousButton.TabStop = true;
            this.continuousButton.Text = "Continuous";
            this.continuousButton.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.channelPlusBox3);
            this.groupBox1.Controls.Add(this.channelMinusBox3);
            this.groupBox1.Controls.Add(this.channelText1);
            this.groupBox1.Controls.Add(this.channelPlusBox1);
            this.groupBox1.Controls.Add(this.channelText0);
            this.groupBox1.Controls.Add(this.channelText2);
            this.groupBox1.Controls.Add(this.channelPlusBox2);
            this.groupBox1.Controls.Add(this.channelText3);
            this.groupBox1.Controls.Add(this.channelMinusBox0);
            this.groupBox1.Controls.Add(this.channelPlusBox0);
            this.groupBox1.Controls.Add(this.channelMinusBox2);
            this.groupBox1.Controls.Add(this.channelMinusBox1);
            this.groupBox1.Location = new System.Drawing.Point(214, 78);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(300, 141);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            // 
            // channelPlusBox3
            // 
            this.channelPlusBox3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.channelPlusBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.channelPlusBox3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.channelPlusBox3.FormattingEnabled = true;
            this.channelPlusBox3.Items.AddRange(new object[] {
            "E8",
            "E9",
            "E10",
            "E11",
            "E12",
            "E13",
            "E14",
            "E15"});
            this.channelPlusBox3.Location = new System.Drawing.Point(235, 102);
            this.channelPlusBox3.Name = "channelPlusBox3";
            this.channelPlusBox3.Size = new System.Drawing.Size(59, 24);
            this.channelPlusBox3.TabIndex = 39;
            // 
            // channelMinusBox3
            // 
            this.channelMinusBox3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.channelMinusBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.channelMinusBox3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.channelMinusBox3.FormattingEnabled = true;
            this.channelMinusBox3.Items.AddRange(new object[] {
            "E8",
            "E9",
            "E10",
            "E11",
            "E12",
            "E13",
            "E14",
            "E15"});
            this.channelMinusBox3.Location = new System.Drawing.Point(162, 102);
            this.channelMinusBox3.Name = "channelMinusBox3";
            this.channelMinusBox3.Size = new System.Drawing.Size(59, 24);
            this.channelMinusBox3.TabIndex = 38;
            // 
            // channelText1
            // 
            this.channelText1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.channelText1.AutoSize = true;
            this.channelText1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.channelText1.Location = new System.Drawing.Point(14, 43);
            this.channelText1.Name = "channelText1";
            this.channelText1.Size = new System.Drawing.Size(86, 19);
            this.channelText1.TabIndex = 28;
            this.channelText1.Text = "Channel 1";
            // 
            // channelPlusBox1
            // 
            this.channelPlusBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.channelPlusBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.channelPlusBox1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.channelPlusBox1.FormattingEnabled = true;
            this.channelPlusBox1.Items.AddRange(new object[] {
            "E0",
            "E1",
            "E2",
            "E3",
            "E4",
            "E5",
            "E6",
            "E7"});
            this.channelPlusBox1.Location = new System.Drawing.Point(235, 42);
            this.channelPlusBox1.Name = "channelPlusBox1";
            this.channelPlusBox1.Size = new System.Drawing.Size(59, 24);
            this.channelPlusBox1.TabIndex = 35;
            // 
            // channelText0
            // 
            this.channelText0.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.channelText0.AutoSize = true;
            this.channelText0.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.channelText0.Location = new System.Drawing.Point(14, 13);
            this.channelText0.Name = "channelText0";
            this.channelText0.Size = new System.Drawing.Size(86, 19);
            this.channelText0.TabIndex = 29;
            this.channelText0.Text = "Channel 0";
            // 
            // channelText2
            // 
            this.channelText2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.channelText2.AutoSize = true;
            this.channelText2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.channelText2.Location = new System.Drawing.Point(14, 73);
            this.channelText2.Name = "channelText2";
            this.channelText2.Size = new System.Drawing.Size(86, 19);
            this.channelText2.TabIndex = 30;
            this.channelText2.Text = "Channel 2";
            // 
            // channelPlusBox2
            // 
            this.channelPlusBox2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.channelPlusBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.channelPlusBox2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.channelPlusBox2.FormattingEnabled = true;
            this.channelPlusBox2.Items.AddRange(new object[] {
            "E8",
            "E9",
            "E10",
            "E11",
            "E12",
            "E13",
            "E14",
            "E15"});
            this.channelPlusBox2.Location = new System.Drawing.Point(235, 72);
            this.channelPlusBox2.Name = "channelPlusBox2";
            this.channelPlusBox2.Size = new System.Drawing.Size(59, 24);
            this.channelPlusBox2.TabIndex = 37;
            // 
            // channelText3
            // 
            this.channelText3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.channelText3.AutoSize = true;
            this.channelText3.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.channelText3.Location = new System.Drawing.Point(14, 103);
            this.channelText3.Name = "channelText3";
            this.channelText3.Size = new System.Drawing.Size(86, 19);
            this.channelText3.TabIndex = 31;
            this.channelText3.Text = "Channel 3";
            // 
            // channelMinusBox0
            // 
            this.channelMinusBox0.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.channelMinusBox0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.channelMinusBox0.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.channelMinusBox0.FormattingEnabled = true;
            this.channelMinusBox0.Items.AddRange(new object[] {
            "E0",
            "E1",
            "E2",
            "E3",
            "E4",
            "E5",
            "E6",
            "E7"});
            this.channelMinusBox0.Location = new System.Drawing.Point(162, 12);
            this.channelMinusBox0.Name = "channelMinusBox0";
            this.channelMinusBox0.Size = new System.Drawing.Size(59, 24);
            this.channelMinusBox0.TabIndex = 32;
            // 
            // channelPlusBox0
            // 
            this.channelPlusBox0.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.channelPlusBox0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.channelPlusBox0.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.channelPlusBox0.FormattingEnabled = true;
            this.channelPlusBox0.Items.AddRange(new object[] {
            "E0",
            "E1",
            "E2",
            "E3",
            "E4",
            "E5",
            "E6",
            "E7"});
            this.channelPlusBox0.Location = new System.Drawing.Point(235, 12);
            this.channelPlusBox0.Name = "channelPlusBox0";
            this.channelPlusBox0.Size = new System.Drawing.Size(59, 24);
            this.channelPlusBox0.TabIndex = 33;
            // 
            // channelMinusBox2
            // 
            this.channelMinusBox2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.channelMinusBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.channelMinusBox2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.channelMinusBox2.FormattingEnabled = true;
            this.channelMinusBox2.Items.AddRange(new object[] {
            "E8",
            "E9",
            "E10",
            "E11",
            "E12",
            "E13",
            "E14",
            "E15"});
            this.channelMinusBox2.Location = new System.Drawing.Point(162, 72);
            this.channelMinusBox2.Name = "channelMinusBox2";
            this.channelMinusBox2.Size = new System.Drawing.Size(59, 24);
            this.channelMinusBox2.TabIndex = 36;
            // 
            // channelMinusBox1
            // 
            this.channelMinusBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.channelMinusBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.channelMinusBox1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.channelMinusBox1.FormattingEnabled = true;
            this.channelMinusBox1.Items.AddRange(new object[] {
            "E0",
            "E1",
            "E2",
            "E3",
            "E4",
            "E5",
            "E6",
            "E7"});
            this.channelMinusBox1.Location = new System.Drawing.Point(162, 42);
            this.channelMinusBox1.Name = "channelMinusBox1";
            this.channelMinusBox1.Size = new System.Drawing.Size(59, 24);
            this.channelMinusBox1.TabIndex = 34;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnStart_Sensing);
            this.groupBox2.Controls.Add(this.btnStop_Sensing);
            this.groupBox2.Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold);
            this.groupBox2.Location = new System.Drawing.Point(214, 487);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(300, 100);
            this.groupBox2.TabIndex = 30;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sensing Control";
            // 
            // btnStart_Sensing
            // 
            this.btnStart_Sensing.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.btnStart_Sensing.Location = new System.Drawing.Point(162, 33);
            this.btnStart_Sensing.Name = "btnStart_Sensing";
            this.btnStart_Sensing.Size = new System.Drawing.Size(132, 54);
            this.btnStart_Sensing.TabIndex = 21;
            this.btnStart_Sensing.Text = "Start Sensing";
            this.btnStart_Sensing.UseVisualStyleBackColor = true;
            this.btnStart_Sensing.Click += new System.EventHandler(this.btnStart_Sensing_Click);
            // 
            // btnStop_Sensing
            // 
            this.btnStop_Sensing.Enabled = false;
            this.btnStop_Sensing.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.btnStop_Sensing.Location = new System.Drawing.Point(18, 33);
            this.btnStop_Sensing.Name = "btnStop_Sensing";
            this.btnStop_Sensing.Size = new System.Drawing.Size(132, 54);
            this.btnStop_Sensing.TabIndex = 20;
            this.btnStop_Sensing.Text = "Stop Sensing";
            this.btnStop_Sensing.UseVisualStyleBackColor = true;
            this.btnStop_Sensing.Click += new System.EventHandler(this.btnStop_Sensing_Click);
            // 
            // btnApply
            // 
            this.btnApply.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnApply.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.btnApply.Location = new System.Drawing.Point(300, 404);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(132, 54);
            this.btnApply.TabIndex = 28;
            this.btnApply.Text = "Apply Settings";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // SetSensingParamForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(809, 612);
            this.Controls.Add(this.panel1);
            this.Name = "SetSensingParamForm";
            this.Text = "Sensing Parameters";
            this.Activated += new System.EventHandler(this.SetSensingParamForm_Update);
            this.Load += new System.EventHandler(this.SetSensingParamForm_Update);
            this.Shown += new System.EventHandler(this.SetSensingParamForm_Update);
            this.Enter += new System.EventHandler(this.SetSensingParamForm_Update);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        protected internal System.Windows.Forms.Panel panel1;
        protected internal System.Windows.Forms.RadioButton periodicButton;
        protected internal System.Windows.Forms.RadioButton continuousButton;
        protected internal System.Windows.Forms.Button btnApply;
        protected internal System.Windows.Forms.GroupBox groupBox2;
        protected internal System.Windows.Forms.Button btnStart_Sensing;
        protected internal System.Windows.Forms.Button btnStop_Sensing;
        private System.Windows.Forms.GroupBox groupBox1;
        protected internal System.Windows.Forms.ComboBox channelPlusBox3;
        protected internal System.Windows.Forms.ComboBox channelMinusBox3;
        protected internal System.Windows.Forms.Label channelText1;
        protected internal System.Windows.Forms.ComboBox channelPlusBox1;
        protected internal System.Windows.Forms.Label channelText0;
        protected internal System.Windows.Forms.Label channelText2;
        protected internal System.Windows.Forms.ComboBox channelPlusBox2;
        protected internal System.Windows.Forms.Label channelText3;
        protected internal System.Windows.Forms.ComboBox channelMinusBox0;
        protected internal System.Windows.Forms.ComboBox channelPlusBox0;
        protected internal System.Windows.Forms.ComboBox channelMinusBox2;
        protected internal System.Windows.Forms.ComboBox channelMinusBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        protected internal System.Windows.Forms.TextBox loopBreakInputBox;
        protected internal System.Windows.Forms.Label loopBreakText;
        protected internal System.Windows.Forms.TextBox loopRecordingInputBox;
        protected internal System.Windows.Forms.Label loopRecordingText;
        private System.Windows.Forms.GroupBox groupBox5;
        protected internal System.Windows.Forms.Label samplingRateText;
        protected internal System.Windows.Forms.ComboBox samplingRateBox;
        protected internal System.Windows.Forms.Label label_settingsTime;
    }
}