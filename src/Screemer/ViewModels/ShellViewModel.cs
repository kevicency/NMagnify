using System.Collections.Generic;
using Caliburn.Micro;

namespace Screemer.ViewModels
{
    public class ShellViewModel : Screen, IShell
    {
        public ShellViewModel(CapturedScreenViewModel capturedScreen, ScreenRegionSettingsViewModel screenRegionSettings)
        {
            DisplayName = "Screemer";
            CapturedScreen = capturedScreen;
            ScreenRegionSettings = screenRegionSettings;

            CapturedScreen.DeactivateWith(this);
            ScreenRegionSettings.ConductWith(this);
        }

        public CapturedScreenViewModel CapturedScreen { get; private set; }
        public ScreenRegionSettingsViewModel ScreenRegionSettings { get; private set; }

        public IEnumerable<IResult> ShowCapturedScreen()
        {
            CapturedScreen.Show();

            yield break;
        } 
    }
}