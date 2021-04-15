using PluginInterface;
using SerialDeviceDriver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScreenMirrorPlugin
{
    public partial class MainForm : Form, IPlugin
    {
        private Bitmap Frame;
        private bool ImageChanged;

        public string PluginName { get { return "Screen Mirror"; } }

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
            this.GrabScreen();
            return ImageChanged;
        }

        public Bitmap NextFrame()
        {
            this.ImageChanged = false;
            return this.Frame;
        }

        private void GrabScreen()
        {

            //Creating a new Bitmap object

            Bitmap captureBitmap = new Bitmap(240, 240, PixelFormat.Format32bppArgb);

            //Bitmap captureBitmap = new Bitmap(int width, int height, PixelFormat);  

            //Creating a Rectangle object which will  

            //capture our Current Screen

            //Rectangle captureRectangle = Screen.AllScreens[0].Bounds;
            Rectangle captureRectangle = new Rectangle(MousePosition.X - 120, MousePosition.Y - 120, 240, 240);

            //Creating a New Graphics Object

            Graphics captureGraphics = Graphics.FromImage(captureBitmap);

            //Copying Image from The Screen

            captureGraphics.CopyFromScreen(captureRectangle.Left, captureRectangle.Top, 0, 0, captureRectangle.Size);

            this.Frame = captureBitmap;
            this.ImageChanged = true;


            //Saving the Image File (I am here Saving it in My E drive).

            /*

            captureBitmap.Save(@"E:\Capture.jpg", ImageFormat.Jpeg);

            //Displaying the Successfull Result 

            MessageBox.Show("Screen Captured");
            */
        }
    }
}
