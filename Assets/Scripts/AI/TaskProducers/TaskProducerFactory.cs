using BattleCruisers.AI.BuildOrders;
using BattleCruisers.AI.TaskProducers.SlotNumber;
using BattleCruisers.AI.Tasks;
using BattleCruisers.AI.ThreatMonitors;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Static;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.AI.TaskProducers
{
    public class TaskProducerFactory : ITaskProducerFactory
	{
        // FELIX  Remove unused player cruiser :)
        private readonly ICruiserController _aiCruiser, _playerCruiser;
        private readonly IPrefabFactory _prefabFactory;
        private readonly ITaskFactory _taskFactory;
        private readonly ISlotNumCalculatorFactory _slotNumCalculatorFactory;
        private readonly IStaticData _staticData;
        private readonly IThreatMonitorFactory _threatMonitorFactory;

        // For spy satellite launcher
        private const int NUM_OF_DECK_SLOTS_TO_RESERVE = 1;

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

        public ITaskProducer CreateBasicTaskProducer(ITaskList tasks, IDynamicBuildOrder buildOrder)
        {
			return new BasicTaskProducer(tasks, _aiCruiser, _prefabFactory, _taskFactory, buildOrder);
		}

		public ITaskProducer CreateReplaceDestroyedBuildingsTaskProducer(ITaskList tasks)
        {
            return new ReplaceDestroyedBuildingsTaskProducer(tasks, _aiCruiser, _prefabFactory, _taskFactory, _staticData.BuildingKeys);
        }

        public ITaskProducer CreateAntiAirTaskProducer(ITaskList tasks, IDynamicBuildOrder antiAirBuildOrder)
        {
            IThreatMonitor airThreatMonitor = _threatMonitorFactory.CreateAirThreatMonitor();
			
            int maxNumOfDeckSlots = Helper.Half(_aiCruiser.SlotWrapper.GetSlotCount(SlotType.Deck) - NUM_OF_DECK_SLOTS_TO_RESERVE, roundUp: true);
            ISlotNumCalculator slotNumCalculator = _slotNumCalculatorFactory.CreateAntiAirSlotNumCalculator(maxNumOfDeckSlots);

            return new AntiThreatTaskProducer(tasks, _aiCruiser, _prefabFactory, _taskFactory, antiAirBuildOrder, airThreatMonitor, slotNumCalculator);
        }

		public ITaskProducer CreateAntiNavalTaskProducer(ITaskList tasks, IDynamicBuildOrder antiNavalBuildOrder)
		{
            IThreatMonitor navalThreatMonitor = _threatMonitorFactory.CreateNavalThreatMonitor();

            int maxNumOfDeckSlots = Helper.Half(_aiCruiser.SlotWrapper.GetSlotCount(SlotType.Deck) - NUM_OF_DECK_SLOTS_TO_RESERVE, roundUp: false);
            ISlotNumCalculator slotNumCalculator = _slotNumCalculatorFactory.CreateAntiNavalSlotNumCalculator(maxNumOfDeckSlots);
			
			return new AntiThreatTaskProducer(tasks, _aiCruiser, _prefabFactory, _taskFactory, antiNavalBuildOrder, navalThreatMonitor, slotNumCalculator);
		}

		public ITaskProducer CreateAntiRocketLauncherTaskProducer(ITaskList tasks, IDynamicBuildOrder antiRocketLauncherBuildOrder)
		{
            IThreatMonitor rocketLauncherThreatMonitor = _threatMonitorFactory.CreateRocketThreatMonitor();
			ISlotNumCalculator slotNumCalculator = _slotNumCalculatorFactory.CreateStaticSlotNumCalculator(numOfSlots: 1);

			return new AntiThreatTaskProducer(tasks, _aiCruiser, _prefabFactory, _taskFactory, antiRocketLauncherBuildOrder, rocketLauncherThreatMonitor, slotNumCalculator);
		}

        public ITaskProducer CreateAntiStealthTaskProducer(ITaskList tasks, IDynamicBuildOrder antiStealthBuildOrder)
        {
            IThreatMonitor stealthThreatMonitor = _threatMonitorFactory.CreateStealthThreatMonitor();
            ISlotNumCalculator slotNumCalculator = _slotNumCalculatorFactory.CreateStaticSlotNumCalculator(numOfSlots: 1);

            return new AntiThreatTaskProducer(tasks, _aiCruiser, _prefabFactory, _taskFactory, antiStealthBuildOrder, stealthThreatMonitor, slotNumCalculator);
        }
    }
}
