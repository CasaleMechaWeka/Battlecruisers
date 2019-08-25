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
        public IPool<ProjectileController, ProjectileActivationArgs<IProjectileStats>> ShellsLargePool { get; }
        public IPool<ProjectileController, ProjectileActivationArgs<IProjectileStats>> ShellsSmallPool { get; }
        public IPool<BombController, ProjectileActivationArgs<IProjectileStats>> BombsPool { get; }
        public IPool<RocketController, TargetProviderActivationArgs<ICruisingProjectileStats>> RocketsPool { get; }
        public IPool<MissileController, TargetProviderActivationArgs<IProjectileStats>> MissilesSmallPool { get; }
        public IPool<MissileController, TargetProviderActivationArgs<IProjectileStats>> MissilesMediumPool { get; }
        public IPool<MissileController, TargetProviderActivationArgs<IProjectileStats>> MissilesLargePool { get; }

        public ProjectilePoolProvider(IFactoryProvider factoryProvider)
        {
            Assert.IsNotNull(factoryProvider);

            BulletsPool
                = new Pool<ProjectileController, ProjectileActivationArgs<IProjectileStats>>(
                    new ProjectileFactory<ProjectileController, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                        factoryProvider,
                        StaticPrefabKeys.Projectiles.Bullet));

            ShellsSmallPool
                = new Pool<ProjectileController, ProjectileActivationArgs<IProjectileStats>>(
                    new ProjectileFactory<ProjectileController, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                        factoryProvider,
                        StaticPrefabKeys.Projectiles.ShellSmall));

            ShellsLargePool
                = new Pool<ProjectileController, ProjectileActivationArgs<IProjectileStats>>(
                    new ProjectileFactory<ProjectileController, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                        factoryProvider,
                        StaticPrefabKeys.Projectiles.ShellLarge));

            BombsPool
                = new Pool<BombController, ProjectileActivationArgs<IProjectileStats>>(
                    new ProjectileFactory<BombController, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                        factoryProvider,
                        StaticPrefabKeys.Projectiles.Bomb));

            RocketsPool
                = new Pool<RocketController, TargetProviderActivationArgs<ICruisingProjectileStats>>(
                    new ProjectileFactory<RocketController, TargetProviderActivationArgs<ICruisingProjectileStats>, ICruisingProjectileStats>(
                        factoryProvider,
                        StaticPrefabKeys.Projectiles.Rocket));

            MissilesSmallPool
                = new Pool<MissileController, TargetProviderActivationArgs<IProjectileStats>>(
                    new ProjectileFactory<MissileController, TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>(
                        factoryProvider,
                        StaticPrefabKeys.Projectiles.MissileSmall));

            MissilesMediumPool
                = new Pool<MissileController, TargetProviderActivationArgs<IProjectileStats>>(
                    new ProjectileFactory<MissileController, TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>(
                        factoryProvider,
                        StaticPrefabKeys.Projectiles.MissileMedium));

            MissilesLargePool
                = new Pool<MissileController, TargetProviderActivationArgs<IProjectileStats>>(
                    new ProjectileFactory<MissileController, TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>(
                        factoryProvider,
                        StaticPrefabKeys.Projectiles.MissileLarge));
        }
    }
}