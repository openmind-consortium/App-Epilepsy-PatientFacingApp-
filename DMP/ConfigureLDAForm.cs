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

namespace DMP
{

    public partial class ConfigureLDAForm : MyFormPage
    /// MyFormPage
    /// Form  
    {
        public ConfigureLDAForm(DMP_Main_MDIParent parent)
        {
            InitializeComponent();
            this.pnl = panel1;
            this.MdiParent = parent;
            ((DMP_Main_MDIParent)this.MdiParent).ReadLDAConfigurationFile();
            ((DMP_Main_MDIParent)this.MdiParent).Write_LDA_UIValues(this);//populate the UI with the LDA values every time the form is open.
            if (!((DMP_Main_MDIParent)this.MdiParent).GetCTMconnection || !((DMP_Main_MDIParent)this.MdiParent).GetINSconnection)
            {               
                btn_ConfigurationLDA.Enabled = false;
                lbl_LD_ConfigStatus.ForeColor = Color.Red;
                lbl_LD_ConfigStatus.Text = "Summit is either not connected or in connection process.";
            }
            else
            {
                btn_ConfigurationLDA.Enabled = true;               
                lbl_LD_ConfigStatus.Text = "Summit is connected. You can apply settings.";
            }
        }

        private void btn_ConfigurationLDA_Click(object sender, EventArgs e)
        {                        
            btn_ConfigurationLDA.Text = "Applying..";
            btn_ConfigurationLDA.Enabled = false;
            //First check if the channels selected are different. If same, then show the message and stop implementing the rest of the code.                      
            lbl_LD_ConfigStatus.Text = "";
            if (cmb_Input1_channels.SelectedIndex == cmb_Input2_channels.SelectedIndex)
            {                
                lbl_LD_ConfigStatus.ForeColor = Color.Red;
                CustomMsgBox.Show("Input channels must be different.\n"+
                    "Please select different channel for Input 2.","Error","OK");
                lbl_LD_ConfigStatus.Text = "Apply Settings Failed";              
                btn_ConfigurationLDA.Text = "Apply Settings";               
                btn_ConfigurationLDA.Enabled = true;               
                return;
            }
            //To restrict the user from passing lower frequncies greater than higher frequencies. 
            if ((numericUpDownInput1LowBand1.Value >= numericUpDownInput1HighBand1.Value) || (numericUpDownInput1LowBand2.Value >= numericUpDownInput1HighBand2.Value) ||
                (numericUpDownInput2LowBand1.Value >= numericUpDownInput2HighBand1.Value) || (numericUpDownInput2LowBand2.Value >= numericUpDownInput2HighBand2.Value))
            {                
                lbl_LD_ConfigStatus.ForeColor = Color.Red;
                CustomMsgBox.Show("High limits for frequency bands must be greater than low limits.\n"
                    +"Please check frequency settings and try again.","Error","OK");               
                lbl_LD_ConfigStatus.Text = "Apply Settings Failed";               
                btn_ConfigurationLDA.Text = "Apply Settings";               
                btn_ConfigurationLDA.Enabled = true;
                return;
            }
            if (chkBox_SaveFftData.Checked)
            {
                ((DMP_Main_MDIParent)this.MdiParent).fft_check_status = true;
            }
            else
            {
                ((DMP_Main_MDIParent)this.MdiParent).fft_check_status = false;
            }
            var time = DateTime.Now;
            string formattedTime = time.ToString("yyyy/MM/dd hh:mm:ss tt");
            ((DMP_Main_MDIParent)this.MdiParent).SetDoing_LDA_config = true; //Started LDA configuration..Change flag to true.
            ((DMP_Main_MDIParent)this.MdiParent).StartStreaming(0, 0, "Off"); // stop all
            ((DMP_Main_MDIParent)this.MdiParent).CopyLDAConfigurationFromUI(this);           
            ((DMP_Main_MDIParent)this.MdiParent).WriteLDASettingsFile(this);
            System.Threading.Thread.Sleep(250);
            ((DMP_Main_MDIParent)this.MdiParent).ReadLDAConfigurationFile();
            ((DMP_Main_MDIParent)this.MdiParent).StartStreaming(((DMP_Main_MDIParent)this.MdiParent).GetStimMode, ((DMP_Main_MDIParent)this.MdiParent).GetStreamingMode, ((DMP_Main_MDIParent)this.MdiParent).CurrState);
            ((DMP_Main_MDIParent)this.MdiParent).SetDoing_LDA_config = false; //finished the LDA configuration. Change flag to false.
            ((DMP_Main_MDIParent)this.MdiParent).UpdateBands_LDA(this);
            // write settings file again, this time with updated bands
            ((DMP_Main_MDIParent)this.MdiParent).WriteLDASettingsFile(this);
            System.Threading.Thread.Sleep(250);
            lbl_LD_ConfigStatus.ForeColor = Color.Blue;
            lbl_LD_ConfigStatus.Text = "Settings applied successfully at: "+formattedTime;
            btn_ConfigurationLDA.Text = "Apply Settings";
            btn_ConfigurationLDA.Enabled = true;
        }
        public void AutomaticConfigureLDAUIpdate(object sender, EventArgs e)
        {
            cmb_Input1_channels.Items.Clear();
            cmb_Input2_channels.Items.Clear();
            ((DMP_Main_MDIParent)this.MdiParent).ReadLDAConfigurationFile();
            ((DMP_Main_MDIParent)this.MdiParent).Write_LDA_UIValues(this);
            ((DMP_Main_MDIParent)this.MdiParent).UpdateBands_LDA(this);
            if (!((DMP_Main_MDIParent)this.MdiParent).GetCTMconnection || !((DMP_Main_MDIParent)this.MdiParent).GetINSconnection)
            {
                btn_ConfigurationLDA.Enabled = false;
                lbl_LD_ConfigStatus.ForeColor = Color.Red;
                lbl_LD_ConfigStatus.Text = "Summit is either not connected or in connection process.";
            }
            else
            {
                btn_ConfigurationLDA.Enabled = true;
                lbl_LD_ConfigStatus.ForeColor = Color.Black;
                lbl_LD_ConfigStatus.Text = "Summit is connected. You can apply settings.";
            }
        }


        private void numericUpDown_Validating(object sender, CancelEventArgs e)
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

    }
}
