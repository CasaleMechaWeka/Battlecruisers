using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Deaths.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Pools;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories
{
    public interface IPvPPoolProviders
    {
        IPvPExplosionPoolProvider ExplosionPoolProvider { get; }
        IPvPShipDeathPoolProvider ShipDeathPoolProvider { get; }
        IPvPProjectilePoolProvider ProjectilePoolProvider { get; }
        IPvPUnitPoolProvider UnitPoolProvider { get; }
        IPvPPool<IPvPDroneController, PvPDroneActivationArgs> DronePool { get; }
        IPvPPool<IPvPAudioSourcePoolable, PvPAudioSourceActivationArgs> AudioSourcePool { get; }
        IPvPUnitToPoolMap UnitToPoolMap { get; }
    }
}