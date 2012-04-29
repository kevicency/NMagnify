using System;

namespace Screemer.Model
{
    public interface IScreenCapturer
    {
        int CapturesPerSecond { get; set; }
        Func<ScreenRegion> CaptureRegionResolver { get; set; }

        bool IsRunning { get; }
        event EventHandler<ScreenCapturedEventArgs> ScreenCaptured;

        void Start();
        void Stop();
    }
}