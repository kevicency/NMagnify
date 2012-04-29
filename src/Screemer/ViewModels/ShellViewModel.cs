using Caliburn.Micro;

namespace Screemer.ViewModels
{
    public class ShellViewModel : Screen, IShell
    {
        public ShellViewModel(CapturedScreenViewModel capturedScreen)
        {
            DisplayName = "Screemer";
            CapturedScreen = capturedScreen;
            CapturedScreen.DeactivateWith(this);
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            IoC.Get<IWindowManager>().ShowWindow(new SelectScreenRegionViewModel());
        }

        public CapturedScreenViewModel CapturedScreen { get; private set; }
    }
}