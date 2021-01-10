using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PluginInterface
{
    public interface IPlugin
    {
        /// <summary>
        /// Host application will call this when the plugin is selected by the user, the method should return the form instance and the host app will show it
        /// </summary>
        Form GetFormInstance();

        /// <summary>
        /// This will be called after the form has been loaded, you should complete any setup here. Maybe generate your first frame
        /// </summary>
        /// <returns></returns>
        bool Setup();

        /// <summary>
        /// This will be called over and over in its own thread, do any continuous processing here
        /// </summary>
        /// <returns></returns>
        bool MainLoop();

        /// <summary>
        /// This will be called each time the display is ready for a new frame. If true is returned, the application will request the next frame
        /// </summary>
        /// <returns></returns>
        bool IsFrameReady();

        /// <summary>
        /// This will be called wach time the display is ready for a new frame and IsFrameReady has returned true
        /// </summary>
        /// <returns></returns>
        Bitmap NextFrame();

        /// <summary>
        /// This should return the name of your plugin
        /// </summary>
        /// <returns></returns>
        string PluginName { get; }

    }
}
