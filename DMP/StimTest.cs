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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DMP
{
    public partial class StimTest : Form
    {
        private System.Timers.Timer testTimer;
        private int secTest;

        private Boolean stopTest;
        private Boolean timerOn;

        private DMP_Main_MDIParent mdiParent;

        public StimTest(DMP_Main_MDIParent parent)
        {
            InitializeComponent();

            mdiParent = parent;
            secTest = 60 * 1/2;
        }

        private void StimTest_Load(object sender, EventArgs e)
        {
            stopTest = true;
            btnStop_testStim.Enabled = true;
            btnStart_testStim.Enabled = true;
            btnStart_testStim.Text = "Test Stimulation";
            btnStop_testStim.Text = "Close Window";

            testTimer = new System.Timers.Timer();
            testTimer.Elapsed += testTimer_Tick;
            testTimer.AutoReset = true;
            testTimer.Interval = secTest * 1000;
        }

        private void btnStart_testStim_Click(object sender, EventArgs e)
        {
            SettextBox1Text("*- Start Test -*");
            btnStop_testStim.Enabled = true;
            btnStart_testStim.Enabled = false;
            btnStop_testStim.Text = "Stop Test";
            btnStart_testStim.Text = "Running...";
            stopTest = false;

            mdiParent.SetDoing_stim = true;
            // Start the background worker
            backgroundWorker1.RunWorkerAsync();
        }


        private void btnStop_testStim_Click(object sender, EventArgs e)
        {
            if (!stopTest)
            {
                timerOn = false; // to stop current timer
                stopTest = true; // to avoid more settings tests
            }
            else // "Close Window"
            {
                testTimer.Dispose();
                mdiParent.SetDoing_stim = false;
                this.Close();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                int i = 0;
                timerOn = false;

                if (stopTest)
                    return;

                Console.WriteLine("Stimulation setting test started");

                SettextBox1Text("Setting to Default settings..");
                // set to default 
                new Thread(() => mdiParent.StartStreaming(1, mdiParent.GetStreamingMode, "Default")).Start();
                System.Threading.Thread.Sleep(1000);
                SettextBox1Text("Starting " + secTest + " of testing Default settings..");

                testTimer.Start();
                timerOn = true;
                i = 0;

                SetlabelCurrentText( "Testing: Default State");
                while (timerOn)
                {
                    System.Threading.Thread.Sleep(1000);
                    i++;
                    // Report progress to 'UI' thread
                    int progress = (i / secTest) * 100;
                    backgroundWorker1.ReportProgress(progress, i);
                }
                SetlabelCurrentText("");
                testTimer.Stop();

                SettextBox1Text("End of Testing Default settings..");

                if (stopTest)
                    return;

                System.Threading.Thread.Sleep(500);

                SettextBox1Text("Setting to Sleep settings..");
                // set to Sleep 
                new Thread(() => mdiParent.StartStreaming(1, mdiParent.GetStreamingMode, "Sleep")).Start();
                System.Threading.Thread.Sleep(1000);
                SettextBox1Text("Starting " + secTest + " of testing Sleep settings..");

                testTimer.Start();
                timerOn = true;
                i = 0;

                SetlabelCurrentText("Testing: Sleep State");
                while (timerOn)
                {
                    System.Threading.Thread.Sleep(1000);
                    i++;
                    // Report progress to 'UI' thread
                    int progress = (i / secTest) * 100;
                    backgroundWorker1.ReportProgress(progress, i);
                }
                SetlabelCurrentText( "");
                testTimer.Stop();

                SettextBox1Text("End of Testing Sleep settings..");

                if (stopTest)
                    return;

                System.Threading.Thread.Sleep(500);

                SettextBox1Text("Setting to Pre-seizure settings..");
                // set to Pre-seizure
                new Thread(() => mdiParent.StartStreaming(1, mdiParent.GetStreamingMode, "Pre")).Start();
                System.Threading.Thread.Sleep(1000);
                SettextBox1Text("Starting " + secTest + " of testing Pre-seizure settings..");

                testTimer.Start();
                timerOn = true;
                i = 0;

                SetlabelCurrentText("Testing: Pre-seizure State");
                while (timerOn)
                {
                    System.Threading.Thread.Sleep(1000);
                    i++;
                    // Report progress to 'UI' thread
                    int progress = (i / secTest) * 100;
                    backgroundWorker1.ReportProgress(progress, i);
                }
                SetlabelCurrentText( "");
                testTimer.Stop();

                SettextBox1Text("End of Testing Pre-seizure settings..");

                System.Threading.Thread.Sleep(500);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception during Stimulation test: " + ex.Message);
                stopTest = true;
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            /// updated from DoWork
            // progressBar1.Value = e.ProgressPercentage;
            int i = (int)e.UserState;
            SettimeLeftText((secTest - i).ToString() + " seconds left");
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!stopTest)
            {
                SettextBox1Text("*- Completed Test For All Settings -*");

                if (mdiParent.GetStimMode == 1)
                {
                    SettextBox1Text("Setting back to Default settings..");
                    // set to default 
                    new Thread(() => mdiParent.StartStreaming(1, mdiParent.GetStreamingMode, "Default")).Start();
                }
                else // stimMode == 0 
                {
                    DialogResult dialogResult = CustomMsgBox.Show("All stimulation settings were tested and verified!\n" +
                            "Please confirm starting Stimulation with curent Settings", "Stimulation Settings", "Start Stimulation", "Cancel");
                    if (dialogResult == DialogResult.Yes)
                    {
                        new Thread(() => mdiParent.StartStim(null)).Start();

                        SettimeLeftText("*- Applied Stimulation Settings. -*");
                        SettimeLeftText("*- Stimulation is ON -*");
                    }
                    else
                    {
                        SettextBox1Text("Turning off stimulation..");
                        // stim off
                        new Thread(() => mdiParent.StartStreaming(0, mdiParent.GetStreamingMode, "Off")).Start();
                    }
                }
            }
            else
            {
                SettextBox1Text("*- Stopped Test -*");

                if (mdiParent.GetStimMode == 1)
                {
                    SettextBox1Text("Setting back to Default settings..");
                    // set to default 
                    new Thread(() => mdiParent.StartStreaming(1, mdiParent.GetStreamingMode, "Default")).Start();
                }
                else // stimMode == 0 
                {
                        SettextBox1Text("Turning off stimulation..");
                        // stim off
                        new Thread(() => mdiParent.StartStreaming(0, mdiParent.GetStreamingMode, "Off")).Start();
                }
            }

            SettextBox1Text("*- Test Done -*");
            SettimeLeftText("*- Test Done -*");
            btnStop_testStim.Text = "Close Window";

            Console.WriteLine("Stimulation setting test ended");

        }

        private void testTimer_Tick(object sender, EventArgs e)
        {
            timerOn = false;
        }

        delegate void SettextBox1TextCallback(String t);
        public void SettextBox1Text(String t)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.textBox1.InvokeRequired)
            {
                SettextBox1TextCallback d = new SettextBox1TextCallback(SettextBox1Text);
                this.Invoke(d, new object[] { t });
            }
            else
            {
                this.textBox1.Text += t + Environment.NewLine;
                Console.WriteLine(t);
            }
        }

        delegate void SetlabelCurrentTextCallback(String t);
        public void SetlabelCurrentText(String t)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.labelCurrent.InvokeRequired)
            {
                SetlabelCurrentTextCallback d = new SetlabelCurrentTextCallback(SetlabelCurrentText);
                this.Invoke(d, new object[] { t });
            }
            else
            {
                this.labelCurrent.Text = t;
            }
        }

        delegate void SettimeLeftTextCallback(String t);
        public void SettimeLeftText(String t)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.timeLeft.InvokeRequired)
            {
                SettimeLeftTextCallback d = new SettimeLeftTextCallback(SettimeLeftText);
                this.Invoke(d, new object[] { t });
            }
            else
            {
                this.timeLeft.Text = t;
            }
        }
    }
}
