using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Pools;
using BattleCruisers.Utils.Factories;
using System.Collections.Generic;

namespace BattleCruisers.Buildables.Pools
{
    public class UnitPoolProvider : IUnitPoolProvider
    {
        private readonly IUIManager _uiManager;
        private readonly IFactoryProvider _factoryProvider;
        private readonly IList<IPool<Unit, BuildableActivationArgs>> _pools;

        // Don't want more than 1 because unit may never be built.  Want at least 1
        // to force prefab to be loaded.  First time load is the slowest, because
        // it fetches everything the prefab needs (materials, sprites???).
        private const int INITIAL_UNIT_CAPACITY = 1;

        // Aircraft
        public IPool<Unit, BuildableActivationArgs> BomberPool { get; }
        public IPool<Unit, BuildableActivationArgs> FighterPool { get; }
        public IPool<Unit, BuildableActivationArgs> GunshipPool { get; }
        public IPool<Unit, BuildableActivationArgs> TestAircraftPool { get; }

        // Ships
        public IPool<Unit, BuildableActivationArgs> AttackBoatPool { get; }
        public IPool<Unit, BuildableActivationArgs> AttackRIBPool { get; }
        public IPool<Unit, BuildableActivationArgs> FrigatePool { get; }
        public IPool<Unit, BuildableActivationArgs> DestroyerPool { get; }
        public IPool<Unit, BuildableActivationArgs> ArchonPool { get; }

        public UnitPoolProvider(IUIManager uiManager, IFactoryProvider factoryProvider)
        {
            Helper.AssertIsNotNull(uiManager, factoryProvider);

            _uiManager = uiManager;
            _factoryProvider = factoryProvider;
            _pools = new List<IPool<Unit, BuildableActivationArgs>>();

            // Aircraft
            BomberPool = CreatePool(StaticPrefabKeys.Units.Bomber);
            FighterPool = CreatePool(StaticPrefabKeys.Units.Fighter);
            GunshipPool = CreatePool(StaticPrefabKeys.Units.Gunship);
            TestAircraftPool = CreatePool(StaticPrefabKeys.Units.TestAircraft);

            // Ship
            AttackBoatPool = CreatePool(StaticPrefabKeys.Units.AttackBoat);
            AttackRIBPool = CreatePool(StaticPrefabKeys.Units.AttackRIB);
            FrigatePool = CreatePool(StaticPrefabKeys.Units.Frigate);
            DestroyerPool = CreatePool(StaticPrefabKeys.Units.Destroyer);
            ArchonPool = CreatePool(StaticPrefabKeys.Units.ArchonBattleship);
        }

        private IPool<Unit, BuildableActivationArgs> CreatePool(IPrefabKey unitKey)
        {
            IPool<Unit, BuildableActivationArgs> pool
                = new Pool<Unit, BuildableActivationArgs>(
                    new UnitFactory(
                        _factoryProvider.PrefabFactory,
                        unitKey,
                        _uiManager,
                        _factoryProvider));
            _pools.Add(pool);
            return pool;
        }

        public void SetInitialCapacity()
        {
            foreach (IPool<Unit, BuildableActivationArgs> pool in _pools)
            {
                pool.AddCapacity(INITIAL_UNIT_CAPACITY);
            }
        }
    }
}