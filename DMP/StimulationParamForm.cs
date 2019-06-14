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
using Medtronic.NeuroStim.Olympus.Commands;
using Medtronic.NeuroStim.Olympus.DataTypes.DeviceManagement;
using Medtronic.NeuroStim.Olympus.DataTypes.Therapy;
using Medtronic.NeuroStim.Olympus.DataTypes.Therapy.Adaptive;

namespace DMP
{
    public partial class StimulationParamForm : MyFormPage
    /// MyFormPage
    /// Form
    {

        /// <summary>
        /// 
        /// Requirement 4.1.16: Set Parameters Mode
        /// Upon invoking Set Parameters Mode the user will be presented a series of configurable options pertaining to the operation of the EPAD device and INS.
        /// This mode shall only be accessible to Physician users.The parameters the user will be able to control in this mode include EEG sampling interval and duration,
        /// and INS stimulation programs to be requested by the EPAD system in each classified state(baseline, sleep stage, seizure, pre-seizure).
        /// The user will also be presented with options to change the minimum INS and CTM battery levels at which the EPAD system will warn the patient, request default stimulation parameters,
        /// and disconnect from the INS and CTM gateway.
        /// The user shall not be allowed to set this battery threshold below 10% of battery life.If the user is inactive for > 10 minutes, the software shall return to Home Mode.
        ///  
        /// 
        /// Requirement 4.1.17: Exit Set Parameters Mode
        /// Upon exiting Set Parameters Mode the software shall return to the mode from which the current mode was invoked.
        /// 
        /// </summary>
        /// 

        protected internal ComboBox[] channelMinusBox;
        protected internal ComboBox[] channelPlusBox;

        StimTest stimTest;

        public StimulationParamForm(DMP_Main_MDIParent parent)
        {
            InitializeComponent();

            this.pnl = panel1;

            this.MdiParent = parent;
            ((DMP_Main_MDIParent)this.MdiParent).stimParamForm = this;

            // read setting and identify current state
            ((DMP_Main_MDIParent)this.MdiParent).ReadStimulationSettingsFile();
            ((DMP_Main_MDIParent)this.MdiParent).UpdateStimSettingsFile(this);
            // When reading setting for the first time - make sure they are correct
            if (!(((DMP_Main_MDIParent)this.MdiParent).StimSettingsCheck(this)))
            {
                Console.WriteLine("Stimulation setting are NOT valid!\nStimulation is turned OFF!");
                CustomMsgBox.Show("Stimulation setting are NOT valid!\nStimulation is turned OFF!", "Error", "OK");

                ((DMP_Main_MDIParent)this.MdiParent).SetStimMode = 0;
            }
            
            ((DMP_Main_MDIParent)this.MdiParent).PropertyChanged -= ConnectionChanges;
            ((DMP_Main_MDIParent)this.MdiParent).PropertyChanged += ConnectionChanges;
            ConnectionChanges(this, null);
        }

        // update UI base on application property changes
        private void ConnectionChanges(object sender, PropertyChangedEventArgs e)
        {
            // Ensure settings are not currently being update
            if (!((DMP_Main_MDIParent)this.MdiParent).GetDoing_stim)
            {
                Setlabel_ActivatedStateText(((DMP_Main_MDIParent)this.MdiParent).CurrState);
                Setlabel_settingsTimeText("Setting file last updated : " + ((DMP_Main_MDIParent)this.MdiParent).DtStimulationSettingsFile.ToString());

                if (((DMP_Main_MDIParent)this.MdiParent).GetCTMconnection && ((DMP_Main_MDIParent)this.MdiParent).GetINSconnection)
                {// if connected
                    if (!((DMP_Main_MDIParent)this.MdiParent).GetIn_connect_to_ctm)
                    {
                        if (((DMP_Main_MDIParent)this.MdiParent).GetIsStiming == 1)
                        {
                            SetbtnStart_StimulationText("Connected");
                            SetbtnStart_StimulationEnable(false);
                            SetbtnStop_StimulationEnable(true);
                        }
                        else
                        {
                            SetbtnStart_StimulationText("Start Stimulation");
                            SetbtnStop_StimulationEnable(false);
                            SetbtnStart_StimulationEnable(true);
                        } // IsStiming

                    } // ctm connected

                    return;

                } // lost connection
            }

            SetbtnStart_StimulationText("Start Stimulation");
            SetbtnStop_StimulationEnable(false);
            SetbtnStart_StimulationEnable(false);
        }

        // When Activated , Enter, Load and Shown
        public void StimulationParamForm_Update(object sender, EventArgs e)
        {
            // read setting and identify current state
            ((DMP_Main_MDIParent)this.MdiParent).ReadStimulationSettingsFile();
            ((DMP_Main_MDIParent)this.MdiParent).UpdateStimSettingsFile(this);

            Setlabel_ActivatedStateText(((DMP_Main_MDIParent)this.MdiParent).CurrState);
            Setlabel_settingsTimeText("Setting file last updated : " + ((DMP_Main_MDIParent)this.MdiParent).DtStimulationSettingsFile.ToString());

            ConnectionChanges(this, null);
        }

        private void BbtnApply_Click(object sender, EventArgs e)
        {
            if (!((DMP_Main_MDIParent)this.MdiParent).GetCTMconnection ||
                !((DMP_Main_MDIParent)this.MdiParent).GetINSconnection)
            {
                CustomMsgBox.Show("Hardware is not connected.\nApplying new settings is currently not possible.", "Error", "OK");
            }
            else
            {
                if (((DMP_Main_MDIParent)this.MdiParent).StimSettingsCheck(this))
                {
                    if (((DMP_Main_MDIParent)this.MdiParent).WriteStimulationSettingsFile())
                    {
                            this.Setlabel_settingsTimeText("Setting successfully saved: " + String.Format("{0:G}", DateTime.Now));
                    }

                    // ask if to start or test stim
                    DialogResult dialogResult = CustomMsgBox.Show("Settings are saved!\nWould you like to start stimulation with current settings or\ntest current stimulation settings?",
                                                                    "Apply Stimulation Settings",
                                                                    "Start Stimulation", "Test Stimulation", "Done");

                    if (dialogResult == DialogResult.No) // Test Stimulation
                    {
                            stimTest = new StimTest((DMP_Main_MDIParent)this.MdiParent);
                            stimTest.ShowDialog();
                    }
                    else if (dialogResult == DialogResult.Yes) // Start Stimulation
                    {
                        new Thread(() => ((DMP_Main_MDIParent)this.MdiParent).StartStim(this)).Start();
                    }
                }
            }
        }


        private void BtnTest_Click(object sender, EventArgs e)
        {
            if (!((DMP_Main_MDIParent)this.MdiParent).GetCTMconnection ||
                !((DMP_Main_MDIParent)this.MdiParent).GetINSconnection)
            {
                CustomMsgBox.Show("Hardware is not connected.\nTesting new settings is currently not possible.", "Error", "OK");
            }
            else
            {
                if (((DMP_Main_MDIParent)this.MdiParent).StimSettingsCheck(this))
                {
                        stimTest = new StimTest((DMP_Main_MDIParent)this.MdiParent);
                        stimTest.ShowDialog();
                }
            }
        }

        private void BtnStart_Stimulation_Click(object sender, EventArgs e)
        {
            if (!((DMP_Main_MDIParent)this.MdiParent).GetCTMconnection ||
                !((DMP_Main_MDIParent)this.MdiParent).GetINSconnection)
            {
                CustomMsgBox.Show("Hardware is not connected.\nStart Stimulation is currently not possible.", "Error", "OK");
            }
            else
            {
                if (((DMP_Main_MDIParent)this.MdiParent).StimSettingsCheck(this))
                {
                    SetbtnStart_StimulationEnable(false);
                    new Thread(() => ((DMP_Main_MDIParent)this.MdiParent).StartStim(this)).Start();
                }
            }
        }

        private void BtnStop_Stimulation_Click(object sender, EventArgs e)
        {
            SetbtnStop_StimulationEnable(false);
            new Thread(() => ((DMP_Main_MDIParent)this.MdiParent).StopStim(this)).Start();
        }

        private void EnableAll(bool enable)
        {
            SetnumericUpDown_defaultAmp1Enable(enable);
            SetnumericUpDown_defaultAmp2Enable(enable);
            SetnumericUpDown_defaultAmp3Enable(enable);
            SetnumericUpDown_defaultAmp4Enable(enable);
            SetnumericUpDown_defaultRateEnable(enable);

            SetnumericUpDown_sleepAmp1Enable(enable);
            SetnumericUpDown_sleepAmp2Enable(enable);
            SetnumericUpDown_sleepAmp3Enable(enable);
            SetnumericUpDown_sleepAmp4Enable(enable);
            SetnumericUpDown_sleepRateEnable(enable);

            SetnumericUpDown_preAmp1Enable(enable);
            SetnumericUpDown_preAmp2Enable(enable);
            SetnumericUpDown_preAmp3Enable(enable);
            SetnumericUpDown_preAmp4Enable(enable);
            SetnumericUpDown_preRateEnable(enable);

            SetnumericUpDown_seizureAmp1Enable(enable);
            SetnumericUpDown_seizureAmp2Enable(enable);
            SetnumericUpDown_seizureAmp3Enable(enable);
            SetnumericUpDown_seizureAmp4Enable(enable);
            SetnumericUpDown_seizureRateEnable(enable);
        }

        private void NumericUpDown_Validating(object sender, CancelEventArgs e)
        {
            NumericUpDown currentnup = (NumericUpDown)sender;
            if (currentnup.Text == "")
            {
                CustomMsgBox.Show("Field can not be empty", "Error", "OK");
                e.Cancel = true;
            }
            else if (currentnup.Value < currentnup.Minimum || currentnup.Value > currentnup.Maximum)
            {
                CustomMsgBox.Show("Value is out of range", "Error", "OK");
                e.Cancel = true;
            }
            else
                e.Cancel = false;

        }

        #region avoid cross-threading

        delegate void Setlabel_settingsTimeTextCallback(string text);
        public void Setlabel_settingsTimeText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.label_settingsTime.InvokeRequired)
            {
                Setlabel_settingsTimeTextCallback d = new Setlabel_settingsTimeTextCallback(Setlabel_settingsTimeText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.label_settingsTime.Text = text;
            }
        }

        delegate void SetbtnStart_StimulationTextCallback(string text);
        public void SetbtnStart_StimulationText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.btnStart_Stimulation.InvokeRequired)
            {
                SetbtnStart_StimulationTextCallback d = new SetbtnStart_StimulationTextCallback(SetbtnStart_StimulationText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.btnStart_Stimulation.Text = text;
            }
        }

        delegate void SetbtnStart_StimulationEnableCallback(bool b);
        public void SetbtnStart_StimulationEnable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.btnStart_Stimulation.InvokeRequired)
            {
                SetbtnStart_StimulationEnableCallback d = new SetbtnStart_StimulationEnableCallback(SetbtnStart_StimulationEnable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.btnStart_Stimulation.Enabled = b;
            }
        }

        delegate void SetbtnStop_StimulationEnableCallback(bool b);
        public void SetbtnStop_StimulationEnable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.btnStop_Stimulation.InvokeRequired)
            {
                SetbtnStop_StimulationEnableCallback d = new SetbtnStop_StimulationEnableCallback(SetbtnStop_StimulationEnable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.btnStop_Stimulation.Enabled = b;
            }
        }

        delegate void Setlabel_ActivatedStateTextCallback(string text);
        public void Setlabel_ActivatedStateText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.label_ActivatedState.InvokeRequired)
            {
                Setlabel_ActivatedStateTextCallback d = new Setlabel_ActivatedStateTextCallback(Setlabel_ActivatedStateText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.label_ActivatedState.Text = text;
            }
        }

        delegate void SetnumericUpDown_defaultRateEnableCallback(bool b);
        public void SetnumericUpDown_defaultRateEnable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_defaultRate.InvokeRequired)
            {
                SetnumericUpDown_defaultRateEnableCallback d = new SetnumericUpDown_defaultRateEnableCallback(SetnumericUpDown_defaultRateEnable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.numericUpDown_defaultRate.Enabled = b;
            }
        }

        delegate void SetnumericUpDown_defaultRateValueCallback(Decimal val);
        public void SetnumericUpDown_defaultRateValue(Decimal val)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_defaultRate.InvokeRequired)
            {
                SetnumericUpDown_defaultRateValueCallback d = new SetnumericUpDown_defaultRateValueCallback(SetnumericUpDown_defaultRateValue);
                this.Invoke(d, new object[] { val });
            }
            else
            {
                this.numericUpDown_defaultRate.Value = val;
            }
        }

        delegate void SetnumericUpDown_sleepRateEnableCallback(bool b);
        public void SetnumericUpDown_sleepRateEnable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_sleepRate.InvokeRequired)
            {
                SetnumericUpDown_sleepRateEnableCallback d = new SetnumericUpDown_sleepRateEnableCallback(SetnumericUpDown_sleepRateEnable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.numericUpDown_sleepRate.Enabled = b;
            }
        }

        delegate void SetnumericUpDown_sleepRateValueCallback(Decimal val);
        public void SetnumericUpDown_sleepRateValue(Decimal val)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_sleepRate.InvokeRequired)
            {
                SetnumericUpDown_sleepRateValueCallback d = new SetnumericUpDown_sleepRateValueCallback(SetnumericUpDown_sleepRateValue);
                this.Invoke(d, new object[] { val });
            }
            else
            {
                this.numericUpDown_sleepRate.Value = val;
            }
        }


        delegate void SetnumericUpDown_preRateEnableCallback(bool b);
        public void SetnumericUpDown_preRateEnable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_preRate.InvokeRequired)
            {
                SetnumericUpDown_preRateEnableCallback d = new SetnumericUpDown_preRateEnableCallback(SetnumericUpDown_preRateEnable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.numericUpDown_preRate.Enabled = b;
            }
        }

        delegate void SetnumericUpDown_preRateValueCallback(Decimal val);
        public void SetnumericUpDown_preRateValue(Decimal val)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_preRate.InvokeRequired)
            {
                SetnumericUpDown_preRateValueCallback d = new SetnumericUpDown_preRateValueCallback(SetnumericUpDown_preRateValue);
                this.Invoke(d, new object[] { val });
            }
            else
            {
                this.numericUpDown_preRate.Value = val;
            }
        }

        delegate void SetnumericUpDown_seizureRateEnableCallback(bool b);
        public void SetnumericUpDown_seizureRateEnable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_seizureRate.InvokeRequired)
            {
                SetnumericUpDown_seizureRateEnableCallback d = new SetnumericUpDown_seizureRateEnableCallback(SetnumericUpDown_seizureRateEnable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.numericUpDown_seizureRate.Enabled = b;
            }
        }

        delegate void SetnumericUpDown_seizureRateValueCallback(Decimal val);
        public void SetnumericUpDown_seizureRateValue(Decimal val)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_seizureRate.InvokeRequired)
            {
                SetnumericUpDown_seizureRateValueCallback d = new SetnumericUpDown_seizureRateValueCallback(SetnumericUpDown_seizureRateValue);
                this.Invoke(d, new object[] { val });
            }
            else
            {
                this.numericUpDown_seizureRate.Value = val;
            }
        }

        delegate void SetnumericUpDown_defaultAmp1EnableCallback(bool b);
        public void SetnumericUpDown_defaultAmp1Enable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_defaultAmp1.InvokeRequired)
            {
                SetnumericUpDown_defaultAmp1EnableCallback d = new SetnumericUpDown_defaultAmp1EnableCallback(SetnumericUpDown_defaultAmp1Enable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.numericUpDown_defaultAmp1.Enabled = b;
            }
        }

        delegate void SetnumericUpDown_defaultAmp1ValueCallback(Decimal val);
        public void SetnumericUpDown_defaultAmp1Value(Decimal val)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_defaultAmp1.InvokeRequired)
            {
                SetnumericUpDown_defaultAmp1ValueCallback d = new SetnumericUpDown_defaultAmp1ValueCallback(SetnumericUpDown_defaultAmp1Value);
                this.Invoke(d, new object[] { val });
            }
            else
            {
                this.numericUpDown_defaultAmp1.Value = val;
            }
        }

        delegate void SetnumericUpDown_defaultAmp2EnableCallback(bool b);
        public void SetnumericUpDown_defaultAmp2Enable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_defaultAmp2.InvokeRequired)
            {
                SetnumericUpDown_defaultAmp2EnableCallback d = new SetnumericUpDown_defaultAmp2EnableCallback(SetnumericUpDown_defaultAmp2Enable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.numericUpDown_defaultAmp2.Enabled = b;
            }
        }

        delegate void SetnumericUpDown_defaultAmp2ValueCallback(Decimal val);
        public void SetnumericUpDown_defaultAmp2Value(Decimal val)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_defaultAmp2.InvokeRequired)
            {
                SetnumericUpDown_defaultAmp2ValueCallback d = new SetnumericUpDown_defaultAmp2ValueCallback(SetnumericUpDown_defaultAmp2Value);
                this.Invoke(d, new object[] { val });
            }
            else
            {
                this.numericUpDown_defaultAmp2.Value = val;
            }
        }

        delegate void SetnumericUpDown_defaultAmp3EnableCallback(bool b);
        public void SetnumericUpDown_defaultAmp3Enable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_defaultAmp3.InvokeRequired)
            {
                SetnumericUpDown_defaultAmp3EnableCallback d = new SetnumericUpDown_defaultAmp3EnableCallback(SetnumericUpDown_defaultAmp3Enable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.numericUpDown_defaultAmp3.Enabled = b;
            }
        }

        delegate void SetnumericUpDown_defaultAmp3ValueCallback(Decimal val);
        public void SetnumericUpDown_defaultAmp3Value(Decimal val)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_defaultAmp3.InvokeRequired)
            {
                SetnumericUpDown_defaultAmp3ValueCallback d = new SetnumericUpDown_defaultAmp3ValueCallback(SetnumericUpDown_defaultAmp3Value);
                this.Invoke(d, new object[] { val });
            }
            else
            {
                this.numericUpDown_defaultAmp3.Value = val;
            }
        }

        delegate void SetnumericUpDown_defaultAmp4EnableCallback(bool b);
        public void SetnumericUpDown_defaultAmp4Enable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_defaultAmp4.InvokeRequired)
            {
                SetnumericUpDown_defaultAmp4EnableCallback d = new SetnumericUpDown_defaultAmp4EnableCallback(SetnumericUpDown_defaultAmp4Enable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.numericUpDown_defaultAmp4.Enabled = b;
            }
        }

        delegate void SetnumericUpDown_defaultAmp4ValueCallback(Decimal val);
        public void SetnumericUpDown_defaultAmp4Value(Decimal val)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_defaultAmp4.InvokeRequired)
            {
                SetnumericUpDown_defaultAmp4ValueCallback d = new SetnumericUpDown_defaultAmp4ValueCallback(SetnumericUpDown_defaultAmp4Value);
                this.Invoke(d, new object[] { val });
            }
            else
            {
                this.numericUpDown_defaultAmp4.Value = val;
            }
        }

        delegate void SetnumericUpDown_sleepAmp1EnableCallback(bool b);
        public void SetnumericUpDown_sleepAmp1Enable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_sleepAmp1.InvokeRequired)
            {
                SetnumericUpDown_sleepAmp1EnableCallback d = new SetnumericUpDown_sleepAmp1EnableCallback(SetnumericUpDown_sleepAmp1Enable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.numericUpDown_sleepAmp1.Enabled = b;
            }
        }

        delegate void SetnumericUpDown_sleepAmp1ValueCallback(Decimal val);
        public void SetnumericUpDown_sleepAmp1Value(Decimal val)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_sleepAmp1.InvokeRequired)
            {
                SetnumericUpDown_sleepAmp1ValueCallback d = new SetnumericUpDown_sleepAmp1ValueCallback(SetnumericUpDown_sleepAmp1Value);
                this.Invoke(d, new object[] { val });
            }
            else
            {
                this.numericUpDown_sleepAmp1.Value = val;
            }
        }

        delegate void SetnumericUpDown_sleepAmp2EnableCallback(bool b);
        public void SetnumericUpDown_sleepAmp2Enable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_sleepAmp2.InvokeRequired)
            {
                SetnumericUpDown_sleepAmp2EnableCallback d = new SetnumericUpDown_sleepAmp2EnableCallback(SetnumericUpDown_sleepAmp2Enable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.numericUpDown_sleepAmp2.Enabled = b;
            }
        }

        delegate void SetnumericUpDown_sleepAmp2ValueCallback(Decimal val);
        public void SetnumericUpDown_sleepAmp2Value(Decimal val)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_sleepAmp2.InvokeRequired)
            {
                SetnumericUpDown_sleepAmp2ValueCallback d = new SetnumericUpDown_sleepAmp2ValueCallback(SetnumericUpDown_sleepAmp2Value);
                this.Invoke(d, new object[] { val });
            }
            else
            {
                this.numericUpDown_sleepAmp2.Value = val;
            }
        }

        delegate void SetnumericUpDown_sleepAmp3EnableCallback(bool b);
        public void SetnumericUpDown_sleepAmp3Enable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_sleepAmp3.InvokeRequired)
            {
                SetnumericUpDown_sleepAmp3EnableCallback d = new SetnumericUpDown_sleepAmp3EnableCallback(SetnumericUpDown_sleepAmp3Enable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.numericUpDown_sleepAmp3.Enabled = b;
            }
        }

        delegate void SetnumericUpDown_sleepAmp3ValueCallback(Decimal val);
        public void SetnumericUpDown_sleepAmp3Value(Decimal val)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_sleepAmp3.InvokeRequired)
            {
                SetnumericUpDown_sleepAmp3ValueCallback d = new SetnumericUpDown_sleepAmp3ValueCallback(SetnumericUpDown_sleepAmp3Value);
                this.Invoke(d, new object[] { val });
            }
            else
            {
                this.numericUpDown_sleepAmp3.Value = val;
            }
        }

        delegate void SetnumericUpDown_sleepAmp4EnableCallback(bool b);
        public void SetnumericUpDown_sleepAmp4Enable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_sleepAmp4.InvokeRequired)
            {
                SetnumericUpDown_sleepAmp4EnableCallback d = new SetnumericUpDown_sleepAmp4EnableCallback(SetnumericUpDown_sleepAmp4Enable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.numericUpDown_sleepAmp4.Enabled = b;
            }
        }

        delegate void SetnumericUpDown_sleepAmp4ValueCallback(Decimal val);
        public void SetnumericUpDown_sleepAmp4Value(Decimal val)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_sleepAmp4.InvokeRequired)
            {
                SetnumericUpDown_sleepAmp4ValueCallback d = new SetnumericUpDown_sleepAmp4ValueCallback(SetnumericUpDown_sleepAmp4Value);
                this.Invoke(d, new object[] { val });
            }
            else
            {
                this.numericUpDown_sleepAmp4.Value = val;
            }
        }

        delegate void SetnumericUpDown_preAmp1EnableCallback(bool b);
        public void SetnumericUpDown_preAmp1Enable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_preAmp1.InvokeRequired)
            {
                SetnumericUpDown_preAmp1EnableCallback d = new SetnumericUpDown_preAmp1EnableCallback(SetnumericUpDown_preAmp1Enable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.numericUpDown_preAmp1.Enabled = b;
            }
        }

        delegate void SetnumericUpDown_preAmp1ValueCallback(Decimal val);
        public void SetnumericUpDown_preAmp1Value(Decimal val)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_preAmp1.InvokeRequired)
            {
                SetnumericUpDown_preAmp1ValueCallback d = new SetnumericUpDown_preAmp1ValueCallback(SetnumericUpDown_preAmp1Value);
                this.Invoke(d, new object[] { val });
            }
            else
            {
                this.numericUpDown_preAmp1.Value = val;
            }
        }

        delegate void SetnumericUpDown_preAmp2EnableCallback(bool b);
        public void SetnumericUpDown_preAmp2Enable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_preAmp2.InvokeRequired)
            {
                SetnumericUpDown_preAmp2EnableCallback d = new SetnumericUpDown_preAmp2EnableCallback(SetnumericUpDown_preAmp2Enable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.numericUpDown_preAmp2.Enabled = b;
            }
        }

        delegate void SetnumericUpDown_preAmp2ValueCallback(Decimal val);
        public void SetnumericUpDown_preAmp2Value(Decimal val)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_preAmp2.InvokeRequired)
            {
                SetnumericUpDown_preAmp2ValueCallback d = new SetnumericUpDown_preAmp2ValueCallback(SetnumericUpDown_preAmp2Value);
                this.Invoke(d, new object[] { val });
            }
            else
            {
                this.numericUpDown_preAmp2.Value = val;
            }
        }

        delegate void SetnumericUpDown_preAmp3EnableCallback(bool b);
        public void SetnumericUpDown_preAmp3Enable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_preAmp3.InvokeRequired)
            {
                SetnumericUpDown_preAmp3EnableCallback d = new SetnumericUpDown_preAmp3EnableCallback(SetnumericUpDown_preAmp3Enable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.numericUpDown_preAmp3.Enabled = b;
            }
        }

        delegate void SetnumericUpDown_preAmp3ValueCallback(Decimal val);
        public void SetnumericUpDown_preAmp3Value(Decimal val)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_preAmp3.InvokeRequired)
            {
                SetnumericUpDown_preAmp3ValueCallback d = new SetnumericUpDown_preAmp3ValueCallback(SetnumericUpDown_preAmp3Value);
                this.Invoke(d, new object[] { val });
            }
            else
            {
                this.numericUpDown_preAmp3.Value = val;
            }
        }

        delegate void SetnumericUpDown_preAmp4EnableCallback(bool b);
        public void SetnumericUpDown_preAmp4Enable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_preAmp4.InvokeRequired)
            {
                SetnumericUpDown_preAmp4EnableCallback d = new SetnumericUpDown_preAmp4EnableCallback(SetnumericUpDown_preAmp4Enable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.numericUpDown_preAmp4.Enabled = b;
            }
        }

        delegate void SetnumericUpDown_preAmp4ValueCallback(Decimal val);
        public void SetnumericUpDown_preAmp4Value(Decimal val)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_preAmp4.InvokeRequired)
            {
                SetnumericUpDown_preAmp4ValueCallback d = new SetnumericUpDown_preAmp4ValueCallback(SetnumericUpDown_preAmp4Value);
                this.Invoke(d, new object[] { val });
            }
            else
            {
                this.numericUpDown_preAmp4.Value = val;
            }
        }

        delegate void SetnumericUpDown_seizureAmp1EnableCallback(bool b);
        public void SetnumericUpDown_seizureAmp1Enable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_seizureAmp1.InvokeRequired)
            {
                SetnumericUpDown_seizureAmp1EnableCallback d = new SetnumericUpDown_seizureAmp1EnableCallback(SetnumericUpDown_seizureAmp1Enable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.numericUpDown_seizureAmp1.Enabled = b;
            }
        }

        delegate void SetnumericUpDown_seizureAmp1ValueCallback(Decimal val);
        public void SetnumericUpDown_seizureAmp1Value(Decimal val)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_seizureAmp1.InvokeRequired)
            {
                SetnumericUpDown_seizureAmp1ValueCallback d = new SetnumericUpDown_seizureAmp1ValueCallback(SetnumericUpDown_seizureAmp1Value);
                this.Invoke(d, new object[] { val });
            }
            else
            {
                this.numericUpDown_seizureAmp1.Value = val;
            }
        }

        delegate void SetnumericUpDown_seizureAmp2EnableCallback(bool b);
        public void SetnumericUpDown_seizureAmp2Enable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_seizureAmp2.InvokeRequired)
            {
                SetnumericUpDown_seizureAmp2EnableCallback d = new SetnumericUpDown_seizureAmp2EnableCallback(SetnumericUpDown_seizureAmp2Enable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.numericUpDown_seizureAmp2.Enabled = b;
            }
        }

        delegate void SetnumericUpDown_seizureAmp2ValueCallback(Decimal val);
        public void SetnumericUpDown_seizureAmp2Value(Decimal val)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_seizureAmp2.InvokeRequired)
            {
                SetnumericUpDown_seizureAmp2ValueCallback d = new SetnumericUpDown_seizureAmp2ValueCallback(SetnumericUpDown_seizureAmp2Value);
                this.Invoke(d, new object[] { val });
            }
            else
            {
                this.numericUpDown_seizureAmp2.Value = val;
            }
        }

        delegate void SetnumericUpDown_seizureAmp3EnableCallback(bool b);
        public void SetnumericUpDown_seizureAmp3Enable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_seizureAmp3.InvokeRequired)
            {
                SetnumericUpDown_seizureAmp3EnableCallback d = new SetnumericUpDown_seizureAmp3EnableCallback(SetnumericUpDown_seizureAmp3Enable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.numericUpDown_seizureAmp3.Enabled = b;
            }
        }

        delegate void SetnumericUpDown_seizureAmp3ValueCallback(Decimal val);
        public void SetnumericUpDown_seizureAmp3Value(Decimal val)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_seizureAmp3.InvokeRequired)
            {
                SetnumericUpDown_seizureAmp3ValueCallback d = new SetnumericUpDown_seizureAmp3ValueCallback(SetnumericUpDown_seizureAmp3Value);
                this.Invoke(d, new object[] { val });
            }
            else
            {
                this.numericUpDown_seizureAmp3.Value = val;
            }
        }

        delegate void SetnumericUpDown_seizureAmp4EnableCallback(bool b);
        public void SetnumericUpDown_seizureAmp4Enable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_seizureAmp4.InvokeRequired)
            {
                SetnumericUpDown_seizureAmp4EnableCallback d = new SetnumericUpDown_seizureAmp4EnableCallback(SetnumericUpDown_seizureAmp4Enable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.numericUpDown_seizureAmp4.Enabled = b;
            }
        }

        delegate void SetnumericUpDown_seizureAmp4ValueCallback(Decimal val);
        public void SetnumericUpDown_seizureAmp4Value(Decimal val)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.numericUpDown_seizureAmp4.InvokeRequired)
            {
                SetnumericUpDown_seizureAmp4ValueCallback d = new SetnumericUpDown_seizureAmp4ValueCallback(SetnumericUpDown_seizureAmp4Value);
                this.Invoke(d, new object[] { val });
            }
            else
            {
                this.numericUpDown_seizureAmp4.Value = val;
            }
        }

        delegate void SetbtnApplyEnableCallback(bool b);
        public void SetbtnApplyEnable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.btnApply.InvokeRequired)
            {
                SetbtnApplyEnableCallback d = new SetbtnApplyEnableCallback(SetbtnApplyEnable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.btnApply.Enabled = b;
            }
        }

        #endregion
    }
}
