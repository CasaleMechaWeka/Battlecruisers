using System.Collections.Generic;
using BattleCruisers.AI.TaskProducers;
using BattleCruisers.Data;
using BattleCruisers.Data.Models.PrefabKeys;
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
        private readonly IBuildOrderProvider _buildOrderProvider;

        public AIFactory(ITaskProducerFactory taskProducerFactory, IBuildOrderProvider buildOrderProvider)
        {
            Helper.AssertIsNotNull(taskProducerFactory, buildOrderProvider);

            _taskProducerFactory = taskProducerFactory;
            _buildOrderProvider = buildOrderProvider;
        }

        /// <summary>
        /// Creates a basic AI that:
        /// 1. Follows a preset build order
        /// 2. Replaces destroyed buildings
        /// </summary>
        public void CreateBasicAI(ILevel level)
        {
            ITaskList tasks = new TaskList();

            IList<IPrefabKey> basicBuildOrder = _buildOrderProvider.GetBasicBuildOrder(level.Num);
            _taskProducerFactory.CreateBasicTaskProducer(tasks, basicBuildOrder);

            _taskProducerFactory.CreateReplaceDestroyedBuildingsTaskProducer(tasks);
            
            new TaskConsumer(tasks);
        }
		
        /// <summary>
        /// Creates an adaptive AI that:
        /// 1. Follows a base strategy (eg:  boom, turtle, ect)
        /// 2. Responds to threats (eg: air, naval)
        /// 3. Replaces destroyed buildings
        /// </summary>
		public void CreateAdaptiveAI(ILevel level)
		{
            ITaskList tasks = new TaskList();

            IList<IPrefabKey> advancedBuildOrder = _buildOrderProvider.GetAdvancedBuildOrder(level.Num);
            _taskProducerFactory.CreateBasicTaskProducer(tasks, advancedBuildOrder);

            IList<IPrefabKey> antiAirBuildOrder = _buildOrderProvider.GetAntiAirBuildOrder(level.Num);
            _taskProducerFactory.CreateAntiAirTaskProducer(tasks, antiAirBuildOrder);

			IList<IPrefabKey> antiNavalBuildOrder = _buildOrderProvider.GetAntiNavalBuildOrder(level.Num);
			_taskProducerFactory.CreateAntiNavalTaskProducer(tasks, antiNavalBuildOrder);

			// FELIX  Anti rocket!
			// FELIX  Anti stealth!
			
            _taskProducerFactory.CreateReplaceDestroyedBuildingsTaskProducer(tasks);

			new TaskConsumer(tasks);
		}
    }
}
