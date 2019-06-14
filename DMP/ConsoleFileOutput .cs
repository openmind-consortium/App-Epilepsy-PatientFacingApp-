/* Copyright (c) 2017-2018, Mayo Foundation for Medical Education and Research (MFMER), All rights reserved. 
Academic non-commercial use of this software is allowed with expressed permission of the developers. 
MFMER disclaims all implied warranties of merchantability and fitness for a particular purpose with 
respect to this software, its application, and any verbal or written statements regarding its use. 
The software may not be distributed to third parties without consent of MFMER. Use of this software 
constitutes acceptance of these terms. 
Contributors: Daniel Crepeau, Tal Pal Attia, Jan Cimbalnik, Hari Guragain, Mona Nasseri, Vaclav Kremen, 
Benjamin Brinkmann, Matt Stead, Gregory Worrell.  */

using System;
using System.IO;
using System.Text;

namespace DMP
{
    /// <summary>
    /// 
    ///  Supporting class for saving console output to file
    ///  http://dejanstojanovic.net/aspnet/2015/september/multiple-console-output-in-c/
    ///  
    /// </summary>

    public class ConsoleFileOutput : TextWriter
    {
        #region Fields
        private Encoding encoding = Encoding.UTF8;
        private StreamWriter writer;
        private TextWriter console;
        #endregion

        #region Properties
        public override Encoding Encoding
        {
            get
            {
                return encoding;
            }
        }
        #endregion

        #region Constructors
        public ConsoleFileOutput(string filePath, TextWriter console, Encoding encoding = null)
        {
            if (encoding != null)
            {
                this.encoding = encoding;
            }
            this.console = console;
            this.writer = new StreamWriter(filePath, false, this.encoding)
            {
                AutoFlush = true
            };
        }
        #endregion


        #region Overrides
        public override void Write(string value)
        {
            Console.SetOut(console);
            Console.Write(value);
            Console.SetOut(this);
            this.writer.Write(value);
        }

        public override void WriteLine(string value)
        {
            Console.SetOut(console);
            Console.WriteLine(value);
            this.writer.WriteLine(value);
            Console.SetOut(this);
        }

        public override void Flush()
        {
            this.writer.Flush();
        }

        public override void Close()
        {
            this.writer.Close();
        }
        #endregion


        #region IDisposable
        public new void Dispose()
        {
            this.writer.Flush();
            this.writer.Close();
            this.writer.Dispose();
            base.Dispose();
        }
        #endregion
    }
}

