using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using PluginInterface;
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

        public List<IPlugin> Plugins;

        public event EventHandler OnDisplayUpdateStart;
        public event EventHandler OnDisplayUpdateComplete;

        public AppManager()
        {

        }

        public void Initialise()
        {
            // TODO initialisation here
            this.Plugins = this.GetPlugins();
        }

        public void DeInitialise()
        {
            // TODO deinitialisation here
        }


        private List<IPlugin> GetPlugins()
        {
            List<Type> types = new List<Type>();
            List<IPlugin> plugins = new List<IPlugin>();

            string[] assemblyPaths = System.IO.Directory.GetFiles(@"E:\Sam\Git\XiaoDisplay\Software\Plugins", "*.dll", System.IO.SearchOption.AllDirectories);
            foreach (string assemblyPath in assemblyPaths)
            {
                Assembly assembly = null;
                assembly = Assembly.LoadFrom(assemblyPath);

                // Go through all the types in it
                foreach (Type t in assembly.GetExportedTypes())
                {
                    if (typeof(IPlugin).IsAssignableFrom(t))
                    {
                        if (t.FullName != "PluginInterface.IPlugin")
                            types.Add(t);

                        Console.WriteLine(t.FullName);
                    }
                }
            }

            foreach (var type in types)
            {
                Assembly assem = type.Assembly;
                IPlugin plugin = assem.CreateInstance(type.FullName) as IPlugin;
                plugins.Add(plugin);
                //Form form = plugin.GetFormInstance();
                //MessageBox.Show(plugin.PluginName);
                //form.ShowDialog();
            }

            return plugins;
        }

        internal void StopPluginStateMachine()
        {
            Display.StopPlugin();
        }

        public void StartPluginStateMachine(IPlugin plugin)
        {
            Display.StartNewPlugin(plugin);
        }

        public bool ConnectToDisplay()
        {
            bool found = false;
            Device = new SerialDisplay();

            var comPorts = SerialDisplay.GetComPorts().ToList();

            OrderComPortList(comPorts);

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
                            Properties.Settings.Default.LastComPort = comPort;
                            Properties.Settings.Default.Save();
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
                this.Device.OnFrameTransferStart += Display_OnFrameTransferStart;
                this.Device.OnFrameTransferComplete += Display_OnFrameTransferComplete;                
            }

            return found;
        }

        private bool OrderComPortList(List<string> comPortList)
        {
            string lastComPort = Properties.Settings.Default.LastComPort;

            if (string.IsNullOrEmpty(lastComPort))
                return false;

            int index = comPortList.FindIndex(a => a.Equals(lastComPort));

            if (index < 0)
                return false;

            comPortList.MoveItemAtIndexToFront(index);

            return true;
        }

        #region Event Handlers

        private void Display_OnFrameTransferStart(object sender, EventArgs e)
        {
            OnDisplayUpdateStart?.Invoke(sender, EventArgs.Empty);
        }

        private void Display_OnFrameTransferComplete(object sender, EventArgs e)
        {
            OnDisplayUpdateComplete?.Invoke(sender, EventArgs.Empty);
        }

        #endregion
    }
}
