using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Screemer
{
    static class BitmapUtility
    {
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        public static BitmapSource ConvertBitmapToImageSourceOnDispatcher(Bitmap bitmap)
        {
            return ConvertBitmapToImageSourceOnDispatcher(bitmap, Dispatcher.CurrentDispatcher);
        }

        public static BitmapSource ConvertBitmapToImageSourceOnDispatcher(Bitmap bitmap, Dispatcher dispatcher)
        {
            BitmapSource source = null;

            Action<object> dispatcherDelegate = x =>
            {
                source = ConvertBitmapToImageSource(x as Bitmap);
            };
            dispatcher.Invoke(dispatcherDelegate, bitmap);

            return source;
        }

        public static BitmapSource ConvertBitmapToImageSource(Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();

            BitmapSource source = Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());

            DeleteObject(hBitmap);

            return source;
        }
    }
}
