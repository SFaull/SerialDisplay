using PluginInterface;
using SerialDeviceDriver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FocusedAppIconPlugin
{
    public partial class MainForm : Form, IPlugin
    {
        private Bitmap Frame;
        private bool ImageChanged;

        public string PluginName { get { return "Focused App Icon"; } }

        public MainForm()
        {
            InitializeComponent();
        }

        public Form GetFormInstance()
        {
            return this;
        }

        public bool Setup()
        {
            this.ImageChanged = false;
            this.Frame = new Bitmap(SerialDisplay.DisplayWidth, SerialDisplay.DisplayHeight);
            return true;
        }

        public bool MainLoop()
        {
            // nothing to be done in main loop.
            // if we return false, it will not be called again
            return false;
        }

        public bool IsFrameReady()
        {
            this.GetIcon();
            return ImageChanged;
        }

        public Bitmap NextFrame()
        {
            this.ImageChanged = false;
            return this.Frame;
        }

        private System.Diagnostics.Process lastProc;

        private void GetIcon()
        {
            System.Diagnostics.Process proc = ProcessFinder.GetForegroundProcess();

            if ((lastProc == null && proc != null) || (lastProc != null && lastProc.ProcessName != proc.ProcessName))
            {
                Console.WriteLine(proc.ProcessName);

                Icon icon = null;

                try
                {
                    icon = Icon.ExtractAssociatedIcon(proc.MainModule.FileName);
                }
                catch
                {
                    icon = null;
                }

                try
                {
                    icon = Icon.ExtractAssociatedIcon(GetMainModuleFilepath(proc.Id));
                }
                catch
                {
                    icon = null;
                }

                if (icon != null)
                {
                    Bitmap _image = Bitmap.FromHicon(new Icon(icon, new Size(240, 240)).Handle);
                    Bitmap resized = _image.Fit(SerialDisplay.DisplayWidth, SerialDisplay.DisplayHeight);

                    this.Frame = resized;
                    this.ImageChanged = true;
                }



                lastProc = proc;
            }
        }


        private string GetMainModuleFilepath(int processId)
        {
            string wmiQueryString = "SELECT ProcessId, ExecutablePath FROM Win32_Process WHERE ProcessId = " + processId;
            using (var searcher = new ManagementObjectSearcher(wmiQueryString))
            {
                using (var results = searcher.Get())
                {
                    ManagementObject mo = results.Cast<ManagementObject>().FirstOrDefault();
                    if (mo != null)
                    {
                        return (string)mo["ExecutablePath"];
                    }
                }
            }
            return null;
        }
    }
}
