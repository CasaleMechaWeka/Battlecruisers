using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Stats;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public interface IPvPProjectilePoolProvider
    {
        IPool<PvPProjectileController, PvPProjectileActivationArgs<IProjectileStats>> BulletsPool { get; }
        IPool<PvPProjectileController, PvPProjectileActivationArgs<IProjectileStats>> HighCalibreBulletsPool { get; }
        IPool<PvPProjectileController, PvPProjectileActivationArgs<IProjectileStats>> TinyBulletsPool { get; }
        IPool<PvPProjectileController, PvPProjectileActivationArgs<IProjectileStats>> FlakBulletsPool { get; }
        IPool<PvPProjectileController, PvPProjectileActivationArgs<IProjectileStats>> ShellsLargePool { get; }
        IPool<PvPProjectileController, PvPProjectileActivationArgs<IProjectileStats>> NovaShellPool { get; }
        IPool<PvPProjectileController, PvPProjectileActivationArgs<IProjectileStats>> FiveShellCluster { get; }
        IPool<PvPProjectileController, PvPProjectileActivationArgs<IProjectileStats>> RocketShellPool { get; }
        IPool<PvPProjectileController, PvPProjectileActivationArgs<IProjectileStats>> ShellsSmallPool { get; }
        IPool<PvPBombController, PvPProjectileActivationArgs<IProjectileStats>> BombsPool { get; }
        IPool<PvPBombController, PvPProjectileActivationArgs<IProjectileStats>> StratBombsPool { get; }
        IPool<PvPRocketController, PvPTargetProviderActivationArgs<IPvPCruisingProjectileStats>> RocketsPool { get; }
        IPool<PvPMissileController, PvPTargetProviderActivationArgs<IProjectileStats>> MissilesSmallPool { get; }
        IPool<PvPRocketController, PvPTargetProviderActivationArgs<IPvPCruisingProjectileStats>> RocketsSmallPool { get; }
        IPool<PvPMissileController, PvPTargetProviderActivationArgs<IProjectileStats>> MissilesMediumPool { get; }
        IPool<PvPMissileController, PvPTargetProviderActivationArgs<IProjectileStats>> MissilesMFPool { get; }
        IPool<PvPRocketController, PvPTargetProviderActivationArgs<IPvPCruisingProjectileStats>> MissilesFirecrackerPool { get; }
        IPool<PvPMissileController, PvPTargetProviderActivationArgs<IProjectileStats>> MissilesLargePool { get; }
        IPool<PvPSmartMissileController, PvPSmartMissileActivationArgs<IPvPSmartProjectileStats>> MissilesSmartPool { get; }

    }
}