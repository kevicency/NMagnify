using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using Caliburn.Micro.Contrib;
using Caliburn.Micro.Contrib.Dialogs;
using NMagnify.Model;
using NMagnify.Properties;
using NMagnify.Views.Dialog;

namespace NMagnify.ViewModels
{
    public class ProfileManagerViewModel : Screen, IActiveProfileProvider
    {
        readonly IProfileManager _profileManager;
        Profile _activeProfile;

        public ProfileManagerViewModel(IProfileManager profileManager)
        {
            _profileManager = profileManager;
            AvailableProfiles = new BindableCollection<Profile>();
        }

        public IObservableCollection<Profile> AvailableProfiles { get; private set; }

        public bool CanDeleteActiveProfile
        {
            get { return AvailableProfiles.Count > 1 || ActiveProfile.Guid != Guid.Empty; }
        }

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

            AvailableProfiles.AddRange(_profileManager.LoadAll()
                .OrderBy(x => x.Name));
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

        public IEnumerable<IResult> CreateNewProfile()
        {
            var newProfile = Profile.Default;
            newProfile.Name = "New profile";

            AvailableProfiles.Add(newProfile);
            ActiveProfile = newProfile;

            return EditActiveProfile();
        }

        public IEnumerable<IResult> EditActiveProfile()
        {
            var editor = new EditProfileDialog
                         {
                             Name = ActiveProfile.Name,
                             CPS = ActiveProfile.CPS
                         };

            yield return editor.AsResult()
                .PrefixViewContextWith("EditProfile")
                .CancelOnResponse(Answer.Cancel);

            ActiveProfile.Name = editor.Name;
            ActiveProfile.CPS = editor.CPS;
        }


        public IEnumerable<IResult> DeleteActiveProfile()
        {
            var confirmation = new Question("Are you sure ?",
                string.Format("Do you really want to delete the profile '{0}' ?", ActiveProfile.Name),
                Answer.Yes,
                Answer.No);

            yield return confirmation.AsResult()
                .CancelOnResponse(Answer.No);

            var toDelete = ActiveProfile;

            if (AvailableProfiles.Count == 1)
            {
                AvailableProfiles.Add(Profile.Default);
            }

            var index = AvailableProfiles.IndexOf(ActiveProfile);
            if (index == AvailableProfiles.Count - 1)
            {
                ActiveProfile = AvailableProfiles[index - 1];
            }
            else
            {
                ActiveProfile = AvailableProfiles[index + 1];
            }

            _profileManager.Delete(toDelete);

            yield break;
        }
    }
}