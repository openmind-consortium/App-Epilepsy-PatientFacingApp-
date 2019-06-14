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
using System.Threading;
using Medtronic.SummitAPI.Classes;
using Medtronic.SummitAPI.Events;
using Medtronic.TelemetryM;
using Medtronic.NeuroStim.Olympus.DataTypes.Core;
using Medtronic.NeuroStim.Olympus.DataTypes.Sensing;
using Medtronic.NeuroStim.Olympus.DataTypes.PowerManagement;
using System.Runtime.InteropServices;
using System.IO;
using System.Timers;
using Medtronic.NeuroStim.Olympus.DataTypes.DeviceManagement;

namespace DMP
{
    public partial class ProgramControlForm : MyFormPage
    /// MyFormPage
    /// Form
    {
        /// <summary>
        /// 
        /// Requirement 4.1.22: Program Shutdown Mode
        /// When Program Shutdown Mode is invoked the software will display a warning dialog alerting the user that if the program is terminated EEG
        /// sensing and dynamic stimulation adjustments will be halted, with options to continue or cancel.If the user elects to cancel, the software
        /// shall return to the mode from which the current mode was invoked. If the user elects to continue, the program shall complete any active tasks,
        /// and note system status at program termination in the log file.The software shall set the INS to the default baseline stimulation program before
        /// terminating the connection, closing open files, and deleting any temporary operational files. 
        ///
        /// </summary>
        /// 

        public ProgramControlForm(DMP_Main_MDIParent parent)
        {
            InitializeComponent();

            this.pnl = panel1;

            this.MdiParent = parent;

            ((DMP_Main_MDIParent)this.MdiParent).PropertyChanged -= ConnectionChanges;
            ((DMP_Main_MDIParent)this.MdiParent).PropertyChanged += ConnectionChanges;
            ConnectionChanges(this, null);
        }

        // update UI base on application property changes
        private void ConnectionChanges(object sender, PropertyChangedEventArgs e)
        {
            if (((DMP_Main_MDIParent)this.MdiParent).GetAnalyticsMode == 1)
                checkBox_Analytics.Checked = true;
            else
                checkBox_Analytics.Checked = false;

            if (((DMP_Main_MDIParent)this.MdiParent).GetIn_connect_to_ctm)
            {
                SetbtnStart_INSText("Connecting");
                SetbtnStart_INSEnable(false);
                SetbtnStop_INSEnable(true);
            }
            else
            {
                if (((DMP_Main_MDIParent)this.MdiParent).GetCTMconnection)
                {// if connected
                    SetbtnStart_INSText("Connected");
                    SetbtnStart_INSEnable(false);
                    SetbtnStop_INSEnable(true);
                }
                else
                {
                    SetbtnStart_INSText("Connect");
                    SetbtnStop_INSEnable(false);
                    SetbtnStart_INSEnable(true);
                }
            }
        }

        private void btnStart_App_Click(object sender, EventArgs e)
        {
            /// https://www.dotnetperls.com/messagebox-show
            DialogResult dialogResult = CustomMsgBox.Show("If the program is terminated EEG sensing and dynamic stimulation adjustments will be halted. \n" +
                                                 "Please confirm restarting the Application", "Restart Application", "Restart Application", "Cancel");
            if (dialogResult == DialogResult.Yes)
            {
                ((DMP_Main_MDIParent)this.MdiParent).CloseEnv();
                Application.Restart();
            }
        }

        private void btnStop_App_Click(object sender, EventArgs e)
        {
            /// https://www.dotnetperls.com/messagebox-show
            DialogResult dialogResult = CustomMsgBox.Show("If the program is terminated EEG sensing and dynamic stimulation adjustments will be halted. \n" +
                                                 "Please confirm closing the Application", "Close Application", "Close Application", "Cancel");
            if (dialogResult == DialogResult.Yes)
                {
                ((DMP_Main_MDIParent)this.MdiParent).CloseEnv();
                System.Environment.Exit(1);
            }
        }

        private void btnStart_INS_Click(object sender, EventArgs e)
        {
            new Thread(() => ((DMP_Main_MDIParent)this.MdiParent).On_start(sender, e)).Start();
        }

        private void btnStop_INS_Click(object sender, EventArgs e)
        {
            new Thread(() => ((DMP_Main_MDIParent)this.MdiParent).On_stop(sender, e)).Start();
        }

        private void btnSave_Notes_Click(object sender, EventArgs e)
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
            string txt1 = unixTimestamp.ToString() + "," + "," + "," + "physician's note: " + textBox1.Text;
            sw.WriteLine(txt1); //write the text.
            sw.Close();
            CustomMsgBox.Show("Note Saved!\nClick OK to continue.", "", "OK");
            textBox1.Clear();

            ((DMP_Main_MDIParent)this.MdiParent).ToggleOSK(false);
        }


        #region avoid cross-threading

        delegate void SetbtnStart_INSTextCallback(string text);
        public void SetbtnStart_INSText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.btnStart_INS.InvokeRequired)
            {
                SetbtnStart_INSTextCallback d = new SetbtnStart_INSTextCallback(SetbtnStart_INSText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.btnStart_INS.Text = text;
            }
        }

        delegate void SetbtnStart_INSEnableCallback(bool b);
        public void SetbtnStart_INSEnable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.btnStart_INS.InvokeRequired)
            {
                SetbtnStart_INSEnableCallback d = new SetbtnStart_INSEnableCallback(SetbtnStart_INSEnable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.btnStart_INS.Enabled = b;
            }
        }

        delegate void SetbtnStop_INSEnableCallback(bool b);
        public void SetbtnStop_INSEnable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.btnStop_INS.InvokeRequired)
            {
                SetbtnStop_INSEnableCallback d = new SetbtnStop_INSEnableCallback(SetbtnStop_INSEnable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.btnStop_INS.Enabled = b;
            }
        }
        #endregion

        private void checkBox_Analytics_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked=false;
            this.Invoke(new MethodInvoker(delegate () { isChecked = checkBox_Analytics.Checked; }));

            ((DMP_Main_MDIParent)this.MdiParent).Toggel_Analytics(isChecked);


        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
                ((DMP_Main_MDIParent)this.MdiParent).ToggleOSK(true);
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
                ((DMP_Main_MDIParent)this.MdiParent).ToggleOSK(false);
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            ((DMP_Main_MDIParent)this.MdiParent).ToggleOSK(true);
        }
    }
}
