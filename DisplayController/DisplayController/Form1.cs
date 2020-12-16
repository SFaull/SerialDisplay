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

namespace DisplayController
{
    public partial class Form1 : Form
    {
        DisplayManager displayManager;
        DisplayController device;
        System.Timers.Timer displayRefresh = new System.Timers.Timer();

        public Form1()
        {
            InitializeComponent();
        }


        private void btnConnect_Click(object sender, EventArgs e)
        {
            device = new DisplayController();
            bool success = device.Connect("COM20");
            if (!success)
            {
                MessageBox.Show("Failed");
                return;
            }
            displayManager = new DisplayManager(device);
        }
        private void btnRead_Click(object sender, EventArgs e)
        {
            string response = device.ReadLine();
            MessageBox.Show(response);
        }

        private void btnIDN_Click(object sender, EventArgs e)
        {
            string response = device.SendRequestCommand("*IDN?");

            MessageBox.Show(response);
        }

        private void btnGetBuff_Click(object sender, EventArgs e)
        {
            string response = device.SendRequestCommand("BUFFER? 0 100");
            MessageBox.Show(response);
        }

        private void btnLoadImage_Click(object sender, EventArgs e)
        {

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "PNG files (*.png)|*.png";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    string filePath = openFileDialog.FileName;
                    if(File.Exists(filePath))
                    {
                        // location of you image
                        Bitmap img = new Bitmap(filePath);
                        displayManager.SetMode(DisplayMode.StaticImage);
                        displayManager.SetImage(img);

                    }
                }
            }
        }
        

        private void cbTimer_CheckedChanged(object sender, EventArgs e)
        {
            if(cbTimer.Checked)
            {
                displayManager.SetMode(DisplayMode.ScreenStream);
                displayRefresh.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                displayRefresh.Interval = 1000;
                displayRefresh.Enabled = true;
            }
            else
            {
                displayRefresh.Enabled = false;
                displayRefresh.Elapsed -= OnTimedEvent;
            }
        }


        // Specify what you want to happen when the Elapsed event is raised.
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            displayManager.SetMousePosition(MousePosition.X, MousePosition.Y);
        }

        private void btnIconMode_Click(object sender, EventArgs e)
        {
            displayManager.SetMode(DisplayMode.Icon);
        }
    }
}
