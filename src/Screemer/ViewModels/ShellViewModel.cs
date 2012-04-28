using System.ComponentModel.Composition;
using System.Drawing;
using System.Windows.Media;
using Caliburn.Micro;
using Screemer.Model;

namespace Screemer.ViewModels 
{
    public class ShellViewModel : Screen, IShell
    {
        readonly IScreenCapturer _screenCapturer;

        public ImageSource Capture { get; set; }

        public ShellViewModel(IScreenCapturer screenCapturer)
        {
            _screenCapturer = screenCapturer;

            _screenCapturer.ScreenRegion = new Rectangle(1620, 785, 290, 290);
            _screenCapturer.CapturesPerSecond = 25;
            _screenCapturer.ScreenCaptured += (sender, args) =>
            {
                Capture = args.CapturedImage;
            };
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
