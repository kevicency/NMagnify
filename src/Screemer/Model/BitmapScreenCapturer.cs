using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Point = System.Drawing.Point;

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

    public interface IScreenCapturer
    {
        int CapturesPerSecond { get; set; }
        Rectangle ScreenRegion { get; set; }

        bool IsRunning { get; }
        event EventHandler<ScreenCapturedEventArgs> ScreenCaptured;

        void Start();
        void Stop();
    }

    public class BitmapScreenCapturer : IScreenCapturer
    {
        readonly Dispatcher _dispatcher;
        BackgroundWorker _worker;

        public BitmapScreenCapturer(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        #region IScreenCapturer Members

        public event EventHandler<ScreenCapturedEventArgs> ScreenCaptured;

        public int CapturesPerSecond { get; set; }
        public Rectangle ScreenRegion { get; set; }

        public bool IsRunning { get; private set; }

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

        internal BitmapSource CaptureAndSleep()
        {
            var timer = new Stopwatch();

            timer.Restart();
            using (var bitmap = CaptureScreen())
            {
                var capture = TransformBitmapToSource(bitmap);

                if (CapturesPerSecond != 0)
                {
                    var sleepTime = (int) ((1000f / CapturesPerSecond) - timer.ElapsedMilliseconds);
                    if (sleepTime > 0)
                    {
                        Thread.Sleep(sleepTime);
                    }
                }

                return capture;
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

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);


        internal Bitmap CaptureScreen()
        {
            var bitmap = new Bitmap(ScreenRegion.Width, ScreenRegion.Height);

            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(ScreenRegion.Location, new Point(0, 0), ScreenRegion.Size);
            }

            return bitmap;
        }

        internal BitmapSource TransformBitmapToSource(Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            BitmapSource source = null;

            //Action dispatcherDelegate = () =>
            //{
                source = Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            //};
            //var dispatcherOperation = _dispatcher.BeginInvoke(dispatcherDelegate);
            //dispatcherOperation.Wait();

            DeleteObject(hBitmap);

            return source;
        }
    }
}