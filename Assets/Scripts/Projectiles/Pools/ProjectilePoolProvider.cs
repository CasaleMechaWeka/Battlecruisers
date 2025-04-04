using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Projectiles.Pools
{
    public class ProjectilePoolProvider : IProjectilePoolProvider
    {
        public Pool<ProjectileController, ProjectileActivationArgs<IProjectileStats>> BulletsPool { get; }
        public Pool<ProjectileController, ProjectileActivationArgs<IProjectileStats>> HighCalibreBulletsPool { get; }
        public Pool<ProjectileController, ProjectileActivationArgs<IProjectileStats>> TinyBulletsPool { get; }
        public Pool<ProjectileController, ProjectileActivationArgs<IProjectileStats>> FlakBulletsPool { get; }
        public Pool<ProjectileController, ProjectileActivationArgs<IProjectileStats>> ShellsLargePool { get; }
        public Pool<ProjectileController, ProjectileActivationArgs<IProjectileStats>> NovaShellPool { get; }
        public Pool<ProjectileController, ProjectileActivationArgs<IProjectileStats>> FiveShellCluster { get; }
        public Pool<ProjectileController, ProjectileActivationArgs<IProjectileStats>> RocketShellPool { get; }
        public Pool<ProjectileController, ProjectileActivationArgs<IProjectileStats>> ShellsSmallPool { get; }
        public Pool<BombController, ProjectileActivationArgs<IProjectileStats>> BombsPool { get; }
        public Pool<BombController, ProjectileActivationArgs<IProjectileStats>> StratBombsPool { get; }
        public Pool<RocketController, TargetProviderActivationArgs<ICruisingProjectileStats>> RocketsPool { get; }
        public Pool<MissileController, TargetProviderActivationArgs<IProjectileStats>> MissilesSmallPool { get; }
        public Pool<RocketController, TargetProviderActivationArgs<ICruisingProjectileStats>> RocketsSmallPool { get; }
        public Pool<MissileController, TargetProviderActivationArgs<IProjectileStats>> MissilesMediumPool { get; }
        public Pool<MissileController, TargetProviderActivationArgs<IProjectileStats>> MissilesMFPool { get; }
        public Pool<MissileController, TargetProviderActivationArgs<IProjectileStats>> RailSlugsPool { get; }
        public Pool<RocketController, TargetProviderActivationArgs<ICruisingProjectileStats>> MissilesFirecrackerPool { get; }
        public Pool<MissileController, TargetProviderActivationArgs<IProjectileStats>> MissilesLargePool { get; }
        public Pool<SmartMissileController, SmartMissileActivationArgs<ISmartProjectileStats>> MissilesSmartPool { get; }

        public ProjectilePoolProvider()
        {
            BulletsPool
                = CreatePool<ProjectileController, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                    StaticPrefabKeys.Projectiles.Bullet,
                    InitialCapacity.BULLET);

            HighCalibreBulletsPool
                = CreatePool<ProjectileController, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                    StaticPrefabKeys.Projectiles.HighCalibreBullet,
                    InitialCapacity.BULLET);

            TinyBulletsPool
                = CreatePool<ProjectileController, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                    StaticPrefabKeys.Projectiles.TinyBullet,
                    InitialCapacity.BULLET);

            FlakBulletsPool
                = CreatePool<ProjectileController, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                    StaticPrefabKeys.Projectiles.FlakBullet,
                    InitialCapacity.BULLET);

            ShellsSmallPool
                = CreatePool<ProjectileController, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                    StaticPrefabKeys.Projectiles.ShellSmall,
                    InitialCapacity.SHELL_SMALL);

            ShellsLargePool
                = CreatePool<ProjectileController, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                    StaticPrefabKeys.Projectiles.ShellLarge,
                    InitialCapacity.SHELL_LARGE);

            NovaShellPool
                = CreatePool<ProjectileController, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                    StaticPrefabKeys.Projectiles.NovaShell,
                    InitialCapacity.SHELL_LARGE);

            FiveShellCluster
                = CreatePool<ProjectileController, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                    StaticPrefabKeys.Projectiles.FiveShellCluster,
                    InitialCapacity.SHELL_LARGE);

            RocketShellPool
                = CreatePool<ProjectileController, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                    StaticPrefabKeys.Projectiles.RocketShell,
                    InitialCapacity.SHELL_LARGE);

            BombsPool
                = CreatePool<BombController, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                    StaticPrefabKeys.Projectiles.Bomb,
                    InitialCapacity.BOMB);

            StratBombsPool
                = CreatePool<BombController, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                    StaticPrefabKeys.Projectiles.StratBomb,
                    InitialCapacity.BOMB);

            RocketsPool
                = CreatePool<RocketController, TargetProviderActivationArgs<ICruisingProjectileStats>, ICruisingProjectileStats>(
                    StaticPrefabKeys.Projectiles.Rocket,
                    InitialCapacity.ROCKET);

            RocketsSmallPool
                = CreatePool<RocketController, TargetProviderActivationArgs<ICruisingProjectileStats>, ICruisingProjectileStats>(
                    StaticPrefabKeys.Projectiles.RocketSmall,
                    InitialCapacity.ROCKET);

            MissilesSmallPool
                = CreatePool<MissileController, TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>(
                    StaticPrefabKeys.Projectiles.MissileSmall,
                    InitialCapacity.MISSILE_SMALL);

            MissilesMediumPool
                = CreatePool<MissileController, TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>(
                    StaticPrefabKeys.Projectiles.MissileMedium,
                    InitialCapacity.MISSILE_MEDIUM);

            MissilesMFPool
                = CreatePool<MissileController, TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>(
                    StaticPrefabKeys.Projectiles.MissileMF,
                    InitialCapacity.MISSILE_MEDIUM);


            RailSlugsPool
                 = CreatePool<MissileController, TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>(
                    StaticPrefabKeys.Projectiles.RailSlug,
                    InitialCapacity.MISSILE_MEDIUM);

            MissilesFirecrackerPool
                = CreatePool<RocketController, TargetProviderActivationArgs<ICruisingProjectileStats>, ICruisingProjectileStats>(
                    StaticPrefabKeys.Projectiles.MissileFirecracker,
                    InitialCapacity.MISSILE_MEDIUM);

            MissilesLargePool
                = CreatePool<MissileController, TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>(
                    StaticPrefabKeys.Projectiles.MissileLarge,
                    InitialCapacity.MISSILE_LARGE);

            MissilesSmartPool
                = CreatePool<SmartMissileController, SmartMissileActivationArgs<ISmartProjectileStats>, ISmartProjectileStats>(
                    StaticPrefabKeys.Projectiles.MissileSmart,
                    InitialCapacity.MISSILE_SMART);
        }

        private Pool<TProjectile, TArgs> CreatePool<TProjectile, TArgs, TStats>(ProjectileKey projectileKey, int initialCapacity)
            where TArgs : ProjectileActivationArgs<TStats>
            where TProjectile : ProjectileControllerBase<TArgs, TStats>
            where TStats : IProjectileStats
        {
            return
                new Pool<TProjectile, TArgs>(
                    new ProjectileFactory<TProjectile, TArgs, TStats>(projectileKey));
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