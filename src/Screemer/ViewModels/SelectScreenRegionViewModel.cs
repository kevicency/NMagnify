using System.Drawing;
using Caliburn.Micro;
using Screemer.Model;
using Screemer.Views;

namespace Screemer.ViewModels
{
    public class SelectScreenRegionViewModel : Screen
    {
        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);

            (view as ISelectScreenRegion).ScreenRegionSelected += (sender, args) =>
            {
                SelectedRegion = args.SelectedRegion;
            };
        }

        protected ScreenRegion SelectedRegion { get; set; }
    }

    public class ScreenRegionSettingsViewModel : Screen
    {
        readonly IScreenCapturer _screenCapturer;

        public ScreenRegionSettingsViewModel(IScreenCapturer screenCapturer)
        {
            _screenCapturer = screenCapturer;
        }

        public ScreenRegion ScreenRegion
        {
            get { return _screenCapturer.ScreenRegion; }
        }

        public int Left
        {
            get { return ScreenRegion.Left; }
            set { ScreenRegion.Left = value; }
        }

        public int Top
        {
            get { return ScreenRegion.Top; }
            set { ScreenRegion.Top = value; }
        }

        public int Right
        {
            get { return ScreenRegion.Right; }
            set { ScreenRegion.Right = value; }
        }

        public int Bottom
        {
            get { return ScreenRegion.Bottom; }
            set { ScreenRegion.Bottom = value; }
        }
    }
}