using System;

namespace Screemer.Model
{
    public interface IScreenCapturer
    {
        Func<ScreenRegion> CaptureRegionResolver { get; set; }

        bool IsRunning { get; }
        Func<int> CapturesPerSecondResolver { get; set; }
        event EventHandler<ScreenCapturedEventArgs> ScreenCaptured;

        void Start();
        void Stop();
    }
}