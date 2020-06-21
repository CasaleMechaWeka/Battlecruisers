using BattleCruisers.Buildables.Pools;
using BattleCruisers.Effects.Deaths.Pools;
using BattleCruisers.Effects.Drones;
using BattleCruisers.Effects.Explosions.Pools;
using BattleCruisers.Projectiles.Pools;
using BattleCruisers.UI.Sound.Pools;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Utils.Factories
{
    public interface IPoolProviders
    {
        IExplosionPoolProvider ExplosionPoolProvider { get; }
        IShipDeathPoolProvider ShipDeathPoolProvider { get; }
        IProjectilePoolProvider ProjectilePoolProvider { get; }
        IUnitPoolProvider UnitPoolProvider { get; }
        IPool<IDroneController, DroneActivationArgs> DronePool { get; }
        IPool<IAudioSourcePoolable, AudioSourceActivationArgs> AudioSourcePool { get; }
        IUnitToPoolMap UnitToPoolMap { get; }
    }
}