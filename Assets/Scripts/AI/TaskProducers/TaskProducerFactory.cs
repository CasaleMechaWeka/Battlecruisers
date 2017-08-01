using System.Collections.Generic;
using BattleCruisers.AI.TaskProducers.SlotNumber;
using BattleCruisers.AI.Tasks;
using BattleCruisers.AI.ThreatMonitors;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Fetchers;
using BattleCruisers.Utils;

namespace BattleCruisers.AI.TaskProducers
{
    public class TaskProducerFactory : ITaskProducerFactory
	{
        private readonly ICruiserController _aiCruiser, _playerCruiser;
        private readonly IPrefabFactory _prefabFactory;
        private readonly ITaskFactory _taskFactory;
        private ISlotNumCalculatorFactory _slotNumCalculatorFactory;

		private const int AIR_HIGH_THREAT_DRONE_NUM = 6;
		private const int NAVAL_HIGH_THREAT_DRONE_NUM = 6;

        public TaskProducerFactory(ICruiserController aiCruiser, ICruiserController playerCruiser, IPrefabFactory prefabFactory, 
            ITaskFactory taskFactory, ISlotNumCalculatorFactory slotNumCalculatorFactory)
        {
            Helper.AssertIsNotNull(aiCruiser, playerCruiser, prefabFactory, taskFactory, slotNumCalculatorFactory);

            _aiCruiser = aiCruiser;
            _playerCruiser = playerCruiser;
            _prefabFactory = prefabFactory;
            _taskFactory = taskFactory;
            _slotNumCalculatorFactory = slotNumCalculatorFactory;
        }

		public void CreateReplaceDestroyedBuildingsTaskProducer(ITaskList tasks, IList<IPrefabKey> unlockedBuildingKeys)
        {
            new ReplaceDestroyedBuildingsTaskProducer(tasks, _aiCruiser, _prefabFactory, _taskFactory, unlockedBuildingKeys);
        }

		public void CreateAntiAirTaskProducer(ITaskList tasks)
        {
            IThreatEvaluator threatEvaluator = new ThreatEvaluator(AIR_HIGH_THREAT_DRONE_NUM);
            IThreatMonitor airThreatMonitor = new BuildingThreatMonitor<AirFactory>(_playerCruiser, threatEvaluator);
			
            int maxNumOfDeckSlots = GetHalfTheSlots(_aiCruiser.SlotWrapper.GetSlotCount(SlotType.Deck), roundUp: true);
            ISlotNumCalculator slotNumCalculator = _slotNumCalculatorFactory.CreateAntiAirSlotNumCalculator(maxNumOfDeckSlots);

            new AntiThreatTaskProducer(tasks, _aiCruiser, _prefabFactory, _taskFactory, BuildOrders.AntiAir, airThreatMonitor, slotNumCalculator);
        }

		public void CreateAntiNavalTaskProducer(ITaskList tasks)
		{
			IThreatEvaluator threatEvaluator = new ThreatEvaluator(NAVAL_HIGH_THREAT_DRONE_NUM);
            IThreatMonitor navalThreatMonitor = new BuildingThreatMonitor<NavalFactory>(_playerCruiser, threatEvaluator);

			int maxNumOfDeckSlots = GetHalfTheSlots(_aiCruiser.SlotWrapper.GetSlotCount(SlotType.Deck), roundUp: false);
            ISlotNumCalculator slotNumCalculator = _slotNumCalculatorFactory.CreateAntiNavalSlotNumCalculator(maxNumOfDeckSlots);

            new AntiThreatTaskProducer(tasks, _aiCruiser, _prefabFactory, _taskFactory, BuildOrders.AntiNaval, navalThreatMonitor, slotNumCalculator);
		}

        private int GetHalfTheSlots(int totalNumOfSlots, bool roundUp)
		{
            int half = totalNumOfSlots / 2;

            if (roundUp)
            {
                half += totalNumOfSlots % 2;
            }

            return half;
		}
	}
}