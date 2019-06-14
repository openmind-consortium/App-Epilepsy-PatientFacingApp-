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
using Medtronic.NeuroStim.Olympus.DataTypes.DeviceManagement;
using Medtronic.NeuroStim.Olympus.DataTypes.Therapy.Adaptive;
using Medtronic.NeuroStim.Olympus.DataTypes.Sensing;
using Medtronic.NeuroStim.Olympus.DataTypes.PowerManagement;
using System.Runtime.InteropServices;
using System.IO;
using System.Timers;
using Medtronic.NeuroStim.Olympus.DataTypes.Therapy;
using Medtronic.NeuroStim.Olympus.DataTypes.Sensing.Packets;

namespace DMP
{
    public partial class DisplayStatPhysicianForm : MyFormPage
    /// MyFormPage
    /// Form
    {
        // DateTime last_Connection_Time;
        static string patient_name;
        DateTime Implant_Date;
        ushort days_till_endofservice;
        string therapy_group;
        string therapy_status;
        string[,] values;
        public bool szfile_existflag = true;
        /// <summary>
        /// 
        /// Display Statistics Mode - 
        /// Upon invoking Display Statistics Mode the software shall display the status of available system components including INS battery status,
        /// CTM gateway battery status, tablet computer battery status, the currently executing therapy program, the current brain state classification(seizure, warning, sleep, or wake),
        /// the size of EEG and annotation data currently stored on the tablet computer, the current data telemetry state, and the current status of data transfer to the cloud repository.
        /// If the user is inactive for > 10 minutes, the software shall return to Home Mode.
        /// 
        /// </summary>
        /// 


        public DisplayStatPhysicianForm(DMP_Main_MDIParent parent)
        {
            InitializeComponent();

            this.pnl = panel1;

            this.MdiParent = parent;
        }

        #region Patient Data and Statistics
        //Directory size in Bytes.
        private static long GetDirectorySize(string folderPath)
        {
            //Folder size in Gb
            long folder_size = 0;
            //check if directory exists
            if (!Directory.Exists(folderPath))
                return folder_size;
            else
            {
                DirectoryInfo di = new DirectoryInfo(folderPath);
                folder_size = di.EnumerateFiles("*", SearchOption.AllDirectories).Sum(fi => fi.Length);
                return folder_size;
            }
        }
        private static long GetTextFileSize(string folderPath)
        {
            long file_size = 0;
            //check if directory exists
            if (!Directory.Exists(folderPath))
                return file_size;
            else
            {
                //DirectoryInfo di = new DirectoryInfo(folderPath);
                file_size = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories).Where(s => s.EndsWith(".txt") || s.EndsWith(".csv")).Sum(t => (new FileInfo(t).Length));
                return file_size;
                //return (di.EnumerateFiles("*", SearchOption.AllDirectories).Sum(fi => fi.Length))/(1024*1024*1024);
            }
        }
        /// <summary>
        /// Convert the Total  data occupied by the EPAD data and other files in Bytes, KB, MB, GB, TB, PB, EB
        /// 
        /// </summary>
        /// <param name="byteCount"></param>
        /// <returns></returns>
        static String BytesToString(long byteCount)
        {
            string[] suf = { " B", " KB", " MB", " GB", " TB", " PB", " EB" }; //Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }
        /// <summary>
        /// Read CSV file needed for display the statistics in physician mode.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public string[,] LoadCsv(string filename)
        {
            try
            {


                // Get the file's text.
                string whole_file = System.IO.File.ReadAllText(filename);

                // Split into lines.
                whole_file = whole_file.Replace('\n', '\r');
                string[] lines = whole_file.Split(new char[] { '\r' },
                    StringSplitOptions.RemoveEmptyEntries);

                // See how many rows and columns there are.
                int num_rows = lines.Length;
                int num_cols = lines[0].Split(',').Length;
                if (num_rows == 0 || num_cols == 0)
                {
                    Console.WriteLine("(" + ((DMP_Main_MDIParent)this.MdiParent).HomeFolder + Environment.MachineName + "/sznotes.csv) has empty rows/columns. Returning null.");
                    return null;
                }
                // Allocate the data array.
                string[,] values = new string[num_rows, num_cols];

                // Load the array.
                for (int r = 0; r < num_rows; r++)
                {
                    string[] line_r = lines[r].Split(',');
                    for (int c = 0; c < num_cols; c++)
                    {
                        values[r, c] = line_r[c];
                    }
                }


                return values;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// Function to display the statistics in physician mode.
        /// </summary>
        /// <param name="StatPhysForm"></param>

        public void Btn_PhysicianDataStatistics_Click(object sender, EventArgs e)
        {
            try
            {

                txtBox_PhysicianDataStatistics.Clear();
                AdaptiveSettings adaptiveSettings = new AdaptiveSettings();
                int counter_seiz = 0;
                int counter_aura = 0;
                int counter_med = 0;

                var filepath = ((DMP_Main_MDIParent)this.MdiParent).HomeFolder + Environment.MachineName + "/sznotes.csv";
                if (File.Exists(filepath))
                {
                    txtBox_PhysicianDataStatistics.Text = "Loading...";
                    values = LoadCsv(filepath);
                    szfile_existflag = true;
                }
                else
                {
                    Console.WriteLine("No seizure annotation file exists");
                    txtBox_PhysicianDataStatistics.Text = "No seizure annotation file found. ";
                    szfile_existflag = false;
                    //return;
                }
                if (values == null)
                {
                    txtBox_PhysicianDataStatistics.Text = "Seizure annotation file is empty.\nLoading...";
                    szfile_existflag = false;
                    // return;
                }

                if (szfile_existflag)
                {
                    int num_rows = values.GetUpperBound(0) + 1;
                    int num_cols = values.GetUpperBound(1) + 1;
                    for (int r = 0; r < num_rows; r++)
                    {
                        if (values[r, 3].Substring(0, 4) == "seiz")
                        {
                            counter_seiz = counter_seiz + 1;
                        }
                        else if (values[r, 3].Substring(0, 4) == "aura")
                        {
                            counter_aura = counter_aura + 1;
                        }
                        else if ((values[r, 3].Substring(0, 4) == "regu") || (values[r, 3].Substring(0, 4) == "supp"))
                        {
                            counter_med = counter_med + 1;
                        }
                    }
                }
                else
                {
                    counter_seiz = 0;
                    counter_aura = 0;
                    counter_med = 0;
                }

            String data_dir_dropbox = ((DMP_Main_MDIParent)this.MdiParent).HomeFolder;
            string New_data_dir;
            patient_name = ((DMP_Main_MDIParent)this.MdiParent).Patient_name; /*We use device serial number for patient name.*/
            if (patient_name == null)
            {
                txtBox_PhysicianDataStatistics.Text = "Patient Name Empty. Exiting.";
                patient_name = "not read from INS";
                return;

            }
            if (Directory.Exists(data_dir_dropbox))
            {
                New_data_dir = data_dir_dropbox + Environment.MachineName;
            }
            else
            {
                txtBox_PhysicianDataStatistics.Text = "Dropbox directory doesn't exist.";
                return;
            }


                DateTime currentDate = DateTime.Now;
                TimeOfDay d_time;
                DateTime? currentDeviceTime = new DateTime();
                double? battery_level_percent;
            if (((DMP_Main_MDIParent)this.MdiParent).GetCTMconnection && ((DMP_Main_MDIParent)this.MdiParent).GetINSconnection)
            { // if connected
                ((DMP_Main_MDIParent)this.MdiParent).TheSummit.ReadBatteryLevel(out BatteryStatusResult theBatteryStatus);
            	((DMP_Main_MDIParent)this.MdiParent).TheSummit.ReadDeviceTimestamp(out d_time);
                ((DMP_Main_MDIParent)this.MdiParent).TheSummit.ReadGeneralInfo(out GeneralInterrogateData theGeneralData);
                if (theGeneralData != null)
                {
                    Implant_Date = theGeneralData.ImplantDate.RealTime;
                    days_till_endofservice = theGeneralData.DaysUntilEos;
                    therapy_group = theGeneralData.TherapyStatusData.ActiveGroup.ToString();
                    therapy_status = theGeneralData.TherapyStatusData.TherapyStatus.ToString();
                }
                else
                {
                    Implant_Date = DateTime.Now;
                    days_till_endofservice = 0;
                    therapy_group = "N/A";
                    therapy_status = "N/A";

                }
                double tot_days = (currentDate - Implant_Date).Days;
                if (d_time != null)
                {
                    currentDeviceTime = d_time.RealTime;
                }
                else
                {
                    currentDeviceTime = null;
                }
                if (theBatteryStatus != null)
                {
                    battery_level_percent = theBatteryStatus.BatteryLevelPercent;
                }
                else
                {//some unknown number.
                    battery_level_percent = null;
                }

                string ld_0_detection_status;
                string ld_1_detection_status;
                if (((DMP_Main_MDIParent)this.MdiParent).GetIsStiming == 1)
                {
                    ((DMP_Main_MDIParent)this.MdiParent).TheSummit.ReadAdaptiveDetectionStatus(out DetectionAdaptivePacket theAdaptiveStatus);
                    if (theAdaptiveStatus != null)
                    {
                        ld_0_detection_status = theAdaptiveStatus.Ld0DetectionStatus.ToString();
                        ld_1_detection_status = theAdaptiveStatus.Ld1DetectionStatus.ToString();
                    }
                    else
                    {
                        ld_0_detection_status = "N/A";
                        ld_1_detection_status = "N/A";
                    }

                }
                else
                {
                    ld_0_detection_status = "N/A";
                    ld_1_detection_status = "N/A";
                }


                    txtBox_PhysicianDataStatistics.Clear();
                    txtBox_PhysicianDataStatistics.Text += "Implant Date: " + Implant_Date.ToString() + Environment.NewLine;
                    txtBox_PhysicianDataStatistics.Text += "Device Serial Number: " + patient_name + Environment.NewLine;
                    txtBox_PhysicianDataStatistics.Text += "Days Until End of Service: " + days_till_endofservice.ToString() + Environment.NewLine;
                    txtBox_PhysicianDataStatistics.Text += "Days Since Initialization: " + tot_days.ToString() + Environment.NewLine;
                    txtBox_PhysicianDataStatistics.Text += "Current Device Time: " + currentDeviceTime.ToString() + Environment.NewLine;
                    txtBox_PhysicianDataStatistics.Text += "Patient Noted Seizures: " + counter_seiz.ToString() + Environment.NewLine;
                    txtBox_PhysicianDataStatistics.Text += "Medical Doses Logged: " + counter_med.ToString() + Environment.NewLine;
                    txtBox_PhysicianDataStatistics.Text += "Storage Used on Tablet: " + BytesToString(GetDirectorySize(data_dir_dropbox)) + Environment.NewLine;
                    txtBox_PhysicianDataStatistics.Text += "iEEG Space Used: " + BytesToString(GetDirectorySize(New_data_dir)) + Environment.NewLine;
                    txtBox_PhysicianDataStatistics.Text += "Annotations and other Space Used: " + BytesToString(GetTextFileSize(data_dir_dropbox)) + Environment.NewLine;
                    txtBox_PhysicianDataStatistics.Text += "Therapy Status: " + therapy_status + Environment.NewLine;
                    txtBox_PhysicianDataStatistics.Text += "Therapy Group: " + therapy_group + Environment.NewLine;
                    txtBox_PhysicianDataStatistics.Text += "LD0 Detection Status: " + ld_0_detection_status + Environment.NewLine;
                    txtBox_PhysicianDataStatistics.Text += "LD1 Detection Status: " + ld_1_detection_status + Environment.NewLine;
                    txtBox_PhysicianDataStatistics.Text += "Battery Percentage: " + battery_level_percent.ToString() + "%" + Environment.NewLine;
                }
                else
                {
                    txtBox_PhysicianDataStatistics.Clear();
                    txtBox_PhysicianDataStatistics.Text = "Summit is not connected. Displaying basic Information." + Environment.NewLine;
                    txtBox_PhysicianDataStatistics.Text += "Patient Noted Seizures: " + counter_seiz.ToString() + Environment.NewLine;
                    txtBox_PhysicianDataStatistics.Text += "Medical Doses Logged: " + counter_med.ToString() + Environment.NewLine;
                    txtBox_PhysicianDataStatistics.Text += "Storage Used on Tablet: " + BytesToString(GetDirectorySize(data_dir_dropbox)) + Environment.NewLine;
                    txtBox_PhysicianDataStatistics.Text += "iEEG Space Used: " + BytesToString(GetDirectorySize(New_data_dir)) + Environment.NewLine;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nException during btn_PhysicianDataStatistics_Click() " + ex.Message);
            }
        }
        #endregion

    }
}
