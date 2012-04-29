using System.Collections.Generic;
using Caliburn.Micro;

namespace Screemer.ViewModels
{
    public class ShellViewModel : Screen, IShell
    {
        public ShellViewModel(PlaybackViewModel playback, CaptureRegionSettingsViewModel captureRegionSettings, SelectProfileViewModel selectProfile)
        {
            DisplayName = "Screemer";
            Playback = playback;
            CaptureRegionSettings = captureRegionSettings;
            SelectProfile = selectProfile;

            Playback.DeactivateWith(this);
            CaptureRegionSettings.ConductWith(this);
            SelectProfile.ConductWith(this);
        }

        public PlaybackViewModel Playback { get; private set; }
        public CaptureRegionSettingsViewModel CaptureRegionSettings { get; private set; }
        public SelectProfileViewModel SelectProfile { get; set; }

        public IEnumerable<IResult> ShowCapturedScreen()
        {
            Playback.Show();

            yield break;
        } 
    }
}