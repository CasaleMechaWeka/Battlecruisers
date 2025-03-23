using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Utils.BattleScene.Pools;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools
{
    public class PvPUnitPoolProvider : IPvPUnitPoolProvider
    {
        private readonly IPvPUIManager _uiManager;
        private readonly IPvPFactoryProvider _factoryProvider;
        private readonly IList<Pool<PvPUnit, PvPBuildableActivationArgs>> _pools;

        // Don't want more than 1 because unit may never be built.  Want at least 1
        // to force prefab to be loaded.  First time load is the slowest, because
        // it fetches everything the prefab needs (materials, sprites???).
        private const int INITIAL_UNIT_CAPACITY = 1;

        // Aircraft
        public Pool<PvPUnit, PvPBuildableActivationArgs> BomberPool { get; }
        public Pool<PvPUnit, PvPBuildableActivationArgs> FighterPool { get; }
        public Pool<PvPUnit, PvPBuildableActivationArgs> GunshipPool { get; }
        public Pool<PvPUnit, PvPBuildableActivationArgs> SteamCopterPool { get; }
        public Pool<PvPUnit, PvPBuildableActivationArgs> BroadswordPool { get; }
        public Pool<PvPUnit, PvPBuildableActivationArgs> StratBomberPool { get; }
        public Pool<PvPUnit, PvPBuildableActivationArgs> SpyPlanePool { get; }
        public Pool<PvPUnit, PvPBuildableActivationArgs> MissileFighterPool { get; }
        public Pool<PvPUnit, PvPBuildableActivationArgs> TestAircraftPool { get; }

        // Ships
        public Pool<PvPUnit, PvPBuildableActivationArgs> AttackBoatPool { get; }
        public Pool<PvPUnit, PvPBuildableActivationArgs> AttackRIBPool { get; }
        public Pool<PvPUnit, PvPBuildableActivationArgs> FrigatePool { get; }
        public Pool<PvPUnit, PvPBuildableActivationArgs> DestroyerPool { get; }
        public Pool<PvPUnit, PvPBuildableActivationArgs> SiegeDestroyerPool { get; }
        public Pool<PvPUnit, PvPBuildableActivationArgs> ArchonPool { get; }
        public Pool<PvPUnit, PvPBuildableActivationArgs> GlassCannoneerPool { get; }
        public Pool<PvPUnit, PvPBuildableActivationArgs> GunBoatPool { get; }
        public Pool<PvPUnit, PvPBuildableActivationArgs> RocketTurtlePool { get; }
        public Pool<PvPUnit, PvPBuildableActivationArgs> FlakTurtlePool { get; }

        public PvPUnitPoolProvider(IPvPFactoryProvider factoryProvider)
        {
            PvPHelper.AssertIsNotNull(factoryProvider);

            // _uiManager = uiManager;
            _factoryProvider = factoryProvider;
            _pools = new List<Pool<PvPUnit, PvPBuildableActivationArgs>>();

            // Aircraft
            BomberPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPBomber);
            FighterPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPFighter);
            GunshipPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPGunship);
            SteamCopterPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPSteamCopter);
            BroadswordPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPBroadsword);
            StratBomberPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPStratBomber);
            SpyPlanePool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPSpyPlane);
            MissileFighterPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPMissileFighter);

            TestAircraftPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPTestAircraft);

            // Ship
            AttackBoatPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPAttackBoat);
            AttackRIBPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPAttackRIB);
            FrigatePool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPFrigate);
            DestroyerPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPDestroyer);
            SiegeDestroyerPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPSiegeDestroyer);
            ArchonPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPArchonBattleship);
            GlassCannoneerPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPGlassCannoneer);
            GunBoatPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPGunBoat);
            RocketTurtlePool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPRocketTurtle);
            FlakTurtlePool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPFlakTurtle);
        }


        public PvPUnitPoolProvider(IPvPUIManager uiManager, IPvPFactoryProvider factoryProvider)
        {
            PvPHelper.AssertIsNotNull(uiManager, factoryProvider);

            _uiManager = uiManager;
            _factoryProvider = factoryProvider;
            _pools = new List<Pool<PvPUnit, PvPBuildableActivationArgs>>();

            // Aircraft
            BomberPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPBomber);
            FighterPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPFighter);
            GunshipPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPGunship);
            SteamCopterPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPSteamCopter);
            BroadswordPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPBroadsword);
            StratBomberPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPStratBomber);
            SpyPlanePool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPSpyPlane);
            MissileFighterPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPMissileFighter);
            TestAircraftPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPTestAircraft);

            // Ship
            AttackBoatPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPAttackBoat);
            AttackRIBPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPAttackRIB);
            FrigatePool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPFrigate);
            DestroyerPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPDestroyer);
            SiegeDestroyerPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPSiegeDestroyer);
            ArchonPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPArchonBattleship);
            GlassCannoneerPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPGlassCannoneer);
            GunBoatPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPGunBoat);
            RocketTurtlePool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPRocketTurtle);
            FlakTurtlePool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPFlakTurtle);
        }

        private Pool<PvPUnit, PvPBuildableActivationArgs> CreatePool(IPrefabKey unitKey)
        {
            Pool<PvPUnit, PvPBuildableActivationArgs> pool
                = new Pool<PvPUnit, PvPBuildableActivationArgs>(
                    new PvPUnitFactory(
                        _factoryProvider.PrefabFactory,
                        unitKey,
                        // _uiManager,
                        _factoryProvider));
            _pools.Add(pool);
            return pool;
        }

        public void SetInitialCapacity()
        {
            foreach (Pool<PvPUnit, PvPBuildableActivationArgs> pool in _pools)
                pool.AddCapacity(INITIAL_UNIT_CAPACITY);
        }
    }
}