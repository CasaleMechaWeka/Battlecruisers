using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Projectiles.Pools
{
    public class ProjectilePoolProvider : IProjectilePoolProvider
    {
        public Pool<ProjectileController, ProjectileActivationArgs<ProjectileStats>> BulletsPool { get; }
        public Pool<ProjectileController, ProjectileActivationArgs<ProjectileStats>> HighCalibreBulletsPool { get; }
        public Pool<ProjectileController, ProjectileActivationArgs<ProjectileStats>> TinyBulletsPool { get; }
        public Pool<ProjectileController, ProjectileActivationArgs<ProjectileStats>> FlakBulletsPool { get; }
        public Pool<ProjectileController, ProjectileActivationArgs<ProjectileStats>> ShellsLargePool { get; }
        public Pool<ProjectileController, ProjectileActivationArgs<ProjectileStats>> NovaShellPool { get; }
        public Pool<ProjectileController, ProjectileActivationArgs<ProjectileStats>> FiveShellCluster { get; }
        public Pool<ProjectileController, ProjectileActivationArgs<ProjectileStats>> RocketShellPool { get; }
        public Pool<ProjectileController, ProjectileActivationArgs<ProjectileStats>> ShellsSmallPool { get; }
        public Pool<BombController, ProjectileActivationArgs<ProjectileStats>> BombsPool { get; }
        public Pool<BombController, ProjectileActivationArgs<ProjectileStats>> StratBombsPool { get; }
        public Pool<RocketController, TargetProviderActivationArgs<ProjectileStats>> RocketsPool { get; }
        public Pool<MissileController, TargetProviderActivationArgs<ProjectileStats>> MissilesSmallPool { get; }
        public Pool<RocketController, TargetProviderActivationArgs<ProjectileStats>> RocketsSmallPool { get; }
        public Pool<MissileController, TargetProviderActivationArgs<ProjectileStats>> MissilesMediumPool { get; }
        public Pool<MissileController, TargetProviderActivationArgs<ProjectileStats>> MissilesMFPool { get; }
        public Pool<MissileController, TargetProviderActivationArgs<ProjectileStats>> RailSlugsPool { get; }
        public Pool<RocketController, TargetProviderActivationArgs<ProjectileStats>> MissilesFirecrackerPool { get; }
        public Pool<MissileController, TargetProviderActivationArgs<ProjectileStats>> MissilesLargePool { get; }
        public Pool<SmartMissileController, SmartMissileActivationArgs<ProjectileStats>> MissilesSmartPool { get; }

        public ProjectilePoolProvider()
        {
            BulletsPool
                = CreatePool<ProjectileController, ProjectileActivationArgs<ProjectileStats>, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.Bullet,
                    InitialCapacity.BULLET);

            HighCalibreBulletsPool
                = CreatePool<ProjectileController, ProjectileActivationArgs<ProjectileStats>, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.HighCalibreBullet,
                    InitialCapacity.BULLET);

            TinyBulletsPool
                = CreatePool<ProjectileController, ProjectileActivationArgs<ProjectileStats>, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.TinyBullet,
                    InitialCapacity.BULLET);

            FlakBulletsPool
                = CreatePool<ProjectileController, ProjectileActivationArgs<ProjectileStats>, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.FlakBullet,
                    InitialCapacity.BULLET);

            ShellsSmallPool
                = CreatePool<ProjectileController, ProjectileActivationArgs<ProjectileStats>, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.ShellSmall,
                    InitialCapacity.SHELL_SMALL);

            ShellsLargePool
                = CreatePool<ProjectileController, ProjectileActivationArgs<ProjectileStats>, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.ShellLarge,
                    InitialCapacity.SHELL_LARGE);

            NovaShellPool
                = CreatePool<ProjectileController, ProjectileActivationArgs<ProjectileStats>, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.NovaShell,
                    InitialCapacity.SHELL_LARGE);

            FiveShellCluster
                = CreatePool<ProjectileController, ProjectileActivationArgs<ProjectileStats>, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.FiveShellCluster,
                    InitialCapacity.SHELL_LARGE);

            RocketShellPool
                = CreatePool<ProjectileController, ProjectileActivationArgs<ProjectileStats>, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.RocketShell,
                    InitialCapacity.SHELL_LARGE);

            BombsPool
                = CreatePool<BombController, ProjectileActivationArgs<ProjectileStats>, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.Bomb,
                    InitialCapacity.BOMB);

            StratBombsPool
                = CreatePool<BombController, ProjectileActivationArgs<ProjectileStats>, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.StratBomb,
                    InitialCapacity.BOMB);

            RocketsPool
                = CreatePool<RocketController, TargetProviderActivationArgs<ProjectileStats>, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.Rocket,
                    InitialCapacity.ROCKET);

            RocketsSmallPool
                = CreatePool<RocketController, TargetProviderActivationArgs<ProjectileStats>, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.RocketSmall,
                    InitialCapacity.ROCKET);

            MissilesSmallPool
                = CreatePool<MissileController, TargetProviderActivationArgs<ProjectileStats>, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.MissileSmall,
                    InitialCapacity.MISSILE_SMALL);

            MissilesMediumPool
                = CreatePool<MissileController, TargetProviderActivationArgs<ProjectileStats>, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.MissileMedium,
                    InitialCapacity.MISSILE_MEDIUM);

            MissilesMFPool
                = CreatePool<MissileController, TargetProviderActivationArgs<ProjectileStats>, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.MissileMF,
                    InitialCapacity.MISSILE_MEDIUM);


            RailSlugsPool
                 = CreatePool<MissileController, TargetProviderActivationArgs<ProjectileStats>, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.RailSlug,
                    InitialCapacity.MISSILE_MEDIUM);

            MissilesFirecrackerPool
                = CreatePool<RocketController, TargetProviderActivationArgs<ProjectileStats>, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.MissileFirecracker,
                    InitialCapacity.MISSILE_MEDIUM);

            MissilesLargePool
                = CreatePool<MissileController, TargetProviderActivationArgs<ProjectileStats>, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.MissileLarge,
                    InitialCapacity.MISSILE_LARGE);

            MissilesSmartPool
                = CreatePool<SmartMissileController, SmartMissileActivationArgs<ProjectileStats>, ProjectileStats>(
                    StaticPrefabKeys.Projectiles.MissileSmart,
                    InitialCapacity.MISSILE_SMART);
        }

        private Pool<TProjectile, TArgs> CreatePool<TProjectile, TArgs, TStats>(ProjectileKey projectileKey, int initialCapacity)
            where TArgs : ProjectileActivationArgs<TStats>
            where TProjectile : ProjectileControllerBase<TArgs, TStats>
            where TStats : ProjectileStats
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