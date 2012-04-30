using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NMagnify.Model
{
    public class BitmapScreenCapturer : IScreenCapturer
    {
        BackgroundWorker _worker;

        public BitmapScreenCapturer()
        {
            BitmapConverter = BitmapUtility.ConvertBitmapToImageSource;
            CaptureRegionResolver = () => new ScreenRegion();
            CapturesPerSecondResolver = () => 25;
        }

        public BitmapToImageSource BitmapConverter { get; set; }

        #region IScreenCapturer Members

        public event EventHandler<ScreenCapturedEventArgs> ScreenCaptured;

        public int CapturesPerSecond { get { return CapturesPerSecondResolver(); } }
        public Func<ScreenRegion> CaptureRegionResolver { get; set; }

        public bool IsRunning { get; private set; }

        public Func<int> CapturesPerSecondResolver { get; set; }

        public void Start()
        {
            IsRunning = true;

            _worker = new BackgroundWorker();
            _worker.DoWork += (sender, args) => { args.Result = CaptureAndSleep(); };
            _worker.RunWorkerCompleted += (sender, args) =>
            {
                var capturedEventArgs = new ScreenCapturedEventArgs(args.Result as BitmapSource);
                OnScreenCaptured(capturedEventArgs);
                if (IsRunning)
                {
                    _worker.RunWorkerAsync();
                }
            };

            _worker.RunWorkerAsync();
        }

        public void Stop()
        {
            IsRunning = false;
        }

        #endregion

        internal ImageSource CaptureAndSleep()
        {
            var timer = new Stopwatch();

            timer.Restart();
            using (var bitmap = CaptureScreen())
            {
                var capturedImage = BitmapConverter(bitmap);

                if (CapturesPerSecond > 0)
                {
                    var sleepTime = (int) ((1000f / CapturesPerSecond) - timer.ElapsedMilliseconds);
                    if (sleepTime > 0)
                    {
                        Thread.Sleep(sleepTime);
                    }
                }

                return capturedImage;
            }
        }

        void OnScreenCaptured(ScreenCapturedEventArgs e)
        {
            EventHandler<ScreenCapturedEventArgs> handler = ScreenCaptured;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        internal Bitmap CaptureScreen()
        {
            var region = CaptureRegionResolver();
            if (region != null)
            {
                var bitmap = new Bitmap((int) region.Width, (int) region.Height);

                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CopyFromScreen(new Point(region.Left, region.Top),
                        new Point(0, 0),
                        new Size(region.Width, region.Height));
                }

                return bitmap;
            }
            return new Bitmap(1,1);
        }
    }
}