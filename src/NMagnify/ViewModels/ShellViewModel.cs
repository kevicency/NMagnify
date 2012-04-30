using System.Collections.Generic;
using Caliburn.Micro;

namespace NMagnify.ViewModels
{
    public class ShellViewModel : Screen, IShell
    {
        readonly IWindowManager _windowManager;

        public ShellViewModel(PlaybackStreamViewModel playbackStream,
                              CaptureRegionSettingsViewModel captureRegionSettings,
                              ProfileManagerViewModel profileManager,
            IWindowManager windowManager)
        {
            _windowManager = windowManager;
            DisplayName = App.Name;
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

        public void ShowAbout()
        {
            _windowManager.ShowDialog(new AboutViewModel());
        }

        public void ToggleStream()
        {
            if (PlaybackStream.CanShow)
            {
                PlaybackStream.Show();
            }
            else
            {
                PlaybackStream.Hide();
            }
        }
    }
}