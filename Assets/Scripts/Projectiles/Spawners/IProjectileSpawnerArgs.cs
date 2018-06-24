using BattleCruisers.Buildables;
using BattleCruisers.Projectiles.Stats.Wrappers;
using BattleCruisers.Utils;

namespace BattleCruisers.Projectiles.Spawners
{
    public interface IProjectileSpawnerArgs
    {
        ITarget Parent { get; }
        IProjectileStats ProjectileStats { get; }
        int BurstSize { get; }
        IFactoryProvider FactoryProvider { get; }
    }
}
