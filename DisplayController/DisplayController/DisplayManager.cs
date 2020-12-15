using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DisplayController
{
    enum DisplayMode
    {
        None,
        StaticImage,
        ScreenStream,
        Icon,
    }

    class DisplayManager
    {
        private DisplayController Display;
        private Bitmap Frame;
        private DisplayMode Mode;
        private bool FrameReady;
        private int MouseX;
        private int MouseY;

        public DisplayManager(DisplayController display)
        {
            this.Display = display;
            this.FrameReady = false;
            this.Start();
        }

        public void SetMode(DisplayMode mode)
        {
            this.Mode = mode;
        }

        public void SetMousePosition(int x, int y)
        {
            this.MouseX = x;
            this.MouseY = y;
        }

        public void SetImage(Bitmap image)
        {
            this.Frame = image;
            this.FrameReady = true;
        }


        private void Start()
        {
            Task t = Task.Run(() => { StateMachine(); })
                .ContinueWith((i) => { Console.WriteLine("State Machine exited"); }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void StateMachine()
        {
            while (true)
            {
                if (this.FrameReady)
                {
                    this.FrameReady = false; // reset the flag
                    this.Display.WriteImage(this.Frame);
                }
                else
                {
                    System.Threading.Thread.Sleep(10);

                    switch (this.Mode)
                    {
                        case DisplayMode.StaticImage:
                            break;
                        case DisplayMode.None:
                            break;
                        case DisplayMode.Icon:
                            this.GetIcon();
                            break;
                        case DisplayMode.ScreenStream:
                            this.GrabScreen();
                            break;
                    }
                }
            }
        }

        private void GrabScreen()
        {

            //Creating a new Bitmap object

            Bitmap captureBitmap = new Bitmap(240, 240, PixelFormat.Format32bppArgb);

            //Bitmap captureBitmap = new Bitmap(int width, int height, PixelFormat);  

            //Creating a Rectangle object which will  

            //capture our Current Screen

            //Rectangle captureRectangle = Screen.AllScreens[0].Bounds;
            Rectangle captureRectangle = new Rectangle(MouseX - 120, MouseY - 120, 240, 240);

            //Creating a New Graphics Object

            Graphics captureGraphics = Graphics.FromImage(captureBitmap);

            //Copying Image from The Screen

            captureGraphics.CopyFromScreen(captureRectangle.Left, captureRectangle.Top, 0, 0, captureRectangle.Size);

            this.SetImage(captureBitmap);


            //Saving the Image File (I am here Saving it in My E drive).

            /*

            captureBitmap.Save(@"E:\Capture.jpg", ImageFormat.Jpeg);

            //Displaying the Successfull Result 

            MessageBox.Show("Screen Captured");
            */
        }

        private System.Diagnostics.Process lastProc;

        private void GetIcon()
        {  
            System.Diagnostics.Process proc = ProcessFinder.GetForegroundProcess();

            if ((lastProc == null && proc != null )|| (lastProc != null && lastProc.ProcessName != proc.ProcessName))
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
                    Bitmap resized = new Bitmap(_image, new Size(240, 240));
                    //resized.Save("myfile.png", ImageFormat.Png);
                    this.SetImage(resized);
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

    static class ProcessFinder
    {
        // https://stackoverflow.com/questions/97283/how-can-i-determine-the-name-of-the-currently-focused-process-in-c-sharp

        // The GetForegroundWindow function returns a handle to the foreground window
        // (the window  with which the user is currently working).
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        // The GetWindowThreadProcessId function retrieves the identifier of the thread
        // that created the specified window and, optionally, the identifier of the
        // process that created the window.
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern Int32 GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        public static System.Diagnostics.Process GetForegroundProcess()
        {
            IntPtr hwnd = GetForegroundWindow();

            // The foreground window can be NULL in certain circumstances, 
            // such as when a window is losing activation.
            if (hwnd == null)
                return null;

            uint pid;
            GetWindowThreadProcessId(hwnd, out pid);

            foreach (System.Diagnostics.Process p in System.Diagnostics.Process.GetProcesses())
            {
                if (p.Id == pid)
                    return p;
            }

            return null;
        }


    }
}
