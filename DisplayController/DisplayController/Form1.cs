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
            displayManager.OnRefreshComplete += display_RefreshComplete;
            displayManager.OnRefreshStart += display_RefreshStart;
            device.OnStartTransferTiles += display_RefreshTileView;
            
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
            displayManager.SetMousePosition(MousePosition.X, MousePosition.Y);
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
            }
        }

        private void btnIconMode_Click(object sender, EventArgs e)
        {
            displayManager.SetMode(DisplayMode.Icon);
        }

        int rotation = 0;
        private void btnSetRotation_Click(object sender, EventArgs e)
        {
            device.SetRotation((DisplayRotation)rotation);
            rotation++;
            if (rotation >= 4)
                rotation = 0;
        }
    }
}
