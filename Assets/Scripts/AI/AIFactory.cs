using BattleCruisers.AI.BuildOrders;
using BattleCruisers.AI.TaskProducers;
using BattleCruisers.Utils;

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
		public void CreateBasicAI(ILevelInfo levelInfo)
        {
            ITaskList tasks = new TaskList();

            IDynamicBuildOrder basicBuildOrder = _buildOrderFactory.CreateBasicBuildOrder(levelInfo);
            _taskProducerFactory.CreateBasicTaskProducer(tasks, basicBuildOrder);

            _taskProducerFactory.CreateReplaceDestroyedBuildingsTaskProducer(tasks);
            
            new TaskConsumer(tasks);
        }

		/// <summary>
		/// Creates an adaptive AI that:
		/// 1. Follows a base strategy (eg:  balanced, boom or rush)
		/// 2. Responds to threats (eg: air, naval)
		/// 3. Replaces destroyed buildings
		/// </summary>
		public void CreateAdaptiveAI(ILevelInfo levelInfo)
		{
            ITaskList tasks = new TaskList();

            // Base build order, main strategy
            IDynamicBuildOrder advancedBuildOrder = _buildOrderFactory.CreateAdaptiveBuildOrder(levelInfo);
            _taskProducerFactory.CreateBasicTaskProducer(tasks, advancedBuildOrder);

            // Anti air
            IDynamicBuildOrder antiAirBuildOrder = _buildOrderFactory.CreateAntiAirBuildOrder(levelInfo);
            _taskProducerFactory.CreateAntiAirTaskProducer(tasks, antiAirBuildOrder);

            // Anti naval
            IDynamicBuildOrder antiNavalBuildOrder = _buildOrderFactory.CreateAntiNavalBuildOrder(levelInfo);
			_taskProducerFactory.CreateAntiNavalTaskProducer(tasks, antiNavalBuildOrder);

            // Anti rocket
            if (_buildOrderFactory.IsAntiRocketBuildOrderAvailable(levelInfo.LevelNum))
            {
                _taskProducerFactory.CreateAntiRocketLauncherTaskProducer(tasks, _buildOrderFactory.CreateAntiRocketBuildOrder());
            }

            // Anti stealth
            if (_buildOrderFactory.IsAntiStealthBuildOrderAvailable(levelInfo.LevelNum))
            {
                _taskProducerFactory.CreateAntiStealthTaskProducer(tasks, _buildOrderFactory.CreateAntiStealthBuildOrder());
            }
			
            _taskProducerFactory.CreateReplaceDestroyedBuildingsTaskProducer(tasks);

			new TaskConsumer(tasks);
		}
    }
}
