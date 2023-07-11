using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.ThreatMonitors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine.Assertions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Data.Models;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.FactoryManagers
{
    public class PvPFactoryManagerFactory : IPvPFactoryManagerFactory
    {
        private readonly IGameModel _gameModel;
        private readonly IPvPPrefabFactory _prefabFactory;
        private readonly IPvPThreatMonitorFactory _threatMonitorFactory;

        private readonly static PvPUnitKey DEFAULT_PLANE_KEY = PvPStaticPrefabKeys.PvPUnits.PvPBomber;
        private readonly static PvPUnitKey LATEGAME_PLANE_KEY = PvPStaticPrefabKeys.PvPUnits.PvPSteamCopter;
        private readonly static PvPUnitKey ANTI_AIR_PLANE_KEY = PvPStaticPrefabKeys.PvPUnits.PvPFighter;
        private readonly static PvPUnitKey ANTI_NAVAL_PLANE_KEY = PvPStaticPrefabKeys.PvPUnits.PvPGunship;

        public PvPFactoryManagerFactory(IGameModel gameModel, IPvPPrefabFactory prefabFactory, IPvPThreatMonitorFactory threatMonitorFactory)
        {
            PvPHelper.AssertIsNotNull(gameModel, prefabFactory, threatMonitorFactory);

            _gameModel = gameModel;
            _prefabFactory = prefabFactory;
            _threatMonitorFactory = threatMonitorFactory;
        }

        public IPvPFactoryManager CreateNavalFactoryManager(IPvPCruiserController aiCruiser)
        {
            IList<PvPUnitKey> availableShipKeys = convertPvEUnitKey2PvPUnitKey(_gameModel.GetUnlockedUnits(convertPvPCategory2PvECategory(PvPUnitCategory.Naval)));
            IList<IPvPBuildableWrapper<IPvPUnit>> availableShips =
                availableShipKeys
                    .Select(key => _prefabFactory.GetUnitWrapperPrefab(key))
                    .ToList();
            IPvPUnitChooser unitChooser
                = new PvPMostExpensiveUnitChooser(
                    availableShips,
                    aiCruiser.DroneManager,
                    new PvPAffordableUnitFilter());
            return new PvPFactoryManager(PvPUnitCategory.Naval, aiCruiser, unitChooser);
        }

        private IList<PvPUnitKey> convertPvEUnitKey2PvPUnitKey(IList<UnitKey> keys)
        {
            IList<PvPUnitKey> iPvPKeys = new List<PvPUnitKey>();
            foreach (UnitKey key in keys)
            {
                iPvPKeys.Add(new PvPUnitKey(convertPvECategory2PvPUnitCategory(key.UnitCategory), "PvP" + key.PrefabName));
            }

            return iPvPKeys;
        }

        private PvPUnitCategory convertPvECategory2PvPUnitCategory(UnitCategory category)
        {
            switch (category)
            {
                case UnitCategory.Naval:
                    return PvPUnitCategory.Naval;
                case UnitCategory.Aircraft:
                    return PvPUnitCategory.Aircraft;
/*                case UnitCategory.Untouchable:
                    return PvPUnitCategory.Untouchable;*/
                default:
                    throw new System.Exception();
            }
        }

        private UnitCategory convertPvPCategory2PvECategory(PvPUnitCategory category)
        {
            switch (category)
            {
                case PvPUnitCategory.Naval:
                    return UnitCategory.Naval;
                case PvPUnitCategory.Aircraft:
                    return UnitCategory.Aircraft;
/*                case PvPUnitCategory.Untouchable:
                    return UnitCategory.Untouchable;*/
                default:
                    throw new System.Exception();
            }
        }

        public IPvPFactoryManager CreateAirfactoryManager(IPvPCruiserController aiCruiser)
        {
            Assert.IsTrue(_gameModel.IsUnitUnlocked(new UnitKey(convertPvPCategory2PvECategory(DEFAULT_PLANE_KEY.UnitCategory), DEFAULT_PLANE_KEY.PrefabName)), "Default plane should always be available.");
            IPvPBuildableWrapper<IPvPUnit> defaultPlane = _prefabFactory.GetUnitWrapperPrefab(DEFAULT_PLANE_KEY);
            IPvPBuildableWrapper<IPvPUnit> lategamePlane;
            if (_gameModel.NumOfLevelsCompleted >= 25)
            {
                lategamePlane = _prefabFactory.GetUnitWrapperPrefab(LATEGAME_PLANE_KEY);
            }
            else
            {
                lategamePlane = defaultPlane;
            }

            IPvPBuildableWrapper<IPvPUnit> antiAirPlane =
                _gameModel.IsUnitUnlocked(new UnitKey(convertPvPCategory2PvECategory( ANTI_AIR_PLANE_KEY.UnitCategory), ANTI_AIR_PLANE_KEY.PrefabName)) ?
                _prefabFactory.GetUnitWrapperPrefab(ANTI_AIR_PLANE_KEY) :
                defaultPlane;

            IPvPBuildableWrapper<IPvPUnit> antiNavalPlane =
                _gameModel.IsUnitUnlocked(new UnitKey(convertPvPCategory2PvECategory(ANTI_NAVAL_PLANE_KEY.UnitCategory), ANTI_NAVAL_PLANE_KEY.PrefabName)) ?
                _prefabFactory.GetUnitWrapperPrefab(ANTI_NAVAL_PLANE_KEY) :
                defaultPlane;

            IPvPThreatMonitor airThreatMonitor = _threatMonitorFactory.CreateDelayedThreatMonitor(_threatMonitorFactory.CreateAirThreatMonitor());
            IPvPThreatMonitor navalThreatMonitor = _threatMonitorFactory.CreateDelayedThreatMonitor(_threatMonitorFactory.CreateNavalThreatMonitor());

            IPvPUnitChooser unitchooser
                = new PvPAircraftUnitChooser(
                    defaultPlane,
                    lategamePlane,
                    antiAirPlane,
                    antiNavalPlane,
                    aiCruiser.DroneManager,
                    airThreatMonitor,
                    navalThreatMonitor,
                    threatLevelThreshold: PvPThreatLevel.High);

            return new PvPFactoryManager(PvPUnitCategory.Aircraft, aiCruiser, unitchooser);
        }
    }
}
