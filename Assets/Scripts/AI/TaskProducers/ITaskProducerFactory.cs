using System.Collections.Generic;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.AI.TaskProducers
{
    public interface ITaskProducerFactory
    {
        void CreateBasicTaskProducer(ITaskList tasks, IList<IPrefabKey> buildOrder);
		void CreateReplaceDestroyedBuildingsTaskProducer(ITaskList tasks);
        void CreateAntiAirTaskProducer(ITaskList tasks, IList<IPrefabKey> antiAirBuildOrder);
        void CreateAntiNavalTaskProducer(ITaskList tasks, IList<IPrefabKey> antiNavalBuildOrder);
        void CreateAntiRocketLauncherTaskProducer(ITaskList tasks, IList<IPrefabKey> antiRocketLauncherBuildOrder);
	}
}
