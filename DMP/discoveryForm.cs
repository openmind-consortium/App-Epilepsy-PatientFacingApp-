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
    public partial class discoveryForm : MyFormPage
    /// MyFormPage
    /// Form
    {

        /// <summary>
        /// 
        /// Device Discovery Mode - 
        /// Upon initializing Device Discovery Mode, the software shall display a status screen showing
        /// the serial number and description of the INS device currently connected, if one is connected.
        /// 
        /// </summary>
        /// 


        public discoveryForm(DMP_Main_MDIParent parent)
        {
            InitializeComponent();
            this.pnl = panel1;

            this.MdiParent = parent;

            INSBattery_circularProgressBar.Value = 0;

            ((DMP_Main_MDIParent)this.MdiParent).PropertyChanged -= ConnectionChanges;
            ((DMP_Main_MDIParent)this.MdiParent).PropertyChanged += ConnectionChanges;
            ConnectionChanges(this, null);
        }

        // update UI base on application property changes
        public void ConnectionChanges(object sender, PropertyChangedEventArgs e)
        {

            SetINSnameText(((DMP_Main_MDIParent)this.MdiParent).Patient_name);

            // get levels from Home screen
            this.SetINSStatus(((DMP_Main_MDIParent)this.MdiParent).homeForm.INSStatusText.Text);
            this.SetINSStatusText(((DMP_Main_MDIParent)this.MdiParent).homeForm.INSStatusText.Text);

            this.SetINSBatteryText(((DMP_Main_MDIParent)this.MdiParent).homeForm.INSBatteryText.Text);
            this.SetINSBattery(((DMP_Main_MDIParent)this.MdiParent).homeForm.INSBattery_circularProgressBar.Value);


        }

        #region avoid cross-threading

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
                if (text.Equals("Not Connected"))
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

        delegate void SetINSnameTextCallback(string text);
        public void SetINSnameText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.INSname.InvokeRequired)
            {
                SetINSnameTextCallback d = new SetINSnameTextCallback(SetINSnameText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.INSname.Text = text;
            }
        }
        #endregion
    }
}
