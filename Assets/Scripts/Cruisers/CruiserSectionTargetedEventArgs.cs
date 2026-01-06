using System;

namespace BattleCruisers.Cruisers
{
    public class CruiserSectionTargetedEventArgs : EventArgs
    {
        public CruiserSection Section { get; }

        public CruiserSectionTargetedEventArgs(CruiserSection section)
        {
            Section = section;
        }
    }
}
