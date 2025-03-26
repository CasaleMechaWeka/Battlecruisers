using BattleCruisers.AI.BuildOrders;
using BattleCruisers.AI.FactoryManagers;
using BattleCruisers.AI.TaskProducers;
using BattleCruisers.AI.TaskProducers.SlotNumber;
using BattleCruisers.AI.Tasks;
using BattleCruisers.AI.ThreatMonitors;
using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static.Strategies.Helper;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.AI
{
    public class AIManager : IAIManager
    {
        private readonly PrefabFactory _prefabFactory;
        private readonly IDeferrer _deferrer;
        private readonly ISlotNumCalculatorFactory _slotNumCalculatorFactory;
        private readonly IThreatMonitorFactory _threatMonitorFactory;
        private readonly IFactoryManagerFactory _factoryManagerFactory;
        private readonly IBuildOrderFactory _buildOrderFactory;

        public AIManager(
            PrefabFactory prefabFactory,
            IDeferrer deferrer,
            ICruiserController playerCruiser,
            IStrategyFactory strategyFactory)
        {
            Helper.AssertIsNotNull(prefabFactory, deferrer, playerCruiser, strategyFactory);

            _prefabFactory = prefabFactory;
            _deferrer = deferrer;

            _slotNumCalculatorFactory = new SlotNumCalculatorFactory();
            _threatMonitorFactory = new ThreatMonitorFactory(playerCruiser, TimeBC.Instance, deferrer);
            _factoryManagerFactory = new FactoryManagerFactory(DataProvider.GameModel, _prefabFactory, _threatMonitorFactory);

            ISlotAssigner slotAssigner = new SlotAssigner();
            _buildOrderFactory = new BuildOrderFactory(slotAssigner, DataProvider.GameModel, strategyFactory);
        }

        public IArtificialIntelligence CreateAI(ILevelInfo levelInfo, Difficulty difficulty)
        {
            // Manage AI unit factories (needs to be before the AI strategy is created,
            // otherwise miss started construction event for first building :) )
            _factoryManagerFactory.CreateNavalFactoryManager(levelInfo.AICruiser);
            _factoryManagerFactory.CreateAirfactoryManager(levelInfo.AICruiser);

            ITaskFactory taskFactory = new TaskFactory(_prefabFactory, levelInfo.AICruiser, _deferrer);
            ITaskProducerFactory taskProducerFactory
                = new TaskProducerFactory(
                    levelInfo.AICruiser,
                    _prefabFactory,
                    taskFactory,
                    _slotNumCalculatorFactory,
                    _threatMonitorFactory);
            IAIFactory aiFactory = new AIFactory(taskProducerFactory, _buildOrderFactory);

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
