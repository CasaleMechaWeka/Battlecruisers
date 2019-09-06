using BattleCruisers.Buildables.Pools;
using BattleCruisers.Effects.Explosions;
using BattleCruisers.Projectiles.Pools;
using BattleCruisers.UI.BattleScene.Manager;

namespace BattleCruisers.Utils.Factories
{
    public class PoolProviders : IPoolProviders
    {
        private ExplosionPoolProvider _explosionPoolProvider;
        public IExplosionPoolProvider ExplosionPoolProvider => _explosionPoolProvider;

        private ProjectilePoolProvider _projectilePoolProvider;
        public IProjectilePoolProvider ProjectilePoolProvider => _projectilePoolProvider;

        private UnitPoolProvider _unitPoolProvider;
        public IUnitPoolProvider UnitPoolProvider => _unitPoolProvider;

        public IUnitToPoolMap UnitToPoolMap { get; }

        public PoolProviders(IFactoryProvider factoryProvider, IUIManager uiManager)
        {
            Helper.AssertIsNotNull(factoryProvider, uiManager);

            _explosionPoolProvider = new ExplosionPoolProvider(factoryProvider.PrefabFactory);
            _projectilePoolProvider = new ProjectilePoolProvider(factoryProvider);
            _unitPoolProvider = new UnitPoolProvider(uiManager, factoryProvider);
            UnitToPoolMap = new UnitToPoolMap(UnitPoolProvider);
        }

        // Not part of constructor, because ProjecilePoolProvider and UnitPollProvider depend on ExplosionPoolProvider :/
        public void SetInitialCapacities()
        {
            _explosionPoolProvider.SetInitialCapacity();
            _projectilePoolProvider.SetInitialCapacity();
            _unitPoolProvider.SetInitialCapacity();
        }
    }
}