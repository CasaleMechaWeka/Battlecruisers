using System.Collections.Generic;
using BattleCruisers.AI.TaskProducers;
using BattleCruisers.AI.Tasks;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.PrefabKeys;
using BattleCruisers.Fetchers;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.AI
{
    public class AIFactory : IAIFactory
    {
        private readonly IPrefabFactory _prefabFactory;
        private readonly IDeferrer _deferrer;

        public AIFactory(IPrefabFactory prefabFactory, IDeferrer deferrer)
        {
            _prefabFactory = prefabFactory;
            _deferrer = deferrer;
        }

        public void CreateAI(ICruiserController aiCruiser, IList<IPrefabKey> buildOrder)
        {
			ITaskList tasks = new TaskList();
            ITaskFactory taskFactory = new TaskFactory(_prefabFactory, aiCruiser, _deferrer);
			new BasicTaskProducer(tasks, aiCruiser, _prefabFactory, taskFactory, buildOrder);

            // FELIX
			//new ReplaceDestroyedBuildingsTaskProducer(tasks, _aiCruiser, prefabFactory, taskFactory, )

			new TaskConsumer(tasks);
		}
    }
}