using System.Collections.Generic;
using Caliburn.Micro;

namespace Screemer.ViewModels
{
    public class ShellViewModel : Screen, IShell
    {
        public ShellViewModel(PlaybackStreamViewModel playbackStream, CaptureRegionSettingsViewModel captureRegionSettings, SelectProfileViewModel selectProfile)
        {
            DisplayName = "Screemer";
            PlaybackStream = playbackStream;
            CaptureRegionSettings = captureRegionSettings;
            SelectProfile = selectProfile;

            PlaybackStream.DeactivateWith(this);
            CaptureRegionSettings.ConductWith(this);
            SelectProfile.ConductWith(this);
        }

        public PlaybackStreamViewModel PlaybackStream { get; private set; }
        public CaptureRegionSettingsViewModel CaptureRegionSettings { get; private set; }
        public SelectProfileViewModel SelectProfile { get; set; }

        public IEnumerable<IResult> ShowCapturedScreen()
        {
            PlaybackStream.Show();

            yield break;
        } 
    }
}