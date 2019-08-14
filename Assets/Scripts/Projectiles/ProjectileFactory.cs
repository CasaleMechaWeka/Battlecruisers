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

        public ProjectileController CreateBullet(Vector3 spawnPosition, IFactoryProvider factoryProvider)
        {
            return CreateProjectile<ProjectileController, IProjectileStats>(StaticPrefabKeys.Projectiles.Bullet, spawnPosition, factoryProvider);
        }

        public ProjectileController CreateShellSmall(Vector3 spawnPosition, IFactoryProvider factoryProvider)
        {
            return CreateProjectile<ProjectileController, IProjectileStats>(StaticPrefabKeys.Projectiles.ShellSmall, spawnPosition, factoryProvider);
        }

        public ProjectileController CreateShellLarge(Vector3 spawnPosition, IFactoryProvider factoryProvider)
        {
            return CreateProjectile<ProjectileController, IProjectileStats>(StaticPrefabKeys.Projectiles.ShellLarge, spawnPosition, factoryProvider);
        }

        public BombController CreateBomb(Vector3 spawnPosition, IFactoryProvider factoryProvider)
        {
            return CreateProjectile<BombController, IProjectileStats>(StaticPrefabKeys.Projectiles.Bomb, spawnPosition, factoryProvider);
        }

        public NukeController CreateNuke(Vector3 spawnPosition, IFactoryProvider factoryProvider)
        {
            return CreateProjectile<NukeController, INukeStats>(StaticPrefabKeys.Projectiles.Nuke, spawnPosition, factoryProvider);
        }

        public RocketController CreateRocket(Vector3 spawnPosition, IFactoryProvider factoryProvider)
        {
            return CreateProjectile<RocketController, ICruisingProjectileStats>(StaticPrefabKeys.Projectiles.Rocket, spawnPosition, factoryProvider);
        }

        public MissileController CreateMissileSmall(Vector3 spawnPosition, IFactoryProvider factoryProvider)
        {
            return CreateProjectile<MissileController, IProjectileStats>(StaticPrefabKeys.Projectiles.MissileSmall, spawnPosition, factoryProvider);
        }

        public MissileController CreateMissileMedium(Vector3 spawnPosition, IFactoryProvider factoryProvider)
        {
            return CreateProjectile<MissileController, IProjectileStats>(StaticPrefabKeys.Projectiles.MissileMedium, spawnPosition, factoryProvider);
        }

        public MissileController CreateMissileLarge(Vector3 spawnPosition, IFactoryProvider factoryProvider)
        {
            return CreateProjectile<MissileController, IProjectileStats>(StaticPrefabKeys.Projectiles.MissileLarge, spawnPosition, factoryProvider);
        }

        private TProjectile CreateProjectile<TProjectile, TStats>(ProjectileKey prefabKey, Vector3 spawnPosition, IFactoryProvider factoryProvider)
            where TProjectile : ProjectileControllerBase<TStats>
            where TStats : IProjectileStats
        {
            Assert.IsNotNull(factoryProvider);

            TProjectile prefab = _prefabFetcher.GetPrefab<TProjectile>(prefabKey);
            TProjectile projectile = Object.Instantiate(prefab, spawnPosition, new Quaternion());
            projectile.Initialise(factoryProvider);
            return projectile;
        }
    }
}