using BattleCruisers.AI;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.BuildOrders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Drones.BuildingMonitors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Drones.Strategies;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.TaskProducers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI
{
    /// <summary>
    /// An AI consists of one or more task producers and a single task consumer.
    /// 
    /// This factory creates different types of AI.
    /// </summary>
    public class PvPAIFactory : IPvPAIFactory
    {
        private readonly IPvPTaskProducerFactory _taskProducerFactory;
        private readonly IPvPBuildOrderFactory _buildOrderFactory;
        private readonly IPvPFactoryMonitorFactory _factoryMonitorFactory;

        public PvPAIFactory(IPvPTaskProducerFactory taskProducerFactory, IPvPBuildOrderFactory buildOrderFactory, IPvPFactoryMonitorFactory factoryMonitorFactory)
        {
            PvPHelper.AssertIsNotNull(taskProducerFactory, buildOrderFactory, factoryMonitorFactory);

            _taskProducerFactory = taskProducerFactory;
            _buildOrderFactory = buildOrderFactory;
            _factoryMonitorFactory = factoryMonitorFactory;
        }

        /// <summary>
        /// Creates a basic AI that:
        /// 1. Follows a base strategy (eg:  balanced, boom or rush)
        /// 2. Replaces destroyed buildings
        /// </summary>
        public IPvPArtificialIntelligence CreateBasicAI(IPvPLevelInfo levelInfo)
        {
            IPvPTaskList tasks = new PvPTaskList();

            IList<IPvPTaskProducer> taskProducers = new List<IPvPTaskProducer>();

            IPvPDynamicBuildOrder basicBuildOrder = _buildOrderFactory.CreateBasicBuildOrder(levelInfo);
            taskProducers.Add(_taskProducerFactory.CreateBasicTaskProducer(tasks, basicBuildOrder));

            taskProducers.Add(_taskProducerFactory.CreateReplaceDestroyedBuildingsTaskProducer(tasks));

            return CreateAI(levelInfo.AICruiser, tasks, taskProducers);
        }

        /// <summary>
        /// Creates an adaptive AI that:
        /// 1. Follows a base strategy (eg:  balanced, boom or rush)
        /// 2. Responds to threats (eg: air, naval)
        /// 3. Replaces destroyed buildings
        /// </summary>
        public IPvPArtificialIntelligence CreateAdaptiveAI(IPvPLevelInfo levelInfo)
        {
            IPvPTaskList tasks = new PvPTaskList();
            IList<IPvPTaskProducer> taskProducers = new List<IPvPTaskProducer>();

            // Base build order, main strategy
            IPvPDynamicBuildOrder advancedBuildOrder = _buildOrderFactory.CreateAdaptiveBuildOrder(levelInfo);
            taskProducers.Add(_taskProducerFactory.CreateBasicTaskProducer(tasks, advancedBuildOrder));

            // Anti air
            IPvPDynamicBuildOrder antiAirBuildOrder = _buildOrderFactory.CreateAntiAirBuildOrder(levelInfo);
            taskProducers.Add(_taskProducerFactory.CreateAntiAirTaskProducer(tasks, antiAirBuildOrder));

            // Anti naval
            IPvPDynamicBuildOrder antiNavalBuildOrder = _buildOrderFactory.CreateAntiNavalBuildOrder(levelInfo);
            taskProducers.Add(_taskProducerFactory.CreateAntiNavalTaskProducer(tasks, antiNavalBuildOrder));

            // Anti rocket
            if (_buildOrderFactory.IsAntiRocketBuildOrderAvailable(levelInfo))
            {
                taskProducers.Add(_taskProducerFactory.CreateAntiRocketLauncherTaskProducer(tasks, _buildOrderFactory.CreateAntiRocketBuildOrder()));
            }

            // Anti stealth
            if (_buildOrderFactory.IsAntiStealthBuildOrderAvailable(levelInfo))
            {
                taskProducers.Add(_taskProducerFactory.CreateAntiStealthTaskProducer(tasks, _buildOrderFactory.CreateAntiStealthBuildOrder()));
            }

            taskProducers.Add(_taskProducerFactory.CreateReplaceDestroyedBuildingsTaskProducer(tasks));

            return CreateAI(levelInfo.AICruiser, tasks, taskProducers);
        }

        private IPvPArtificialIntelligence CreateAI(IPvPCruiserController aiCruiser, IPvPTaskList tasks, IList<IPvPTaskProducer> taskProducers)
        {
            PvPTaskConsumer taskConsumer = new PvPTaskConsumer(tasks);
            PvPDroneConsumerFocusManager focusManager = CreateDroneFocusManager(aiCruiser);

            return new PvPArtificialIntelligence(taskConsumer, taskProducers, focusManager);
        }

        private PvPDroneConsumerFocusManager CreateDroneFocusManager(IPvPCruiserController aiCruiser)
        {
            IPvPFactoryAnalyzer factoryAnalyzer
                = new PvPFactoryAnalyzer(
                    new PvPFactoriesMonitor(aiCruiser.BuildingMonitor, _factoryMonitorFactory),
                    new PvPFactoryWastingDronesFilter());

            IPvPInProgressBuildingMonitor inProgressBuildingMonitor = new PvPInProgressBuildingMonitor(aiCruiser);

            IPvPDroneConsumerFocusHelper focusHelper
                = new PvPDroneConsumerFocusHelper(
                    aiCruiser.DroneManager,
                    factoryAnalyzer,
                    new PvPAffordableInProgressNonFocusedProvider(aiCruiser.DroneManager, inProgressBuildingMonitor));

            return
                new PvPDroneConsumerFocusManager(
                    new PvPResponsiveStrategy(),
                    aiCruiser,
                    focusHelper);
        }
    }
}
