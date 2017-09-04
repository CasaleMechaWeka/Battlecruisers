using BattleCruisers.AI.BuildOrders;
using BattleCruisers.AI.TaskProducers.SlotNumber;
using BattleCruisers.AI.Tasks;
using BattleCruisers.AI.ThreatMonitors;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
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
        private readonly IThreatMonitorFactory _threatMonitorFactory;

        public TaskProducerFactory(
            ICruiserController aiCruiser, 
            ICruiserController playerCruiser, 
            IPrefabFactory prefabFactory, 
            ITaskFactory taskFactory, 
            ISlotNumCalculatorFactory slotNumCalculatorFactory, 
            IStaticData staticData,
            IThreatMonitorFactory threatMonitorFactory)
        {
            Helper.AssertIsNotNull(aiCruiser, playerCruiser, prefabFactory, taskFactory, slotNumCalculatorFactory, staticData, threatMonitorFactory);

            _aiCruiser = aiCruiser;
            _playerCruiser = playerCruiser;
            _prefabFactory = prefabFactory;
            _taskFactory = taskFactory;
            _slotNumCalculatorFactory = slotNumCalculatorFactory;
            _staticData = staticData;
            _threatMonitorFactory = threatMonitorFactory;
        }

        public void CreateBasicTaskProducer(ITaskList tasks, IDynamicBuildOrder buildOrder)
        {
			new BasicTaskProducer(tasks, _aiCruiser, _prefabFactory, _taskFactory, buildOrder);
		}

		public void CreateReplaceDestroyedBuildingsTaskProducer(ITaskList tasks)
        {
            new ReplaceDestroyedBuildingsTaskProducer(tasks, _aiCruiser, _prefabFactory, _taskFactory, _staticData.BuildingKeys);
        }

        public void CreateAntiAirTaskProducer(ITaskList tasks, IDynamicBuildOrder antiAirBuildOrder)
        {
            IThreatMonitor airThreatMonitor = _threatMonitorFactory.CreateAirThreatMonitor(_playerCruiser);
			
            int maxNumOfDeckSlots = Helper.Half(_aiCruiser.SlotWrapper.GetSlotCount(SlotType.Deck), roundUp: true);
            ISlotNumCalculator slotNumCalculator = _slotNumCalculatorFactory.CreateAntiAirSlotNumCalculator(maxNumOfDeckSlots);

            new AntiThreatTaskProducer(tasks, _aiCruiser, _prefabFactory, _taskFactory, antiAirBuildOrder, airThreatMonitor, slotNumCalculator);
        }

		public void CreateAntiNavalTaskProducer(ITaskList tasks, IDynamicBuildOrder antiNavalBuildOrder)
		{
            IThreatMonitor navalThreatMonitor = _threatMonitorFactory.CreateNavalThreatMonitor(_playerCruiser);

            int maxNumOfDeckSlots = Helper.Half(_aiCruiser.SlotWrapper.GetSlotCount(SlotType.Deck), roundUp: false);
            ISlotNumCalculator slotNumCalculator = _slotNumCalculatorFactory.CreateAntiNavalSlotNumCalculator(maxNumOfDeckSlots);
			
			new AntiThreatTaskProducer(tasks, _aiCruiser, _prefabFactory, _taskFactory, antiNavalBuildOrder, navalThreatMonitor, slotNumCalculator);
		}

		public void CreateAntiRocketLauncherTaskProducer(ITaskList tasks, IDynamicBuildOrder antiRocketLauncherBuildOrder)
		{
            IThreatMonitor rocketLauncherThreatMonitor = _threatMonitorFactory.CreateRocketThreatMonitor(_playerCruiser);
			ISlotNumCalculator slotNumCalculator = _slotNumCalculatorFactory.CreateStaticSlotNumCalculator(numOfSlots: 1);

			new AntiThreatTaskProducer(tasks, _aiCruiser, _prefabFactory, _taskFactory, antiRocketLauncherBuildOrder, rocketLauncherThreatMonitor, slotNumCalculator);
		}
	}
}
