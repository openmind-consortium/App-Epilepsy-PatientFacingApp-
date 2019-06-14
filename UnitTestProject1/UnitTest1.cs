/* Copyright (c) 2017-2018, Mayo Foundation for Medical Education and Research (MFMER), All rights reserved. 
Academic non-commercial use of this software is allowed with expressed permission of the developers. 
MFMER disclaims all implied warranties of merchantability and fitness for a particular purpose with 
respect to this software, its application, and any verbal or written statements regarding its use. 
The software may not be distributed to third parties without consent of MFMER. Use of this software 
constitutes acceptance of these terms. 
Contributors: Daniel Crepeau, Tal Pal Attia, Jan Cimbalnik, Hari Guragain, Mona Nasseri, Vaclav Kremen, 
Benjamin Brinkmann, Matt Stead, Gregory Worrell.  */

using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        public unsafe struct CHANNEL_STATE
        {
            public fixed byte first[5000];   // CHANNEL_STATE size is 2576, according to sizeof() in c.
        }

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


        [TestMethod]
        public void TestMethod1()
        {
            // arrange

            int i;
            double sampling_frequency;
            double sine_amplitude;
            double sine_frequency;
            double seconds_per_block;
            CHANNEL_STATE mef_channel_state_struct;
            int[] samps;
            ulong[] packet_times;
            ulong base_timestamp;
            string dir_name;
            string chan_name;

            // initialize variables
            sampling_frequency = 1000.0;   // Hz
            seconds_per_block = 1.0;
            dir_name = "c:/Dropbox/sine-test";
            chan_name = "sine-test";

            // allocate buffers
            samps = new int[10000];
            packet_times = new ulong[10000];

            // set base data
            sine_amplitude = 20000.0;
            sine_frequency = 10.0;
            base_timestamp = 946684800000000;  // midnight, 1 January 2000

            // generate 10000 samples of sine wave
            for (i = 0; i < 10000; i++)
            {
                samps[i] = (int)(sine_amplitude * Math.Sin(2 * Math.PI * i * sine_frequency / sampling_frequency));
                packet_times[i] = (ulong)(base_timestamp + (i * ((1e6) / sampling_frequency)));  // extrapolate, putting into microseconds
            }

            // act

            // initialize MEF3 library
            initialize_meflib_dll();

            // create MEF3 channel
            initialize_mef_channel_data(ref mef_channel_state_struct,
                                        seconds_per_block,           // seconds per block
                                        chan_name, // channel name
                                        0,// bit shift flag, set to 1 for neuralynx, to chop off 2 least-significant sample bits
                                        0.0,           // low filt freq
                                        9000.0,        // high filt freq
                                        -1.0,           // notch filt freq
                                        60.0,          // AC line freq
                                        1.0,           // units conversion factor
                                        "not entered", // chan description
                                        sampling_frequency, // starter freq for channel, make it as high or higher than actual freq to allocate buffers
                                        1 * 1000000, // block interval, needs to be correct, this value is used for all channels
                                        1,             // chan number
                                        dir_name,      // absolute path of session
                                        (float)-6.0,                  // GMT offset
                                        "not entered",        // session description
                                        "anon",                // anonymized subject name
                                        "First",                // subject first name
                                        "Last",                 // subject second name
                                        "",               // subject ID
                                        "",           // institution
                                        null,                  // level 1 password (technical data)
                                        null,                  // level 2 password (subject data), must also specify level 1 password if specifying level 2
                                        "not entered",        // study comments
                                        "not entered",         // channel comments
                                        0                      // secs per segment
                                        );

            // add buffered data to MEF channel
            write_mef_channel_data(ref mef_channel_state_struct, packet_times, samps, 10000, seconds_per_block, sampling_frequency);

            // close MEF channel
            close_mef_channel(ref mef_channel_state_struct);

            // assert

            // Test file sizes against expected file sizes

            // TMET file
            // 16384 is the size of all metadata files
            Assert.AreEqual(16384, new System.IO.FileInfo(
                dir_name + ".mefd/" +
                chan_name + ".timd/" +
                chan_name + "-000000.segd/" +
                chan_name + "-000000.tmet").Length);

            // TIDX file
            // 1584 -> 1024 byte universal header + (56 bytes per index entry * 10 index entries)
            Assert.AreEqual(1584, new System.IO.FileInfo(
               dir_name + ".mefd/" +
               chan_name + ".timd/" +
               chan_name + "-000000.segd/" +
               chan_name + "-000000.tidx").Length);

            // TDAT file
            // 29904 is the size of 1024 byte universal header + these specific 10 RED blocks
            Assert.AreEqual(29904, new System.IO.FileInfo(
               dir_name + ".mefd/" +
               chan_name + ".timd/" +
               chan_name + "-000000.segd/" +
               chan_name + "-000000.tdat").Length);
        }

    }
}
