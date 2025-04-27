using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Projectiles.Pools
{
    public class ProjectileFactory<TProjectile, TActivationArgs, TStats> : IPoolableFactory<TProjectile, TActivationArgs>
        where TActivationArgs : ProjectileActivationArgs<TStats>
        where TProjectile : ProjectileControllerBase<TActivationArgs, TStats>
        where TStats : ProjectileStats
    {
        private readonly ProjectileKey _projectileKey;

        public ProjectileFactory(ProjectileKey projectileKey)
        {
            Helper.AssertIsNotNull(projectileKey);

            _projectileKey = projectileKey;
        }

        public TProjectile CreateItem()
        {
            return PrefabFactory.CreateProjectile<TProjectile, TActivationArgs, TStats>(_projectileKey);
        }

        public override string ToString()
        {
            return $"{nameof(ProjectileFactory<TProjectile, TActivationArgs, TStats>)} {_projectileKey}";
        }
    }
}