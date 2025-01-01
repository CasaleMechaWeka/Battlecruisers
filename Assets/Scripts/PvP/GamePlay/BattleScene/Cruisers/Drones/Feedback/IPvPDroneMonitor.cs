using System.Collections.Generic;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;
using BattleCruisers.Buildables;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback
{
    public interface IPvPDroneMonitor
    {
        IReadOnlyDictionary<Faction, int> FactionToActiveDroneNum { get; }
        IPvPBroadcastingProperty<bool> PlayerACruiserHasActiveDrones { get; }
        IPvPBroadcastingProperty<bool> PlayerBCruiserHasActiveDrones { get; }
    }
}
