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
        DisplayController device;
        Bitmap img;
        string imgPath;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            device.WriteImage(img);
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
                        img = new Bitmap(filePath);
                        imgPath = filePath;
                        
                        UpdatePreview();
                    }
                }
            }
        }

        private void UpdatePreview()
        {
            this.Invoke((MethodInvoker)delegate {
                pbPreview.Image = img;
                pbPreview.Update();
            });
            
        }

        private void GrabScreen()
        {

            //Creating a new Bitmap object

            Bitmap captureBitmap = new Bitmap(240, 240, PixelFormat.Format32bppArgb);

            //Bitmap captureBitmap = new Bitmap(int width, int height, PixelFormat);  

            //Creating a Rectangle object which will  

            //capture our Current Screen

            //Rectangle captureRectangle = Screen.AllScreens[0].Bounds;
            Rectangle captureRectangle = new Rectangle(MousePosition.X, MousePosition.Y, 240, 240);

            //Creating a New Graphics Object

            Graphics captureGraphics = Graphics.FromImage(captureBitmap);

            //Copying Image from The Screen

            captureGraphics.CopyFromScreen(captureRectangle.Left, captureRectangle.Top, 0, 0, captureRectangle.Size);

            img = captureBitmap;

            UpdatePreview();

            //Saving the Image File (I am here Saving it in My E drive).

            /*

            captureBitmap.Save(@"E:\Capture.jpg", ImageFormat.Jpeg);

            //Displaying the Successfull Result 

            MessageBox.Show("Screen Captured");
            */
        }

        private void btnScreenCapture_Click(object sender, EventArgs e)
        {
            this.GrabScreen();
            pbPreview.Image = img;
        }

        System.Timers.Timer displayRefresh = new System.Timers.Timer();

        private void cbTimer_CheckedChanged(object sender, EventArgs e)
        {
            if(cbTimer.Checked)
            {
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
            GrabScreen();
            device.WriteImage(img);

        }

        private void btnLoop_Click(object sender, EventArgs e)
        {
            while(true)
            {
                GrabScreen();
                device.WriteImage(img);
            }
        }
    }
}
