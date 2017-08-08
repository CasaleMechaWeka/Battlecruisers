using System.Collections.Generic;
using BattleCruisers.AI.Providers;
using BattleCruisers.AI.TaskProducers;
using BattleCruisers.Cruisers;
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
        public void CreateBasicAI(ILevel level, ISlotWrapper slotWrapper)
        {
            ITaskList tasks = new TaskList();

            IList<IPrefabKey> basicBuildOrder = _buildOrderProvider.GetBasicBuildOrder(level.Num, slotWrapper);
            _taskProducerFactory.CreateBasicTaskProducer(tasks, basicBuildOrder);

            _taskProducerFactory.CreateReplaceDestroyedBuildingsTaskProducer(tasks);
            
            new TaskConsumer(tasks);
        }

		/// <summary>
		/// Creates an adaptive AI that:
		/// 1. Follows a base strategy (eg:  balanced, boom or rushIStaticData staticData)
		/// 2. Responds to threats (eg: air, naval)
		/// 3. Replaces destroyed buildings
		/// </summary>
		public void CreateAdaptiveAI(ILevel level, ISlotWrapper slotWrapper)
		{
            ITaskList tasks = new TaskList();

            // Base build order, main strategy
            IList<IPrefabKey> advancedBuildOrder = _buildOrderProvider.GetAdaptiveBuildOrder(level.Num, slotWrapper);
            _taskProducerFactory.CreateBasicTaskProducer(tasks, advancedBuildOrder);

            // Anti air
            IList<IPrefabKey> antiAirBuildOrder = _buildOrderProvider.GetAntiAirBuildOrder(level.Num);
            _taskProducerFactory.CreateAntiAirTaskProducer(tasks, antiAirBuildOrder);

            // Anti naval
			IList<IPrefabKey> antiNavalBuildOrder = _buildOrderProvider.GetAntiNavalBuildOrder(level.Num);
			_taskProducerFactory.CreateAntiNavalTaskProducer(tasks, antiNavalBuildOrder);

            // Anti rocket
            if (_buildOrderProvider.IsAntiRocketBuildOrderAvailable(level.Num))
            {
                _taskProducerFactory.CreateAntiRocketLauncherTaskProducer(tasks, _buildOrderProvider.AntiRocketBuildOrder);
            }

			// FELIX  Anti stealth!
			
            _taskProducerFactory.CreateReplaceDestroyedBuildingsTaskProducer(tasks);

			new TaskConsumer(tasks);
		}
    }
}
