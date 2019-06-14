/* Copyright (c) 2017-2018, Mayo Foundation for Medical Education and Research (MFMER), All rights reserved. 
Academic non-commercial use of this software is allowed with expressed permission of the developers. 
MFMER disclaims all implied warranties of merchantability and fitness for a particular purpose with 
respect to this software, its application, and any verbal or written statements regarding its use. 
The software may not be distributed to third parties without consent of MFMER. Use of this software 
constitutes acceptance of these terms. 
Contributors: Daniel Crepeau, Tal Pal Attia, Jan Cimbalnik, Hari Guragain, Mona Nasseri, Vaclav Kremen, 
Benjamin Brinkmann, Matt Stead, Gregory Worrell.  */

using System;
using Medtronic.NeuroStim.Olympus.DataTypes.Measurement;
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

namespace DMP
{

    public partial class checkImpForm : MyFormPage
    /// MyFormPage
    /// Form
    {
        private Thread ImpedanceRun_Thread
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// Check Impedance Mode -
        /// Upon invoking Check Impedance Mode the software shall warn the user that stimulation and sensing will be temporarily discontinued and offer options to continue or cancel.
        /// If the user chooses to continue the software will wait for any active sensing loops to complete, and will turn off sensing and stimulation functions. Once these functions
        /// are confirmed to have stopped by the INS device the software shall invoke a preprogrammed impedance test as recommended by the INS device manufacturer. Measured impedances
        /// for electrode contacts shall be written to the log file and to the Annotations list along with the testing parameters used to measure impedances.
        /// If the user is inactive for > 10 minutes (excluding the impedance test itself), the software shall return to Home Mode.
        /// 
        /// </summary>
        /// 

        public checkImpForm(DMP_Main_MDIParent parent)
        {
            InitializeComponent();

            this.pnl = panel1;

            this.MdiParent = parent;
        }


        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Text = "Impedance Test Running...";
            SetbtnStartEnable(false);

            DialogResult dialogResult = CustomMsgBox.Show("Sensing and stimulation will be temporarily stopped while the impedance test is performed.\n" +
                                                "Do you want to continue?", "Start Impedance Test", "Yes", "No");
            if (dialogResult == DialogResult.Yes)
            {
                SetbtnStopEnable(true);
                ImpedanceRun_Thread = new Thread(() => { ((DMP_Main_MDIParent)this.MdiParent).StartTestingImpedance(this); });
                ImpedanceRun_Thread.Start();
            }
            else if (dialogResult == DialogResult.Cancel)
            {
                Console.WriteLine("The Impedance test is rejected by user.");
                //To avoid cross-threading issue.
                this.Invoke((MethodInvoker)delegate ()
                {
                    SetbtnStartEnable(true);
                    btnStart.Text = "Start Impedance Test";
                    SetbtnStopEnable(false);
                });
            }
        }


        private void btnStop_Click(object sender, EventArgs e)
        {                             
            btnStop.Text = "Stopping...";                  
            ((DMP_Main_MDIParent)this.MdiParent).StopTestingImpedance(this);
            ((DMP_Main_MDIParent)this.MdiParent).StartStreaming(((DMP_Main_MDIParent)this.MdiParent).GetStimMode, ((DMP_Main_MDIParent)this.MdiParent).GetStreamingMode, ((DMP_Main_MDIParent)this.MdiParent).CurrState);
            ((DMP_Main_MDIParent)this.MdiParent).SetDoing_impTest = false; // imp test done 
            SetbtnStopEnable(false);
            btnStop.Text = "Stop Impedance Test";
            SetbtnStartEnable(true);
            btnStart.Text = "Start Impedance Test";
        }
        /// <summary>
        /// Use to start streaming and set the stim parameters to default.
        ///This resets the impedance test to false and automatically starts the streaming and stimulation.        
        ///
        /// </summary>
        public void Reset_ImpedanceTest()
        {
            //Change impedance test flag to false.
            GlobalVariablesForImpedanceTest.isRunning_impedanceTest = false;
            ((DMP_Main_MDIParent)this.MdiParent).StartStreaming(((DMP_Main_MDIParent)this.MdiParent).GetStimMode, ((DMP_Main_MDIParent)this.MdiParent).GetStreamingMode, ((DMP_Main_MDIParent)this.MdiParent).CurrState);
            ((DMP_Main_MDIParent)this.MdiParent).SetDoing_impTest = false;   
            //To avoid cross-threading issue.
            this.Invoke((MethodInvoker)delegate ()
            {
                SetbtnStartEnable(true);
                btnStart.Text = "Start Impedance Test";
                SetbtnStopEnable(false);
            });           
        }


        #region avoid cross-threading

        delegate void SetbtnStartEnableCallback(bool b);
        public void SetbtnStartEnable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.btnStart.InvokeRequired)
            {
                SetbtnStartEnableCallback d = new SetbtnStartEnableCallback(SetbtnStartEnable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.btnStart.Enabled = b;
            }
        }

        delegate void SetbtnStopEnableCallback(bool b);
        public void SetbtnStopEnable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.btnStop.InvokeRequired)
            {
                SetbtnStopEnableCallback d = new SetbtnStopEnableCallback(SetbtnStopEnable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.btnStop.Enabled = b;
            }
        }

        #endregion
    }
}
