using BattleCruisers.Buildables;
using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats
{
    public interface IPvPSmartProjectileStats : IPvPProjectileStats
    {
        float DetectionRangeM { get; }
        ReadOnlyCollection<TargetType> AttackCapabilities { get; }
    }
}
