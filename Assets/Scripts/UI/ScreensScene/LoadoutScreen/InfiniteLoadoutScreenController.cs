using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.UI.ScreensScene.ProfileScreen;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using System.Collections.Generic;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class InfiniteLoadoutScreenController : ScreenController, ILoadoutScreenController, IManagedDisposable
    {
        private PrefabFactory _prefabFactory;
        private ItemDetails.IItemDetailsManager _itemDetailsManager;
        private IComparingItemFamilyTracker _comparingFamilyTracker;
        private LoadoutItemColourControllerV2 _loadoutItemColourController;

        public ItemDetailsPanel itemDetailsPanel;
        public ItemPanelsController itemPanels;
        public CategoryButtonsPanel categoryButtonsPanel;
        public CompareButton compareButton;
        public SelectCruiserButton selectCruiserButton;
        public SelectBuildingButton selectBuildingButton;
        public SelectUnitButton selectUnitButton;
        public SelectHeckleButton selectHeckleButton;
        public CancelButtonController homeButton;
        public LimitDisplayer limitDisplayer;

        public HeckleDetailsController _heckleDetails;
        public BodykitDetailController _bodykitDetails;
        public BuildingDetailController _buildingDetails;
        public UnitDetailController _unitDetails;

        public CanvasGroupButton heckleButton;
        public CanvasGroupButton shopButton;


        private ISingleSoundPlayer _soundPlayer;
        IScreensSceneGod _screensSceneGod;
        private IList<IItemButton> _itemButtons = new List<IItemButton>();



        public void Initialise(
            IScreensSceneGod screensSceneGod,
            ISingleSoundPlayer soundPlayer,
            PrefabFactory prefabFactory)
        {
            _soundPlayer = soundPlayer;
            _screensSceneGod = screensSceneGod;
            Logging.Log(Tags.SCREENS_SCENE_GOD, "START");

            base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(itemDetailsPanel, itemPanels, categoryButtonsPanel, compareButton, selectCruiserButton, homeButton, heckleButton);
            Helper.AssertIsNotNull(prefabFactory);

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
            _heckleDetails.Initialize();
            _itemDetailsManager = new LoadoutScreen.ItemDetails.ItemDetailsManager(buildingDetails, unitDetails, cruiserDetails);
            _itemDetailsManager.HeckleDetails = _heckleDetails;

            _comparingFamilyTracker = new ComparingItemFamilyTracker();
            IComparisonStateTracker comparisonStateTracker = new ComparisonStateTracker(_comparingFamilyTracker.ComparingFamily, _itemDetailsManager);

            compareButton.Initialise(soundPlayer, _itemDetailsManager, _comparingFamilyTracker, comparisonStateTracker);
            selectCruiserButton
                .Initialise(
                    soundPlayer,
                    cruiserDetails,
                    comparisonStateTracker,
                    new HullNameToKey(DataProvider.GameModel.UnlockedHulls, prefabFactory));

            selectBuildingButton.Initialise(
                soundPlayer,
                buildingDetails,
                new BuildingNameToKey(DataProvider.GameModel.UnlockedBuildings, prefabFactory),
                _comparingFamilyTracker.ComparingFamily,
                _comparingFamilyTracker);

            selectUnitButton.Initialise(
                soundPlayer,
                unitDetails,
                new UnitNameToKey(DataProvider.GameModel.UnlockedUnits, prefabFactory),
                _comparingFamilyTracker.ComparingFamily,
                _comparingFamilyTracker);

            selectHeckleButton.Initialise(soundPlayer, _heckleDetails, _comparingFamilyTracker.ComparingFamily,
                _comparingFamilyTracker);

            limitDisplayer.Initialise(
                buildingDetails,
                unitDetails,
                _heckleDetails,
                _comparingFamilyTracker);

            IList<IItemButton> itemButtons
                = itemPanels.Initialise(
                    _itemDetailsManager,
                    ItemType.Hull,
                    _comparingFamilyTracker,
                    selectCruiserButton.SelectedHull,
                    soundPlayer,
                    prefabFactory);

            _itemButtons = itemButtons;
            _bodykitDetails.RegisterSelectedHull(selectCruiserButton.SelectedHull);
            _loadoutItemColourController = new LoadoutItemColourControllerV2(_itemDetailsManager, itemButtons);
            categoryButtonsPanel.Initialise(itemPanels, _comparingFamilyTracker.ComparingFamily, soundPlayer, DataProvider.GameModel, itemButtons, _comparingFamilyTracker);
            homeButton.Initialise(soundPlayer, this);
            heckleButton.Initialise(soundPlayer, ShowHeckles);
            shopButton.Initialise(soundPlayer, GoToBodykitsShop);

            ShowPlayerHull();

            Logging.Log(Tags.SCREENS_SCENE_GOD, "END");
        }

        public void AddHeckle(HeckleData heckleData)
        {
            itemPanels.AddHeckle(heckleData);
            /*IList<IItemButton> itemButtons
                = await itemPanels.Initialise(
                    _itemDetailsManager,
                    ItemType.Hull,
                    _comparingFamilyTracker,
                    DataProvider.GameModel,
                    selectCruiserButton.SelectedHull,
                    _soundPlayer,
                    _prefabFactory);
                    */
        }

        private void ShowHeckles()
        {

            _itemDetailsManager.HideDetails();
            itemPanels.CurrentlyShownPanel?.Hide();
            itemPanels.ShowHecklePanel();
        }

        private void GoToBodykitsShop()
        {
            _screensSceneGod.GoToBodykitsShop();
        }


        private void ShowPlayerHull()
        {
            ICruiser playerCruiser = _prefabFactory.GetCruiserPrefab(DataProvider.GameModel.PlayerLoadout.Hull);
            _itemDetailsManager.ShowDetails(playerCruiser);
            _itemDetailsManager.ShowDetails(GetHullType(DataProvider.GameModel.PlayerLoadout.Hull));
            _comparingFamilyTracker.SetComparingFamily(ItemFamily.Hulls);
            _comparingFamilyTracker.SetComparingFamily(null);
        }

        public void RefreshBodykitsUI()
        {
            _itemDetailsManager.ShowDetails(GetHullType(DataProvider.GameModel.PlayerLoadout.Hull));
        }

        private HullType GetHullType(HullKey hullKey)
        {
            switch (hullKey.PrefabName)
            {
                case "Trident":
                    return HullType.Trident;
                case "BlackRig":
                    return HullType.BlackRig;
                case "Bullshark":
                    return HullType.Bullshark;
                case "Eagle":
                    return HullType.Eagle;
                case "Flea":
                    return HullType.Flea;
                case "Goatherd":
                    return HullType.Goatherd;
                case "Hammerhead":
                    return HullType.Hammerhead;
                case "Longbow":
                    return HullType.Longbow;
                case "Megalodon":
                    return HullType.Megalodon;
                case "Megalith":
                    return HullType.Megalith;
                case "Microlodon":
                    return HullType.Microlodon;
                case "Raptor":
                    return HullType.Raptor;
                case "Rickshaw":
                    return HullType.Rickshaw;
                case "Rockjaw":
                    return HullType.Rockjaw;
                case "Pistol":
                    return HullType.Pistol;
                case "Shepherd":
                    return HullType.Shepherd;
                case "TasDevil":
                    return HullType.TasDevil;
                default:
                    return HullType.Yeti;
            }
        }

        public override void Cancel()
        {
            DataProvider.SaveGame();
            DataProvider.CloudSave();
            _comparingFamilyTracker.SetComparingFamily(null);
            _screensSceneGod.GotoHubScreen();
        }

        public void DisposeManagedState()
        {
            categoryButtonsPanel.DisposeManagedState();
        }
    }
}