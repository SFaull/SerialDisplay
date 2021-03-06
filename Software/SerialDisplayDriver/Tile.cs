﻿
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace SerialDeviceDriver
{
    public struct TileOffset
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class Tile
    {
        public TileOffset Offset;
        public int Width;
        public int Height;
        public byte[] PixelData;
        public Bitmap Image;

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

        public static Bitmap GenerateBitmapFromTiles(List<Tile> tiles)
        {
            Bitmap frame = new Bitmap(240, 240);    // todo don't hardcode this

            using (Graphics g = Graphics.FromImage(frame)) { g.Clear(Color.White); }

            foreach (Tile tile in tiles)
            {
                using (Graphics g = Graphics.FromImage(frame))
                {
                    g.DrawImage(tile.Image, new Rectangle(tile.Offset.X, tile.Offset.Y, tile.Image.Width, tile.Image.Height));
                }
            }

            return new Bitmap(frame);
        }
    }
}
