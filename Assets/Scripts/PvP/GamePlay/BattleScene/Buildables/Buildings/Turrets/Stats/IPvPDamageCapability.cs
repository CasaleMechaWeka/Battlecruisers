using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats
{
    public interface IPvPDamageCapability
    {
        float DamagePerS { get; }
        IList<PvPTargetType> AttackCapabilities { get; }
    }
}