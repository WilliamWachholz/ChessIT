﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessIT.KuricaUtils
{
    static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Controller.MainController mainController = new Controller.MainController();

            System.Windows.Forms.Application.Run();
        }
    }
}
