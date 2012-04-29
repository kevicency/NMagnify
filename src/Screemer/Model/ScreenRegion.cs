using System;
using System.Windows;

namespace Screemer.Model
{
    public class ScreenRegion
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }

        public int Width
        {
            get { return Right - Left; }
        }

        public int Height
        {
            get { return Bottom - Top; }
        }

        public ScreenRegion()
        {
        }

        public ScreenRegion(int x1, int y1, int x2, int y2)
        {
            Left = Math.Min(x1, x2);
            Top = Math.Min(y1, y2);
            Right = Math.Max(x1, x2);
            Bottom = Math.Max(y1, y2);
        }

        public ScreenRegion(Point p1, Point p2)
            : this((int) p1.X, (int) p1.Y, (int) p2.X, (int) p2.Y)
        { }

        public ScreenRegion Clone()
        {
            return new ScreenRegion(
                    Left, Top, Right, Bottom
                );
        }
    }
}