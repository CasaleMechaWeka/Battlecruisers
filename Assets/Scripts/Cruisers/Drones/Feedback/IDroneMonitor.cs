using BattleCruisers.Buildables;
using System.Collections.Generic;
using BattleCruisers.Utils.Properties;

namespace BattleCruisers.Cruisers.Drones.Feedback
{
    public interface IDroneMonitor
    {
        IReadOnlyDictionary<Faction, int> FactionToActiveDroneNum { get; }
        IBroadcastingProperty<bool> PlayerCruiserHasActiveDrones { get; }
        IBroadcastingProperty<bool> AICruiserHasActiveDrones { get; }
    }
}