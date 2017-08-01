using System.Collections.Generic;
using BattleCruisers.AI.TaskProducers.SlotNumber;
using BattleCruisers.AI.Tasks;
using BattleCruisers.AI.ThreatMonitors;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.BuildOrders;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Fetchers;

namespace BattleCruisers.AI.TaskProducers
{
    public class TaskProducerFactory : ITaskProducerFactory
	{
        private readonly ICruiserController _aiCruiser, _playerCruiser;
        private readonly IPrefabFactory _prefabFactory;
        private readonly ITaskFactory _taskFactory;
        private ISlotNumCalculatorFactory _slotNumCalculatorFactory;

        private const int AIR_HIGH_THREAT_DRONE_NUM = 6;

        public TaskProducerFactory(ICruiserController aiCruiser, ICruiserController playerCruiser, IPrefabFactory prefabFactory, 
            ITaskFactory taskFactory, ISlotNumCalculatorFactory slotNumCalculatorFactory)
        {
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
			
            int maxNumOfDeckSlots = FindMaxNumOfAntiAirSlots(_aiCruiser.SlotWrapper.GetSlotCount(SlotType.Deck));
            ISlotNumCalculator slotNumCalculator = _slotNumCalculatorFactory.CreateAntiAirSlotNumCalculator(maxNumOfDeckSlots);

            new AntiAirTaskProducer(tasks, _aiCruiser, _prefabFactory, _taskFactory, AntiAir.BuildOrder, airThreatMonitor, slotNumCalculator);
        }


		/// <returns>Half, rounded up.</returns>
		private int FindMaxNumOfAntiAirSlots(int totalNumOfSlots)
		{
			return totalNumOfSlots / 2 + totalNumOfSlots % 2;
		}
	}
}