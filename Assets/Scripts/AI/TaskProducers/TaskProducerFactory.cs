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
    public class TaskProducerFactory
    {
        private readonly ICruiserController _aiCruiser;
        private readonly ITaskFactory _taskFactory;
        private readonly ThreatMonitorFactory _threatMonitorFactory;

        // For spy satellite launcher and shields.  All cruisers have at least 6
        // deck slots:
        // + Anti-air: 2
        // + Anti-ship: 2
        // + Shields/spy satellite: 2
        private const int NUM_OF_DECK_SLOTS_TO_RESERVE = 2;

        public TaskProducerFactory(
            ICruiserController aiCruiser,
            ITaskFactory taskFactory,
            ThreatMonitorFactory threatMonitorFactory)
        {
            Helper.AssertIsNotNull(aiCruiser, taskFactory, threatMonitorFactory);

            _aiCruiser = aiCruiser;
            _taskFactory = taskFactory;
            _threatMonitorFactory = threatMonitorFactory;
        }

        public IManagedDisposable CreateBasicTaskProducer(TaskList tasks, IDynamicBuildOrder buildOrder)
        {
            return new BasicTaskProducer(tasks, _aiCruiser, _taskFactory, buildOrder);
        }

        public IManagedDisposable CreateReplaceDestroyedBuildingsTaskProducer(TaskList tasks)
        {
            return new ReplaceDestroyedBuildingsTaskProducer(tasks, _aiCruiser, _taskFactory, StaticData.BuildingKeys);
        }

        public IManagedDisposable CreateAntiAirTaskProducer(TaskList tasks, IDynamicBuildOrder antiAirBuildOrder)
        {
            BaseThreatMonitor airThreatMonitor = _threatMonitorFactory.CreateDelayedThreatMonitor(_threatMonitorFactory.CreateAirThreatMonitor());

            int maxNumOfDeckSlots = Helper.Half(_aiCruiser.SlotAccessor.GetSlotCount(SlotType.Deck) - NUM_OF_DECK_SLOTS_TO_RESERVE, roundUp: true);
            ISlotNumCalculator slotNumCalculator = new AntiAirSlotNumCalculator(maxNumOfDeckSlots);

            return new AntiThreatTaskProducer(tasks, _aiCruiser, _taskFactory, antiAirBuildOrder, airThreatMonitor, slotNumCalculator);
        }

        public IManagedDisposable CreateAntiNavalTaskProducer(TaskList tasks, IDynamicBuildOrder antiNavalBuildOrder)
        {
            BaseThreatMonitor navalThreatMonitor = _threatMonitorFactory.CreateDelayedThreatMonitor(_threatMonitorFactory.CreateNavalThreatMonitor());

            int maxNumOfDeckSlots = Helper.Half(_aiCruiser.SlotAccessor.GetSlotCount(SlotType.Deck) - NUM_OF_DECK_SLOTS_TO_RESERVE, roundUp: false);
            ISlotNumCalculator slotNumCalculator = new AntiNavalSlotNumCalculator(maxNumOfDeckSlots);

            return new AntiThreatTaskProducer(tasks, _aiCruiser, _taskFactory, antiNavalBuildOrder, navalThreatMonitor, slotNumCalculator);
        }

        public IManagedDisposable CreateAntiRocketLauncherTaskProducer(TaskList tasks, IDynamicBuildOrder antiRocketLauncherBuildOrder)
        {
            BaseThreatMonitor rocketLauncherThreatMonitor = _threatMonitorFactory.CreateRocketThreatMonitor();
            ISlotNumCalculator slotNumCalculator = new StaticSlotNumCalculator(numOfSlots: 1);

            return new AntiThreatTaskProducer(tasks, _aiCruiser, _taskFactory, antiRocketLauncherBuildOrder, rocketLauncherThreatMonitor, slotNumCalculator);
        }

        public IManagedDisposable CreateAntiStealthTaskProducer(TaskList tasks, IDynamicBuildOrder antiStealthBuildOrder)
        {
            BaseThreatMonitor stealthThreatMonitor = _threatMonitorFactory.CreateStealthThreatMonitor();
            ISlotNumCalculator slotNumCalculator = new StaticSlotNumCalculator(numOfSlots: 1);

            return new AntiThreatTaskProducer(tasks, _aiCruiser, _taskFactory, antiStealthBuildOrder, stealthThreatMonitor, slotNumCalculator);
        }
    }
}
