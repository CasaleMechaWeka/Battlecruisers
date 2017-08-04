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
        private readonly IBuildOrderProvider _buildOrderProvider;

        public AIManager(IPrefabFactory prefabFactory, IDeferrer deferrer, IDataProvider dataProvider)
        {
            Helper.AssertIsNotNull(prefabFactory, deferrer, dataProvider);

            _prefabFactory = prefabFactory;
            _deferrer = deferrer;
            _dataProvider = dataProvider;
			
            _slotNumCalculatorFactory = new SlotNumCalculatorFactory();
			_buildOrderProvider = new BuildOrderProvider();
        }

        public void CreateAI(ILevel currentLevel, ICruiserController playerCruiser, ICruiserController aiCruiser)
        {
            ITaskFactory taskFactory = new TaskFactory(_prefabFactory, aiCruiser, _deferrer);
            ITaskProducerFactory taskProducerFactory = new TaskProducerFactory(
                aiCruiser, playerCruiser, _prefabFactory, taskFactory, _slotNumCalculatorFactory, _dataProvider.StaticData);
			IAIFactory aiFactory = new AIFactory(taskProducerFactory, _buildOrderProvider);

			switch (_dataProvider.SettingsManager.AIDifficulty)
			{
				case Difficulty.Normal:
					aiFactory.CreateBasicAI(currentLevel);
					break;
				case Difficulty.Hard:
					aiFactory.CreateAdaptiveAI(currentLevel);
					break;
			}
        }
    }
}