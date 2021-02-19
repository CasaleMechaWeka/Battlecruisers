using BattleCruisers.AI.BuildOrders;
using BattleCruisers.AI.Drones.BuildingMonitors;
using BattleCruisers.AI.FactoryManagers;
using BattleCruisers.AI.TaskProducers;
using BattleCruisers.AI.TaskProducers.SlotNumber;
using BattleCruisers.AI.Tasks;
using BattleCruisers.AI.ThreatMonitors;
using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static.Strategies;
using BattleCruisers.Data.Static.Strategies.Helper;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.AI
{
    public class AIManager : IAIManager
    {
        private readonly IPrefabFactory _prefabFactory;
        private readonly IDataProvider _dataProvider;
        private readonly IDeferrer _deferrer;
        private readonly ISlotNumCalculatorFactory _slotNumCalculatorFactory;
		private readonly IThreatMonitorFactory _threatMonitorFactory;
		private readonly IFactoryManagerFactory _factoryManagerFactory;
        private readonly IBuildOrderFactory _buildOrderFactory;
        private readonly IFactoryMonitorFactory _factoryMonitorFactory;

        public AIManager(
            IPrefabFactory prefabFactory, 
            IDataProvider dataProvider, 
            IDeferrer deferrer, 
            ICruiserController playerCruiser, 
            IStrategyFactory strategyFactory)
        {
            Helper.AssertIsNotNull(prefabFactory, dataProvider, deferrer, playerCruiser, strategyFactory);

            _prefabFactory = prefabFactory;
            _dataProvider = dataProvider;
            _deferrer = deferrer;

            _slotNumCalculatorFactory = new SlotNumCalculatorFactory();
            _threatMonitorFactory = new ThreatMonitorFactory(playerCruiser, TimeBC.Instance, deferrer);
            _factoryManagerFactory = new FactoryManagerFactory(_dataProvider.GameModel, _prefabFactory, _threatMonitorFactory);

            ISlotAssigner slotAssigner = new SlotAssigner();
            _buildOrderFactory = new BuildOrderFactory(slotAssigner, _dataProvider.StaticData, _dataProvider.GameModel, strategyFactory);

            _factoryMonitorFactory = new FactoryMonitorFactory(RandomGenerator.Instance);
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
                    _dataProvider.StaticData,
                    _threatMonitorFactory);
            IAIFactory aiFactory = new AIFactory(taskProducerFactory, _buildOrderFactory, _factoryMonitorFactory);

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
            return difficulty != Difficulty.Easy;
        }
    }
}
