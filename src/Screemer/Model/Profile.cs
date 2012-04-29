using System;

namespace Screemer.Model
{
    public class Profile
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public ScreenRegion CaptureRegion { get; set; }
        public ScreenRegion PlaybackRegion { get; set; }

        public Profile()
        {
            CaptureRegion = new ScreenRegion();
            PlaybackRegion = new ScreenRegion();
        }

        public static Profile Default
        {
            get
            {
                return new Profile()
                       {
                           PlaybackRegion = new ScreenRegion(0, 0, 200, 200),
                           Guid = Guid.Empty,
                           Name = "Default",
                           CaptureRegion = new ScreenRegion(0, 200, 200, 400)
                       };
            }
        }
    }
}