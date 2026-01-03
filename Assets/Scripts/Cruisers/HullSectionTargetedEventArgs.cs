using System;

namespace BattleCruisers.Cruisers
{
    public class HullSectionTargetedEventArgs : EventArgs
    {
        public Hull Hull { get; }

        public HullSectionTargetedEventArgs(Hull hull)
        {
            Hull = hull;
        }
    }
}
