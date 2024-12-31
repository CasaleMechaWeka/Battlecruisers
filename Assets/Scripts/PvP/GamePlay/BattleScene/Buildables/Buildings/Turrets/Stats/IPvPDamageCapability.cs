using System.Collections.Generic;
using BattleCruisers.Buildables;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.Stats
{
    public interface IPvPDamageCapability
    {
        float DamagePerS { get; }
        IList<TargetType> AttackCapabilities { get; }
    }
}