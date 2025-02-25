using BattleCruisers.Effects.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Deaths.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using BattleCruisers.UI.Sound.Pools;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories
{
    public interface IPvPPoolProviders
    {
        IPvPExplosionPoolProvider ExplosionPoolProvider { get; }
        IPvPShipDeathPoolProvider ShipDeathPoolProvider { get; }
        IPvPProjectilePoolProvider ProjectilePoolProvider { get; }
        IPvPUnitPoolProvider UnitPoolProvider { get; }
        IPvPPool<IDroneController, DroneActivationArgs> DronePool { get; }
        IPvPPool<IPoolable<AudioSourceActivationArgs>, AudioSourceActivationArgs> AudioSourcePool { get; }
        IPvPUnitToPoolMap UnitToPoolMap { get; }
    }
}