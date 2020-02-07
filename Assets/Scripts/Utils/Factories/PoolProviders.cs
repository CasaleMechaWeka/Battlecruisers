using BattleCruisers.Buildables.Pools;
using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Effects.Deaths.Pools;
using BattleCruisers.Effects.Drones;
using BattleCruisers.Effects.Explosions.Pools;
using BattleCruisers.Projectiles.Pools;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils.BattleScene.Pools;

namespace BattleCruisers.Utils.Factories
{
    public class PoolProviders : IPoolProviders
    {
        private ExplosionPoolProvider _explosionPoolProvider;
        public IExplosionPoolProvider ExplosionPoolProvider => _explosionPoolProvider;

        private ShipDeathPoolProvider _shipDeathPoolProvider;
        public IShipDeathPoolProvider ShipDeathPoolProvider => _shipDeathPoolProvider;

        private ProjectilePoolProvider _projectilePoolProvider;
        public IProjectilePoolProvider ProjectilePoolProvider => _projectilePoolProvider;

        private UnitPoolProvider _unitPoolProvider;
        public IUnitPoolProvider UnitPoolProvider => _unitPoolProvider;

        private Pool<IDroneController, DroneActivationArgs> _dronePool;
        public IPool<IDroneController, DroneActivationArgs> DronePool => _dronePool;

        public IUnitToPoolMap UnitToPoolMap { get; }

        // 16 per cruiser
        private const int DRONES_INITIAL_CAPACITY = 32;

        public PoolProviders(IFactoryProvider factoryProvider, IUIManager uiManager, IDroneFactory droneFactory)
        {
            Helper.AssertIsNotNull(factoryProvider, uiManager, droneFactory);

            _explosionPoolProvider = new ExplosionPoolProvider(factoryProvider.PrefabFactory);
            _shipDeathPoolProvider = new ShipDeathPoolProvider(factoryProvider.PrefabFactory);
            _projectilePoolProvider = new ProjectilePoolProvider(factoryProvider);
            _unitPoolProvider = new UnitPoolProvider(uiManager, factoryProvider);
            _dronePool = new Pool<IDroneController, DroneActivationArgs>(droneFactory);
            UnitToPoolMap = new UnitToPoolMap(UnitPoolProvider);
        }

        // Not part of constructor, because ProjecilePoolProvider and UnitPollProvider depend on ExplosionPoolProvider :/
        public void SetInitialCapacities()
        {
            _explosionPoolProvider.SetInitialCapacity();
            _shipDeathPoolProvider.SetInitialCapacity();
            _projectilePoolProvider.SetInitialCapacity();
            _unitPoolProvider.SetInitialCapacity();
            _dronePool.AddCapacity(DRONES_INITIAL_CAPACITY);
        }
    }
}