using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PluginInterface;
using SerialDeviceDriver;

namespace DisplayApp
{

    public class DisplayManager
    {
        private bool PluginChanged;
        private IPlugin Plugin;

        private SerialDisplay Display;
        private Bitmap Frame;

        public event EventHandler OnRefreshComplete;
        public event EventHandler OnRefreshStart;

        public DisplayManager(SerialDisplay display)
        {
            this.Display = display;
        }


        public void StartNewPlugin(IPlugin plugin)
        {
            this.Plugin = plugin;
            this.PluginChanged = true;

            plugin.Setup();

            Task tState = Task.Run(() => { StateMachine(); })
                .ContinueWith((i) => { Console.WriteLine("State Machine exited"); }, TaskScheduler.FromCurrentSynchronizationContext());

            Task tMain = Task.Run(() => { MainLoop(); })
                .ContinueWith((i) => { Console.WriteLine("Plugin Mainloop exited"); }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void StopPlugin()
        {
            this.PluginChanged = true;
        }

        private void StateMachine()
        {
            this.PluginChanged = false;

            while (!this.PluginChanged)
            {
                if (Plugin.IsFrameReady())
                {
                    this.Frame = Plugin.NextFrame();
                    OnRefreshStart?.Invoke(this.Frame, EventArgs.Empty);
                    this.Display.WriteImage(this.Frame);
                    OnRefreshComplete?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    System.Threading.Thread.Sleep(10);
                }
            }
        }

        private void MainLoop()
        {
            bool active = true;
            while(active && !this.PluginChanged)
            {
                active = Plugin.MainLoop();
            }
        }
    }
}
