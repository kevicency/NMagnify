using System;
using System.Drawing;

namespace Screemer.Model
{
    public interface IScreenCapturer
    {
        int CapturesPerSecond { get; set; }
        Rectangle ScreenRegion { get; set; }

        bool IsRunning { get; }
        event EventHandler<ScreenCapturedEventArgs> ScreenCaptured;

        void Start();
        void Stop();
    }
}