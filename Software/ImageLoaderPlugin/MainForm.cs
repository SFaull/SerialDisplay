using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PluginInterface;
using SerialDeviceDriver;

namespace ImageLoaderPlugin 
{
    public partial class MainForm : Form, IPlugin
    {
        private Bitmap Frame;
        private bool ImageChanged;

        public string PluginName { get { return "Image Loader"; } }

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
            return ImageChanged;
        }

        public Bitmap NextFrame()
        {

            this.ImageChanged = false;
            return this.Frame;
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
                        Bitmap img = new Bitmap(filePath);
                        this.Frame = img.Fit(SerialDisplay.DisplayWidth, SerialDisplay.DisplayHeight);
                        this.ImageChanged = true;
                    }
                }
            }
        }
    }
}
