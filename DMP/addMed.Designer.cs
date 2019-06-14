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
    partial class AddMed
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
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.c = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox_Medication = new System.Windows.Forms.ComboBox();
            this.comboBox_Dosage = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSave.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(117, 274);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(107, 41);
            this.btnSave.TabIndex = 36;
            this.btnSave.Text = "Save ";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Location = new System.Drawing.Point(236, 274);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(107, 41);
            this.btnCancel.TabIndex = 34;
            this.btnCancel.Text = "Done";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // c
            // 
            this.c.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.c.AutoSize = true;
            this.c.Font = new System.Drawing.Font("Arial", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.c.Location = new System.Drawing.Point(77, 31);
            this.c.Name = "c";
            this.c.Size = new System.Drawing.Size(317, 44);
            this.c.TabIndex = 33;
            this.c.Text = "Medication Dose";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(10, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(451, 26);
            this.label1.TabIndex = 37;
            this.label1.Text = "Please select medication and dosage to log";
            // 
            // comboBox_Medication
            // 
            this.comboBox_Medication.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.comboBox_Medication.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_Medication.Location = new System.Drawing.Point(117, 148);
            this.comboBox_Medication.Name = "comboBox_Medication";
            this.comboBox_Medication.Size = new System.Drawing.Size(226, 30);
            this.comboBox_Medication.TabIndex = 38;
            this.comboBox_Medication.Text = "Medication";
            this.comboBox_Medication.Enter += new System.EventHandler(this.ComboBox_Medication_Enter);
            this.comboBox_Medication.Leave += new System.EventHandler(this.ComboBox_Medication_Leave);
            this.comboBox_Medication.MouseEnter += new System.EventHandler(this.ComboBox_Medication_Enter);
            // 
            // comboBox_Dosage
            // 
            this.comboBox_Dosage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.comboBox_Dosage.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox_Dosage.Items.AddRange(new object[] {
            "0.5 mg",
            "1.0 mg",
            "2.0 mg",
            "25 mg",
            "30 mg",
            "50 mg",
            "100 mg",
            "200 mg",
            "300 mg ",
            "400 mg",
            "1200 mg",
            "1500 mg"});
            this.comboBox_Dosage.Location = new System.Drawing.Point(117, 199);
            this.comboBox_Dosage.Name = "comboBox_Dosage";
            this.comboBox_Dosage.Size = new System.Drawing.Size(226, 30);
            this.comboBox_Dosage.TabIndex = 39;
            this.comboBox_Dosage.Text = "Dosage";
            this.comboBox_Dosage.Enter += new System.EventHandler(this.ComboBox_Medication_Enter);
            this.comboBox_Dosage.Leave += new System.EventHandler(this.ComboBox_Medication_Leave);
            this.comboBox_Dosage.MouseEnter += new System.EventHandler(this.ComboBox_Medication_Enter);
            // 
            // addMed
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 401);
            this.ControlBox = false;
            this.Controls.Add(this.comboBox_Dosage);
            this.Controls.Add(this.comboBox_Medication);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.c);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "addMed";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Medication";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.AddMed_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        protected internal System.Windows.Forms.Button btnSave;
        protected internal System.Windows.Forms.Button btnCancel;
        protected internal System.Windows.Forms.Label c;
        protected internal System.Windows.Forms.Label label1;
        protected internal System.Windows.Forms.ComboBox comboBox_Medication;
        protected internal System.Windows.Forms.ComboBox comboBox_Dosage;
    }
}