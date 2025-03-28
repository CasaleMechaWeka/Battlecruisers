using BattleCruisers.AI.BuildOrders;
using BattleCruisers.AI.FactoryManagers;
using BattleCruisers.AI.TaskProducers;
using BattleCruisers.AI.Tasks;
using BattleCruisers.AI.ThreatMonitors;
using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static.Strategies.Helper;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.AI
{
    public class AIManager : IAIManager
    {
        private readonly IDeferrer _deferrer;
        private readonly IThreatMonitorFactory _threatMonitorFactory;
        private readonly IFactoryManagerFactory _factoryManagerFactory;
        private readonly IBuildOrderFactory _buildOrderFactory;

        public AIManager(
            IDeferrer deferrer,
            ICruiserController playerCruiser,
            IStrategyFactory strategyFactory)
        {
            Helper.AssertIsNotNull(deferrer, playerCruiser, strategyFactory);

            _deferrer = deferrer;

            _threatMonitorFactory = new ThreatMonitorFactory(playerCruiser, TimeBC.Instance, deferrer);
            _factoryManagerFactory = new FactoryManagerFactory(DataProvider.GameModel, _threatMonitorFactory);

            SlotAssigner slotAssigner = new SlotAssigner();
            _buildOrderFactory = new BuildOrderFactory(slotAssigner, DataProvider.GameModel, strategyFactory);
        }

        public IArtificialIntelligence CreateAI(LevelInfo levelInfo, Difficulty difficulty)
        {
            // Manage AI unit factories (needs to be before the AI strategy is created,
            // otherwise miss started construction event for first building :) )
            _factoryManagerFactory.CreateNavalFactoryManager(levelInfo.AICruiser);
            _factoryManagerFactory.CreateAirfactoryManager(levelInfo.AICruiser);

            ITaskFactory taskFactory = new TaskFactory(levelInfo.AICruiser, _deferrer);
            ITaskProducerFactory taskProducerFactory
                = new TaskProducerFactory(
                    levelInfo.AICruiser,
                    taskFactory,
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
