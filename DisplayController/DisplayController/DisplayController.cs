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


        public void WriteImage(Bitmap bitmap)
        {
            List<Tile> tiles = DisplayController.ImageToTiles(bitmap);
            foreach (Tile tile in tiles)
            {
                this.UpdateTile(tile);
            }
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


    }

    public struct TileOffset
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
