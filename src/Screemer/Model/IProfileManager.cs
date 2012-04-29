using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Screemer.Model
{
    public class ProfileEventArgs : EventArgs
    {
        public Profile Profile { get; private set; }

        public ProfileEventArgs(Profile profile)
        {
            Profile = profile;
        }
    }
    public interface IProfileManager
    {
        event EventHandler<ProfileEventArgs> ProfileCreated;
        event EventHandler<ProfileEventArgs> ProfileDeleted;
        
        IEnumerable<Profile> LoadAll();

        void Save(Profile profile);
        void Delete(Profile profile);
        Profile Copy(Profile profile);
        Profile Load(Guid guid);
    }

    public class ProfileManager : IProfileManager
    {
        public string AppFolder
        {
            get
            {
                var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

                return Path.Combine(appDataFolder, "MagniMap");
            }
        }

        public string ProfilesFolder { get { return Path.Combine(AppFolder, "Profiles"); } }

        void EnsureFolders()
        {
            if(!Directory.Exists(AppFolder))
            {
                Directory.CreateDirectory(AppFolder);
            }
            if (!Directory.Exists(ProfilesFolder))
            {
                Directory.CreateDirectory(ProfilesFolder);
            }
        }

        public event EventHandler<ProfileEventArgs> ProfileCreated;

        public void OnProfileCreated(ProfileEventArgs e)
        {
            EventHandler<ProfileEventArgs> handler = ProfileCreated;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler<ProfileEventArgs> ProfileDeleted;

        public void OnProfileDeleted(ProfileEventArgs e)
        {
            EventHandler<ProfileEventArgs> handler = ProfileDeleted;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public IEnumerable<Profile> LoadAll()
        {
            EnsureFolders();

            bool hasProfiles = false;
            foreach (var profileFile in Directory.EnumerateFiles(ProfilesFolder, "*.json"))
            {
                Profile profile = null;
                try
                {
                    var fileContent = File.ReadAllText(profileFile);
                    profile = JsonConvert.DeserializeObject<Profile>(fileContent);
                }
                catch
                {
                    continue;
                }
                yield return profile;
                hasProfiles = true;
            }

            if (!hasProfiles) yield return Profile.Default;
        }


        public Profile Load(Guid guid)
        {
            var profileFile = GetFile(guid);
            try
            {
                var fileContent = File.ReadAllText(profileFile);
                return JsonConvert.DeserializeObject<Profile>(fileContent);
            }
            catch
            {
                return null;
            }
        }

        public void Save(Profile profile)
        {
            bool isTransient = profile.Guid == Guid.Empty;

            if (isTransient) profile.Guid = Guid.NewGuid();
            var json = JsonConvert.SerializeObject(profile);
            var file = GetFile(profile.Guid);
            File.WriteAllText(file, json);

            if(isTransient) OnProfileCreated(new ProfileEventArgs(profile));
        }

        string GetFile(Guid guid)
        {
            EnsureFolders();
            var fileName = string.Format("{0}.json", guid);
            var file = Path.Combine(ProfilesFolder, fileName);
            return file;
        }

        public void Delete(Profile profile)
        {
            var file = GetFile(profile.Guid);
            if (File.Exists(file))
            {
                File.Delete(file);
                OnProfileDeleted(new ProfileEventArgs(profile));
            }
        }

        public Profile Copy(Profile profile)
        {
            var copy = new Profile();
            copy.Guid = Guid.Empty;
            copy.Name = string.Format("{0} - Copy", profile.Name);
            copy.CaptureRegion = profile.CaptureRegion.Clone();
            copy.PlaybackRegion = profile.PlaybackRegion.Clone();

            return copy;
        }
    }
}