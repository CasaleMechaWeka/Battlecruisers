using System;

namespace BattleCruisers.Cruisers
{
    public class HullSectionDestroyedEventArgs : EventArgs
    {
        public Hull DestroyedHull { get; }

        public HullSectionDestroyedEventArgs(Hull destroyedHull)
        {
            DestroyedHull = destroyedHull;
        }
    }
}
