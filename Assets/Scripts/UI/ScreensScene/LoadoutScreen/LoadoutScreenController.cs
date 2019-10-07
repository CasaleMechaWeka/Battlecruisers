using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Scenes;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class LoadoutScreenController : ScreenController, ICancellable
    {
        private IDataProvider _dataProvider;
        private IPrefabFactory _prefabFactory;
        private IItemDetailsManager _itemDetailsManager;
        private IComparingItemFamilyTracker _comparingFamilyTracker;
        private LoadoutItemColourController _loadoutItemColourController;

        public void Initialise(
            ISoundPlayer soundPlayer,
            IScreensSceneGod screensSceneGod,
            IDataProvider dataProvider,
            IPrefabFactory prefabFactory)
        {
            Logging.Log(Tags.SCREENS_SCENE_GOD, "START");

            base.Initialise(soundPlayer, screensSceneGod);

            Helper.AssertIsNotNull(dataProvider, prefabFactory);

            _dataProvider = dataProvider;
            _prefabFactory = prefabFactory;


            // FELIX  TEMP
            if (!_dataProvider.GameModel.NewHulls.Items.Contains(StaticPrefabKeys.Hulls.Longbow))
            {
                _dataProvider.GameModel.NewHulls.AddItem(StaticPrefabKeys.Hulls.Longbow);
            }
            if (!_dataProvider.GameModel.NewBuildings.Items.Contains(StaticPrefabKeys.Buildings.AntiShipTurret))
            {
                _dataProvider.GameModel.NewBuildings.AddItem(StaticPrefabKeys.Buildings.AntiShipTurret);
            }
            if (!_dataProvider.GameModel.NewUnits.Items.Contains(StaticPrefabKeys.Units.ArchonBattleship))
            {
                _dataProvider.GameModel.NewUnits.AddItem(StaticPrefabKeys.Units.ArchonBattleship);
            }


            ItemDetailsPanel itemDetailsPanel = GetComponentInChildren<ItemDetailsPanel>(includeInactive: true);
            Assert.IsNotNull(itemDetailsPanel);
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

            CompareButton compareButton = GetComponentInChildren<CompareButton>();
            Assert.IsNotNull(compareButton);
            compareButton.Initialise(soundPlayer, _itemDetailsManager, _comparingFamilyTracker, comparisonStateTracker);

            SelectCruiserButton selectCruiserButton = GetComponentInChildren<SelectCruiserButton>();
            Assert.IsNotNull(selectCruiserButton);
            selectCruiserButton
                .Initialise(
                    soundPlayer,
                    cruiserDetails,
                    comparisonStateTracker,
                    new HullNameToKey(_dataProvider.GameModel.UnlockedHulls, prefabFactory),
                    _dataProvider);

            ItemPanelsController itemPanels = GetComponentInChildren<ItemPanelsController>(includeInactive: true);
            Assert.IsNotNull(itemPanels);
            IList<IItemButton> itemButtons 
                = itemPanels.Initialise(
                    _itemDetailsManager, 
                    ItemType.Hull, 
                    _comparingFamilyTracker, 
                    dataProvider.GameModel, 
                    selectCruiserButton.SelectedHull, 
                    soundPlayer);

            _loadoutItemColourController = new LoadoutItemColourController(_itemDetailsManager, itemButtons);

            CategoryButtonsPanel categoryButtonsPanel = GetComponentInChildren<CategoryButtonsPanel>(includeInactive: true);
            Assert.IsNotNull(categoryButtonsPanel);
            categoryButtonsPanel.Initialise(itemPanels, _comparingFamilyTracker.ComparingFamily, soundPlayer, _dataProvider.GameModel);

            ShowPlayerHull();

            CancelButtonController homeButton = GetComponentInChildren<CancelButtonController>();
            Assert.IsNotNull(homeButton);
            homeButton.Initialise(soundPlayer, this);

            Logging.Log(Tags.SCREENS_SCENE_GOD, "END");
        }

        private void ShowPlayerHull()
        {
            ICruiser playerCruiser = _prefabFactory.GetCruiserPrefab(_dataProvider.GameModel.PlayerLoadout.Hull);
            _itemDetailsManager.ShowDetails(playerCruiser);
        }

        public void Cancel()
        {
            _comparingFamilyTracker.SetComparingFamily(null);
            _screensSceneGod.GoToHomeScreen();
        }
    }
}