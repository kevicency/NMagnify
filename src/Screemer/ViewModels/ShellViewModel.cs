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

        public CapturedScreenViewModel CapturedScreen { get; private set; }


    }
}