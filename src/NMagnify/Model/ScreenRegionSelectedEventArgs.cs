using System;

namespace NMagnify.Model
{
    public class ScreenRegionSelectedEventArgs : EventArgs
    {
        public ScreenRegionSelectedEventArgs(ScreenRegion selectedRegion)
        {
            SelectedRegion = selectedRegion;
        }

        public ScreenRegion SelectedRegion { get; private set; }
    }
}