using System;
using System.ComponentModel;

namespace NMagnify.Model
{
    public class Profile : INotifyPropertyChanged
    {
        public Profile()
        {
            CaptureRegion = new ScreenRegion();
            PlaybackRegion = new ScreenRegion();
        }

        public Guid Guid { get; set; }
        public string Name { get; set; }
        public ScreenRegion CaptureRegion { get; set; }
        public ScreenRegion PlaybackRegion { get; set; }
        public int CPS { get; set; }

        public static Profile Default
        {
            get
            {
                return new Profile
                       {
                           PlaybackRegion = new ScreenRegion(0, 0, 200, 200),
                           Guid = Guid.Empty,
                           Name = "Default",
                           CPS = 25,
                           CaptureRegion = new ScreenRegion(0, 200, 200, 400)
                       };
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}