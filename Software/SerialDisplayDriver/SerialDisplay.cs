using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public static int DisplayHeight { get; private set; } = 240;
        public static int DisplayWidth{ get; private set; } = 240;

        public event EventHandler OnFrameTransferStart;
        public event EventHandler OnFrameTransferComplete;
        public event EventHandler OnTileTranferStart;
        public event EventHandler OnTileTransferComplete;

        private TiledFrame LastFrame;
        private TiledFrame CurrentFrame;


        public SerialDisplay()
        {

        }

        public void WriteImage(Bitmap bitmap, bool useTiles = true)
        {
            this.LastFrame = this.CurrentFrame;

            // convert the image into a TiledFrame object
            this.CurrentFrame = new TiledFrame(DisplayWidth, DisplayHeight, MaxTileWidth, MaxTileHeight);
            this.CurrentFrame.LoadImage(bitmap);

            // get a list of the tiles that need to be writen to the display
            List<Tile> tiles;
            if (this.LastFrame == null)
                tiles = this.CurrentFrame.Tiles;
            else
                this.CurrentFrame.GetTiledFrameDelta(this.LastFrame, out tiles);  // todo check successful

            // fire a transfer started event
            List<Bitmap> bitmaps = new List<Bitmap>();
            bitmaps.Add(this.CurrentFrame.Image);
            bitmaps.Add(Tile.GenerateBitmapFromTiles(tiles));

            OnFrameTransferStart?.Invoke(bitmaps, EventArgs.Empty);

            var timer = new Stopwatch();
            timer.Start();
            long len = 0;

            if (useTiles)
            {
                // actually write each of the tiles to the display
                foreach (Tile tile in tiles)
                {
                    OnTileTranferStart?.Invoke(tile, EventArgs.Empty);
                    this.UpdateTile(tile);
                    len += tile.PixelData.Length;
                    OnTileTransferComplete?.Invoke(tile, EventArgs.Empty);
                }
            }
            else
            {
                len += CurrentFrame.PixelData.Length;
                this.UpdateFrame(this.CurrentFrame);
            }

            timer.Stop();

            long millis = timer.ElapsedMilliseconds;
            double mult = 1000.0 / (double)millis;
            double rate = (double)len * mult;


            Console.WriteLine("{0} bytes/s", (int)rate);

            OnFrameTransferComplete?.Invoke(bitmaps, EventArgs.Empty);
        }

        public void WriteImage(string path)
        {
            Bitmap bitmap = new Bitmap(path);
            this.WriteImage(bitmap);
        }

        public string SpeedTest(int bytes)
        {
            string result = string.Empty;

            byte[] data = new byte[bytes];
            string cmd = string.Format("TEST {0}", bytes);
            this.SendExecuteCommand(cmd);
            this.SendBytes(data);
            string timeInMsStr = this.ReadLine();
            if(int.TryParse(timeInMsStr, out int timeInMs))
            {
                double mult = 1000.0 / (double)timeInMs;
                double bytesPerSecond = bytes * mult;
                result = ((int)bytesPerSecond).ToString();
            }

            return result;
        }


        private void UpdateFrame(TiledFrame frame)
        {
            this.SendExecuteCommand("FRAME");
            this.SendBytes(frame.PixelData);
        }

        public void UpdateTile(Tile tile)
        {
            // TODO add error checking

            string cmd = string.Format("TILE {0} {1} {2} {3}", tile.Offset.X, tile.Offset.Y, tile.Width, tile.Height);
            
            this.SendExecuteCommand(cmd);
            this.SendBytes(tile.PixelData);
        }



        public void UpdateTile(int x, int y, int w, int h, byte[] array)
        {
            // TODO add error checking

            string cmd = string.Format("TILE {0} {1} {2} {3}", x, y, w, h);
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
