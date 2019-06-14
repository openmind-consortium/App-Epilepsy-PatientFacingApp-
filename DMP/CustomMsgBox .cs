/* Copyright (c) 2017-2018, Mayo Foundation for Medical Education and Research (MFMER), All rights reserved. 
Academic non-commercial use of this software is allowed with expressed permission of the developers. 
MFMER disclaims all implied warranties of merchantability and fitness for a particular purpose with 
respect to this software, its application, and any verbal or written statements regarding its use. 
The software may not be distributed to third parties without consent of MFMER. Use of this software 
constitutes acceptance of these terms. 
Contributors: Daniel Crepeau, Tal Pal Attia, Jan Cimbalnik, Hari Guragain, Mona Nasseri, Vaclav Kremen, 
Benjamin Brinkmann, Matt Stead, Gregory Worrell.  */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DMP
{

    /// <summary>
    /// 
    /// Replacment for conventional Message Box
    /// http://tutplusplus.blogspot.com/2010/07/c-tutorial-create-your-own-custom.html
    /// 
    /// </summary>


    public partial class CustomMsgBox : Form
    {
        public CustomMsgBox()
        {
            InitializeComponent();
        }

        static CustomMsgBox MsgBox;
        static DialogResult result = DialogResult.Cancel;


        public static DialogResult Show(string Text, string Caption, string btnOK)
        {
            MsgBox = new CustomMsgBox();
            MsgBox.label1.Text = Text;
            System.Media.SystemSounds.Exclamation.Play();

            MsgBox.button1.Visible = false;
            MsgBox.button2.Visible = false;
            MsgBox.button3.Visible = true;

            MsgBox.button3.Text = btnOK;

            MsgBox.Text = Caption;
            result = DialogResult.Cancel;
            MsgBox.ShowDialog();
            return result;
        }

        public static DialogResult Show(string Text, string Caption, string btnOK, string btnCancel)
        {
            MsgBox = new CustomMsgBox();
            MsgBox.label1.Text = Text;
            System.Media.SystemSounds.Exclamation.Play();

            MsgBox.button1.Visible = true;
            MsgBox.button2.Visible = false;
            MsgBox.button3.Visible = true;

            MsgBox.button1.Text = btnOK;
            MsgBox.button3.Text = btnCancel;

            MsgBox.Text = Caption;
            result = DialogResult.Cancel;
            MsgBox.ShowDialog();
            return result;
        }

        public static DialogResult Show(string Text, string Caption, string btnYes, string btnNo, string btnCancel)
        {
            MsgBox = new CustomMsgBox();
            MsgBox.label1.Text = Text;
            System.Media.SystemSounds.Exclamation.Play();

            MsgBox.button1.Visible = true;
            MsgBox.button2.Visible = true;
            MsgBox.button3.Visible = true;

            MsgBox.button1.Text = btnYes;
            MsgBox.button2.Text = btnNo;
            MsgBox.button3.Text = btnCancel;

            MsgBox.Text = Caption;
            result = DialogResult.Cancel;
            MsgBox.ShowDialog();
            return result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            result = DialogResult.Yes;
            MsgBox.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            result = DialogResult.No;
            MsgBox.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            result = DialogResult.Cancel;
            MsgBox.Close();
        }
    }
}
