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

namespace DMP
{
    /// <summary>
    /// 
    /// 4.1.4 Physician Mode
    /// 
    /// Requirement 4.1.8: Entry to Physician Mode
    /// When entering Physician Mode, the software shall display a Menu Dialog with functional choices
    /// appropriate for the managing physician to access.The Physician-screen shall display current battery levels,
    /// device ID and software version information, and shall present a control for input of physician annotations.
    /// 
    /// Requirement 4.1.9: Exit from Physician Mode
    /// If the user elects to view and/or edit acquisition and stimulation parameters,
    /// the software enters Set Parameter Mode. If the user elects to view performance statistics the program enters
    /// Display Statistics Mode. If the user chooses to check electrode impedances, the software enters Check Impedance mode.
    /// If the user elects to synchronize data and annotations with the remote cloud server the program
    /// enters Synchronize Data Mode. If the user elects to power-down the system the software shall initiate
    /// power-down of the system. If the user is inactive for > 10 minutes, the software shall return to Home Mode.
    /// 
    /// </summary>
    public partial class Physician : Form
    {
        bool addTabs;

        InactiveTimeRetriever _inactiveTimeRetriever;

        public Physician()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.WindowState = FormWindowState.Normal;

            addTabs = false;

            _inactiveTimeRetriever = new InactiveTimeRetriever();

        }

        public void Physician_Load(object sender, EventArgs e)
        {
            // set mode everytime the form is loaded
            ConfigurationManager.AppSettings["Mode"] = "Physician";
            Mode.Text = ConfigurationManager.AppSettings["Mode"];

            InactivityTimer.Enabled = true;

            if (!addTabs)
            {
                physicianTabs.TabPages.Add(new MyTabPage(new SetSensingParamForm((DMP_Main_MDIParent)this.MdiParent, this)));
                physicianTabs.TabPages.Add(new MyTabPage(new ConfigureLDAForm((DMP_Main_MDIParent)this.MdiParent)));
                physicianTabs.TabPages.Add(new MyTabPage(new StimulationParamForm((DMP_Main_MDIParent)this.MdiParent)));
                physicianTabs.TabPages.Add(new MyTabPage(new checkImpForm((DMP_Main_MDIParent)this.MdiParent)));
                physicianTabs.TabPages.Add(new MyTabPage(new DisplayStatPhysicianForm((DMP_Main_MDIParent)this.MdiParent)));
                physicianTabs.TabPages.Add(new MyTabPage(new ProgramControlForm((DMP_Main_MDIParent)this.MdiParent)));
                addTabs = true;
            }
        }

        private void Home_button_Click(object sender, EventArgs e)
        {
            ///Run Home Screen
            foreach (Form childForm in this.MdiParent.MdiChildren)
            {
                if (childForm.Name == "Home")
                    childForm.Show();
            }
            this.Hide();
        }

        private void Physician_FormClosing(object sender, FormClosingEventArgs e)
        {
            ///Run Home Screen
            foreach (Form childForm in this.MdiParent.MdiChildren)
            {
                if (childForm.Name == "Home")
                    childForm.Show();
            }
        }

        private void InactivityTimer_Tick(object sender, EventArgs e)
        {
            var inactiveTime = _inactiveTimeRetriever.GetInactiveTime();
            if (inactiveTime == null)
            {
                IndicatorLabel.Visible = false;
                // IndicatorLabel.Text = "Unknown";
                // IndicatorLabel.BackColor = Color.Yellow;
            }
            else if (inactiveTime.Value.TotalSeconds > 5)
            {
                IndicatorLabel.Visible = true;
                IndicatorLabel.Text = string.Format("Inactive for {0}s", (int)inactiveTime.Value.TotalSeconds);

                if (inactiveTime.Value.TotalSeconds > 60 * 10)
                { // 10 min
                  ///Run Home Screen
                    foreach (Form childForm in this.MdiParent.MdiChildren)
                    {
                        if (childForm.Name == "Home")
                            childForm.Show();
                    }
                    this.Hide();
                    InactivityTimer.Enabled = false;
                }
            }
            else
            {
                IndicatorLabel.Visible = false;
                // IndicatorLabel.Text = "Active";
                // IndicatorLabel.BackColor = Color.MediumBlue;
            }
        }

        public void updateLDAform(object sender, EventArgs e)
        {
            foreach (Form childForm in this.MdiParent.MdiChildren)
            {
                if (childForm.Name == "ConfigureLDAForm")
                {
                    ((ConfigureLDAForm)childForm).AutomaticConfigureLDAUIpdate(null, null);
                    ((DMP_Main_MDIParent)this.MdiParent).WriteLDASettingsFile((ConfigureLDAForm)childForm);
                }
            }

        }

        private void physicianTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Form childForm in this.MdiParent.MdiChildren)
            {
                if (((MyFormPage)((MyTabPage)physicianTabs.SelectedTab).frm).Name == childForm.Name)
                {
                    if (childForm.Name == "StimulationParamForm")
                    {
                        ((StimulationParamForm)childForm).StimulationParamForm_Update(null, null);
                    }

                    if (childForm.Name == "SetSensingParamForm")
                    {
                        ((SetSensingParamForm)childForm).SetSensingParamForm_Update(null, null);
                    }

                    if (childForm.Name == "DisplayStatPhysicianForm")
                    {
                        ((DisplayStatPhysicianForm)childForm).txtBox_PhysicianDataStatistics.Text = "Loading...";
                        ((DisplayStatPhysicianForm)childForm).Btn_PhysicianDataStatistics_Click(null, null);
                    }

                    if (childForm.Name == "ConfigureLDAForm")
                    {
                        // add update function call
                        ((ConfigureLDAForm)childForm).AutomaticConfigureLDAUIpdate(null, null);
                    }
                }
            }           
        }
    }
}
