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
        public IPool<ProjectileControllerBase<ProjectileActivationArgs<IProjectileStats>, IProjectileStats>, ProjectileActivationArgs<IProjectileStats>> BulletsPool { get; }
        public IPool<ProjectileControllerBase<ProjectileActivationArgs<IProjectileStats>, IProjectileStats>, ProjectileActivationArgs<IProjectileStats>> ShellsLargePool { get; }
        public IPool<ProjectileControllerBase<ProjectileActivationArgs<IProjectileStats>, IProjectileStats>, ProjectileActivationArgs<IProjectileStats>> ShellsSmallPool { get; }
        public IPool<ProjectileControllerBase<ProjectileActivationArgs<IProjectileStats>, IProjectileStats>, ProjectileActivationArgs<IProjectileStats>> BombsPool { get; }
        public IPool<ProjectileControllerBase<TargetProviderActivationArgs<ICruisingProjectileStats>, ICruisingProjectileStats>, TargetProviderActivationArgs<ICruisingProjectileStats>> RocketsPool { get; }
        public IPool<ProjectileControllerBase<TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>, TargetProviderActivationArgs<IProjectileStats>> MissilesSmallPool { get; }
        public IPool<ProjectileControllerBase<TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>, TargetProviderActivationArgs<IProjectileStats>> MissilesMediumPool { get; }
        public IPool<ProjectileControllerBase<TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>, TargetProviderActivationArgs<IProjectileStats>> MissilesLargePool { get; }

        public ProjectilePoolProvider(IFactoryProvider factoryProvider)
        {
            Assert.IsNotNull(factoryProvider);

            BulletsPool
                = new Pool<ProjectileControllerBase<ProjectileActivationArgs<IProjectileStats>, IProjectileStats>, ProjectileActivationArgs<IProjectileStats>>(
                    new ProjectileFactory<ProjectileControllerBase<ProjectileActivationArgs<IProjectileStats>, IProjectileStats>, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                        factoryProvider,
                        StaticPrefabKeys.Projectiles.Bullet));

            ShellsSmallPool
                = new Pool<ProjectileControllerBase<ProjectileActivationArgs<IProjectileStats>, IProjectileStats>, ProjectileActivationArgs<IProjectileStats>>(
                    new ProjectileFactory<ProjectileControllerBase<ProjectileActivationArgs<IProjectileStats>, IProjectileStats>, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                        factoryProvider,
                        StaticPrefabKeys.Projectiles.ShellSmall));

            ShellsLargePool
                = new Pool<ProjectileControllerBase<ProjectileActivationArgs<IProjectileStats>, IProjectileStats>, ProjectileActivationArgs<IProjectileStats>>(
                    new ProjectileFactory<ProjectileControllerBase<ProjectileActivationArgs<IProjectileStats>, IProjectileStats>, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                        factoryProvider,
                        StaticPrefabKeys.Projectiles.ShellLarge));

            BombsPool
                = new Pool<ProjectileControllerBase<ProjectileActivationArgs<IProjectileStats>, IProjectileStats>, ProjectileActivationArgs<IProjectileStats>>(
                    new ProjectileFactory<ProjectileControllerBase<ProjectileActivationArgs<IProjectileStats>, IProjectileStats>, ProjectileActivationArgs<IProjectileStats>, IProjectileStats>(
                        factoryProvider,
                        StaticPrefabKeys.Projectiles.Bomb));

            RocketsPool
                = new Pool<ProjectileControllerBase<TargetProviderActivationArgs<ICruisingProjectileStats>, ICruisingProjectileStats>, TargetProviderActivationArgs<ICruisingProjectileStats>>(
                    new ProjectileFactory<ProjectileControllerBase<TargetProviderActivationArgs<ICruisingProjectileStats>, ICruisingProjectileStats>, TargetProviderActivationArgs<ICruisingProjectileStats>, ICruisingProjectileStats>(
                        factoryProvider,
                        StaticPrefabKeys.Projectiles.Rocket));

            MissilesSmallPool
                = new Pool<ProjectileControllerBase<TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>, TargetProviderActivationArgs<IProjectileStats>>(
                    new ProjectileFactory<ProjectileControllerBase<TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>, TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>(
                        factoryProvider,
                        StaticPrefabKeys.Projectiles.MissileSmall));

            MissilesMediumPool
                = new Pool<ProjectileControllerBase<TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>, TargetProviderActivationArgs<IProjectileStats>>(
                    new ProjectileFactory<ProjectileControllerBase<TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>, TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>(
                        factoryProvider,
                        StaticPrefabKeys.Projectiles.MissileMedium));

            MissilesLargePool
                = new Pool<ProjectileControllerBase<TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>, TargetProviderActivationArgs<IProjectileStats>>(
                    new ProjectileFactory<ProjectileControllerBase<TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>, TargetProviderActivationArgs<IProjectileStats>, IProjectileStats>(
                        factoryProvider,
                        StaticPrefabKeys.Projectiles.MissileLarge));
        }
    }
}