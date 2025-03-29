using BattleCruisers.AI.BuildOrders;
using BattleCruisers.AI.Drones;
using BattleCruisers.AI.Drones.BuildingMonitors;
using BattleCruisers.AI.TaskProducers;
using BattleCruisers.AI.Tasks;
using BattleCruisers.AI.ThreatMonitors;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data.Static;
using BattleCruisers.Utils;
using System.Collections.Generic;

namespace BattleCruisers.AI
{
    /// <summary>
    /// An AI consists of one or more task producers and a single task consumer.
    /// 
    /// This factory creates different types of AI.
    /// </summary>
    public class AIFactory
    {
        private readonly BuildOrderFactory _buildOrderFactory;
        private readonly ITaskFactory _taskFactory;
        private readonly ThreatMonitorFactory _threatMonitorFactory;

        // For spy satellite launcher and shields.  All cruisers have at least 6
        // deck slots:
        // + Anti-air: 2
        // + Anti-ship: 2
        // + Shields/spy satellite: 2
        private const int NUM_OF_DECK_SLOTS_TO_RESERVE = 2;


        public AIFactory(BuildOrderFactory buildOrderFactory, ITaskFactory taskFactory, ThreatMonitorFactory threatMonitorFactory)
        {
            Helper.AssertIsNotNull(buildOrderFactory, taskFactory, threatMonitorFactory);

            _buildOrderFactory = buildOrderFactory;
            _taskFactory = taskFactory;
            _threatMonitorFactory = threatMonitorFactory;
        }

        /// <summary>
        /// Creates an adaptive AI that:
        /// 1. Follows a base strategy (eg:  balanced, boom or rush)
        /// 2. Responds to threats (eg: air, naval)
        /// 3. Replaces destroyed buildings
        /// </summary>
        public IManagedDisposable CreateAdaptiveAI(LevelInfo levelInfo)
        {
            ICruiserController aiCruiser = levelInfo.AICruiser;
            TaskList tasks = new TaskList();
            IList<IManagedDisposable> taskProducers = new List<IManagedDisposable>();

            // Base build order, main strategy
            IDynamicBuildOrder advancedBuildOrder = _buildOrderFactory.CreateAdaptiveBuildOrder(levelInfo);
            taskProducers.Add(new BasicTaskProducer(tasks, aiCruiser, _taskFactory, advancedBuildOrder));

            // Anti air
            IDynamicBuildOrder antiAirBuildOrder = _buildOrderFactory.CreateAntiAirBuildOrder(levelInfo);
            BaseThreatMonitor airThreatMonitor = _threatMonitorFactory.CreateDelayedThreatMonitor(_threatMonitorFactory.CreateAirThreatMonitor());
            int maxNumOfDeckSlots = Helper.Half(aiCruiser.SlotAccessor.GetSlotCount(SlotType.Deck) - NUM_OF_DECK_SLOTS_TO_RESERVE, roundUp: true);
            SlotNumCalculator slotNumCalculator = new SlotNumCalculator(maxNumOfDeckSlots, 0, 2, 4);

            taskProducers.Add(new AntiThreatTaskProducer(tasks, aiCruiser, _taskFactory, antiAirBuildOrder, airThreatMonitor, slotNumCalculator));

            // Anti naval
            IDynamicBuildOrder antiNavalBuildOrder = _buildOrderFactory.CreateAntiNavalBuildOrder(levelInfo);
            BaseThreatMonitor navalThreatMonitor = _threatMonitorFactory.CreateDelayedThreatMonitor(_threatMonitorFactory.CreateNavalThreatMonitor());
            // keep the SlotNumCalculator
            maxNumOfDeckSlots = Helper.Half(aiCruiser.SlotAccessor.GetSlotCount(SlotType.Deck) - NUM_OF_DECK_SLOTS_TO_RESERVE, roundUp: false);

            taskProducers.Add(new AntiThreatTaskProducer(tasks, aiCruiser, _taskFactory, antiNavalBuildOrder, navalThreatMonitor, slotNumCalculator));

            // Anti rocket
            if (_buildOrderFactory.IsAntiRocketBuildOrderAvailable())
            {
                BaseThreatMonitor rocketLauncherThreatMonitor = _threatMonitorFactory.CreateRocketThreatMonitor();
                slotNumCalculator = new SlotNumCalculator(1, 0, 1, 1);

                taskProducers.Add(new AntiThreatTaskProducer(tasks, aiCruiser, _taskFactory, _buildOrderFactory.CreateAntiRocketBuildOrder(), rocketLauncherThreatMonitor, slotNumCalculator));
            }

            // Anti stealth
            if (_buildOrderFactory.IsAntiStealthBuildOrderAvailable())
            {
                BaseThreatMonitor stealthThreatMonitor = _threatMonitorFactory.CreateStealthThreatMonitor();
                // keep the SlotNumCalculator
                taskProducers.Add(new AntiThreatTaskProducer(tasks, aiCruiser, _taskFactory, _buildOrderFactory.CreateAntiStealthBuildOrder(), stealthThreatMonitor, slotNumCalculator));
            }

            taskProducers.Add(new ReplaceDestroyedBuildingsTaskProducer(tasks, aiCruiser, _taskFactory, StaticData.BuildingKeys));

            return CreateAI(levelInfo.AICruiser, tasks, taskProducers);
        }

        private IManagedDisposable CreateAI(ICruiserController aiCruiser, TaskList tasks, IList<IManagedDisposable> taskProducers)
        {
            TaskConsumer taskConsumer = new TaskConsumer(tasks);
            DroneConsumerFocusManager focusManager = CreateDroneFocusManager(aiCruiser);

            return new ArtificialIntelligence(taskConsumer, taskProducers, focusManager);
        }

        private DroneConsumerFocusManager CreateDroneFocusManager(ICruiserController aiCruiser)
        {
            FactoryAnalyzer factoryAnalyzer
                = new FactoryAnalyzer(
                    new FactoriesMonitor(aiCruiser.BuildingMonitor),
                    new FactoryWastingDronesFilter());

            InProgressBuildingMonitor inProgressBuildingMonitor = new InProgressBuildingMonitor(aiCruiser);

            DroneConsumerFocusHelper focusHelper
                = new DroneConsumerFocusHelper(
                    aiCruiser.DroneManager,
                    factoryAnalyzer,
                    new AffordableInProgressNonFocusedProvider(aiCruiser.DroneManager, inProgressBuildingMonitor));

            return
                new DroneConsumerFocusManager(
                    new ResponsiveStrategy(),
                    aiCruiser,
                    focusHelper);
        }
    }
}
