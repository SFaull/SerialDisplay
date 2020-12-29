using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SerialDeviceDriver;

namespace DisplayApp
{
    public sealed class AppManager
    {
        #region Singleton Implementation
        private static readonly Lazy<AppManager>
            lazy =
            new Lazy<AppManager>
                (() => new AppManager());

        public static AppManager Instance { get { return lazy.Value; } }
        #endregion

        #region Public Properties

        /// <summary>
        /// Get the assembly version number
        /// </summary>
        public static string Version
        {
            get
            {
                return "v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
            }
        }

        #endregion

        public SerialDisplay Device;
        public DisplayManager Display;

        public event EventHandler OnDisplayUpdateStart;
        public event EventHandler OnDisplayUpdateComplete;

        public AppManager()
        {

        }

        public void Initialise()
        {
            // TODO initialisation here
        }

        public void DeInitialise()
        {
            // TODO deinitialisation here
        }

        public bool ConnectToDisplay()
        {
            bool found = false;
            Device = new SerialDisplay();

            var comPorts = SerialDisplay.GetComPorts();

            foreach(var comPort in comPorts)
            {
                Console.WriteLine("Attempting connection to {0}", comPort);
                try
                {
                    bool success = Device.Connect(comPort);

                    if (success)
                    {
                        if (Device.IsCorrectDevice())
                        {
                            found = true;
                            break;
                        }
                        else
                        {
                            Device.Disconnect();
                            Console.WriteLine("Not a valid device");
                        }
                    }
                    else
                    {
                        Device.Disconnect();
                        Console.WriteLine("Connection failed");
                    }
                }
                catch { Console.WriteLine("Exception thrown"); }
            }
            
            if (found)
            {
                this.Display = new DisplayManager(Device);
                this.Device.OnFrameTranferStart += Display_OnFrameTransferStart;
                this.Device.OnFrameTransferComplete += Display_OnFrameTransferComplete;
            }

            return found;
        }



        private void Display_OnFrameTransferStart(object sender, EventArgs e)
        {
            OnDisplayUpdateStart?.BeginInvoke(sender, EventArgs.Empty, null, null);
        }

        private void Display_OnFrameTransferComplete(object sender, EventArgs e)
        {
            OnDisplayUpdateComplete?.BeginInvoke(sender, EventArgs.Empty, null, null);
        }
    }
}
