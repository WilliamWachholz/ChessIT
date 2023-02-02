/*
 *  JBC Framework - OpenSource Development framework for SAP Business One
 *  Copyright (C) 2020 João Claudino
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 *  
 *  Contact me at <joao.claudino@uol.com.br>
 * 
 */
using System;  
using System.Collections.Generic;
using System.Linq;
using JBC.Framework;

namespace JBC
{ 
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Application b1App;

                b1App = new Application();
                b1App.Run();
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(string.Format("Unexpected error on JBC: {0}\n {1}", e.Message, e.StackTrace));
            }
        }

    }
}
