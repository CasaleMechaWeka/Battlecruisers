using System;

namespace BattleCruisers.Cruisers
{
    public class CruiserSectionDestroyedEventArgs : EventArgs
    {
        public CruiserSection DestroyedSection { get; }

        public CruiserSectionDestroyedEventArgs(CruiserSection destroyedSection)
        {
            DestroyedSection = destroyedSection;
        }
    }
}
