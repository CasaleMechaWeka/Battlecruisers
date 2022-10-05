using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Factories;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles.Pools
{
    public class ProjectilePoolProvider : IProjectilePoolProvider
    {
        public IPool<ProjectileController, ProjectileActivationArgs<IProjectileStats>> BulletsPool { get; }
        public IPool<ProjectileController, ProjectileActivationArgs<IProjectileStats>> HighCalibreBulletsPool { get; }
        public IPool<ProjectileController, ProjectileActivationArgs<IProjectileStats>> TinyBulletsPool { get; }
        public IPool<ProjectileController, ProjectileActivationArgs<IProjectileStats>> ShellsLargePool { get; }
        public IPool<ProjectileController, ProjectileActivationArgs<IProjectileStats>> ShellsSmallPool { get; }
        public IPool<ProjectileController, ProjectileActivationArgs<IProjectileStats>> NovaShellPool { get; }
        public IPool<BombController, ProjectileActivationArgs<IProjectileStats>> BombsPool { get; }
        public IPool<RocketController, TargetProviderActivationArgs<ICruisingProjectileStats>> RocketsPool { get; }
        public IPool<MissileController, TargetProviderActivationArgs<IProjectileStats>> MissilesSmallPool { get; }
        public IPool<RocketController, TargetProviderActivationArgs<ICruisingProjectileStats>> RocketsSmallPool { get; }
        public IPool<MissileController, TargetProviderActivationArgs<IProjectileStats>> MissilesMediumPool { get; }
        public IPool<MissileController, TargetProviderActivationArgs<IProjectileStats>> MissilesLargePool { get; }
        public IPool<SmartMissileController, SmartMissileActivationArgs<ISmartProjectileStats>> MissilesSmartPool { get; }

        public ProjectilePoolProvider(IFactoryProvider factoryProvider)
        {
            Assert.IsNotNull(factoryProvider);

            BulletsPool
                = CreatePool<ProjectileController, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                    factoryProvider,
                    StaticPrefabKeys.Projectiles.Bullet,
                    InitialCapacity.BULLET);

            HighCalibreBulletsPool
                = CreatePool<ProjectileController, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                    factoryProvider,
                    StaticPrefabKeys.Projectiles.HighCalibreBullet,
                    InitialCapacity.BULLET);

             TinyBulletsPool
                = CreatePool<ProjectileController, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                    factoryProvider,
                    StaticPrefabKeys.Projectiles.TinyBullet,
                    InitialCapacity.BULLET);

            ShellsSmallPool
                = CreatePool<ProjectileController, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                    factoryProvider,
                    StaticPrefabKeys.Projectiles.ShellSmall,
                    InitialCapacity.SHELL_SMALL);

            ShellsLargePool
                = CreatePool<ProjectileController, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                    factoryProvider,
                    StaticPrefabKeys.Projectiles.ShellLarge,
                    InitialCapacity.SHELL_LARGE);

            NovaShellPool
                = CreatePool<ProjectileController, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                    factoryProvider,
                    StaticPrefabKeys.Projectiles.NovaShell,
                    InitialCapacity.SHELL_LARGE);
                

            BombsPool
                = CreatePool<BombController, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                    factoryProvider,
                    StaticPrefabKeys.Projectiles.Bomb,
                    InitialCapacity.BOMB);

            RocketsPool
                = CreatePool<RocketController, TargetProviderActivationArgs<ICruisingProjectileStats>, ICruisingProjectileStats>(
                    factoryProvider,
                    StaticPrefabKeys.Projectiles.Rocket,
                    InitialCapacity.ROCKET);

            RocketsSmallPool
                = CreatePool<RocketController, TargetProviderActivationArgs<ICruisingProjectileStats>, ICruisingProjectileStats>(
                    factoryProvider,
                    StaticPrefabKeys.Projectiles.RocketSmall,
                    InitialCapacity.ROCKET);

            MissilesSmallPool
                = CreatePool<MissileController, TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>(
                    factoryProvider,
                    StaticPrefabKeys.Projectiles.MissileSmall,
                    InitialCapacity.MISSILE_SMALL);

            MissilesMediumPool
                = CreatePool<MissileController, TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>(
                    factoryProvider,
                    StaticPrefabKeys.Projectiles.MissileMedium,
                    InitialCapacity.MISSILE_MEDIUM);

            MissilesLargePool
                = CreatePool<MissileController, TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>(
                    factoryProvider,
                    StaticPrefabKeys.Projectiles.MissileLarge,
                    InitialCapacity.MISSILE_LARGE);

            MissilesSmartPool
                = CreatePool<SmartMissileController, SmartMissileActivationArgs<ISmartProjectileStats>, ISmartProjectileStats>(
                    factoryProvider,
                    StaticPrefabKeys.Projectiles.MissileSmart,
                    InitialCapacity.MISSILE_SMART);

                
        }

        private IPool<TProjectile, TArgs> CreatePool<TProjectile, TArgs, TStats>(IFactoryProvider factoryProvider, ProjectileKey projectileKey, int initialCapacity)
            where TArgs: ProjectileActivationArgs<TStats>
            where TProjectile : ProjectileControllerBase<TArgs, TStats>
            where TStats : IProjectileStats
        {
            return
                new Pool<TProjectile, TArgs>(
                    new ProjectileFactory<TProjectile, TArgs, TStats>(
                        factoryProvider,
                        projectileKey));
        }

        public void SetInitialCapacity()
        {
            BulletsPool.AddCapacity(InitialCapacity.BULLET);
            HighCalibreBulletsPool.AddCapacity(InitialCapacity.BULLET);
            TinyBulletsPool.AddCapacity(InitialCapacity.BULLET);
            ShellsSmallPool.AddCapacity(InitialCapacity.SHELL_SMALL);
            ShellsLargePool.AddCapacity(InitialCapacity.SHELL_LARGE);
            NovaShellPool.AddCapacity(InitialCapacity.SHELL_LARGE);
            BombsPool.AddCapacity(InitialCapacity.BOMB);
            RocketsPool.AddCapacity(InitialCapacity.ROCKET);
            RocketsSmallPool.AddCapacity(InitialCapacity.ROCKET);
            MissilesSmallPool.AddCapacity(InitialCapacity.MISSILE_SMALL);
            MissilesMediumPool.AddCapacity(InitialCapacity.MISSILE_MEDIUM);
            MissilesLargePool.AddCapacity(InitialCapacity.MISSILE_LARGE);
            MissilesSmartPool.AddCapacity(InitialCapacity.MISSILE_SMART);
        }
    }
}