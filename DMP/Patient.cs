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
    public partial class Patient : Form
    {

        /// <summary>
        /// 
        /// 4.1.3 Patient Mode
        /// 
        /// Requirement 4.1.6: Entry to Patient Mode
        /// When entering Patient Mode, the software shall display buttons to enter annotations,
        /// including medication doses and notes, and a Menu Dialog with Patient Mode functional choices.
        /// The Patient-screen shall also display current battery levels, device ID and software version information.
        /// 
        /// Requirement 4.1.7: Exit from Patient Mode
        /// If the user selects Discover Device, the software shall enter the Discover Device Mode.
        /// If the user selects Display Statistics, the software shall enter the Display Statistics Mode.
        /// If the user elects to enter Physician Mode the program will prompt the user for the Physician password,
        /// and if validated shall initiate Physician Mode.If the user elects to view the
        /// seizure diary the program will enter View Diary mode. If the user is inactive for > 10 minutes,
        /// the software shall return to Home Mode.
        /// 
        /// </summary>

        bool addTabs;

        InactiveTimeRetriever _inactiveTimeRetriever;

        public Patient()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.WindowState = FormWindowState.Normal;

            addTabs = false;

            _inactiveTimeRetriever = new InactiveTimeRetriever();

        }

        public void Patient_Load(object sender, EventArgs e)
        {
            // set mode everytime the form is loaded
            ConfigurationManager.AppSettings["Mode"] = "Patient";
            Mode.Text = ConfigurationManager.AppSettings["Mode"];

            InactivityTimer.Enabled = true;

            if (!addTabs)
            {
                patientTabs.TabPages.Add(new MyTabPage(new notesForm((DMP_Main_MDIParent)this.MdiParent)));
                patientTabs.TabPages.Add(new MyTabPage(new discoveryForm((DMP_Main_MDIParent)this.MdiParent)));

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

        private void Patient_FormClosing(object sender, FormClosingEventArgs e)
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


        private void patientTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Form childForm in this.MdiParent.MdiChildren)
            {
                if (((MyFormPage)((MyTabPage)patientTabs.SelectedTab).frm).Name == childForm.Name)
                {

                    if (childForm.Name == "discoveryForm")
                    {
                        ((discoveryForm)childForm).ConnectionChanges(null, null);
                    }
                }
            }
        }
    }
}
