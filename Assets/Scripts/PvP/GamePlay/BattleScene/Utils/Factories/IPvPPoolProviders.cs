using BattleCruisers.Effects.Deaths.Pools;
using BattleCruisers.Effects.Drones;
using BattleCruisers.Effects.Explosions.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools;
using BattleCruisers.UI.Sound.Pools;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories
{
    public interface IPvPPoolProviders
    {
        IExplosionPoolProvider ExplosionPoolProvider { get; }
        IShipDeathPoolProvider ShipDeathPoolProvider { get; }
        IPvPProjectilePoolProvider ProjectilePoolProvider { get; }
        IPvPUnitPoolProvider UnitPoolProvider { get; }
        IPool<IDroneController, DroneActivationArgs> DronePool { get; }
        IPool<IPoolable<AudioSourceActivationArgs>, AudioSourceActivationArgs> AudioSourcePool { get; }
        IPvPUnitToPoolMap UnitToPoolMap { get; }
    }
}