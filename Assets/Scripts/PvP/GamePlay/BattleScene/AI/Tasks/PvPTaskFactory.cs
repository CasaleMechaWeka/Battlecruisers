using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Tasks
{
    public class PvPTaskFactory : IPvPTaskFactory
    {
        private readonly IPvPPrefabFactory _prefabFactory;
        private readonly IPvPCruiserController _cruiser;
        private readonly IPvPDeferrer _deferrer;

        // For cheating :)
        public static PvPDelayProvider delayProvider;

        public const float DEFAULT_DELAY_IN_S = 1.5f;
        public const float MIN_DELAY_IN_S = 0.1f;

        public PvPTaskFactory(IPvPPrefabFactory prefabFactory, IPvPCruiserController cruiser, IPvPDeferrer deferrer)
        {
            PvPHelper.AssertIsNotNull(prefabFactory, cruiser, deferrer);

            _prefabFactory = prefabFactory;
            _cruiser = cruiser;
            _deferrer = deferrer;

            delayProvider = new PvPDelayProvider(DEFAULT_DELAY_IN_S);
        }

        public IPvPPrioritisedTask CreateConstructBuildingTask(PvPTaskPriority priority, IPvPPrefabKey buildingKey)
        {
            IPvPTask constructBuildingTask = new PvPConstructBuildingTask(buildingKey, _prefabFactory, _cruiser);
            return CreatePrioritisedTask(constructBuildingTask, priority);
        }

        private IPvPPrioritisedTask CreatePrioritisedTask(IPvPTask task, PvPTaskPriority priority)
        {
            IPvPPrioritisedTask prioritisedTask = new PvPPrioritisedTask(priority, task);
            return new PvPDeferredPrioritisedTask(prioritisedTask, _deferrer, delayProvider);
        }
    }
}
