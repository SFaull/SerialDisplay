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

        public static int DisplayHeight { get; private set; } = 240;
        public static int DisplayWidth{ get; private set; } = 240;

        public event EventHandler OnStartTransferTiles;

        private TiledFrame LastFrame;
        private TiledFrame CurrentFrame;


        public SerialDisplay()
        {

        }

        public void WriteImage(Bitmap bitmap)
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
            OnStartTransferTiles?.Invoke(tiles, EventArgs.Empty);

            // actually write each of the tiles to the display
            foreach (Tile tile in tiles)
                this.UpdateTile(tile);
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
