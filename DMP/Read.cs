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
using System.IO;
using System.Windows.Forms;

namespace DMP
{
    class Read
    {
        public int nLines { get; private set; }
        public int nColumns { get; private set; }
        public string[] header { get; private set; }
        public float[,] data { get; private set; }
        public string fileName { get; set; }
        public string[] section { get; private set; }

        public Read(string file)
        {
            string[] pieces;

            fileName = Path.GetFileName(file);
            string[] lines = File.ReadAllLines(file); // read all lines
            if (lines == null || lines.Length < 2) return; //no data in file
            header = lines[0].Split(','); //first line is header
            nLines = lines.Length - 1; //first line is header
            nColumns = header.Length;

            //read the numerical data and section name from the file
            data = new float[nLines, nColumns - 1]; // *** 1 less than nColumns as last col is sectionName
            section = new string[nLines]; // *** 
            for (int i = 0; i < nLines; i++)
            {
                pieces = lines[i + 1].Split(','); // first line is header
                if (pieces.Length != nColumns) { CustomMsgBox.Show("Invalid data at line " + (i + 2) + " of file " + fileName, "Invalid data","OK"); return; }
                for (int j = 0; j < nColumns - 1; j++)
                {
                    float.TryParse(pieces[j], out data[i, j]); //data[i, j] = float.Parse(pieces[j]);
                }
                section[i] = pieces[nColumns - 1]; //last item is section
            }
        }

    }
}


