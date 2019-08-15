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
        public IPool<ProjectileActivationArgs<IProjectileStats>> BulletsPool { get; }
        public IPool<ProjectileActivationArgs<IProjectileStats>> ShellsSmallPool { get; }
        public IPool<ProjectileActivationArgs<IProjectileStats>> ShellsLargePool { get; }
        public IPool<ProjectileActivationArgs<IProjectileStats>> BombsPool { get; }
        public IPool<TargetProviderActivationArgs<ICruisingProjectileStats>> RocketsPool { get; }
        public IPool<TargetProviderActivationArgs<IProjectileStats>> MissilesSmallPool { get; }
        public IPool<TargetProviderActivationArgs<IProjectileStats>> MissilesMediumPool { get; }
        public IPool<TargetProviderActivationArgs<IProjectileStats>> MissilesLargePool { get; }

        public ProjectilePoolProvider(IFactoryProvider factoryProvider)
        {
            Assert.IsNotNull(factoryProvider);

            BulletsPool
                = new Pool<ProjectileActivationArgs<IProjectileStats>>(
                    new ProjectileFactory<ProjectileController, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                        factoryProvider,
                        StaticPrefabKeys.Projectiles.Bullet));

            ShellsSmallPool
                = new Pool<ProjectileActivationArgs<IProjectileStats>>(
                    new ProjectileFactory<ProjectileController, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                        factoryProvider,
                        StaticPrefabKeys.Projectiles.ShellSmall));

            ShellsLargePool
                = new Pool<ProjectileActivationArgs<IProjectileStats>>(
                    new ProjectileFactory<ProjectileController, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                        factoryProvider,
                        StaticPrefabKeys.Projectiles.ShellLarge));

            BombsPool
                = new Pool<ProjectileActivationArgs<IProjectileStats>>(
                    new ProjectileFactory<BombController, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                        factoryProvider,
                        StaticPrefabKeys.Projectiles.Bomb));

            RocketsPool
                = new Pool<TargetProviderActivationArgs<ICruisingProjectileStats>>(
                    new ProjectileFactory<RocketController, TargetProviderActivationArgs<ICruisingProjectileStats>, ICruisingProjectileStats>(
                        factoryProvider,
                        StaticPrefabKeys.Projectiles.Rocket));

            MissilesSmallPool
                = new Pool<TargetProviderActivationArgs<IProjectileStats>>(
                    new ProjectileFactory<MissileController, TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>(
                        factoryProvider,
                        StaticPrefabKeys.Projectiles.MissileSmall));

            MissilesMediumPool
                = new Pool<TargetProviderActivationArgs<IProjectileStats>>(
                    new ProjectileFactory<MissileController, TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>(
                        factoryProvider,
                        StaticPrefabKeys.Projectiles.MissileMedium));

            MissilesLargePool
                = new Pool<TargetProviderActivationArgs<IProjectileStats>>(
                    new ProjectileFactory<MissileController, TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>(
                        factoryProvider,
                        StaticPrefabKeys.Projectiles.MissileLarge));
        }
    }
}