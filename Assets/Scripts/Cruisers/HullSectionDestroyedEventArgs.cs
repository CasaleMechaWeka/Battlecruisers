using System;

namespace BattleCruisers.Cruisers
{
    public class HullSectionDestroyedEventArgs : EventArgs
    {
        public CruiserSection DestroyedHull { get; }

        public HullSectionDestroyedEventArgs(CruiserSection destroyedHull)
        {
            DestroyedHull = destroyedHull;
        }
    }
}
