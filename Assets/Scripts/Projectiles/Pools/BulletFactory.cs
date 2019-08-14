using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Projectiles.Pools
{
    public class BulletFactory : IPoolableFactory<ProjectileActivationArgs<ProjectileStats>>
    {
        private readonly IProjectileFactory _projectileFactory;

        // FELIX  Constructor :/

        public IPoolable<ProjectileActivationArgs<ProjectileStats>> CreateItem()
        {
            // FELIX  Dang :/
            //_projectileFactory.CreateBullet()
                return null;
        }
    }
}