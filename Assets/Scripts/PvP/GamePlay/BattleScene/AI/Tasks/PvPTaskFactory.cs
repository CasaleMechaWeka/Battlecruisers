using BattleCruisers.AI.Tasks;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Tasks
{
    public class PvPTaskFactory : ITaskFactory
    {
        private readonly IPvPCruiserController _cruiser;
        private readonly IDeferrer _deferrer;

        // For cheating :)
        public static float delayInS;

        public const float DEFAULT_DELAY_IN_S = 1.5f;
        public const float MIN_DELAY_IN_S = 0.1f;

        public PvPTaskFactory(IPvPCruiserController cruiser, IDeferrer deferrer)
        {
            PvPHelper.AssertIsNotNull(cruiser, deferrer);

            _cruiser = cruiser;
            _deferrer = deferrer;

            delayInS = DEFAULT_DELAY_IN_S;
        }

        public IPrioritisedTask CreateConstructBuildingTask(TaskPriority priority, IPrefabKey buildingKey)
        {
            ITask constructBuildingTask = new PvPConstructBuildingTask(buildingKey, _cruiser);
            return CreatePrioritisedTask(constructBuildingTask, priority);
        }

        private IPrioritisedTask CreatePrioritisedTask(ITask task, TaskPriority priority)
        {
            IPrioritisedTask prioritisedTask = new PrioritisedTask(priority, task);
            return new DeferredPrioritisedTask(prioritisedTask, _deferrer, delayInS);
        }
    }
}
