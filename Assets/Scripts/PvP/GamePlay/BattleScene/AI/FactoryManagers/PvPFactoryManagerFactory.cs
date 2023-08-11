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
        private PvPBattleSceneGodTunnel _battleSceneGodTunnel;
        private readonly IPvPPrefabFactory _prefabFactory;
        private readonly IPvPThreatMonitorFactory _threatMonitorFactory;

        private readonly static PvPUnitKey DEFAULT_PLANE_KEY = PvPStaticPrefabKeys.PvPUnits.PvPBomber;
        private readonly static PvPUnitKey LATEGAME_PLANE_KEY = PvPStaticPrefabKeys.PvPUnits.PvPSteamCopter;
        private readonly static PvPUnitKey ANTI_AIR_PLANE_KEY = PvPStaticPrefabKeys.PvPUnits.PvPFighter;
        private readonly static PvPUnitKey ANTI_NAVAL_PLANE_KEY = PvPStaticPrefabKeys.PvPUnits.PvPGunship;

        public PvPFactoryManagerFactory(PvPBattleSceneGodTunnel battleSceneGodTunnel, IPvPPrefabFactory prefabFactory, IPvPThreatMonitorFactory threatMonitorFactory)
        {
            PvPHelper.AssertIsNotNull(battleSceneGodTunnel, prefabFactory, threatMonitorFactory);

            _battleSceneGodTunnel = battleSceneGodTunnel;
            _prefabFactory = prefabFactory;
            _threatMonitorFactory = threatMonitorFactory;
        }

        public IPvPFactoryManager CreateNavalFactoryManager(PvPCruiser aiCruiser)
        {
            IList<PvPUnitKey> availableShipKeys = aiCruiser.Faction == PvPFaction.Blues ? _battleSceneGodTunnel.GetUnlockedUnits_LeftPlayer(PvPUnitCategory.Naval) : _battleSceneGodTunnel.GetUnlockedUnits_RightPlayer(PvPUnitCategory.Naval);
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

        public IPvPFactoryManager CreateAirfactoryManager(PvPCruiser aiCruiser)
        {
            Assert.IsTrue(aiCruiser.Faction == PvPFaction.Blues ? _battleSceneGodTunnel.IsUnitUnlocked_LeftPlayer(DEFAULT_PLANE_KEY) : _battleSceneGodTunnel.IsUnitUnlocked_RightPlayer(DEFAULT_PLANE_KEY), "Default plane should always be available.");
            IPvPBuildableWrapper<IPvPUnit> defaultPlane = _prefabFactory.GetUnitWrapperPrefab(DEFAULT_PLANE_KEY);
            IPvPBuildableWrapper<IPvPUnit> lategamePlane;
            if (_battleSceneGodTunnel.currentLevelNum >= 25)
            {
                lategamePlane = _prefabFactory.GetUnitWrapperPrefab(LATEGAME_PLANE_KEY);
            }
            else
            {
                lategamePlane = defaultPlane;
            }

            IPvPBuildableWrapper<IPvPUnit> antiAirPlane =
                (aiCruiser.Faction == PvPFaction.Blues ? _battleSceneGodTunnel.IsUnitUnlocked_LeftPlayer(ANTI_AIR_PLANE_KEY) : _battleSceneGodTunnel.IsUnitUnlocked_RightPlayer(ANTI_AIR_PLANE_KEY)) ?
                _prefabFactory.GetUnitWrapperPrefab(ANTI_AIR_PLANE_KEY) :
                defaultPlane;

            IPvPBuildableWrapper<IPvPUnit> antiNavalPlane =
              (aiCruiser.Faction == PvPFaction.Blues ? _battleSceneGodTunnel.IsUnitUnlocked_LeftPlayer(ANTI_NAVAL_PLANE_KEY) : _battleSceneGodTunnel.IsUnitUnlocked_RightPlayer(ANTI_NAVAL_PLANE_KEY)) ?
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
