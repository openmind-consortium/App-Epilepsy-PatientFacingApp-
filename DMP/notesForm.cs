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
using Medtronic.SummitAPI.Classes;
using Medtronic.SummitAPI.Events;
using Medtronic.TelemetryM;
using Medtronic.NeuroStim.Olympus.DataTypes.Core;
using Medtronic.NeuroStim.Olympus.DataTypes.Sensing;
using Medtronic.NeuroStim.Olympus.DataTypes.PowerManagement;
using System.Runtime.InteropServices;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Timers;

namespace DMP
{
    public partial class notesForm : MyFormPage
    /// MyFormPage
    /// Form
    {

        /// <summary>
        /// 
        /// </summary>

        public notesForm(DMP_Main_MDIParent parent)
        {
            InitializeComponent();
            this.pnl = panel1;

            this.MdiParent = parent;
        }

        //event recording by patient here.
        private void Eventbutton_Click(object sender, EventArgs e)
        {
            // using functiom from Home form
            foreach (Form childForm in this.MdiParent.MdiChildren)
            {
                if (childForm.Name == "Home")
                    ((Home)childForm).Eventbutton_Click(sender,e);
            }
        }

        //Regular Medication Dose here.
        private void Medbutton_Click(object sender, EventArgs e)
        {
            // using functiom from Home form
            foreach (Form childForm in this.MdiParent.MdiChildren)
            {
                if (childForm.Name == "Home")
                    ((Home)childForm).Medbutton_Click(sender, e);
            }
        }
        //Supplemental Medication Dose
        private void SupMedbutton_Click(object sender, EventArgs e)
        {
            // using functiom from Home form
            foreach (Form childForm in this.MdiParent.MdiChildren)
            {
                if (childForm.Name == "Home")
                    ((Home)childForm).SupMedbutton_Click(sender, e);
            }
        }

        //Patient notes.
        private void button5_Click(object sender, EventArgs e)
        {
            string file_name_sz = ((DMP_Main_MDIParent)this.MdiParent).HomeFolder + Environment.MachineName + "\\sznotes.csv";
            StreamWriter sw;
            if (File.Exists(file_name_sz))
            {
                sw = new StreamWriter(file_name_sz, true);
            }
            else
            {
                sw = new StreamWriter(file_name_sz, true);
                sw.WriteLine("start_time, end_time, channel, Event");
            }

            Int64 unixTimestamp = (Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
            string txt1 = unixTimestamp.ToString() + "," + "," + "," + "patient's note: " + textBox1.Text;
            sw.WriteLine(txt1); //write the text.
            sw.Close();
            CustomMsgBox.Show("Note Saved!\nClick OK to continue.", "", "OK");
            textBox1.Clear();

            ((DMP_Main_MDIParent)this.MdiParent).ToggleOSK(false);


        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            ((DMP_Main_MDIParent)this.MdiParent).ToggleOSK(true);

            this.textBox1.Text = string.Empty;
            this.textBox1.ForeColor = Color.Black;
            this.textBox1.Font = new Font(textBox1.Font, FontStyle.Regular);
            this.textBox1.Font = new Font(textBox1.Font, FontStyle.Bold);

        }

        private void checkBox_Privacy_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = false;
            this.Invoke(new MethodInvoker(delegate () { isChecked = checkBox_Privacy.Checked; }));

            ((DMP_Main_MDIParent)this.MdiParent).Toggel_PrivacyMode(isChecked);

        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            ((DMP_Main_MDIParent)this.MdiParent).ToggleOSK(true);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            ((DMP_Main_MDIParent)this.MdiParent).ToggleOSK(false);
        }
    }
}
