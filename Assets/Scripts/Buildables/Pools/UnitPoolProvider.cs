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
        private readonly FactoryProvider _factoryProvider;
        private readonly IList<Pool<Unit, BuildableActivationArgs>> _pools;

        // Don't want more than 1 because unit may never be built.  Want at least 1
        // to force prefab to be loaded.  First time load is the slowest, because
        // it fetches everything the prefab needs (materials, sprites???).
        private const int INITIAL_UNIT_CAPACITY = 1;

        // Aircraft
        public Pool<Unit, BuildableActivationArgs> BomberPool { get; }
        public Pool<Unit, BuildableActivationArgs> FighterPool { get; }
        public Pool<Unit, BuildableActivationArgs> GunshipPool { get; }
        public Pool<Unit, BuildableActivationArgs> SteamCopterPool { get; }
        public Pool<Unit, BuildableActivationArgs> BroadswordPool { get; }
        public Pool<Unit, BuildableActivationArgs> StratBomberPool { get; }
        public Pool<Unit, BuildableActivationArgs> SpyPlanePool { get; }
        public Pool<Unit, BuildableActivationArgs> TestAircraftPool { get; }
        public Pool<Unit, BuildableActivationArgs> MissileFighterPool { get; }

        // Ships
        public Pool<Unit, BuildableActivationArgs> AttackBoatPool { get; }
        public Pool<Unit, BuildableActivationArgs> AttackRIBPool { get; }
        public Pool<Unit, BuildableActivationArgs> FrigatePool { get; }
        public Pool<Unit, BuildableActivationArgs> DestroyerPool { get; }
        public Pool<Unit, BuildableActivationArgs> SiegeDestroyerPool { get; }
        public Pool<Unit, BuildableActivationArgs> ArchonPool { get; }
        public Pool<Unit, BuildableActivationArgs> GlassCannoneerPool { get; }
        public Pool<Unit, BuildableActivationArgs> GunBoatPool { get; }
        public Pool<Unit, BuildableActivationArgs> RocketTurtlePool { get; }
        public Pool<Unit, BuildableActivationArgs> FlakTurtlePool { get; }

        public UnitPoolProvider(IUIManager uiManager, FactoryProvider factoryProvider)
        {
            Helper.AssertIsNotNull(uiManager, factoryProvider);

            _uiManager = uiManager;
            _factoryProvider = factoryProvider;
            _pools = new List<Pool<Unit, BuildableActivationArgs>>();

            // Aircraft
            BomberPool = CreatePool(StaticPrefabKeys.Units.Bomber);
            FighterPool = CreatePool(StaticPrefabKeys.Units.Fighter);
            GunshipPool = CreatePool(StaticPrefabKeys.Units.Gunship);
            SteamCopterPool = CreatePool(StaticPrefabKeys.Units.SteamCopter);
            BroadswordPool = CreatePool(StaticPrefabKeys.Units.Broadsword);
            StratBomberPool = CreatePool(StaticPrefabKeys.Units.StratBomber);
            SpyPlanePool = CreatePool(StaticPrefabKeys.Units.SpyPlane);
            TestAircraftPool = CreatePool(StaticPrefabKeys.Units.TestAircraft);
            MissileFighterPool = CreatePool(StaticPrefabKeys.Units.MissileFighter);

            // Ship
            AttackBoatPool = CreatePool(StaticPrefabKeys.Units.AttackBoat);
            AttackRIBPool = CreatePool(StaticPrefabKeys.Units.AttackRIB);
            FrigatePool = CreatePool(StaticPrefabKeys.Units.Frigate);
            DestroyerPool = CreatePool(StaticPrefabKeys.Units.Destroyer);
            SiegeDestroyerPool = CreatePool(StaticPrefabKeys.Units.SiegeDestroyer);
            ArchonPool = CreatePool(StaticPrefabKeys.Units.ArchonBattleship);
            GlassCannoneerPool = CreatePool(StaticPrefabKeys.Units.GlassCannoneer);
            GunBoatPool = CreatePool(StaticPrefabKeys.Units.GunBoat);
            RocketTurtlePool = CreatePool(StaticPrefabKeys.Units.RocketTurtle);
            FlakTurtlePool = CreatePool(StaticPrefabKeys.Units.FlakTurtle);
        }

        private Pool<Unit, BuildableActivationArgs> CreatePool(IPrefabKey unitKey)
        {
            Pool<Unit, BuildableActivationArgs> pool
                = new Pool<Unit, BuildableActivationArgs>(
                    new UnitFactory(
                        unitKey,
                        _uiManager,
                        _factoryProvider));
            _pools.Add(pool);
            return pool;
        }

        public void SetInitialCapacity()
        {
            foreach (Pool<Unit, BuildableActivationArgs> pool in _pools)
            {
                pool.AddCapacity(INITIAL_UNIT_CAPACITY);
            }
        }
    }
}