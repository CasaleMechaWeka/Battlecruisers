using BattleCruisers.Buildables.Pools;
using BattleCruisers.Effects;
using BattleCruisers.Effects.Explosions.Pools;
using BattleCruisers.Projectiles.Pools;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

namespace BattleCruisers.Utils.Factories
{
    public interface IPoolProviders
    {
        IExplosionPoolProvider ExplosionPoolProvider { get; }
        IProjectilePoolProvider ProjectilePoolProvider { get; }
        IUnitPoolProvider UnitPoolProvider { get; }
        IPool<IDroneController, DroneActivationArgs> DronePool { get; }
        IUnitToPoolMap UnitToPoolMap { get; }
    }
}