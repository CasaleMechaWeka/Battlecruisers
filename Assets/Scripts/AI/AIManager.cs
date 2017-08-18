using BattleCruisers.AI.FactoryManagers;
using BattleCruisers.AI.Providers;
using BattleCruisers.AI.Providers.BuildingKey;
using BattleCruisers.AI.TaskProducers;
using BattleCruisers.AI.TaskProducers.SlotNumber;
using BattleCruisers.AI.Tasks;
using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Data.Settings;
using BattleCruisers.Fetchers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.AI
{
    public class AIManager : IAIManager
    {
        private readonly IPrefabFactory _prefabFactory;
        private readonly IDeferrer _deferrer;
        private readonly IDataProvider _dataProvider;
        private readonly ISlotNumCalculatorFactory _slotNumCalculatorFactory;
		private readonly IFactoryManagerFactory _factoryManagerFactory;
        private readonly IBuildOrderProvider _buildOrderProvider;

        public AIManager(IPrefabFactory prefabFactory, IDeferrer deferrer, IDataProvider dataProvider)
        {
            Helper.AssertIsNotNull(prefabFactory, deferrer, dataProvider);

            _prefabFactory = prefabFactory;
            _deferrer = deferrer;
            _dataProvider = dataProvider;
			
            _slotNumCalculatorFactory = new SlotNumCalculatorFactory();
            _factoryManagerFactory = new FactoryManagerFactory(_dataProvider.StaticData, _prefabFactory);

            IBuildingKeyProviderFactory buildingKeyProviderFactory = new BuildingKeyProviderFactory(_dataProvider.StaticData);
            IOffensiveBuildOrderProvider offensiveBuildOrderProvider = new OffensiveBuildOrderProvider();
            IAntiUnitBuildOrderProvider antiAirBuildOrderProvider = new AntiAirBuildOrderProvivder(_dataProvider.StaticData);
            IAntiUnitBuildOrderProvider antiNavalBuildOrderProvider = new AntiNavalBuildOrderProvivder(_dataProvider.StaticData);
            _buildOrderProvider = new BuildOrderProvider(buildingKeyProviderFactory, offensiveBuildOrderProvider, antiAirBuildOrderProvider, antiNavalBuildOrderProvider, _dataProvider.StaticData);
        }

        public void CreateAI(ILevel currentLevel, ICruiserController playerCruiser, ICruiserController aiCruiser)
        {
            // Manage AI unit factories (needs to be before the AI strategy is created,
            // otherwise miss started construction event for first building :) )
            _factoryManagerFactory.CreateNavalFactoryManager(currentLevel.Num, aiCruiser);
            // FELIX  Create air factory manager :)
			
            ITaskFactory taskFactory = new TaskFactory(_prefabFactory, aiCruiser, _deferrer);
            ITaskProducerFactory taskProducerFactory = new TaskProducerFactory(
                aiCruiser, playerCruiser, _prefabFactory, taskFactory, _slotNumCalculatorFactory, _dataProvider.StaticData);
            IAIFactory aiFactory = new AIFactory(taskProducerFactory, _buildOrderProvider);

            switch (_dataProvider.SettingsManager.AIDifficulty)
            {
                case Difficulty.Normal:
                    aiFactory.CreateBasicAI(currentLevel, aiCruiser.SlotWrapper);
                    break;
                case Difficulty.Hard:
                    aiFactory.CreateAdaptiveAI(currentLevel, aiCruiser.SlotWrapper);
                    break;
            }
        }
    }
}
