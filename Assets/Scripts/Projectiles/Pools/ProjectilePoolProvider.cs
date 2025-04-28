using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Projectiles.Pools
{
    public class ProjectilePoolProvider : IProjectilePoolProvider
    {
        public Pool<ProjectileController, ProjectileActivationArgs> BulletsPool { get; }
        public Pool<ProjectileController, ProjectileActivationArgs> HighCalibreBulletsPool { get; }
        public Pool<ProjectileController, ProjectileActivationArgs> TinyBulletsPool { get; }
        public Pool<ProjectileController, ProjectileActivationArgs> FlakBulletsPool { get; }
        public Pool<ProjectileController, ProjectileActivationArgs> ShellsLargePool { get; }
        public Pool<ProjectileController, ProjectileActivationArgs> NovaShellPool { get; }
        public Pool<ProjectileController, ProjectileActivationArgs> FiveShellCluster { get; }
        public Pool<ProjectileController, ProjectileActivationArgs> RocketShellPool { get; }
        public Pool<ProjectileController, ProjectileActivationArgs> ShellsSmallPool { get; }
        public Pool<BombController, ProjectileActivationArgs> BombsPool { get; }
        public Pool<BombController, ProjectileActivationArgs> StratBombsPool { get; }
        public Pool<RocketController, ProjectileActivationArgs> RocketsPool { get; }
        public Pool<MissileController, ProjectileActivationArgs> MissilesSmallPool { get; }
        public Pool<RocketController, ProjectileActivationArgs> RocketsSmallPool { get; }
        public Pool<MissileController, ProjectileActivationArgs> MissilesMediumPool { get; }
        public Pool<MissileController, ProjectileActivationArgs> MissilesMFPool { get; }
        public Pool<MissileController, ProjectileActivationArgs> RailSlugsPool { get; }
        public Pool<RocketController, ProjectileActivationArgs> MissilesFirecrackerPool { get; }
        public Pool<MissileController, ProjectileActivationArgs> MissilesLargePool { get; }
        public Pool<SmartMissileController, ProjectileActivationArgs> MissilesSmartPool { get; }

        public ProjectilePoolProvider()
        {
            BulletsPool
                = CreatePool<ProjectileController, ProjectileActivationArgs, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.Bullet,
                    InitialCapacity.BULLET);

            HighCalibreBulletsPool
                = CreatePool<ProjectileController, ProjectileActivationArgs, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.HighCalibreBullet,
                    InitialCapacity.BULLET);

            TinyBulletsPool
                = CreatePool<ProjectileController, ProjectileActivationArgs, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.TinyBullet,
                    InitialCapacity.BULLET);

            FlakBulletsPool
                = CreatePool<ProjectileController, ProjectileActivationArgs, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.FlakBullet,
                    InitialCapacity.BULLET);

            ShellsSmallPool
                = CreatePool<ProjectileController, ProjectileActivationArgs, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.ShellSmall,
                    InitialCapacity.SHELL_SMALL);

            ShellsLargePool
                = CreatePool<ProjectileController, ProjectileActivationArgs, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.ShellLarge,
                    InitialCapacity.SHELL_LARGE);

            NovaShellPool
                = CreatePool<ProjectileController, ProjectileActivationArgs, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.NovaShell,
                    InitialCapacity.SHELL_LARGE);

            FiveShellCluster
                = CreatePool<ProjectileController, ProjectileActivationArgs, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.FiveShellCluster,
                    InitialCapacity.SHELL_LARGE);

            RocketShellPool
                = CreatePool<ProjectileController, ProjectileActivationArgs, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.RocketShell,
                    InitialCapacity.SHELL_LARGE);

            BombsPool
                = CreatePool<BombController, ProjectileActivationArgs, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.Bomb,
                    InitialCapacity.BOMB);

            StratBombsPool
                = CreatePool<BombController, ProjectileActivationArgs, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.StratBomb,
                    InitialCapacity.BOMB);

            RocketsPool
                = CreatePool<RocketController, ProjectileActivationArgs, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.Rocket,
                    InitialCapacity.ROCKET);

            RocketsSmallPool
                = CreatePool<RocketController, ProjectileActivationArgs, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.RocketSmall,
                    InitialCapacity.ROCKET);

            MissilesSmallPool
                = CreatePool<MissileController, ProjectileActivationArgs, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.MissileSmall,
                    InitialCapacity.MISSILE_SMALL);

            MissilesMediumPool
                = CreatePool<MissileController, ProjectileActivationArgs, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.MissileMedium,
                    InitialCapacity.MISSILE_MEDIUM);

            MissilesMFPool
                = CreatePool<MissileController, ProjectileActivationArgs, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.MissileMF,
                    InitialCapacity.MISSILE_MEDIUM);


            RailSlugsPool
                 = CreatePool<MissileController, ProjectileActivationArgs, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.RailSlug,
                    InitialCapacity.MISSILE_MEDIUM);

            MissilesFirecrackerPool
                = CreatePool<RocketController, ProjectileActivationArgs, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.MissileFirecracker,
                    InitialCapacity.MISSILE_MEDIUM);

            MissilesLargePool
                = CreatePool<MissileController, ProjectileActivationArgs, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.MissileLarge,
                    InitialCapacity.MISSILE_LARGE);

            MissilesSmartPool
                = CreatePool<SmartMissileController, ProjectileActivationArgs, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.MissileSmart,
                    InitialCapacity.MISSILE_SMART);
        }

        private Pool<TProjectile, TArgs> CreatePool<TProjectile, TArgs, TStats>(ProjectileKey projectileKey, int initialCapacity)
            where TArgs : ProjectileActivationArgs
            where TProjectile : ProjectileControllerBase<TArgs>
        {
            return
                new Pool<TProjectile, TArgs>(
                    new ProjectileFactory<TProjectile, TArgs>(projectileKey));
        }

        public void SetInitialCapacity()
        {
            BulletsPool.AddCapacity(InitialCapacity.BULLET);
            HighCalibreBulletsPool.AddCapacity(InitialCapacity.BULLET);
            TinyBulletsPool.AddCapacity(InitialCapacity.BULLET);
            FlakBulletsPool.AddCapacity(InitialCapacity.BULLET);
            ShellsSmallPool.AddCapacity(InitialCapacity.SHELL_SMALL);
            ShellsLargePool.AddCapacity(InitialCapacity.SHELL_LARGE);
            NovaShellPool.AddCapacity(InitialCapacity.SHELL_LARGE);
            FiveShellCluster.AddCapacity(InitialCapacity.SHELL_LARGE);
            RocketShellPool.AddCapacity(InitialCapacity.SHELL_LARGE);
            BombsPool.AddCapacity(InitialCapacity.BOMB);
            StratBombsPool.AddCapacity(InitialCapacity.BOMB);
            RocketsPool.AddCapacity(InitialCapacity.ROCKET);
            RocketsSmallPool.AddCapacity(InitialCapacity.ROCKET);
            MissilesSmallPool.AddCapacity(InitialCapacity.MISSILE_SMALL);
            MissilesMediumPool.AddCapacity(InitialCapacity.MISSILE_MEDIUM);
            MissilesFirecrackerPool.AddCapacity(InitialCapacity.MISSILE_MEDIUM);
            MissilesLargePool.AddCapacity(InitialCapacity.MISSILE_LARGE);
            MissilesSmartPool.AddCapacity(InitialCapacity.MISSILE_SMART);
        }
    }
}