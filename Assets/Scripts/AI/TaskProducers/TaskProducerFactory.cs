using BattleCruisers.AI.BuildOrders;
using BattleCruisers.AI.TaskProducers.SlotNumber;
using BattleCruisers.AI.Tasks;
using BattleCruisers.AI.ThreatMonitors;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Static;
using BattleCruisers.Utils;

namespace BattleCruisers.AI.TaskProducers
{
    public class TaskProducerFactory : ITaskProducerFactory
    {
        private readonly ICruiserController _aiCruiser;
        private readonly ITaskFactory _taskFactory;
        private readonly IThreatMonitorFactory _threatMonitorFactory;

        // For spy satellite launcher and shields.  All cruisers have at least 6
        // deck slots:
        // + Anti-air: 2
        // + Anti-ship: 2
        // + Shields/spy satellite: 2
        private const int NUM_OF_DECK_SLOTS_TO_RESERVE = 2;

        public TaskProducerFactory(
            ICruiserController aiCruiser,
            ITaskFactory taskFactory,
            IThreatMonitorFactory threatMonitorFactory)
        {
            Helper.AssertIsNotNull(aiCruiser, taskFactory, threatMonitorFactory);

            _aiCruiser = aiCruiser;
            _taskFactory = taskFactory;
            _threatMonitorFactory = threatMonitorFactory;
        }

        public ITaskProducer CreateBasicTaskProducer(ITaskList tasks, IDynamicBuildOrder buildOrder)
        {
            return new BasicTaskProducer(tasks, _aiCruiser, _taskFactory, buildOrder);
        }

        public ITaskProducer CreateReplaceDestroyedBuildingsTaskProducer(ITaskList tasks)
        {
            return new ReplaceDestroyedBuildingsTaskProducer(tasks, _aiCruiser, _taskFactory, StaticData.BuildingKeys);
        }

        public ITaskProducer CreateAntiAirTaskProducer(ITaskList tasks, IDynamicBuildOrder antiAirBuildOrder)
        {
            IThreatMonitor airThreatMonitor = _threatMonitorFactory.CreateDelayedThreatMonitor(_threatMonitorFactory.CreateAirThreatMonitor());

            int maxNumOfDeckSlots = Helper.Half(_aiCruiser.SlotAccessor.GetSlotCount(SlotType.Deck) - NUM_OF_DECK_SLOTS_TO_RESERVE, roundUp: true);
            ISlotNumCalculator slotNumCalculator = new AntiAirSlotNumCalculator(maxNumOfDeckSlots);

            return new AntiThreatTaskProducer(tasks, _aiCruiser, _taskFactory, antiAirBuildOrder, airThreatMonitor, slotNumCalculator);
        }

        public ITaskProducer CreateAntiNavalTaskProducer(ITaskList tasks, IDynamicBuildOrder antiNavalBuildOrder)
        {
            IThreatMonitor navalThreatMonitor = _threatMonitorFactory.CreateDelayedThreatMonitor(_threatMonitorFactory.CreateNavalThreatMonitor());

            int maxNumOfDeckSlots = Helper.Half(_aiCruiser.SlotAccessor.GetSlotCount(SlotType.Deck) - NUM_OF_DECK_SLOTS_TO_RESERVE, roundUp: false);
            ISlotNumCalculator slotNumCalculator = new AntiNavalSlotNumCalculator(maxNumOfDeckSlots);

            return new AntiThreatTaskProducer(tasks, _aiCruiser, _taskFactory, antiNavalBuildOrder, navalThreatMonitor, slotNumCalculator);
        }

        public ITaskProducer CreateAntiRocketLauncherTaskProducer(ITaskList tasks, IDynamicBuildOrder antiRocketLauncherBuildOrder)
        {
            IThreatMonitor rocketLauncherThreatMonitor = _threatMonitorFactory.CreateRocketThreatMonitor();
            ISlotNumCalculator slotNumCalculator = new StaticSlotNumCalculator(numOfSlots: 1);

            return new AntiThreatTaskProducer(tasks, _aiCruiser, _taskFactory, antiRocketLauncherBuildOrder, rocketLauncherThreatMonitor, slotNumCalculator);
        }

        public ITaskProducer CreateAntiStealthTaskProducer(ITaskList tasks, IDynamicBuildOrder antiStealthBuildOrder)
        {
            IThreatMonitor stealthThreatMonitor = _threatMonitorFactory.CreateStealthThreatMonitor();
            ISlotNumCalculator slotNumCalculator = new StaticSlotNumCalculator(numOfSlots: 1);

            return new AntiThreatTaskProducer(tasks, _aiCruiser, _taskFactory, antiStealthBuildOrder, stealthThreatMonitor, slotNumCalculator);
        }
    }
}
