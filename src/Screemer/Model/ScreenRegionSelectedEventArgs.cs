using System;
using System.Drawing;

namespace Screemer.Model
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