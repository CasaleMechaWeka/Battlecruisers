using System.Collections.Generic;
using BattleCruisers.AI.Tasks;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Fetchers;

namespace BattleCruisers.AI.TaskProducers
{
    public class TaskProducerFactory : ITaskProducerFactory
	{
		public void CreateReplaceDestroyedBuildingsTaskProducer(ITaskList tasks, ICruiserController cruiser,
            IPrefabFactory prefabFactory, ITaskFactory taskFactory, IList<IPrefabKey> unlockedBuildingKeys)
        {
            new ReplaceDestroyedBuildingsTaskProducer(tasks, cruiser, prefabFactory, taskFactory, unlockedBuildingKeys);
        }
	}
}