using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayApp.SubApps
{
    interface ISubApp
    {
        string Name { get; set; }

        /// <summary>
        /// This will be run in its own thread
        /// </summary>
        void MainLoop();

        /// <summary>
        /// This must return a bitmap containing the next frame to display
        /// </summary>
        Bitmap NextFrame();
    }
}
