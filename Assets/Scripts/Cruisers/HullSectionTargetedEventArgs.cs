using System;

namespace BattleCruisers.Cruisers
{
    public class HullSectionTargetedEventArgs : EventArgs
    {
        public CruiserSection Hull { get; }

        public HullSectionTargetedEventArgs(CruiserSection hull)
        {
            Hull = hull;
        }
    }
}
