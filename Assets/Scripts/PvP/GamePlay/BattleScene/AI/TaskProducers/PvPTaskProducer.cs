using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Tasks;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.TaskProducers
{
    public abstract class PvPTaskProducer : IPvPTaskProducer
    {
        protected readonly IPvPTaskList _tasks;
        protected readonly IPvPCruiserController _cruiser;
        protected readonly IPvPTaskFactory _taskFactory;
        protected readonly IPvPPrefabFactory _prefabFactory;

        public PvPTaskProducer(
            IPvPTaskList tasks,
            IPvPCruiserController cruiser,
            IPvPTaskFactory taskFactory,
            IPvPPrefabFactory prefabFactory)
        {
            PvPHelper.AssertIsNotNull(tasks, cruiser, taskFactory, prefabFactory);

            _tasks = tasks;
            _cruiser = cruiser;
            _taskFactory = taskFactory;
            _prefabFactory = prefabFactory;
        }

        public virtual void DisposeManagedState()
        {
        }
    }
}
