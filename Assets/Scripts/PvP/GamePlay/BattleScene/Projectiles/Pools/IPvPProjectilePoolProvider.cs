using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public interface IPvPProjectilePoolProvider
    {
        IPool<PvPProjectileController, ProjectileActivationArgs<IProjectileStats>> BulletsPool { get; }
        IPool<PvPProjectileController, ProjectileActivationArgs<IProjectileStats>> HighCalibreBulletsPool { get; }
        IPool<PvPProjectileController, ProjectileActivationArgs<IProjectileStats>> TinyBulletsPool { get; }
        IPool<PvPProjectileController, ProjectileActivationArgs<IProjectileStats>> FlakBulletsPool { get; }
        IPool<PvPProjectileController, ProjectileActivationArgs<IProjectileStats>> ShellsLargePool { get; }
        IPool<PvPProjectileController, ProjectileActivationArgs<IProjectileStats>> NovaShellPool { get; }
        IPool<PvPProjectileController, ProjectileActivationArgs<IProjectileStats>> FiveShellCluster { get; }
        IPool<PvPProjectileController, ProjectileActivationArgs<IProjectileStats>> RocketShellPool { get; }
        IPool<PvPProjectileController, ProjectileActivationArgs<IProjectileStats>> ShellsSmallPool { get; }
        IPool<PvPBombController, ProjectileActivationArgs<IProjectileStats>> BombsPool { get; }
        IPool<PvPBombController, ProjectileActivationArgs<IProjectileStats>> StratBombsPool { get; }
        IPool<PvPRocketController, TargetProviderActivationArgs<ICruisingProjectileStats>> RocketsPool { get; }
        IPool<PvPMissileController, TargetProviderActivationArgs<IProjectileStats>> MissilesSmallPool { get; }
        IPool<PvPRocketController, TargetProviderActivationArgs<ICruisingProjectileStats>> RocketsSmallPool { get; }
        IPool<PvPMissileController, TargetProviderActivationArgs<IProjectileStats>> MissilesMediumPool { get; }
        IPool<PvPMissileController, TargetProviderActivationArgs<IProjectileStats>> MissilesMFPool { get; }
        IPool<PvPRocketController, TargetProviderActivationArgs<ICruisingProjectileStats>> MissilesFirecrackerPool { get; }
        IPool<PvPMissileController, TargetProviderActivationArgs<IProjectileStats>> MissilesLargePool { get; }
        IPool<PvPSmartMissileController, PvPSmartMissileActivationArgs<ISmartProjectileStats>> MissilesSmartPool { get; }

    }
}