using System.Windows.Media;
using Caliburn.Micro;
using NMagnify.Model;
using NMagnify.Views;

namespace NMagnify.ViewModels
{
    public class PlaybackStreamViewModel : Screen
    {
        readonly IActiveProfileProvider _activeProfileProvider;
        readonly IScreenCapturer _screenCapturer;
        readonly IWindowManager _windowManager;
        PlaybackStreamView _streamView;

        Profile _activeProfile;

        public PlaybackStreamViewModel(IScreenCapturer screenCapturer,
                                       IWindowManager windowManager,
                                       IActiveProfileProvider activeProfileProvider)
        {
            _screenCapturer = screenCapturer;
            _windowManager = windowManager;
            _activeProfileProvider = activeProfileProvider;

            _activeProfileProvider.ActiveProfileChanged += ChangeProfile;

            _screenCapturer.CaptureRegionResolver = () => _activeProfile != null
                                                              ? _activeProfile.CaptureRegion
                                                              : new ScreenRegion();
            _screenCapturer.CapturesPerSecondResolver = () => _activeProfile != null
                                                                  ? _activeProfile.CPS
                                                                  : 25;
            _screenCapturer.ScreenCaptured += (sender, args) => { CapturedImage = args.CapturedImage; };

            CanShow = true;
        }

        void ChangeProfile(object sender, ProfileEventArgs e)
        {
            ChangeProfile(e.Profile);
        }

        void ChangeProfile(Profile profile)
        {
            PersistViewPosition();
            _activeProfile = profile;
            LoadViewPosition();
        }

        public ScreenRegion PlaybackRegion
        {
            get
            {
                return _activeProfile != null
                           ? _activeProfile.PlaybackRegion
                           : null;
            }
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
            _streamView = view as PlaybackStreamView;
            if (_screenCapturer is BitmapScreenCapturer)
            {
                var dispatcher = _streamView.Dispatcher;
                (_screenCapturer as BitmapScreenCapturer).BitmapConverter =
                    x => BitmapUtility.ConvertBitmapToImageSourceOnDispatcher(x, dispatcher);
            }
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            LoadViewPosition();
        }

        void LoadViewPosition()
        {
            if (_streamView != null && PlaybackRegion != null)
            {
                _streamView.Width = PlaybackRegion.Width;
                _streamView.Height = PlaybackRegion.Height;
                _streamView.Left = PlaybackRegion.Left;
                _streamView.Top = PlaybackRegion.Top;
            }
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            _activeProfile = _activeProfileProvider.ActiveProfile;
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
            if (close && _streamView != null)
            {
                PersistViewPosition();
                _streamView = null;
                CanShow = true;
            }
        }

        void PersistViewPosition()
        {
            if (_streamView != null && PlaybackRegion != null)
            {
                PlaybackRegion.Left = (int) _streamView.Left;
                PlaybackRegion.Top = (int) _streamView.Top;
                PlaybackRegion.Right = PlaybackRegion.Left + (int) _streamView.Width;
                PlaybackRegion.Bottom = PlaybackRegion.Top + (int) _streamView.Height;
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
                _streamView.Close();
                _streamView = null;
                CanShow = true;
            }
        }
    }
}