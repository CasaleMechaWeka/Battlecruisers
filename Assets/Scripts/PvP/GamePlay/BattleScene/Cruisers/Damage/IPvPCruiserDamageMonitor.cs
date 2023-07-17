using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Damage
{
    public interface IPvPCruiserDamageMonitor
    {
        event EventHandler CruiserOrBuildingDamaged;
    }
}