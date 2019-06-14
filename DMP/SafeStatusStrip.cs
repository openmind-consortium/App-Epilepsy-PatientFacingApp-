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
using System.Text;
using System.Windows.Forms;

using DMP;

namespace SafeControls
{
    /// <summary>
    /// Provides a Safe Text property for a ToolStripStatusLabel
    /// https://www.codeproject.com/articles/19506/a-thread-safe-toolstripstatuslabel-control?fid=435744&df=90&mpp=10&prof=True&sort=Position&view=Topic&spc=Relaxed&fr=11
    /// 
    /// </summary>
    public class SafeStatusStrip : StatusStrip
    {
        delegate void SetText(ToolStripLabel toolStrip, string text);

        public void SafeSetText(ToolStripLabel toolStripLabel, string text)
        {
            if (InvokeRequired)
            {
                SetText setTextDel = delegate(ToolStripLabel toolStrip, string textVal)
                {
                    foreach (ToolStripItem item in base.Items)
                    {
                        if (item == toolStrip)
                        {
                            item.Text = textVal;
                        }
                    }
                };

                try
                {
                    Invoke(setTextDel, new object[] { toolStripLabel, text });
                }
                catch
                {
                }
            }
            else
            {
                foreach (ToolStripItem item in base.Items)
                {
                    if (item == toolStripLabel)
                    {
                        item.Text = text;
                    }
                }
            }
        }
    }
}
