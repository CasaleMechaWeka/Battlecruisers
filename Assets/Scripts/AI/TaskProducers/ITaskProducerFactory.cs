using System.Collections.Generic;
using BattleCruisers.AI.Tasks;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.PrefabKeys;

namespace BattleCruisers.AI.TaskProducers
{
    public interface ITaskProducerFactory
    {
        void CreateReplaceDestroyedBuildingsTaskProducer(ITaskList tasks, ICruiserController cruiser, ITaskFactory taskFactory, IDictionary<IBuilding, IPrefabKey> buildingToKey);
    }
}