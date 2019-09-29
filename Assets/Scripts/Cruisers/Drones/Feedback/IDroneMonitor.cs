using BattleCruisers.Buildables;
using System.Collections.Generic;
using UnityCommon.Properties;

namespace BattleCruisers.Cruisers.Drones.Feedback
{
    public interface IDroneMonitor
    {
        // FELIX  Remove?
        IReadOnlyDictionary<Faction, int> FactionToActiveDroneNum { get; }

        IBroadcastingProperty<bool> PlayerCruiserHasActiveDrones { get; }
        IBroadcastingProperty<bool> AICruiserHasActiveDrones { get; }
    }
}