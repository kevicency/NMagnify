using System;
using System.Windows.Media;

namespace NMagnify.Model
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