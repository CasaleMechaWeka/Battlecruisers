using System.Collections.Generic;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.AI.TaskProducers
{
    public interface ITaskProducerFactory
    {
        void CreateBasicTaskProducer(ITaskList tasks, IList<IPrefabKey> buildOrder);
		void CreateReplaceDestroyedBuildingsTaskProducer(ITaskList tasks, IList<IPrefabKey> unlockedBuildingKeys);
		void CreateAntiAirTaskProducer(ITaskList tasks);
        void CreateAntiNavalTaskProducer(ITaskList tasks);
        void CreateAntiRocketLauncherTaskProducer(ITaskList tasks);
	}
}