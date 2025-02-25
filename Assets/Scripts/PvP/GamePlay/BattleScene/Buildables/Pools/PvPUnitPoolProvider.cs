using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Utils.BattleScene.Pools;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools
{
    public class PvPUnitPoolProvider : IPvPUnitPoolProvider
    {
        private readonly IPvPUIManager _uiManager;
        private readonly IPvPFactoryProvider _factoryProvider;
        private readonly IList<IPool<PvPUnit, PvPBuildableActivationArgs>> _pools;

        // Don't want more than 1 because unit may never be built.  Want at least 1
        // to force prefab to be loaded.  First time load is the slowest, because
        // it fetches everything the prefab needs (materials, sprites???).
        private const int INITIAL_UNIT_CAPACITY = 1;

        // Aircraft
        public IPool<PvPUnit, PvPBuildableActivationArgs> BomberPool { get; }
        public IPool<PvPUnit, PvPBuildableActivationArgs> FighterPool { get; }
        public IPool<PvPUnit, PvPBuildableActivationArgs> GunshipPool { get; }
        public IPool<PvPUnit, PvPBuildableActivationArgs> SteamCopterPool { get; }
        public IPool<PvPUnit, PvPBuildableActivationArgs> BroadswordPool { get; }
        public IPool<PvPUnit, PvPBuildableActivationArgs> StratBomberPool { get; }
        public IPool<PvPUnit, PvPBuildableActivationArgs> SpyPlanePool { get; }
        public IPool<PvPUnit, PvPBuildableActivationArgs> MissileFighterPool { get; }
        public IPool<PvPUnit, PvPBuildableActivationArgs> TestAircraftPool { get; }

        // Ships
        public IPool<PvPUnit, PvPBuildableActivationArgs> AttackBoatPool { get; }
        public IPool<PvPUnit, PvPBuildableActivationArgs> AttackRIBPool { get; }
        public IPool<PvPUnit, PvPBuildableActivationArgs> FrigatePool { get; }
        public IPool<PvPUnit, PvPBuildableActivationArgs> DestroyerPool { get; }
        public IPool<PvPUnit, PvPBuildableActivationArgs> SiegeDestroyerPool { get; }
        public IPool<PvPUnit, PvPBuildableActivationArgs> ArchonPool { get; }
        public IPool<PvPUnit, PvPBuildableActivationArgs> GlassCannoneerPool { get; }
        public IPool<PvPUnit, PvPBuildableActivationArgs> GunBoatPool { get; }
        public IPool<PvPUnit, PvPBuildableActivationArgs> RocketTurtlePool { get; }
        public IPool<PvPUnit, PvPBuildableActivationArgs> FlakTurtlePool { get; }

        public PvPUnitPoolProvider(IPvPFactoryProvider factoryProvider)
        {
            PvPHelper.AssertIsNotNull(factoryProvider);

            // _uiManager = uiManager;
            _factoryProvider = factoryProvider;
            _pools = new List<IPool<PvPUnit, PvPBuildableActivationArgs>>();

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
            _pools = new List<IPool<PvPUnit, PvPBuildableActivationArgs>>();

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

        private IPool<PvPUnit, PvPBuildableActivationArgs> CreatePool(IPrefabKey unitKey)
        {
            IPool<PvPUnit, PvPBuildableActivationArgs> pool
                = new PvPPool<PvPUnit, PvPBuildableActivationArgs>(
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
            foreach (IPool<PvPUnit, PvPBuildableActivationArgs> pool in _pools)
            {
                pool.AddCapacity(INITIAL_UNIT_CAPACITY);
            }
        }
    }
}