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
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Threading;
using System;

namespace BattleCruisers.AI
{
    public class AIManager : IAIManager
    {
        private readonly IPrefabFactory _prefabFactory;
        private readonly IDataProvider _dataProvider;
        private readonly IVariableDelayDeferrer _variableDelayDeferrer;
        private readonly ISlotNumCalculatorFactory _slotNumCalculatorFactory;
		private readonly IThreatMonitorFactory _threatMonitorFactory;
		private readonly IFactoryManagerFactory _factoryManagerFactory;
        private readonly IBuildOrderFactory _buildOrderFactory;
        private readonly IFactoryMonitorFactory _factoryMonitorFactory;

        public AIManager(IPrefabFactory prefabFactory, IDataProvider dataProvider, IVariableDelayDeferrer variableDelayDeferrer, ICruiserController playerCruiser)
        {
            Helper.AssertIsNotNull(prefabFactory, dataProvider, variableDelayDeferrer, playerCruiser);

            _prefabFactory = prefabFactory;
            _dataProvider = dataProvider;
            _variableDelayDeferrer = variableDelayDeferrer;

            _slotNumCalculatorFactory = new SlotNumCalculatorFactory();
			_threatMonitorFactory = new ThreatMonitorFactory(playerCruiser);
            _factoryManagerFactory = new FactoryManagerFactory(_dataProvider.StaticData, _prefabFactory, _threatMonitorFactory);

            ISlotAssigner slotAssigner = new SlotAssigner();
            _buildOrderFactory = new BuildOrderFactory(slotAssigner, _dataProvider.StaticData);

            _factoryMonitorFactory = new FactoryMonitorFactory(new RandomGenerator());
        }

        public IArtificialIntelligence CreateAI(ILevelInfo levelInfo)
        {
            // Manage AI unit factories (needs to be before the AI strategy is created,
            // otherwise miss started construction event for first building :) )
            _factoryManagerFactory.CreateNavalFactoryManager(levelInfo);
            _factoryManagerFactory.CreateAirfactoryManager(levelInfo);

            ITaskFactory taskFactory = new TaskFactory(_prefabFactory, levelInfo.AICruiser, _variableDelayDeferrer);
            ITaskProducerFactory taskProducerFactory 
                = new TaskProducerFactory(
                    levelInfo.AICruiser, 
                    levelInfo.PlayerCruiser, 
                    _prefabFactory, 
                    taskFactory, 
                    _slotNumCalculatorFactory, 
                    _dataProvider.StaticData,
                    _threatMonitorFactory);
            IAIFactory aiFactory = new AIFactory(taskProducerFactory, _buildOrderFactory, _factoryMonitorFactory);

            switch (_dataProvider.SettingsManager.AIDifficulty)
            {
                case Difficulty.Easy:
                case Difficulty.Normal:
                case Difficulty.Hard:
                    return aiFactory.CreateBasicAI(levelInfo);

                case Difficulty.Harder:
                case Difficulty.Insane:
                    return aiFactory.CreateAdaptiveAI(levelInfo);

                default:
                    throw new ArgumentException("Unkonwn difficulty: " + _dataProvider.SettingsManager.AIDifficulty);
            }
        }
    }
}
