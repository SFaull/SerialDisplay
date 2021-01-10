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

        }
        #endregion

        #region Event Handlers

        private void Display_UpdateStart(object sender, EventArgs e)
        {
            List<Bitmap> bitmaps = sender as List<Bitmap>;

            if (bitmaps.Count < 2)
                return;

            pbPreview.Image = new Bitmap(bitmaps.ElementAt(0));
            pbTileView.Image = new Bitmap(bitmaps.ElementAt(1));
        }

        private void Display_UpdateComplete(object sender, EventArgs e)
        {
            if(AppManager.Instance.Display.Mode == DisplayMode.ScreenStream)
                AppManager.Instance.Display.SetMousePosition(MousePosition.X, MousePosition.Y);
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

        private void btnLoadImage_Click(object sender, EventArgs e)
        {

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png, *.gif) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png; *.gif";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    string filePath = openFileDialog.FileName;
                    if (File.Exists(filePath))
                    {
                        // location of you image
                        Bitmap img = new Bitmap(filePath);
                        AppManager.Instance.Display.SetMode(DisplayMode.StaticImage);
                        AppManager.Instance.Display.SetImage(img);

                    }
                }
            }
        }

        private void btnScreenStream_Click(object sender, EventArgs e)
        {
            AppManager.Instance.Display.SetMousePosition(MousePosition.X, MousePosition.Y);
            AppManager.Instance.Display.SetMode(DisplayMode.ScreenStream);
        }

        private void btnIconMode_Click(object sender, EventArgs e)
        {
            AppManager.Instance.Display.SetMode(DisplayMode.Icon);
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
