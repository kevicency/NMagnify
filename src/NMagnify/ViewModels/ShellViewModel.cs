using System.Collections.Generic;
using Caliburn.Micro;

namespace NMagnify.ViewModels
{
    public class ShellViewModel : Screen, IShell
    {
        public ShellViewModel(PlaybackStreamViewModel playbackStream,
                              CaptureRegionSettingsViewModel captureRegionSettings,
                              ProfileManagerViewModel profileManager)
        {
            DisplayName = "Screemer";
            PlaybackStream = playbackStream;
            CaptureRegionSettings = captureRegionSettings;
            ProfileManager = profileManager;

            PlaybackStream.DeactivateWith(this);
            CaptureRegionSettings.ConductWith(this);
            ProfileManager.ConductWith(this);
        }

        public PlaybackStreamViewModel PlaybackStream { get; private set; }
        public CaptureRegionSettingsViewModel CaptureRegionSettings { get; private set; }
        public ProfileManagerViewModel ProfileManager { get; set; }

        public IEnumerable<IResult> ShowCapturedScreen()
        {
            PlaybackStream.Show();

            yield break;
        }
    }
}