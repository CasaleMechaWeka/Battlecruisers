using BattleCruisers.Buildables;
using BattleCruisers.Projectiles.Stats;
using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats
{
    public interface IPvPSmartProjectileStats : IProjectileStats
    {
        float DetectionRangeM { get; }
        ReadOnlyCollection<TargetType> AttackCapabilities { get; }
    }
}
