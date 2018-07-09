using BattleCruisers.AI.BuildOrders;
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

        public AIFactory(ITaskProducerFactory taskProducerFactory, IBuildOrderFactory buildOrderFactory)
        {
            Helper.AssertIsNotNull(taskProducerFactory, buildOrderFactory);

            _taskProducerFactory = taskProducerFactory;
            _buildOrderFactory = buildOrderFactory;
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
            taskProducers.Add(_taskProducerFactory.CreatePostFactoryTaskProducer(tasks));
            
            ITaskConsumer taskConsumer = new TaskConsumer(tasks);

            return new ArtificialIntelligence(taskConsumer, taskProducers);
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
            taskProducers.Add(_taskProducerFactory.CreatePostFactoryTaskProducer(tasks));

            ITaskConsumer taskConsumer = new TaskConsumer(tasks);

            return new ArtificialIntelligence(taskConsumer, taskProducers);
		}
    }
}
