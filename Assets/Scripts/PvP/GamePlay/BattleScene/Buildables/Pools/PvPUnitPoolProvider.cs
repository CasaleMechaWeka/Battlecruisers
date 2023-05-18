using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Pools;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Pools
{
    public class PvPUnitPoolProvider : IPvPUnitPoolProvider
    {
        private readonly IPvPUIManager _uiManager;
        private readonly IPvPFactoryProvider _factoryProvider;
        private readonly IList<IPvPPool<PvPUnit, PvPBuildableActivationArgs>> _pools;

        // Don't want more than 1 because unit may never be built.  Want at least 1
        // to force prefab to be loaded.  First time load is the slowest, because
        // it fetches everything the prefab needs (materials, sprites???).
        private const int INITIAL_UNIT_CAPACITY = 1;

        // Aircraft
        public IPvPPool<PvPUnit, PvPBuildableActivationArgs> BomberPool { get; }
        public IPvPPool<PvPUnit, PvPBuildableActivationArgs> FighterPool { get; }
        public IPvPPool<PvPUnit, PvPBuildableActivationArgs> GunshipPool { get; }
        public IPvPPool<PvPUnit, PvPBuildableActivationArgs> SteamCopterPool { get; }

        public IPvPPool<PvPUnit, PvPBuildableActivationArgs> TestAircraftPool { get; }

        // Ships
        public IPvPPool<PvPUnit, PvPBuildableActivationArgs> AttackBoatPool { get; }
        public IPvPPool<PvPUnit, PvPBuildableActivationArgs> AttackRIBPool { get; }
        public IPvPPool<PvPUnit, PvPBuildableActivationArgs> FrigatePool { get; }
        public IPvPPool<PvPUnit, PvPBuildableActivationArgs> DestroyerPool { get; }
        public IPvPPool<PvPUnit, PvPBuildableActivationArgs> ArchonPool { get; }

        public PvPUnitPoolProvider(IPvPUIManager uiManager, IPvPFactoryProvider factoryProvider)
        {
            PvPHelper.AssertIsNotNull(uiManager, factoryProvider);

            _uiManager = uiManager;
            _factoryProvider = factoryProvider;
            _pools = new List<IPvPPool<PvPUnit, PvPBuildableActivationArgs>>();

            // Aircraft
            BomberPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPBomber);
            FighterPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPFighter);
            GunshipPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPGunship);
            SteamCopterPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPSteamCopter);
            TestAircraftPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPTestAircraft);

            // Ship
            AttackBoatPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPAttackBoat);
            AttackRIBPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPAttackRIB);
            FrigatePool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPFrigate);
            DestroyerPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPDestroyer);
            ArchonPool = CreatePool(PvPStaticPrefabKeys.PvPUnits.PvPArchonBattleship);
        }

        private IPvPPool<PvPUnit, PvPBuildableActivationArgs> CreatePool(IPvPPrefabKey unitKey)
        {
            IPvPPool<PvPUnit, PvPBuildableActivationArgs> pool
                = new PvPPool<PvPUnit, PvPBuildableActivationArgs>(
                    new PvPUnitFactory(
                        _factoryProvider.PrefabFactory,
                        unitKey,
                        _uiManager,
                        _factoryProvider));
            _pools.Add(pool);
            return pool;
        }

        public void SetInitialCapacity()
        {
            foreach (IPvPPool<PvPUnit, PvPBuildableActivationArgs> pool in _pools)
            {
                pool.AddCapacity(INITIAL_UNIT_CAPACITY);
            }
        }
    }
}