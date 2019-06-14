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

namespace DMP
{
    public partial class SetSensingParamForm : MyFormPage
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

        // store pointer to parent form, because sensing form needs to call update function
        // on LDA form, which is done through the Physician parent form
        private Physician physician_parent_form;

        public SetSensingParamForm(DMP_Main_MDIParent parent, Physician parent_form)
        {
            InitializeComponent();

            channelMinusBox = new ComboBox[] { channelMinusBox0, channelMinusBox1, channelMinusBox2, channelMinusBox3 };
            channelPlusBox = new ComboBox[] { channelPlusBox0, channelPlusBox1, channelPlusBox2, channelPlusBox3 };

            this.pnl = panel1;

            this.MdiParent = parent;

            this.physician_parent_form = parent_form;

            // get current settings to update in GUI
            ((DMP_Main_MDIParent)this.MdiParent).ReadSettingsFile();
            ((DMP_Main_MDIParent)this.MdiParent).UpdateSensingSettingsFile(this);

            ((DMP_Main_MDIParent)this.MdiParent).PropertyChanged -= ConnectionChanges;
            ((DMP_Main_MDIParent)this.MdiParent).PropertyChanged += ConnectionChanges;
            ConnectionChanges(this, null);
        }

        // update UI base on application property changes
        private void ConnectionChanges(object sender, PropertyChangedEventArgs e)
        {
            if (!((DMP_Main_MDIParent)this.MdiParent).GetDoing_sensing)
            {
                if (((DMP_Main_MDIParent)this.MdiParent).GetIn_connect_to_ctm)
                {
                    SetbtnStart_SensingText("Connecting");
                    SetbtnStart_SensingEnable(false);
                    SetbtnStop_SensingEnable(true);
                }
                else
                {
                    if (((DMP_Main_MDIParent)this.MdiParent).GetCTMconnection && ((DMP_Main_MDIParent)this.MdiParent).GetINSconnection)
                    {// if connected
                        if (((DMP_Main_MDIParent)this.MdiParent).GetStreamingMode == 1)
                        {
                            SetbtnStart_SensingText("Sensing");
                            SetbtnStart_SensingEnable(false);
                            SetbtnStop_SensingEnable(true);
                        }
                    }
                    else
                    {
                        // lost connection
                        SetbtnStart_SensingText("Start Sensing");
                        SetbtnStop_SensingEnable(false);
                        SetbtnStart_SensingEnable(true);
                    }
                }
            }
        }

        // When Activated , Enter, Load and Shown
        public void SetSensingParamForm_Update(object sender, EventArgs e)
        {
            // get current settings to update in GUI
            ((DMP_Main_MDIParent)this.MdiParent).ReadSettingsFile();
            ((DMP_Main_MDIParent)this.MdiParent).UpdateSensingSettingsFile(this);

            ConnectionChanges(this, null);
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            // check if channel specifications are valid
            for (int i = 0; i < 4; i++)
            {
                if (channelMinusBox[i].Text.Equals(channelPlusBox[i].Text))
                {
                    String message_to_show = "Channel number " + i + " electrode specification is invalid, please fix and try again.";
                    CustomMsgBox.Show(message_to_show, "Error", "OK");
                    return;
                }
            }

            // write new setting file
            ((DMP_Main_MDIParent)this.MdiParent).WriteSettingsFile(this);

            System.Threading.Thread.Sleep(250);
            // get current settings to update in GUI
            ((DMP_Main_MDIParent)this.MdiParent).ReadSettingsFile();
            ((DMP_Main_MDIParent)this.MdiParent).UpdateSensingSettingsFile(this);

            System.Threading.Thread.Sleep(250);

            if (!((DMP_Main_MDIParent)this.MdiParent).GetCTMconnection ||
                !((DMP_Main_MDIParent)this.MdiParent).GetINSconnection)
            {
                CustomMsgBox.Show("Hardware is not connected.\nNew settings is will be applied when harware will connect successfuly!", "Error", "OK");
                ((DMP_Main_MDIParent)this.MdiParent).EndCurrentRecording(true);  // closes existing MEF files
            }
            else
            {
                SetbtnApplyEnable(false);

                new Thread(() =>
                {
                    ((DMP_Main_MDIParent)this.MdiParent).SetDoing_sensing = true;
                    ((DMP_Main_MDIParent)this.MdiParent).StartStreaming(0, 0, ((DMP_Main_MDIParent)this.MdiParent).CurrState);
                    System.Threading.Thread.Sleep(250);  // give streaming time to stop before closing MEF files
                    ((DMP_Main_MDIParent)this.MdiParent).EndCurrentRecording(true);  // closes existing MEF files
                    ((DMP_Main_MDIParent)this.MdiParent).StartStreaming(((DMP_Main_MDIParent)this.MdiParent).GetStimMode, ((DMP_Main_MDIParent)this.MdiParent).GetStreamingMode, ((DMP_Main_MDIParent)this.MdiParent).CurrState);
                    ((DMP_Main_MDIParent)this.MdiParent).SetDoing_sensing = false;
                }).Start();

                // update LDA form and re-write LDA settings files
                ((Physician)(this.physician_parent_form)).updateLDAform(null, null);

                SetbtnApplyEnable(true);
            }

        }

        private void btnStart_Sensing_Click(object sender, EventArgs e)
        {
            if (!((DMP_Main_MDIParent)this.MdiParent).GetCTMconnection ||
                !((DMP_Main_MDIParent)this.MdiParent).GetINSconnection)
            {
                CustomMsgBox.Show("Hardware is not connected.\nStart Sensing is currently not possible.\n\nConnect to Hardware via Program Control tab.", "Error", "OK");
            }
            else
            {
                SetbtnStart_SensingEnable(false);

                new Thread(() =>
                {
                    ((DMP_Main_MDIParent)this.MdiParent).SetStreamingMode = 1;
                    ((DMP_Main_MDIParent)this.MdiParent).SetDoing_sensing = true;
                    ((DMP_Main_MDIParent)this.MdiParent).StartStreaming(((DMP_Main_MDIParent)this.MdiParent).GetStimMode, ((DMP_Main_MDIParent)this.MdiParent).GetStreamingMode, ((DMP_Main_MDIParent)this.MdiParent).CurrState);
                    ((DMP_Main_MDIParent)this.MdiParent).SetDoing_sensing = false;
                }).Start();
            }
        }

        private void btnStop_Sensing_Click(object sender, EventArgs e)
        {
            SetbtnStop_SensingEnable(false);

            new Thread(() =>
            {
                ((DMP_Main_MDIParent)this.MdiParent).SetStreamingMode = 0;
            ((DMP_Main_MDIParent)this.MdiParent).SetDoing_sensing = true;
            ((DMP_Main_MDIParent)this.MdiParent).StartStreaming(((DMP_Main_MDIParent)this.MdiParent).GetStimMode, ((DMP_Main_MDIParent)this.MdiParent).GetStreamingMode, ((DMP_Main_MDIParent)this.MdiParent).CurrState);
            ((DMP_Main_MDIParent)this.MdiParent).SetDoing_sensing = false;
            }).Start();
        }


        private void enableAll(bool enable)
        {

            SetsamplingRateBoxEnable(enable);

            // channelMinusBox - array
            SetchannelMinusBox0Enable(enable);
            SetchannelMinusBox1Enable(enable);
            SetchannelMinusBox2Enable(enable);
            SetchannelMinusBox3Enable(enable);

            // channelPlusBox - array
            SetchannelPlusBox0Enable(enable);
            SetchannelPlusBox1Enable(enable);
            SetchannelPlusBox2Enable(enable);
            SetchannelPlusBox3Enable(enable);

            SetcontinuousButtonEnable(enable);
            SetperiodicButtonEnable(enable);
            SetloopRecordingInputBoxEnable(enable);
            SetloopBreakInputBoxEnable(enable);
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

        delegate void SetsamplingRateBoxEnableCallback(bool b);
        public void SetsamplingRateBoxEnable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.samplingRateBox.InvokeRequired)
            {
                SetsamplingRateBoxEnableCallback d = new SetsamplingRateBoxEnableCallback(SetsamplingRateBoxEnable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.samplingRateBox.Enabled = b;
            }
        }

        delegate void SetsamplingRateBoxTextCallback(string text);
        public void SetsamplingRateBoxText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.samplingRateBox.InvokeRequired)
            {
                SetsamplingRateBoxTextCallback d = new SetsamplingRateBoxTextCallback(SetsamplingRateBoxText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.samplingRateBox.Text = text;
            }
        }

        delegate void SetchannelMinusBox0EnableCallback(bool b);
        public void SetchannelMinusBox0Enable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.channelMinusBox[0].InvokeRequired)
            {
                SetchannelMinusBox0EnableCallback d = new SetchannelMinusBox0EnableCallback(SetchannelMinusBox0Enable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.channelMinusBox[0].Enabled = b;
            }
        }

        delegate void SetchannelMinusBox0TextCallback(string text);
        public void SetchannelMinusBox0Text(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.channelMinusBox[0].InvokeRequired)
            {
                SetchannelMinusBox0TextCallback d = new SetchannelMinusBox0TextCallback(SetchannelMinusBox0Text);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.channelMinusBox[0].Text = text;
            }
        }

        delegate void SetchannelMinusBox1EnableCallback(bool b);
        public void SetchannelMinusBox1Enable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.channelMinusBox[1].InvokeRequired)
            {
                SetchannelMinusBox1EnableCallback d = new SetchannelMinusBox1EnableCallback(SetchannelMinusBox1Enable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.channelMinusBox[1].Enabled = b;
            }
        }

        delegate void SetchannelMinusBox1TextCallback(string text);
        public void SetchannelMinusBox1Text(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.channelMinusBox[1].InvokeRequired)
            {
                SetchannelMinusBox1TextCallback d = new SetchannelMinusBox1TextCallback(SetchannelMinusBox1Text);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.channelMinusBox[1].Text = text;
            }
        }

        delegate void SetchannelMinusBox2EnableCallback(bool b);
        public void SetchannelMinusBox2Enable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.channelMinusBox[2].InvokeRequired)
            {
                SetchannelMinusBox2EnableCallback d = new SetchannelMinusBox2EnableCallback(SetchannelMinusBox2Enable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.channelMinusBox[2].Enabled = b;
            }
        }

        delegate void SetchannelMinusBox2TextCallback(string text);
        public void SetchannelMinusBox2Text(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.channelMinusBox[2].InvokeRequired)
            {
                SetchannelMinusBox2TextCallback d = new SetchannelMinusBox2TextCallback(SetchannelMinusBox2Text);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.channelMinusBox[2].Text = text;
            }
        }

        delegate void SetchannelMinusBox3EnableCallback(bool b);
        public void SetchannelMinusBox3Enable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.channelMinusBox[3].InvokeRequired)
            {
                SetchannelMinusBox3EnableCallback d = new SetchannelMinusBox3EnableCallback(SetchannelMinusBox3Enable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.channelMinusBox[3].Enabled = b;
            }
        }

        delegate void SetchannelMinusBox3TextCallback(string text);
        public void SetchannelMinusBox3Text(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.channelMinusBox[3].InvokeRequired)
            {
                SetchannelMinusBox3TextCallback d = new SetchannelMinusBox3TextCallback(SetchannelMinusBox3Text);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.channelMinusBox[3].Text = text;
            }
        }

        delegate void SetchannelPlusBox0EnableCallback(bool b);
        public void SetchannelPlusBox0Enable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.channelPlusBox[0].InvokeRequired)
            {
                SetchannelPlusBox0EnableCallback d = new SetchannelPlusBox0EnableCallback(SetchannelPlusBox0Enable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.channelPlusBox[0].Enabled = b;
            }
        }

        delegate void SetchannelPlusBox0TextCallback(string text);
        public void SetchannelPlusBox0Text(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.channelPlusBox[0].InvokeRequired)
            {
                SetchannelPlusBox0TextCallback d = new SetchannelPlusBox0TextCallback(SetchannelPlusBox0Text);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.channelPlusBox[0].Text = text;
            }
        }

        delegate void SetchannelPlusBox1EnableCallback(bool b);
        public void SetchannelPlusBox1Enable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.channelPlusBox[1].InvokeRequired)
            {
                SetchannelPlusBox1EnableCallback d = new SetchannelPlusBox1EnableCallback(SetchannelPlusBox1Enable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.channelPlusBox[1].Enabled = b;
            }
        }

        delegate void SetchannelPlusBox1TextCallback(string text);
        public void SetchannelPlusBox1Text(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.channelPlusBox[1].InvokeRequired)
            {
                SetchannelPlusBox1TextCallback d = new SetchannelPlusBox1TextCallback(SetchannelPlusBox1Text);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.channelPlusBox[1].Text = text;
            }
        }

        delegate void SetchannelPlusBox2EnableCallback(bool b);
        public void SetchannelPlusBox2Enable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.channelPlusBox[2].InvokeRequired)
            {
                SetchannelPlusBox2EnableCallback d = new SetchannelPlusBox2EnableCallback(SetchannelPlusBox2Enable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.channelPlusBox[2].Enabled = b;
            }
        }

        delegate void SetchannelPlusBox2TextCallback(string text);
        public void SetchannelPlusBox2Text(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.channelPlusBox[2].InvokeRequired)
            {
                SetchannelPlusBox2TextCallback d = new SetchannelPlusBox2TextCallback(SetchannelPlusBox2Text);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.channelPlusBox[2].Text = text;
            }
        }

        delegate void SetchannelPlusBox3EnableCallback(bool b);
        public void SetchannelPlusBox3Enable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.channelPlusBox[3].InvokeRequired)
            {
                SetchannelPlusBox3EnableCallback d = new SetchannelPlusBox3EnableCallback(SetchannelPlusBox3Enable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.channelPlusBox[3].Enabled = b;
            }
        }

        delegate void SetchannelPlusBox3TextCallback(string text);
        public void SetchannelPlusBox3Text(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.channelPlusBox[3].InvokeRequired)
            {
                SetchannelPlusBox3TextCallback d = new SetchannelPlusBox3TextCallback(SetchannelPlusBox3Text);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.channelPlusBox[3].Text = text;
            }
        }

        delegate void SetcontinuousButtonEnableCallback(bool b);
        public void SetcontinuousButtonEnable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.continuousButton.InvokeRequired)
            {
                SetcontinuousButtonEnableCallback d = new SetcontinuousButtonEnableCallback(SetcontinuousButtonEnable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.continuousButton.Enabled = b;
            }
        }


        delegate void SetperiodicButtonEnableCallback(bool b);
        public void SetperiodicButtonEnable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.periodicButton.InvokeRequired)
            {
                SetperiodicButtonEnableCallback d = new SetperiodicButtonEnableCallback(SetperiodicButtonEnable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.periodicButton.Enabled = b;
            }
        }

        delegate void SetloopRecordingInputBoxEnableCallback(bool b);
        public void SetloopRecordingInputBoxEnable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.loopRecordingInputBox.InvokeRequired)
            {
                SetloopRecordingInputBoxEnableCallback d = new SetloopRecordingInputBoxEnableCallback(SetloopRecordingInputBoxEnable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.loopRecordingInputBox.Enabled = b;
            }
        }

        delegate void SetloopRecordingInputBoxTextCallback(string text);
        public void SetloopRecordingInputBoxText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.loopRecordingInputBox.InvokeRequired)
            {
                SetloopRecordingInputBoxTextCallback d = new SetloopRecordingInputBoxTextCallback(SetloopRecordingInputBoxText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.loopRecordingInputBox.Text = text;
            }
        }

        delegate void SetloopBreakInputBoxEnableCallback(bool b);
        public void SetloopBreakInputBoxEnable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.loopBreakInputBox.InvokeRequired)
            {
                SetloopBreakInputBoxEnableCallback d = new SetloopBreakInputBoxEnableCallback(SetloopBreakInputBoxEnable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.loopBreakInputBox.Enabled = b;
            }
        }

        delegate void SetloopBreakInputBoxTextCallback(string text);
        public void SetloopBreakInputBoxText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.loopBreakInputBox.InvokeRequired)
            {
                SetloopBreakInputBoxTextCallback d = new SetloopBreakInputBoxTextCallback(SetloopBreakInputBoxText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.loopBreakInputBox.Text = text;
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

        delegate void SetbtnStart_SensingTextCallback(string text);
        public void SetbtnStart_SensingText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.btnStart_Sensing.InvokeRequired)
            {
                SetbtnStart_SensingTextCallback d = new SetbtnStart_SensingTextCallback(SetbtnStart_SensingText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.btnStart_Sensing.Text = text;
            }
        }

        delegate void SetbtnStart_SensingEnableCallback(bool b);
        public void SetbtnStart_SensingEnable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.btnStart_Sensing.InvokeRequired)
            {
                SetbtnStart_SensingEnableCallback d = new SetbtnStart_SensingEnableCallback(SetbtnStart_SensingEnable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.btnStart_Sensing.Enabled = b;
            }
        }

        delegate void SetbtnStop_SensingEnableCallback(bool b);
        public void SetbtnStop_SensingEnable(bool b)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.btnStop_Sensing.InvokeRequired)
            {
                SetbtnStop_SensingEnableCallback d = new SetbtnStop_SensingEnableCallback(SetbtnStop_SensingEnable);
                this.Invoke(d, new object[] { b });
            }
            else
            {
                this.btnStop_Sensing.Enabled = b;
            }
        }

        #endregion

        private void InputBox_Enter(object sender, EventArgs e)
        {
                ((DMP_Main_MDIParent)this.MdiParent).ToggleOSK(true);
        }

        private void InputBox_Leave(object sender, EventArgs e)
        {
                ((DMP_Main_MDIParent)this.MdiParent).ToggleOSK(false);
        }

        private void InputBox_Validating(object sender, CancelEventArgs e)
        {
            TextBox currentb = (TextBox)sender;

            if (currentb.Text == "")
            {
                CustomMsgBox.Show("Field can not be empty", "Error", "OK");
                e.Cancel = true;
            }
            else if (!(int.TryParse(currentb.Text,out int res)))
            {
                CustomMsgBox.Show("Field should be a number", "Error", "OK");
                e.Cancel = true;
            }
            else
                e.Cancel = false;

        }
    }
}
