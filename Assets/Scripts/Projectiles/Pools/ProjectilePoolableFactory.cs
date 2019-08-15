using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Projectiles.ActivationArgs;
using BattleCruisers.Projectiles.Stats;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Factories;

namespace BattleCruisers.Projectiles.Pools
{
    // FELIX  Rename to ProjectileFactory :)
    public class ProjectilePoolableFactory<TProjectile, TActivationArgs, TStats> : IPoolableFactory<TActivationArgs>
        where TActivationArgs : ProjectileActivationArgs<TStats>
        where TProjectile : ProjectileControllerBase<TActivationArgs, TStats>
        where TStats : IProjectileStats
    {
        private readonly IFactoryProvider _factoryProvider;
        private readonly ProjectileKey _projectileKey;

        public ProjectilePoolableFactory(IFactoryProvider factoryProvider, ProjectileKey projectileKey)
        {
            Helper.AssertIsNotNull(factoryProvider, projectileKey);

            _factoryProvider = factoryProvider;
            _projectileKey = projectileKey;
        }

        public IPoolable<TActivationArgs> CreateItem()
        {
            return _factoryProvider.PrefabFactory.CreateProjectile<TProjectile, TActivationArgs, TStats>(_projectileKey, _factoryProvider);
        }
    }
}