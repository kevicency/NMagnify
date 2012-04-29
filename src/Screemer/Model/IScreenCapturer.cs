using System;

namespace Screemer.Model
{
    public interface IScreenCapturer
    {
        int CapturesPerSecond { get; set; }
        ScreenRegion ScreenRegion { get; set; }

        bool IsRunning { get; }
        event EventHandler<ScreenCapturedEventArgs> ScreenCaptured;

        void Start();
        void Stop();
    }
}