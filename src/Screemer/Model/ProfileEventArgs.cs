using System;

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
}