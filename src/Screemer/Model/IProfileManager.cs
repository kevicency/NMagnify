using System;
using System.Collections.Generic;

namespace Screemer.Model
{
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
}