using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Projectiles.Pools
{
    public interface IPvPProjectilePoolProvider
    {
        Pool<PvPProjectileController, ProjectileActivationArgs<IProjectileStats>> BulletsPool { get; }
        Pool<PvPProjectileController, ProjectileActivationArgs<IProjectileStats>> HighCalibreBulletsPool { get; }
        Pool<PvPProjectileController, ProjectileActivationArgs<IProjectileStats>> TinyBulletsPool { get; }
        Pool<PvPProjectileController, ProjectileActivationArgs<IProjectileStats>> FlakBulletsPool { get; }
        Pool<PvPProjectileController, ProjectileActivationArgs<IProjectileStats>> ShellsLargePool { get; }
        Pool<PvPProjectileController, ProjectileActivationArgs<IProjectileStats>> NovaShellPool { get; }
        Pool<PvPProjectileController, ProjectileActivationArgs<IProjectileStats>> FiveShellCluster { get; }
        Pool<PvPProjectileController, ProjectileActivationArgs<IProjectileStats>> RocketShellPool { get; }
        Pool<PvPProjectileController, ProjectileActivationArgs<IProjectileStats>> ShellsSmallPool { get; }
        Pool<PvPBombController, ProjectileActivationArgs<IProjectileStats>> BombsPool { get; }
        Pool<PvPBombController, ProjectileActivationArgs<IProjectileStats>> StratBombsPool { get; }
        Pool<PvPRocketController, TargetProviderActivationArgs<ICruisingProjectileStats>> RocketsPool { get; }
        Pool<PvPMissileController, TargetProviderActivationArgs<IProjectileStats>> MissilesSmallPool { get; }
        Pool<PvPRocketController, TargetProviderActivationArgs<ICruisingProjectileStats>> RocketsSmallPool { get; }
        Pool<PvPMissileController, TargetProviderActivationArgs<IProjectileStats>> MissilesMediumPool { get; }
        Pool<PvPMissileController, TargetProviderActivationArgs<IProjectileStats>> MissilesMFPool { get; }
        Pool<PvPRocketController, TargetProviderActivationArgs<ICruisingProjectileStats>> MissilesFirecrackerPool { get; }
        Pool<PvPMissileController, TargetProviderActivationArgs<IProjectileStats>> MissilesLargePool { get; }
        Pool<PvPSmartMissileController, PvPSmartMissileActivationArgs<ISmartProjectileStats>> MissilesSmartPool { get; }

    }
}