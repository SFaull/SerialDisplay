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
        System.Timers.Timer displayRefresh = new System.Timers.Timer();

        public MainForm()
        {
            InitializeComponent();


            AppManager.Instance.OnDisplayUpdateComplete += Display_UpdateComplete;
            AppManager.Instance.OnDisplayUpdateStart += Display_UpdateStart;

        }


        private void btnConnect_Click(object sender, EventArgs e)
        {
            bool success = AppManager.Instance.ConnectToDisplay();
            if (!success)
            {
                MessageBox.Show("Failed");
                return;
            }
            btnConnect.Enabled = false;
        }

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
            //AppManager.Instance.Display.SetMousePosition(MousePosition.X, MousePosition.Y);
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
                    if(File.Exists(filePath))
                    {
                        // location of you image
                        Bitmap img = new Bitmap(filePath);
                        AppManager.Instance.Display.SetMode(DisplayMode.StaticImage);
                        AppManager.Instance.Display.SetImage(img);

                    }
                }
            }
        }
        

        private void cbTimer_CheckedChanged(object sender, EventArgs e)
        {
            if(cbTimer.Checked)
            {
                AppManager.Instance.Display.SetMode(DisplayMode.ScreenStream);
            }
        }

        private void btnIconMode_Click(object sender, EventArgs e)
        {
            AppManager.Instance.Display.SetMode(DisplayMode.Icon);
        }

        int rotation = 0;
        private void btnSetRotation_Click(object sender, EventArgs e)
        {
            AppManager.Instance.Device.SetRotation((DisplayRotation)rotation);
            rotation++;
            if (rotation >= 4)
                rotation = 0;
        }
    }
}
