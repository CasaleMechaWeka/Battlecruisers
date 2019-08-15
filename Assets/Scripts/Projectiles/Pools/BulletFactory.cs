//// FELIX  Remove :)
//using BattleCruisers.Projectiles.ActivationArgs;
//using BattleCruisers.Projectiles.Stats;
//using BattleCruisers.Utils.BattleScene.Pools;

//namespace BattleCruisers.Projectiles.Pools
//{
//    public class BulletFactory : IPoolableFactory<ProjectileActivationArgs<IProjectileStats>>
//    {
//        private readonly IProjectileFactory _projectileFactory;

//        // FELIX  Constructor :/

//        public IPoolable<ProjectileActivationArgs<IProjectileStats>> CreateItem()
//        {
//            return _projectileFactory.CreateBullet();
//        }
//    }
//}