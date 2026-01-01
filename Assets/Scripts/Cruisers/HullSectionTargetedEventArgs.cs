using System;

namespace BattleCruisers.Cruisers
{
    public class HullSectionTargetedEventArgs : EventArgs
    {
        public HullSection HullSection { get; }

        public HullSectionTargetedEventArgs(HullSection hullSection)
        {
            HullSection = hullSection;
        }
    }
}
