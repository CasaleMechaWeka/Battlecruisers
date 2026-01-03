using System;

namespace BattleCruisers.Cruisers
{
    public class HullSectionDestroyedEventArgs : EventArgs
    {
        public HullSection DestroyedHull { get; }

        public HullSectionDestroyedEventArgs(HullSection destroyedHull)
        {
            DestroyedHull = destroyedHull;
        }
    }
}
