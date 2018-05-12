using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Tutorial;
using BattleCruisers.Tutorial.Steps.Providers;
using BattleCruisers.UI;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Scenes
{
    public class TutorialHelper : IBattleSceneHelper, ITutorialProvider
    {
        private readonly IDataProvider _dataProvider;
        private readonly SpecificSlotsFilter _slotFilter;
        private readonly BuildingNameFilter _buildingNameFilter;
        private readonly BuildingCategoryFilter _buildingCategoryFilter;

        public ISlotPermitter SlotPermitter { get { return _slotFilter; } }
        public IBuildingCategoryPermitter BuildingCategoryPermitter { get { return _buildingCategoryFilter; } }
        public IFilter<IBuildable> ShouldBuildingBeEnabledFilter { get { return _buildingNameFilter; } }
        public IBuildingPermitter BuildingPermitter { get { return _buildingNameFilter; } }
        public BasicFilter NavigationPermitter { get; private set; }
        public BasicFilter BackButtonPermitter { get; private set; }
        public IUIManagerSettablePermissions UIManagerPermissions { get; private set; }

        public ISingleBuildableProvider LastBuildingStartedProvider { get; private set; }
        public ISingleBuildableProvider SingleAircraftProvider { get; private set; }
        public ISingleBuildableProvider SingleShipProvider { get; private set; }

        public IBuildProgressCalculator PlayerCruiserBuildProgressCalculator { get; private set; }
		public IBuildSpeedController PlayerCruiserBuildSpeedController { get; private set; }
        public IBuildProgressCalculator AICruiserBuildProgressCalculator { get; private set; }
        public IBuildSpeedController AICruiserBuildSpeedController { get; private set; }

        public TutorialHelper(IDataProvider dataProvider, IPrefabFactory prefabFactory)
        {
            Helper.AssertIsNotNull(dataProvider, prefabFactory);

            _dataProvider = dataProvider;

            _slotFilter = new SpecificSlotsFilter();
            _buildingNameFilter = new BuildingNameFilter(prefabFactory);
            _buildingCategoryFilter = new BuildingCategoryFilter();
            NavigationPermitter = new BasicFilter(isMatch: false);
            BackButtonPermitter = new BasicFilter(isMatch: false);
            SingleAircraftProvider = new SingleBuildableProvider(GameObjectTags.AIRCRAFT);
            SingleShipProvider = new SingleBuildableProvider(GameObjectTags.SHIP);

			IBuildProgressCalculator slowCalculator = new AsymptoticCalculator();
            IBuildProgressCalculator normalCalculator = new LinearCalculator(BuildSpeedMultipliers.DEFAULT_TUTORIAL_BUILD_SPEED_MULTIPLIER);
            IBuildProgressCalculator fastCalculator = new LinearCalculator(BuildSpeedMultipliers.FAST_BUILD_SPEED_MULTIPLIER);

            CompositeCalculator playerCruiserBuildSpeedCalculator = new CompositeCalculator(slowCalculator, normalCalculator, fastCalculator);
            PlayerCruiserBuildProgressCalculator = playerCruiserBuildSpeedCalculator;
            PlayerCruiserBuildSpeedController = playerCruiserBuildSpeedCalculator;

            CompositeCalculator aiCruiserBuildSpeedCalculator = new CompositeCalculator(slowCalculator, normalCalculator, fastCalculator);
            AICruiserBuildProgressCalculator = aiCruiserBuildSpeedCalculator;
            AICruiserBuildSpeedController = aiCruiserBuildSpeedCalculator;
        }

        public IUIManager CreateUIManager(IManagerArgs args)
        {
            UIManagerPermissions uiManagerPermissions = new UIManagerPermissions()
            {
                CanDismissItemDetails = false,
                CanShowItemDetails = false
            };
            UIManagerPermissions = uiManagerPermissions;
            return new LimitableUIManager(args, uiManagerPermissions);
        }

        public ILoadout GetPlayerLoadout()
        {
            return _dataProvider.StaticData.InitialGameModel.PlayerLoadout;
        }
		
        public void CreateAI(ICruiserController aiCruiser, ICruiserController playerCruiser, int currentLevelNum)
		{
            // The tutorial has no AI :)
		}
		
		public ISlotFilter CreateHighlightableSlotFilter()
		{
            return _slotFilter;
		}

        public IFilter<IBuildable> CreateBuildableButtonFilter(IDroneManager droneManager)
        {
            return _buildingNameFilter;
        }

        public IFilter<BuildingCategory> CreateCategoryButtonFilter()
        {
            return _buildingCategoryFilter;
        }

        public IFilter<IBuilding> CreateBuildingDeleteButtonFilter(ICruiser playerCruiser)
        {
            return new StaticFilter<IBuilding>(isMatch: false);
        }

        public BasicFilter CreateNavigationFilter()
        {
            return NavigationPermitter;
        }

        public BasicFilter CreateBackButtonFilter()
        {
            return BackButtonPermitter;
        }

        public ISingleBuildableProvider CreateLastIncompleteBuildingStartedProvider(ICruiserController cruiser)
        {
            return new LastIncompleteBuildingStartedProvider(cruiser);
        }
    }
}
