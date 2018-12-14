using BattleCruisers.AI;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.Tutorial;
using BattleCruisers.Tutorial.Steps.Providers;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Filters;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Threading;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.BattleScene
{
    public class TutorialHelper : IBattleSceneHelper, ITutorialProvider
    {
        private readonly IDataProvider _dataProvider;
        private readonly SpecificSlotsFilter _slotFilter;
        private readonly BuildingNameFilter _buildingNameFilter;
        private readonly BuildingCategoryFilter _buildingCategoryFilter;
        private LimitableUIManager _uiManager;

        public ISlotPermitter SlotPermitter { get { return _slotFilter; } }
        public IBuildingCategoryPermitter BuildingCategoryPermitter { get { return _buildingCategoryFilter; } }
        public IBroadcastingFilter<IBuildable> ShouldBuildingBeEnabledFilter { get { return _buildingNameFilter; } }
        public IBuildingPermitter BuildingPermitter { get { return _buildingNameFilter; } }
        public BroadcastingFilter BackButtonPermitter { get; private set; }
        // FELIX  Create IPermitter to set IsMatch?
        public BroadcastingFilter SpeedButtonsPermitter { get; private set; }
        public IUIManagerSettablePermissions UIManagerPermissions { get; private set; }
        public BroadcastingFilter IsNavigationEnabledFilter { get; private set; }

        public ISingleBuildableProvider SingleAircraftProvider { get; private set; }
        public ISingleBuildableProvider SingleShipProvider { get; private set; }
        public ISingleBuildableProvider SingleOffensiveProvider { get; private set; }

        public IBuildProgressCalculator PlayerCruiserBuildProgressCalculator { get; private set; }
		public IBuildSpeedController PlayerCruiserBuildSpeedController { get; private set; }
        public IBuildProgressCalculator AICruiserBuildProgressCalculator { get; private set; }
        public IBuildSpeedController AICruiserBuildSpeedController { get; private set; }
        public IUserChosenTargetHelperSettablePermissions UserChosenTargetPermissions { get; private set; }

        public TutorialHelper(IDataProvider dataProvider, IPrefabFactory prefabFactory)
        {
            Helper.AssertIsNotNull(dataProvider, prefabFactory);

            _dataProvider = dataProvider;

            _slotFilter = new SpecificSlotsFilter();
            _buildingNameFilter = new BuildingNameFilter(prefabFactory);
            _buildingCategoryFilter = new BuildingCategoryFilter();
            BackButtonPermitter = new BroadcastingFilter(isMatch: false);
            SingleAircraftProvider = new SingleBuildableProvider(GameObjectTags.AIRCRAFT);
            SingleShipProvider = new SingleBuildableProvider(GameObjectTags.SHIP);
            SingleOffensiveProvider = new SingleBuildableProvider(GameObjectTags.OFFENSIVE);
            IsNavigationEnabledFilter = new BroadcastingFilter(isMatch: false);
            SpeedButtonsPermitter = new BroadcastingFilter(isMatch: false);

			IBuildProgressCalculator slowCalculator = new AsymptoticCalculator();
            IBuildProgressCalculator normalCalculator = new LinearCalculator(BuildSpeedMultipliers.DEFAULT_TUTORIAL);
            IBuildProgressCalculator fastCalculator = new LinearCalculator(BuildSpeedMultipliers.FAST);

            CompositeCalculator playerCruiserBuildSpeedCalculator = new CompositeCalculator(slowCalculator, normalCalculator, fastCalculator);
            PlayerCruiserBuildProgressCalculator = playerCruiserBuildSpeedCalculator;
            PlayerCruiserBuildSpeedController = playerCruiserBuildSpeedCalculator;

            CompositeCalculator aiCruiserBuildSpeedCalculator = new CompositeCalculator(slowCalculator, normalCalculator, fastCalculator);
            AICruiserBuildProgressCalculator = aiCruiserBuildSpeedCalculator;
            AICruiserBuildSpeedController = aiCruiserBuildSpeedCalculator;
        }
        
        public ILoadout GetPlayerLoadout()
        {
            return _dataProvider.StaticData.InitialGameModel.PlayerLoadout;
        }
		
        public IArtificialIntelligence CreateAI(ICruiserController aiCruiser, ICruiserController playerCruiser, int currentLevelNum)
		{
            // There is no AI for the tutorial :)
            return new DummyArtificialIntelligence();
		}
		
		public ISlotFilter CreateHighlightableSlotFilter()
		{
            return _slotFilter;
		}

        public ISingleBuildableProvider CreateLastIncompleteBuildingStartedProvider(ICruiserController cruiser)
        {
            return new LastIncompleteBuildingStartedProvider(cruiser);
        }

        public IFilter<ITarget> CreateDeletButtonVisiblityFilter()
        {
            return new StaticFilter<ITarget>(isMatch: false);
        }

        public IButtonVisibilityFilters CreateButtonVisibilityFilters(IDroneManager droneManager)
        {
            return
                new ButtonVisibilityFilters(
                    _buildingNameFilter,
                    _buildingCategoryFilter,
                    new StaticFilter<ITarget>(isMatch: false),
                    new StaticFilter<ITarget>(isMatch: false),
                    BackButtonPermitter,
                    SpeedButtonsPermitter);
        }

        public IManagedDisposable CreateDroneEventSoundPlayer(ICruiser playerCruiser, IVariableDelayDeferrer deferrer)
        {
            return new NullManagedDispoasable();
        }

        public IPrioritisedSoundPlayer GetBuildableButtonSoundPlayer(ICruiser playerCruiser)
        {
            return new DummySoundPlayer();
        }

        public IBroadcastingFilter CreateNavigationWheelEnabledFilter()
        {
            return IsNavigationEnabledFilter;
        }

        public IUIManager CreateUIManager()
        {
            Assert.IsNull(_uiManager, "CreateUIManager() should only be called once");
            _uiManager = new LimitableUIManager();
            return _uiManager;
        }

        public void InitialiseUIManager(ManagerArgs args)
        {
            Assert.IsNotNull(_uiManager, "InitialiseUIManager() should only be called after CreaetUIManager()");

            UIManagerPermissions uiManagerPermissions = new UIManagerPermissions()
            {
                CanDismissItemDetails = false,
                CanShowItemDetails = false
            };
            UIManagerPermissions = uiManagerPermissions;

            _uiManager.Initialise(args, uiManagerPermissions);
        }

        public IUserChosenTargetHelper CreateUserChosenTargetHelper(
            IUserChosenTargetManager playerCruiserUserChosenTargetManager, 
            IPrioritisedSoundPlayer soundPlayer)
        {
            Helper.AssertIsNotNull(playerCruiserUserChosenTargetManager, soundPlayer);

            UserChosenTargetHelperPermissions permissions = new UserChosenTargetHelperPermissions(isEnabled: false);
            UserChosenTargetPermissions = permissions;

            return
                new TogglableUserChosenTargetHelper(
                    new UserChosenTargetHelper(playerCruiserUserChosenTargetManager, soundPlayer),
                    permissions);
        }
    }
}
