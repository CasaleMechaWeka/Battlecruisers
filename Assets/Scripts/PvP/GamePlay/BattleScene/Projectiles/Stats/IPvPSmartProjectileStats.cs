using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats
{
    public interface IPvPSmartProjectileStats : IPvPProjectileStats
    {
        float DetectionRangeM { get; }
        ReadOnlyCollection<PvPTargetType> AttackCapabilities { get; }
    }
}
