using System;
using Screemer.Model;

namespace Screemer.Messages
{
    public class ProfileSelectedMessage
    {
        public Profile Profile { get; private set; }
        public ProfileSelectedMessage(Profile profile)
        {
            Profile = profile;
        }
    }

    public class ProfileCreatedMessage
    {
        public Guid Guid { get; private set; }
        public ProfileCreatedMessage(Guid guid)
        {
            Guid = guid;
        }
    }

    public class ProfileDeletedMessage
    {
        public Guid Guid { get; private set; }
        public ProfileDeletedMessage(Guid guid)
        {
            Guid = guid;
        }
    }

    public class CaptureRegionChangedMessage
    {
        public ScreenRegion CaptureRegion { get; private set; }
        public CaptureRegionChangedMessage(ScreenRegion captureRegion)
        {
            CaptureRegion = captureRegion;
        }
    }
}