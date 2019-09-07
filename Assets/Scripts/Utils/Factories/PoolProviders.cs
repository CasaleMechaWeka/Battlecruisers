using BattleCruisers.Buildables.Pools;
using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Effects;
using BattleCruisers.Effects.Explosions.Pools;
using BattleCruisers.Projectiles.Pools;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils.BattleScene.Pools;
using UnityEngine;

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

        private Pool<IDroneController, Vector2> _dronePool;
        public IPool<IDroneController, Vector2> DronePool => _dronePool;

        public IUnitToPoolMap UnitToPoolMap { get; }

        // 16 per cruiser
        private const int DRONES_INITIAL_CAPACITY = 32;

        public PoolProviders(IFactoryProvider factoryProvider, IUIManager uiManager)
        {
            Helper.AssertIsNotNull(factoryProvider, uiManager);

            _explosionPoolProvider = new ExplosionPoolProvider(factoryProvider.PrefabFactory);
            _projectilePoolProvider = new ProjectilePoolProvider(factoryProvider);
            _unitPoolProvider = new UnitPoolProvider(uiManager, factoryProvider);
            _dronePool
                = new Pool<IDroneController, Vector2>(
                    new DroneFactory(factoryProvider.PrefabFactory));
            UnitToPoolMap = new UnitToPoolMap(UnitPoolProvider);
        }

        // Not part of constructor, because ProjecilePoolProvider and UnitPollProvider depend on ExplosionPoolProvider :/
        public void SetInitialCapacities()
        {
            _explosionPoolProvider.SetInitialCapacity();
            _projectilePoolProvider.SetInitialCapacity();
            _unitPoolProvider.SetInitialCapacity();
            _dronePool.AddCapacity(DRONES_INITIAL_CAPACITY);
        }
    }
}