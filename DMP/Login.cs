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
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Threading;

namespace DMP
{
    public partial class Login : Form
    {
        private String User;
        private Boolean logingclick;
        // The following passwords are hashed using PBKDF2 (RFC 2898) with 1000 iterations
        private String patientPass = "0abc9868df41564d02bd4464ad4b2791";
        private String patientSalt = "dc7656f79dee8d53";
        private String physicianPass = "b572f0f082cef2663f34a9c1fd77ce04";
        private String physicianSalt = "fc085a0ac27777be";

        /// <summary>
        /// 
        /// Requirement 4.1.3: Password Validation
        /// After the user enters a password and clicks OK, the software compares the entered password to a hashed, encrypted locally stored password for validation.
        /// If the password is correct the program enters Patient or Physician mode as appropriate.
        /// 
        /// </summary>

        public Login(String user)
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.WindowState = FormWindowState.Normal;


            User = user;
            logingclick = false;
            if (User == "Patient")
            {
                label1.Text = "Patient Data";
            }
            else // if User == "Physician"
            {
                label1.Text = "Physician Configurations";
            }
        }

        //btn_Login Click event
        private void Button_Login_Click(object sender, EventArgs e)
        {
            if (textBox_Password.Text == "")
            {
                CustomMsgBox.Show("Please enter Password!", "Error", "OK");
                return;
            }
            else
            {
                if (User == "Patient" && VerifyPassword(textBox_Password.Text, patientPass, patientSalt))
                {
                    ///Run Patient Screen
                    Patient PatientScreen = null;
                    foreach (Form childForm in this.MdiParent.MdiChildren)
                    {
                        if (childForm.Name == "Patient")
                        {
                            PatientScreen = (Patient)childForm;
                        }
                    }
                    if (PatientScreen == null)
                    {
                        PatientScreen = new Patient()
                        {
                            MdiParent = this.MdiParent
                        };
                    }
                    PatientScreen.Show();
                    logingclick = true;
                    this.Close();

                }
                else if (User == "Physician" && VerifyPassword(textBox_Password.Text, physicianPass, physicianSalt))
                {
                    ///Run Physician Screen
                    Physician PhysiciantScreen = null;
                    foreach (Form childForm in this.MdiParent.MdiChildren)
                    {
                        if (childForm.Name == "Physician")
                        {
                            PhysiciantScreen = (Physician)childForm;
                        }
                    }
                    if (PhysiciantScreen == null)
                    {
                        PhysiciantScreen = new Physician()
                        {
                            MdiParent = this.MdiParent
                        };
                    }
                    PhysiciantScreen.Show();
                    logingclick = true;
                    this.Close();
                }
                else
                {
                    CustomMsgBox.Show("Login Failed!", "Error", "OK");
                }
            }
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!logingclick)
            {
                foreach (Form childForm in this.MdiParent.MdiChildren)
                {
                    if (childForm.Name == "Home")
                        childForm.Show();
                }
            }
            ((DMP_Main_MDIParent)this.MdiParent).ToggleOSK(false);
        }


        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox_Password_Enter(object sender, EventArgs e)
        {
                ((DMP_Main_MDIParent)this.MdiParent).ToggleOSK(true);
        }

        private void textBox_Password_Leave(object sender, EventArgs e)
        {
                ((DMP_Main_MDIParent)this.MdiParent).ToggleOSK(true);
        }
		
		private bool VerifyPassword(string password_entered, string password_hashed_validate, string salt_string)
        {
            // Passwords and salt are stored as hex-based strings for ease of use
            int myIterations = 1000;
            byte[] salt = StringToByteArray(salt_string);
            Rfc2898DeriveBytes k1 = new Rfc2898DeriveBytes(password_entered, salt, myIterations);
            byte[] my_hash = k1.GetBytes(16);
            string hashed_string = ByteArrayToString(my_hash);

            if (hashed_string == password_hashed_validate)
                return true;
           
            return false;
        }
        
        static string ByteArrayToString(byte[] ba)
        {
            return BitConverter.ToString(ba).ToLower().Replace("-", "");
        }

        public static byte[] StringToByteArray(string hex)
        {
            var NumberChars = hex.Length;
            var bytes = new byte[NumberChars / 2];

            for (var i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);

            return bytes;
        }
    }
}
