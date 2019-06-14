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
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
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
    public partial class Home : Form
    {
        /// <summary>
        /// 
        /// 4.1.2 Home Mode
        /// 
        /// Requirement 4.1.2: Home Mode
        /// While in Home Mode the software shall display Login buttons offering Patient and Physician mode login options,
        /// which will bring up a password entry form.The Home screen shall display Seizure and Aura annotation buttons
        /// regardless of whether a user has logged in. **Battery levels
        /// 
        /// Requirement 4.1.3: Password Validation
        /// After the user enters a password and clicks OK, the software compares the entered password to a hashed,
        /// encrypted locally stored password for validation.If the password is correct the program enters
        /// Patient or Physician mode as appropriate.
        /// 
        /// Requirement 4.1.4: User Role
        /// Users shall be assigned one of the following roles by the software: Physician or Patient.
        /// 
        /// Requirement 4.1.5: User Privileges
        /// Patient users shall be given access to options detailed under section 4.1.3.
        /// Physician users shall be given access to options detailed under section 4.1.4. 
        /// 
        /// </summary>


        public Home(DMP_Main_MDIParent parent)
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.WindowState = FormWindowState.Normal;


            // initialize all progress bars to 0
            // https://www.youtube.com/watch?v=o7MGaf8YW6s

            CTMBattery_circularProgressBar.Value = 0;
            INSBattery_circularProgressBar.Value = 0;
            TabletBattery_circularProgressBar.Value = 0;

            this.MdiParent = parent;

            ((DMP_Main_MDIParent)this.MdiParent).PropertyChanged -= ConnectionChanges;
            ((DMP_Main_MDIParent)this.MdiParent).PropertyChanged += ConnectionChanges;
            ConnectionChanges(this, null);
        }

        // update UI base on application property changes
        private void ConnectionChanges(object sender, PropertyChangedEventArgs e)
        {
            Setlabel_Network(System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable());

            Setlabel_privacyMode(((DMP_Main_MDIParent)this.MdiParent).GetPrivacyMode);

            if (((DMP_Main_MDIParent)this.MdiParent).GetIn_connect_to_ctm)
            {
                // CTM might be done connecting
                if (((DMP_Main_MDIParent)this.MdiParent).GetCTMconnection)
                {
                    this.SetCTMStatus("Connected");
                    this.SetCTMStatusText("Connected");

                    this.SetINSStatus("Connecting");
                    this.SetINSStatusText("Connecting");
                }
                else
                {
                    this.SetCTMStatus("Connecting");
                    this.SetCTMStatusText("Connecting");

                    this.SetINSStatus("Not\nConnected");
                    this.SetINSStatusText("Not\nConnected");
                }

                // if connecting everything else is OFF
                this.SetINSIDText("N\\A");

                this.SetDataStatus("Off");
                this.SetDataStatusText("Off");

                this.SetStimulationStatus("Off");
                this.SetStimulationStatusText("Off");

                return;
            }

            // if NOT connecting check the rest

            if (!((DMP_Main_MDIParent)this.MdiParent).GetCTMconnection)
            { // if NOT connected to CTM everything else is OFF as well
                this.SetCTMStatus("Not\nConnected");
                this.SetCTMStatusText("Not\nConnected");

                this.SetINSStatus("Not\nConnected");
                this.SetINSStatusText("Not\nConnected");
                this.SetINSIDText("N\\A");

                this.SetDataStatus("Off");
                this.SetDataStatusText("Off");

                this.SetStimulationStatus("Off");
                this.SetStimulationStatusText("Off");

                return;
            }

            // if connected to CTM check the rest
            this.SetCTMStatus("Connected");
            this.SetCTMStatusText("Connected");

            if (!((DMP_Main_MDIParent)this.MdiParent).GetINSconnection)
            {
                this.SetINSStatus("Not\nConnected");
                this.SetINSStatusText("Not\nConnected");
                this.SetINSIDText("N\\A");

                this.SetDataStatus("Off");
                this.SetDataStatusText("Off");

                this.SetStimulationStatus("Off");
                this.SetStimulationStatusText("Off");

                return;
            }

            // if connected to INS check the rest

            this.SetINSStatus("Connected");
            this.SetINSStatusText("Connected");
            this.SetINSIDText(((DMP_Main_MDIParent)this.MdiParent).Patient_name);

            if (((DMP_Main_MDIParent)this.MdiParent).GetIsStreaming == 1)
            {
                this.SetDataStatus("Streaming");
                this.SetDataStatusText("Streaming");
            }
            else
            {
                this.SetDataStatus("Off");
                this.SetDataStatusText("Off");
            }

            if (((DMP_Main_MDIParent)this.MdiParent).GetIsStiming == 1)
            {
                this.SetStimulationStatus("On");
                this.SetStimulationStatusText("On");

                if (((DMP_Main_MDIParent)this.MdiParent).GetSafeMode)
                    safeMode.Text = "Cancel Safe Mode";
                else
                    safeMode.Text = "Safe Mode";
            }
            else
            {
                this.SetStimulationStatus("Off");
                this.SetStimulationStatusText("Off");
            }
        }

        public void Home_Load(object sender, EventArgs e)
        {
            ((DMP_Main_MDIParent)this.MdiParent).ToggleOSK(false);

            // set mode everytime the form is loaded
            ConfigurationManager.AppSettings["Mode"] = "Home";
            Mode.Text = ConfigurationManager.AppSettings["Mode"];
        }

        // log in to patient options
        private void PatientMode_Click(object sender, EventArgs e)
        {
            Login LoginScreen = new Login("Patient")
            {
                MdiParent = this.MdiParent
            };
            LoginScreen.Show();
            this.Hide();
        }

        // log in to physician options
        private void PhysicianMode_Click(object sender, EventArgs e)
        {
            Login LoginScreen = new Login("Physician")
            {
                MdiParent = this.MdiParent
            };
            LoginScreen.Show();
            this.Hide();
        }

        private void CloseApp_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = CustomMsgBox.Show("If the program is terminated EEG sensing and dynamic stimulation adjustments will be halted. \n" +
                                                 "Please confirm closing the Application", "Close Application", "Close Application", "Cancel");
            if (dialogResult == DialogResult.Yes)
            {
                ((DMP_Main_MDIParent)this.MdiParent).CloseEnv();
                System.Environment.Exit(1);
            }
        }

        private void Home_FormClosing(object sender, FormClosingEventArgs e)
        {
            Home HomeScreen = new Home((DMP_Main_MDIParent)this.MdiParent);
            HomeScreen.Show();

        }

        #region Annotattions

        System.Windows.Forms.Timer Eventtimer = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer Stopstimtimer = new System.Windows.Forms.Timer();
        int timersInterval = 30000;

        public void Eventbutton_Click(object sender, EventArgs e)
        {
            Console.Beep();
            new Thread(() =>
            {
                ((DMP_Main_MDIParent)this.MdiParent).CaptureVideo();
            }).Start();
            Eventtimer.Interval = timersInterval;
            Eventtimer.Tick += Eventtimer_Tick;
            Eventtimer.Start();
            Eventbutton.Enabled = false;

            string file_name_sz = ((DMP_Main_MDIParent)this.MdiParent).HomeFolder + Environment.MachineName + "\\sznotes.csv";
            StreamWriter sw;
            if (File.Exists(file_name_sz))
            {
                sw = new StreamWriter(file_name_sz, true);
            }
            else
            {
                sw = new StreamWriter(file_name_sz, true);
                sw.WriteLine("start_time, end_time, channel, annotation");
            }

            Int64 unixTimestamp = (Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
            string txt1 = unixTimestamp.ToString() + "," + "," + "," + "Event";
            sw.WriteLine(txt1); //write the text.

            sw.Close();
        }
        public void Medbutton_Click(object sender, EventArgs e)
        {
            Console.Beep();
            AddMed MedScreen = new AddMed(false)
            {
                MdiParent = this.MdiParent
            };
            MedScreen.Show();
        }

        public void SupMedbutton_Click(object sender, EventArgs e)
        {
            Console.Beep();
            AddMed MedScreen = new AddMed(true)
            {
                MdiParent = this.MdiParent
            };
            MedScreen.Show();
        }

        void Eventtimer_Tick(object sender, System.EventArgs e)
        {
            Eventbutton.Enabled = true;
            Eventtimer.Stop();
        }
        
        void Stopstimtimer_Tick(object sender, System.EventArgs e)
        {
            stopStim.Enabled = true;
            Stopstimtimer.Stop();
        }

        #endregion


        private void safeMode_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult;

            if (((DMP_Main_MDIParent)this.MdiParent).TheSummit == null ||
                (((DMP_Main_MDIParent)this.MdiParent).TheSummit.IsDisposed))
            {
                CustomMsgBox.Show("Hardware is not connected.\nSafe mode is currently not possible.", "Error", "OK");
            }
            else
            {
                // safe mode
                if (((DMP_Main_MDIParent)this.MdiParent).GetIsStiming == 1)
                {
                    if (((DMP_Main_MDIParent)this.MdiParent).GetStimMode == 1)
                    {
                        if (safeMode.Text.Equals("Safe Mode"))
                        {
                            dialogResult = CustomMsgBox.Show("Stimulation settings will be changed to default.\n\n" +
                            "Safe Mode sets Default stimulation parameters and disables stimulation changes.Use for patient safety reasons only."
                            , "Safe Mode", "Default settings", "Cancel");
                            if (dialogResult == DialogResult.Yes)
                            {
                                safeMode.Text = "Cancel Safe Mode";
                                ((DMP_Main_MDIParent)this.MdiParent).SetSafeMode = true;
                                ((DMP_Main_MDIParent)this.MdiParent).WriteStimulationSettingsFile();

                                Thread t = new Thread(() =>
                                {
                                    ((DMP_Main_MDIParent)this.MdiParent).SetDoing_stim = true;
                                    ((DMP_Main_MDIParent)this.MdiParent).StartStreaming(((DMP_Main_MDIParent)this.MdiParent).GetStimMode, ((DMP_Main_MDIParent)this.MdiParent).GetStreamingMode, "Default");
                                    ((DMP_Main_MDIParent)this.MdiParent).SetDoing_stim = false;
                                });
                                t.Start();
                            }
                        }
                        else
                        {
                            dialogResult = CustomMsgBox.Show("Confirm canceling Safe Mode.\n" ,"Safe Mode", "Confirm", "Cancel");
                            if (dialogResult == DialogResult.Yes)
                            {
                                safeMode.Text = "Safe Mode";
                                ((DMP_Main_MDIParent)this.MdiParent).SetSafeMode = false;
                                ((DMP_Main_MDIParent)this.MdiParent).WriteStimulationSettingsFile();

                                Thread t = new Thread(() =>
                                {
                                    ((DMP_Main_MDIParent)this.MdiParent).SetDoing_stim = true;
                                    ((DMP_Main_MDIParent)this.MdiParent).StartStreaming(((DMP_Main_MDIParent)this.MdiParent).GetStimMode, ((DMP_Main_MDIParent)this.MdiParent).GetStreamingMode, "Default");
                                    ((DMP_Main_MDIParent)this.MdiParent).SetDoing_stim = false;
                                });
                                t.Start();
                            }
                        }
                    }
                    else // STIMULATION IS ON BUT WE DONT WANT TO STIM
                    {
                        Console.WriteLine("We don't want to stim...\nStimulation is ON but stimMode is 0!\n");
                        new Thread(() => ((DMP_Main_MDIParent)this.MdiParent).StopStim(null)).Start();
                    }
                }
                else
                {   // stimulation is off but we want to stim
                    if (((DMP_Main_MDIParent)this.MdiParent).GetStimMode == 1)
                    {
                        Console.WriteLine("Stimulation is OFF but stimMode is 1!\n");
                        new Thread(() => ((DMP_Main_MDIParent)this.MdiParent).StartStim(null)).Start();
                    }
                    else
                    {
                        dialogResult = CustomMsgBox.Show("Stimulation is currently off.\n", "", "OK");
                    }
                }
            }
        }

        private void stopStim_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult;

            Stopstimtimer.Interval = timersInterval;
            Stopstimtimer.Tick += Stopstimtimer_Tick;
            Stopstimtimer.Start();
            stopStim.Enabled = false;

            if (((DMP_Main_MDIParent)this.MdiParent).TheSummit == null ||
                (((DMP_Main_MDIParent)this.MdiParent).TheSummit.IsDisposed))
            {
                CustomMsgBox.Show("Hardware is not connected.\nStop Stimulation is currently not possible.", "Error", "OK");
            }
            else
            {
                // stop stimulation
                if (((DMP_Main_MDIParent)this.MdiParent).GetIsStiming == 1)
                {
                    if (((DMP_Main_MDIParent)this.MdiParent).GetStimMode == 1)
                    {
                        dialogResult = CustomMsgBox.Show("Stimulation will be stopped.\n\n" +
                                         "Stimulation should be stopped for patient safety reasons only"
                                         , "Stop Stimulation", "Stop Stimulation", "Cancel");
                        if (dialogResult == DialogResult.Yes)
                        {
                            new Thread(() => ((DMP_Main_MDIParent)this.MdiParent).StopStim(null)).Start();
                        }
                    }
                    else // STIMULATION IS ON BUT WE DONT WANT TO STIM
                    {
                        Console.WriteLine("We don't want to stim...\nStimulation is ON but stimMode is 0!\n");
                        new Thread(() => ((DMP_Main_MDIParent)this.MdiParent).StopStim(null)).Start();
                    }
                }
                else
                {   // stimulation is off but we want to stim
                    if (((DMP_Main_MDIParent)this.MdiParent).GetStimMode == 1)
                    {
                        Console.WriteLine("Stimulation is OFF but stimMode is 1!\n");
                        new Thread(() => ((DMP_Main_MDIParent)this.MdiParent).StartStim(null)).Start();
                    }
                    else
                    {
                        dialogResult = CustomMsgBox.Show("Stimulation is currently off.\n", "", "OK");
                    }
                }
            }
        }

        #region avoid cross-threading

        delegate void Setlabel_recTimeTextCallback(string text);
        public void Setlabel_recTimeText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.label_recTime.InvokeRequired)
            {
                Setlabel_recTimeTextCallback d = new Setlabel_recTimeTextCallback(Setlabel_recTimeText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.label_recTime.Text = text;
            }
        }

        delegate void SetCTMStatusCallback(string text);
        public void SetCTMStatus(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.panel_CTMStatus.InvokeRequired)
            {
                SetCTMStatusCallback d = new SetCTMStatusCallback(SetCTMStatus);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                if (text.Equals("Not\nConnected"))
                    this.panel_CTMStatus.BackColor = Color.Firebrick;
                else if (text.Equals("Connecting"))
                    this.panel_CTMStatus.BackColor = Color.Gold;
                else if (text.Equals("Connected"))
                    this.panel_CTMStatus.BackColor = Color.MediumBlue;
                else
                    this.panel_CTMStatus.BackColor = Color.Silver;
            }
        }

        delegate void SetCTMBatteryCallback(int v);
        public void SetCTMBattery(int v)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.CTMBattery_circularProgressBar.InvokeRequired)
            {
                SetCTMBatteryCallback d = new SetCTMBatteryCallback(SetCTMBattery);
                this.Invoke(d, new object[] { v });
            }
            else
            {
                this.CTMBattery_circularProgressBar.Value = v;
                this.CTMBattery_circularProgressBar.Update();
            }
        }

        delegate void SetCTMBatteryTextCallback(string text);
        public void SetCTMBatteryText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.CTMBatteryText.InvokeRequired)
            {
                SetCTMBatteryTextCallback d = new SetCTMBatteryTextCallback(SetCTMBatteryText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.CTMBatteryText.Text = text;
                this.CTMBatteryText.ForeColor = (text.Equals("N\\A") ? Color.Black : (text.Equals("Low") ? Color.Firebrick : Color.MediumBlue));
            }
        }

        delegate void SetCTMStatusTextCallback(string text);
        public void SetCTMStatusText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.CTMStatusText.InvokeRequired)
            {
                SetCTMStatusTextCallback d = new SetCTMStatusTextCallback(SetCTMStatusText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.CTMStatusText.Text = text;
            }
        }

        delegate void SetINSStatusCallback(string text);
        public void SetINSStatus(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.panel_INSStatus.InvokeRequired)
            {
                SetINSStatusCallback d = new SetINSStatusCallback(SetINSStatus);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                if (text.Equals("Not\nConnected"))
                    this.panel_INSStatus.BackColor = Color.Firebrick;
                else if (text.Equals("Connecting"))
                    this.panel_INSStatus.BackColor = Color.Gold;
                else if (text.Equals("Connected"))
                    this.panel_INSStatus.BackColor = Color.MediumBlue;
                else
                    this.panel_INSStatus.BackColor = Color.Silver;
            }
        }

        delegate void SetINSBatteryCallback(int v);
        public void SetINSBattery(int v)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.INSBattery_circularProgressBar.InvokeRequired)
            {
                SetINSBatteryCallback d = new SetINSBatteryCallback(SetINSBattery);
                this.Invoke(d, new object[] { v });
            }
            else
            {
                this.INSBattery_circularProgressBar.Value = v;
                this.INSBattery_circularProgressBar.Update();
            }
        }

        delegate void SetINSBatteryTextCallback(string text);
        public void SetINSBatteryText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.INSBatteryText.InvokeRequired)
            {
                SetINSBatteryTextCallback d = new SetINSBatteryTextCallback(SetINSBatteryText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.INSBatteryText.Text = text;
            }
        }

        delegate void SetTabBatteryCallback(int v);
        public void SetTabBattery(int v)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.TabletBattery_circularProgressBar.InvokeRequired)
            {
                SetTabBatteryCallback d = new SetTabBatteryCallback(SetTabBattery);
                this.Invoke(d, new object[] { v });
            }
            else
            {
                this.TabletBattery_circularProgressBar.Value = v;
                this.TabletBattery_circularProgressBar.Update();
            }
        }

        delegate void SetTabBatteryTextCallback(string text);
        public void SetTabBatteryText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.TabBatteryText.InvokeRequired)
            {
                SetTabBatteryTextCallback d = new SetTabBatteryTextCallback(SetTabBatteryText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.TabBatteryText.Text = text;
            }
        }

        delegate void SetINSStatusTextCallback(string text);
        public void SetINSStatusText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.INSStatusText.InvokeRequired)
            {
                SetINSStatusTextCallback d = new SetINSStatusTextCallback(SetINSStatusText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.INSStatusText.Text = text;
            }
        }

        delegate void SetINSIDTextCallback(string text);
        public void SetINSIDText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.INSIDText.InvokeRequired)
            {
                SetINSIDTextCallback d = new SetINSIDTextCallback(SetINSIDText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.INSIDText.Text = text;
            }
        }

        delegate void SetDataStatusCallback(string text);
        public void SetDataStatus(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.panel_DataStatus.InvokeRequired)
            {
                SetDataStatusCallback d = new SetDataStatusCallback(SetDataStatus);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                if (text.Equals("Off"))
                    this.panel_DataStatus.BackColor = Color.Firebrick;
                else if (text.Equals("Streaming"))
                    this.panel_DataStatus.BackColor = Color.MediumBlue;
                else
                    this.panel_DataStatus.BackColor = Color.Silver;
            }
        }

        delegate void SetDataStatusTextCallback(string text);
        public void SetDataStatusText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.DataStatusText.InvokeRequired)
            {
                SetDataStatusTextCallback d = new SetDataStatusTextCallback(SetDataStatusText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.DataStatusText.Text = text;
            }
        }

        delegate void SetStimulationStatusCallback(string text);
        public void SetStimulationStatus(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.panel_StimStatus.InvokeRequired)
            {
                SetStimulationStatusCallback d = new SetStimulationStatusCallback(SetStimulationStatus);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                if (text.Equals("Off"))
                    this.panel_StimStatus.BackColor = Color.Firebrick;
                else if (text.Equals("On"))
                    this.panel_StimStatus.BackColor = Color.MediumBlue;
                else
                    this.panel_StimStatus.BackColor = Color.Silver;
            }
        }

        delegate void SetStimulationStatusTextCallback(string text);
        public void SetStimulationStatusText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.StimStatusText.InvokeRequired)
            {
                SetStimulationStatusTextCallback d = new SetStimulationStatusTextCallback(SetStimulationStatusText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.StimStatusText.Text = text;
            }
        }

        delegate void Setlabel_privacyModeCallback(bool v);
        public void Setlabel_privacyMode(bool v)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.panel_Privacy.InvokeRequired)
            {
                Setlabel_privacyModeCallback d = new Setlabel_privacyModeCallback(Setlabel_privacyMode);
                this.Invoke(d, new object[] { v });
            }
            else
            {
                if (v)
                {
                    this.panel_Privacy.BackColor = Color.MediumBlue;
                    this.SetprivacyStatusText("On");
                }
                else
                {
                    this.panel_Privacy.BackColor = Color.Firebrick;
                    this.SetprivacyStatusText("Off");
                }
            }
        }

        delegate void SetprivacyStatusTextCallback(string text);
        public void SetprivacyStatusText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.privacyStatusText.InvokeRequired)
            {
                SetprivacyStatusTextCallback d = new SetprivacyStatusTextCallback(SetprivacyStatusText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.privacyStatusText.Text = text;
            }
        }

        delegate void Setlabel_NetworkCallback(bool v);
        public void Setlabel_Network(bool v)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.panel_Network.InvokeRequired)
            {
                Setlabel_NetworkCallback d = new Setlabel_NetworkCallback(Setlabel_Network);
                this.Invoke(d, new object[] { v });
            }
            else
            {
                if (v)
                {
                    this.panel_Network.BackColor = Color.MediumBlue;
                    this.SetNetworkStatusText("Network\navailable");
                }
                else
                {
                    this.panel_Network.BackColor = Color.Firebrick;
                    this.SetNetworkStatusText("Network\nunavailable");
                }
            }
        }

        delegate void SetNetworkStatusTextCallback(string text);
        public void SetNetworkStatusText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.NetworkStatusText.InvokeRequired)
            {
                SetNetworkStatusTextCallback d = new SetNetworkStatusTextCallback(SetNetworkStatusText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.NetworkStatusText.Text = text;
            }
        }

        #endregion
    }
}
