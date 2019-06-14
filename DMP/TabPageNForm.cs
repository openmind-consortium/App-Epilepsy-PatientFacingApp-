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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace DMP
{
    public class MyTabPage : TabPage
    {
        protected internal Form frm;
        public MyTabPage(MyFormPage frm_content)
        {
            this.frm = frm_content;
            this.Controls.Add(frm_content.pnl);
            this.Text = frm_content.Text;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                frm.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class MyFormPage : Form
    {
        public Panel pnl;
    }
}
