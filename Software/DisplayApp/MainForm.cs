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

#if false
            AppManager.Instance.OnDisplayRefreshComplete += display_RefreshComplete;
            AppManager.Instance.OnDisplayRefreshStarting += display_RefreshStart;
            AppManager.Instance.OnDisplayRefreshingTiles += display_RefreshTileView;
#endif
        }


        private void btnConnect_Click(object sender, EventArgs e)
        {
            bool success = AppManager.Instance.ConnectToDisplay();
            if (!success)
            {
                MessageBox.Show("Failed");
                return;
            }
        }

        private void display_RefreshTileView(object sender, EventArgs e)
        {
            List<Tile> tiles = sender as List<Tile>;

            Bitmap frame = new Bitmap(240, 240);
            using (Graphics g = Graphics.FromImage(frame)) { g.Clear(Color.White); }

            foreach (Tile tile in tiles)
            {
                using (Graphics g = Graphics.FromImage(frame))
                {
                    g.DrawImage(tile.Image, new Rectangle(tile.Offset.X, tile.Offset.Y, tile.Image.Width, tile.Image.Height));
                }

#if false


                // save the actual tile bitmpa
                Rectangle cropRect = new Rectangle(tile.Offset.X, tile.Offset.Y, tile.Width, tile.Height);
                //Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);
                using (Graphics g = Graphics.FromImage(frame))
                {
                    g.DrawImage(tile.Image, new Rectangle(0, 0, tile.Image.Width, tile.Image.Height),
                                     cropRect,
                                     GraphicsUnit.Pixel);
                }
#endif
            }

            //frame.Save("delta.png", ImageFormat.Png);

            pbTileView.Image = new Bitmap(frame);
        }

        private void display_RefreshStart(object sender, EventArgs e)
        {
            Bitmap frame = sender as Bitmap;

            pbPreview.Image = new Bitmap(frame);
        }

        private void display_RefreshComplete(object sender, EventArgs e)
        {
            AppManager.Instance.Display.SetMousePosition(MousePosition.X, MousePosition.Y);
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
                openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
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
