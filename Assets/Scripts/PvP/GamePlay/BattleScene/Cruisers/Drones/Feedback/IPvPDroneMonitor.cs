using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using System.Collections.Generic;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback
{
    public interface IPvPDroneMonitor
    {
        IReadOnlyDictionary<PvPFaction, int> FactionToActiveDroneNum { get; }
        IPvPBroadcastingProperty<bool> PlayerCruiserHasActiveDrones { get; }
        IPvPBroadcastingProperty<bool> AICruiserHasActiveDrones { get; }
    }
}
