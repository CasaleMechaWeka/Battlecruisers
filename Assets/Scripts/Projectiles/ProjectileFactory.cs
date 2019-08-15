//using BattleCruisers.Data.Static;
//using BattleCruisers.Projectiles.Stats;
//using BattleCruisers.Utils.Factories;
//using UnityEngine.Assertions;

//namespace BattleCruisers.Projectiles
//{
//    // FELIX  Delete :)
//    public class ProjectileFactory : IProjectileFactory
//    {
//        private readonly IFactoryProvider _factoryProvider;

//        public ProjectileFactory(IFactoryProvider factoryProvider)
//        {
//            Assert.IsNotNull(factoryProvider);
//            _factoryProvider = factoryProvider;
//        }

//        public ProjectileController CreateBullet()
//        {
//            return _factoryProvider.PrefabFactory.CreateProjectile<ProjectileController, IProjectileStats>(StaticPrefabKeys.Projectiles.Bullet, _factoryProvider);
//        }

//        public ProjectileController CreateShellSmall()
//        {
//            return _factoryProvider.PrefabFactory.CreateProjectile<ProjectileController, IProjectileStats>(StaticPrefabKeys.Projectiles.ShellSmall, _factoryProvider);
//        }

//        public ProjectileController CreateShellLarge()
//        {
//            return _factoryProvider.PrefabFactory.CreateProjectile<ProjectileController, IProjectileStats>(StaticPrefabKeys.Projectiles.ShellLarge, _factoryProvider);
//        }

//        public BombController CreateBomb()
//        {
//            return _factoryProvider.PrefabFactory.CreateProjectile<BombController, IProjectileStats>(StaticPrefabKeys.Projectiles.Bomb, _factoryProvider);
//        }

//        public NukeController CreateNuke()
//        {
//            return _factoryProvider.PrefabFactory.CreateProjectile<NukeController, INukeStats>(StaticPrefabKeys.Projectiles.Nuke, _factoryProvider);
//        }

//        public RocketController CreateRocket()
//        {
//            return _factoryProvider.PrefabFactory.CreateProjectile<RocketController, ICruisingProjectileStats>(StaticPrefabKeys.Projectiles.Rocket, _factoryProvider);
//        }

//        public MissileController CreateMissileSmall()
//        {
//            return _factoryProvider.PrefabFactory.CreateProjectile<MissileController, IProjectileStats>(StaticPrefabKeys.Projectiles.MissileSmall, _factoryProvider);
//        }

//        public MissileController CreateMissileMedium()
//        {
//            return _factoryProvider.PrefabFactory.CreateProjectile<MissileController, IProjectileStats>(StaticPrefabKeys.Projectiles.MissileMedium, _factoryProvider);
//        }

//        public MissileController CreateMissileLarge()
//        {
//            return _factoryProvider.PrefabFactory.CreateProjectile<MissileController, IProjectileStats>(StaticPrefabKeys.Projectiles.MissileLarge, _factoryProvider);
//        }
//    }
//}