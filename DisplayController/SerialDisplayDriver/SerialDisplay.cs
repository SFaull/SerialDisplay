using System;
using System.Collections.Generic;
using System.Drawing;

namespace SerialDeviceDriver
{
    public enum DisplayRotation
    {
        Rotation_0 = 0,
        Rotation_90 = 1,
        Rotation_180 = 2,
        Rotation_270 = 3,
    }

    public class SerialDisplay : SerialDevice
    {
        public static int MaxTileWidth { get; private set; } = 10;
        public static int MaxTileHeight { get; private set; } = 10;

        public event EventHandler OnStartTransferTiles;


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

                    // swap the endianess of entire array
                    for (int a = 0; a < pixArray.Length; a++)
                    {
                        pixArray[a] = pixArray[a].SwitchEndianness();
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
            List<Tile> tiles = SerialDisplay.ImageToTiles(bitmap);

            List<Tile> tilesToTransfer = new List<Tile>();
            foreach (Tile tile in tiles)
                tilesToTransfer.Add(tile);

            /****** This section is to reduce the number of transfers *****/

            // if tiles have already been sent, compare them to see if they are identical. These should be removed from the list
            if ((TileHistory != null) && (tilesToTransfer.Count == TileHistory.Count))
            {
                for (int i = tilesToTransfer.Count - 1; i >= 0; i--)
                {
                    if (tilesToTransfer[i].IsEqualTo(TileHistory[i]))
                        tilesToTransfer.RemoveAt(i);
                }
            }

            /*****************************************************************/

            OnStartTransferTiles?.Invoke(tilesToTransfer, EventArgs.Empty);

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



        public void SetRotation(DisplayRotation rotation)
        {
            string cmd = string.Format("ROTATION {0}", (int)rotation);
            this.SendExecuteCommand(cmd);
        }

        public bool IsCorrectDevice()
        {
            string response = this.SendRequestCommand("*IDN?");

            return (response.Contains("Xiao"));
        }

    }
}
