using System;
using Caliburn.Micro;
using Screemer.Model;
using System.Linq;
using Screemer.Properties;

namespace Screemer.ViewModels
{
    public class SelectProfileViewModel : Screen, IActiveProfileProvider
    {
        readonly IProfileManager _profileManager;
        Profile _activeProfile;

        public SelectProfileViewModel(IProfileManager profileManager)
        {
            _profileManager = profileManager;
            AvailableProfiles = new BindableCollection<Profile>();
        }

        public IObservableCollection<Profile> AvailableProfiles { get; private set; }

        #region IActiveProfileProvider Members

        public event EventHandler<ProfileEventArgs> ActiveProfileChanged;

        public Profile ActiveProfile
        {
            get { return _activeProfile; }
            set
            {
                _activeProfile = value;
                OnActiveProfileChanged(new ProfileEventArgs(value));
            }
        }

        #endregion

        public void OnActiveProfileChanged(ProfileEventArgs e)
        {
            EventHandler<ProfileEventArgs> handler = ActiveProfileChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            AvailableProfiles.AddRange(_profileManager.LoadAll());
            ActiveProfile = AvailableProfiles.SingleOrDefault(x => x.Guid == Settings.Default.LastActiveProfile)
                ?? AvailableProfiles.First();

            _profileManager.ProfileCreated += AddCreatedProfile;
            _profileManager.ProfileDeleted += RemoveDeletedProfile;
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            if (close)
            {
                _profileManager.ProfileCreated -= AddCreatedProfile;
                _profileManager.ProfileDeleted -= RemoveDeletedProfile;

                Settings.Default.LastActiveProfile = ActiveProfile.Guid;
                Settings.Default.Save();

                foreach (var profile in AvailableProfiles)
                {
                    _profileManager.Save(profile);
                }
            }
        }

        void RemoveDeletedProfile(object sender, ProfileEventArgs e)
        {
            AvailableProfiles.Remove(e.Profile);
        }

        void AddCreatedProfile(object sender, ProfileEventArgs e)
        {
            AvailableProfiles.Add(e.Profile);
        }
    }
}