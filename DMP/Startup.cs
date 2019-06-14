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
using System.Configuration;
using System.IO;
using System.Threading;
using Medtronic.SummitAPI.Classes;
using Medtronic.SummitAPI.Events;
using Medtronic.TelemetryM;
using Medtronic.NeuroStim.Olympus.DataTypes.Core;
using Medtronic.NeuroStim.Olympus.DataTypes.Sensing;
using Medtronic.NeuroStim.Olympus.DataTypes.PowerManagement;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Timers;


namespace DMP
{
    public partial class Startup : Form
    {
        static System.Timers.Timer StartupTimer;
        bool StartupTimerON;


        public Startup(DMP_Main_MDIParent parent)
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.WindowState = FormWindowState.Normal;

            this.MdiParent = parent;
        }

        private void Startup_Load(object sender, EventArgs e)
        {
            Mode.Text = ConfigurationManager.AppSettings["Mode"];
            SetNumbers();

            ///Home Screen
            Home HomeScreen = new Home((DMP_Main_MDIParent)this.MdiParent);
            ((DMP_Main_MDIParent)this.MdiParent).homeForm = HomeScreen;

            /// excute the backgroundworker_DoWork
            backgroundWorker1.RunWorkerAsync();
        }

        /// <summary>
        /// 
        /// Requirement 2.1.1: Identification and Model Number
        /// The Startup-screen shall include identification of the software(“Mayo EPAD Application”.)
        /// 
        /// Requirement 2.1.2: Version, and Build
        /// The Startup-screen shall include identification of the software version and build number.
        /// 
        /// Requirement 2.1.3: Summit RDK Libraries
        /// The Startup-screen shall include identification of the ID, version, and build number of
        /// the Medtronic software libraries used for iEEG streaming and stimulation control.
        /// 
        /// Requirement 2.1.4: User Limitation
        /// The Startup-screen shall include a statement that the device is intended to be operated by
        /// personnel trained in its proper use.
        /// 
        /// Requirement 2.1.5: Investigational Device
        /// The Startup-screen shall include a statement that the device is restricted to investigational purposes.
        /// For devices to be used in the United States, the marking shall read: “CAUTION – Investigational device.
        /// Limited by Federal (or United States) law to investigational use.”
        /// 
        /// 
        /// Requirement 4.1.1: Initialization Mode
        /// Following application start, the Application shall enter the Initialization Mode.
        /// While in the Initialization Mode, the Application shall place a session initialization entry in the Log File,
        /// load saved settings, verify access to the internal Bluetooth system, verify access and writability of data storage,
        /// and check for a Bluetooth-connected Medtronic Summit device.If connected, the Application shall
        /// read the INS battery level and display a notification if levels are below the threshold specified
        /// in the saved settings.
        /// Following successful initialization of the system, the Application shall enter the Home Mode
        /// Otherwise the Application displays an error dialog and exits.
        /// 
        /// </summary>

        /// code based on: https://www.youtube.com/watch?v=2qQgctSi4iY

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            // start startup screen timer
            StartupTimer = new System.Timers.Timer();
            StartupTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnStartupTimer);
            StartupTimer.Interval = 30000;  // 30 sec
            StartupTimer.AutoReset = true;
            StartupTimer.Enabled = true;

            StartupTimerON = true;

            /// Run Initialization
            ((DMP_Main_MDIParent)this.MdiParent).Initialization();

            int i = 0;
            while (StartupTimerON)
            {
                if (i == 99) // take progress bar backwards
                    i = 75;
                System.Threading.Thread.Sleep(300);
                i++;
                backgroundWorker1.ReportProgress(i);
            }

        }

        private void BackgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            /// updated from DoWork
            progressBar1.Value = e.ProgressPercentage;
            bool setlabel = !label_starting.Enabled;
            label_starting.Enabled = setlabel;
        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            /// called when process in background is complete
            Console.WriteLine("Startup screen backgroundworker finished: " + String.Format("{0:G}", DateTime.Now));
            ((DMP_Main_MDIParent)this.MdiParent).homeForm.Show();
            this.Close();



        }

        private void OnStartupTimer(object source, System.Timers.ElapsedEventArgs e)
        {
            /// called when timer is complete
            StartupTimer.Enabled = false;
            StartupTimer.Stop();
            StartupTimer.Dispose();
            StartupTimer = null;

            StartupTimerON = false;
        }

        private void SetNumbers()
        {
            /// set and get from application setting

            Version verNum = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            DateTime buildDate = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).LastWriteTime;

            verNum_software.Text = verNum.ToString();
            buildNum_software.Text = verNum.Build.ToString();
            date_software.Text = buildDate.ToString();

            verNum_stim.Text = (typeof(SummitSystem)).Assembly.GetName().Version.ToString();


        }
    }
}
