using System.Collections.Generic;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.AI.TaskProducers
{
    public interface ITaskProducerFactory
    {
        void CreateReplaceDestroyedBuildingsTaskProducer(ITaskList tasks, IList<IPrefabKey> unlockedBuildingKeys);
        void CreateAntiAirTaskProducer(ITaskList tasks);
	}
}