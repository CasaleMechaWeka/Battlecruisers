using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Fetchers;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Projectiles
{
    public class ProjectileFactory : IProjectileFactory
    {
        private readonly PrefabFetcher _prefabFetcher;

        public ProjectileFactory(PrefabFetcher prefabFetcher)
        {
            Assert.IsNotNull(prefabFetcher);
            _prefabFetcher = prefabFetcher;
        }

        public ProjectileController CreateBullet(IFactoryProvider factoryProvider)
        {
            return CreateProjectile<ProjectileController, IProjectileStats>(StaticPrefabKeys.Projectiles.Bullet, factoryProvider);
        }

        public ProjectileController CreateShellSmall(IFactoryProvider factoryProvider)
        {
            return CreateProjectile<ProjectileController, IProjectileStats>(StaticPrefabKeys.Projectiles.ShellSmall, factoryProvider);
        }

        public ProjectileController CreateShellLarge(IFactoryProvider factoryProvider)
        {
            return CreateProjectile<ProjectileController, IProjectileStats>(StaticPrefabKeys.Projectiles.ShellLarge, factoryProvider);
        }

        public BombController CreateBomb(IFactoryProvider factoryProvider)
        {
            return CreateProjectile<BombController, IProjectileStats>(StaticPrefabKeys.Projectiles.Bomb, factoryProvider);
        }

        public NukeController CreateNuke(IFactoryProvider factoryProvider)
        {
            return CreateProjectile<NukeController, INukeStats>(StaticPrefabKeys.Projectiles.Nuke, factoryProvider);
        }

        public RocketController CreateRocket(IFactoryProvider factoryProvider)
        {
            return CreateProjectile<RocketController, ICruisingProjectileStats>(StaticPrefabKeys.Projectiles.Rocket, factoryProvider);
        }

        public MissileController CreateMissileSmall(IFactoryProvider factoryProvider)
        {
            return CreateProjectile<MissileController, IProjectileStats>(StaticPrefabKeys.Projectiles.MissileSmall, factoryProvider);
        }

        public MissileController CreateMissileMedium(IFactoryProvider factoryProvider)
        {
            return CreateProjectile<MissileController, IProjectileStats>(StaticPrefabKeys.Projectiles.MissileMedium, factoryProvider);
        }

        public MissileController CreateMissileLarge(IFactoryProvider factoryProvider)
        {
            return CreateProjectile<MissileController, IProjectileStats>(StaticPrefabKeys.Projectiles.MissileLarge, factoryProvider);
        }

        // FELIX  Remove spawnPosition from everywhere :P
        private TProjectile CreateProjectile<TProjectile, TStats>(ProjectileKey prefabKey, IFactoryProvider factoryProvider)
            where TProjectile : ProjectileControllerBase<TStats>
            where TStats : IProjectileStats
        {
            Assert.IsNotNull(factoryProvider);

            TProjectile prefab = _prefabFetcher.GetPrefab<TProjectile>(prefabKey);
            TProjectile projectile = Object.Instantiate(prefab);
            projectile.Initialise(factoryProvider);
            return projectile;
        }
    }
}