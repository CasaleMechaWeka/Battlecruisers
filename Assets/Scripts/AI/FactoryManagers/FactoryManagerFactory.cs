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

namespace BattleCruisers.AI.FactoryManagers
{
    public class FactoryManagerFactory : IFactoryManagerFactory
    {
        private readonly IGameModel _gameModel;
        private readonly IPrefabFactory _prefabFactory;
		private readonly IThreatMonitorFactory _threatMonitorFactory;

        private readonly static UnitKey DEFAULT_PLANE_KEY = StaticPrefabKeys.Units.Bomber;
        private readonly static UnitKey ANTI_AIR_PLANE_KEY = StaticPrefabKeys.Units.Fighter;
        private readonly static UnitKey ANTI_NAVAL_PLANE_KEY = StaticPrefabKeys.Units.Gunship;

		public FactoryManagerFactory(IGameModel gameModel, IPrefabFactory prefabFactory, IThreatMonitorFactory threatMonitorFactory)
        {
            Helper.AssertIsNotNull(gameModel, prefabFactory, threatMonitorFactory);

            _gameModel = gameModel;
            _prefabFactory = prefabFactory;
            _threatMonitorFactory = threatMonitorFactory;
        }

        public IFactoryManager CreateNavalFactoryManager(ILevelInfo levelInfo)
        {
            IList<UnitKey> availableShipKeys = _gameModel.GetUnlockedUnits(UnitCategory.Naval);
            IList<IBuildableWrapper<IUnit>> availableShips =
                availableShipKeys
                    .Select(key => _prefabFactory.GetUnitWrapperPrefab(key))
                    .ToList();
            IUnitChooser unitChooser 
                = new MostExpensiveUnitChooser(
                    availableShips, 
                    levelInfo.AICruiser.DroneManager, 
                    new AffordableUnitFilter());

            return new FactoryManager(UnitCategory.Naval, levelInfo.AICruiser, unitChooser);
        }

        public IFactoryManager CreateAirfactoryManager(ILevelInfo levelInfo)
        {
            Assert.IsTrue(_gameModel.IsUnitUnlocked(DEFAULT_PLANE_KEY),"Default plane should always be available.");
            IBuildableWrapper<IUnit> defaultPlane = _prefabFactory.GetUnitWrapperPrefab(DEFAULT_PLANE_KEY);

            IBuildableWrapper<IUnit> antiAirPlane =
                _gameModel.IsUnitUnlocked(ANTI_AIR_PLANE_KEY) ?
                _prefabFactory.GetUnitWrapperPrefab(ANTI_AIR_PLANE_KEY) :
                defaultPlane;

            IBuildableWrapper<IUnit> antiNavalPlane =
                _gameModel.IsUnitUnlocked(ANTI_NAVAL_PLANE_KEY) ?
                _prefabFactory.GetUnitWrapperPrefab(ANTI_NAVAL_PLANE_KEY) :
                defaultPlane;

            IThreatMonitor airThreatMonitor = _threatMonitorFactory.CreateDelayedThreatMonitor(_threatMonitorFactory.CreateAirThreatMonitor());
            IThreatMonitor navalThreatMonitor = _threatMonitorFactory.CreateDelayedThreatMonitor(_threatMonitorFactory.CreateNavalThreatMonitor());

            IUnitChooser unitchooser
                = new AircraftUnitChooser(
                    defaultPlane,
                    antiAirPlane,
                    antiNavalPlane,
                    levelInfo.AICruiser.DroneManager,
                    airThreatMonitor,
                    navalThreatMonitor,
                    threatLevelThreshold: ThreatLevel.High);

            return new FactoryManager(UnitCategory.Aircraft, levelInfo.AICruiser, unitchooser);
        }
    }
}
