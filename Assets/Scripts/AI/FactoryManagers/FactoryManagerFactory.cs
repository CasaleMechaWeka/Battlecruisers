using System.Collections.Generic;
using System.Linq;
using BattleCruisers.AI.ThreatMonitors;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils;
using UnityEngine.Assertions;
using BattleCruisers.Data.Models;
using BattleCruisers.Cruisers;

namespace BattleCruisers.AI.FactoryManagers
{
    public class FactoryManagerFactory
    {
        private readonly GameModel _gameModel;
        private readonly ThreatMonitorFactory _threatMonitorFactory;

        private readonly static UnitKey DEFAULT_PLANE_KEY = StaticPrefabKeys.Units.Bomber;
        private readonly static UnitKey LATEGAME_PLANE_KEY = StaticPrefabKeys.Units.SteamCopter;
        private readonly static UnitKey ANTI_AIR_PLANE_KEY = StaticPrefabKeys.Units.Fighter;
        private readonly static UnitKey ANTI_NAVAL_PLANE_KEY = StaticPrefabKeys.Units.Gunship;
        private readonly static UnitKey BROADSWROD_GUNSHIP_KEY = StaticPrefabKeys.Units.Broadsword;
        private readonly static UnitKey STRATBOMBER_KEY = StaticPrefabKeys.Units.StratBomber;

        public FactoryManagerFactory(GameModel gameModel, ThreatMonitorFactory threatMonitorFactory)
        {
            Helper.AssertIsNotNull(gameModel, threatMonitorFactory);

            _gameModel = gameModel;
            _threatMonitorFactory = threatMonitorFactory;
        }

        public IManagedDisposable CreateNavalFactoryManager(ICruiserController aiCruiser)
        {
            IList<UnitKey> availableShipKeys = _gameModel.GetUnlockedUnits(UnitCategory.Naval);
            IList<IBuildableWrapper<IUnit>> availableShips =
                availableShipKeys
                    .Select(key => PrefabFactory.GetUnitWrapperPrefab(key))
                    .ToList();
            UnitChooser unitChooser
                = new MostExpensiveUnitChooser(
                    availableShips,
                    aiCruiser.DroneManager);

            return new FactoryManager(UnitCategory.Naval, aiCruiser, unitChooser);
        }

        public IManagedDisposable CreateAirfactoryManager(ICruiserController aiCruiser)
        {
            Assert.IsTrue(_gameModel.IsUnitUnlocked(DEFAULT_PLANE_KEY), "Default plane should always be available.");
            IBuildableWrapper<IUnit> defaultPlane = PrefabFactory.GetUnitWrapperPrefab(DEFAULT_PLANE_KEY);
            IBuildableWrapper<IUnit> lategamePlane;
            if (_gameModel.NumOfLevelsCompleted >= 25)
            {
                lategamePlane = PrefabFactory.GetUnitWrapperPrefab(LATEGAME_PLANE_KEY);
            }
            else
            {
                lategamePlane = defaultPlane;
            }

            IBuildableWrapper<IUnit> antiAirPlane =
                _gameModel.IsUnitUnlocked(ANTI_AIR_PLANE_KEY) ?
                PrefabFactory.GetUnitWrapperPrefab(ANTI_AIR_PLANE_KEY) :
                defaultPlane;

            IBuildableWrapper<IUnit> antiNavalPlane =
                _gameModel.IsUnitUnlocked(ANTI_NAVAL_PLANE_KEY) ?
                PrefabFactory.GetUnitWrapperPrefab(ANTI_NAVAL_PLANE_KEY) :
                defaultPlane;

            IBuildableWrapper<IUnit> broadswordGunship =
            _gameModel.IsUnitUnlocked(BROADSWROD_GUNSHIP_KEY) ?
            PrefabFactory.GetUnitWrapperPrefab(BROADSWROD_GUNSHIP_KEY) :
            lategamePlane;

            IBuildableWrapper<IUnit> stratBomber =
            _gameModel.IsUnitUnlocked(STRATBOMBER_KEY) ?
            PrefabFactory.GetUnitWrapperPrefab(STRATBOMBER_KEY) :
            lategamePlane;

            BaseThreatMonitor airThreatMonitor = _threatMonitorFactory.CreateDelayedThreatMonitor(_threatMonitorFactory.CreateAirThreatMonitor());
            BaseThreatMonitor navalThreatMonitor = _threatMonitorFactory.CreateDelayedThreatMonitor(_threatMonitorFactory.CreateNavalThreatMonitor());

            UnitChooser unitchooser
                = new AircraftUnitChooser(
                    defaultPlane,
                    lategamePlane,
                    antiAirPlane,
                    antiNavalPlane,
                    broadswordGunship,
                    stratBomber,
                    aiCruiser.DroneManager,
                    airThreatMonitor,
                    navalThreatMonitor,
                    threatLevelThreshold: ThreatLevel.High);

            return new FactoryManager(UnitCategory.Aircraft, aiCruiser, unitchooser);
        }
    }
}
