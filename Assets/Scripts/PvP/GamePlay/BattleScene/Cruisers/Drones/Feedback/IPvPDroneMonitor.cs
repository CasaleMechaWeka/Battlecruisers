using System.Collections.Generic;
using BattleCruisers.Buildables;
using BattleCruisers.Utils.Properties;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback
{
    public interface IPvPDroneMonitor
    {
        IReadOnlyDictionary<Faction, int> FactionToActiveDroneNum { get; }
        IBroadcastingProperty<bool> PlayerACruiserHasActiveDrones { get; }
        IBroadcastingProperty<bool> PlayerBCruiserHasActiveDrones { get; }
    }
}
