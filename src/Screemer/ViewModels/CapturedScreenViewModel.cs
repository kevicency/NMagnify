using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Caliburn.Micro;
using Screemer.Model;

namespace Screemer.ViewModels
{
    public class CapturedScreenViewModel : Screen
    {
        readonly IScreenCapturer _screenCapturer;

        public ImageSource CapturedImage { get; set; }

        public CapturedScreenViewModel(IScreenCapturer screenCapturer)
        {
            _screenCapturer = screenCapturer;

            _screenCapturer.ScreenRegion = new Rectangle(1620, 785, 290, 290);
            _screenCapturer.CapturesPerSecond = 25;
            _screenCapturer.ScreenCaptured += (sender, args) =>
            {
                CapturedImage = args.CapturedImage;
            };
        }

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);
            if (_screenCapturer is BitmapScreenCapturer)
            {
                var dispatcher = (view as DependencyObject).Dispatcher;
                (_screenCapturer as BitmapScreenCapturer).BitmapConverter =
                    x => BitmapUtility.ConvertBitmapToImageSourceOnDispatcher(x, dispatcher);
            }
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            _screenCapturer.Start();
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            _screenCapturer.Stop();
        }
    }
}
