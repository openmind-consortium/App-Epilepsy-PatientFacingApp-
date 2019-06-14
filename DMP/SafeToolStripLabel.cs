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
    public class SafeToolStripLabel : ToolStripStatusLabel
    {
        delegate void SetText(string text);
        delegate string GetString();

        public override string Text
        {
            get
            {
                if ((base.Parent != null) &&        // Make sure that the container is already built
                    (base.Parent.InvokeRequired))   // Is Invoke required?
                {
                    GetString getTextDel = delegate ()
                                            {
                                                return base.Text;
                                            };
                    string text = String.Empty;
                    try
                    {
                        // Invoke the SetText operation from the Parent of the ToolStripStatusLabel
                        text = (string)base.Parent.Invoke(getTextDel, null);
                    }
                    catch
                    {
                    }

                    return text;
                }
                else
                {
                    return base.Text;
                }
            }

            set
            {
                // Get from the container if Invoke is required
                if ((base.Parent != null) &&        // Make sure that the container is already built
                    (base.Parent.InvokeRequired))   // Is Invoke required?
                {
                    SetText setTextDel = delegate (string text)
                    {
                        base.Text = text;
                    };

                    try
                    {
                        // Invoke the SetText operation from the Parent of the ToolStripStatusLabel
                        base.Parent.Invoke(setTextDel, new object[] { value });
                    }
                    catch
                    {
                    }
                }
                else
                    base.Text = value;
            }
        }
    }
}
