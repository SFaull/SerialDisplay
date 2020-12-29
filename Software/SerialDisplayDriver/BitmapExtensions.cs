using System;
using System.Drawing;

namespace SerialDeviceDriver
{
    public enum ImageLayout
    {
        Stretch,        // strech without maining aspect ratio
        FitAspectRatio, // stretch to max size, but maintain aspect ratio  
        Center  // don't resize, just put in the center
    }

    public static class BitmapExtensions
    {
        /// <summary>
        /// Resizes the bitmap to a specified width and height, maintains aspect ratio by default
        /// </summary>
        /// <param name="original"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="layout"></param>
        /// <returns></returns>
        public static Bitmap Fit(this Bitmap original, int width, int height, ImageLayout layout = ImageLayout.FitAspectRatio)
        {
            Bitmap modified = null;

            switch (layout)
            {
                case ImageLayout.Center:
                    // TODO
                    break;
                case ImageLayout.FitAspectRatio:
                    double scaleX = (double)original.Width / (double)width;
                    double scaleY = (double)original.Height / (double)height;
                    double scale;
                    if (scaleX > scaleY)
                    {
                        // make the x axis fit
                        scale = scaleX;
                    }
                    else if (scaleY > scaleX)
                    {
                        // make the y axis fit
                        scale = scaleY;
                    }
                    else
                    {
                        // it doesn't matter
                        scale = scaleX;
                    }

                    int x = (int)Math.Floor((double)original.Width / scale);
                    int y = (int)Math.Floor((double)original.Height / scale);
                    modified = new Bitmap(original, new Size(x, y));

                    Bitmap bmp = new Bitmap(width, height);
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        Rectangle ImageSize = new Rectangle(0, 0, width, height);
                        g.FillRectangle(Brushes.Black, ImageSize);
                        g.DrawImage(modified, 0, 0);
                    }

                    modified = bmp;

                    break;
                case ImageLayout.Stretch:
                    modified = new Bitmap(original, new Size(width, height));
                    break;
                default:
                    modified = original;
                    break;
            }

            return modified;
        }
    }
}
