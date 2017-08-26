using BattleCruisers.AI.FactoryManagers;
using BattleCruisers.AI.BuildOrders;
using BattleCruisers.AI.TaskProducers;
using BattleCruisers.AI.TaskProducers.SlotNumber;
using BattleCruisers.AI.Tasks;
using BattleCruisers.Data;
using BattleCruisers.Data.Settings;
using BattleCruisers.Fetchers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;
using BattleCruisers.AI.Drones;

namespace BattleCruisers.AI
{
    public class AIManager : IAIManager
    {
        private readonly IPrefabFactory _prefabFactory;
        private readonly IDeferrer _deferrer;
        private readonly IDataProvider _dataProvider;
        private readonly ISlotNumCalculatorFactory _slotNumCalculatorFactory;
		private readonly IFactoryManagerFactory _factoryManagerFactory;
        private readonly IBuildOrderFactory _buildOrderFactory;

        public AIManager(IPrefabFactory prefabFactory, IDeferrer deferrer, IDataProvider dataProvider)
        {
            Helper.AssertIsNotNull(prefabFactory, deferrer, dataProvider);

            _prefabFactory = prefabFactory;
            _deferrer = deferrer;
            _dataProvider = dataProvider;
			
            _slotNumCalculatorFactory = new SlotNumCalculatorFactory();
            _factoryManagerFactory = new FactoryManagerFactory(_dataProvider.StaticData, _prefabFactory);

            ISlotAssigner slotAssigner = new SlotAssigner();
            _buildOrderFactory = new BuildOrderFactory(slotAssigner, _dataProvider.StaticData);
        }

        public void CreateAI(ILevelInfo levelInfo)
        {
            // Manage AI unit factories (needs to be before the AI strategy is created,
            // otherwise miss started construction event for first building :) )
            _factoryManagerFactory.CreateNavalFactoryManager(levelInfo.LevelNum, levelInfo.AICruiser);
            // FELIX  Create air factory manager :)

            new DroneConsumerFocusManager(levelInfo.AICruiser);

            ITaskFactory taskFactory = new TaskFactory(_prefabFactory, levelInfo.AICruiser, _deferrer);
            ITaskProducerFactory taskProducerFactory 
                = new TaskProducerFactory(
                    levelInfo.AICruiser, 
                    levelInfo.PlayerCruiser, 
                    _prefabFactory, 
                    taskFactory, 
                    _slotNumCalculatorFactory, 
                    _dataProvider.StaticData);
            IAIFactory aiFactory = new AIFactory(taskProducerFactory, _buildOrderFactory);

            switch (_dataProvider.SettingsManager.AIDifficulty)
            {
                case Difficulty.Normal:
                    aiFactory.CreateBasicAI(levelInfo);
                    break;
                case Difficulty.Hard:
                    aiFactory.CreateAdaptiveAI(levelInfo);
                    break;
            }
        }
    }
}
