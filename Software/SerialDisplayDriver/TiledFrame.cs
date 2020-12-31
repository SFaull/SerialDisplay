using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialDeviceDriver
{
    public class TiledFrame
    {
        public int Height { get; private set; }
        public int Width { get; private set; }

        public int TileHeight { get; private set; }

        public int TileWidth { get; private set; }

        public Bitmap Image { get; private set; }

        public List<Tile> Tiles { get; private set; }

        public TiledFrame(int width, int height, int tileWidth, int tileHeight)
        {
            this.Width = width;
            this.Height = height;
            this.TileWidth = tileWidth;
            this.TileHeight = tileHeight;

            this.Tiles = new List<Tile>();
        }

        public void LoadImage(string path)
        {
            Bitmap bitmap = new Bitmap(path);
            this.LoadImage(bitmap);
        }

        public void LoadImage(Bitmap image)
        {
            this.Image = image;
            this.Tiles = ImageToTiles(image);
        }

        public bool GetTiledFrameDelta(TiledFrame oldFrame, out List<Tile> tiles)
        {
            TiledFrame currentFrame = this; // this current instance is the new full frame
            tiles = null;

            // first throw a hissy fit if the tile sizes don't match
            if ((oldFrame.TileWidth != currentFrame.TileWidth) || (oldFrame.TileHeight != currentFrame.TileHeight) || (oldFrame.Tiles.Count != currentFrame.Tiles.Count))
                return false;

            // so we need to compare the two frames and calculate the delta
            tiles = new List<Tile>();

            for (int i = 0; i < oldFrame.Tiles.Count; i++)
            {
                Tile oldTile = oldFrame.Tiles.ElementAt(i);
                Tile currentTile = currentFrame.Tiles.ElementAt(i);

                if (!oldTile.IsEqualTo(currentTile))
                    tiles.Add(currentTile);
            }

            return true;
        }

        /// <summary>
        /// Convert a 4byte colour into a 2 byte R5G6B5 colour (supported by ST7789 display)
        /// </summary>
        /// <param name="clr"></param>
        /// <returns></returns>
        private UInt16 ColorToR5G6B5(Color clr)
        {
            UInt16 pixel = 0;

            // convert the colour to r5g6b5
            pixel |= (UInt16)((UInt16)(clr.B & 0b11111000) << 8);
            pixel |= (UInt16)((UInt16)(clr.G & 0b11111100) << 3);
            pixel |= (UInt16)((UInt16)(clr.R & 0b11111000) >> 3);

            return pixel;
        }

        private List<Tile> ImageToTiles(Bitmap bitmap)
        {
            List<Tile> tiles = new List<Tile>();

            int xIterations = bitmap.Width / TileWidth;
            int yIterations = bitmap.Height / TileHeight;

            int xRemaining = bitmap.Width % TileWidth;
            int yRemaining = bitmap.Height % TileHeight;

            if (xRemaining > 0)
                xIterations++;

            if (yRemaining > 0)
                yIterations++;


            int xOffset = 0;
            int yOffset = 0;

            int width = TileWidth;
            int height = TileHeight;

            for (int i = 0; i < yIterations; i++)
            {
                if (i == (yIterations - 1) && yRemaining > 0)
                    height = yRemaining;
                else
                    height = TileHeight;


                for (int j = 0; j < xIterations; j++)
                {
                    if (j == (xIterations - 1) && xRemaining > 0)
                        width = xRemaining;
                    else
                        width = TileWidth;


                    UInt16[] pixArray = new UInt16[width * height];
                    int index = 0;
                    int xFinish = xOffset + width;
                    int yFinish = yOffset + height;

                    for (int y = yOffset; y < yFinish; y++)
                    {
                        for (int x = xOffset; x < xFinish; x++)
                        {
                            Color clr = bitmap.GetPixel(x, y);
                            pixArray[index] = ColorToR5G6B5(clr);
                            index++;
                        }
                    }

                    // copy into byte array
                    byte[] result = new byte[pixArray.Length * sizeof(UInt16)];
                    Buffer.BlockCopy(pixArray, 0, result, 0, result.Length);

                    // save the actual tile bitmap
                    Rectangle cropRect = new Rectangle(xOffset, yOffset, width, height);
                    Bitmap target = new Bitmap(cropRect.Width, cropRect.Height);
                    using (Graphics g = Graphics.FromImage(target))
                    {
                        g.DrawImage(bitmap, new Rectangle(0, 0, target.Width, target.Height),
                                         cropRect,
                                         GraphicsUnit.Pixel);
                    }

                    // put all of the data into a tile object
                    Tile tile = new Tile(xOffset, yOffset, width, height);
                    tile.PixelData = result;
                    tile.Image = target;
                    //tile.Image.Save("images/" + tile.Offset.X.ToString() + "-" + tile.Offset.Y.ToString() + ".png", ImageFormat.Png);

                    // add it to the list
                    tiles.Add(tile);

                    xOffset += TileWidth;
                }
                yOffset += TileHeight;
                xOffset = 0;
            }

            return tiles;
        }
    }
}
