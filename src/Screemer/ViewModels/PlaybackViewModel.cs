using System.Windows.Media;
using Caliburn.Micro;
using Screemer.Model;
using Screemer.Views;

namespace Screemer.ViewModels
{
    public class PlaybackViewModel : Screen
    {
        readonly IActiveProfileProvider _activeProfileProvider;
        readonly IScreenCapturer _screenCapturer;
        readonly IWindowManager _windowManager;
        PlaybackView _view;

        public PlaybackViewModel(IScreenCapturer screenCapturer,
                                 IWindowManager windowManager,
                                 IActiveProfileProvider activeProfileProvider)
        {
            _screenCapturer = screenCapturer;
            _windowManager = windowManager;
            _activeProfileProvider = activeProfileProvider;

            _screenCapturer.CaptureRegionResolver = () => ActiveProfile != null
                                                              ? ActiveProfile.CaptureRegion
                                                              : null;
            _screenCapturer.CapturesPerSecond = 25;
            _screenCapturer.ScreenCaptured += (sender, args) => { CapturedImage = args.CapturedImage; };

            CanShow = true;

            Width = 100;
        }

        Profile ActiveProfile
        {
            get { return _activeProfileProvider.ActiveProfile; }
        }

        public ScreenRegion PlaybackRegion
        {
            get { return ActiveProfile.PlaybackRegion; }
        }

        public int Width { get; set; }

        public ImageSource CapturedImage { get; set; }
        public bool CanShow { get; private set; }

        public bool CanHide
        {
            get { return !CanShow; }
        }

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);
            _view = view as PlaybackView;
            if (_screenCapturer is BitmapScreenCapturer)
            {
                var dispatcher = _view.Dispatcher;
                (_screenCapturer as BitmapScreenCapturer).BitmapConverter =
                    x => BitmapUtility.ConvertBitmapToImageSourceOnDispatcher(x, dispatcher);
            }
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            _view.Width = PlaybackRegion.Width;
            _view.Height = PlaybackRegion.Height;
            _view.Left = PlaybackRegion.Left;
            _view.Top = PlaybackRegion.Top;
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
            if (close && _view != null)
            {
                PlaybackRegion.Left = (int) _view.Left;
                PlaybackRegion.Top = (int) _view.Top;
                PlaybackRegion.Right = PlaybackRegion.Left + (int)_view.Width;
                PlaybackRegion.Bottom = PlaybackRegion.Top + (int)_view.Height;
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