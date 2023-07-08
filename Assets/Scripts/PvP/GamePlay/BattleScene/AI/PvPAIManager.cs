using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.BuildOrders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Drones.BuildingMonitors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.FactoryManagers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.TaskProducers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.TaskProducers.SlotNumber;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Tasks;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.ThreatMonitors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static.Strategies.Helper;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI.Tasks;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static.Strategies.Helper;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Threading;
using BattleCruisers.AI;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI
{
    public class PvPAIManager : IPvPAIManager
    {
        private readonly IPvPPrefabFactory _prefabFactory;
        private readonly IDataProvider _dataProvider;
        private readonly IPvPDeferrer _deferrer;
        private readonly IPvPSlotNumCalculatorFactory _slotNumCalculatorFactory;
        private readonly IPvPThreatMonitorFactory _threatMonitorFactory;
        private readonly IPvPFactoryManagerFactory _factoryManagerFactory;
        private readonly IPvPBuildOrderFactory _buildOrderFactory;
        private readonly IPvPFactoryMonitorFactory _factoryMonitorFactory;

        public PvPAIManager(
            IPvPPrefabFactory prefabFactory,
            IDataProvider dataProvider,
            IPvPDeferrer deferrer,
            IPvPCruiserController playerCruiser,
            IPvPStrategyFactory strategyFactory)
        {
            PvPHelper.AssertIsNotNull(prefabFactory, dataProvider, deferrer, playerCruiser, strategyFactory);

            _prefabFactory = prefabFactory;
            _dataProvider = dataProvider;
            _deferrer = deferrer;

            _slotNumCalculatorFactory = new PvPSlotNumCalculatorFactory();
            _threatMonitorFactory = new PvPThreatMonitorFactory(playerCruiser, PvPTimeBC.Instance, deferrer);
            _factoryManagerFactory = new PvPFactoryManagerFactory(_dataProvider.GameModel, _prefabFactory, _threatMonitorFactory);

            IPvPSlotAssigner slotAssigner = new PvPSlotAssigner();
            _buildOrderFactory = new PvPBuildOrderFactory(slotAssigner, _dataProvider.StaticData, _dataProvider.GameModel, strategyFactory);

            _factoryMonitorFactory = new PvPFactoryMonitorFactory(PvPRandomGenerator.Instance);
        }

        public IPvPArtificialIntelligence CreateAI(IPvPLevelInfo levelInfo, Difficulty difficulty)
        {
            // Manage AI unit factories (needs to be before the AI strategy is created,
            // otherwise miss started construction event for first building :) )
            _factoryManagerFactory.CreateNavalFactoryManager(levelInfo.AICruiser);
            _factoryManagerFactory.CreateAirfactoryManager(levelInfo.AICruiser);

            IPvPTaskFactory taskFactory = new PvPTaskFactory(_prefabFactory, levelInfo.AICruiser, _deferrer);
            IPvPTaskProducerFactory taskProducerFactory
                = new PvPTaskProducerFactory(
                    levelInfo.AICruiser,
                    _prefabFactory,
                    taskFactory,
                    _slotNumCalculatorFactory,
                    _dataProvider.StaticData,
                    _threatMonitorFactory);
            IPvPAIFactory aiFactory = new PvPAIFactory(taskProducerFactory, _buildOrderFactory, _factoryMonitorFactory);

            if (IsAdaptiveAI(difficulty))
            {
                return aiFactory.CreateAdaptiveAI(levelInfo);
            }
            else
            {
                return aiFactory.CreateBasicAI(levelInfo);
            }
        }

        private bool IsAdaptiveAI(Difficulty difficulty)
        {
            return
                difficulty == Difficulty.Hard
                || difficulty == Difficulty.Harder
                || difficulty == Difficulty.Normal;//add this condition 
        }
    }
}
