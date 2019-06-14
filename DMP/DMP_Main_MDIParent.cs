/* Copyright (c) 2017-2018, Mayo Foundation for Medical Education and Research (MFMER), All rights reserved. 
Academic non-commercial use of this software is allowed with expressed permission of the developers. 
MFMER disclaims all implied warranties of merchantability and fitness for a particular purpose with 
respect to this software, its application, and any verbal or written statements regarding its use. 
The software may not be distributed to third parties without consent of MFMER. Use of this software 
constitutes acceptance of these terms. 
Contributors: Daniel Crepeau, Tal Pal Attia, Jan Cimbalnik, Hari Guragain, Mona Nasseri, Vaclav Kremen, 
Benjamin Brinkmann, Matt Stead, Gregory Worrell.  */

using System;
using Medtronic.NeuroStim.Olympus.DataTypes.Measurement;
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
using System.Windows.Controls;
using SafeControls;
using System.Net.NetworkInformation;
using Medtronic.NeuroStim.Olympus.DataTypes.Sensing.Packets;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Collections;
using Microsoft.VisualBasic;
using System.ServiceProcess;

namespace DMP
{
    public partial class DMP_Main_MDIParent : Form, INotifyPropertyChanged
    {

        #region global Variables

        static string homeFolder = "c:/Dropbox/";
        static string summitProjectID = "MayoSeizureDogsRCS";

        #endregion

        #region Impedance Variables

        // public static string INS_Name; // replaced with - ((DMP_Main_MDIParent)this.MdiParent).Patient_name
        static int max_channels_impedance = 16;

        #endregion

        #region LDA variables

        const int vector_size = 4;    //This is for the channel names which is 4 for now, weight vectors, normalization vector etc.     
        public static double[] Input1;
        public static double[] Input2;
        public static double[] Input1_Corrected;
        public static double[] Input2_Corrected;
        //Linear discriminator configuration.
        LinearDiscriminantConfiguration configLd0 = new LinearDiscriminantConfiguration();
        LinearDiscriminantConfiguration configLd1 = new LinearDiscriminantConfiguration();
        //LinearDiscriminantStatus ldstatus = new LinearDiscriminantStatus();
        public ushort LD0_Onset_Duration;
        public ushort LD0_Termination_Duration;
        public ushort LD0_HoldOff_Time;
        public ushort LD0_Blanking_Duration_Upon_StateChange;
        public ushort LD0_UpdateRate;
        public int LDA_configuration;
        public uint LDA_Threshold;
        public uint[] Weight_Vector;
        public byte LD0_Fractional_FixedPointValue;
        public uint[] Normalization_Multiply_Vector;
        public uint[] Normalization_Subtract_Vector;
        public static int[] input_channel_index = new int[2];
        public bool fft_check_status;
        static bool doing_lda_config;
        static bool lda_file_exists;
        const int FFT_Size = 1024;//We decided to use the maximum FFT size. This won't change through out the code. 
        public string[] ch_names = new string[vector_size];
        long directory_size;
        long previous_directory_size;
        int previous_timer_day;

        #endregion

        #region recording Video Variables

        static System.Timers.Timer LDAstatusTimer;

        string txtFilename;
        private System.Diagnostics.Process video_process = null;
        int record_again_after_done = 0;

        static System.Timers.Timer Videotimer;
        int milltorec = 60000 * 2;
        int timeleft;

        DetectionOutputStatus lastDetectionOutputStatus;
        byte lastAdaptiveState;

        #endregion

        #region Stimulation Variables

        public StimulationParamForm stimParamForm;
        String currState;                                                   // current brain state stimulation
        AdaptiveSettings DefaultStateSettings = new AdaptiveSettings();     // object with Default State Settings
        AdaptiveSettings SleepStateSettings = new AdaptiveSettings();       // object with Sleep State Settings
        AdaptiveSettings PreStateSettings = new AdaptiveSettings();         // object with Pre State Settings
        AdaptiveSettings SeizureStateSettings = new AdaptiveSettings();     // object with Seizure State Settings

        static int isStiming;           // stimulation on/off flag
        static int stimMode;            // stim should be on/off
        static bool safeMode;           // stim safe mode = stime on defaulte settings with no modulations
        static bool doing_impTest;      // impedance check flag
        static bool doing_stim;         // stimulation configuration flag
        static bool doing_sensing;      // sensing configuration flag
        static bool doing_looprecorder; // loop recorder uploading flag

        static int adaptivePacketCount;
        DateTime dtStimulationSettingsFile;

        #endregion

        #region Sensing Variables

        static int TDPacketCount;

        static int timer_counter; // batteryTimer counter
        static Int32 battery_last_timestamp;
        static System.Timers.Timer batteryTimer;
        static System.Timers.Timer stopRecordingTimer;
        static System.Timers.Timer startRecordingTimer;
        static System.Timers.Timer analyticsPredictionTimer;
        static System.Timers.Timer analyticsDetectionTimer;

        // Defining SummitSystem to be static so it can be properly accessed by sensing event handlers
        private static SummitSystem theSummit;

        static SummitManager theSummitManager;

        // mutex to protect MEF channels.
        static Mutex mut = new Mutex();

        static int MEF_channel_created_sorted;
        static double mef3_sampling_rate;

        static int isStreaming; // Are we currently streaming? - - 1 or 0
        static int streamingMode; // Do we currently want to stream? - 1 or 0
        static int isInStreamingGap; // TODO: DAN

        static int analyticsMode; // Do we currently want to run analytics? - 1 or 0
        static double analytics_detection_new_data; // TODO: DAN
        static double analytics_prediction_new_data; // TODO: DAN

        static bool in_connect_to_ctm; // Are we currently trying to connect to CTM? - True or False
        static int doing_start_or_stop_button; // Are we currently sarting or stoping streaming? - 1 or 0
        static int new_packets_since_last_sweep; // TODO: DAN

        static string[] chan_name; // streaming channels
        static string data_dir;    // location of MEF files, without the .mefd
        static int handlers_added; // summit handler bee added? - 1 or 0
        static string patient_name; // INS Serial Number
        static string patient_subject_id; // INS subject ID

        static string version_string; // version to show on startup

        // streaming variables from GUI/file
        static String samplingRateBoxString;
        static String[] channelMinusBoxString;
        static String[] channelPlusBoxString;
        static String loopRecordingInputBox_Text;
        static String loopBreakInputBox_Text;
        static String samplingRateBox_Text;
        static String[] channelMinusBox;
        static String[] channelPlusBox;
        static int loopRecordingInputBoxInt;
        static int loopBreakInputBoxInt;

        static bool isPeriodic; // // Do we currently want to stream periodic mode? - 1 or 0 (continuous)
        static bool continuousButton_Checked;
        static bool periodicButton_Checked;

        static int in_analytics_detection; // Are we currently running detection algorithm? - 1 or 0
        static int in_analytics_prediction; // Are we currently running prediction algorithm? - 1 or 0

        static int in_power_callback; // TODO: DAN
        static StreamWriter power_file_output; // TODO: DAN

        static ulong last_MEF_written_timestamp; // timestemp for last data written to mef files
        static int num_reject_code_7_in_a_row;

        static int last_INS_battery_level;
        static bool ins_battery_low_stop_streaming; // Is INS battery low and we need to stop streaming? - True or False


        public unsafe struct CHANNEL_STATE
        {
            public fixed byte first[5000];   // CHANNEL_STATE size is 2576, according to sizeof() in c.
        }

        static CHANNEL_STATE[] mef_channel_state_struct_sorted_appending; // TODO: DAN

        static List<SensingEventTD> a_list = new List<SensingEventTD>(); // TODO: DAN
        static Mutex mut_a_list = new Mutex();
        static List<SensingEventTD> b_list = new List<SensingEventTD>(); // TODO: DAN
        static Mutex mut_b_list = new Mutex();

        static int processing_b_list; // TODO: DAN

        // no data notification variables
        static string message_no_data = "No data has been received recently, please verify CTM has working batteries and is turned on.";
        static Int64 last_data_received_timestamp;
        static Int64 WARNING_MINUTES_IN_MILLISECONDS = 30 * (60 * 1000);  // 30 minutes in milliseconds
        static bool warning_in_progress;

        // TODO: DAN

#if WIN64
        [DllImport("DllTest1")]
#else
        [DllImport("DllTest1", CallingConvention = CallingConvention.Cdecl)]
#endif
        protected static extern int initialize_meflib_dll();

#if WIN64
        [DllImport("DllTest1")]
#else
        [DllImport("DllTest1", CallingConvention = CallingConvention.Cdecl)]
#endif
        protected static extern int close_mef_channel(ref CHANNEL_STATE inputs);

#if WIN64
        [DllImport("DllTest1")]
#else
        [DllImport("DllTest1", CallingConvention = CallingConvention.Cdecl)]
#endif
        protected static extern int update_metadata_dll(ref CHANNEL_STATE inputs);

#if WIN64
        [DllImport("DllTest1")]
#else
        [DllImport("DllTest1", CallingConvention = CallingConvention.Cdecl)]
#endif
        protected static extern int write_mef_channel_data(ref CHANNEL_STATE inputs,
                                                       ulong[] packet_times,
                                                       int[] samps,
                                                       ulong n_packets_to_process,
                                                       double secs_per_block,
                                                       double sampling_frequency);

#if WIN64
        [DllImport("DllTest1")]
#else
        [DllImport("DllTest1", CallingConvention = CallingConvention.Cdecl)]
#endif
        protected static extern int flush_mef_channel(ref CHANNEL_STATE inputs);

#if WIN64
        [DllImport("DllTest1")]
#else
        [DllImport("DllTest1", CallingConvention = CallingConvention.Cdecl)]
#endif
        protected static extern int initialize_mef_channel_data(ref CHANNEL_STATE inputs,
                                                            double secs_per_block,
                                                            string chan_map_name,
                                                            int bit_shift_flag,
                                                            double low_frequency_filter_setting,
                                                            double high_frequency_filter_setting,
                                                            double notch_filter_frequency,
                                                            double AC_line_frequency,
                                                            double units_conversion_factor,
                                                            string channel_description,
                                                            double sampling_frequency,
                                                            long block_interval,
                                                            int chan_num,
                                                            string mef3_session_directory,
                                                            float gmt_offset,
                                                            string session_description,
                                                            string anonymized_subject_name,
                                                            string subject_first_name,
                                                            string subject_second_name,
                                                            string subject_id,
                                                            string institution,
                                                            string mef_3_level_1_password,
                                                            string mef_3_level_2_password,
                                                            string study_comments,
                                                            string channel_comments,
                                                            ulong num_secs_per_segment
                                                            );

#if WIN64
        [DllImport("DllTest1")]
#else
        [DllImport("DllTest1", CallingConvention = CallingConvention.Cdecl)]
#endif
        protected static extern int append_mef_channel_data(ref CHANNEL_STATE inputs,
        string chan_map_name,
        int new_segment_number,
        string mef_3_level_1_password,
        string mef_3_level_2_password,
        string mef3_session_directory,
        ulong num_secs_per_segment,
        int bit_shift_flag);

        DateTime dtSensingSettingsFile;


        #endregion

        #region DMP_Main_MDIParent

        private bool privacyMode; // Is privacy mode ON? - True or False
        static System.Timers.Timer privacyTimer;

        private bool CTMconnection; // Are we currently connected to CTM? - True or False
        private bool INSconnection; // Are we currently connected to INS? - True or False

        private EventGenerator m_eventGen = null;
        public Home homeForm;
        static String CTMBattery;
        static String INSBattery;
        static String TabBattery;
        static String UserMessage;

        // Console output and toggle variables
        TextWriter oldOut = Console.Out;
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        Boolean consoleVisible = true;

        public DMP_Main_MDIParent()
        {
            InitializeComponent();

            this.homeForm = null;
            this.stimParamForm = null;

            // Create the event generator
            m_eventGen = new EventGenerator();

            // Attach the event handlers
            m_eventGen.EventINS += new EventGenerator.EventINSHandler(EventGen_Event_INSEvent);
            m_eventGen.EventCTM += new EventGenerator.EventCTMHandler(EventGen_Event_CTMEvent);
            m_eventGen.EventTab += new EventGenerator.EventTabHandler(EventGen_Event_TabEvent);
            m_eventGen.EventMessage += new EventGenerator.EventMessageHandler(EventGen_Event_MessageEvent);

            Startup StartupWindow = new Startup(this);
            StartupWindow.Show();

        }

        // Show/Hide Console window
        public void ToggelConsole()
        {
            try
            {
                var handle = GetConsoleWindow();

                if (consoleVisible)
                {
                    // Hide
                    ShowWindow(handle, SW_HIDE);
                    consoleVisible = false;
                }
                else
                {
                    // Show
                    ShowWindow(handle, SW_SHOW);
                    consoleVisible = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception during toggelConsole(): " + ex.Message);
            }
        }

        // Run or not to run Analytics algorithms
        public void Toggel_Analytics(bool toRun)
        {
            try
            {
                if (toRun && analyticsMode == 0)
                {
                    // Start the analytics timers.
                    // Detection timer
                    analyticsDetectionTimer = new System.Timers.Timer();
                    analyticsDetectionTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnAnalyticsDetectionTimer);
                    analyticsDetectionTimer.Interval = 30000;  // 30 seconds
                    analyticsDetectionTimer.AutoReset = true;
                    analyticsDetectionTimer.Enabled = true;
                    SetIn_analytics_detection = 0;

                    // Prediction timer
                    analyticsPredictionTimer = new System.Timers.Timer();
                    analyticsPredictionTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnAnalyticsPredictionTimer);
                    analyticsPredictionTimer.Interval = 600000;  // 10 minutes
                    analyticsPredictionTimer.AutoReset = true;
                    analyticsPredictionTimer.Enabled = true;
                    SetIn_analytics_prediction = 0;

                    SetAnalyticsMode = 1;
                }
                else if (!toRun && analyticsMode == 1)
                {
                    if (analyticsDetectionTimer != null)
                    {
                        analyticsDetectionTimer.Enabled = false;
                        analyticsDetectionTimer.Stop();
                        analyticsDetectionTimer.Dispose();
                        analyticsDetectionTimer = null;
                    }

                    if (analyticsPredictionTimer != null)
                    {
                        analyticsPredictionTimer.Enabled = false;
                        analyticsPredictionTimer.Stop();
                        analyticsPredictionTimer.Dispose();
                        analyticsPredictionTimer = null;
                    }

                    SetAnalyticsMode = 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception during toggel_Analytics(): " + ex.Message);
            }
        }

        // Get tablet network status
        void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            Console.WriteLine("NetworkChange_NetworkAvailabilityChanged " + (e.IsAvailable));
            this.homeForm.Setlabel_Network(e.IsAvailable);
        }

        // Turn Privacy Mode ON or OFF  
        public void Toggel_PrivacyMode(bool privacy)
        {
            try
            {
                if (privacy && !privacyMode)
                {
                    // Start the privacy timer.
                    privacyTimer = new System.Timers.Timer();
                    privacyTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnprivacyTimer);
                    privacyTimer.Interval = 7200000;  // 120 min
                    privacyTimer.AutoReset = true;
                    privacyTimer.Enabled = true;

                    SetPrivacyMode = true;
                    this.homeForm.Setlabel_privacyMode(true);

                    CustomMsgBox.Show("Privacy Mode is now On", "Privacy Mode", "OK");


                }
                else if (!privacy && privacyMode)
                {
                    if (privacyTimer != null)
                    {
                        privacyTimer.Enabled = false;
                        privacyTimer.Stop();
                        privacyTimer.Dispose();
                        privacyTimer = null;
                    }

                    this.homeForm.Setlabel_privacyMode(false);
                    SetPrivacyMode = false;

                    CustomMsgBox.Show("Privacy Mode is now Off", "Privacy Mode", "OK");

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception during toggel_PrivacyMode(): " + ex.Message);
            }
        }

        // update UI base on application property changes
        private void ConnectionChanges(object sender, PropertyChangedEventArgs e)
        {
            if (!CTMconnection)
            {
                SetCTMBatteryLevelText("N\\A");
                SetINSBatteryLevelText("N\\A");
            }
            if (!INSconnection)
            {
                SetINSBatteryLevelText("N\\A");
            }
        }

        // TODO: DAN
        private void runFileMover()
        {
            try
            {
                ServiceController sc = new ServiceController("Filemover");
                ProcessStartInfo info = new ProcessStartInfo("StartStopFilemover.exe")
                {
                    UseShellExecute = true,
                    CreateNoWindow = true,
                    Verb = "runas"
                };

                Console.WriteLine("Calling StartStopFilemover.exe.");
                // Stop the service
                Process.Start(info);
                Thread.Sleep(2000);

                // Refresh and display the current service status.
                sc.Refresh();

                Console.WriteLine("Calling StartStopFilemover.exe, again.");
                // Restart the service
                Process.Start(info);
                Thread.Sleep(2000);

                // Refresh and display the current service status.
                sc.Refresh();
            }
            catch
            {
                Console.WriteLine("Exception during StartStopFilemover.exe");
            }
        }

        // Initialization of all application variables
        public void Initialization()
        {

            ToggelConsole();

            // varaiables initialization

            fft_check_status = false;
            doing_lda_config = false;
            lda_file_exists = false;
            Input1 = new double[vector_size] { 2.0, 4.0, 7.0, 8.0 };
            Input2 = new double[vector_size] { 3.0, 5.0, 8.0, 10.0 };
            Input1_Corrected = new double[vector_size] { 0, 0, 0, 0 };
            Input2_Corrected = new double[vector_size] { 0, 0, 0, 0 };
            LD0_UpdateRate = 8;
            LD0_HoldOff_Time = 3;
            LD0_Onset_Duration = 2;
            LD0_Blanking_Duration_Upon_StateChange = 11;
            LD0_Termination_Duration = 1;
            Weight_Vector = new uint[vector_size] { 1, 1, 1, 1 };
            Normalization_Multiply_Vector = new uint[vector_size] { 1, 1, 1, 1 };
            Normalization_Subtract_Vector = new uint[vector_size] { 1, 1, 1, 1 };
            LD0_Fractional_FixedPointValue = 8;
            input_channel_index[0] = 0;
            input_channel_index[1] = 1;
            directory_size = 100;
            previous_directory_size = 100;
            previous_timer_day = -1;
            timer_counter = 0;
            doing_impTest = false;
            doing_stim = false;
            doing_sensing = false;
            doing_looprecorder = false;
            SetPrivacyMode = false;
            SetSafeMode = false;
            SetCTMconnection = false;
            SetINSconnection = false;
            SetIsStreaming = 0;
            SetStreamingMode = 0;
            isInStreamingGap = 0;
            handlers_added = 0;
            isPeriodic = false;
            doing_start_or_stop_button = 0;
            mef3_sampling_rate = 250.0;
            processing_b_list = 0;
            chan_name = new string[4];
            SetIn_connect_to_ctm = false;
            new_packets_since_last_sweep = -1;
            channelMinusBox = new string[4];
            channelPlusBox = new string[4];
            last_INS_battery_level = 100;
            ins_battery_low_stop_streaming = false;
            currState = "Off";
            adaptivePacketCount = 0;
            TDPacketCount = 0;
            warning_in_progress = false;
            patient_name = null;
            patient_subject_id = null;
            analytics_detection_new_data = 0.0;
            analytics_prediction_new_data = 0.0;
            in_power_callback = 0;
            power_file_output = null;
            num_reject_code_7_in_a_row = 0;

            // initialize MEF3 library
            initialize_meflib_dll();

            // make sure directories exist, and create them if they don't.
            System.IO.Directory.CreateDirectory(homeFolder + Environment.MachineName);
            System.IO.Directory.CreateDirectory(homeFolder + Environment.MachineName + "/DMP Application Logs");
            System.IO.Directory.CreateDirectory(homeFolder + Environment.MachineName + "/Videos");
            System.IO.Directory.CreateDirectory("c:/ctm_config");

            // Write everything printed to console to log file
            DateTime dt = DateTime.Now;
            String logFilepath = homeFolder + Environment.MachineName + "\\DMP Application Logs\\Log_" + Environment.MachineName + "_" + dt.ToString("yyyyMMddHHmmss") + ".txt";
            var dualOutput = new ConsoleFileOutput(logFilepath, Console.Out);
            Console.SetOut(dualOutput);

            // get version and build information
            Version verNum = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            String buildNum = verNum.Build.ToString();
            DateTime buildDate = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).LastWriteTime;

            version_string = "DMP version: " + verNum.ToString() + " " + buildDate;

#if WIN64
            version_string = version_string + "  (x64)";
#else
            version_string = version_string + "  (x86)";
#endif

            Console.WriteLine(version_string);
            Console.WriteLine("Summit RDK version: " + (typeof(SummitSystem)).Assembly.GetName().Version.ToString());

            // Tell user this code is not for human use
            Console.WriteLine("Starting Mayo DMP Application " + String.Format("{0:G}", dt));

            // run ORCA vanquisher
            runFileMover();

            // start 1 min battery timer
            batteryTimer = new System.Timers.Timer();
            batteryTimer.Elapsed += NewTimer_Elapsed;
            batteryTimer.Interval = 30000;  // 30 sec
            batteryTimer.Enabled = true;

            battery_last_timestamp = 0;

            NetworkChange.NetworkAvailabilityChanged += new NetworkAvailabilityChangedEventHandler(NetworkChange_NetworkAvailabilityChanged);

            // call to imitiate connection
            var t = new Thread(() => On_start(null, null));
            t.Start();
        }

        // closing application function
        public void CloseEnv()
        {
            Console.WriteLine("Closing DMP environment.");

            List<APIReturnInfo> bufferReturnInfo = new List<APIReturnInfo>();

            if (theSummit != null)
            {
                if (!(theSummit.IsDisposed))
                {
                    //Turn off streams and set stim params back to defaults
                    SetDoing_stim = true;
                    StartStreaming(stimMode, 0, "Default");
                    SetDoing_stim = false;

                    // Dispose
                    theSummitManager.DisposeSummit(theSummit);
                }
                theSummitManager.Dispose();
            }

            System.Threading.Thread.Sleep(250);

            // Remove event handlers
            m_eventGen.EventINS -= new EventGenerator.EventINSHandler(EventGen_Event_INSEvent);
            m_eventGen.EventCTM -= new EventGenerator.EventCTMHandler(EventGen_Event_CTMEvent);
            m_eventGen.EventTab -= new EventGenerator.EventTabHandler(EventGen_Event_TabEvent);
            m_eventGen.EventMessage -= new EventGenerator.EventMessageHandler(EventGen_Event_MessageEvent);

            // Stop EventGenerator thread
            m_eventGen.Dispose();

            // garbage collector - keep aTimer alive
            GC.KeepAlive(stopRecordingTimer);
            GC.KeepAlive(startRecordingTimer);
            GC.KeepAlive(analyticsDetectionTimer);
            GC.KeepAlive(analyticsPredictionTimer);
            Dispose();
        }

        private void DMP_Main_MDIParent_MdiChildActivate(object sender, EventArgs e)
        {
            if (this.ActiveMdiChild.Name == "Home")
            {
                ((Home)this.ActiveMdiChild).Home_Load(null, null);
            }

            if (this.ActiveMdiChild.Name == "Patient")
            {
                ((Patient)this.ActiveMdiChild).Patient_Load(null, null);
            }

            else if (this.ActiveMdiChild.Name == "Physician")
            {
                ((Physician)this.ActiveMdiChild).Physician_Load(null, null);
            }


        }

        // Show/Hide On Screen Keyboard
        public void ToggleOSK(bool OnOff)
        {
            try
            {
                Process p = System.Diagnostics.Process.GetProcessesByName("TabTip.exe").FirstOrDefault();
                if (p != null)
                {
                    p.Kill();
                }
                if (OnOff)
                {
                    Process.Start(@"C:\Program Files\Common Files\Microsoft Shared\ink\TabTip.exe");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception during launching On-Screen Keyboard " + ex.Message);
            }
        }
        private void DMP_Main_MDIParent_KeyDown(object sender, KeyEventArgs e)
        {
            // http://csharp.net-informations.com/gui/key-press-cs.htm
            try
            {
                if (e.Control && e.Shift && e.KeyCode == Keys.L)
                {
                    ToggelConsole();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception during DMP_Main_MDIParent_KeyDown(): " + ex.Message);
            }
        }

        #endregion

        #region events

        // events triggers GUI updates

        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool SetIn_connect_to_ctm
        {
            set
            {
                if (value != in_connect_to_ctm)
                {
                    in_connect_to_ctm = value;
                    OnPropertyChanged("In_connect_to_ctm");
                }
            }
        }
        public bool GetIn_connect_to_ctm { get => in_connect_to_ctm; }

        public bool SetCTMconnection
        {
            set
            {
                if (value != CTMconnection)
                {
                    CTMconnection = value;
                    OnPropertyChanged("CTMconnection");
                }
            }

        }
        public bool GetCTMconnection { get => CTMconnection; }

        public bool SetINSconnection
        {
            set
            {
                if (value != INSconnection)
                {
                    INSconnection = value;
                    OnPropertyChanged("INSconnection");
                }
            }

        }
        public bool GetINSconnection { get => INSconnection; }

        public int SetIsStreaming
        {
            set
            {
                if (value != isStreaming)
                {
                    isStreaming = value;
                    OnPropertyChanged("IsStreaming");
                }
            }
        }
        public int GetIsStreaming { get => isStreaming; }

        public int SetStreamingMode
        {
            set
            {
                if (value != streamingMode)
                {
                    streamingMode = value;
                    OnPropertyChanged("StreamingMode");
                }
            }
        }
        public int GetStreamingMode { get => streamingMode; }

        public int SetAnalyticsMode
        {
            set
            {
                if (value != analyticsMode)
                {
                    analyticsMode = value;
                    OnPropertyChanged("AnalyticsMode");
                }
            }
        }
        public int GetAnalyticsMode { get => analyticsMode; }

        public int SetStimMode
        {
            set
            {
                if (value != stimMode)
                {
                    stimMode = value;
                    OnPropertyChanged("StimMode");
                }
            }
        }
        public int GetStimMode { get => stimMode; }

        public bool SetSafeMode
        {
            set
            {
                if (value != safeMode)
                {
                    safeMode = value;
                    OnPropertyChanged("SafeMode");
                }
            }
        }
        public bool GetSafeMode { get => safeMode; }

        public int SetIsStiming
        {
            set
            {
                if (value != isStiming)
                {
                    isStiming = value;
                    OnPropertyChanged("IsStiming");
                }
            }
        }
        public int GetIsStiming { get => isStiming; }

        public bool SetDoing_impTest
        {
            set
            {
                if (value != doing_impTest)
                {
                    doing_impTest = value;
                    OnPropertyChanged("Doing_impTest");
                }
            }
        }
        public bool GetDoing_impTest { get => doing_impTest; }

        public bool SetDoing_LDA_config
        {
            set
            {
                if (value != doing_lda_config)
                {
                    doing_lda_config = value;
                    OnPropertyChanged("Doing_LDA_config");
                }
            }
        }
        public bool GetDoing_LDA_config { get => doing_lda_config; }

        public bool SetDoing_stim
        {
            set
            {
                if (value != doing_stim)
                {
                    doing_stim = value;
                    OnPropertyChanged("Doing_stim");
                }
            }
        }
        public bool GetDoing_stim { get => doing_stim; }

        public bool SetDoing_looprecorder
        {
            set
            {
                if (value != doing_looprecorder)
                {
                    doing_looprecorder = value;
                }
            }
        }
        public bool GetDoing_looprecorder { get => doing_looprecorder; }

        public bool SetDoing_sensing
        {
            set
            {
                if (value != doing_sensing)
                {
                    doing_sensing = value;
                    OnPropertyChanged("Doing_sensing");
                }
            }
        }
        public bool GetDoing_sensing { get => doing_sensing; }

        public int SetIn_analytics_detection
        {
            set
            {
                if (value != in_analytics_detection)
                {
                    in_analytics_detection = value;
                    OnPropertyChanged("In_analytics_detection");
                }
            }
        }
        public int GetIn_analytics_detection { get => in_analytics_detection; }

        public int SetIn_analytics_prediction
        {
            set
            {
                if (value != in_analytics_prediction)
                {
                    in_analytics_prediction = value;
                    OnPropertyChanged("In_analytics_prediction");
                }
            }
        }
        public int GetIn_analytics_prediction { get => in_analytics_prediction; }

        public bool SetPrivacyMode
        {
            set
            {
                if (value != privacyMode)
                {
                    privacyMode = value;
                    OnPropertyChanged("PrivacyMode");
                }
            }
        }
        public bool GetPrivacyMode { get => privacyMode; }

        public string CurrState
        {
            get => currState;
            set
            {
                if (value != currState)
                {
                    currState = value;
                    OnPropertyChanged("CurrState");
                }
            }
        }
        public SummitSystem TheSummit { get => theSummit; set => theSummit = value; }
        public string Patient_name { get => patient_name; set => patient_name = value; }
        public DateTime DtStimulationSettingsFile { get => dtStimulationSettingsFile; set => dtStimulationSettingsFile = value; }
        public DateTime DtSensingSettingsFile { get => dtSensingSettingsFile; set => dtSensingSettingsFile = value; }
        public string HomeFolder { get => homeFolder; set => homeFolder = value; }
        // INS Battery tool Strip event
        void EventGen_Event_INSEvent()
        {
            BatterytoolStrip.SafeSetText(INSbatteryLevelText, INSBattery);
            (this.homeForm).SetINSBatteryText(INSBattery);

            Char delimiter = '%';
            String[] substrings = INSBattery.Split(delimiter);
            if (Int32.TryParse(substrings[0], out int v))
            {
                (this.homeForm).SetINSBattery(v);
            }
            else
            {
                (this.homeForm).SetINSBattery(0);
            }

        }
        // CTM Battery tool Strip event
        void EventGen_Event_CTMEvent()
        {
            BatterytoolStrip.SafeSetText(ctmBatteryLevelText, CTMBattery);
            (this.homeForm).SetCTMBatteryText(CTMBattery);

            Char delimiter = '%';
            String[] substrings = CTMBattery.Split(delimiter);
            if (Int32.TryParse(substrings[0], out int v))
            {
                (this.homeForm).SetCTMBattery(v);
            }
            else
            {
                (this.homeForm).SetCTMBattery(0);
            }
        }
        // Tablet Battery tool Strip event
        void EventGen_Event_TabEvent()
        {
            BatterytoolStrip.SafeSetText(tabletBatteryLevelText, TabBattery + "%");
            (this.homeForm).SetTabBatteryText(TabBattery + "%");
            (this.homeForm).SetTabBattery(Convert.ToInt32(TabBattery));

        }

        // TODO: dAN - CAN WE CHANGE THAT TO NOT BEING AN EVENT?
        void EventGen_Event_MessageEvent()
        {
            Thread t = new Thread(() => MessageBoxSeparateThread(UserMessage));
            t.Start();
        }

        void MessageBoxSeparateThread(object text)
        {
            // set a flag, but only for "message_no_data" message, so we don't get multiple of these messages
            // in close succession
            if (message_no_data.Equals((string)text))
                warning_in_progress = true;

            DialogResult dialogResult = CustomMsgBox.Show((string)text, "Error", "OK");

            // after user acknowledges message, we can reset the clock for last_data_received,
            // and release flag.  This only matters for "message_no_data" message.
            if (message_no_data.Equals((string)text))
            {
                if (dialogResult == DialogResult.OK)
                {
                    // Reset last_data_received_timestamp, so it will be a while before this message shows again
                    last_data_received_timestamp = (Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;

                    // release flag
                    warning_in_progress = false;
                }
            }
            /*
                if (message_no_data.Equals((string)text))
                {
                    // Reset last_data_received_timestamp, so it will be a while before this message shows again
                    last_data_received_timestamp = (Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;

                    // release flag
                    warning_in_progress = false;
                }
                */
        }

        #endregion

        #region Sensing Code - Methods

        public void EndCurrentRecording(Boolean clear_sorting_buffers)
        {
            doing_start_or_stop_button = 1;

            mut.WaitOne();

            // close sorted MEF channels, and clear buffer lists
            mut_a_list.WaitOne();
            mut_b_list.WaitOne();
            for (int i = 0; i < 4; i++)
            {
                if (MEF_channel_created_sorted == 1)
                {
                    close_mef_channel(ref mef_channel_state_struct_sorted_appending[i]);
                }
            }

            MEF_channel_created_sorted = 0;

            if (clear_sorting_buffers)
                b_list.Clear();
            mut_b_list.ReleaseMutex();

            if (clear_sorting_buffers)
                a_list.Clear();
            mut_a_list.ReleaseMutex();

            mut.ReleaseMutex();

            doing_start_or_stop_button = 0;
        }

        public void On_start(object sender, EventArgs e)
        {
            if (doing_start_or_stop_button == 1)
            {
                Console.WriteLine("Start or Stop button is in progress, so don't do anything.");
                return;
            }

            doing_start_or_stop_button = 1;

            SetStreamingMode = 1;
            isInStreamingGap = 0;

            last_data_received_timestamp = (Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;

            // read sensing setting file
            ReadSettingsFile();

            if (!ConnectToCTM(false))
            {
                // for now, just try to connect.  If you can't, oh well.  Timers will get it later.
                SetCTMconnection = false;
                SetINSconnection = false;
            }

            mut.WaitOne();

            //MEF_channel_created = 0;
            MEF_channel_created_sorted = 0;
            last_MEF_written_timestamp = 0;

            // read stimulation setting file
            ReadStimulationSettingsFile();
            // Read the LDA configuration file
            ReadLDAConfigurationFile();

            SetDoing_sensing = true;
            StartStreaming(stimMode, streamingMode, "Default");
            SetDoing_sensing = false;


            // Start the sweepTimer.  SweepTimer runs every 15 seconds and makes sure that
            // any "dangling" packets are processed, as well as flushing out data to MEF channels.
            new_packets_since_last_sweep = -1;

            Toggel_Analytics(true);

            // Start the LDA status timers.
            LDAstatusTimer = new System.Timers.Timer();
            LDAstatusTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnLDAstatusTimer);
            LDAstatusTimer.Interval = 15000;  // 15 seconds
            LDAstatusTimer.AutoReset = true;
            LDAstatusTimer.Enabled = true;


            if (periodicButton_Checked)
            {
                stopRecordingTimer = new System.Timers.Timer();
                stopRecordingTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnStopRecordingTimer);
                stopRecordingTimer.Interval = 60000 * loopRecordingInputBoxInt;
                stopRecordingTimer.AutoReset = false;
                stopRecordingTimer.Enabled = true;

            }

            PropertyChanged -= ConnectionChanges;
            PropertyChanged += ConnectionChanges;

            mut.ReleaseMutex();

            doing_start_or_stop_button = 0;

        }

        public void On_stop(object sender, EventArgs e)
        {

            if (doing_start_or_stop_button == 1)
            {
                Console.WriteLine("Start or Stop button is in progress, so don't do anything.");
                return;
            }

            SetStreamingMode = 0;

            doing_start_or_stop_button = 1;

            mut.WaitOne();

            SetStreamingMode = 0;
            SetDoing_sensing = true;
            StartStreaming(stimMode, streamingMode, currState);
            SetDoing_sensing = false;

            theSummitManager.DisposeSummit(theSummit);
            theSummit = null;

            SetCTMconnection = false;
            SetINSconnection = false;

            if (startRecordingTimer != null)
            {
                startRecordingTimer.Enabled = false;
                startRecordingTimer.Dispose();
                startRecordingTimer = null;
            }

            if (stopRecordingTimer != null)
            {
                stopRecordingTimer.Enabled = false;
                stopRecordingTimer.Dispose();
                stopRecordingTimer = null;
            }

            Toggel_Analytics(false);

            if (LDAstatusTimer != null)
            {
                LDAstatusTimer.Enabled = false;
                LDAstatusTimer.Dispose();
                LDAstatusTimer = null;
            }

            // close sorted MEF channels, and clear buffer lists
            mut_a_list.WaitOne();
            mut_b_list.WaitOne();
            for (int i = 0; i < 4; i++)
            {
                if (MEF_channel_created_sorted == 1)
                {
                    close_mef_channel(ref mef_channel_state_struct_sorted_appending[i]);
                }
            }

            MEF_channel_created_sorted = 0;
            last_MEF_written_timestamp = 0;

            b_list.Clear();
            mut_b_list.ReleaseMutex();

            a_list.Clear();
            mut_a_list.ReleaseMutex();

            mut.ReleaseMutex();

            if (power_file_output != null)
            {
                power_file_output.Close();
                power_file_output = null;
            }

            doing_start_or_stop_button = 0;
        }

        private void OnLDAstatusTimer(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("\n--- LDA status Timer fired: " + String.Format("{0:G}", DateTime.Now) + "---");

            // Asychronously reads back the detector and adaptive stimulation status independant of stream timing.
            // theAdaptiveStatus - Output buffer that will be written with the current state of the embedded adaptive controllers and detectors. Null if an error occurs.

            try
            {
                theSummit.ReadAdaptiveDetectionStatus(out DetectionAdaptivePacket theAdaptiveStatus);
                if (theAdaptiveStatus != null)
                {
                    if (!lastDetectionOutputStatus.Equals(theAdaptiveStatus.Ld0DetectionStatus)
                        && theAdaptiveStatus.Ld0DetectionStatus.Equals(DetectionOutputStatus.LowDetect)) // it is low (seizure and wasnt sizure last time we checked
                    {
                        Console.WriteLine("Change in LDA Status was detected! The low boundary is in detect.\nChecking CurrentAdaptiveState...");
                        Console.WriteLine("Current State: " + theAdaptiveStatus.CurrentAdaptiveState.ToString());

                        // curent state is seizure and last one was NOT seizure
                        if (theAdaptiveStatus.CurrentAdaptiveState.ToString().Equals("1")
                            && !lastAdaptiveState.ToString().Equals("1"))
                        {
                            Console.WriteLine("Seizure was detected!");
                            if (stimParamForm != null)
                            {
                                stimParamForm.Setlabel_ActivatedStateText(CurrState + " - Seizure");
                            }
                        }

                        // curent state is seizure and there are less than 15 sec to current recording
                        if (theAdaptiveStatus.CurrentAdaptiveState.ToString().Equals("1") && timeleft < 15)
                        {
                            CaptureVideo();
                        }
                    }
                    else
                        Console.WriteLine("No detection.");

                    lastAdaptiveState = theAdaptiveStatus.CurrentAdaptiveState;
                    lastDetectionOutputStatus = theAdaptiveStatus.Ld0DetectionStatus;

                    /*
                     Enumerator:
                     None - The none
                     LowImmediateDetect - The low boundary is in immediate detect
                     HighImmediateDetect - The high boundary is in immediate detect
                     LowDetect - The low boundary is in detect
                     HighDetect - The high boundary is in detect
                     OutputOverRange - The ld output is over range and the LD output is held at last valid computed value
                     Blanked - The LD is being blanked currently due to just having entered a new state
                     InputOverRange - A power channel input to the LD is over range and the LD output is held at last valid computed value
                     InHoldoff - The LD is in a hold off state to allow for it to stabilize (LD detect output not valid)
                     */
                }
                else
                {
                    Console.WriteLine("LDA status reading failed...");
                }
            }
            catch
            {
                Console.WriteLine("Failed reading LDA status status...");
            }

            Console.WriteLine("--- LDA status Timer finished ---\n");
        }

        public void CaptureVideo()
        {
            try
            {
                // only capture video if privacy mode is OFF
                if (!privacyMode)
                {
                    homeForm.Setlabel_recTimeText("Getting ready to record video...");

                    if (video_process != null)
                    {
                        // we are already recording a video so record again after done
                        record_again_after_done = 1;
                        return;
                    }

                    txtFilename = homeFolder + Environment.MachineName + "/Videos/" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".avi\"";
                    timeleft = milltorec;

                    Videotimer = new System.Timers.Timer();
                    Videotimer.Elapsed += Videotimer_Tick;
                    Videotimer.AutoReset = true;
                    Videotimer.Interval = 1000;

                    Console.WriteLine("Capturing a video: " + String.Format("{0:G}", DateTime.Now));
                    Console.WriteLine(txtFilename);

                    Videotimer.Start();

                    // start video/audio recording
                    //String cmd_txt = "-y -f dshow -i video=\"HP HD Camera\":audio=\"Internal Microphone (Conexant S\" -r 25 -t 120 " + txtFilename;
                    String cmd_txt = "-y -f dshow -i video=\"Microsoft Camera Front\":audio=\"Microphone Array (Realtek High Definition Audio(SST))\" -r 25 -t 120 " + txtFilename;
                    Console.WriteLine("Arguments for video: " + cmd_txt);

                    video_process = new System.Diagnostics.Process();

                    video_process.StartInfo.FileName = "C:/EPAD/ffmpeg";

                    video_process.StartInfo.Arguments = cmd_txt;
                    video_process.StartInfo.UseShellExecute = false;
                    video_process.StartInfo.Verb = "runas";
                    video_process.StartInfo.RedirectStandardOutput = true;
                    video_process.StartInfo.CreateNoWindow = true;

                    // set the correct working directory
                    video_process.StartInfo.WorkingDirectory = @"C:/EPAD/";

                    try
                    {
                        // Start the process
                        video_process.Start();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        Console.WriteLine("Exception during starting video\n");
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("Privacy Mode is ON - NOT Capturing a video: " + String.Format("{0:G}", DateTime.Now));

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception while capturing the video\n\n" + e.Message + "\n\n" + e.ToString());
            }
        }

        void Videotimer_Tick(object sender, System.EventArgs e)
        {
            try
            {
                if (timeleft > 0)
                {
                    timeleft = timeleft - 1000;

                    // if another recording will be done after this one, then add milltorec seconds to
                    // displayed time, to inform the user
                    int timeleft_display = timeleft;
                    if (record_again_after_done == 1)
                        timeleft_display += milltorec;

                    homeForm.Setlabel_recTimeText("Time left: " + double.Parse(string.Format("{0}e-3", timeleft_display)));
                }
                else
                {
                    Videotimer.Stop();
                    Videotimer.Dispose();

                    homeForm.Setlabel_recTimeText("Ending Recording...");

                    // Wait for video to be done, and close process.
                    video_process.WaitForExit();
                    video_process.Close();
                    video_process = null;

                    Console.WriteLine("Finished recording video: " + String.Format("{0:G}", DateTime.Now));

                    // start new recording, if we wanted to start one during the one that just finished.
                    if (record_again_after_done == 1)
                    {
                        record_again_after_done = 0;
                        CaptureVideo();
                    }

                    System.Threading.Thread.Sleep(250);
                    homeForm.Setlabel_recTimeText(" ");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception during Videotimer_Tick(): " + ex.Message);
            }
        }

        private void LoopRecorderCheck()
        {
            // If MEF files haven't been created yet, then skip this check
            if (MEF_channel_created_sorted == 0)
                return;

            // Set flag for rest of code to know that we are examining the loop recorder
            SetDoing_looprecorder = true;

            // Begin process of writing data to MEF
            mut.WaitOne();
            mut_a_list.WaitOne();
            mut_b_list.WaitOne();

            try
            {
                // check loop-recorder status
                // query the INS for the LR status.
                APIReturnInfo returnStatusBuffer = theSummit.ReadSensingLoopRecordStatus(out LoopRecordingStatus lr_status);

                // Write the status out to the console if it was returned.
                if (lr_status != null)
                {
                    Console.WriteLine("LR Status: " + lr_status.LrFlags.ToString());

                    if (!(lr_status.LrFlags.HasFlag(LoopRecordingFlags.Triggered)))
                    {
                        Console.WriteLine("No Loop Recorder trigger found.");
                        mut_b_list.ReleaseMutex();
                        mut_a_list.ReleaseMutex();
                        mut.ReleaseMutex();
                        SetDoing_looprecorder = false;
                        return;
                    }

                    // Wait up to 45 seconds for loop recorder to finish, if it is triggered but not completed
                    int seconds_waited = 0;
                    while (lr_status.LrFlags.HasFlag(LoopRecordingFlags.Triggered) &&
                        !lr_status.LrFlags.HasFlag(LoopRecordingFlags.Completed))
                    {
                        if (seconds_waited >= 45)
                        {
                            Console.WriteLine("Took too long for LR to complete, ignoring loop recorder");
                            mut_b_list.ReleaseMutex();
                            mut_a_list.ReleaseMutex();
                            mut.ReleaseMutex();
                            SetDoing_looprecorder = false;
                            return;
                        }

                        System.Threading.Thread.Sleep(5000);
                        seconds_waited += 5;
                        returnStatusBuffer = theSummit.ReadSensingLoopRecordStatus(out lr_status);
                        Console.WriteLine("LR Status: " + lr_status.LrFlags.ToString());
                    }

                    Console.WriteLine("Looprecorder read data, RejectCode = " + returnStatusBuffer.RejectCode + " - " + returnStatusBuffer.RejectCodeType + " - " + returnStatusBuffer.Descriptor);


                    if (lr_status.LrFlags.HasFlag(LoopRecordingFlags.Completed))
                    {

                        //string trigger_timestamp = lr_status.TriggerTimestamp.ToString();
                        double trigger_timestamp = DateTimeToUnixTimestamp(lr_status.TriggerTimestamp.RealTime);
                        Console.WriteLine("LR completed, trigger timestamp = " + trigger_timestamp);

                        // Sensing must be disabled to pull the loop record, make sure it's able to turn off.
                        int count_turn_sensing_off = 0;
                        while (theSummit.WriteSensingState(SenseStates.None, 0).RejectCode != 0)
                        {
                            if (count_turn_sensing_off >= 10)
                            {
                                Console.WriteLine("Couldn't turn sensing off!  Not using LR data.");
                                mut_b_list.ReleaseMutex();
                                mut_a_list.ReleaseMutex();
                                mut.ReleaseMutex();
                                SetDoing_looprecorder = false;
                                return;
                            }
                            count_turn_sensing_off++;
                        };

                        // Pull the LR from the INS, logged to file
                        APIReturnInfo returnLrBuffer = theSummit.ReadSensingLoopRecordData(out LoopRecord record);

                        Console.WriteLine("Done reading Loop Recorder Data");
                        Console.WriteLine("Looprecorder read data, RejectCode = " + returnLrBuffer.RejectCode + " - " + returnLrBuffer.RejectCodeType + " - " + returnLrBuffer.Descriptor);


                        // Write the data properties out to the console
                        if (record != null && record.ChannelSamples != null)
                        {
                            Console.WriteLine("The Data: " + record.ChannelSamples.ToString());
                            Console.WriteLine("Num count: " + record.ChannelSamples[0].Count);
                            ulong end_timestamp = (ulong)(DateTimeToUnixTimestamp(record.LrStatus.EndTimestamp.RealTime) * 1000000);
                            Console.WriteLine("End Timestamp: " + end_timestamp);
                            ulong trigger_timestamp2 = (ulong)(DateTimeToUnixTimestamp(record.LrStatus.TriggerTimestamp.RealTime) * 1000000);
                            Console.WriteLine("Trigger Timestamp: " + trigger_timestamp2);

                            ulong[] packet_times;
                            ulong[] packet_times_with_skipping;
                            int[] samps;
                            int[] samps_with_skipping;
                            int largest_sample_num = 0;
                            foreach (var item in record.ChannelSamples)
                            {
                                if ((item.Key != SenseTimeDomainChannel.Ch0) &&
                                    (item.Key != SenseTimeDomainChannel.Ch1) &&
                                    (item.Key != SenseTimeDomainChannel.Ch2) &&
                                    (item.Key != SenseTimeDomainChannel.Ch3))
                                {
                                    Console.WriteLine("Invalid channel Key from LR!  Not using LR data.");
                                    mut_b_list.ReleaseMutex();
                                    mut_a_list.ReleaseMutex();
                                    mut.ReleaseMutex();
                                    SetDoing_looprecorder = false;
                                    return;
                                }
                                if (item.Value.Count > largest_sample_num)
                                {
                                    largest_sample_num = item.Value.Count;
                                }
                            }

                            ulong base_timestamp = end_timestamp - (ulong)((largest_sample_num - 1) * ((1e6) / mef3_sampling_rate));

                            packet_times = new ulong[largest_sample_num];
                            packet_times_with_skipping = new ulong[largest_sample_num];
                            samps = new int[largest_sample_num];
                            samps_with_skipping = new int[largest_sample_num];
                            for (int i = 0; i < largest_sample_num; i++)
                            {
                                packet_times[i] = base_timestamp + (ulong)((i * ((1e6) / mef3_sampling_rate)));
                            }

                            // empty out buffered lists so out-of-order packets don't end up in MEF stream
                            a_list.Clear();
                            b_list.Clear();

                            foreach (var item in record.ChannelSamples)
                            {
                                for (int j = 0; j < item.Value.Count; j++)
                                {
                                    // convert from volts to microvolts.
                                    if (item.Value[j] >= 0)
                                        samps[j] = (int)((item.Value[j] * 1000000) + 0.5);
                                    else
                                        samps[j] = (int)((item.Value[j] * 1000000) - 0.5);
                                    //Console.WriteLine("sample: " + samps[j]);
                                }

                                // Figure out which channel this data corresponds to
                                // we've already checked that it does correspond to a real channel
                                int k = -1;
                                switch (item.Key)
                                {
                                    case SenseTimeDomainChannel.Ch0:
                                        k = 0;
                                        break;
                                    case SenseTimeDomainChannel.Ch1:
                                        k = 1;
                                        break;
                                    case SenseTimeDomainChannel.Ch2:
                                        k = 2;
                                        break;
                                    case SenseTimeDomainChannel.Ch3:
                                        k = 3;
                                        break;
                                }

                                // fill new arrays, with some samples possibly removed if those timestamps
                                // have already been written to MEF.  This logic could probably be done with
                                // pointer arithmetic as well, but this code shouldn't get called very often.
                                int new_count = 0;
                                Console.WriteLine("Last MEF written timestamp: " + last_MEF_written_timestamp);
                                for (int i = 0; i < item.Value.Count; i++)
                                {
                                    if (packet_times[i] > last_MEF_written_timestamp)
                                    {
                                        packet_times_with_skipping[new_count] = packet_times[i];
                                        samps_with_skipping[new_count] = samps[i];
                                        new_count++;
                                    }
                                }

                                write_mef_channel_data(ref (mef_channel_state_struct_sorted_appending[k]), packet_times_with_skipping, samps_with_skipping, (ulong)(new_count), 15, mef3_sampling_rate);
                                Console.WriteLine("Number of loop recorder samples added to MEF stream: " + new_count);

                            }

                        }
                        else
                        {
                            Console.WriteLine("Loop recorder data not read.");
                            if (record == null)
                                Console.WriteLine("record == null");
                            if (record.ChannelSamples == null)
                                Console.WriteLine("record.ChannelSamples == null");

                        }
                    }
                }
                else
                {
                    Console.WriteLine("Loop recorder status not read.");
                }
            }
            catch
            {
                mut_b_list.ReleaseMutex();
                mut_a_list.ReleaseMutex();
                mut.ReleaseMutex();

                SetDoing_looprecorder = false;

                return;
            }

            mut_b_list.ReleaseMutex();
            mut_a_list.ReleaseMutex();
            mut.ReleaseMutex();

            SetDoing_looprecorder = false;
        }

        private void OnStartRecordingTimer(object source, System.Timers.ElapsedEventArgs e)
        {
            if (streamingMode == 0 || !isPeriodic)
            {
                return;
            }

            DateTime dt = DateTime.Now;
            Console.WriteLine("\n--- Start Recording Timer fired: " + String.Format("{0:G}", dt) + "---");

            if (doing_impTest || doing_lda_config || doing_stim || doing_sensing)
            {
                if (doing_impTest)
                    Console.WriteLine("Imp test is in progress, so don't do anything.");
                if (doing_lda_config)
                    Console.WriteLine("LDA configuration is in progress, so don't do anything.");
                if (doing_stim)
                    Console.WriteLine("Stim setting is in progress, so don't do anything.");
                if (doing_sensing)
                    Console.WriteLine("Sensing setting is in progress, so don't do anything.");
            }
            else
            {
                LoopRecorderCheck();

                Console.WriteLine("Start Recording Timer calls startStreaming");

                SetDoing_sensing = true;
                StartStreaming(stimMode, 1, currState);
                SetDoing_sensing = false;

            }

            //streamingMode = 1;
            isInStreamingGap = 0;

            if (periodicButton_Checked)
            {
                startRecordingTimer.Enabled = false;
                startRecordingTimer.Dispose();
                startRecordingTimer = null;

                stopRecordingTimer = new System.Timers.Timer();
                stopRecordingTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnStopRecordingTimer);
                stopRecordingTimer.Interval = 60000 * loopRecordingInputBoxInt;
                stopRecordingTimer.AutoReset = false;
                stopRecordingTimer.Enabled = true;

            }

            Console.WriteLine("--- Start Recording Timer finished ---\n");
        }

        private void OnStopRecordingTimer(object source, System.Timers.ElapsedEventArgs e)
        {
            //streamingMode = 1;
            if (streamingMode == 0 || !isPeriodic)
            {
                return;
            }

            DateTime dt = DateTime.Now;
            Console.WriteLine("\n--- Stop Recording Timer fired: " + String.Format("{0:G}", dt) + "---");

            try
            {
                // query the INS for the LR status.
                APIReturnInfo returnStatusBuffer = theSummit.ReadSensingLoopRecordStatus(out LoopRecordingStatus lr_status);

                // Wait up to 45 seconds for loop recorder to finish, if it is triggered but not completed
                // This means that if the loop recorder is in progress, we will keep recording TD packets
                // unitl the period of interest is over.  Then we will stop the recording.
                // So we're not actually downloading loop recorder data, just using the status of of the LR
                // to see if we want to keep recording normally.
                int seconds_waited = 0;
                while (lr_status.LrFlags.HasFlag(LoopRecordingFlags.Triggered) &&
                    !lr_status.LrFlags.HasFlag(LoopRecordingFlags.Completed))
                {
                    if (seconds_waited >= 45)
                    {
                        Console.WriteLine("Took too long for LR to complete, proceeding with OnStopRecordingTimer");
                        break;
                    }

                    System.Threading.Thread.Sleep(5000);
                    seconds_waited += 5;
                    returnStatusBuffer = theSummit.ReadSensingLoopRecordStatus(out lr_status);
                    Console.WriteLine("LR Status: " + lr_status.LrFlags.ToString());
                }
            }
            catch { }

            isInStreamingGap = 1;

            if (doing_impTest || doing_lda_config || doing_stim || doing_sensing)
            {
                if (doing_impTest)
                    Console.WriteLine("Imp test is in progress, so don't do anything.");
                if (doing_lda_config)
                    Console.WriteLine("LDA configuration is in progress, so don't do anything.");
                if (doing_stim)
                    Console.WriteLine("Stim setting is in progress, so don't do anything.");
                if (doing_sensing)
                    Console.WriteLine("Sensing setting is in progress, so don't do anything.");
            }
            else
            {
                Console.WriteLine("Stop Recording Timer calls startStreaming");
                SetDoing_sensing = true;
                StartStreaming(stimMode, 0, currState);
                SetDoing_sensing = false;
            }

            if (periodicButton_Checked)
            {
                stopRecordingTimer.Enabled = false;
                stopRecordingTimer.Dispose();
                stopRecordingTimer = null;

                startRecordingTimer = new System.Timers.Timer();
                startRecordingTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnStartRecordingTimer);
                startRecordingTimer.Interval = 60000 * loopBreakInputBoxInt;
                startRecordingTimer.AutoReset = false;
                startRecordingTimer.Enabled = true;
            }

            Console.WriteLine("--- Stop Recording Timer finished ---\n");
        }

        private void OnAnalyticsDetectionTimer(object source, System.Timers.ElapsedEventArgs e)
        {
            Console.WriteLine("\n--- Analytics Detection Timer fired ---");

            if (in_analytics_detection == 1)
            {
                // previous instace of the analytics is already running, so skip this instance
                Console.WriteLine("Previous analytics detection instance is still running, so skip this one.");
                Console.WriteLine("--- Analytics Detection Timer finished ---\n");
                return;
            }

            SetIn_analytics_detection = 1;

            Int64 unixTimestamp = (Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
            var time_start = DateTime.Now;
            string formattedTime_start = time_start.ToString("yyyy/MM/dd hh:mm:ss tt");
            //Console.WriteLine("Current timestamp when detection algorithm fired is: " + formattedTime_start);
            long current_Time;
            long starting_time;
            current_Time = (unixTimestamp - 30000) * 1000;  // subtract 30 seconds, then convert to microseconds
            starting_time = current_Time - 30000000;
            if (String.IsNullOrEmpty(data_dir) || chan_name?.Length < 4)
            {
                Console.WriteLine("Data directory for running analytics is empty. Exiting. ");
                SetIn_analytics_detection = 0;
                Console.WriteLine("--- Analytics Detection Timer finished ---\n");
                return;
            }
            if (patient_name == null)
            {
                Console.WriteLine("Patient name is null.  Analytics detection is exiting. ");
                SetIn_analytics_detection = 0;
                Console.WriteLine("--- Analytics Detection Timer finished ---\n");
                return;
            }


            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
            pProcess.StartInfo.FileName = "C:\\AlgorithmAnalytics\\Detection_Code\\dist\\LiveSeizureDetect_WithArguments.exe";
            string New_data_dir = FirstCharacterToUpper(data_dir);
            New_data_dir = New_data_dir.Replace(" ", string.Empty);
            pProcess.StartInfo.Arguments = current_Time + " \"" + New_data_dir + ".mefd/\"" + " " + chan_name[0] + ".timd" + " " + chan_name[1] + ".timd" + " " + chan_name[2] + ".timd" + " " + chan_name[3] + ".timd";
            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.Verb = "runas";
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.StartInfo.CreateNoWindow = true;

            // set the correct working directory
            pProcess.StartInfo.WorkingDirectory = @"C:\\AlgorithmAnalytics\\Detection_Code\\dist";

            try
            {
                // Start the process
                pProcess.Start();
                // Wait for process to finish
                pProcess.WaitForExit();
                //Get program output
#if DEBUG
                Console.WriteLine("Current timestamp is " + unixTimestamp);
                string strOutput = pProcess.StandardOutput.ReadToEnd();
                Console.WriteLine(strOutput);
                Console.WriteLine(pProcess.StartInfo.FileName);
                Console.WriteLine(pProcess.StartInfo.Arguments);
#endif
                pProcess.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                SetIn_analytics_detection = 0;
                Console.WriteLine("--- Analytics Detection Timer finished ---\n");
                return;
            }
            var time_end = DateTime.Now;
            string formattedTime_end = time_end.ToString("yyyy/MM/dd hh:mm:ss tt");
            Int64 unixTimestamp_end = (Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
            Console.WriteLine("\nDetection algorithm run starting at " + formattedTime_start + " and finished at " + formattedTime_end);
            Console.WriteLine("Analytics (detection) ran successfully from {0} to {1}.", starting_time, current_Time);
            ///Following code for reading the .txt file from c:\ctm_config\detection_tmp.txt
            int counter = 0;
            string line;
            DateTime curr_Time = DateTime.Now;
            //Initialize the file to in dropbox for saving outputs.
            String det_filename = homeFolder + Environment.MachineName + "\\detection_output_" + patient_name + ".csv";
            StreamWriter sw;
            if (File.Exists(det_filename))
            {
                sw = new StreamWriter(det_filename, true);

            }
            else
            {
                sw = new StreamWriter(det_filename, true);
                sw.WriteLine("start_time, end_time, channel, Event");
            }

            try
            {
                // Read the file and display it line by line.  
                System.IO.StreamReader file =
                    new System.IO.StreamReader(@"c:\\ctm_config\\detection_tmp.txt");

                while ((line = file.ReadLine()) != null)
                {
                    //System.Console.WriteLine(line);
                    sw.WriteLine(line);
                    counter++;
                }
                // System.Console.WriteLine("There were {0} lines.", counter);
                file.Close();
                sw.Close();
                // Console.ReadLine();
            }
            catch (FileNotFoundException er)
            {
                Console.WriteLine("Exception: " + er.Message);
            }

            SetIn_analytics_detection = 0;
            Console.WriteLine("--- Analytics Detection Timer finished ---\n");
        }

        private void OnAnalyticsPredictionTimer(object source, System.Timers.ElapsedEventArgs e)
        {
            Console.WriteLine("\n--- Analytics Prediction Timer fired ---");

            if (in_analytics_prediction == 1)
            {
                // previous instace of the analytics is already running, so skip this instance
                Console.WriteLine("Previous analytics prediction instance is still running, so skip this one.");
                Console.WriteLine("--- Analytics Prediction Timer finished ---\n");
                return;
            }

            double prediction_test_threshold_ratio;

            // calculate expected recording ratio based on continuous or periodic settings
            if (isPeriodic)
                prediction_test_threshold_ratio = ((double)loopRecordingInputBoxInt / (double)(loopRecordingInputBoxInt + loopBreakInputBoxInt));
            else
                prediction_test_threshold_ratio = 1.0;  // continuous

            // 40% of expected recording is our threshold
            // save the original ratio value, becuase we are sending it as a parameter to the
            // prediction algorithm executable
            double prediction_test_threshold_percentage = prediction_test_threshold_ratio * 0.4;

            mut_b_list.WaitOne();
            if (analytics_prediction_new_data < (600 * prediction_test_threshold_percentage)) // 10 minutes
            {
                Console.WriteLine("Not enough new data for analytics prediction to run, returning.");
                Console.WriteLine("Data received (sec): " + analytics_prediction_new_data + ", Prediction threshold: " +
                    600 * prediction_test_threshold_percentage);
                analytics_prediction_new_data = 0.0;
                mut_b_list.ReleaseMutex();
                Console.WriteLine("--- Analytics Prediction Timer finished ---\n");
                return;
            }
            analytics_prediction_new_data = 0.0;
            mut_b_list.ReleaseMutex();


            SetIn_analytics_prediction = 1;
            var time_start = DateTime.Now;
            string formattedTime_start = time_start.ToString("yyyy/MM/dd hh:mm:ss tt");
            Int64 unixTimestamp = (Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
            // Console.WriteLine("Current timestamp when the prediction algorithm is fired is: " + formattedTime_start);
            long current_Time;
            long starting_time;
            current_Time = (unixTimestamp - 30000) * 1000;  // subtract 30 seconds, then convert to microseconds
            starting_time = current_Time - 600000000; //10 minutes before
            if (String.IsNullOrEmpty(data_dir) || chan_name?.Length < 4)
            {
                Console.WriteLine("Data directory path not valid. Exiting. "); //Ben
                SetIn_analytics_prediction = 0;
                return;
            }
            if (patient_name == null)
            {
                Console.WriteLine("Patient name is null.  Analytics prediction is exiting. ");
                SetIn_analytics_detection = 0;
                Console.WriteLine("--- Analytics Prediction Timer finished ---\n");
                return;
            }

            System.Diagnostics.Process pProcess = new System.Diagnostics.Process();
            pProcess.StartInfo.FileName = "C:\\AlgorithmAnalytics\\Prediction_Code\\dist\\Hill-Prediction.exe";
            string New_data_dir = FirstCharacterToUpper(data_dir);
            New_data_dir = New_data_dir.Replace(" ", string.Empty);
            string comb_chan = chan_name[0] + ".timd" + "," + chan_name[1] + ".timd" + "," + chan_name[2] + ".timd" + "," + chan_name[3] + ".timd";
            string prediction_ratio = prediction_test_threshold_ratio.ToString();
            pProcess.StartInfo.Arguments = current_Time + " " + New_data_dir + ".mefd/" + " " + comb_chan + " " + prediction_ratio;
            //  pProcess.StartInfo.Arguments = current_Time + " " + "C:/Dropbox/NPC700236H/NPC700236H_1515419491.mefd/" + " " + "[E0-E1.timd,E2-E3.timd,E8-E9.timd,E10-E11.timd]";
            Console.WriteLine(pProcess.StartInfo.Arguments);

            pProcess.StartInfo.UseShellExecute = false;
            pProcess.StartInfo.Verb = "runas";
            pProcess.StartInfo.RedirectStandardOutput = true;
            pProcess.StartInfo.CreateNoWindow = true;

            //set the correct working directory
            pProcess.StartInfo.WorkingDirectory = @"C:\AlgorithmAnalytics\Prediction_Code\dist";

            try
            {
                // Start the process
                pProcess.Start();

                // Wait for process to finish
                pProcess.WaitForExit();

                //Get program output
#if DEBUG
                //Console.WriteLine("Current timestamp is " + unixTimestamp);  
                Console.WriteLine(pProcess.StartInfo.FileName);
                Console.WriteLine(pProcess.StartInfo.Arguments);
                string strOutput = pProcess.StandardOutput.ReadToEnd();
                Console.WriteLine(strOutput);
#endif
                //in_analytics_prediction = 0;

                pProcess.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                SetIn_analytics_prediction = 0;
                Console.WriteLine("--- Analytics Prediction Timer finished ---\n");
                return;
            }
            Int64 unixTimestamp_end = (Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
            var time_end = DateTime.Now;
            string formattedTime_end = time_end.ToString("yyyy/MM/dd hh:mm:ss tt");
            Console.WriteLine("\nPrediction algorithm run starting at " + formattedTime_start + " and finished at " + formattedTime_end + "\n");
            Console.WriteLine("Analytics (prediction) ran successfully from {0} to {1}.", starting_time, current_Time);


            int counter = 0;
            string line;
            string Pre_Prob = "-1"; ///Mona-prediction probability value, str
            float Pre_Prob_fl = -1; ///Mona-prediction probability value, float
            string analytics_output_file = homeFolder + Environment.MachineName + "\\prediction_output_" + patient_name + ".csv";
            StreamWriter sw;
            if (File.Exists(analytics_output_file))
            {
                sw = new StreamWriter(analytics_output_file, true);

            }
            else
            {
                sw = new StreamWriter(analytics_output_file, true);
                sw.WriteLine("start_time, end_time, channel, Event");
            }

            try
            {
                // Read the file and display it line by line.  
                System.IO.StreamReader file = new System.IO.StreamReader(@"c:\\ctm_config\\prediction.txt");
                while ((line = file.ReadLine()) != null)
                {
                    //System.Console.WriteLine(line);
                    sw.WriteLine(line);
                    counter++;
                }
                //file.Close();
                sw.Close();
                file.Close();
                System.IO.StreamReader file1 = new System.IO.StreamReader(@"c:\\ctm_config\\prediction.txt");
                while (file1.Peek() >= 0) ///Mona-reading the probability value from prediction.txt
                {
                    string str_pre;
                    string[] strArray_pre;
                    str_pre = file1.ReadLine();

                    strArray_pre = str_pre.Split(',');

                    //string strArray_pre_picked = strArray_pre[0];
                    //String[] substrings = strArray_pre_picked.Split(' ', '[', ']', ',', '\'');
                    Pre_Prob = strArray_pre[strArray_pre.Length - 1];
                    if (Pre_Prob == "1")
                    {
                        Pre_Prob = "-1";
                    }
                    if (Pre_Prob != "" && Pre_Prob != null)
                    {
                        Pre_Prob_fl = float.Parse(Pre_Prob);
                    }

                } ///Mona-End of reading the probability

                file1.Close();
            }
            catch (FileNotFoundException er)
            {
                Console.WriteLine("Exception: " + er.Message);
            }

            //if prediction probability is greater than threshold   
            double threshold = 0.6;

            if (stimMode == 1) // if we currently want to stim
            {
                if (Pre_Prob_fl < threshold // need to be in Default
                    && !(currState.Equals("Default"))) // if not in Default
                {
                    currState = "Default";
                    SetDoing_stim = true;
                    StartStreaming(1, streamingMode, currState);
                    SetDoing_stim = false;
                }

                if (Pre_Prob_fl >= threshold // need to be in Pre
                    && !(currState.Equals("Pre"))) // if not in Pre
                {
                    currState = "Pre";
                    SetDoing_stim = true;
                    StartStreaming(1, streamingMode, currState);
                    SetDoing_stim = false;
                }

                // sleep state in not implemented
            }

            SetIn_analytics_prediction = 0;
            Console.WriteLine("--- Analytics Prediction Timer finished ---\n");
        }

        private void OnprivacyTimer(object source, System.Timers.ElapsedEventArgs e)
        {
            privacyTimer.Enabled = false;
            privacyTimer.Stop();
            privacyTimer.Dispose();
            privacyTimer = null;

            SetPrivacyMode = false;
            this.homeForm.Setlabel_privacyMode(false);

            CustomMsgBox.Show("Privacy Mode is now Off", "Privacy Mode", "OK");
        }

        public static TdSampleRates SampleRateTextToTdSamplesRates(String text)
        {
            switch (text)
            {
                case "250 Hz":
                    mef3_sampling_rate = 250.0;
                    return TdSampleRates.Sample0250Hz;
                case "500 Hz":
                    mef3_sampling_rate = 500.0;
                    return TdSampleRates.Sample0500Hz;
                case "1000 Hz":
                    mef3_sampling_rate = 1000.0;
                    return TdSampleRates.Sample1000Hz;
                default:
                    mef3_sampling_rate = 250.0;
                    return TdSampleRates.Sample0250Hz;
            }
        }

        public static TdMuxInputs MuxTextToTdMuxInputs(String text)
        {
            switch (text)
            {
                case "E0":
                case "E8":
                    return TdMuxInputs.Mux0;
                case "E1":
                case "E9":
                    return TdMuxInputs.Mux1;
                case "E2":
                case "E10":
                    return TdMuxInputs.Mux2;
                case "E3":
                case "E11":
                    return TdMuxInputs.Mux3;
                case "E4":
                case "E12":
                    return TdMuxInputs.Mux4;
                case "E5":
                case "E13":
                    return TdMuxInputs.Mux5;
                case "E6":
                case "E14":
                    return TdMuxInputs.Mux6;
                case "E7":
                case "E15":
                    return TdMuxInputs.Mux7;
                default:
                    return TdMuxInputs.Mux0;
            }
        }

        public void ReadSettingsFile()
        {
            // set defaults, in case file isn't readable
            loopRecordingInputBox_Text = "1";
            loopBreakInputBox_Text = "1";
            samplingRateBox_Text = "250 Hz";
            channelMinusBox[0] = "E0";
            channelPlusBox[0] = "E3";
            channelMinusBox[1] = "E4";
            channelPlusBox[1] = "E7";
            channelMinusBox[2] = "E8";
            channelPlusBox[2] = "E11";
            channelMinusBox[3] = "E12";
            channelPlusBox[3] = "E15";
            continuousButton_Checked = true;
            periodicButton_Checked = false;

            // now try to read the file
            try
            {
                String path = "c:\\ctm_config\\ctm_config.txt";
                System.IO.StreamReader file = new StreamReader(path);
                dtSensingSettingsFile = File.GetLastWriteTime(path);
                String line;
                if ((line = file.ReadLine()) != null)
                    loopRecordingInputBox_Text = line;
                else
                {
                    file.Close();
                    return;
                }
                if ((line = file.ReadLine()) != null)
                    loopBreakInputBox_Text = line;
                else
                {
                    file.Close();
                    return;
                }
                if ((line = file.ReadLine()) != null)
                    samplingRateBox_Text = line;
                else
                {
                    file.Close();
                    return;
                }
                for (int i = 0; i < 4; i++)
                {
                    if ((line = file.ReadLine()) != null)
                        channelMinusBox[i] = line;
                    else
                    {
                        file.Close();
                        return;
                    }
                    if ((line = file.ReadLine()) != null)
                        channelPlusBox[i] = line;
                    else
                    {
                        file.Close();
                        return;
                    }
                }
                if ((line = file.ReadLine()) != null)
                {
                    if (line.Equals("true"))
                    {

                        continuousButton_Checked = true;
                        periodicButton_Checked = false;
                    }
                    else
                    {
                        continuousButton_Checked = false;
                        periodicButton_Checked = true;
                    }
                }
                else
                {
                    file.Close();
                    return;
                }

                file.Close();

                // Updating app variables

                // set periodic flag at beginning of recording
                isPeriodic = true;
                if (continuousButton_Checked)
                    isPeriodic = false;

                samplingRateBoxString = samplingRateBox_Text;
                channelMinusBoxString = new String[4];
                channelPlusBoxString = new String[4];
                for (int i = 0; i < 4; i++)
                {
                    channelMinusBoxString[i] = channelMinusBox[i];
                    channelPlusBoxString[i] = channelPlusBox[i];
                }

                loopRecordingInputBoxInt = Convert.ToInt32(loopRecordingInputBox_Text);
                loopBreakInputBoxInt = Convert.ToInt32(loopBreakInputBox_Text);

            }
            catch
            {
                Console.WriteLine("Exception during readSettingsFile()");
            }
        }

        public void WriteSettingsFile(SetSensingParamForm paramForm)
        {
            try
            {
                //FileStream overwrite = new FileStream("c:\\ctm_config\\ctm_config.txt", FileMode.CreateNew);
                System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\ctm_config\\ctm_config.txt", false);

                file.WriteLine(paramForm.loopRecordingInputBox.Text);
                file.WriteLine(paramForm.loopBreakInputBox.Text);
                file.WriteLine(paramForm.samplingRateBox.Text);
                for (int i = 0; i < 4; i++)
                {
                    file.WriteLine(paramForm.channelMinusBox[i].Text);
                    file.WriteLine(paramForm.channelPlusBox[i].Text);
                }
                if (paramForm.continuousButton.Checked == true)
                    file.WriteLine("true");
                else
                    file.WriteLine("false");
                file.Close();



            }
            catch (Exception e)
            {
                Console.WriteLine("Exception during writeSettingsFile(): " + e.Message);
            }
        }

        public void UpdateSensingSettingsFile(SetSensingParamForm paramForm)
        {
            // update SetSensingParamForm
            try
            {
                paramForm.Setlabel_settingsTimeText("Setting file last updated : " + (dtSensingSettingsFile.ToString()));

                paramForm.SetloopRecordingInputBoxText(loopRecordingInputBox_Text);
                paramForm.SetloopBreakInputBoxText(loopBreakInputBox_Text);
                paramForm.SetsamplingRateBoxText(samplingRateBox_Text);

                paramForm.SetchannelMinusBox0Text(channelMinusBox[0]);
                paramForm.SetchannelPlusBox0Text(channelPlusBox[0]);
                paramForm.SetchannelMinusBox1Text(channelMinusBox[1]);
                paramForm.SetchannelPlusBox1Text(channelPlusBox[1]);
                paramForm.SetchannelMinusBox2Text(channelMinusBox[2]);
                paramForm.SetchannelPlusBox2Text(channelPlusBox[2]);
                paramForm.SetchannelMinusBox3Text(channelMinusBox[3]);
                paramForm.SetchannelPlusBox3Text(channelPlusBox[3]);

                paramForm.continuousButton.Checked = continuousButton_Checked;
                paramForm.periodicButton.Checked = periodicButton_Checked;

            }
            catch
            {
                Console.WriteLine("Exception during updateSensingSettingsFile()");
                return;
            }
        }

        private void NewTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            int counter = timer_counter;
            timer_counter++;
            if (timer_counter > 999999)
                timer_counter = 0;

            DateTime dt = DateTime.Now;
            String temp_timer_counter = counter + "# on " + String.Format("{0:G}", dt);

            Console.WriteLine("\n--- Timer fired: " + temp_timer_counter + " ---");
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            // check for daily DMP action
            if (previous_timer_day >= 0)
            {
                // Perform action on each new day
                if (dt.Day != previous_timer_day)
                {
                    // close MEF files
                    EndCurrentRecording(false);

                    String logFilepath = homeFolder + Environment.MachineName + "\\DMP Application Logs\\Log_" + Environment.MachineName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt";
                    var dualOutput = new ConsoleFileOutput(logFilepath, Console.Out);
                    Console.SetOut(dualOutput);

                }
            }
            previous_timer_day = dt.Day;

            Console.WriteLine("Streaming Mode: " + streamingMode + ", Recording: " + (!isPeriodic ? "Continuous" : "Periodic"));
            Console.WriteLine("Analytics Mode: " + analyticsMode + (in_analytics_prediction == 1 ? ",  running seizure prediction" : "") + (in_analytics_detection == 1 ? ", running seizure detection" : ""));
            Console.WriteLine("Stimulation Mode: " + stimMode + ", Current Brain State: " + currState + (safeMode ? " Safe Mode is ON" : ""));


            // The following code prints total threads and available threads in the thread pool.
            // Can be helpful in debugging problems where the thread pool is being depleted.
#if DEBUG
            ThreadPool.GetAvailableThreads(out int available_threads, out int io);
            Console.WriteLine("Available Threads: " + available_threads + " " + io);
            ThreadPool.GetMaxThreads(out int max_threads, out io);
            Console.WriteLine("Max Threads: " + max_threads + " " + io);
#endif

            // Read and update tablet battery level
            PowerStatus status = SystemInformation.PowerStatus;
            String TabBatteryLevel = Convert.ToString(Convert.ToInt32(status.BatteryLifePercent * 100));
            SetTabBatteryLevelText(TabBatteryLevel);
            Console.WriteLine("Current Tablet Battery Level: " + TabBatteryLevel + "%");

            // Show warning if no data has been received recently
            Int64 timestamp_now = (Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
            if ((timestamp_now > (last_data_received_timestamp + WARNING_MINUTES_IN_MILLISECONDS)) &&
                (warning_in_progress == false))
            {
                Console.WriteLine(message_no_data + " presented to user on " + String.Format("{0:G}", DateTime.Now));
                SetUserMessage(message_no_data);
            }

            #region exit with doing nothing

            /*
             * exit with doing nothing if -
             *      trying to connect / during setting stim / during imp test / during lad config /
             *      setting updates / during loop recorder pull / streamingMode is false
             */

            if (doing_start_or_stop_button == 1 || in_connect_to_ctm == true)
            {
                Console.WriteLine("Start or Stop button is in progress or already connecting, so don't do anything.");

                Console.WriteLine("--- Timer finished (" + temp_timer_counter + ") ---\n");
                return;
            }

            if (doing_impTest || doing_lda_config || doing_stim || doing_sensing || doing_looprecorder)
            {
                if (doing_impTest)
                    Console.WriteLine("Imp test is in progress, so don't do anything.");
                if (doing_lda_config)
                    Console.WriteLine("LDA configuration is in progress, so don't do anything.");
                if (doing_stim)
                    Console.WriteLine("Stim setting is in progress, so don't do anything.");
                if (doing_sensing)
                    Console.WriteLine("Sensing setting is in progress, so don't do anything.");
                if (doing_looprecorder)
                    Console.WriteLine("Loop recorder uploading in progress, so don't do anything.");

                Console.WriteLine("--- Timer finished (" + temp_timer_counter + ") ---\n");
                return;
            }

            if (streamingMode == 0)
            {
                if (theSummit != null)
                {
                    Console.WriteLine("streamingMode is false and theSummit is not null, so disposing it.");
                    theSummitManager.DisposeSummit(theSummit);
                    theSummit = null;

                    SetCTMconnection = false;
                    SetINSconnection = false;
                }
                else
                    Console.WriteLine("streamingMode is false, so nothing to do");

                Console.WriteLine("--- Timer finished (" + temp_timer_counter + ") ---\n");
                return;
            }

            #endregion

            // try-catch for battery level and status readings (CTM + INS)
            try
            {
                BatteryStatusResult outputBuffer = new BatteryStatusResult();

                Console.WriteLine("Checking CTM battery...");
                bool battery_read = false;

                #region Check connection

                // Ensure we are connected
                if (theSummit != null)
                {
                    if (!(theSummit.IsDisposed))
                    {
                        APIReturnInfo ctm_return_info = theSummit.ReadTelemetryModuleInfo(out TelemetryModuleInfo telem_info);

                        // Ensure the command was successful before using the result
                        if (ctm_return_info.RejectCode == 0)
                        {
                            Console.WriteLine("Current CTM battery level: " + telem_info.BatteryLevel + "%");
                            SetCTMBatteryLevelText(telem_info.BatteryLevel > 50 ? "Good" : "Low"); // update indicators
                            battery_read = true;
                        }
                        else
                        {
                            Console.WriteLine("RejectCode: " + ctm_return_info.RejectCode + " - " + ctm_return_info.RejectCodeType + " - " + ctm_return_info.Descriptor);
                            if (ctm_return_info.RejectCode == 7)
                            {
                                num_reject_code_7_in_a_row++;
                                if (num_reject_code_7_in_a_row < 3)
                                {
                                    Console.WriteLine("Found reject code 7 - igonring, and ending timer.");
                                    Console.WriteLine("--- Timer finished (" + temp_timer_counter + ") ---\n");

                                    return;
                                }
                            }
                        }
                    }
                    else // theSummit.IsDisposed
                        Console.WriteLine("Summit == Disposed");
                }
                else // theSummit == null
                    Console.WriteLine("Summit == null");

                num_reject_code_7_in_a_row = 0;

                #endregion

                #region Not connected
                // if we are not connected properly to the hardware -> reconnect
                if (!battery_read)
                {
                    Console.WriteLine("Battery not read (no Summit session), will try to reconnect to CTM...");

                    if (ConnectToCTM(true))  // reconnect to CTM if checking failed
                    {
                        // if we are in continouous streaming mode, make sure that gets restarted after a reconnection
                        if (!isPeriodic && (streamingMode == 1))
                        {
                            SetDoing_sensing = true;
                            StartStreaming(stimMode, streamingMode, "Default");
                            SetDoing_sensing = false;
                        }
                    }
                    else // ConnectToCTM returned false
                    {
                        Console.WriteLine("ConnectToCTM == false");
                        SetCTMconnection = false;
                        SetINSconnection = false;
                    }

                    Console.WriteLine("--- Timer finished (" + temp_timer_counter + ") ---\n");
                    return;
                }
                #endregion

                // We are connected properly so check INS battery
                Console.WriteLine("Checking INS battery...");
                APIReturnInfo commandInfo = theSummit.ReadBatteryLevel(out outputBuffer);

                // Ensure the command was successful before using the result
                if (commandInfo.RejectCode == 0)
                {
                    string INSbatteryLevel = outputBuffer.BatteryLevelPercent.ToString();
                    theSummit.LogCustomEvent(commandInfo.TxTime, commandInfo.RxTime, "INS BatteryLevel", INSbatteryLevel);
                    Console.WriteLine("Current INS Battery Level: " + INSbatteryLevel + "%");
                    battery_last_timestamp = unixTimestamp;
                    SetINSBatteryLevelText(INSbatteryLevel + "%"); // update indicators

                    #region INS Low battery?

                    // check to see if we dropped be.35%, to warn user to recharge INS
                    if ((last_INS_battery_level > 35) && (Convert.ToInt32(INSbatteryLevel) <= 35))
                    {
                        Console.WriteLine("Warning, INS battery low, charge INS!" + " - presented to user on " + String.Format("{0:G}", DateTime.Now));
                        //SetUserMessage("Warning, INS battery low, charge INS!");
                        Thread t = new Thread(() => MessageBoxSeparateThread("Warning, INS battery low, charge INS!"));
                        t.Start();
                    }

                    // catch transition from INS battery to below 25% -> pop-up message and turn off stim, if necessary
                    if (((last_INS_battery_level > 25) && (Convert.ToInt32(INSbatteryLevel) <= 25)) &&
                        (ins_battery_low_stop_streaming == false))
                    {
                        Console.WriteLine("INS battery below 25%.\nTurning off streaming and, if needed, turning stim settings back to default" + " - presented to user on " + String.Format("{0:G}", DateTime.Now));
                        //SetUserMessage("INS battery below 25%, turning off streaming and, if needed, turning stim settings back to default until INS battery is charged.");
                        Thread t = new Thread(() => MessageBoxSeparateThread("INS battery below 25%, turning off streaming and, if needed, turning stim settings back to default until INS battery is charged."));
                        t.Start();

                        ins_battery_low_stop_streaming = true;

                        //set stim params back to defaults and turn off streaming/sensing
                        SetDoing_stim = true;
                        StartStreaming(stimMode, 0, "Default");
                        SetDoing_stim = false;
                    }

                    // update last INS battery level
                    last_INS_battery_level = Convert.ToInt32(INSbatteryLevel);

                    // if battery gets recharged, turn streaming back on at next battery check timer
                    if ((Convert.ToInt32(INSbatteryLevel) > 25))
                    {
                        ins_battery_low_stop_streaming = false;
                    }
                    else // if below 25 % battery
                    {
                        // if below 25% battery, streaming should be off and stim on Default already
                        Console.WriteLine("Below 25% battery, streaming should be off and stim on Default already.");
                        ins_battery_low_stop_streaming = true;
                    }
                    #endregion

                    #region Check status

                    String path_harddrive = @"C:\";   //We are checking the total free space for drive c: for now.               
                    directory_size = GetPercentageFreeSpace(path_harddrive);
                    Console.WriteLine("{0} drive has {1}% free disk space", path_harddrive, directory_size);
                    if (previous_directory_size > 20 && directory_size <= 20)
                    {
                        //SetUserMessage("!!!Warning!!! Tablet disk space is below 20%. Contact tech support team!!!!");
                        Thread t = new Thread(() => MessageBoxSeparateThread("!!!Warning!!! Tablet disk space is below 20%. Contact tech support team!!!!"));
                        t.Start();
                    }
                    if (previous_directory_size > 10 && directory_size <= 10)
                    {
                        //SetUserMessage("!!!Warning!!! Tablet disk space is below 10%. Contact tech support team!!!!");
                        Thread t = new Thread(() => MessageBoxSeparateThread("!!!Warning!!! Tablet disk space is below 10%. Contact tech support team!!!!"));
                        t.Start();
                    }
                    //update last directory size
                    previous_directory_size = GetPercentageFreeSpace(path_harddrive);

                    // check streaming status
                    APIReturnInfo bufferReturnInfoStreaming = theSummit.ReadSensingStreamState(out StreamState theStreamState);
                    if (bufferReturnInfoStreaming.RejectCode == 0)
                    {
                        Console.WriteLine("Currently streaming: " + theStreamState.TimeDomainStreamEnabled);
                        if (streamingMode == 1)
                        {
                            if (theStreamState.TimeDomainStreamEnabled) // streaming is true
                            {
                                if (ins_battery_low_stop_streaming)
                                { // streaming on low battery - turn off.
                                    Console.WriteLine("Streaming on low battery level - turning OFF");
                                    SetDoing_sensing = true;
                                    StartStreaming(stimMode, 0, "Default");
                                    SetDoing_sensing = false;
                                }
                                // else streaming on regular level - OK
                            }
                            else // !theStreamState.TimeDomainStreamEnabled // streaming is false
                            {
                                if (!ins_battery_low_stop_streaming)
                                { // not streaming on regular battery - turn on.
                                    Console.WriteLine("Not streaming on good battery level - turning ON");
                                    SetDoing_sensing = true;
                                    StartStreaming(stimMode, streamingMode, "Default");
                                    SetDoing_sensing = false;
                                }
                                // else not streaming on low level - OK 
                            }
                        }
                        else // (streamingMode == 0)
                        {
                            if (theStreamState.TimeDomainStreamEnabled)  // streaming is true
                            {// streaming when we don't want to - turn off.
                                Console.WriteLine("Streaming when we don't want to - turning OFF");
                                SetDoing_sensing = true;
                                StartStreaming(stimMode, 0, "Default");
                                SetDoing_sensing = false;
                            }
                            // else not streaming  - OK
                        }
                    }
                    else
                    {
                        Console.WriteLine("Failed reading current streaming status...");
                    }

                    // check LDA status
#if DEBUG
                    APIReturnInfo bufferReturnInfoLDA = theSummit.ReadAdaptiveDetectionSettings(out DetectionConfiguration theDetectionConfigs);
                    if (bufferReturnInfoLDA.RejectCode == 0)
                    {
                        Console.WriteLine("LDA Current Hold Off Time: " + theDetectionConfigs.Lds[0].HoldoffTime.ToString());
                        Console.WriteLine("LDA Current Onset Duration: " + theDetectionConfigs.Lds[0].OnsetDuration.ToString());
                        Console.WriteLine("LDA Current Termination Duration: " + theDetectionConfigs.Lds[0].TerminationDuration.ToString());
                        string currentWeightVectors = "";
                        foreach (var item in theDetectionConfigs.Lds[0].Features)
                        {
                            currentWeightVectors = currentWeightVectors + " " + item.WeightVector.ToString();
                        }
                        Console.WriteLine("LDA Current Weight Vectors: [" + currentWeightVectors + "]");
                    }
#endif

                    // check stim status
                    APIReturnInfo bufferReturnInfo = theSummit.ReadGeneralInfo(out GeneralInterrogateData interrogateBuffer);
                    if (bufferReturnInfo.RejectCode == 0)
                    {
                        TherapyStatusByte therapyStatus = interrogateBuffer.TherapyStatusData;
                        Console.WriteLine("Current Therapy Group and Status: "
                            + therapyStatus.ActiveGroup.ToString() + " " + therapyStatus.TherapyStatus.ToString());

                        // When stim is switched ON settings are read from file and saved to variables ( readStimulationSettingsFile() )
                        // comparing current stim settings on INS to variables will confirm everything is OK
                        if (stimMode == 1) // stimulation should be ON
                        {
                            // compare settings
                            bool settingsOK = true;

                            if (therapyStatus.TherapyStatus == InterrogateTherapyStatusTypes.TherapyOff)
                            { // stimulation should be ON but it is not.. 
                                settingsOK = false;
                                Console.WriteLine("Stimulation should be ON - need to switch settings");
                            }

                            if (therapyStatus.TherapyStatus == InterrogateTherapyStatusTypes.TherapyActive)
                            {  // stimulation is ON and active (not comparing in transtion...)
                                if (therapyStatus.ActiveGroup == ActiveGroup.Group3)
                                {
                                    if (ins_battery_low_stop_streaming)
                                    { // group D on low battery - switch
                                        settingsOK = false;
                                        Console.WriteLine("Set to Group D on low battery level - need to switch settings");
                                    }
                                    else
                                    { // group D on regular battery - verify
                                        Console.WriteLine("Set to Group D on good battery level - verifying settings");

                                        APIReturnInfo stimbufferReturnInfo = theSummit.ReadAdaptiveStimSettings(out AdaptiveSettings currsettings);
                                        if (stimbufferReturnInfo.RejectCode == 0)
                                        {
                                            double currProg0AmpInMilliamps = currsettings.State0.Prog0AmpInMilliamps;
                                            double currProg1AmpInMilliamps = currsettings.State0.Prog1AmpInMilliamps;
                                            double currProg2AmpInMilliamps = currsettings.State0.Prog2AmpInMilliamps;
                                            double currProg3AmpInMilliamps = currsettings.State0.Prog3AmpInMilliamps;
                                            double currRateTargetInHz = currsettings.State0.RateTargetInHz;

                                            if (currState.Equals("Default"))
                                            {
                                                Console.WriteLine("Comparing Default state stimulation settings...");
                                                if (DefaultStateSettings.InitialState.Prog0AmpInMilliamps != currProg0AmpInMilliamps)
                                                {
                                                    settingsOK = false;
                                                    Console.WriteLine("Prog0 set to: " + currProg0AmpInMilliamps.ToString() +
                                                        "\n needs to be: " + DefaultStateSettings.InitialState.Prog0AmpInMilliamps.ToString());
                                                }
                                                if (DefaultStateSettings.InitialState.Prog1AmpInMilliamps != currProg1AmpInMilliamps)
                                                {
                                                    settingsOK = false;
                                                    Console.WriteLine("Prog1 set to: " + currProg1AmpInMilliamps.ToString() +
                                                        "\n needs to be: " + DefaultStateSettings.InitialState.Prog1AmpInMilliamps.ToString());
                                                }
                                                if (DefaultStateSettings.InitialState.Prog2AmpInMilliamps != currProg2AmpInMilliamps)
                                                {
                                                    settingsOK = false;
                                                    Console.WriteLine("Prog2 set to: " + currProg2AmpInMilliamps.ToString() +
                                                        "\n needs to be: " + DefaultStateSettings.InitialState.Prog2AmpInMilliamps.ToString());
                                                }
                                                if (DefaultStateSettings.InitialState.Prog3AmpInMilliamps != currProg3AmpInMilliamps)
                                                {
                                                    settingsOK = false;
                                                    Console.WriteLine("Prog3 set to: " + currProg3AmpInMilliamps.ToString() +
                                                        "\n needs to be: " + DefaultStateSettings.InitialState.Prog3AmpInMilliamps.ToString());
                                                }
                                                if (DefaultStateSettings.InitialState.RateTargetInHz != currRateTargetInHz)
                                                {
                                                    settingsOK = false;
                                                    Console.WriteLine("Rate set to: " + currRateTargetInHz.ToString() +
                                                        "\n needs to be: " + DefaultStateSettings.InitialState.RateTargetInHz.ToString());
                                                }
                                            }
                                            else if (currState.Equals("Sleep"))
                                            {
                                                Console.WriteLine("Comparing Sleep state stimulation settings...");
                                                if (SleepStateSettings.InitialState.Prog0AmpInMilliamps != currProg0AmpInMilliamps)
                                                {
                                                    settingsOK = false;
                                                    Console.WriteLine("Prog0 set to: " + currProg0AmpInMilliamps.ToString() +
                                                        "\n needs to be: " + SleepStateSettings.InitialState.Prog0AmpInMilliamps.ToString());
                                                }
                                                if (SleepStateSettings.InitialState.Prog1AmpInMilliamps != currProg1AmpInMilliamps)
                                                {
                                                    settingsOK = false;
                                                    Console.WriteLine("Prog1 set to: " + currProg1AmpInMilliamps.ToString() +
                                                        "\n needs to be: " + SleepStateSettings.InitialState.Prog1AmpInMilliamps.ToString());
                                                }
                                                if (SleepStateSettings.InitialState.Prog2AmpInMilliamps != currProg2AmpInMilliamps)
                                                {
                                                    settingsOK = false;
                                                    Console.WriteLine("Prog2 set to: " + currProg2AmpInMilliamps.ToString() +
                                                        "\n needs to be: " + SleepStateSettings.InitialState.Prog2AmpInMilliamps.ToString());
                                                }
                                                if (SleepStateSettings.InitialState.Prog3AmpInMilliamps != currProg3AmpInMilliamps)
                                                {
                                                    settingsOK = false;
                                                    Console.WriteLine("Prog3 set to: " + currProg3AmpInMilliamps.ToString() +
                                                        "\n needs to be: " + SleepStateSettings.InitialState.Prog3AmpInMilliamps.ToString());
                                                }
                                                if (SleepStateSettings.InitialState.RateTargetInHz != currRateTargetInHz)
                                                {
                                                    settingsOK = false;
                                                    Console.WriteLine("Rate set to: " + currRateTargetInHz.ToString() +
                                                        "\n needs to be: " + SleepStateSettings.InitialState.RateTargetInHz.ToString());
                                                }

                                            }
                                            else if (currState.Equals("Pre"))
                                            {
                                                Console.WriteLine("Comparing Pre-seizure state stimulation settings...");
                                                if (PreStateSettings.InitialState.Prog0AmpInMilliamps != currProg0AmpInMilliamps)
                                                {
                                                    settingsOK = false;
                                                    Console.WriteLine("Prog0 set to: " + currProg0AmpInMilliamps.ToString() +
                                                        "\n needs to be: " + PreStateSettings.InitialState.Prog0AmpInMilliamps.ToString());
                                                }
                                                if (PreStateSettings.InitialState.Prog1AmpInMilliamps != currProg1AmpInMilliamps)
                                                {
                                                    settingsOK = false;
                                                    Console.WriteLine("Prog1 set to: " + currProg1AmpInMilliamps.ToString() +
                                                        "\n needs to be: " + PreStateSettings.InitialState.Prog1AmpInMilliamps.ToString());
                                                }
                                                if (PreStateSettings.InitialState.Prog2AmpInMilliamps != currProg2AmpInMilliamps)
                                                {
                                                    settingsOK = false;
                                                    Console.WriteLine("Prog2 set to: " + currProg2AmpInMilliamps.ToString() +
                                                        "\n needs to be: " + PreStateSettings.InitialState.Prog2AmpInMilliamps.ToString());
                                                }
                                                if (PreStateSettings.InitialState.Prog3AmpInMilliamps != currProg3AmpInMilliamps)
                                                {
                                                    settingsOK = false;
                                                    Console.WriteLine("Prog3 set to: " + currProg3AmpInMilliamps.ToString() +
                                                        "\n needs to be: " + PreStateSettings.InitialState.Prog3AmpInMilliamps.ToString());
                                                }
                                                if (PreStateSettings.InitialState.RateTargetInHz != currRateTargetInHz)
                                                {
                                                    settingsOK = false;
                                                    Console.WriteLine("Rate set to: " + currRateTargetInHz.ToString() +
                                                        "\n needs to be: " + PreStateSettings.InitialState.RateTargetInHz.ToString());
                                                }
                                            }
                                        }
                                    }
                                }

                                if (therapyStatus.ActiveGroup == ActiveGroup.Group0)
                                {
                                    if (ins_battery_low_stop_streaming || safeMode)
                                    { // group A on low battery - verify
                                        Console.WriteLine("Set to Group A on low battery level or safe mode - verifying settings");


                                        APIReturnInfo stimbufferReturnInfo = theSummit.ReadStimGroup(GroupNumber.Group0, out TherapyGroup groupAsettings);
                                        if (stimbufferReturnInfo.RejectCode == 0)
                                        {
                                            double currProg0AmpInMilliamps = groupAsettings.Programs[0].AmplitudeInMilliamps;
                                            double currProg1AmpInMilliamps = groupAsettings.Programs[1].AmplitudeInMilliamps;
                                            double currProg2AmpInMilliamps = groupAsettings.Programs[2].AmplitudeInMilliamps;
                                            double currProg3AmpInMilliamps = groupAsettings.Programs[3].AmplitudeInMilliamps;
                                            double currRateTargetInHz = groupAsettings.RateInHz;

                                            Console.WriteLine("Comparing Default state stimulation settings...");

                                            if (DefaultStateSettings.InitialState.Prog0AmpInMilliamps != currProg0AmpInMilliamps)
                                            {
                                                settingsOK = false;
                                                Console.WriteLine("Prog0 set to: " + currProg0AmpInMilliamps.ToString() +
                                                    "\n needs to be: " + DefaultStateSettings.InitialState.Prog0AmpInMilliamps.ToString());
                                            }
                                            if (DefaultStateSettings.InitialState.Prog1AmpInMilliamps != currProg1AmpInMilliamps)
                                            {
                                                settingsOK = false;
                                                Console.WriteLine("Prog1 set to: " + currProg1AmpInMilliamps.ToString() +
                                                    "\n needs to be: " + DefaultStateSettings.InitialState.Prog1AmpInMilliamps.ToString());
                                            }
                                            if (DefaultStateSettings.InitialState.Prog2AmpInMilliamps != currProg2AmpInMilliamps)
                                            {
                                                settingsOK = false;
                                                Console.WriteLine("Prog2 set to: " + currProg2AmpInMilliamps.ToString() +
                                                    "\n needs to be: " + DefaultStateSettings.InitialState.Prog2AmpInMilliamps.ToString());
                                            }
                                            if (DefaultStateSettings.InitialState.Prog3AmpInMilliamps != currProg3AmpInMilliamps)
                                            {
                                                settingsOK = false;
                                                Console.WriteLine("Prog3 set to: " + currProg3AmpInMilliamps.ToString() +
                                                    "\n needs to be: " + DefaultStateSettings.InitialState.Prog3AmpInMilliamps.ToString());
                                            }
                                            if (DefaultStateSettings.InitialState.RateTargetInHz != currRateTargetInHz)
                                            {
                                                settingsOK = false;
                                                Console.WriteLine("Rate set to: " + currRateTargetInHz.ToString() +
                                                    "\n needs to be: " + DefaultStateSettings.InitialState.RateTargetInHz.ToString());
                                            }
                                        }
                                    }
                                    else
                                    { // group A on regular battery
                                        if (!safeMode) // not safe mode -switch
                                        {
                                            Console.WriteLine("Set to Group A on regular battery - need to switch settings");
                                            settingsOK = false;
                                        }
                                    }
                                }
                            }

                            if (settingsOK)
                            {
                                Console.WriteLine("Current stimulation settings verified!");
                            }
                            else
                            {
                                Console.WriteLine("Restarting stimulation!");
                                // restart stim
                                StartStim(null);
                            }
                        }
                        else // stimMode == 0
                        {
                            // Double check stim is off or in transition to off
                            if (!(therapyStatus.TherapyStatus == InterrogateTherapyStatusTypes.TherapyOff) ||
                                 !(therapyStatus.TherapyStatus == InterrogateTherapyStatusTypes.TransitionToOff))
                            {
                                // STOP STIM NOW
                                Console.WriteLine("Stimulation should be OFF!");
                                StopStim(null);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Failed reading current stimulation status...");
                    }

                    #endregion
                }
                else // INS battery read failed
                {
                    Console.WriteLine("Failed reading Current INS Battery Level");
                    Console.WriteLine("RejectCode: " + commandInfo.RejectCode + " - " + commandInfo.RejectCodeType + " - " + commandInfo.Descriptor);

                    // According to conversation with Medtronic, the two following reject codes indicate that the API
                    // is auto-attempting to to re-establish the connection.  So this process should not be interrupted.
                    // Only when auto-reconnect has failed, should manul reconnection begin.
                    if (!(commandInfo.RejectCode == (int)(APIRejectCodes.NoCtmConnected) ||
                        commandInfo.RejectCode == (int)(APIRejectCodes.NoInsConnected)))
                    {
                        if (ConnectToCTM(true))  // reconnect to CTM if checking failed, but not if already autoreconnecting
                        {
                            // if we are in continouous streaming mode, make sure that gets restarted after a reconnection
                            if (!isPeriodic && (streamingMode == 1))
                            {
                                SetDoing_sensing = true;
                                StartStreaming(stimMode, streamingMode, "Default");
                                SetDoing_sensing = false;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Auto-reconnect in progress detected.");
                        SetCTMconnection = false;
                        SetINSconnection = false;
                    }
                }

                Console.WriteLine("--- Timer finished (" + temp_timer_counter + ") ---\n");

            }
            catch (Exception ex)
            {
                Console.WriteLine("\nException during NewTimer_Elapsed() " + ex.Message);

                Console.WriteLine("--- Timer finished (" + temp_timer_counter + ") ---\n");
                return;

            }
        }

        public bool ConnectToCTM(bool repeat_forever)
        {
            SetCTMconnection = false;
            SetINSconnection = false;

            // We never want to try to connect if we are already trying to connect...
            if (in_connect_to_ctm == true)
            {
                Console.WriteLine("Already connecting, so don't retry");
                return false;
            }

            int count;
            SetIn_connect_to_ctm = true;

            try
            {
                // Initialize the Summit Interface
                Console.WriteLine("Creating Summit Interface...");

                // Create a manager
                if (theSummitManager == null)
                    theSummitManager = new SummitManager(summitProjectID, 200, false);

                // explicitly dispose summit, just in case we are in a weird state, so we're not connecting
                // while already connecting.
                if (theSummit != null)
                    theSummitManager.DisposeSummit(theSummit);

                theSummit = null;

                count = 0;

                while (theSummit == null)
                {

                    if (!SummitConnect(theSummitManager))
                    {
                        Console.WriteLine("Couldn't create Summit.");
                        System.Threading.Thread.Sleep(5000);

                        if (!repeat_forever)
                        {
                            //theSummitManager.DisposeSummit(theSummit);
                            SetIn_connect_to_ctm = false;
                            return false;
                        }

                        count++;
                        if (count >= 2)
                        {
                            Console.WriteLine("Couldn't create summit more than 2 times, bailing out");
                            //theSummitManager.DisposeSummit(theSummit);
                            SetIn_connect_to_ctm = false;
                            return false;
                        }
                    }
                }
                handlers_added = 0;

                // Read the battery level
                Console.WriteLine("Checking battery...");

                APIReturnInfo ctm_return_info;
                ctm_return_info = theSummit.ReadTelemetryModuleInfo(out TelemetryModuleInfo telem_info);
                // Ensure the command was successful before using the result
                if (ctm_return_info.RejectCode == 0)
                {
                    Console.WriteLine("CTM Battery level: " + telem_info.BatteryLevel + "%");
                    SetCTMBatteryLevelText(telem_info.BatteryLevel > 50 ? "Good" : "Low");
                }

                APIReturnInfo commandInfo;
                commandInfo = theSummit.ReadBatteryLevel(out BatteryStatusResult outputBuffer);

                // Ensure the command was successful before using the result
                if (commandInfo.RejectCode == 0)
                {
                    string batteryLevel = outputBuffer.BatteryLevelPercent.ToString();
                    theSummit.LogCustomEvent(commandInfo.TxTime, commandInfo.TxTime, "BatteryLevel", batteryLevel);
                    Console.WriteLine("INS Battery Level: " + batteryLevel + "%");
                    SetINSBatteryLevelText(batteryLevel + "%");

                    // if battery gets recharged, turn streaming back on
                    if ((Convert.ToInt32(batteryLevel) > 10))
                    {
                        ins_battery_low_stop_streaming = false;
                    }
                }

                Console.WriteLine("Done ConnectToCTM, successful");
            }
            catch (Exception)
            {
                SetIn_connect_to_ctm = false;
                throw;
            }

            SetIn_connect_to_ctm = false;

            SetINSconnection = true;
            SetCTMconnection = true;

            return true;
        }

        private void SetCTMBatteryLevelText(string text)
        {
            CTMBattery = text;
            m_eventGen.TriggerEventCTM();
        }

        private void SetINSBatteryLevelText(string text)
        {
            INSBattery = text;
            m_eventGen.TriggerEventINS();
        }

        private void SetTabBatteryLevelText(string text)
        {
            TabBattery = text;
            m_eventGen.TriggerEventTab();
        }

        private void SetUserMessage(string text)
        {
            UserMessage = text;
            m_eventGen.TriggerEventMessage();
        }

        public void StartStreaming(int withStim, int withStreaming, String currentAdaptiveSettings)
        {
            /*
             * startStreaming(0,0, currentAdaptiveSettings) - STOP sensing/streaming data and stimulation
             * startStreaming(0,1,currentAdaptiveSettings) - only sensing/streaming data
             * startStreaming(1,0, currentAdaptiveSettings) - only stimulation
             * startStreaming(1,1, currentAdaptiveSettings) - sensing/streaming data and stimulation

             * 1. Ensure we are not currently in embedded and ensure sensing is off before configuring.
             * 2. Sensing settings
             * 3. Make Group A/D Active
             * 4. Detector settings
             * 5. Adaptive sitmulation settings
             * 6. if stiming - Start adaptive stimulation with sensing
             *    else - Start sensing
             * 7. Register the data listeners
             */

            Console.WriteLine("\n --- startStreaming(" + (withStim==1 ? "Stimulation ON" : "Stimulation OFF") + " , " +
                (withStreaming == 1 ? "Streaming ON" : "Streaming OFF") + ") - " + String.Format("{0:G}", DateTime.Now) + " --- ");

            // Not connected to hardware
            if (theSummit == null || theSummit.IsDisposed)
            {
                Console.WriteLine("startStreaming() not enabled because hardware is not connected");
                return;
            }

            // get stimulation settings
            AdaptiveSettings adaptiveSettings = new AdaptiveSettings();
            if (withStim == 1)
            { // invaild state
                if (currentAdaptiveSettings.Equals("Default"))
                    adaptiveSettings = DefaultStateSettings;
                else if (currentAdaptiveSettings.Equals("Sleep"))
                    adaptiveSettings = SleepStateSettings;
                else if (currentAdaptiveSettings.Equals("Pre"))
                    adaptiveSettings = PreStateSettings;
                else if (currentAdaptiveSettings.Equals("Off"))
                {
                    withStim = 0;
                    Console.WriteLine("Stimulation is not enabled because stim state is set to Off");
                }
                else
                {
                    Console.WriteLine("Stimulation is not enabled because stim state does not exist");
                    withStim = 0;
                }
            }

            // low INS battery level
            if (ins_battery_low_stop_streaming)
            {
                withStreaming = 0; // no streaming
                adaptiveSettings = DefaultStateSettings; // stimulation set to default
                Console.WriteLine("withStreaming is set to 0 and stim state to Default because INS battery is below 25%");
            }

            // List of APIReturnInfo objects for tracking rejects throughout the program
            List<APIReturnInfo> bufferReturnInfo = new List<APIReturnInfo>();

            try
            {
                // Ensure we are not currently in embedded.
                Console.WriteLine("Turning off Stim and Sensing for Configuration...");

                bufferReturnInfo.Add(theSummit.StimChangeTherapyOff(false));
                if (bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0)
                    Console.WriteLine("Initial Therapy Off Status:" + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                else
                    throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                // Therapy must be off before turning adaptive mode off, wait for the previous stim off command to take effect. 
                Thread.Sleep(250);
                bufferReturnInfo.Add(theSummit.WriteAdaptiveMode(AdaptiveTherapyModes.Disabled));
                if (bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0)
                    Console.WriteLine("Initial Adaptive Mode (Disable):" + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                else
                    throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);

                // Ensure sensing is off before configuring
                // Turn off sensing so we can config sensing
                bufferReturnInfo.Add(theSummit.WriteSensingDisableStreams(true, true, true, true, true, true, true, true));
                if (bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0)
                    Console.WriteLine("Initial Streaming Off Status:" + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                else
                    throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);

                bufferReturnInfo.Add(theSummit.WriteSensingState(SenseStates.None, 0x00));
                if (bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0)
                    Console.WriteLine("Initial Sensing Off Status:" + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                else
                    throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);

                #region Sensing Settings

                // Attempt to configure the INS sensing
                Console.WriteLine("Writing sensing configuration...");

                chan_name = new string[4];
                for (int i = 0; i < 4; i++)
                    chan_name[i] = channelMinusBoxString[i] + "-" + channelPlusBoxString[i];

                ch_names = chan_name;
                // ******************* Create a sensing configuration for Time Domain channels *******************
                List<TimeDomainChannel> TimeDomainChannels = new List<TimeDomainChannel>(4);
                //TdSampleRates the_sample_rate = TdSampleRates.Sample0250Hz;
                TdSampleRates the_sample_rate = SampleRateTextToTdSamplesRates(samplingRateBoxString);

                // First Channel Specific configuration: Channels 0 and 1 are Bore 0.
                // Sample rate must be consistent across all TD channels or disabled for individuals.
                // Channel differentially senses from contact 0 to contact 1
                // Low pass filter of 100Hz applied.
                // Second low pass filter also at 100Hz applied
                // High pass filter at 8Hz applied.
                TimeDomainChannels.Add(new TimeDomainChannel(
                                                             the_sample_rate,
                                                             //TdMuxInputs.Mux0,
                                                             //TdMuxInputs.Mux1,
                                                             MuxTextToTdMuxInputs(channelMinusBoxString[0]),
                                                             MuxTextToTdMuxInputs(channelPlusBoxString[0]),
                                                             TdEvokedResponseEnable.Standard,
                                                             TdLpfStage1.Lpf100Hz,
                                                             TdLpfStage2.Lpf100Hz,
                                                             TdHpfs.Hpf8_6Hz));

                // Second Channel Specific configuration: Channels 0 and 1 are Bore 0.
                // Sample rate must be consistent across all TD channels or disabled for individuals.
                // Channel differentially senses from contact 2 to contact 3
                // Low pass filter of 100Hz applied.
                // Second low pass filter also at 100Hz applied
                // High pass filter at 8Hz applied.
                TimeDomainChannels.Add(new TimeDomainChannel(
                                                             the_sample_rate,
                                                             //TdMuxInputs.Mux2,
                                                             //TdMuxInputs.Mux3,
                                                             MuxTextToTdMuxInputs(channelMinusBoxString[1]),
                                                             MuxTextToTdMuxInputs(channelPlusBoxString[1]),
                                                             TdEvokedResponseEnable.Standard,
                                                             TdLpfStage1.Lpf100Hz,
                                                             TdLpfStage2.Lpf100Hz,
                                                             TdHpfs.Hpf8_6Hz));

                // Third Channel Specific configuration: Channels 2 and 3 are Bore 1.
                // Sample rate must be consistent across all TD channels or disabled for individuals.
                // Channel differentially senses from contact 8 to contact 9 (Mux values indexed per bore)
                // Low pass filter of 100Hz applied.
                // Second low pass filter also at 100Hz applied
                // High pass filter at 8Hz applied.
                TimeDomainChannels.Add(new TimeDomainChannel(
                                                             the_sample_rate,
                                                             //TdMuxInputs.Mux0,
                                                             //TdMuxInputs.Mux1,
                                                             MuxTextToTdMuxInputs(channelMinusBoxString[2]),
                                                             MuxTextToTdMuxInputs(channelPlusBoxString[2]),
                                                             TdEvokedResponseEnable.Standard,
                                                             TdLpfStage1.Lpf100Hz,
                                                             TdLpfStage2.Lpf100Hz,
                                                             TdHpfs.Hpf8_6Hz));

                // Third Channel Specific configuration: Channels 2 and 3 are Bore 1.
                // Sample rate must be consistent across all TD channels or disabled for individuals.
                // Channel differentially senses from contact 10 to contact 11 (Mux values indexed per bore)
                // Low pass filter of 100Hz applied.
                // Second low pass filter also at 100Hz applied
                // High pass filter at 8Hz applied.
                TimeDomainChannels.Add(new TimeDomainChannel(
                                                             the_sample_rate,
                                                             //TdMuxInputs.Mux2,
                                                             //TdMuxInputs.Mux3,
                                                             MuxTextToTdMuxInputs(channelMinusBoxString[3]),
                                                             MuxTextToTdMuxInputs(channelPlusBoxString[3]),
                                                             TdEvokedResponseEnable.Standard,
                                                             TdLpfStage1.Lpf100Hz,
                                                             TdLpfStage2.Lpf100Hz,
                                                             TdHpfs.Hpf8_6Hz));

                // ******************* Set up the FFT *******************
                // Create a 256-element FFT that triggers every half second. Use a Hann window and stream all of the bins.

                ///********************Updating the FFT values******************************//               

                FftConfiguration fftChannel = new FftConfiguration
                {
                    Size = FftSizes.Size1024,
                    Interval = 500,// in milliseconds.
                    WindowEnabled = true,
                    WindowLoad = FftWindowAutoLoads.Hann100,
                    //BandFormationConfig = FftWeightMultiplies.Shift7,
                    StreamSizeBins = 0,
                    StreamOffsetBins = 0
                };
                ///Following equations are defined based on discussion within the group. 

                ushort input1_band1_lower_bin = (ushort)Math.Round(FFT_Size * Input1[0] / mef3_sampling_rate);//Input values are from UI
                ushort input1_band1_uppper_bin = (ushort)(Math.Round(FFT_Size * Input1[1] / mef3_sampling_rate) - 1);//1 is subtracted to compensate the bin which start at 0.
                ushort input1_band2_lower_bin = (ushort)Math.Round(FFT_Size * Input1[2] / mef3_sampling_rate);
                ushort input1_band2_uppper_bin = (ushort)(Math.Round(FFT_Size * Input1[3] / mef3_sampling_rate) - 1);

                ushort input2_band1_lower_bin = (ushort)Math.Round(FFT_Size * Input2[0] / mef3_sampling_rate);
                ushort input2_band1_uppper_bin = (ushort)(Math.Round(FFT_Size * Input2[1] / mef3_sampling_rate) - 1);
                ushort input2_band2_lower_bin = (ushort)Math.Round(FFT_Size * Input2[2] / mef3_sampling_rate);
                ushort input2_band2_uppper_bin = (ushort)(Math.Round(FFT_Size * Input2[3] / mef3_sampling_rate) - 1);

                //Modified inputs.
                double frequency_equivalent_half_bin_value = 0.5 * mef3_sampling_rate / FFT_Size;
                Input1_Corrected[0] = frequency_equivalent_half_bin_value + (mef3_sampling_rate * input1_band1_lower_bin / FFT_Size);
                Input1_Corrected[1] = frequency_equivalent_half_bin_value + (mef3_sampling_rate * (input1_band1_uppper_bin + 1) / FFT_Size);
                Input1_Corrected[2] = frequency_equivalent_half_bin_value + (mef3_sampling_rate * input1_band2_lower_bin / FFT_Size);
                Input1_Corrected[3] = frequency_equivalent_half_bin_value + (mef3_sampling_rate * (input1_band2_uppper_bin + 1) / FFT_Size);

                Input2_Corrected[0] = frequency_equivalent_half_bin_value + (mef3_sampling_rate * input2_band1_lower_bin / FFT_Size);
                Input2_Corrected[1] = frequency_equivalent_half_bin_value + (mef3_sampling_rate * (input2_band1_uppper_bin + 1) / FFT_Size);
                Input2_Corrected[2] = frequency_equivalent_half_bin_value + (mef3_sampling_rate * input2_band2_lower_bin / FFT_Size);
                Input2_Corrected[3] = frequency_equivalent_half_bin_value + (mef3_sampling_rate * (input2_band2_uppper_bin + 1) / FFT_Size);
#if DEBUG
                for(int i=0;i<4;i++)
                {
                    Console.WriteLine("Input 1: {0}; Corrected Inputs 1: {1}", Input1[i],Input1_Corrected[i]);
                    Console.WriteLine("Input 2: {0}; Corrected Inputs 2: {1}", Input2[i], Input2_Corrected[i]);
                }
#endif

                // ******************* Set up the Power channels *******************
                // Set up two power summation channels per time domain channel, use various bands for each.        

                List<PowerChannel> powerChannels = new List<PowerChannel>();
                for (int i = 0; i < vector_size; i++)
                {
                    if (input_channel_index[0] == i)
                    {
                        powerChannels.Add(new PowerChannel(input1_band1_lower_bin, input1_band1_uppper_bin, input1_band2_lower_bin, input1_band2_uppper_bin));
                    }
                    else if (input_channel_index[1] == i)
                    {
                        powerChannels.Add(new PowerChannel(input2_band1_lower_bin, input2_band1_uppper_bin, input2_band2_lower_bin, input2_band2_uppper_bin));
                    }
                    else
                    {
                        powerChannels.Add(new PowerChannel(1, 2, 3, 4));
                    }
                }

                // Enable the calculation of both bands per time domain channel.                
                BandEnables theBandEnables = BandEnables.Ch0Band0Enabled | BandEnables.Ch1Band0Enabled | BandEnables.Ch2Band0Enabled | BandEnables.Ch3Band0Enabled |
                    BandEnables.Ch0Band1Enabled | BandEnables.Ch1Band1Enabled | BandEnables.Ch2Band1Enabled | BandEnables.Ch3Band1Enabled;

                // ******************* Set up the miscellaneous settings *******************
                // Stream time domain data every 50ms.
                // Disable the loop recorder
                MiscellaneousSensing miscsettings = new MiscellaneousSensing
                {
                    StreamingRate = StreamingFrameRate.Frame50ms,
                    //LrTriggers = LoopRecordingTriggers.None
                    LrPostBufferTime = 40,
                    //LrTriggers = LoopRecordingTriggers.State0 | LoopRecordingTriggers.State1 |
                    //LoopRecordingTriggers.State2 | LoopRecordingTriggers.State3 |
                    //LoopRecordingTriggers.State4 | LoopRecordingTriggers.State5 |
                    //LoopRecordingTriggers.State6 | LoopRecordingTriggers.State7 |
                    //LoopRecordingTriggers.State8
                    LrTriggers = LoopRecordingTriggers.State1

                };

                // ******************* Write the sensing configuration to the device *******************
                Console.WriteLine("Writing sense configuration...");

                bufferReturnInfo.Add(theSummit.WriteSensingTimeDomainChannels(TimeDomainChannels));
                if (bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0)

                    Console.WriteLine("Write TD Config Status: " + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                else
                    throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                bufferReturnInfo.Add(theSummit.WriteSensingFftSettings(fftChannel));
                if (bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0)
                    Console.WriteLine("Write FFT Config Status: " + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                else
                    throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                bufferReturnInfo.Add(theSummit.WriteSensingPowerChannels(theBandEnables, powerChannels));
                if (bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0)
                    Console.WriteLine("Write Power Config Status: " + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                else
                    throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                bufferReturnInfo.Add(theSummit.WriteSensingMiscSettings(miscsettings));
                if (bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0)
                    Console.WriteLine("Write Misc Config Status: " + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                else
                    throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                bufferReturnInfo.Add(theSummit.WriteSensingAccelSettings(AccelSampleRate.Sample32));
                if (bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0)
                    Console.WriteLine("Write Accel Config Status: " + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                else
                    throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);

                #endregion

                #region Active Group (Safe Mode)

                if (safeMode || ins_battery_low_stop_streaming) // low INS battery or safe mode stim
                {   // Make Group A Active
                    // Change the active group to a different one specified by the function argument
                    bufferReturnInfo.Add(theSummit.StimChangeActiveGroup(ActiveGroup.Group0));
                    if (bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0)
                        Console.WriteLine("Make Group A Active:" + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                    else
                        throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                }
                else
                { // Group D is active only to allow LDA's to control stimulation (to enable Embedded)

                    // Make Group D Active
                    // Change the active group to a different one specified by the function argument
                    bufferReturnInfo.Add(theSummit.StimChangeActiveGroup(ActiveGroup.Group3));
                    if (bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0)
                        Console.WriteLine("Make Group D Active:" + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                    else
                        throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);

                }

                #endregion

                #region Detector configuration

                Console.WriteLine("Writing detector configuration...");
                configLd0.DetectionEnable = DetectionEnables.None; //No enables are set. This means that single threshold detect mode is used

                if ((input_channel_index[0] == 0 && input_channel_index[1] == 1) || (input_channel_index[0] == 1 && input_channel_index[1] == 0))
                {
                    configLd0.DetectionInputs = DetectionInputs.Ch0Band0 | DetectionInputs.Ch0Band1 | DetectionInputs.Ch1Band0 | DetectionInputs.Ch1Band1;
                }
                else if ((input_channel_index[0] == 0 && input_channel_index[1] == 2) || (input_channel_index[0] == 2 && input_channel_index[1] == 0))
                {
                    configLd0.DetectionInputs = DetectionInputs.Ch0Band0 | DetectionInputs.Ch0Band1 | DetectionInputs.Ch2Band0 | DetectionInputs.Ch2Band1;
                }
                else if ((input_channel_index[0] == 0 && input_channel_index[1] == 3) || (input_channel_index[0] == 3 && input_channel_index[1] == 0))
                {
                    configLd0.DetectionInputs = DetectionInputs.Ch0Band0 | DetectionInputs.Ch0Band1 | DetectionInputs.Ch3Band0 | DetectionInputs.Ch3Band1;
                }
                else if ((input_channel_index[0] == 1 && input_channel_index[1] == 2) || (input_channel_index[0] == 2 && input_channel_index[1] == 1))
                {
                    configLd0.DetectionInputs = DetectionInputs.Ch1Band0 | DetectionInputs.Ch1Band1 | DetectionInputs.Ch2Band0 | DetectionInputs.Ch2Band1;
                }
                else if ((input_channel_index[0] == 1 && input_channel_index[1] == 3) || (input_channel_index[0] == 3 && input_channel_index[1] == 1))
                {
                    configLd0.DetectionInputs = DetectionInputs.Ch1Band0 | DetectionInputs.Ch1Band1 | DetectionInputs.Ch3Band0 | DetectionInputs.Ch3Band1;
                }
                else if ((input_channel_index[0] == 2 && input_channel_index[1] == 3) || (input_channel_index[0] == 3 && input_channel_index[1] == 2))
                {
                    configLd0.DetectionInputs = DetectionInputs.Ch2Band0 | DetectionInputs.Ch2Band1 | DetectionInputs.Ch3Band0 | DetectionInputs.Ch3Band1;
                }


                // Update LD state every 8 FFT period
                configLd0.UpdateRate = LD0_UpdateRate;
                configLd0.OnsetDuration = LD0_Onset_Duration;
                configLd0.TerminationDuration = LD0_Termination_Duration;
                configLd0.HoldoffTime = LD0_HoldOff_Time;
                configLd0.BlankingDurationUponStateChange = LD0_Blanking_Duration_Upon_StateChange;

                // Set the weight vectors for the power inputs, since only one channel is used rest can be zero.
                // But we choose 1 for now.
                for (int i = 0; i < vector_size; i++)
                {
                    configLd0.Features[i].WeightVector = Weight_Vector[i];
                    configLd0.Features[i].NormalizationMultiplyVector = Normalization_Multiply_Vector[i];
                    configLd0.Features[i].NormalizationSubtractVector = Normalization_Subtract_Vector[i];
                }

                // Set the thresholds
                configLd0.BiasTerm[0] = LDA_Threshold;
                configLd0.BiasTerm[1] = LDA_Threshold + 10000;
                /* We added 10,000 after discussion with Ben.
                   Since, the configuration doesn't use Upper Threshold so we are leaving as it is for now.
                   We can't leave it blank as the code need the value. */

                // Set the fixed point value
                configLd0.FractionalFixedPointValue = LD0_Fractional_FixedPointValue;
                // Write the detector down to the INS
                bufferReturnInfo.Add(theSummit.WriteAdaptiveDetectionParameters(0, configLd0));
                if (bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0)
                    Console.WriteLine("Write Detector 0 Command ReturnInfo:" + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                else
                    throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);

                // Disable the second LD

                /* !!! CONFIRM WITH JEFF!!!!!
                
                LinearDiscriminantConfiguration configLd1 = new LinearDiscriminantConfiguration
                {
                    // Enable dual threshold mode - LD output can be high, in-range, or below the two thesholds defined in the bias term.
                    // Enable blank both mode - blank both LDs upon a state change of either one
                    DetectionEnable = DetectionEnables.None,//Single threshold detection is used. Only this LD is blanked.
                                                            // configLd1.DetectionEnable = DetectionEnables.DualThresholdEnabled | DetectionEnables.BlankBoth;

                    // This LD will only use ch0 power band 0 for simplicty
                    DetectionInputs = DetectionInputs.Ch0Band0,
                    // Update LD state every 8 FFT period
                    UpdateRate = 8,
                    // Set other timing parameters
                    OnsetDuration = 1,
                    TerminationDuration = 1,
                    HoldoffTime = 3,
                    BlankingDurationUponStateChange = 11
                };
                // Set the weight vectors for the power inputs, since only one channel is used rest can be zero.
                configLd1.Features[0].WeightVector = 16534;
                configLd1.Features[1].WeightVector = 0;
                configLd1.Features[2].WeightVector = 0;
                configLd1.Features[3].WeightVector = 0;
                // Set the normalization vectors for the power inputs, since only one channel is used rest can be zero. 
                configLd1.Features[0].NormalizationMultiplyVector = 598;
                configLd1.Features[1].NormalizationMultiplyVector = 0;
                configLd1.Features[2].NormalizationMultiplyVector = 0;
                configLd1.Features[3].NormalizationMultiplyVector = 0;
                // Set the normalization subtract vectors for the power inputs
                configLd1.Features[0].NormalizationSubtractVector = 0;
                configLd1.Features[0].NormalizationSubtractVector = 0;
                configLd1.Features[0].NormalizationSubtractVector = 0;
                configLd1.Features[0].NormalizationSubtractVector = 0;

                // Set the thresholds
                configLd1.BiasTerm[0] = 1000;
                configLd1.BiasTerm[1] = 1000;
                // Set the fixed point value
                configLd1.FractionalFixedPointValue = 8;

                // Write the detector down to the INS
                bufferReturnInfo.Add(theSummit.WriteAdaptiveDetectionParameters(1, configLd1));
                if (bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0)
                    Console.WriteLine("Write Detector 1 Command ReturnInfo:" + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                else
                    throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);

                */

                #endregion

                if (withStim == 1) // if with stim
                {
                    #region Adaptive Settings

                    // Set the Adaptive Mode, make sure it is disabled
                    Console.WriteLine("Writing adaptive settings...");

                    // Clear settings
                    bufferReturnInfo.Add(theSummit.WriteAdaptiveClearSettings(AdaptiveClearTypes.All, 0));
                    if (bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0)
                        Console.WriteLine("Clear Adaptive Settings Command:" + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                    else
                        throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);

                    // Deltas
                    bufferReturnInfo.Add(theSummit.WriteAdaptiveDeltas(adaptiveSettings.Deltas));
                    if (bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0)
                        Console.WriteLine("Write Delta Limits Command:" + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                    else
                        throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);

                    // States
                    AdaptiveState setState = adaptiveSettings.InitialState;
                    AdaptiveState seizureState = SeizureStateSettings.InitialState;

                    // 1 LD with 1 threshold
                    bufferReturnInfo.Add(theSummit.WriteAdaptiveState(0, setState));
                    if ((bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0))
                        Console.WriteLine("WriteAdaptiveState 0:" + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                    else
                        throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                    bufferReturnInfo.Add(theSummit.WriteAdaptiveState(1, seizureState));
                    if ((bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0))
                        Console.WriteLine("WriteAdaptiveState 1:" + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                    else
                        throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);

                    // if not all state are set WriteAdaptiveMode commands
                    // returns an error - " ChangeMode (0x32): 0x14 - A state is not valid "
                    bufferReturnInfo.Add(theSummit.WriteAdaptiveState(2, setState));
                    if ((bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0))
                        Console.WriteLine("WriteAdaptiveState 0:" + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                    else
                        throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                    bufferReturnInfo.Add(theSummit.WriteAdaptiveState(3, setState));
                    if ((bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0))
                        Console.WriteLine("WriteAdaptiveState 0:" + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                    else
                        throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                    bufferReturnInfo.Add(theSummit.WriteAdaptiveState(4, setState));
                    if ((bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0))
                        Console.WriteLine("WriteAdaptiveState 0:" + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                    else
                        throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                    bufferReturnInfo.Add(theSummit.WriteAdaptiveState(5, setState));
                    if ((bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0))
                        Console.WriteLine("WriteAdaptiveState 0:" + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                    else
                        throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                    bufferReturnInfo.Add(theSummit.WriteAdaptiveState(6, setState));
                    if ((bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0))
                        Console.WriteLine("WriteAdaptiveState 0:" + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                    else
                        throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                    bufferReturnInfo.Add(theSummit.WriteAdaptiveState(7, setState));
                    if ((bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0))
                        Console.WriteLine("WriteAdaptiveState 0:" + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                    else
                        throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                    bufferReturnInfo.Add(theSummit.WriteAdaptiveState(8, setState));
                    if ((bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0))
                        Console.WriteLine("WriteAdaptiveState 0:" + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                    else
                        throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);


                    #endregion

                    #region Start up Embmedded

                    // Set the Adaptive Mode, make sure it is disabled
                    Console.WriteLine("Starting up adaptive stimulation... \nSetting - " + currentAdaptiveSettings + " state");

                    // Start sensing and streaming

                    if (withStreaming == 1) // Stimualation and streaming
                    {
                        // ******************* Turn on Sensing Components *******************
                        // Configures the INS to enable or disable specific sensing components.
                        bufferReturnInfo.Add(theSummit.WriteSensingState(SenseStates.LfpSense | SenseStates.Fft | SenseStates.Power | SenseStates.DetectionLd0 | SenseStates.LoopRecording, 0x00));
                        if (bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0)
                            Console.WriteLine("Set Sense State Command ReturnInfo:" + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                        else
                            throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);

                        // ******************* Start streaming *******************
                        // Configures which sensors to enable streaming data from

                        // power needs to be enabled based on fftflag from LDA GUI
                        bufferReturnInfo.Add(theSummit.WriteSensingEnableStreams(true, false, fft_check_status, true, true, true, true, true));
                        if (bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0)
                            Console.WriteLine("Start Streaming Command ReturnInfo:" + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                        else
                            throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                    }
                    else
                    {
                        // Only Stimulation (No Streaming)

                        // make sure we are not in these situation (because we need to completley turn off everything in it)
                        // otherwise , we are only sensing and Loop recorder needs these streames

                        if (!doing_impTest && !doing_lda_config && !doing_looprecorder)
                        {
                            bufferReturnInfo.Add(theSummit.WriteSensingState(SenseStates.LfpSense | SenseStates.Fft | SenseStates.Power | SenseStates.DetectionLd0 | SenseStates.LoopRecording, 0x00));
                            if (bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0)
                                Console.WriteLine("Write Sensing Config Status: " + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                            else
                                throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);

                            bufferReturnInfo.Add(theSummit.WriteSensingEnableStreams(false, false, fft_check_status, false, false, false, true, true));
                            if (bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0)
                                Console.WriteLine("Write Stream Config Status: " + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                            else
                                throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                        }

                    }

                    // Set the Stimulation Mode to Adaptive
                    //Writes the adaptive stimulation mode to the device, either enabling or disabling embedded adaptive stim
                    bufferReturnInfo.Add(theSummit.WriteAdaptiveMode(AdaptiveTherapyModes.Embedded));
                    if (bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0)
                        Console.WriteLine("Write Adaptive Mode (Embedded):" + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                    else
                        throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);

                    // Turn on Stim
                    // Turns on the stimulation engine, required for any therapy to be output to patient
                    bufferReturnInfo.Add(theSummit.StimChangeTherapyOn());
                    if (bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0)
                        Console.WriteLine("Therapy On Status:" + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                    else
                        throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);

                    #endregion

                    #region Update Group A Settings

                    if (safeMode || ins_battery_low_stop_streaming)
                    {
                        double? outBufferDouble;

                        bufferReturnInfo.Add(theSummit.ReadStimGroup(GroupNumber.Group0, out TherapyGroup groupA));
                        if (bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0)
                            Console.WriteLine("Read Group A Settings:" + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                        else
                            throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);

                        // prog0
                        double needed0 = DefaultStateSettings.InitialState.Prog0AmpInMilliamps - groupA.Programs[0].AmplitudeInMilliamps;

                        if (needed0 != 0)
                        {
                            // Increment the Amplitude by needed mA
                            bufferReturnInfo.Add(theSummit.StimChangeStepAmp(0, needed0, out outBufferDouble));
                            if (bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0)
                            {
                                Console.WriteLine("Increment/Decrement the Amplitude by needed mA:" + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                                Console.WriteLine("Current Prog0 Amplitude set to: " + outBufferDouble);
                            }
                            else
                            {
                                Console.WriteLine("Command Failed - Increment/Decrement the Amplitude by needed mA - " + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                                //throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                            }
                        }

                        // Sleep to avoid - IncrementDecrementAmplitude (0x2B): 0x15 - Previous inc Amp/PW or dec rate period not delivered yet
                        Thread.Sleep(250);

                        // prog1
                        double needed1 = DefaultStateSettings.InitialState.Prog1AmpInMilliamps - groupA.Programs[1].AmplitudeInMilliamps;

                        if (needed1 != 0)
                        {
                            // Increment the Amplitude by needed mA
                            bufferReturnInfo.Add(theSummit.StimChangeStepAmp(1, needed1, out outBufferDouble));
                            if (bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0)
                            {
                                Console.WriteLine("Increment/Decrement the Amplitude by needed mA:" + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                                Console.WriteLine("Current Prog1 Amplitude set to: " + outBufferDouble);
                            }
                            else
                            {
                                Console.WriteLine("Command Failed - Increment/Decrement the Amplitude by needed mA - " + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                                //throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                            }
                        }

                        // Sleep to avoid - IncrementDecrementAmplitude (0x2B): 0x15 - Previous inc Amp/PW or dec rate period not delivered yet
                        Thread.Sleep(250);

                        // prog2
                        double needed2 = DefaultStateSettings.InitialState.Prog2AmpInMilliamps - groupA.Programs[2].AmplitudeInMilliamps;

                        if (needed2 != 0)
                        {
                            // Increment the Amplitude by needed mA
                            bufferReturnInfo.Add(theSummit.StimChangeStepAmp(2, needed2, out outBufferDouble));
                            if (bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0)
                            {
                                Console.WriteLine("Increment/Decrement the Amplitude by needed mA:" + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                                Console.WriteLine("Current Prog2 Amplitude set to: " + outBufferDouble);
                            }
                            else
                            {
                                Console.WriteLine("Command Failed - Increment/Decrement the Amplitude by needed mA - " + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                                //throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                            }
                        }

                        // Sleep to avoid - IncrementDecrementAmplitude (0x2B): 0x15 - Previous inc Amp/PW or dec rate period not delivered yet
                        Thread.Sleep(250);

                        // prog3
                        double needed3 = DefaultStateSettings.InitialState.Prog3AmpInMilliamps - groupA.Programs[3].AmplitudeInMilliamps;

                        if (needed3 != 0)
                        {
                            // Increment the Amplitude by needed mA
                            bufferReturnInfo.Add(theSummit.StimChangeStepAmp(3, needed3, out outBufferDouble));
                            if (bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0)
                            {
                                Console.WriteLine("Increment/Decrement the Amplitude by needed mA:" + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                                Console.WriteLine("Current Prog3 Amplitude set to: " + outBufferDouble);
                            }
                            else
                            {
                                Console.WriteLine("Command Failed - Increment/Decrement the Amplitude by needed mA - " + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                                //throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                            }
                        }

                        // Sleep to avoid - IncrementDecrementAmplitude (0x2B): 0x15 - Previous inc Amp/PW or dec rate period not delivered yet
                        Thread.Sleep(250);

                        // rate
                        double neededrate = DefaultStateSettings.InitialState.RateTargetInHz - groupA.RateInHz;

                        if (neededrate != 0)
                        {
                            // Increment the Amplitude by needed mA
                            bufferReturnInfo.Add(theSummit.StimChangeStepFrequency(neededrate, true, out outBufferDouble));
                            if (bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0)
                            {
                                Console.WriteLine("Increment/Decrement the Frequency by needed Hz:" + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                                Console.WriteLine("Current Frequency set to: " + outBufferDouble);
                            }
                            else
                            {
                                Console.WriteLine("Command Failed - Increment/Decrement the Amplitude by needed mA - " + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                                //throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                            }
                        }
                    }

                    #endregion

                }
                else // No Stim
                {
                    if (withStreaming == 1) // streaming no stim - if streaming with stim it was already set
                    {
                        // ******************* Turn on Sensing Components *******************
                        // Configures the INS to enable or disable specific sensing components.
                        bufferReturnInfo.Add(theSummit.WriteSensingState(SenseStates.LfpSense | SenseStates.Fft | SenseStates.Power | SenseStates.DetectionLd0 | SenseStates.LoopRecording, 0x00));
                        if (bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0)
                            Console.WriteLine("Write Sensing Config Status: " + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                        else
                            throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);

                        // ******************* Start streaming *******************
                        // Configures which sensors to enable streaming data from

                        // power needs to be enabled based on fftflag from LDA GUI
                        bufferReturnInfo.Add(theSummit.WriteSensingEnableStreams(true, false, fft_check_status, false, false, true, true, true));
                        if (bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0)
                            Console.WriteLine("Write Stream Config Status: " + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                        else
                            throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                    }
                    else
                    { // Only sensing - no streaming no stim

                        // make sure we are not in these situation (because we need to completley turn off everything in it)
                        // otherwise , we are only sensing and Loop recorder needs these streames
                        if (!doing_impTest && !doing_lda_config && !doing_looprecorder)
                        {

                            bufferReturnInfo.Add(theSummit.WriteSensingState(SenseStates.LfpSense | SenseStates.Fft | SenseStates.Power | SenseStates.DetectionLd0 | SenseStates.LoopRecording, 0x00));
                            if (bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0)
                                Console.WriteLine("Write Sensing Config Status: " + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                            else
                                throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);

                            bufferReturnInfo.Add(theSummit.WriteSensingEnableStreams(false, false, fft_check_status, false, false, false, true, true));
                            if (bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0)
                                Console.WriteLine("Write Stream Config Status: " + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                            else
                                throw new Exception(bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception during StartStreaming() " + e.Message + " " + e.ToString());

                // Reset POR if set
                if (bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCodeType == typeof(MasterRejectCode)
                    && (MasterRejectCode)bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == MasterRejectCode.ChangeTherapyPor)
                {
                    ResetPOR(theSummit);
                    bufferReturnInfo.Add(theSummit.StimChangeTherapyOn());
                    Console.WriteLine("Therapy On Status:" + bufferReturnInfo[bufferReturnInfo.Count - 1].Descriptor);
                }

                // untill the timer will try to connect - update the GUI
                if ((e.Message.Equals(APIRejectCodes.NoCtmConnected.ToString())))
                {
                    SetCTMconnection = false;
                    SetINSconnection = false;
                }
                else if ((e.Message.Equals(APIRejectCodes.NoInsConnected.ToString())))
                    SetINSconnection = false;
            }

            #region Register the data listeners
            // Attach events
            if (handlers_added == 0)
            {
                theSummit.DataReceivedTDHandler += theSummit_DataReceived_TD;
                theSummit.DataReceivedPowerHandler += theSummit_DataReceived_Power;
                theSummit.DataReceivedFFTHandler += theSummit_DataReceived_FFT;
                theSummit.DataReceivedAccelHandler += theSummit_DataReceived_Accel;

                theSummit.DataReceivedDetectorHandler += TheSummit_dataReceivedDetector;
                handlers_added = 1;
            }
            #endregion

            #region Start/Stop timers

            if (ins_battery_low_stop_streaming || !isPeriodic)
            {// if we want continuous (or not to stream) and the periodic timers still exist, get rid of them
                if (startRecordingTimer != null)
                {
                    startRecordingTimer.Enabled = false;
                    startRecordingTimer.Dispose();
                    startRecordingTimer = null;

                }

                if (stopRecordingTimer != null)
                {
                    stopRecordingTimer.Enabled = false;
                    stopRecordingTimer.Dispose();
                    stopRecordingTimer = null;
                }
            }
            else if (isPeriodic)
            {// if we want periodic and there is no timer and there are no timer - create:
                if (stopRecordingTimer == null && startRecordingTimer == null)
                {
                    stopRecordingTimer = new System.Timers.Timer();
                    stopRecordingTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnStopRecordingTimer);
                    stopRecordingTimer.Interval = 60000 * loopRecordingInputBoxInt;
                    stopRecordingTimer.AutoReset = false;
                    stopRecordingTimer.Enabled = true;
                }
            }

            #endregion

            SetIsStiming = 0;
            if (withStim == 1)
            {
                SetIsStiming = 1;
                currState = currentAdaptiveSettings;
                // check stimulation status
                bufferReturnInfo.Add(theSummit.ReadGeneralInfo(out GeneralInterrogateData interrogateBuffer));
                if (bufferReturnInfo[bufferReturnInfo.Count - 1].RejectCode == 0)
                {
                    TherapyStatusByte therapyStatus = interrogateBuffer.TherapyStatusData;
                    Console.WriteLine("Current Therapy Group and Status:"
                        + therapyStatus.ActiveGroup.ToString() + " " + therapyStatus.TherapyStatus.ToString());
                }
            }

            SetIsStreaming = 0;
            if (withStreaming == 1)
            {
                SetIsStreaming = 1;
                // check streaming status
                APIReturnInfo bufferReturnInfoStreaming = theSummit.ReadSensingStreamState(out StreamState theStreamState);
                if (bufferReturnInfoStreaming.RejectCode == 0)
                {
                    Console.WriteLine("Currently streaming: " + theStreamState.TimeDomainStreamEnabled);
                }
            }


            Console.WriteLine("\n --- Done with startStreaming(" + (isStiming == 1 ? "Stimulation ON" : "Stimulation OFF") + " , " +
                (isStreaming == 1 ? "Streaming ON" : "Streaming OFF") + ") - " + String.Format("{0:G}", DateTime.Now) + " --- ");


        }

        private bool SummitConnect(SummitManager theSummitManager)
        {
            SetINSconnection = false;
            int INSDiscoveredCountErrors = 0;

            // Look for known telemetry
            List<InstrumentInfo> knownTelemetry = theSummitManager.GetKnownTelemetry();

            // Check for empty list, look for USB CTMS
            if (knownTelemetry.Count == 0)
            {
                do
                {
                    // Inform user we will loop until a CTM is found on USBs
                    Console.WriteLine("No CTMs found, retrying on USB...");
                    Thread.Sleep(2000);
                    // No previously paired CTMs found, look for CTMs on USB
                    knownTelemetry = theSummitManager.GetUsbTelemetry();
                } while (knownTelemetry.Count == 0);
            }

            // Write out the known instruments
            Console.WriteLine("Bonded Instruments Found:");
            foreach (InstrumentInfo inst in knownTelemetry)
            {
                Console.WriteLine(inst.SerialNumber);
            }

            // Connect to the first CTM available, tnen try others if it fails
            SummitSystem tempSummit = null; ;
            for (int i = 0; i < theSummitManager.GetKnownTelemetry().Count; i++)
            {
                // Perform the connection
                ManagerConnectStatus connectReturn = theSummitManager.CreateSummit(out tempSummit, theSummitManager.GetKnownTelemetry()[i],
                    InstrumentPhysicalLayers.Any, 3, 32);

                // Write out the result
                Console.WriteLine("Create Summit Result: " + connectReturn.ToString());

                // Break if it failed successful
                if (connectReturn == ManagerConnectStatus.Success)
                {
                    break;
                }
            }

            // Make sure telemetry was connected to, if not fail
            if (tempSummit == null)
            {
                // inform user that CTM was not successfully connected to
                Console.WriteLine("Failed to connect to CTM...");
                SetCTMconnection = false;
                return false;
            }
            else
            {
                // inform user that CTM was successfully connected to
                Console.WriteLine("CTM Connection Successful!");
                SetCTMconnection = true;

                retry_INS_discovery:
                Console.WriteLine("Trying INS discovery...");

                // Discovery INS with the connected CTM, loop until a device has been discovered
                List<DiscoveredDevice> discoveredDevices;
                int count = 0;
                do
                {
                    if (count >= 2)
                    {
                        Console.WriteLine("SummitConnect: OlympusDiscovery() more than 2 counts of zero, returning false...");

                        INSDiscoveredCountErrors++;
                        if (INSDiscoveredCountErrors < 10)
                            goto retry_INS_discovery;
                        else
                        {
                            theSummitManager.DisposeSummit(tempSummit);
                            return false;
                        }
                    }

                    tempSummit.OlympusDiscovery(out discoveredDevices);
                    Console.WriteLine("OlympusDiscovery() complete");

                    // the following check was recommended by Jeff Heron, returning null evidently means the API
                    // thinks there is no CTM connected.
                    if (discoveredDevices == null)
                    {
                        Console.WriteLine("SummitConnect: OlympusDiscovery() return null value, returning false...");

                        INSDiscoveredCountErrors++;
                        if (INSDiscoveredCountErrors < 10)
                            goto retry_INS_discovery;
                        else
                        {
                            theSummitManager.DisposeSummit(tempSummit);
                            return false;
                        }
                    }

                    count++;

                } while (discoveredDevices.Count == 0);

                // Report Discovery Results to User
                Console.WriteLine("Olympi found:");
                foreach (DiscoveredDevice ins in discoveredDevices)
                {
                    Console.WriteLine(ins);
                }

                // Connect to the INS with default parameters and ORCA annotations
                Console.WriteLine("Creating Summit Interface.");

                Int32 timestamp_begin = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                // We can disable ORCA annotations because this is a non-human use INS (see disclaimer)
                // Human-use INS devices ignore the OlympusConnect disableAnnotation flag and always enable annotations.
                // Connect to a device
                ConnectReturn theWarnings;
                APIReturnInfo connectReturn;
                int i = 0;
                do
                {
                    // StartInsSession - Starts a session with a specific Summit implantable neurostimulation device
                    connectReturn = tempSummit.StartInsSession(discoveredDevices[0], out theWarnings, true);
                    Console.WriteLine("RejectCode = " + connectReturn.RejectCode + " - " + connectReturn.RejectCodeType + " - " + connectReturn.Descriptor);
                    if (connectReturn.RejectCode == 12)
                    { //InsConnectionAttemptsExceeded
                        Console.WriteLine("During session creation, the INS connection failed too many times. Move CTM and try again.");
                        break;
                    }

                    if (connectReturn.RejectCode == 10 && (connectReturn.RejectCodeType == typeof(Medtronic.TelemetryM.InstrumentReturnCode)))
                    {
                        Console.WriteLine("During session creation, the INS connection failed. Move CTM and try again.");
                        INSDiscoveredCountErrors++;
                        if (INSDiscoveredCountErrors < 10)
                            goto retry_INS_discovery;
                        else
                            break;
                    }

                    i++;
                    if (i == 500) // upper limit to number of attempts
                        break;

                    Int32 timestamp_now = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    if ((timestamp_now - timestamp_begin) > (15 * 60))
                    {
                        Console.WriteLine("More than 15 minutes has passed connecting to INS, will restart CTM connection.");
                        break;
                    }

                } while (theWarnings.HasFlag(ConnectReturn.InitializationError));

                // Write out the number of times a StartInsSession was attempted with initialization errors
                Console.WriteLine("Initialization Error Count: " + i.ToString());

                // Write out the final result of the example
                if ((connectReturn.RejectCode != 0) || (i == 500))
                {
                    Console.WriteLine("Summit Initialization: INS failed to connect");
                    theSummitManager.DisposeSummit(tempSummit);
                    return false;
                }
                else
                {
                    // Write out the warnings if they exist
                    Console.WriteLine("Summit Initialization: INS connected, warnings: " + theWarnings.ToString());
                    theSummit = tempSummit;
                    patient_name = discoveredDevices[0].deviceSerial;
                    Console.WriteLine("Patient name set to INS name: " + patient_name);

                    // Try to read the SubjectID, up to 5 times, otherwise give up
                    // The subjectID can be used, optionally, in the naming of the output EEG files.
                    APIReturnInfo returnInfo;
                    for (int j = 0; j < 5; j++)
                    {
                        returnInfo = theSummit.FlashReadSubjectInfo(out Medtronic.SummitAPI.Flash.SubjectInfo theSubject);
                        if (returnInfo.RejectCode == 0)
                        {
                            Console.WriteLine("theSubject.Id = " + theSubject.ID);
                            patient_subject_id = theSubject.ID;

                            // set to null if the subject ID is a blank string.
                            if (patient_subject_id.Equals(""))
                                patient_subject_id = null;

                            break;
                        }

                        Thread.Sleep(250);
                    }

                    // set INS timestamp to current PC timstamp.  This is necessary because some timestamps
                    // received from the INS (eg. loop recorder) are relative to the INS clock.
                    returnInfo = theSummit.WriteDeviceTimestamp(new TimeOfDay(DateTime.Now));

                    if (returnInfo.RejectCode == 0)
                    {
                        Console.WriteLine("INS clock successfully set to PC clock.");
                    }

                    SetINSconnection = true;
                    return true;
                }
            }
        }

        public static double DateTimeToUnixTimestamp(DateTime dateTime)
        {
            return (TimeZoneInfo.ConvertTimeToUtc(dateTime) - new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds;
        }

        #region Sensing data received event handlers
        private static void theSummit_DataReceived_TD(object sender, SensingEventTD TdSenseEvent)
        {
            // Annouce to console that packet was received by handler
            // Console.WriteLine("TD Packet Received, Global SeqNum:" + TdSenseEvent.Header.GlobalSequence.ToString()
            //                  + "; Time Generated:" + TdSenseEvent.GenerationTimeEstimate.Ticks.ToString() + "; Time Event Called:" + DateTime.Now.Ticks.ToString());

            // Log some inforamtion about the received packet out to file
            try
            {
                theSummit.LogCustomEvent(TdSenseEvent.GenerationTimeEstimate, DateTime.Now, "TdPacketReceived", TdSenseEvent.Header.GlobalSequence.ToString());
            }
            catch
            {
                return;
            }

            if (TDPacketCount == 500)
            {
                Console.WriteLine("\n --- TD Packet Received! --- ");
                Console.WriteLine("Current Sample Rate: " + TdSenseEvent.SampleRate);
                Console.WriteLine("---    ---   ---    --- \n");

                TDPacketCount = 0;
            }

            TDPacketCount++;
            Console.Write(".");

            ulong base_timestamp = (ulong)(DateTimeToUnixTimestamp(TdSenseEvent.GenerationTimeEstimate) * 1000000);

            // disregard this packet if the timestamp is invalid  // TBD do something else?
            if (base_timestamp < 0 || base_timestamp > 1900272907000000)
                return;

            mut.WaitOne();


            // check if still in streaming mode, if not, ignore data
            if (streamingMode == 0)
            {
                mut.ReleaseMutex();
                return;
            }

            // check if streaming, but in a gap, then ignore data
            if (isInStreamingGap == 1)
            {
                mut.ReleaseMutex();
                return;
            }

            mut_a_list.WaitOne();

            last_data_received_timestamp = (Int64)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;

            // This line can be used for debugging, if a mutex problem is suspected
            // Console.Write(",");

            // minus 1 is a flag that means we just swept, so skip zero and go straight to one in that case.
            if (new_packets_since_last_sweep == -1)
                new_packets_since_last_sweep = 1;
            else
                new_packets_since_last_sweep++;

            a_list.Add(TdSenseEvent);
            if (a_list.Count >= 100)
                process_a_list(0);

            mut_a_list.ReleaseMutex();

            mut.ReleaseMutex();
        }

        private static void theSummit_DataReceived_FFT(object sender, SensingEventFFT FftSenseEvent)
        {
            // Annouce to console that packet was received by handler
            // Console.WriteLine("FFT Packet Received, Global SeqNum:" + FftSenseEvent.Header.GlobalSequence.ToString()
            //                  + "; Time Generated:" + FftSenseEvent.GenerationTimeEstimate.Ticks.ToString() + "; Time Event Called:" + DateTime.Now.Ticks.ToString());

            // Log some inforamtion about the received packet out to file
            try
            {
                theSummit.LogCustomEvent(FftSenseEvent.GenerationTimeEstimate, DateTime.Now, "TdPacketReceived", FftSenseEvent.Header.GlobalSequence.ToString());
            }
            catch
            {
                return;
            }
        }

        private static void theSummit_DataReceived_Power(object sender, SensingEventPower PowerSenseEvent)
        {
            // Annouce to console that packet was received by handler
            // Console.WriteLine("Power Packet Received, Global SeqNum:" + PowerSenseEvent.Header.GlobalSequence.ToString()
            //                  + "; Time Generated:" + PowerSenseEvent.GenerationTimeEstimate.Ticks.ToString() + "; Time Event Called:" + DateTime.Now.Ticks.ToString());

            // Log some inforamtion about the received packet out to file
            try
            {
                theSummit.LogCustomEvent(PowerSenseEvent.GenerationTimeEstimate, DateTime.Now, "TdPacketReceived", PowerSenseEvent.Header.GlobalSequence.ToString());
            }
            catch
            {
                return;
            }

            ulong base_timestamp = (ulong)(DateTimeToUnixTimestamp(PowerSenseEvent.GenerationTimeEstimate) * 1000000);

            // disregard this packet if the timestamp is invalid  // TBD do something else?
            if (base_timestamp < 0 || base_timestamp > 1900272907000000)
                return;

            // if we are behind real-time, then just drop this packet, as it is not critical.
            // but we do want to make sure multiple power packets aren't writing to the output file
            // at the same time.
            if (in_power_callback == 1)
                return;

            in_power_callback = 1;

            // start with timestamp and number of power bands
            string output_line = base_timestamp + " " + PowerSenseEvent.Bands.Count + " ";

            // then add the value of the bands
            for (int i = 0; i < PowerSenseEvent.Bands.Count; i++)
                output_line = output_line + PowerSenseEvent.Bands[i] + " ";

            // then add int value of overrange boolean
            int overrange;
            if (PowerSenseEvent.IsPowerChannelOverrange)
                overrange = 1;
            else
                overrange = 0;
            output_line = output_line + overrange;

            try
            {
                if (power_file_output == null)
                {
                    String power_filename = homeFolder + Environment.MachineName + "\\powerband_" + patient_name + ".txt";
                    power_file_output = new StreamWriter(power_filename, true);
                    Console.WriteLine("Opening powerband output file: " + power_filename);
                }

                power_file_output.WriteLine(output_line);
                power_file_output.Flush();
            }
            catch
            {
                in_power_callback = 0;
                return;
            }

            in_power_callback = 0;
        }

        private static void theSummit_DataReceived_Accel(object sender, SensingEventAccel AccelSenseEvent)
        {
            // Annouce to console that packet was received by handler
            // Console.WriteLine("AccelPacket Received, Global SeqNum:" + AccelSenseEvent.Header.GlobalSequence.ToString()
            //                  + "; Time Generated:" + AccelSenseEvent.GenerationTimeEstimate.Ticks.ToString() + "; Time Event Called:" + DateTime.Now.Ticks.ToString());

            // Log some inforamtion about the received packet out to file

            try
            {
                theSummit.LogCustomEvent(AccelSenseEvent.GenerationTimeEstimate, DateTime.Now, "TdPacketReceived", AccelSenseEvent.Header.GlobalSequence.ToString());
            }
            catch
            {
                return;
            }
        }
        #endregion

        public static void process_a_list(int empty_entire_list)
        {
            // check for overflow situation
            if (processing_b_list == 1)
            {
                Console.WriteLine("Behind real-time, dumping a_list...");
                a_list.Clear();
                return;
            }

            // no overflow, so copy "a" list to "b" list.  There might already be stuff in "b" list
            for (int i = 0; i < a_list.Count; i++)
                b_list.Add(a_list[i]);

            // empty out a_list
            a_list.Clear();

            // If we are emptying the entire list, do it single-threaded, because no packets are coming
            // in in real-time, and we still need to flush the mef channels afterward.
            // If we are not emptying entire list, then do it threaded, because new packets are
            // continuing to come in, and those packets are added to the a-list, so that doesn't affect
            // the b-list, so we can multi-task.
            if (empty_entire_list == 0)
            {
                // multi-threaded
                var t = new Thread(() => process_b_list(empty_entire_list));
                t.Start();
            }
            else
            {
                // single-threaded
                process_b_list(empty_entire_list);
            }
        }

        public static void process_b_list(int empty_entire_list)
        {
            processing_b_list = 1;

            mut_b_list.WaitOne();

            Console.WriteLine("Sorting B List, " + b_list.Count.ToString() + " elements.");
            // sort into timestamp order
            b_list.Sort((a, b) => a.GenerationTimeEstimate.CompareTo(b.GenerationTimeEstimate));

            Console.WriteLine("Processing B list");
            // upper_bound means that some packets are left behind to be processed next time,
            // this overlap means sorting can work across processing boundaries
            int upper_bound = b_list.Count - 100;
            // If we are emptying entire list, then leave nothing behind.
            if (empty_entire_list == 1)
            {
                upper_bound = b_list.Count;
                Console.WriteLine("Sweeping " + upper_bound + " packets.");
            }
            for (int list_num = 0; list_num < upper_bound; list_num++)
            {
                // The GenerationTimeEstimate is the timestamp of the last sample in the packet, not the first.
                // this is fine, for the purposes of ordering the packets.
                ulong base_timestamp = (ulong)(DateTimeToUnixTimestamp(b_list[list_num].GenerationTimeEstimate) * 1000000);

                if (MEF_channel_created_sorted == 0)
                {
                    string dir_name_appending;
                    string[] appending_chan_name;
                    appending_chan_name = new string[4];
                    bool doing_appending;

                    MEF_channel_created_sorted = 1;

                    //Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                    DateTime dt = DateTime.Now;

                    doing_appending = false;

                    if (ReadRecordingSettingsFile(out string appending_dir,
                        out string appending_chan_name0,
                        out string appending_chan_name1,
                        out string appending_chan_name2,
                        out string appending_chan_name3,
                        out int appending_segment,
                        out int appending_frequency))
                    {
                        doing_appending = true;
                        appending_chan_name[0] = appending_chan_name0;
                        appending_chan_name[1] = appending_chan_name1;
                        appending_chan_name[2] = appending_chan_name2;
                        appending_chan_name[3] = appending_chan_name3;
                    }
                    else
                    {
                        appending_segment = -1;
                    }

                    // in case we are not appending, this is the where the new recording will go
                    // include patient_subject_id in directory name, if patient_subject_id is known.
                    if (patient_subject_id == null)
                        dir_name_appending = homeFolder + Environment.MachineName + "/" + patient_name + "_" + dt.ToString("yyyyMMddHHmmss");
                    else
                        dir_name_appending = homeFolder + Environment.MachineName + "/" + patient_subject_id + "_" + patient_name + "_" + dt.ToString("yyyyMMddHHmmss");


                    // if we are doing appending, do some simple checks to make sure it is feasible.
                    if (doing_appending)
                    {
                        string index_file_name =
                            appending_dir + ".mefd/" +
                            appending_chan_name0 + ".timd/" +
                            appending_chan_name0 + "-" + appending_segment.ToString().PadLeft(6, '0') + ".segd/" +
                            appending_chan_name0 + "-" + appending_segment.ToString().PadLeft(6, '0') + ".tidx";

                        Console.WriteLine("Looking for existence of file: " + index_file_name);

                        if (File.Exists(index_file_name) == false)
                        {
                            // file doesn't exist
                            doing_appending = false;
                            appending_segment = -1;
                            Console.WriteLine("Previous segment files don't exist, so starting new recording.");
                        }
                        else
                        {
                            // file exists, check on size of file
                            long length = new System.IO.FileInfo(index_file_name).Length;

                            Console.WriteLine("length: " + length);
                            // if file is empty, or just a header, then it contains no data, so don't use it
                            if ((length == 0) || (length == 1024))  // 1024 == length of universal header in MEF 3.0
                            {
                                doing_appending = false;
                                appending_segment = -1;
                                Console.WriteLine("Previous segment files don't contain data, so starting new recording.");
                            }
                        }
                    }

                    // check to see if settings have changed
                    bool settings_changed = false;
                    if (doing_appending)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            if (appending_chan_name[i] != chan_name[i])
                            {
                                Console.WriteLine("Name of channel " + i + " has changed, do not append.");
                                settings_changed = true;
                            }
                        }
                        if (Convert.ToInt32(mef3_sampling_rate) != appending_frequency)
                        {
                            Console.WriteLine("Sampling frequency has changed, do not append.");
                            settings_changed = true;
                        }
                    }

                    if (settings_changed)
                    {
                        appending_segment = -1;
                        doing_appending = false;
                    }

                    mef_channel_state_struct_sorted_appending = new CHANNEL_STATE[4];


                    if (!doing_appending)
                        data_dir = dir_name_appending;
                    else
                        data_dir = appending_dir;

                    // write bad data to recording settings file, this way if there is a program crash during the appending
                    // initialization process, the next time the app runs, a brand new recording will be started
                    // (so we won't keep making the same mistake (app crash) over and over)
                    WriteRecordingSettingsFile(false, data_dir,
                            chan_name[0],
                            chan_name[1],
                            chan_name[2],
                            chan_name[3],
                            appending_segment + 1,
                            Convert.ToInt32(mef3_sampling_rate));

                    try_again_no_appending:

                    for (int i = 0; i < 4; i++)
                    {

                        mef_channel_state_struct_sorted_appending[i] = new DMP_Main_MDIParent.CHANNEL_STATE();

                        if (!doing_appending)
                        {

                            initialize_mef_channel_data(ref mef_channel_state_struct_sorted_appending[i],
                                                        15,           // seconds per block
                                                        chan_name[i], // channel name
                                                        0,// bit shift flag, set to 1 for neuralynx, to chop off 2 least-significant sample bits
                                                        0.0,           // low filt freq
                                                        9000.0,        // high filt freq
                                                        -1.0,           // notch filt freq
                                                        60.0,          // AC line freq
                                                        1.0,           // units conversion factor
                                                        "not entered ",// chan description
                                                        mef3_sampling_rate, // starter freq for channel, make it as high or higher than actual freq to allocate buffers
                                                        15 * 1000000, // block interval, needs to be correct, this value is used for all channels
                                                        i,             // chan number
                                                        dir_name_appending,      // absolute path of session
                                                        (float)-6.0,                  // GMT offset
                                                        "not entered2",        // session description
                                                        "anon",                // anonymized subject name
                                                        "Firstname",                // subject first name
                                                        "Secondname",               // subject second name
                                                        patient_name,               // subject ID
                                                        "",           // institution
                                                        null,                  // level 1 password (technical data)
                                                        null,                  // level 2 password (subject data), must also specify level 1 password if specifying level 2
                                                        "not entered",        // study comments
                                                        "not entered",         // channel comments
                                                        0                      // secs per segment
                                                        );
                        }
                        else
                        {

                            try
                            {
                                append_mef_channel_data(ref mef_channel_state_struct_sorted_appending[i],
                                    chan_name[i],
                                    appending_segment + 1,
                                    null,
                                    null,
                                    appending_dir + ".mefd",
                                    0,   // secs per segment
                                    0);  // bit_shift_flag
                            }
                            catch (Exception e)
                            {
                                // okay, something went wrong when trying to read previous segment for appending.
                                // we can't append if we can't read the previous segment.  So, try again, but this time
                                // don't do appending.
                                // this may be kludgy, but this is exceptional behavior that may never actually occur.
                                // after testing, this doesn't even work anyway, it doesn't catch
                                // System.AccessViolationExceptions.
                                doing_appending = false;
                                appending_segment = -1;
                                data_dir = dir_name_appending;
                                Console.WriteLine("Couldn't append, bailing out, trying again without appending: " + e.Message);
                                goto try_again_no_appending;
                            }
                        }

                    }

                    // now write the good data to recording settings file, since we know a new segment was successfully created.
                    WriteRecordingSettingsFile(true, data_dir,
                            chan_name[0],
                            chan_name[1],
                            chan_name[2],
                            chan_name[3],
                            appending_segment + 1,
                            Convert.ToInt32(mef3_sampling_rate));

                    Console.WriteLine("Done initing sorted MEF channels");

                }

                ulong[] packet_times;
                int[] samps;
                int largest_sample_num = 0;
                foreach (var item in b_list[list_num].ChannelSamples)
                    if (item.Value.Count > largest_sample_num)
                    {
                        largest_sample_num = item.Value.Count;
                    }

                // adjust base_timestamp because the timestamp of the packet is actually the timestamp
                // of the last sample in the packet (per verbal conversation with Medtronic)
                base_timestamp = base_timestamp - (ulong)((largest_sample_num - 1) * ((1e6) / mef3_sampling_rate));

                packet_times = new ulong[largest_sample_num];
                samps = new int[largest_sample_num];
                for (int i = 0; i < largest_sample_num; i++)
                {
                    packet_times[i] = base_timestamp + (ulong)((i * ((1e6) / mef3_sampling_rate)));
                }
                int k = 0;
                foreach (var item in b_list[list_num].ChannelSamples)
                {
                    for (int j = 0; j < item.Value.Count; j++)
                    {
                        // convert from volts to microvolts.
                        if (item.Value[j] >= 0)
                            samps[j] = (int)((item.Value[j] * 1000000) + 0.5);
                        else
                            samps[j] = (int)((item.Value[j] * 1000000) - 0.5);
                        //Console.WriteLine("sample: " + samps[j]);
                    }

                    // Figure out which channel this data corresponds to
                    k = -1;
                    switch (item.Key)
                    {
                        case SenseTimeDomainChannel.Ch0:
                            k = 0;
                            break;
                        case SenseTimeDomainChannel.Ch1:
                            k = 1;
                            break;
                        case SenseTimeDomainChannel.Ch2:
                            k = 2;
                            break;
                        case SenseTimeDomainChannel.Ch3:
                            k = 3;
                            break;
                    }

                    // This shouldn't happen, and how do we handle it if it does, maybe ignore packet?
                    // For now just see if it ever happens
                    if (k == -1)
                        throw new ArgumentNullException("whatever", "Invalid Channel Key in Dictionary");

                    //write_mef_channel_data(ref (mef_channel_state_struct_sorted[k]), packet_times, samps, (ulong)(item.Value.Count), 15, mef3_sampling_rate);
                    write_mef_channel_data(ref (mef_channel_state_struct_sorted_appending[k]), packet_times, samps, (ulong)(item.Value.Count), 15, mef3_sampling_rate);

                    // keep track of last timestamp, this will be check when adding loop recorder data
                    last_MEF_written_timestamp = packet_times[item.Value.Count - 1];

                    // keep track of how much data has been sent to MEF, this is used for analytics
                    // only do this for first channel, which is representative of all channels
                    if (k == 0)
                    {
                        analytics_detection_new_data += ((double)(item.Value.Count)) / mef3_sampling_rate;
                        analytics_prediction_new_data += ((double)(item.Value.Count)) / mef3_sampling_rate;
                    }
                }

                // Don't need to update_metadata explicitly, because after each RED block is written, the metadata should be updated
                // within the write_mef_channel_data() .dll call
                /*
                packet_counter_sorted++;

                if (packet_counter_sorted >= 500)
                {
                    for (int j = 0; j < 4; j++)
                        update_metadata_dll(ref (mef_channel_state_struct_sorted[j]));

                    packet_counter_sorted = 0;
                }
                */
            }

            // Remove the elements just processed.
            // TBD this can probably be optimized
            for (int i = 0; i < upper_bound; i++)
            {
                b_list.RemoveAt(0);
            }

            mut_b_list.ReleaseMutex();

            processing_b_list = 0;
        }

        public static bool ReadRecordingSettingsFile(out string appending_dir,
                out string out_chan_name0,
                out string out_chan_name1,
                out string out_chan_name2,
                out string out_chan_name3,
                out int segment_num,
                out int frequency_num)
        {
            appending_dir = "";
            out_chan_name0 = "";
            out_chan_name1 = "";
            out_chan_name2 = "";
            out_chan_name3 = "";
            segment_num = 0;
            frequency_num = 0;

            try
            {
                System.IO.StreamReader file = new StreamReader("c:\\ctm_config\\" + patient_name + ".txt");
                String line;
                if ((line = file.ReadLine()) != null)
                {
                    Console.WriteLine("Appending_dir: " + line);
                    appending_dir = line;
                }
                else
                {
                    Console.WriteLine("Appending_dir name not read from  c:\\ctm_config\\" + patient_name + ".txt.");
                    return false;
                }
                if ((line = file.ReadLine()) != null)
                {
                    Console.WriteLine("channel name0: " + line);
                    out_chan_name0 = line;
                }
                else
                {
                    Console.WriteLine("chan_name0 not read from  c:\\ctm_config\\" + patient_name + ".txt.");
                    return false;
                }
                if ((line = file.ReadLine()) != null)
                {
                    Console.WriteLine("channel name1 " + line);
                    out_chan_name1 = line;
                }
                else
                {
                    Console.WriteLine("chan_name1 not read from  c:\\ctm_config\\" + patient_name + ".txt.");
                    return false;
                }
                if ((line = file.ReadLine()) != null)
                {
                    Console.WriteLine("channel name2: " + line);
                    out_chan_name2 = line;
                }
                else
                {
                    Console.WriteLine("chan_name2 not read from  c:\\ctm_config\\" + patient_name + ".txt.");
                    return false;
                }
                if ((line = file.ReadLine()) != null)
                {
                    Console.WriteLine("channel name3: " + line);
                    out_chan_name3 = line;
                }
                else
                {
                    Console.WriteLine("chan_name3 not read from  c:\\ctm_config\\" + patient_name + ".txt.");
                    return false;
                }

                if ((line = file.ReadLine()) != null)
                {
                    Console.WriteLine("Segment_num: " + line);
                    segment_num = Convert.ToInt32(line);
                }
                else
                {
                    Console.WriteLine("segment_num not read from  c:\\ctm_config\\" + patient_name + ".txt.");
                    return false;
                }
                if ((line = file.ReadLine()) != null)
                {
                    Console.WriteLine("Frequency: " + line);
                    frequency_num = Convert.ToInt32(line);
                }
                else
                {
                    Console.WriteLine("Frequency not read from  c:\\ctm_config\\" + patient_name + ".txt.");
                    return false;
                }

                file.Close();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception during readRecordingSettingsFile(): " + e.Message + " c:\\ctm_config\\" + patient_name + ".txt does not exist?  or contains bad data");
                return false;
            }

        }

        public static bool WriteRecordingSettingsFile(bool write_good_data,
            string appending_dir,
            string out_chan_name0,
            string out_chan_name1,
            string out_chan_name2,
            string out_chan_name3,
            int segment_num,
            int frequency_num)
        {
            try
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\ctm_config\\" + patient_name + ".txt", false);
                if (write_good_data)
                {
                    file.WriteLine(appending_dir);
                    file.WriteLine(out_chan_name0);
                    file.WriteLine(out_chan_name1);
                    file.WriteLine(out_chan_name2);
                    file.WriteLine(out_chan_name3);
                    file.WriteLine(segment_num);
                    file.WriteLine(frequency_num);
                }
                else
                {
                    // here we are intentionally writing garbage.
                    file.WriteLine("blah");
                }
                file.Close();

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception during writeRecordingSettingsFile(): " + e.Message + " Unable to write c:\\ctm_config\\" + patient_name + ".txt?");
                return false;
            }

        }

        #endregion

        #region Stimulation Code - Methods

        public bool StimSettingsCheck(StimulationParamForm stimForm)
        {
            try
            {
                #region Read current settings from UI

                // read data from UI and create objects

                // default state
                AdaptiveState defaultState = new AdaptiveState(
                Convert.ToDouble(stimForm.numericUpDown_defaultAmp1.Value),
                Convert.ToDouble(stimForm.numericUpDown_defaultAmp2.Value),
                Convert.ToDouble(stimForm.numericUpDown_defaultAmp3.Value),
                Convert.ToDouble(stimForm.numericUpDown_defaultAmp4.Value),
                Convert.ToDouble(stimForm.numericUpDown_defaultRate.Value));
                // default state Deltas
                AdaptiveDeltas[] defaultDeltas = new AdaptiveDeltas[4]{
                new AdaptiveDeltas(DefaultStateSettings.Deltas[0].Rise,DefaultStateSettings.Deltas[0].Fall),
                new AdaptiveDeltas(DefaultStateSettings.Deltas[1].Rise,DefaultStateSettings.Deltas[1].Fall),
                new AdaptiveDeltas(DefaultStateSettings.Deltas[2].Rise,DefaultStateSettings.Deltas[2].Fall),
                new AdaptiveDeltas(DefaultStateSettings.Deltas[3].Rise,DefaultStateSettings.Deltas[3].Fall) };
                AdaptiveSettings defaultadaptiveSettings = new AdaptiveSettings
                {
                    Deltas = defaultDeltas,
                    InitialState = defaultState
                };


                // sleep state 
                AdaptiveState sleepState = new AdaptiveState(
                    Convert.ToDouble(stimForm.numericUpDown_sleepAmp1.Value),
                    Convert.ToDouble(stimForm.numericUpDown_sleepAmp2.Value),
                    Convert.ToDouble(stimForm.numericUpDown_sleepAmp3.Value),
                    Convert.ToDouble(stimForm.numericUpDown_sleepAmp4.Value),
                    Convert.ToDouble(stimForm.numericUpDown_sleepRate.Value));
                // sleep state Deltas
                AdaptiveDeltas[] sleepDeltas = new AdaptiveDeltas[4]{
                new AdaptiveDeltas(SleepStateSettings.Deltas[0].Rise,SleepStateSettings.Deltas[0].Fall),
                new AdaptiveDeltas(SleepStateSettings.Deltas[1].Rise,SleepStateSettings.Deltas[1].Fall),
                new AdaptiveDeltas(SleepStateSettings.Deltas[2].Rise,SleepStateSettings.Deltas[2].Fall),
                new AdaptiveDeltas(SleepStateSettings.Deltas[3].Rise,SleepStateSettings.Deltas[3].Fall) };
                AdaptiveSettings sleepadaptiveSettings = new AdaptiveSettings
                {
                    Deltas = sleepDeltas,
                    InitialState = sleepState
                };


                // pre state
                AdaptiveState preState = new AdaptiveState(
                    Convert.ToDouble(stimForm.numericUpDown_preAmp1.Value),
                    Convert.ToDouble(stimForm.numericUpDown_preAmp2.Value),
                    Convert.ToDouble(stimForm.numericUpDown_preAmp3.Value),
                    Convert.ToDouble(stimForm.numericUpDown_preAmp4.Value),
                    Convert.ToDouble(stimForm.numericUpDown_preRate.Value));
                // pre state Deltas
                AdaptiveDeltas[] preDeltas = new AdaptiveDeltas[4]{
               new AdaptiveDeltas(PreStateSettings.Deltas[0].Rise,PreStateSettings.Deltas[0].Fall),
                new AdaptiveDeltas(PreStateSettings.Deltas[1].Rise,PreStateSettings.Deltas[1].Fall),
                new AdaptiveDeltas(PreStateSettings.Deltas[2].Rise,PreStateSettings.Deltas[2].Fall),
                new AdaptiveDeltas(PreStateSettings.Deltas[3].Rise,PreStateSettings.Deltas[3].Fall) };
                AdaptiveSettings preadaptiveSettings = new AdaptiveSettings
                {
                    Deltas = preDeltas,
                    InitialState = preState
                };


                // Seizure state
                AdaptiveState SeizuretState = new AdaptiveState(
                    Convert.ToDouble(stimForm.numericUpDown_seizureAmp1.Value),
                    Convert.ToDouble(stimForm.numericUpDown_seizureAmp2.Value),
                    Convert.ToDouble(stimForm.numericUpDown_seizureAmp3.Value),
                    Convert.ToDouble(stimForm.numericUpDown_seizureAmp4.Value),
                    Convert.ToDouble(stimForm.numericUpDown_seizureRate.Value));
                AdaptiveSettings SeizuretStateadaptiveSettings = new AdaptiveSettings
                {
                    InitialState = SeizuretState
                };

                #endregion

                #region Deltas validation

                // read delta limits from INS
                theSummit.ReadAdaptiveStimSettings(out AdaptiveSettings nofilesettings);
                AdaptiveDeltas[] upperLimits = nofilesettings.DeltaUpperLimits;

                // check deltas in each state
                defaultadaptiveSettings.DeltasValid = true;
                for (int i = 0; i < 4; i++)
                {
                    if (defaultadaptiveSettings.Deltas[i].Fall > upperLimits[i].Fall
                        || defaultadaptiveSettings.Deltas[i].Rise > upperLimits[i].Rise)
                    {
                        defaultadaptiveSettings.DeltasValid = false;
                    }
                }

                sleepadaptiveSettings.DeltasValid = true;
                for (int i = 0; i < 4; i++)
                {
                    if (sleepadaptiveSettings.Deltas[i].Fall > upperLimits[i].Fall
                        || sleepadaptiveSettings.Deltas[i].Rise > upperLimits[i].Rise)
                    {
                        sleepadaptiveSettings.DeltasValid = false;
                    }
                }

                preadaptiveSettings.DeltasValid = true;
                for (int i = 0; i < 4; i++)
                {
                    if (preadaptiveSettings.Deltas[i].Fall > upperLimits[i].Fall
                        || preadaptiveSettings.Deltas[i].Rise > upperLimits[i].Rise)
                    {
                        preadaptiveSettings.DeltasValid = false;
                    }
                }

                #endregion

                #region Amplitude validation

                // read amplitude limits from INS
                APIReturnInfo bufferReturnInfo = theSummit.ReadStimAmplitudeLimits(GroupNumber.Group3, out AmplitudeLimits ampLimits);
                if (bufferReturnInfo.RejectCode == 0)
                {

                    // check each state
                    SeizuretStateadaptiveSettings.InitialState.IsValid = true;
                    if (SeizuretStateadaptiveSettings.InitialState.Prog0AmpInMilliamps < ampLimits.Prog0LowerInMilliamps ||
                        SeizuretStateadaptiveSettings.InitialState.Prog0AmpInMilliamps > ampLimits.Prog0UpperInMilliamps ||
                        SeizuretStateadaptiveSettings.InitialState.Prog1AmpInMilliamps < ampLimits.Prog1LowerInMilliamps ||
                        SeizuretStateadaptiveSettings.InitialState.Prog1AmpInMilliamps > ampLimits.Prog1UpperInMilliamps ||
                        SeizuretStateadaptiveSettings.InitialState.Prog2AmpInMilliamps < ampLimits.Prog2LowerInMilliamps ||
                        SeizuretStateadaptiveSettings.InitialState.Prog2AmpInMilliamps > ampLimits.Prog2UpperInMilliamps ||
                        SeizuretStateadaptiveSettings.InitialState.Prog3AmpInMilliamps < ampLimits.Prog3LowerInMilliamps ||
                        SeizuretStateadaptiveSettings.InitialState.Prog3AmpInMilliamps > ampLimits.Prog3UpperInMilliamps)
                    {
                        SeizuretStateadaptiveSettings.InitialState.IsValid = false;
                    }

                    defaultadaptiveSettings.InitialState.IsValid = true;
                    if (defaultadaptiveSettings.InitialState.Prog0AmpInMilliamps < ampLimits.Prog0LowerInMilliamps ||
                        defaultadaptiveSettings.InitialState.Prog0AmpInMilliamps > ampLimits.Prog0UpperInMilliamps ||
                        defaultadaptiveSettings.InitialState.Prog1AmpInMilliamps < ampLimits.Prog1LowerInMilliamps ||
                        defaultadaptiveSettings.InitialState.Prog1AmpInMilliamps > ampLimits.Prog1UpperInMilliamps ||
                        defaultadaptiveSettings.InitialState.Prog2AmpInMilliamps < ampLimits.Prog2LowerInMilliamps ||
                        defaultadaptiveSettings.InitialState.Prog2AmpInMilliamps > ampLimits.Prog2UpperInMilliamps ||
                        defaultadaptiveSettings.InitialState.Prog3AmpInMilliamps < ampLimits.Prog3LowerInMilliamps ||
                        defaultadaptiveSettings.InitialState.Prog3AmpInMilliamps > ampLimits.Prog3UpperInMilliamps)
                    {
                        defaultadaptiveSettings.InitialState.IsValid = false;
                    }

                    sleepadaptiveSettings.InitialState.IsValid = true;
                    if (sleepadaptiveSettings.InitialState.Prog0AmpInMilliamps < ampLimits.Prog0LowerInMilliamps ||
                        sleepadaptiveSettings.InitialState.Prog0AmpInMilliamps > ampLimits.Prog0UpperInMilliamps ||
                        sleepadaptiveSettings.InitialState.Prog1AmpInMilliamps < ampLimits.Prog1LowerInMilliamps ||
                        sleepadaptiveSettings.InitialState.Prog1AmpInMilliamps > ampLimits.Prog1UpperInMilliamps ||
                        sleepadaptiveSettings.InitialState.Prog2AmpInMilliamps < ampLimits.Prog2LowerInMilliamps ||
                        sleepadaptiveSettings.InitialState.Prog2AmpInMilliamps > ampLimits.Prog2UpperInMilliamps ||
                        sleepadaptiveSettings.InitialState.Prog3AmpInMilliamps < ampLimits.Prog3LowerInMilliamps ||
                        sleepadaptiveSettings.InitialState.Prog3AmpInMilliamps > ampLimits.Prog3UpperInMilliamps)
                    {
                        sleepadaptiveSettings.InitialState.IsValid = false;
                    }

                    preadaptiveSettings.InitialState.IsValid = true;
                    if (preadaptiveSettings.InitialState.Prog0AmpInMilliamps < ampLimits.Prog0LowerInMilliamps ||
                        preadaptiveSettings.InitialState.Prog0AmpInMilliamps > ampLimits.Prog0UpperInMilliamps ||
                        preadaptiveSettings.InitialState.Prog1AmpInMilliamps < ampLimits.Prog1LowerInMilliamps ||
                        preadaptiveSettings.InitialState.Prog1AmpInMilliamps > ampLimits.Prog1UpperInMilliamps ||
                        preadaptiveSettings.InitialState.Prog2AmpInMilliamps < ampLimits.Prog2LowerInMilliamps ||
                        preadaptiveSettings.InitialState.Prog2AmpInMilliamps > ampLimits.Prog2UpperInMilliamps ||
                        preadaptiveSettings.InitialState.Prog3AmpInMilliamps < ampLimits.Prog3LowerInMilliamps ||
                        preadaptiveSettings.InitialState.Prog3AmpInMilliamps > ampLimits.Prog3UpperInMilliamps)
                    {
                        preadaptiveSettings.InitialState.IsValid = false;
                    }
                }
                #endregion

                // check if all settings are valid accordind to the INS
                if (SeizuretStateadaptiveSettings.InitialState.IsValid &&
                    defaultadaptiveSettings.DeltasValid &&
                    defaultadaptiveSettings.InitialState.IsValid &&
                    sleepadaptiveSettings.DeltasValid &&
                    sleepadaptiveSettings.InitialState.IsValid &&
                    preadaptiveSettings.DeltasValid &&
                    preadaptiveSettings.InitialState.IsValid)
                {
                    SeizureStateSettings = SeizuretStateadaptiveSettings;
                    DefaultStateSettings = defaultadaptiveSettings;
                    SleepStateSettings = sleepadaptiveSettings;
                    PreStateSettings = preadaptiveSettings;
                    return true;
                }
                else
                {
                    CustomMsgBox.Show("Stimulation setting are NOT valid!", "Error", "OK");
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception during StimSettingsCheck() " + e.Message);
                return false;
            }
        }

        public void UpdateStimSettingsFile(StimulationParamForm paramForm)
        { //valuse from stim setting file sent to GUI
            try
            {
                // Update GUI
                paramForm.SetnumericUpDown_defaultRateValue(Decimal.Parse(DefaultStateSettings.InitialState.RateTargetInHz.ToString()));
                paramForm.SetnumericUpDown_defaultAmp1Value(Decimal.Parse(DefaultStateSettings.InitialState.Prog0AmpInMilliamps.ToString()));
                paramForm.SetnumericUpDown_defaultAmp2Value(Decimal.Parse(DefaultStateSettings.InitialState.Prog1AmpInMilliamps.ToString()));
                paramForm.SetnumericUpDown_defaultAmp3Value(Decimal.Parse(DefaultStateSettings.InitialState.Prog2AmpInMilliamps.ToString()));
                paramForm.SetnumericUpDown_defaultAmp4Value(Decimal.Parse(DefaultStateSettings.InitialState.Prog3AmpInMilliamps.ToString()));

                paramForm.SetnumericUpDown_sleepRateValue(Decimal.Parse(SleepStateSettings.InitialState.RateTargetInHz.ToString()));
                paramForm.SetnumericUpDown_sleepAmp1Value(Decimal.Parse(SleepStateSettings.InitialState.Prog0AmpInMilliamps.ToString()));
                paramForm.SetnumericUpDown_sleepAmp2Value(Decimal.Parse(SleepStateSettings.InitialState.Prog1AmpInMilliamps.ToString()));
                paramForm.SetnumericUpDown_sleepAmp3Value(Decimal.Parse(SleepStateSettings.InitialState.Prog2AmpInMilliamps.ToString()));
                paramForm.SetnumericUpDown_sleepAmp4Value(Decimal.Parse(SleepStateSettings.InitialState.Prog3AmpInMilliamps.ToString()));

                paramForm.SetnumericUpDown_preRateValue(Decimal.Parse(PreStateSettings.InitialState.RateTargetInHz.ToString()));
                paramForm.SetnumericUpDown_preAmp1Value(Decimal.Parse(PreStateSettings.InitialState.Prog0AmpInMilliamps.ToString()));
                paramForm.SetnumericUpDown_preAmp2Value(Decimal.Parse(PreStateSettings.InitialState.Prog1AmpInMilliamps.ToString()));
                paramForm.SetnumericUpDown_preAmp3Value(Decimal.Parse(PreStateSettings.InitialState.Prog2AmpInMilliamps.ToString()));
                paramForm.SetnumericUpDown_preAmp4Value(Decimal.Parse(PreStateSettings.InitialState.Prog3AmpInMilliamps.ToString()));

                paramForm.SetnumericUpDown_seizureRateValue(Decimal.Parse(SeizureStateSettings.InitialState.RateTargetInHz.ToString()));
                paramForm.SetnumericUpDown_seizureAmp1Value(Decimal.Parse(SeizureStateSettings.InitialState.Prog0AmpInMilliamps.ToString()));
                paramForm.SetnumericUpDown_seizureAmp2Value(Decimal.Parse(SeizureStateSettings.InitialState.Prog1AmpInMilliamps.ToString()));
                paramForm.SetnumericUpDown_seizureAmp3Value(Decimal.Parse(SeizureStateSettings.InitialState.Prog2AmpInMilliamps.ToString()));
                paramForm.SetnumericUpDown_seizureAmp4Value(Decimal.Parse(SeizureStateSettings.InitialState.Prog3AmpInMilliamps.ToString()));

                // if hardware is connected read current stim electrodes from it
                if (theSummit != null)
                {
                    if (!(theSummit.IsDisposed))
                    {
                        APIReturnInfo bufferReturnInfo = theSummit.ReadStimGroup(GroupNumber.Group3, out TherapyGroup theTherapyGroup);
                        if (bufferReturnInfo.RejectCode == 0)
                        {
                            List<TherapyProgram> therapyPrograms = theTherapyGroup.Programs;

                            string prog0electrodes = null;
                            TherapyElectrodes prog0electrodesALL = (therapyPrograms[0].Electrodes);
                            foreach (Electrode electrode in prog0electrodesALL)
                            {
                                if (!electrode.IsOff)
                                {

                                    prog0electrodes = prog0electrodes + "\nE" + prog0electrodesALL.IndexOf(electrode) + " = " + electrode.ToString();
                                }
                            }

                            string prog1electrodes = null;
                            TherapyElectrodes prog1electrodesALL = (therapyPrograms[1].Electrodes);
                            foreach (Electrode electrode in prog1electrodesALL)
                            {
                                if (!electrode.IsOff)
                                {

                                    prog1electrodes = prog1electrodes + "\nE" + prog1electrodesALL.IndexOf(electrode) + " = " + electrode.ToString();
                                }
                            }

                            string prog2electrodes = null;
                            TherapyElectrodes prog2electrodesALL = (therapyPrograms[2].Electrodes);
                            foreach (Electrode electrode in prog2electrodesALL)
                            {
                                if (!electrode.IsOff)
                                {

                                    prog2electrodes = prog2electrodes + "\nE" + prog2electrodesALL.IndexOf(electrode) + " = " + electrode.ToString();
                                }
                            }

                            string prog3electrodes = null;
                            TherapyElectrodes prog3electrodesALL = (therapyPrograms[3].Electrodes);
                            foreach (Electrode electrode in prog3electrodesALL)
                            {
                                if (!electrode.IsOff)
                                {

                                    prog3electrodes = prog3electrodes + "\nE" + prog3electrodesALL.IndexOf(electrode) + " = " + electrode.ToString();
                                }
                            }

                            paramForm.label_stimchannel0.Text = prog0electrodes;
                            paramForm.label_stimchannel1.Text = prog1electrodes;
                            paramForm.label_stimchannel2.Text = prog2electrodes;
                            paramForm.label_stimchannel3.Text = prog3electrodes;
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception during updateStimSettingsFile() " + e.Message);
                return;
            }
        }

        public void ReadStimulationSettingsFile()
        {
            // read stim settings file
            try
            {
                String path = "c:\\ctm_config\\ctm_config_stimSettings.txt";
                System.IO.StreamReader file = new StreamReader(path);
                dtStimulationSettingsFile = File.GetLastWriteTime(path);
                List<string> lines = new List<string>();
                while (!file.EndOfStream)
                    lines.Add(file.ReadLine());
                file.Close();

                // stimMode
                int statusi = lines.IndexOf("stimMode");
                SetStimMode = (lines[statusi + 1].Equals("ON") ? 1 : 0);

                // safeMode
                int safeModei = lines.IndexOf("safeMode");
                SetSafeMode = (lines[safeModei + 1].Equals("ON") ? true : false);

                // default
                int defaulti = lines.IndexOf("default");

                DefaultStateSettings.InitialState.RateTargetInHz = Convert.ToDouble(lines[defaulti + 1]);
                DefaultStateSettings.InitialState.Prog0AmpInMilliamps = Convert.ToDouble(lines[defaulti + 2]);
                DefaultStateSettings.Deltas[0].Rise = Convert.ToUInt32(lines[defaulti + 3]);
                DefaultStateSettings.Deltas[0].Fall = Convert.ToUInt32(lines[defaulti + 4]);
                DefaultStateSettings.InitialState.Prog1AmpInMilliamps = Convert.ToDouble(lines[defaulti + 5]);
                DefaultStateSettings.Deltas[1].Rise = Convert.ToUInt32(lines[defaulti + 6]);
                DefaultStateSettings.Deltas[1].Fall = Convert.ToUInt32(lines[defaulti + 7]);
                DefaultStateSettings.InitialState.Prog2AmpInMilliamps = Convert.ToDouble(lines[defaulti + 8]);
                DefaultStateSettings.Deltas[2].Rise = Convert.ToUInt32(lines[defaulti + 9]);
                DefaultStateSettings.Deltas[2].Fall = Convert.ToUInt32(lines[defaulti + 10]);
                DefaultStateSettings.InitialState.Prog3AmpInMilliamps = Convert.ToDouble(lines[defaulti + 11]);
                DefaultStateSettings.Deltas[3].Rise = Convert.ToUInt32(lines[defaulti + 12]);
                DefaultStateSettings.Deltas[3].Fall = Convert.ToUInt32(lines[defaulti + 13]);

                // sleep
                int sleepi = lines.IndexOf("sleep");

                SleepStateSettings.InitialState.RateTargetInHz = Convert.ToDouble(lines[sleepi + 1]);
                SleepStateSettings.InitialState.Prog0AmpInMilliamps = Convert.ToDouble(lines[sleepi + 2]);
                SleepStateSettings.Deltas[0].Rise = Convert.ToUInt32(lines[sleepi + 3]);
                SleepStateSettings.Deltas[0].Fall = Convert.ToUInt32(lines[sleepi + 4]);
                SleepStateSettings.InitialState.Prog1AmpInMilliamps = Convert.ToDouble(lines[sleepi + 5]);
                SleepStateSettings.Deltas[1].Rise = Convert.ToUInt32(lines[sleepi + 6]);
                SleepStateSettings.Deltas[1].Fall = Convert.ToUInt32(lines[sleepi + 7]);
                SleepStateSettings.InitialState.Prog2AmpInMilliamps = Convert.ToDouble(lines[sleepi + 8]);
                SleepStateSettings.Deltas[2].Rise = Convert.ToUInt32(lines[sleepi + 9]);
                SleepStateSettings.Deltas[2].Fall = Convert.ToUInt32(lines[sleepi + 10]);
                SleepStateSettings.InitialState.Prog3AmpInMilliamps = Convert.ToDouble(lines[sleepi + 11]);
                SleepStateSettings.Deltas[3].Rise = Convert.ToUInt32(lines[sleepi + 12]);
                SleepStateSettings.Deltas[3].Fall = Convert.ToUInt32(lines[sleepi + 13]);

                // pre
                int prei = lines.IndexOf("pre");

                PreStateSettings.InitialState.RateTargetInHz = Convert.ToDouble(lines[prei + 1]);
                PreStateSettings.InitialState.Prog0AmpInMilliamps = Convert.ToDouble(lines[prei + 2]);
                PreStateSettings.Deltas[0].Rise = Convert.ToUInt32(lines[prei + 3]);
                PreStateSettings.Deltas[0].Fall = Convert.ToUInt32(lines[prei + 4]);
                PreStateSettings.InitialState.Prog1AmpInMilliamps = Convert.ToDouble(lines[prei + 5]);
                PreStateSettings.Deltas[1].Rise = Convert.ToUInt32(lines[prei + 6]);
                PreStateSettings.Deltas[1].Fall = Convert.ToUInt32(lines[prei + 7]);
                PreStateSettings.InitialState.Prog2AmpInMilliamps = Convert.ToDouble(lines[prei + 8]);
                PreStateSettings.Deltas[2].Rise = Convert.ToUInt32(lines[prei + 9]);
                PreStateSettings.Deltas[2].Fall = Convert.ToUInt32(lines[prei + 10]);
                PreStateSettings.InitialState.Prog3AmpInMilliamps = Convert.ToDouble(lines[prei + 11]);
                PreStateSettings.Deltas[3].Rise = Convert.ToUInt32(lines[prei + 12]);
                PreStateSettings.Deltas[3].Fall = Convert.ToUInt32(lines[prei + 13]);

                // Seizure
                int seizurei = lines.IndexOf("seizure");

                SeizureStateSettings.InitialState.RateTargetInHz = Convert.ToDouble(lines[seizurei + 1]);
                SeizureStateSettings.InitialState.Prog0AmpInMilliamps = Convert.ToDouble(lines[seizurei + 2]);
                SeizureStateSettings.InitialState.Prog1AmpInMilliamps = Convert.ToDouble(lines[seizurei + 3]);
                SeizureStateSettings.InitialState.Prog2AmpInMilliamps = Convert.ToDouble(lines[seizurei + 4]);
                SeizureStateSettings.InitialState.Prog3AmpInMilliamps = Convert.ToDouble(lines[seizurei + 5]);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception during readStimulationSettingsFile() " + e.Message);

                // failed reading from setting file
                // if hardware is connected read current stim settings from it
                if (theSummit != null)
                {
                    if (!(theSummit.IsDisposed))
                    {
                        APIReturnInfo bufferReturnInfo = theSummit.ReadAdaptiveStimSettings(out AdaptiveSettings nofilesettings);
                        if (bufferReturnInfo.RejectCode == 0)
                        {
                            DefaultStateSettings = nofilesettings;
                            SleepStateSettings = nofilesettings;
                            PreStateSettings = nofilesettings;
                            SeizureStateSettings = nofilesettings;
                        }

                        // read stimMode
                        APIReturnInfo stimbufferReturnInfo = theSummit.ReadGeneralInfo(out GeneralInterrogateData interrogateBuffer);
                        if (stimbufferReturnInfo.RejectCode == 0)
                        {
                            TherapyStatusByte therapyStatus = interrogateBuffer.TherapyStatusData;
                            if ((therapyStatus.ActiveGroup == ActiveGroup.Group3 || therapyStatus.ActiveGroup == ActiveGroup.Group0) && // D or A
                                !(therapyStatus.TherapyStatus == InterrogateTherapyStatusTypes.TherapyOff))
                            {
                                SetStimMode = 1;
                            }
                        }
                    }
                }
            }
        }

        public bool WriteStimulationSettingsFile()
        {
            try
            {
                // write stim setting file

                System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\ctm_config\\ctm_config_stimSettings.txt", false);

                // stimMode
                file.WriteLine("stimMode");
                file.WriteLine((stimMode == 1 ? "ON" : "OFF"));

                // safeMode
                file.WriteLine("safeMode");
                file.WriteLine((safeMode ? "ON" : "OFF"));


                file.WriteLine("default");
                file.WriteLine(DefaultStateSettings.InitialState.RateTargetInHz);
                file.WriteLine(DefaultStateSettings.InitialState.Prog0AmpInMilliamps); file.WriteLine(DefaultStateSettings.Deltas[0].Rise); file.WriteLine(DefaultStateSettings.Deltas[0].Fall);
                file.WriteLine(DefaultStateSettings.InitialState.Prog1AmpInMilliamps); file.WriteLine(DefaultStateSettings.Deltas[1].Rise); file.WriteLine(DefaultStateSettings.Deltas[1].Fall);
                file.WriteLine(DefaultStateSettings.InitialState.Prog2AmpInMilliamps); file.WriteLine(DefaultStateSettings.Deltas[2].Rise); file.WriteLine(DefaultStateSettings.Deltas[2].Fall);
                file.WriteLine(DefaultStateSettings.InitialState.Prog3AmpInMilliamps); file.WriteLine(DefaultStateSettings.Deltas[3].Rise); file.WriteLine(DefaultStateSettings.Deltas[3].Fall);

                file.WriteLine("sleep");
                file.WriteLine(SleepStateSettings.InitialState.RateTargetInHz);
                file.WriteLine(SleepStateSettings.InitialState.Prog0AmpInMilliamps); file.WriteLine(SleepStateSettings.Deltas[0].Rise); file.WriteLine(SleepStateSettings.Deltas[0].Fall);
                file.WriteLine(SleepStateSettings.InitialState.Prog1AmpInMilliamps); file.WriteLine(SleepStateSettings.Deltas[1].Rise); file.WriteLine(SleepStateSettings.Deltas[1].Fall);
                file.WriteLine(SleepStateSettings.InitialState.Prog2AmpInMilliamps); file.WriteLine(SleepStateSettings.Deltas[2].Rise); file.WriteLine(SleepStateSettings.Deltas[2].Fall);
                file.WriteLine(SleepStateSettings.InitialState.Prog3AmpInMilliamps); file.WriteLine(SleepStateSettings.Deltas[3].Rise); file.WriteLine(SleepStateSettings.Deltas[3].Fall);

                file.WriteLine("pre");
                file.WriteLine(PreStateSettings.InitialState.RateTargetInHz);
                file.WriteLine(PreStateSettings.InitialState.Prog0AmpInMilliamps); file.WriteLine(PreStateSettings.Deltas[0].Rise); file.WriteLine(PreStateSettings.Deltas[0].Fall);
                file.WriteLine(PreStateSettings.InitialState.Prog1AmpInMilliamps); file.WriteLine(PreStateSettings.Deltas[1].Rise); file.WriteLine(PreStateSettings.Deltas[1].Fall);
                file.WriteLine(PreStateSettings.InitialState.Prog2AmpInMilliamps); file.WriteLine(PreStateSettings.Deltas[2].Rise); file.WriteLine(PreStateSettings.Deltas[2].Fall);
                file.WriteLine(PreStateSettings.InitialState.Prog3AmpInMilliamps); file.WriteLine(PreStateSettings.Deltas[3].Rise); file.WriteLine(PreStateSettings.Deltas[3].Fall);

                file.WriteLine("seizure");
                file.WriteLine(SeizureStateSettings.InitialState.RateTargetInHz);
                file.WriteLine(SeizureStateSettings.InitialState.Prog0AmpInMilliamps);
                file.WriteLine(SeizureStateSettings.InitialState.Prog1AmpInMilliamps);
                file.WriteLine(SeizureStateSettings.InitialState.Prog2AmpInMilliamps);
                file.WriteLine(SeizureStateSettings.InitialState.Prog3AmpInMilliamps);

                file.Close();

                return true;

            }
            catch (Exception e)
            {

                Console.WriteLine("Exception during writeStimulationSettingsFile(): " + e.Message);
                return false;
            }

        }

        public bool StartStim(StimulationParamForm paramForm)
        {
            try
            {
                // all settings are valid -> write settings to file
                SetStimMode = 1;

                if (WriteStimulationSettingsFile())
                {
                    if (paramForm != null)
                        paramForm.Setlabel_settingsTimeText("Setting successfully saved: " + String.Format("{0:G}", DateTime.Now));
                }
                else
                {
                    if (paramForm != null)
                        paramForm.Setlabel_settingsTimeText("Faild saving settings...");
                }

                System.Threading.Thread.Sleep(250);

                // read setting
                ReadStimulationSettingsFile();
                if (paramForm != null)
                    UpdateStimSettingsFile(paramForm);

                SetDoing_stim = true;
                StartStreaming(stimMode, streamingMode, "Default");
                SetDoing_stim = false;
            }
            catch (Exception e)
            {

                Console.WriteLine("Exception during startStim(): " + e.Message);
                return false;
            }

            return true;

        }

        public bool StopStim(StimulationParamForm paramForm)
        {
            try
            {
                CurrState = "Off";

                SetStimMode = 0;

                if (WriteStimulationSettingsFile())
                {
                    if (paramForm != null)
                        paramForm.Setlabel_settingsTimeText("Setting successfully saved: " + String.Format("{0:G}", DateTime.Now));
                }
                else
                {
                    if (paramForm != null)
                        paramForm.Setlabel_settingsTimeText("Faild saving settings...");
                }

                System.Threading.Thread.Sleep(250);

                // read setting
                ReadStimulationSettingsFile();
                if (paramForm != null)
                    UpdateStimSettingsFile(paramForm);

                SetDoing_stim = true;
                StartStreaming(stimMode, streamingMode, CurrState);
                SetDoing_stim = false;
            }
            catch (Exception e)
            {

                Console.WriteLine("Exception during stopStim(): " + e.Message + " " + e.ToString());
                return false;
            }

            return true;
        }

        // Event handler that triggers when an adaptive 
        private static void TheSummit_dataReceivedDetector(object sender, AdaptiveDetectEvent e)
        {
            if (adaptivePacketCount == 50)
            {
                Console.WriteLine("\n --- AdaptiveS Packet received! --- ");
                Console.WriteLine("Current State: " + e.CurrentAdaptiveState.ToString());

                if (stimMode == 1)
                {
                    // The current rate period of the embedded group's stimulation
                    Console.WriteLine("StimRateInHz: " + e.StimRateInHz.ToString());
                    // Byte array incidating the current program amplitudes
                    string currentAmps = "";
                    foreach (var item in e.CurrentProgramAmplitudesInMilliamps)
                    {
                        currentAmps = currentAmps + " " + item.ToString();
                    }
                    Console.WriteLine("Current program amplitudes: " + currentAmps);
                }
                Console.WriteLine("Current Detector 0 status: " + e.Ld0DetectionStatus);
                // Console.WriteLine("Current Detector 1 status: " + e.Ld1DetectionStatus);
                Console.WriteLine("---    ---   ---    --- \n");

                adaptivePacketCount = 0;
            }

            adaptivePacketCount++;
            Console.Write("*");
        }

        // Resets the POR bit if it was set
        static void ResetPOR(SummitSystem theSummit)
        {
            Console.WriteLine("POR was set, resetting...");

            // reset POR
            theSummit.ResetErrorFlags(Medtronic.NeuroStim.Olympus.DataTypes.Core.StatusBits.Por);

            // check battery
            theSummit.ReadBatteryLevel(out BatteryStatusResult theStatus);

            // perform interrogate command and check if therapy is enabled.
            theSummit.ReadGeneralInfo(out GeneralInterrogateData interrogateBuffer);
            if (interrogateBuffer.IsTherapyUnavailable)
            {
                Console.WriteLine("Therapy still unavailable after reset");
                return;
            }
        }

        #endregion

        #region Impedance test  

        public void StartTestingImpedance(checkImpForm ImpedanceForm)
        {
            ///Put text to textbox and avoid the cross-threading issue.
            void AppendTextBox(string value)
            {
                if (InvokeRequired)
                {
                    this.Invoke(new Action<string>(AppendTextBox), new object[] { value });
                    return;
                }
                ImpedanceForm.textbox1.Text += value;
            }
            ///Clear textbox and avoid cross validations.
            void ClearTextBox()
            {
                if (InvokeRequired)
                {
                    this.Invoke(new Action(ClearTextBox));
                    return;
                }
                ImpedanceForm.textbox1.Clear();
            }

            GlobalVariablesForImpedanceTest.isRunning_impedanceTest = true;
            ClearTextBox();
            this.Invoke((MethodInvoker)delegate ()
            {
                //  ImpedanceForm.textbox1.Clear();
                ImpedanceForm.textbox1.Font = new Font("Microsoft Sans Serif", 11.5f);
            });
            if (theSummit == null)
            {
                AppendTextBox("Summit Not connected. Exiting. ");
                Console.WriteLine("Summit Not connected. Exiting Now. ");
                return;
            }

            SetDoing_impTest = true; // imp test started
            StartStreaming(0, 0, "Off"); // stop all

            var time = DateTime.Now;
            string formattedTime = time.ToString("yyyy/MM/dd hh:mm:ss tt");
            Console.WriteLine("Started Impedance Test. Please Wait.......");
            AppendTextBox("Started Impedance Test. Please Wait.......");
            // Performing lead integrity test.                                       
            this.Invoke((MethodInvoker)delegate ()
            {
                ImpedanceForm.pgsBar_Impedance.Visible = true;
                ImpedanceForm.pgsBar_Impedance.Value = 0;
                ImpedanceForm.pgsBar_Impedance.Maximum = max_channels_impedance;
                ImpedanceForm.pgsBar_Impedance.Step = 1;
            });
            ///Save the impedance output to dropbox.
            string impedence_test_string = homeFolder + Environment.MachineName + "\\Impedance_output.txt";
            using (StreamWriter Im = File.AppendText(impedence_test_string))
            {
                Im.WriteLine("Channel Pair\tImpedance" + Environment.NewLine);
                ClearTextBox();
                AppendTextBox("Channel Pair\tImpedance" + Environment.NewLine);
                for (int i = 0; i < max_channels_impedance; i++)
                {
                    byte x = Convert.ToByte(i);
                    ///Keep on running the Impedance test until it finished all channels or aborted by user.
                    if (GlobalVariablesForImpedanceTest.isRunning_impedanceTest)
                    {
                        APIReturnInfo testReturnInfo = theSummit.LeadIntegrityTest(new List<Tuple<byte, byte>> { new Tuple<byte, byte>(x, 16) }, out LeadIntegrityTestResult testResultBuffer);
                        if (testReturnInfo.RejectCode == 0 && testResultBuffer != null)
                        {
                            Console.WriteLine("Channel " + "E" + i.ToString() + " - " + "Device" + ":\t" + testResultBuffer.PairResults[0].Impedance.ToString() + " \u03A9");
                            AppendTextBox("E" + i.ToString() + " - " + "Device" + ":\t" + testResultBuffer.PairResults[0].Impedance.ToString() + " \u03A9" + Environment.NewLine);
                            Im.WriteLine("E" + i.ToString() + " - " + "Device" + ":\t" + testResultBuffer.PairResults[0].Impedance.ToString() + " \u03A9" + Environment.NewLine);
                            this.Invoke((MethodInvoker)delegate ()
                            {
                                ImpedanceForm.pgsBar_Impedance.PerformStep();
                            });
                        }
                        else
                        {
                            Console.WriteLine("E" + i.ToString() + " - " + " Device" + ":\t" + "Test failed. " + Environment.NewLine);
                            AppendTextBox("E" + i.ToString() + " - " + "Device" + ":\t" + "Test failed. " + Environment.NewLine);
                            Im.WriteLine("E" + i.ToString() + " - " + " Device" + ":\t" + "Test failed. " + Environment.NewLine);
                            ImpedanceForm.Reset_ImpedanceTest();
                            AppendTextBox("Sensing and stimulation restarted automatically." + Environment.NewLine);
                        }
                    }
                    else
                    {
                        Console.WriteLine("The impedance test was aborted." + Environment.NewLine + "Sensing and stimulation restarted automatically." + Environment.NewLine);
                        AppendTextBox("The impedance test was aborted." + Environment.NewLine);
                        AppendTextBox("Sensing and stimulation restarted automatically." + Environment.NewLine);
                        Im.WriteLine("The impedance test was aborted.");
                        GlobalVariablesForImpedanceTest.isRunning_impedanceTest = false;
                        return;
                    }
                }
                AppendTextBox("Impedance test completed: " + formattedTime + Environment.NewLine);
                Im.WriteLine("Impedance test completed: " + formattedTime);
            }
            Console.WriteLine("Finished Impedance Test");
            ImpedanceForm.Reset_ImpedanceTest();
            AppendTextBox("Sensing and stimulation restarted automatically." + Environment.NewLine);
            this.Invoke((MethodInvoker)delegate ()
            {
                ImpedanceForm.pgsBar_Impedance.Value = 0;
                ImpedanceForm.pgsBar_Impedance.Visible = false;
            });
        }

        public void StopTestingImpedance(checkImpForm ImpedanceForm)
        {
            GlobalVariablesForImpedanceTest.isRunning_impedanceTest = false;
            this.Invoke((MethodInvoker)delegate ()
            {
                ImpedanceForm.pgsBar_Impedance.Visible = false;
            });
        }

        /// <summary>
        /// The following function is used to change the first character of a string to upper case if it is in lowercase. 
        /// Needed just for the data directory C:/**. The first letter we got from the code is lower case c.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FirstCharacterToUpper(string str)
        {
            if (String.IsNullOrEmpty(str) || Char.IsUpper(str, 0))
            {
                return str;
            }
            else
            {
                return Char.ToUpperInvariant(str[0]) + str.Substring(1);
            }
        }

        static long GetDirectorySize(string p)
        {
            // 1.
            // Get array of all file names.
            string[] a = Directory.GetFiles(p, "*.*");

            // 2.
            // Calculate total bytes of all files in a loop.
            long b = 0;
            foreach (string name in a)
            {
                // 3.
                // Use FileInfo to get length of each file.
                FileInfo info = new FileInfo(name);
                b += info.Length;
            }
            // 4.
            // Return total size
            return b;
        }
        //Get the percentage of free disk space.
        private long GetPercentageFreeSpace(string driveName)
        {
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady && drive.Name == driveName)
                {
                    return (100 * drive.TotalFreeSpace / drive.TotalSize);
                }
            }
            return -1;
        }

        #endregion

        #region Configure LDA
        /// <summary>
        /// Work under progress...................
        /// </summary>
        /// <param name="configLDAform"></param>
        /// 

        /*******************************************************************/
        public void ReadLDAConfigurationFile()
        {
            //Set Default values. Just in case if there is no configuration file available.           
            Input1[0] = 2;
            Input1[1] = 4;
            Input1[2] = 8;
            Input1[3] = 12;
            Input2[0] = 2;
            Input2[1] = 6;
            Input2[2] = 18;
            Input2[3] = 22;
            LD0_Onset_Duration = 2;
            LD0_Termination_Duration = 1;
            LD0_HoldOff_Time = 3;
            LD0_Blanking_Duration_Upon_StateChange = 11;
            LDA_Threshold = 3000;
            input_channel_index[0] = 0;
            input_channel_index[1] = 1;
            LD0_Fractional_FixedPointValue = 8;
            for (int i = 0; i < vector_size; i++)
            {
                Weight_Vector[i] = 1;
                Normalization_Multiply_Vector[i] = 1;
                Normalization_Subtract_Vector[i] = 1;
            }

            try
            {
                // Read the file line by line and save the values.                  
                string[] lines = System.IO.File.ReadAllLines(@"C:/ctm_config/ins_config_ldaSettings.txt");
                var lineCount = lines.Length;
                if (lineCount < 31)
                {
                    Console.WriteLine("Insufficient LDA configuration information. Using default values for now.");
                    return;
                }

                //Onset duration and termination duration only use ushort which should be a integers.

                for (int i = 0; i < vector_size; i++)
                {
                    Input1[i] = double.Parse(lines[i + 1]);
                    Input2[i] = double.Parse(lines[i + 6]);
                    Weight_Vector[i] = uint.Parse(lines[i + 19]);
                    Normalization_Multiply_Vector[i] = uint.Parse(lines[i + 23]);
                    Normalization_Subtract_Vector[i] = uint.Parse(lines[i + 27]);
                }
                LD0_Onset_Duration = UInt16.Parse(lines[10]);
                LD0_Termination_Duration = Convert.ToUInt16(lines[11]);
                LDA_Threshold = Convert.ToUInt32(lines[12]);
                input_channel_index[0] = Convert.ToUInt16(lines[13]);
                input_channel_index[1] = Convert.ToUInt16(lines[14]);
                fft_check_status = Convert.ToBoolean(lines[15]);
                LD0_HoldOff_Time = Convert.ToUInt16(lines[16]);
                LD0_Blanking_Duration_Upon_StateChange = Convert.ToUInt16(lines[17]);
                LD0_Fractional_FixedPointValue = Convert.ToByte(lines[18]);
            }
            catch (FileNotFoundException er)
            {
                Console.WriteLine("LDA config file exception: " + er.Message);
                //return;
            }
        }

        public void WriteLDASettingsFile(ConfigureLDAForm configLDAform)
        {
            string path = "c:\\ctm_config\\ins_config_lDASettings.txt";
            if (File.Exists(path))
            {
                //readLDAConfigurationFile();
                string[] lines = System.IO.File.ReadAllLines(path);
                var lineCount = lines.Length;
                if (lineCount > 16)
                {
                    lda_file_exists = true;
                }
                else
                {
                    lda_file_exists = false;
                }

            }
            else
            {
                lda_file_exists = false;
                Console.WriteLine("ins_config_lDASettings.txt file doesn't exist. ");
            }

            try
            {
                // write LDA setting file
                System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\ctm_config\\ins_config_lDASettings.txt", false);
                /*
                 The output will be saved in sequence as:
                 1. Input 1 channel
                 2. Input 1 Band 1 low frequency
                 3. Input 1 Band 1 high frequency
                 4. Input 1 Band 2 low frequency
                 5. Input 1 Band 2 high frequency
                 6. Input 2 channel
                 7. Input 2 Band 1 low frequency
                 8. Input 2 Band 1 high frequency
                 9. Input 2 Band 2 low frequency
                 10. Input 2 Band 2 high frequency
                 11. Onset duration
                 12. Termination Duration
                 13. LDA threshold 
                 14. combobox1 selected item 
                 15. combobox2 selected item
                 16. FFT check box (True or false)
                 17. HoldOff
                 18. Blanking Duration 
                 19 - LD0_Fractional_FixedPointValue
                 20 - 23. weight vector
                 24 - 27. Normalization multiply vector
                 28 - 31. Normalization subtract vector.                                 
                 */
                file.WriteLine(configLDAform.cmb_Input1_channels.Text);             //line: 1 
                file.WriteLine(configLDAform.numericUpDownInput1LowBand1.Value.ToString()); //line: 2
                file.WriteLine(configLDAform.numericUpDownInput1HighBand1.Value.ToString()); //line: 3
                file.WriteLine(configLDAform.numericUpDownInput1LowBand2.Value.ToString());//line: 4
                file.WriteLine(configLDAform.numericUpDownInput1HighBand2.Value.ToString());//line: 5
                file.WriteLine(configLDAform.cmb_Input2_channels.Text); //line: 6
                file.WriteLine(configLDAform.numericUpDownInput2LowBand1.Value.ToString()); //line: 7
                file.WriteLine(configLDAform.numericUpDownInput2HighBand1.Value.ToString());//line: 8
                file.WriteLine(configLDAform.numericUpDownInput2LowBand2.Value.ToString());//line: 9
                file.WriteLine(configLDAform.numericUpDownInput2HighBand2.Value.ToString());//line: 10
                file.WriteLine(configLDAform.numericUpDownOnsetDuration.Value.ToString());//line: 11
                file.WriteLine(configLDAform.numericUpDownTerminationDuration.Value.ToString());//line: 12                
                file.WriteLine(configLDAform.numeric_UpDown_Threshold.Value.ToString());//line: 13               
                //the following is for the channels used for lda.                
                file.WriteLine(configLDAform.cmb_Input1_channels.SelectedIndex.ToString());//line: 14
                file.WriteLine(configLDAform.cmb_Input2_channels.SelectedIndex.ToString());    //line: 15  
                file.WriteLine(fft_check_status); //line: 16
                if (!lda_file_exists)
                {
                    file.WriteLine("3");//line: 17 Holdoff time
                    file.WriteLine("11");//line: 18 Blanking duration
                    file.WriteLine("8");//line: 19 LD0_Fractional_FixedPointValue               
                    for (int i = 0; i < vector_size; i++)//line: 20-23
                    {
                        file.WriteLine(Weight_Vector[i].ToString());
                    }
                    for (int j = 0; j < vector_size; j++) //line: 24-27
                    {
                        file.WriteLine(Normalization_Multiply_Vector[j].ToString());
                    }
                    for (int k = 0; k < vector_size; k++)//line: 28-31
                    {
                        file.WriteLine(Normalization_Subtract_Vector[k].ToString());
                    }

                }
                else
                {
                    file.WriteLine(LD0_HoldOff_Time);//line: 17 Holdoff time
                    file.WriteLine(LD0_Blanking_Duration_Upon_StateChange);//line: 18 Blanking duration
                    file.WriteLine(LD0_Fractional_FixedPointValue);//line: 19 LD0_Fractional_FixedPointValue               
                    for (int i = 0; i < vector_size; i++)//line: 20-23
                    {
                        file.WriteLine(Weight_Vector[i].ToString());
                    }
                    for (int j = 0; j < vector_size; j++) //line: 24-27
                    {
                        file.WriteLine(Normalization_Multiply_Vector[j].ToString());
                    }
                    for (int k = 0; k < vector_size; k++)//line: 28-31
                    {
                        file.WriteLine(Normalization_Subtract_Vector[k].ToString());
                    }
                }


                file.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception during writeLDASettingsFile(): " + e.Message);
                return;
            }
        }
        public void CopyLDAConfigurationFromUI(ConfigureLDAForm configLDAform)
        {
            //Onset duration and termination duration only use ushort which should be a integers.
            try
            {
                LD0_Onset_Duration = (ushort)configLDAform.numericUpDownOnsetDuration.Value;
                LD0_Termination_Duration = (ushort)configLDAform.numericUpDownTerminationDuration.Value;
                Input1[0] = Convert.ToDouble(configLDAform.numericUpDownInput1LowBand1.Value);
                Input1[1] = Convert.ToDouble(configLDAform.numericUpDownInput1HighBand1.Value);
                Input1[2] = Convert.ToDouble(configLDAform.numericUpDownInput1LowBand2.Value);
                Input1[3] = Convert.ToDouble(configLDAform.numericUpDownInput1HighBand2.Value);
                Input2[0] = Convert.ToDouble(configLDAform.numericUpDownInput2LowBand1.Value);
                Input2[1] = Convert.ToDouble(configLDAform.numericUpDownInput2HighBand1.Value);
                Input2[2] = Convert.ToDouble(configLDAform.numericUpDownInput2LowBand2.Value);
                Input2[3] = Convert.ToDouble(configLDAform.numericUpDownInput2HighBand2.Value);
                LDA_Threshold = (uint)configLDAform.numeric_UpDown_Threshold.Value;
                Console.WriteLine("LDA threshold from UI is: " + LDA_Threshold);
                if (LDA_Threshold > 2147483647)
                {
                    SetUserMessage("The LDA threshold is greater than the maximum value it can accept (2147483647). " +
                        "We are assigning the medtronic assigned value (lower threshold: 7741395) for now.");
                    LDA_Threshold = 7741395;
                }
                input_channel_index[0] = configLDAform.cmb_Input1_channels.SelectedIndex;
                input_channel_index[1] = configLDAform.cmb_Input2_channels.SelectedIndex;
                fft_check_status = configLDAform.chkBox_SaveFftData.Checked;
            }
            catch (FileNotFoundException er)
            {
                Console.WriteLine("Exception during CopyLDAConfigurationFromUI() - Couldn't update the LDA config setup with error: " + er.Message);
                return;
            }
        }

        public void Write_LDA_UIValues(ConfigureLDAForm comboBoxForm)
        {
            //Populate the UI with the values from previous data or applied new data.           
            comboBoxForm.cmb_Input1_channels.DisplayMember = "text";
            comboBoxForm.cmb_Input1_channels.ValueMember = "value";
            comboBoxForm.cmb_Input2_channels.DisplayMember = "text";
            comboBoxForm.cmb_Input2_channels.ValueMember = "value";
            ch_names = new string[vector_size];
            try
            {
                //read settings file just in case it is not read properly. Sometimes no connections etc can cause the problem. 
                ReadSettingsFile();
                ReadLDAConfigurationFile();
                for (int i = 0; i < vector_size; i++)
                {
                    ch_names[i] = channelMinusBox[i] + "-" + channelPlusBox[i];
                }
                comboBoxForm.cmb_Input1_channels.Items.Clear();
                comboBoxForm.cmb_Input2_channels.Items.Clear();
                for (int i = 0; i < vector_size; i++)
                {
                    comboBoxForm.cmb_Input1_channels.Items.Add(ch_names[i]);
                    comboBoxForm.cmb_Input2_channels.Items.Add(ch_names[i]);
                }
            }
            catch
            {
                Console.WriteLine("Exception during readSettingsFile(), and readLDAConfigurationFile() within the Write_LDA_UIValues().");
                return;
            }

            comboBoxForm.cmb_Input1_channels.SelectedIndex = input_channel_index[0];
            comboBoxForm.cmb_Input2_channels.SelectedIndex = input_channel_index[1];
            comboBoxForm.numericUpDownInput1LowBand1.Value = Convert.ToDecimal(Input1[0]);
            comboBoxForm.numericUpDownInput1HighBand1.Value = Convert.ToDecimal(Input1[1]);
            comboBoxForm.numericUpDownInput1LowBand2.Value = Convert.ToDecimal(Input1[2]);
            comboBoxForm.numericUpDownInput1HighBand2.Value = Convert.ToDecimal(Input1[3]);
            comboBoxForm.numericUpDownInput2LowBand1.Value = Convert.ToDecimal(Input2[0]);
            comboBoxForm.numericUpDownInput2HighBand1.Value = Convert.ToDecimal(Input2[1]);
            comboBoxForm.numericUpDownInput2LowBand2.Value = Convert.ToDecimal(Input2[2]);
            comboBoxForm.numericUpDownInput2HighBand2.Value = Convert.ToDecimal(Input2[3]);
            comboBoxForm.numericUpDownOnsetDuration.Value = LD0_Onset_Duration;
            comboBoxForm.numericUpDownTerminationDuration.Value = LD0_Termination_Duration;
            comboBoxForm.numeric_UpDown_Threshold.Value = Convert.ToUInt32(LDA_Threshold); //I changed here.
            Console.WriteLine("LDA threshold after update: " + LDA_Threshold);
            if (LDA_Threshold > 2147483647)
            {
                SetUserMessage("The LDA threshold is greater than the maximum value it can accept (2147483647). " +
                    "We are assigning the medtronic assigned value (lower threshold: 7741395) for now.");
                comboBoxForm.numeric_UpDown_Threshold.Value = 7741395;
            }

            if (fft_check_status)
            {
                comboBoxForm.chkBox_SaveFftData.Checked = true;
            }
            else
            {
                comboBoxForm.chkBox_SaveFftData.Checked = false;
            }
        }
        public void UpdateBands_LDA(ConfigureLDAForm comboBoxForm)
        {
            //Populate the UI with the values from previous data or applied new data. 
            try
            {
                comboBoxForm.numericUpDownInput1LowBand1.Value = Convert.ToDecimal(Input1_Corrected[0]);
                comboBoxForm.numericUpDownInput1HighBand1.Value = Convert.ToDecimal(Input1_Corrected[1]);
                comboBoxForm.numericUpDownInput1LowBand2.Value = Convert.ToDecimal(Input1_Corrected[2]);
                comboBoxForm.numericUpDownInput1HighBand2.Value = Convert.ToDecimal(Input1_Corrected[3]);
                comboBoxForm.numericUpDownInput2LowBand1.Value = Convert.ToDecimal(Input2_Corrected[0]);
                comboBoxForm.numericUpDownInput2HighBand1.Value = Convert.ToDecimal(Input2_Corrected[1]);
                comboBoxForm.numericUpDownInput2LowBand2.Value = Convert.ToDecimal(Input2_Corrected[2]);
                comboBoxForm.numericUpDownInput2HighBand2.Value = Convert.ToDecimal(Input2_Corrected[3]);
            }
            catch
            {
                Console.WriteLine("Exception during updating the LDA bands.");
                return;
            }

        }

        #endregion
    }
}

