using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using PluginInterface;
using SerialDeviceDriver;

namespace DisplayApp
{
    public partial class MainForm : Form
    {
        #region Constructor

        public MainForm()
        {
            InitializeComponent();

            AppManager.Instance.OnDisplayUpdateComplete += Display_UpdateComplete;
            AppManager.Instance.OnDisplayUpdateStart += Display_UpdateStart;

            foreach (IPlugin plugin in AppManager.Instance.Plugins)
            {
                Button btn = new Button();

                btn.AutoSize = true;
                btn.Text = plugin.PluginName;
                btn.Tag = plugin;
                btn.Click += btnPlugin_Click;
                flpPlugins.Controls.Add(btn);
            }
        }

        private void btnPlugin_Click(object sender, EventArgs e)
        {
            this.gbPlugin.Controls.Clear();
            AppManager.Instance.StopPluginStateMachine();

            Button btn = sender as Button;
            IPlugin plugin = btn.Tag as IPlugin;

            AppManager.Instance.StartPluginStateMachine(plugin);
            var form = plugin.GetFormInstance();
            form.FormClosed += PluginForm_Closed;
            form.TopLevel = false;
            form.AutoScroll = true;
            form.FormBorderStyle = FormBorderStyle.None;
            this.gbPlugin.Controls.Add(form);
            form.Show();

            //form.ShowDialog();

        }

        private void PluginForm_Closed(object sender, FormClosedEventArgs e)
        {
            this.gbPlugin.Controls.Clear();
            AppManager.Instance.StopPluginStateMachine();
        }


        #endregion

        #region Event Handlers

        private void Display_UpdateStart(object sender, EventArgs e)
        {
            try
            {
                List<Bitmap> bitmaps = sender as List<Bitmap>;

                if (bitmaps.Count < 2)
                    return;

                pbPreview.Image = new Bitmap(bitmaps.ElementAt(0));
                pbTileView.Image = new Bitmap(bitmaps.ElementAt(1));
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
        }

        private void Display_UpdateComplete(object sender, EventArgs e)
        {
            // do nothing
        }


        #endregion

        #region Controls

        private void btnConnect_Click(object sender, EventArgs e)
        {
            bool success = AppManager.Instance.ConnectToDisplay();  // warning: this function blocks
            if (!success)
            {
                MessageBox.Show("Failed");
                return;
            }
            btnConnect.Enabled = false;
            gbMain.Visible = true;
        }

#endregion

        #region Diagnostics

        int rotation = 0;
        private void btnSetRotation_Click(object sender, EventArgs e)
        {
            AppManager.Instance.Device.SetRotation((DisplayRotation)rotation);
            rotation++;
            if (rotation >= 4)
                rotation = 0;
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            string response = AppManager.Instance.Device.ReadLine();
            MessageBox.Show(response);
        }

        private void btnIDN_Click(object sender, EventArgs e)
        {
            string response = AppManager.Instance.Device.SendRequestCommand("*IDN?");

            MessageBox.Show(response);
        }

        private void btnGetBuff_Click(object sender, EventArgs e)
        {
            string response = AppManager.Instance.Device.SendRequestCommand("BUFFER? 0 100");
            MessageBox.Show(response);
        }

        private void btnUSBSpeedTest_Click(object sender, EventArgs e)
        {
            int bytes = 240 * 240 * 2;
            string result = AppManager.Instance.Device.USBSpeedTest(bytes);
            float.TryParse(result, out float bps);
            MessageBox.Show(result + " bytes/s\n" + bps / (float)bytes + " fps");
        }

        private void btnDisplaySpeedTest_Click(object sender, EventArgs e)
        {
            string result = AppManager.Instance.Device.DisplaySpeedTest();
            MessageBox.Show(result + " fps");
        }

        private void btnSpeedTest_Click(object sender, EventArgs e)
        {
            string result = AppManager.Instance.Device.SingleFrameSpeedTest();
            MessageBox.Show(result + " fps");
        }

#endregion
    }
}
