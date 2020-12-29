using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DisplayApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Handle the ApplicationExit event to know when the application is exiting.
            Application.ApplicationExit += new EventHandler(OnApplicationExit);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppManager.Instance.Initialise();
            Application.Run(new MainForm());
        }

        /// <summary>
        /// Event triggered when application exits
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnApplicationExit(object sender, EventArgs e)
        {
            // perform any application tidyup before close here:

            try
            {
                AppManager.Instance.DeInitialise();
            }
            catch
            {
                Console.WriteLine("Failed to deitinitalise");
            }
        }
    }
}
