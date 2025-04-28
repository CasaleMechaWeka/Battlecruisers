using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Projectiles.Pools
{
    public class ProjectileFactory<TProjectile, TActivationArgs> : IPoolableFactory<TProjectile, TActivationArgs>
        where TActivationArgs : ProjectileActivationArgs
        where TProjectile : ProjectileControllerBase<TActivationArgs>
    {
        private readonly ProjectileKey _projectileKey;

        public ProjectileFactory(ProjectileKey projectileKey)
        {
            Helper.AssertIsNotNull(projectileKey);

            _projectileKey = projectileKey;
        }

        public TProjectile CreateItem()
        {
            return PrefabFactory.CreateProjectile<TProjectile, TActivationArgs>(_projectileKey);
        }

        public override string ToString()
        {
            return $"{nameof(ProjectileFactory<TProjectile, TActivationArgs>)} {_projectileKey}";
        }
    }
}