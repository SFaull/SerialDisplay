using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayController
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

        public DisplayController Device;
        public DisplayManager Display;

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
            Device = new DisplayController();

            var comPorts = DisplayController.GetComPorts();

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
                Display = new DisplayManager(Device);
            }

            return found;
        }

        #region Event Bubbling
#if false
        public event EventHandler OnDisplayRefreshStarting
        {
            add { this.Display.OnRefreshStart += value; }
            remove { this.Display.OnRefreshStart -= value; }
        }

        public event EventHandler OnDisplayRefreshComplete
        {
            add { this.Display.OnRefreshComplete += value; }
            remove { this.Display.OnRefreshComplete -= value; }
        }

        public event EventHandler OnDisplayRefreshingTiles
        {
            add { this.Device.OnStartTransferTiles += value; }
            remove { this.Device.OnStartTransferTiles -= value; }
        }
#endif
#endregion
    }
}
