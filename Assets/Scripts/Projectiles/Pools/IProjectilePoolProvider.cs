using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Projectiles.Pools
{
    public interface IProjectilePoolProvider
    {
        Pool<ProjectileController, ProjectileActivationArgs<IProjectileStats>> BulletsPool { get; }
        Pool<ProjectileController, ProjectileActivationArgs<IProjectileStats>> HighCalibreBulletsPool { get; }
        Pool<ProjectileController, ProjectileActivationArgs<IProjectileStats>> TinyBulletsPool { get; }
        Pool<ProjectileController, ProjectileActivationArgs<IProjectileStats>> FlakBulletsPool { get; }
        Pool<ProjectileController, ProjectileActivationArgs<IProjectileStats>> ShellsLargePool { get; }
        Pool<ProjectileController, ProjectileActivationArgs<IProjectileStats>> NovaShellPool { get; }
        Pool<ProjectileController, ProjectileActivationArgs<IProjectileStats>> FiveShellCluster { get; }
        Pool<ProjectileController, ProjectileActivationArgs<IProjectileStats>> RocketShellPool { get; }
        Pool<ProjectileController, ProjectileActivationArgs<IProjectileStats>> ShellsSmallPool { get; }
        Pool<BombController, ProjectileActivationArgs<IProjectileStats>> BombsPool { get; }
        Pool<BombController, ProjectileActivationArgs<IProjectileStats>> StratBombsPool { get; }
        Pool<RocketController, TargetProviderActivationArgs<ICruisingProjectileStats>> RocketsPool { get; }
        Pool<MissileController, TargetProviderActivationArgs<IProjectileStats>> MissilesSmallPool { get; }
        Pool<RocketController, TargetProviderActivationArgs<ICruisingProjectileStats>> RocketsSmallPool { get; }
        Pool<MissileController, TargetProviderActivationArgs<IProjectileStats>> MissilesMediumPool { get; }
        Pool<MissileController, TargetProviderActivationArgs<IProjectileStats>> MissilesMFPool { get; }
        Pool<RocketController, TargetProviderActivationArgs<ICruisingProjectileStats>> MissilesFirecrackerPool { get; }
        Pool<MissileController, TargetProviderActivationArgs<IProjectileStats>> MissilesLargePool { get; }
        Pool<SmartMissileController, SmartMissileActivationArgs<ISmartProjectileStats>> MissilesSmartPool { get; }

    }
}