using System;

namespace NMagnify.Model
{
    public class ProfileEventArgs : EventArgs
    {
        public ProfileEventArgs(Profile profile)
        {
            Profile = profile;
        }

        public Profile Profile { get; private set; }
    }
}