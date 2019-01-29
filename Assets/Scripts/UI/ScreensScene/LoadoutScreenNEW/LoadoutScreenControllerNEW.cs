using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreenNEW.Items;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Properties;
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
        private ISettableBroadcastingProperty<ItemFamily?> _itemFamilyToCompare;

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

            _itemFamilyToCompare = new SettableBroadcastingProperty<ItemFamily?>(initialValue: null);
            IComparisonStateTracker comparisonStateTracker = new ComparisonStateTracker(_itemFamilyToCompare, _itemDetailsManager);

            itemDetailsPanel.InitialiseComponents(_itemDetailsManager, _itemFamilyToCompare, comparisonStateTracker);

            ItemPanelsController itemPanels = GetComponentInChildren<ItemPanelsController>(includeInactive: true);
            Assert.IsNotNull(itemPanels);
            itemPanels.Initialise(_itemDetailsManager, ItemType.Hull, _itemFamilyToCompare);

            CategoryButtonsPanel categoryButtonsPanel = GetComponentInChildren<CategoryButtonsPanel>(includeInactive: true);
            Assert.IsNotNull(categoryButtonsPanel);
            categoryButtonsPanel.Initialise(itemPanels, _itemFamilyToCompare);

            ShowPlayerHull();
        }

        private void ShowPlayerHull()
        {
            ICruiser playerCruiser = _prefabFactory.GetCruiserPrefab(_dataProvider.GameModel.PlayerLoadout.Hull);
            _itemDetailsManager.ShowDetails(playerCruiser);
        }

        public void Save()
        {
            // FELIX  Save hull choice :)
            Cancel();
        }

        public void Cancel()
        {
            _itemFamilyToCompare.Value = null;
            _itemDetailsManager.HideDetails();
            _screensSceneGod.GoToHomeScreen();
        }
    }
}