using BattleCruisers.Buildables;
using System.Collections.Generic;

namespace BattleCruisers.Cruisers.Drones.Feedback
{
    public interface IDroneMonitor
    {
        IReadOnlyDictionary<Faction, int> FactionToActiveDroneNum { get; }

        // FELIX  Remove
        bool ShouldPlaySound(Faction faction);
    }
}