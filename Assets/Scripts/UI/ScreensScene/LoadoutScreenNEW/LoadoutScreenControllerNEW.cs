using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreenNEW
{
    // FELIX  Delete all old classes :D
    // FELIX  Remove NEW from all file and class names :)
    public class LoadoutScreenControllerNEW : ScreenController
    {
        private IDataProvider _dataProvider;
        private IPrefabFactory _prefabFactory;
        private IItemDetailsManager _itemDetailsManager;
        private IComparingItemFamilyTracker _comparingFamilyTracker;
        private SelectCruiserButton _selectCruiserButton;

        public void Initialise(
            IScreensSceneGod screensSceneGod, 
            IDataProvider dataProvider,
            IPrefabFactory prefabFactory)
        {
            base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(dataProvider, prefabFactory);

            _dataProvider = dataProvider;
            _prefabFactory = prefabFactory;

            ItemDetailsPanel itemDetailsPanel = GetComponentInChildren<ItemDetailsPanel>(includeInactive: true);
            Assert.IsNotNull(itemDetailsPanel);
            itemDetailsPanel.FindComponents();

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

            itemDetailsPanel.InitialiseComponents(_itemDetailsManager, _comparingFamilyTracker, comparisonStateTracker);

            _selectCruiserButton = GetComponentInChildren<SelectCruiserButton>();
            Assert.IsNotNull(_selectCruiserButton);
            _selectCruiserButton
                .Initialise(
                    cruiserDetails,
                    comparisonStateTracker,
                    new HullNameToKey(_dataProvider.GameModel.UnlockedHulls, prefabFactory),
                    _dataProvider.GameModel.PlayerLoadout.Hull);

            ItemPanelsController itemPanels = GetComponentInChildren<ItemPanelsController>(includeInactive: true);
            Assert.IsNotNull(itemPanels);
            itemPanels.Initialise(_itemDetailsManager, ItemType.Hull, _comparingFamilyTracker, dataProvider.GameModel, _selectCruiserButton.SelectedHull);

            CategoryButtonsPanel categoryButtonsPanel = GetComponentInChildren<CategoryButtonsPanel>(includeInactive: true);
            Assert.IsNotNull(categoryButtonsPanel);
            categoryButtonsPanel.Initialise(itemPanels, _comparingFamilyTracker.ComparingFamily);

            ShowPlayerHull();
        }

        private void ShowPlayerHull()
        {
            ICruiser playerCruiser = _prefabFactory.GetCruiserPrefab(_dataProvider.GameModel.PlayerLoadout.Hull);
            _itemDetailsManager.ShowDetails(playerCruiser);
        }

        public void Save()
        {
            ILoadout playerLoadout = _dataProvider.GameModel.PlayerLoadout;
            HullKey selectedHull = _selectCruiserButton.SelectedHull.Value;

            if (!playerLoadout.Hull.Equals(selectedHull))
            {
                playerLoadout.Hull = selectedHull;
                _dataProvider.SaveGame();
            }

            Cancel();
        }

        public void Cancel()
        {
            _comparingFamilyTracker.SetComparingFamily(null);
            _itemDetailsManager.HideDetails();
            _screensSceneGod.GoToHomeScreen();
        }
    }
}