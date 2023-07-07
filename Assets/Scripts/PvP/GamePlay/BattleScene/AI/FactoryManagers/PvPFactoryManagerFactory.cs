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
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.FactoryManagers
{
    public class PvPFactoryManagerFactory : IPvPFactoryManagerFactory
    {
        private readonly IPvPGameModel _gameModel;
        private readonly IPvPPrefabFactory _prefabFactory;
        private readonly IPvPThreatMonitorFactory _threatMonitorFactory;

        private readonly static PvPUnitKey DEFAULT_PLANE_KEY = PvPStaticPrefabKeys.PvPUnits.PvPBomber;
        private readonly static PvPUnitKey LATEGAME_PLANE_KEY = PvPStaticPrefabKeys.PvPUnits.PvPSteamCopter;
        private readonly static PvPUnitKey ANTI_AIR_PLANE_KEY = PvPStaticPrefabKeys.PvPUnits.PvPFighter;
        private readonly static PvPUnitKey ANTI_NAVAL_PLANE_KEY = PvPStaticPrefabKeys.PvPUnits.PvPGunship;

        public PvPFactoryManagerFactory(IPvPGameModel gameModel, IPvPPrefabFactory prefabFactory, IPvPThreatMonitorFactory threatMonitorFactory)
        {
            PvPHelper.AssertIsNotNull(gameModel, prefabFactory, threatMonitorFactory);

            _gameModel = gameModel;
            _prefabFactory = prefabFactory;
            _threatMonitorFactory = threatMonitorFactory;
        }

        public IPvPFactoryManager CreateNavalFactoryManager(IPvPCruiserController aiCruiser)
        {
            IList<PvPUnitKey> availableShipKeys = _gameModel.GetUnlockedUnits(PvPUnitCategory.Naval);
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

        public IPvPFactoryManager CreateAirfactoryManager(IPvPCruiserController aiCruiser)
        {
            Assert.IsTrue(_gameModel.IsUnitUnlocked(DEFAULT_PLANE_KEY), "Default plane should always be available.");
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
                _gameModel.IsUnitUnlocked(ANTI_AIR_PLANE_KEY) ?
                _prefabFactory.GetUnitWrapperPrefab(ANTI_AIR_PLANE_KEY) :
                defaultPlane;

            IPvPBuildableWrapper<IPvPUnit> antiNavalPlane =
                _gameModel.IsUnitUnlocked(ANTI_NAVAL_PLANE_KEY) ?
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
