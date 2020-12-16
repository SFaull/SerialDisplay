using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisplayController
{
    class DisplayController : SerialDevice
    {
        public static int MaxTileWidth { get; private set; } = 60;
        public static int MaxTileHeight { get; private set; } = 60;


        static List<Tile> ImageToTiles(Bitmap bitmap)
        {
            List<Tile> tiles = new List<Tile>();

            int xIterations = bitmap.Width / MaxTileWidth;
            int yIterations = bitmap.Height / MaxTileHeight;

            int xRemaining = bitmap.Width % MaxTileWidth;
            int yRemaining = bitmap.Height % MaxTileHeight;

            if (xRemaining > 0)
                xIterations++;

            if (yRemaining > 0)
                yIterations++;


            int xOffset = 0;
            int yOffset = 0;

            int width = MaxTileWidth;
            int height = MaxTileHeight;

            for (int i = 0; i < yIterations; i++)
            {
                if (i == (yIterations - 1) && yRemaining > 0)
                    height = yRemaining;
                else
                    height = MaxTileHeight;


                for (int j = 0; j < xIterations; j++)
                {
                    if (j == (xIterations - 1) && xRemaining > 0)
                        width = xRemaining;
                    else
                        width = MaxTileWidth;


                    UInt16[] pixArray = new UInt16[width * height];
                    int index = 0;
                    int xFinish = xOffset + width;
                    int yFinish = yOffset + height;

                    for (int y = yOffset; y < yFinish; y++)
                    {
                        for (int x = xOffset; x < xFinish; x++)
                        {
                            Color clr = bitmap.GetPixel(x, y);

                            // convert the colour to r5g6b5
                            pixArray[index] |= (UInt16)((UInt16)(clr.B & 0b11111000) << 8);
                            pixArray[index] |= (UInt16)((UInt16)(clr.G & 0b11111100) << 3);
                            pixArray[index] |= (UInt16)((UInt16)(clr.R & 0b11111000) >> 3);
                            index++;
                        }
                    }

                    for (int a = 0; a < pixArray.Length; a++)
                    {
                        pixArray[a] = pixArray[a].SwitchEndianness();
                    }

                    byte[] result = new byte[pixArray.Length * sizeof(UInt16)];
                    Buffer.BlockCopy(pixArray, 0, result, 0, result.Length);

                    Tile tile = new Tile(xOffset, yOffset, width, height);
                    tile.PixelData = result;

                    tiles.Add(tile);



                    xOffset += MaxTileWidth;
                }
                yOffset += MaxTileHeight;
                xOffset = 0;
            }

            return tiles;
        }

        private List<Tile> TileHistory;
        public void WriteImage(Bitmap bitmap)
        {
            // convert the image into tile
            List<Tile> tiles = DisplayController.ImageToTiles(bitmap);

            List<Tile> tilesToTransfer = new List<Tile>();
            foreach (Tile tile in tiles)
                tilesToTransfer.Add(tile);

            /****** This section is to reduce the number of transfers *****/

            // if tiles have already been sent, compare them to see if they are identical. These should be removed from the list
            if((TileHistory != null) && (tilesToTransfer.Count == TileHistory.Count))
            {
                for(int i = tilesToTransfer.Count - 1; i >= 0; i--)
                {
                    if (tilesToTransfer[i].IsEqualTo(TileHistory[i]))
                        tilesToTransfer.RemoveAt(i);
                }
            }

            /*****************************************************************/

            foreach (Tile tile in tilesToTransfer)
            {
               this.UpdateTile(tile);
            }

            TileHistory = tiles;
        }

        public void WriteImage(string path)
        {
            Bitmap bitmap = new Bitmap(path);
            this.WriteImage(bitmap);
        }


        public void UpdateTile(Tile tile)
        {
            // TODO add error checking

            string cmd = string.Format("DISPLAY {0} {1} {2} {3}", tile.Offset.X, tile.Offset.Y, tile.Width, tile.Height);
            this.SendExecuteCommand(cmd);
            this.SendBytes(tile.PixelData);
        }

        

        public void UpdateTile(int x, int y, int w, int h, byte[] array)
        {
            // TODO add error checking

            string cmd = string.Format("DISPLAY {0} {1} {2} {3}", x, y, w, h);
            this.SendExecuteCommand(cmd);
            this.SendBytes(array);
        }


    }

    public class Tile
    {
        public TileOffset Offset;
        public int Width;
        public int Height;
        public byte[] PixelData;

        public bool IsValid
        {
            get
            {
                return (Offset.X >= 0 && Offset.Y >= 0 && Width >= 0 && Height >= 0);
            }
        }

        // TODO: add reference to source image?

        public Tile()
        {
            Offset.X = -1;
            Offset.Y = -1;
            Width = -1;
            Height = -1;
        }

        public Tile(int x, int y, int w, int h)
        {
            Offset.X = x;
            Offset.Y = y;
            Width = w;
            Height = h;
        }

        public bool IsEqualTo(Tile other)
        {
            return PixelData.SequenceEqual(other.PixelData);
        }


    }

    public struct TileOffset
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public enum ImageLayout
    {
        Stretch,        // strech without maining aspect ratio
        FitAspectRatio, // stretch to max size, but maintain aspect ratio  
        Center  // don't resize, just put in the center
    }

    public static class BitmapExtensions
    {
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
                    if(scaleX > scaleY)
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
