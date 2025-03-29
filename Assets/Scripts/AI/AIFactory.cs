using BattleCruisers.AI.BuildOrders;
using BattleCruisers.AI.Drones;
using BattleCruisers.AI.Drones.BuildingMonitors;
using BattleCruisers.AI.TaskProducers;
using BattleCruisers.Cruisers;
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
        private readonly TaskProducerFactory _taskProducerFactory;
        private readonly BuildOrderFactory _buildOrderFactory;

        public AIFactory(TaskProducerFactory taskProducerFactory, BuildOrderFactory buildOrderFactory)
        {
            Helper.AssertIsNotNull(taskProducerFactory, buildOrderFactory);

            _taskProducerFactory = taskProducerFactory;
            _buildOrderFactory = buildOrderFactory;
        }

        /// <summary>
        /// Creates an adaptive AI that:
        /// 1. Follows a base strategy (eg:  balanced, boom or rush)
        /// 2. Responds to threats (eg: air, naval)
        /// 3. Replaces destroyed buildings
        /// </summary>
        public IManagedDisposable CreateAdaptiveAI(LevelInfo levelInfo)
        {
            TaskList tasks = new TaskList();
            IList<IManagedDisposable> taskProducers = new List<IManagedDisposable>();

            // Base build order, main strategy
            IDynamicBuildOrder advancedBuildOrder = _buildOrderFactory.CreateAdaptiveBuildOrder(levelInfo);
            taskProducers.Add(_taskProducerFactory.CreateBasicTaskProducer(tasks, advancedBuildOrder));

            // Anti air
            IDynamicBuildOrder antiAirBuildOrder = _buildOrderFactory.CreateAntiAirBuildOrder(levelInfo);
            taskProducers.Add(_taskProducerFactory.CreateAntiAirTaskProducer(tasks, antiAirBuildOrder));

            // Anti naval
            IDynamicBuildOrder antiNavalBuildOrder = _buildOrderFactory.CreateAntiNavalBuildOrder(levelInfo);
            taskProducers.Add(_taskProducerFactory.CreateAntiNavalTaskProducer(tasks, antiNavalBuildOrder));

            // Anti rocket
            if (_buildOrderFactory.IsAntiRocketBuildOrderAvailable())
            {
                taskProducers.Add(_taskProducerFactory.CreateAntiRocketLauncherTaskProducer(tasks, _buildOrderFactory.CreateAntiRocketBuildOrder()));
            }

            // Anti stealth
            if (_buildOrderFactory.IsAntiStealthBuildOrderAvailable())
            {
                taskProducers.Add(_taskProducerFactory.CreateAntiStealthTaskProducer(tasks, _buildOrderFactory.CreateAntiStealthBuildOrder()));
            }

            taskProducers.Add(_taskProducerFactory.CreateReplaceDestroyedBuildingsTaskProducer(tasks));

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
