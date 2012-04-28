using System;
using System.Windows.Media;

namespace Screemer.Model
{
    public class ScreenCapturedEventArgs : EventArgs
    {
        public ScreenCapturedEventArgs(ImageSource capture)
        {
            CapturedImage = capture;
        }

        public ImageSource CapturedImage { get; private set; }
    }
}