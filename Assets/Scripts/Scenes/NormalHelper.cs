using BattleCruisers.AI;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.UI;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Common.BuildableDetails.Buttons;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Scenes
{
    public class NormalHelper : IBattleSceneHelper
    {
        private readonly IDataProvider _dataProvider;
        private readonly IPrefabFactory _prefabFactory;
        private readonly IDeferrer _deferrer;

        public IBuildProgressCalculator PlayerCruiserBuildProgressCalculator { get; private set; }
        public IBuildProgressCalculator AICruiserBuildProgressCalculator { get; private set; }

        public NormalHelper(IDataProvider dataProvider, IPrefabFactory prefabFactory, IDeferrer deferrer)
        {
            Helper.AssertIsNotNull(dataProvider, prefabFactory, deferrer);

            _dataProvider = dataProvider;
            _prefabFactory = prefabFactory;
            _deferrer = deferrer;

            IBuildProgressCalculator normalCalculator = new LinearCalculator(BuildSpeedMultipliers.NORMAL_BUILD_SPEED_MULTIPLIER);
            PlayerCruiserBuildProgressCalculator = normalCalculator;
            AICruiserBuildProgressCalculator = normalCalculator;
        }

        public IUIManager CreateUIManager(IManagerArgs args)
        {
            return new UIManager(args);
        }

        public ILoadout GetPlayerLoadout()
        {
            return _dataProvider.GameModel.PlayerLoadout;
        }
		
        public void CreateAI(ICruiserController aiCruiser, ICruiserController playerCruiser, int currentLevelNum)
		{
            ILevelInfo levelInfo = new LevelInfo(aiCruiser, playerCruiser, _dataProvider.StaticData, _prefabFactory, currentLevelNum);
            IAIManager aiManager = new AIManager(_prefabFactory, _deferrer, _dataProvider);
            aiManager.CreateAI(levelInfo);
		}
		
		public ISlotFilter CreateHighlightableSlotFilter()
		{
            return new FreeSlotFilter();
		}

        public IFilter<IBuildable> CreateBuildableButtonFilter(IDroneManager droneManager)
        {
            return new AffordableBuildableFilter(droneManager);
        }

        public IFilter<BuildingCategory> CreateCategoryButtonFilter()
        {
            // For the real game want to enable all building categories :)
            return new StaticFilter<BuildingCategory>(isMatch: true);
        }

        public IFilter<IBuilding> CreateBuildingDeleteButtonFilter(ICruiser playerCruiser)
        {
            return new PlayerCruiserBuildingFilter(playerCruiser);
        }

        public BasicFilter CreateNavigationFilter()
        {
            return new BasicFilter(isMatch: true);
        }

        public BasicFilter CreateBackButtonFilter()
        {
            return new BasicFilter(isMatch: true);
        }
    }
}
