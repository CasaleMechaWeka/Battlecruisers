using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using System.Collections.Generic;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class InfiniteLoadoutScreenController : ScreenController, ILoadoutScreenController, IManagedDisposable
    {
        private IDataProvider _dataProvider;
        private IPrefabFactory _prefabFactory;
        private IItemDetailsManager _itemDetailsManager;
        private IComparingItemFamilyTracker _comparingFamilyTracker;
        private LoadoutItemColourControllerV2 _loadoutItemColourController;

        public ItemDetailsPanel itemDetailsPanel;
        public ItemPanelsController itemPanels;
        public CategoryButtonsPanel categoryButtonsPanel;
        public CompareButton compareButton;
        public SelectCruiserButton selectCruiserButton;
        public SelectBuildingButton selectBuildingButton;
        public SelectUnitButton selectUnitButton;
        public CancelButtonController homeButton;
        public LimitDisplayer limitDisplayer;

        public void Initialise(
            IScreensSceneGod screensSceneGod,
            ISingleSoundPlayer soundPlayer,
            IDataProvider dataProvider,
            IPrefabFactory prefabFactory)
        {
            Logging.Log(Tags.SCREENS_SCENE_GOD, "START");

            base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(itemDetailsPanel, itemPanels, categoryButtonsPanel, compareButton, selectCruiserButton, homeButton);
            Helper.AssertIsNotNull(dataProvider, prefabFactory);

            _dataProvider = dataProvider;
            _prefabFactory = prefabFactory;

            itemDetailsPanel.Initialise();

            IItemDetailsDisplayer<IBuilding> buildingDetails
                = new ItemDetailsDisplayer<IBuilding>(
                    itemDetailsPanel.LeftBuildingDetails,
                    itemDetailsPanel.RightBuildingDetails);

            IItemDetailsDisplayer<IUnit> unitDetails
                = new ItemDetailsDisplayer<IUnit>(
                    itemDetailsPanel.LeftUnitDetails,
                    itemDetailsPanel.RightUnitDetails);

            IItemDetailsDisplayer<ICruiser> cruiserDetails
                = new ItemDetailsDisplayer<ICruiser>(
                    itemDetailsPanel.LeftCruiserDetails,
                    itemDetailsPanel.RightCruiserDetails);

            _itemDetailsManager = new ItemDetailsManager(buildingDetails, unitDetails, cruiserDetails);

            _comparingFamilyTracker = new ComparingItemFamilyTracker();
            IComparisonStateTracker comparisonStateTracker = new ComparisonStateTracker(_comparingFamilyTracker.ComparingFamily, _itemDetailsManager);

            compareButton.Initialise(soundPlayer, _itemDetailsManager, _comparingFamilyTracker, comparisonStateTracker);
            selectCruiserButton
                .Initialise(
                    soundPlayer,
                    cruiserDetails,
                    comparisonStateTracker,
                    new HullNameToKey(_dataProvider.GameModel.UnlockedHulls, prefabFactory),
                    _dataProvider);

            selectBuildingButton.Initialise(
                soundPlayer,
                dataProvider,
                buildingDetails,
                new BuildingNameToKey(_dataProvider.GameModel.UnlockedBuildings, prefabFactory),
                _comparingFamilyTracker.ComparingFamily,
                _comparingFamilyTracker);

            selectUnitButton.Initialise(
                soundPlayer,
                dataProvider,
                unitDetails,
                new UnitNameToKey(_dataProvider.GameModel.UnlockedUnits, prefabFactory),
                _comparingFamilyTracker.ComparingFamily,
                _comparingFamilyTracker);

            limitDisplayer.Initialise(dataProvider,
                buildingDetails,
                unitDetails,
                _comparingFamilyTracker);

            IList<IItemButton> itemButtons
                = itemPanels.Initialise(
                    _itemDetailsManager,
                    ItemType.Hull,
                    _comparingFamilyTracker,
                    dataProvider.GameModel,
                    selectCruiserButton.SelectedHull,
                    soundPlayer,
                    prefabFactory);

            _loadoutItemColourController = new LoadoutItemColourControllerV2(_itemDetailsManager, itemButtons);
            categoryButtonsPanel.Initialise(itemPanels, _comparingFamilyTracker.ComparingFamily, soundPlayer, _dataProvider.GameModel, itemButtons, _comparingFamilyTracker);
            homeButton.Initialise(soundPlayer, this);

            ShowPlayerHull();

            Logging.Log(Tags.SCREENS_SCENE_GOD, "END");
        }

        private void ShowPlayerHull()
        {
            ICruiser playerCruiser = _prefabFactory.GetCruiserPrefab(_dataProvider.GameModel.PlayerLoadout.Hull);
            _itemDetailsManager.ShowDetails(playerCruiser);
        }

        public override void Cancel()
        {
            _dataProvider.SaveGame();
            _comparingFamilyTracker.SetComparingFamily(null);
            _screensSceneGod.GoToHomeScreen();
        }

        public void DisposeManagedState()
        {
            categoryButtonsPanel.DisposeManagedState();
        }
    }
}