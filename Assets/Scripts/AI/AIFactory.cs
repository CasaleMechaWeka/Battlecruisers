using BattleCruisers.AI.BuildOrders;
using BattleCruisers.AI.Drones;
using BattleCruisers.AI.Drones.BuildingMonitors;
using BattleCruisers.AI.Drones.Strategies;
using BattleCruisers.AI.TaskProducers;
using BattleCruisers.Utils;
using System.Collections.Generic;

namespace BattleCruisers.AI
{
    /// <summary>
    /// An AI consists of one or more task producers and a single task consumer.
    /// 
    /// This factory creates different types of AI.
    /// </summary>
    public class AIFactory : IAIFactory
    {
        private readonly ITaskProducerFactory _taskProducerFactory;
        private readonly IBuildOrderFactory _buildOrderFactory;
        private readonly IFactoryMonitorFactory _factoryMonitorFactory;

        public AIFactory(ITaskProducerFactory taskProducerFactory, IBuildOrderFactory buildOrderFactory, IFactoryMonitorFactory factoryMonitorFactory)
        {
            Helper.AssertIsNotNull(taskProducerFactory, buildOrderFactory, factoryMonitorFactory);

            _taskProducerFactory = taskProducerFactory;
            _buildOrderFactory = buildOrderFactory;
            _factoryMonitorFactory = factoryMonitorFactory;
        }

		/// <summary>
		/// Creates a basic AI that:
		/// 1. Follows a base strategy (eg:  balanced, boom or rush)
		/// 2. Replaces destroyed buildings
		/// </summary>
		public IArtificialIntelligence CreateBasicAI(ILevelInfo levelInfo)
        {
            ITaskList tasks = new TaskList();

            IList<ITaskProducer> taskProducers = new List<ITaskProducer>();

            IDynamicBuildOrder basicBuildOrder = _buildOrderFactory.CreateBasicBuildOrder(levelInfo);
            taskProducers.Add(_taskProducerFactory.CreateBasicTaskProducer(tasks, basicBuildOrder));

            taskProducers.Add(_taskProducerFactory.CreateReplaceDestroyedBuildingsTaskProducer(tasks));

            return CreateAI(levelInfo, tasks, taskProducers);
        }

		/// <summary>
		/// Creates an adaptive AI that:
		/// 1. Follows a base strategy (eg:  balanced, boom or rush)
		/// 2. Responds to threats (eg: air, naval)
		/// 3. Replaces destroyed buildings
		/// </summary>
		public IArtificialIntelligence CreateAdaptiveAI(ILevelInfo levelInfo)
        {
            ITaskList tasks = new TaskList();
            IList<ITaskProducer> taskProducers = new List<ITaskProducer>();

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
            if (_buildOrderFactory.IsAntiRocketBuildOrderAvailable(levelInfo.LevelNum))
            {
                taskProducers.Add(_taskProducerFactory.CreateAntiRocketLauncherTaskProducer(tasks, _buildOrderFactory.CreateAntiRocketBuildOrder()));
            }

            // Anti stealth
            if (_buildOrderFactory.IsAntiStealthBuildOrderAvailable(levelInfo.LevelNum))
            {
                taskProducers.Add(_taskProducerFactory.CreateAntiStealthTaskProducer(tasks, _buildOrderFactory.CreateAntiStealthBuildOrder()));
            }

            taskProducers.Add(_taskProducerFactory.CreateReplaceDestroyedBuildingsTaskProducer(tasks));

            return CreateAI(levelInfo, tasks, taskProducers);
        }

        private IArtificialIntelligence CreateAI(ILevelInfo levelInfo, ITaskList tasks, IList<ITaskProducer> taskProducers)
        {
            TaskConsumer taskConsumer = new TaskConsumer(tasks);
            DroneConsumerFocusManager focusManager = CreateDroneFocusManager(levelInfo);

            return new ArtificialIntelligence(taskConsumer, taskProducers, focusManager);
        }

        private DroneConsumerFocusManager CreateDroneFocusManager(ILevelInfo levelInfo)
        {
            IFactoryAnalyzer factoryAnalyzer
                = new FactoryAnalyzer(
                    new FactoriesMonitor(levelInfo.AICruiser.BuildingMonitor, _factoryMonitorFactory),
                    new FactoryWastingDronesFilter());

            IInProgressBuildingMonitor inProgressBuildingMonitor = new InProgressBuildingMonitor(levelInfo.AICruiser);

            IDroneConsumerFocusHelper focusHelper
                = new DroneConsumerFocusHelper(
                    levelInfo.AICruiser.DroneManager,
                    factoryAnalyzer,
                    new AffordableInProgressNonFocusedProvider(levelInfo.AICruiser.DroneManager, inProgressBuildingMonitor));

            return
                new DroneConsumerFocusManager(
                    new ResponsiveStrategy(),
                    levelInfo.AICruiser,
                    focusHelper);
        }
    }
}
