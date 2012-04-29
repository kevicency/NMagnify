using System;
using System.Drawing;

namespace Screemer.Model
{
    public class ScreenRegionSelectedEventArgs : EventArgs
    {
        public ScreenRegionSelectedEventArgs(Rectangle selectedRegion)
        {
            SelectedRegion = selectedRegion;
        }

        public Rectangle SelectedRegion { get; private set; }
    }
}