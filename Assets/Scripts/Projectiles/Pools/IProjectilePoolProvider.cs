using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Projectiles.Pools
{
    public interface IProjectilePoolProvider
    {
        Pool<ProjectileController, ProjectileActivationArgs> BulletsPool { get; }
        Pool<ProjectileController, ProjectileActivationArgs> HighCalibreBulletsPool { get; }
        Pool<ProjectileController, ProjectileActivationArgs> TinyBulletsPool { get; }
        Pool<ProjectileController, ProjectileActivationArgs> FlakBulletsPool { get; }
        Pool<ProjectileController, ProjectileActivationArgs> ShellsLargePool { get; }
        Pool<ProjectileController, ProjectileActivationArgs> NovaShellPool { get; }
        Pool<ProjectileController, ProjectileActivationArgs> FiveShellCluster { get; }
        Pool<ProjectileController, ProjectileActivationArgs> RocketShellPool { get; }
        Pool<ProjectileController, ProjectileActivationArgs> ShellsSmallPool { get; }
        Pool<BombController, ProjectileActivationArgs> BombsPool { get; }
        Pool<BombController, ProjectileActivationArgs> StratBombsPool { get; }
        Pool<RocketController, ProjectileActivationArgs> RocketsPool { get; }
        Pool<MissileController, ProjectileActivationArgs> MissilesSmallPool { get; }
        Pool<RocketController, ProjectileActivationArgs> RocketsSmallPool { get; }
        Pool<MissileController, ProjectileActivationArgs> MissilesMediumPool { get; }
        Pool<MissileController, ProjectileActivationArgs> MissilesMFPool { get; }
        Pool<MissileController, ProjectileActivationArgs> RailSlugsPool { get; }
        Pool<RocketController, ProjectileActivationArgs> MissilesFirecrackerPool { get; }
        Pool<MissileController, ProjectileActivationArgs> MissilesLargePool { get; }
        Pool<SmartMissileController, ProjectileActivationArgs> MissilesSmartPool { get; }

    }
}