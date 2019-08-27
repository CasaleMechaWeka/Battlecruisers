using BattleCruisers.Buildables.Pools;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.Projectiles.Pools;

namespace BattleCruisers.Utils.Factories
{
    public interface IPoolProviders
    {
        IExplosionPoolProvider ExplosionPoolProvider { get; }
        IProjectilePoolProvider ProjectilePoolProvider { get; }
        IUnitPoolProvider UnitPoolProvider { get; }
        IUnitToPoolMap UnitToPoolMap { get; }
    }
}