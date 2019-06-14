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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DMP
{
    public partial class AddMed : Form
    {
        bool SupMed;

        public AddMed(bool sup)
        {
            InitializeComponent();
            SupMed = sup;
        }

        private void AddMed_Load(object sender, EventArgs e)
        {
            try
            {
				// look for patient medication list in order to create the dropdown list items
                String filename = ((DMP_Main_MDIParent)this.MdiParent).HomeFolder + Environment.MachineName + ((DMP_Main_MDIParent)this.MdiParent).Patient_name + "_medicationList.txt";
                if (!File.Exists(filename))
                {
                    Console.WriteLine("Patient medication list file " + filename + " does not exist");
                }
                else
                {
                    // read patient medication list and add items to dropdown list
                    System.IO.StreamReader file = new StreamReader(filename);
                    String line;
                    while (!file.EndOfStream)
                    {
                        line = file.ReadLine();
                        comboBox_Medication.Items.Add(line);
                    }
                    file.Close();
                }
            }
            catch
            {
                // if somthing went wrong when reading patient medication list - clear all items
                Console.WriteLine("Exception during reading patient medication list.");
                comboBox_Medication.Items.Clear();
            }

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // input check
                if (comboBox_Medication.Text.Equals("Medication") || comboBox_Medication.Text.Equals("") ||
                    comboBox_Dosage.Text.Equals("Dosage"))
                {
                    CustomMsgBox.Show("Please select Medication and Dosage!", "Error", "OK");
                    return;
                }

                // write to annotations file
                string file_name_sz = ((DMP_Main_MDIParent)this.MdiParent).HomeFolder + Environment.MachineName + "\\sznotes.csv";
                StreamWriter sw;
                if (File.Exists(file_name_sz))
                {
                    sw = new StreamWriter(file_name_sz, true);
                }
                else
                {
                    sw = new StreamWriter(file_name_sz, true);
                    sw.WriteLine("start_time, end_time, channel, Event");
                }
                Int64 unixTimestamp = (Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
                String medType = SupMed ? "sm" : "rm";
                string medType_name;
                if (medType == "sm")
                {
                    medType_name = "supplemental medication";
                }
                else
                {
                    medType_name = "regular medication";
                }
                string txt1 = unixTimestamp.ToString() + "," + "," + "," + medType_name + ": " + comboBox_Medication.Text + " " + comboBox_Dosage.Text;
                sw.WriteLine(txt1); //write the text.

                sw.Close();
                CustomMsgBox.Show("Logged succesfuly!", "", "OK");
            }
            catch (Exception ex)
            {
                CustomMsgBox.Show("Error while logging...\n" + ex.Message, "Error", "OK");
            }

        }


        private void BtnCancel_Click(object sender, EventArgs e)
        {
            ((DMP_Main_MDIParent)this.MdiParent).ToggleOSK(false);
            this.Close();
        }

        private void ComboBox_Medication_Enter(object sender, EventArgs e)
        {
            ((DMP_Main_MDIParent)this.MdiParent).ToggleOSK(true);
        }

        private void ComboBox_Medication_Leave(object sender, EventArgs e)
        {
            ((DMP_Main_MDIParent)this.MdiParent).ToggleOSK(false);
        }
    }
}