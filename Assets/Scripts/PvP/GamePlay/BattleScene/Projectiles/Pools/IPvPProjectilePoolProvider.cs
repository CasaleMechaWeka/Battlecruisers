using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public interface IPvPProjectilePoolProvider
    {
        IPool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>> BulletsPool { get; }
        IPool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>> HighCalibreBulletsPool { get; }
        IPool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>> TinyBulletsPool { get; }
        IPool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>> FlakBulletsPool { get; }
        IPool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>> ShellsLargePool { get; }
        IPool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>> NovaShellPool { get; }
        IPool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>> FiveShellCluster { get; }
        IPool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>> RocketShellPool { get; }
        IPool<PvPProjectileController, PvPProjectileActivationArgs<IPvPProjectileStats>> ShellsSmallPool { get; }
        IPool<PvPBombController, PvPProjectileActivationArgs<IPvPProjectileStats>> BombsPool { get; }
        IPool<PvPBombController, PvPProjectileActivationArgs<IPvPProjectileStats>> StratBombsPool { get; }
        IPool<PvPRocketController, PvPTargetProviderActivationArgs<IPvPCruisingProjectileStats>> RocketsPool { get; }
        IPool<PvPMissileController, PvPTargetProviderActivationArgs<IPvPProjectileStats>> MissilesSmallPool { get; }
        IPool<PvPRocketController, PvPTargetProviderActivationArgs<IPvPCruisingProjectileStats>> RocketsSmallPool { get; }
        IPool<PvPMissileController, PvPTargetProviderActivationArgs<IPvPProjectileStats>> MissilesMediumPool { get; }
        IPool<PvPMissileController, PvPTargetProviderActivationArgs<IPvPProjectileStats>> MissilesMFPool { get; }
        IPool<PvPRocketController, PvPTargetProviderActivationArgs<IPvPCruisingProjectileStats>> MissilesFirecrackerPool { get; }
        IPool<PvPMissileController, PvPTargetProviderActivationArgs<IPvPProjectileStats>> MissilesLargePool { get; }
        IPool<PvPSmartMissileController, PvPSmartMissileActivationArgs<IPvPSmartProjectileStats>> MissilesSmartPool { get; }

    }
}