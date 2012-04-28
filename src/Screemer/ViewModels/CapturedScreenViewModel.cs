using System.Drawing;
using System.Windows.Media;
using Caliburn.Micro;
using Screemer.Model;
using Screemer.Views;

namespace Screemer.ViewModels
{
    public class CapturedScreenViewModel : Screen
    {
        readonly IScreenCapturer _screenCapturer;
        readonly IWindowManager _windowManager;
        CapturedScreenView _view;

        public CapturedScreenViewModel(IScreenCapturer screenCapturer, IWindowManager windowManager)
        {
            _screenCapturer = screenCapturer;
            _windowManager = windowManager;

            _screenCapturer.ScreenRegion = new Rectangle(1620, 785, 290, 290);
            _screenCapturer.CapturesPerSecond = 25;
            _screenCapturer.ScreenCaptured += (sender, args) => { CapturedImage = args.CapturedImage; };

            CanShow = true;
        }

        public ImageSource CapturedImage { get; set; }
        public bool CanShow { get; private set; }

        public bool CanHide
        {
            get { return !CanShow; }
        }

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);
            _view = view as CapturedScreenView;
            if (_screenCapturer is BitmapScreenCapturer)
            {
                var dispatcher = _view.Dispatcher;
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
            if (close)
            {
                _view = null;
            }
        }

        public void Show()
        {
            if (!IsActive)
            {
                _windowManager.ShowWindow(this);
                CanShow = false;
            }
        }

        public void Hide()
        {
            if (IsActive)
            {
                _view.Close();
                _view = null;
                CanShow = true;
            }
        }
    }
}