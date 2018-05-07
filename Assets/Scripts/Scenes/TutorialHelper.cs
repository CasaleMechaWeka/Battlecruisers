using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Tutorial;
using BattleCruisers.Tutorial.Steps.Providers;
using BattleCruisers.UI;
using BattleCruisers.UI.BattleScene.Buttons.ActivenessDeciders;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Scenes
{
    public class TutorialHelper : IBattleSceneHelper, IPermitterProvider
    {
        private readonly IDataProvider _dataProvider;
        private readonly SpecificSlotsFilter _slotFilter;
        private readonly BuildableTutorialDecider _buildableDecider;
        private readonly BuildingCategoryTutorialDecider _buildingCategoryDecider;

        public ISlotPermitter SlotPermitter { get { return _slotFilter; } }
		public IBuildingCategoryPermitter BuildingCategoryPermitter { get { return _buildingCategoryDecider; } }
        public IActivenessDecider<IBuildable> BuildingActivenessDecider { get { return _buildableDecider; } }
        public IBuildingPermitter BuildingPermitter { get { return _buildableDecider; } }
        public BasicDecider NavigationPermitter { get; private set; }
        public BasicDecider BackButtonPermitter { get; private set; }
        public ILastBuildingStartedProvider LastBuildingStartedProvider { get; private set; }

        public TutorialHelper(IDataProvider dataProvider, IPrefabFactory prefabFactory)
        {
            Helper.AssertIsNotNull(dataProvider, prefabFactory);

            _dataProvider = dataProvider;

            _slotFilter = new SpecificSlotsFilter();
            _buildableDecider = new BuildableTutorialDecider(prefabFactory);
            _buildingCategoryDecider = new BuildingCategoryTutorialDecider();
            NavigationPermitter = new BasicDecider(shouldBeEnabled: false);
            BackButtonPermitter = new BasicDecider(shouldBeEnabled: false);
        }

        public IUIManager CreateUIManager(IManagerArgs args)
        {
            IUIManagerPermissions permissions = new UIManagerPermissions()
            {
                CanDismissItemDetails = false,
                CanShowItemDetails = false
            };
            return new LimitableUIManager(args, permissions);
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

        public IActivenessDecider<IBuildable> CreateBuildableButtonActivenessDecider(IDroneManager droneManager)
        {
            return _buildableDecider;
        }

        public IActivenessDecider<BuildingCategory> CreateCategoryButtonActivenessDecider()
        {
            return _buildingCategoryDecider;
        }

        public IActivenessDecider<IBuilding> CreateBuildingDeleteButtonActivenessDecider(ICruiser playerCruiser)
        {
            return new StaticDecider<IBuilding>(shouldBeEnabled: false);
        }

        public BasicDecider CreateNavigationDecider()
        {
            return NavigationPermitter;
        }

        public BasicDecider CreateBackButtonDecider()
        {
            return BackButtonPermitter;
        }

        public ILastBuildingStartedProvider CreateLastBuildingStartedProvider(ICruiserController cruiser)
        {
            return new LastBuildingStartedProvider(cruiser);
        }
    }
}
