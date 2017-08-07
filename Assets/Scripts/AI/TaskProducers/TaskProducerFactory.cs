using System.Collections.Generic;
using BattleCruisers.AI.TaskProducers.SlotNumber;
using BattleCruisers.AI.Tasks;
using BattleCruisers.AI.ThreatMonitors;
using BattleCruisers.Buildables.Buildings.Turrets.Offensive;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Fetchers;
using BattleCruisers.Utils;

namespace BattleCruisers.AI.TaskProducers
{
    public class TaskProducerFactory : ITaskProducerFactory
	{
        private readonly ICruiserController _aiCruiser, _playerCruiser;
        private readonly IPrefabFactory _prefabFactory;
        private readonly ITaskFactory _taskFactory;
        private readonly ISlotNumCalculatorFactory _slotNumCalculatorFactory;
        private readonly IStaticData _staticData;

		private const int AIR_HIGH_THREAT_DRONE_NUM = 6;
		private const int NAVAL_HIGH_THREAT_DRONE_NUM = 6;
        private const float ROCKET_LAUNCHER_HIGH_THREAT_BUILDING_NUM = 0.5f;

        public TaskProducerFactory(ICruiserController aiCruiser, ICruiserController playerCruiser, IPrefabFactory prefabFactory, 
            ITaskFactory taskFactory, ISlotNumCalculatorFactory slotNumCalculatorFactory, IStaticData staticData)
        {
            Helper.AssertIsNotNull(aiCruiser, playerCruiser, prefabFactory, taskFactory, slotNumCalculatorFactory, staticData);

            _aiCruiser = aiCruiser;
            _playerCruiser = playerCruiser;
            _prefabFactory = prefabFactory;
            _taskFactory = taskFactory;
            _slotNumCalculatorFactory = slotNumCalculatorFactory;
            _staticData = staticData;
        }

        public void CreateBasicTaskProducer(ITaskList tasks, IList<IPrefabKey> buildOrder)
        {
			new BasicTaskProducer(tasks, _aiCruiser, _prefabFactory, _taskFactory, buildOrder);
		}

		public void CreateReplaceDestroyedBuildingsTaskProducer(ITaskList tasks)
        {
            new ReplaceDestroyedBuildingsTaskProducer(tasks, _aiCruiser, _prefabFactory, _taskFactory, _staticData.BuildingKeys);
        }

        public void CreateAntiAirTaskProducer(ITaskList tasks, IList<IPrefabKey> antiAirBuildOrder)
        {
            IThreatEvaluator threatEvaluator = new ThreatEvaluator(AIR_HIGH_THREAT_DRONE_NUM);
            IThreatMonitor airThreatMonitor = new FactoryThreatMonitor(_playerCruiser, threatEvaluator, UnitCategory.Aircraft);
			
            int maxNumOfDeckSlots = Helper.Half(_aiCruiser.SlotWrapper.GetSlotCount(SlotType.Deck), roundUp: true);
            ISlotNumCalculator slotNumCalculator = _slotNumCalculatorFactory.CreateAntiAirSlotNumCalculator(maxNumOfDeckSlots);

            new AntiThreatTaskProducer(tasks, _aiCruiser, _prefabFactory, _taskFactory, antiAirBuildOrder, airThreatMonitor, slotNumCalculator);
        }

		public void CreateAntiNavalTaskProducer(ITaskList tasks, IList<IPrefabKey> antiNavalBuildOrder)
		{
			IThreatEvaluator threatEvaluator = new ThreatEvaluator(NAVAL_HIGH_THREAT_DRONE_NUM);
            IThreatMonitor navalThreatMonitor = new FactoryThreatMonitor(_playerCruiser, threatEvaluator, UnitCategory.Naval);

            int maxNumOfDeckSlots = Helper.Half(_aiCruiser.SlotWrapper.GetSlotCount(SlotType.Deck), roundUp: false);
            ISlotNumCalculator slotNumCalculator = _slotNumCalculatorFactory.CreateAntiNavalSlotNumCalculator(maxNumOfDeckSlots);

            new AntiThreatTaskProducer(tasks, _aiCruiser, _prefabFactory, _taskFactory, antiNavalBuildOrder, navalThreatMonitor, slotNumCalculator);
		}

		public void CreateAntiRocketLauncherTaskProducer(ITaskList tasks, IList<IPrefabKey> antiRocketLauncherBuildOrder)
		{
            IThreatEvaluator threatEvaluator = new ThreatEvaluator(ROCKET_LAUNCHER_HIGH_THREAT_BUILDING_NUM);
			IThreatMonitor rocketLauncherThreatMonitor = new BuildingThreatMonitor<RocketLauncherController>(_playerCruiser, threatEvaluator);
            ISlotNumCalculator slotNumCalculator = _slotNumCalculatorFactory.CreateStaticSlotNumCalculator(numOfSlots: 1);

            new AntiThreatTaskProducer(tasks, _aiCruiser, _prefabFactory, _taskFactory, antiRocketLauncherBuildOrder, rocketLauncherThreatMonitor, slotNumCalculator);
		}
	}
}
