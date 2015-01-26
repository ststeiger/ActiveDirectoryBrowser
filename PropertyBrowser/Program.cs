
using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace PropertyBrowser
{


    static class Program
    {


        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {

            if (true)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmPropertyBrowser());
            }

        } // End Sub Main


    } // End Class Program 


} // End Namespace PropertyBrowser 
