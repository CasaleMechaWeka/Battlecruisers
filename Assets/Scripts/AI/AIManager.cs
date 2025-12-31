using System.Collections.Generic;
using BattleCruisers.AI.BuildOrders;
using BattleCruisers.AI.Drones;
using BattleCruisers.AI.Drones.BuildingMonitors;
using BattleCruisers.AI.FactoryManagers;
using BattleCruisers.AI.TaskProducers;
using BattleCruisers.AI.Tasks;
using BattleCruisers.AI.ThreatMonitors;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Data.Static.Strategies.Helper;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.AI
{
    public class AIManager
    {
        private readonly IDeferrer _deferrer;
        private readonly ThreatMonitorFactory _threatMonitorFactory;
        private readonly FactoryManagerFactory _factoryManagerFactory;
        private readonly BuildOrderFactory _buildOrderFactory;

        // For spy satellite launcher and shields.  All cruisers have at least 6
        // deck slots:
        // + Anti-air: 2
        // + Anti-ship: 2
        // + Shields/spy satellite: 2
        private const int NUM_OF_DECK_SLOTS_TO_RESERVE = 2;

        public AIManager(
            IDeferrer deferrer,
            ICruiserController playerCruiser,
            IStrategyFactory strategyFactory)
        {
            Helper.AssertIsNotNull(deferrer, playerCruiser, strategyFactory);

            _deferrer = deferrer;

            _threatMonitorFactory = new ThreatMonitorFactory(playerCruiser, TimeBC.Instance, deferrer);
            _factoryManagerFactory = new FactoryManagerFactory(DataProvider.GameModel, _threatMonitorFactory);

            SlotAssigner slotAssigner = new SlotAssigner();
            _buildOrderFactory = new BuildOrderFactory(slotAssigner, strategyFactory);
        }

        public IManagedDisposable CreateAI(LevelInfo levelInfo)
        {
            // Manage AI unit factories (needs to be before the AI strategy is created,
            // otherwise miss started construction event for first building :) )
            _factoryManagerFactory.CreateNavalFactoryManager(levelInfo.AICruiser);
            _factoryManagerFactory.CreateAirfactoryManager(levelInfo.AICruiser);

            ITaskFactory taskFactory = new TaskFactory(levelInfo.AICruiser, _deferrer);


            ICruiserController aiCruiser = levelInfo.AICruiser;
            TaskList tasks = new TaskList();
            List<IManagedDisposable> taskProducers = new List<IManagedDisposable>();

            // Base build order, main strategy
            BuildingKey[] advancedBuildOrder = _buildOrderFactory.CreateAdaptiveBuildOrder(levelInfo);
            taskProducers.Add(new BasicTaskProducer(tasks, aiCruiser, taskFactory, advancedBuildOrder));

            // Anti air
            BuildingKey[] antiAirBuildOrder = _buildOrderFactory.CreateAntiAirBuildOrder(levelInfo);
            BaseThreatMonitor airThreatMonitor = _threatMonitorFactory.CreateDelayedThreatMonitor(_threatMonitorFactory.CreateAirThreatMonitor());
            int maxNumOfDeckSlots = Helper.Half(aiCruiser.SlotAccessor.GetSlotCount(SlotType.Deck) - NUM_OF_DECK_SLOTS_TO_RESERVE, roundUp: true);
            SlotNumCalculator slotNumCalculator = new SlotNumCalculator(maxNumOfDeckSlots, 0, 2, 4);

            taskProducers.Add(new AntiThreatTaskProducer(tasks, aiCruiser, taskFactory, antiAirBuildOrder, airThreatMonitor, slotNumCalculator, levelInfo));

            // Anti naval
            BuildingKey[] antiNavalBuildOrder = _buildOrderFactory.CreateAntiNavalBuildOrder(levelInfo);
            BaseThreatMonitor navalThreatMonitor = _threatMonitorFactory.CreateDelayedThreatMonitor(_threatMonitorFactory.CreateNavalThreatMonitor());
            // keep the SlotNumCalculator
            maxNumOfDeckSlots = Helper.Half(aiCruiser.SlotAccessor.GetSlotCount(SlotType.Deck) - NUM_OF_DECK_SLOTS_TO_RESERVE, roundUp: false);

            taskProducers.Add(new AntiThreatTaskProducer(tasks, aiCruiser, taskFactory, antiNavalBuildOrder, navalThreatMonitor, slotNumCalculator, levelInfo));

            // Anti rocket
            if (DataProvider.GameModel.IsBuildingUnlocked(StaticPrefabKeys.Buildings.TeslaCoil))
            {
                BaseThreatMonitor rocketLauncherThreatMonitor = _threatMonitorFactory.CreateRocketThreatMonitor();
                slotNumCalculator = new SlotNumCalculator(1, 0, 1, 1);

                taskProducers.Add(new AntiThreatTaskProducer(tasks,
                                                             aiCruiser,
                                                             taskFactory,
                                                             new BuildingKey[] { StaticPrefabKeys.Buildings.TeslaCoil },
                                                             rocketLauncherThreatMonitor,
                                                             slotNumCalculator,
                                                             levelInfo));
            }

            // Anti stealth
            if (DataProvider.GameModel.IsBuildingUnlocked(StaticPrefabKeys.Buildings.StealthGenerator))
            {
                BaseThreatMonitor stealthThreatMonitor = _threatMonitorFactory.CreateStealthThreatMonitor();
                // keep the SlotNumCalculator
                taskProducers.Add(new AntiThreatTaskProducer(tasks,
                                                             aiCruiser,
                                                             taskFactory,
                                                             new BuildingKey[] { StaticPrefabKeys.Buildings.SpySatelliteLauncher },
                                                             stealthThreatMonitor,
                                                             slotNumCalculator,
                                                             levelInfo));
            }

            taskProducers.Add(new ReplaceDestroyedBuildingsTaskProducer(tasks, aiCruiser, taskFactory, StaticData.BuildingKeys));

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
