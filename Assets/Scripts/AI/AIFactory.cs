using System.Collections.Generic;
using BattleCruisers.AI.TaskProducers;
using BattleCruisers.AI.Tasks;
using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Fetchers;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.AI
{
    public class AIFactory : IAIFactory
    {
        private readonly IPrefabFactory _prefabFactory;
        private readonly IDeferrer _deferrer;
        private readonly IStaticData _staticData;

        public AIFactory(IPrefabFactory prefabFactory, IDeferrer deferrer, IStaticData staticData)
        {
            _prefabFactory = prefabFactory;
            _deferrer = deferrer;
            _staticData = staticData;
        }

        public void CreateAI(ICruiserController aiCruiser, IList<IPrefabKey> buildOrder)
        {
			ITaskList tasks = new TaskList();
            ITaskFactory taskFactory = new TaskFactory(_prefabFactory, aiCruiser, _deferrer);

            new BasicTaskProducer(tasks, aiCruiser, _prefabFactory, taskFactory, buildOrder);
            new ReplaceDestroyedBuildingsTaskProducer(tasks, aiCruiser, _prefabFactory, taskFactory, _staticData.BuildingKeys); 
			
            new TaskConsumer(tasks);
		}
    }
}