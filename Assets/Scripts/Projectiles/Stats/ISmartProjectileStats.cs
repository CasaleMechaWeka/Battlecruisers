using BattleCruisers.Buildables;
using System.Collections.ObjectModel;

namespace BattleCruisers.Projectiles.Stats
{
    public interface ISmartProjectileStats : IProjectileStats
    {
        float DetectionRangeM { get; }
        ReadOnlyCollection<TargetType> AttackCapabilities { get; }
    }
}
