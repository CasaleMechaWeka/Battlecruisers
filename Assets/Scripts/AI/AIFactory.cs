using System.Collections.Generic;
using BattleCruisers.AI.TaskProducers;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;

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

        public AIFactory(ITaskProducerFactory taskProducerFactory)
        {
            _taskProducerFactory = taskProducerFactory;
        }

        /// <summary>
        /// Creates a basic AI that:
        /// 1. Follows a preset build order
        /// 2. Replaces destroyed buildings
        /// </summary>
        public void CreateBasicAI(ICruiserController aiCruiser, IList<IPrefabKey> buildOrder)
        {
			ITaskList tasks = new TaskList();

            _taskProducerFactory.CreateBasicTaskProducer(tasks, buildOrder);
            _taskProducerFactory.CreateReplaceDestroyedBuildingsTaskProducer(tasks);
			
            new TaskConsumer(tasks);
		}
    }
}
