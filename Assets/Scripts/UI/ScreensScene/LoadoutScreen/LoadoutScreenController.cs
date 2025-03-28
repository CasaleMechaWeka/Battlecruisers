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
    public class LoadoutScreenController : ScreenController, ILoadoutScreenController, IManagedDisposable
    {
        private IItemDetailsManager _itemDetailsManager;
        private IComparingItemFamilyTracker _comparingFamilyTracker;
        private LoadoutItemColourController _loadoutItemColourController;

        public ItemDetailsPanel itemDetailsPanel;
        public ItemPanelsController itemPanels;
        public CategoryButtonsPanel categoryButtonsPanel;
        public CompareButton compareButton;
        public SelectCruiserButton selectCruiserButton;
        public CancelButtonController homeButton;

        public void Initialise(
            IScreensSceneGod screensSceneGod,
            ISingleSoundPlayer soundPlayer)
        {
            Logging.Log(Tags.SCREENS_SCENE_GOD, "START");

            base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(itemDetailsPanel, itemPanels, categoryButtonsPanel, compareButton, selectCruiserButton, homeButton);

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
                    new HullNameToKey(DataProvider.GameModel.UnlockedHulls));

            IList<IItemButton> itemButtons
                = itemPanels.Initialise(
                    _itemDetailsManager,
                    ItemType.Hull,
                    _comparingFamilyTracker,
                    selectCruiserButton.SelectedHull,
                    soundPlayer);

            _loadoutItemColourController = new LoadoutItemColourController(_itemDetailsManager, itemButtons);
            categoryButtonsPanel.Initialise(itemPanels, _comparingFamilyTracker.ComparingFamily, soundPlayer, DataProvider.GameModel, itemButtons, _comparingFamilyTracker);
            homeButton.Initialise(soundPlayer, this);

            ShowPlayerHull();

            Logging.Log(Tags.SCREENS_SCENE_GOD, "END");
        }

        private void ShowPlayerHull()
        {
            ICruiser playerCruiser = PrefabFactory.GetCruiserPrefab(DataProvider.GameModel.PlayerLoadout.Hull);
            _itemDetailsManager.ShowDetails(playerCruiser);
        }

        public override void Cancel()
        {
            DataProvider.SaveGame();
            _comparingFamilyTracker.SetComparingFamily(null);
            _screensSceneGod.GoToHomeScreen();
        }

        public void DisposeManagedState()
        {
            categoryButtonsPanel.DisposeManagedState();
        }
    }
}