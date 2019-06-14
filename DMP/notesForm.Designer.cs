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
    partial class notesForm
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
            this.Annotations_groupBox = new System.Windows.Forms.GroupBox();
            this.Eventbutton = new System.Windows.Forms.Button();
            this.Medbutton = new System.Windows.Forms.Button();
            this.SupMedbutton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox_Privacy = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button5 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.Annotations_groupBox.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Annotations_groupBox);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(809, 612);
            this.panel1.TabIndex = 1;
            // 
            // Annotations_groupBox
            // 
            this.Annotations_groupBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Annotations_groupBox.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.Annotations_groupBox.Controls.Add(this.Eventbutton);
            this.Annotations_groupBox.Controls.Add(this.Medbutton);
            this.Annotations_groupBox.Controls.Add(this.SupMedbutton);
            this.Annotations_groupBox.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold);
            this.Annotations_groupBox.Location = new System.Drawing.Point(27, 42);
            this.Annotations_groupBox.Name = "Annotations_groupBox";
            this.Annotations_groupBox.Size = new System.Drawing.Size(279, 522);
            this.Annotations_groupBox.TabIndex = 23;
            this.Annotations_groupBox.TabStop = false;
            // 
            // Eventbutton
            // 
            this.Eventbutton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Eventbutton.BackColor = System.Drawing.Color.Yellow;
            this.Eventbutton.Font = new System.Drawing.Font("Arial Black", 24F);
            this.Eventbutton.Location = new System.Drawing.Point(29, 28);
            this.Eventbutton.Name = "Eventbutton";
            this.Eventbutton.Size = new System.Drawing.Size(220, 150);
            this.Eventbutton.TabIndex = 17;
            this.Eventbutton.Text = "Event";
            this.Eventbutton.UseCompatibleTextRendering = true;
            this.Eventbutton.UseVisualStyleBackColor = false;
            this.Eventbutton.Click += new System.EventHandler(this.Eventbutton_Click);
            // 
            // Medbutton
            // 
            this.Medbutton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Medbutton.BackColor = System.Drawing.Color.Green;
            this.Medbutton.Font = new System.Drawing.Font("Arial Black", 18F);
            this.Medbutton.Location = new System.Drawing.Point(29, 191);
            this.Medbutton.Name = "Medbutton";
            this.Medbutton.Size = new System.Drawing.Size(220, 150);
            this.Medbutton.TabIndex = 18;
            this.Medbutton.Text = "Regular Medication Dose";
            this.Medbutton.UseCompatibleTextRendering = true;
            this.Medbutton.UseVisualStyleBackColor = false;
            this.Medbutton.Click += new System.EventHandler(this.Medbutton_Click);
            // 
            // SupMedbutton
            // 
            this.SupMedbutton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.SupMedbutton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.SupMedbutton.Font = new System.Drawing.Font("Arial Black", 18F);
            this.SupMedbutton.Location = new System.Drawing.Point(29, 352);
            this.SupMedbutton.Name = "SupMedbutton";
            this.SupMedbutton.Size = new System.Drawing.Size(220, 150);
            this.SupMedbutton.TabIndex = 19;
            this.SupMedbutton.Text = "Supplemental Medication Dose";
            this.SupMedbutton.UseCompatibleTextRendering = true;
            this.SupMedbutton.UseVisualStyleBackColor = false;
            this.SupMedbutton.Click += new System.EventHandler(this.SupMedbutton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox_Privacy);
            this.groupBox2.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold);
            this.groupBox2.Location = new System.Drawing.Point(428, 418);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(192, 65);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            // 
            // checkBox_Privacy
            // 
            this.checkBox_Privacy.AutoSize = true;
            this.checkBox_Privacy.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox_Privacy.Location = new System.Drawing.Point(12, 25);
            this.checkBox_Privacy.Name = "checkBox_Privacy";
            this.checkBox_Privacy.Size = new System.Drawing.Size(165, 28);
            this.checkBox_Privacy.TabIndex = 20;
            this.checkBox_Privacy.Text = "Privacy Mode";
            this.checkBox_Privacy.UseVisualStyleBackColor = true;
            this.checkBox_Privacy.CheckedChanged += new System.EventHandler(this.checkBox_Privacy_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button5);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(328, 42);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(418, 347);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Patient Note";
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.SystemColors.Control;
            this.button5.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.button5.Location = new System.Drawing.Point(100, 303);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(202, 38);
            this.button5.TabIndex = 15;
            this.button5.Text = "Save Note";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Font = new System.Drawing.Font("Arial", 12F);
            this.textBox1.ForeColor = System.Drawing.Color.Gray;
            this.textBox1.Location = new System.Drawing.Point(12, 28);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(400, 269);
            this.textBox1.TabIndex = 14;
            this.textBox1.Text = "Patient Note";
            this.textBox1.Click += new System.EventHandler(this.textBox1_Click);
            this.textBox1.Enter += new System.EventHandler(this.textBox1_Enter);
            this.textBox1.Leave += new System.EventHandler(this.textBox1_Leave);
            this.textBox1.MouseEnter += new System.EventHandler(this.textBox1_Enter);
            // 
            // notesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(809, 612);
            this.Controls.Add(this.panel1);
            this.Name = "notesForm";
            this.Text = "Notes and Events";
            this.panel1.ResumeLayout(false);
            this.Annotations_groupBox.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        protected internal System.Windows.Forms.Button Eventbutton;
        protected internal System.Windows.Forms.Button Medbutton;
        protected internal System.Windows.Forms.Button SupMedbutton;
        protected internal System.Windows.Forms.CheckBox checkBox_Privacy;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        protected internal System.Windows.Forms.GroupBox Annotations_groupBox;
    }
}