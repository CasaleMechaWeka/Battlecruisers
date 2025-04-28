using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Projectiles.Pools
{
    public interface IProjectilePoolProvider
    {
        Pool<ProjectileController, ProjectileActivationArgs<ProjectileStats>> BulletsPool { get; }
        Pool<ProjectileController, ProjectileActivationArgs<ProjectileStats>> HighCalibreBulletsPool { get; }
        Pool<ProjectileController, ProjectileActivationArgs<ProjectileStats>> TinyBulletsPool { get; }
        Pool<ProjectileController, ProjectileActivationArgs<ProjectileStats>> FlakBulletsPool { get; }
        Pool<ProjectileController, ProjectileActivationArgs<ProjectileStats>> ShellsLargePool { get; }
        Pool<ProjectileController, ProjectileActivationArgs<ProjectileStats>> NovaShellPool { get; }
        Pool<ProjectileController, ProjectileActivationArgs<ProjectileStats>> FiveShellCluster { get; }
        Pool<ProjectileController, ProjectileActivationArgs<ProjectileStats>> RocketShellPool { get; }
        Pool<ProjectileController, ProjectileActivationArgs<ProjectileStats>> ShellsSmallPool { get; }
        Pool<BombController, ProjectileActivationArgs<ProjectileStats>> BombsPool { get; }
        Pool<BombController, ProjectileActivationArgs<ProjectileStats>> StratBombsPool { get; }
        Pool<RocketController, TargetProviderActivationArgs<ProjectileStats>> RocketsPool { get; }
        Pool<MissileController, TargetProviderActivationArgs<ProjectileStats>> MissilesSmallPool { get; }
        Pool<RocketController, TargetProviderActivationArgs<ProjectileStats>> RocketsSmallPool { get; }
        Pool<MissileController, TargetProviderActivationArgs<ProjectileStats>> MissilesMediumPool { get; }
        Pool<MissileController, TargetProviderActivationArgs<ProjectileStats>> MissilesMFPool { get; }
        Pool<MissileController, TargetProviderActivationArgs<ProjectileStats>> RailSlugsPool { get; }
        Pool<RocketController, TargetProviderActivationArgs<ProjectileStats>> MissilesFirecrackerPool { get; }
        Pool<MissileController, TargetProviderActivationArgs<ProjectileStats>> MissilesLargePool { get; }
        Pool<SmartMissileController, SmartMissileActivationArgs<ProjectileStats>> MissilesSmartPool { get; }

    }
}