using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Data;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static;
using BattleCruisers.Scenes;
using BattleCruisers.UI.Common.BuildableDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.ItemDetails;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Items;
using BattleCruisers.UI.ScreensScene.ShopScreen;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleCruisers.UI.ScreensScene.LoadoutScreen
{
    public class InfiniteLoadoutScreenController : ScreenController, IManagedDisposable
    {
        private ItemDetails.ItemDetailsManager _itemDetailsManager;
        private ComparingItemFamilyTracker _comparingFamilyTracker;
        private LoadoutItemColourControllerV2 _loadoutItemColourController;

        public ItemDetailsPanel itemDetailsPanel;
        public ItemPanelsController itemPanels;
        public CategoryButtonsPanel categoryButtonsPanel;
        public SelectCruiserButton selectCruiserButton;
        public SelectBuildingButton selectBuildingButton;
        public SelectUnitButton selectUnitButton;
        public CancelButtonController homeButton;
        public LimitDisplayer limitDisplayer;

        public ProfileDetailsController profileDetails;
        public BodykitDetailController bodykitDetails;
        public BuildingDetailController buildingDetails;
        public UnitDetailController unitDetails;

        public CanvasGroupButton profileButton;
        public CanvasGroupButton shopButton;

        private SingleSoundPlayer _soundPlayer;
        private IList<ItemButton> _itemButtons = new List<ItemButton>();
        private bool _purchaseModeActive;

        [SerializeField, Tooltip("Limit tracker container to force-hide on Hulls")]
        private GameObject limitTrackerGroup;

        public void Initialise(
            ScreensSceneGod screensSceneGod,
            SingleSoundPlayer soundPlayer)
        {
            _soundPlayer = soundPlayer;
            _screensSceneGod = screensSceneGod;
            Logging.Log(Tags.SCREENS_SCENE_GOD, "START");

            base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(itemDetailsPanel, itemPanels, categoryButtonsPanel, selectCruiserButton, homeButton, profileButton);

            itemDetailsPanel.Initialise();

            ItemDetailsDisplayer<IBuilding> buildingDetails
                = new ItemDetailsDisplayer<IBuilding>(
                    itemDetailsPanel.LeftBuildingDetails,
                    itemDetailsPanel.RightBuildingDetails);

            ItemDetailsDisplayer<IUnit> unitDetails
                = new ItemDetailsDisplayer<IUnit>(
                    itemDetailsPanel.LeftUnitDetails,
                    itemDetailsPanel.RightUnitDetails);

            ItemDetailsDisplayer<ICruiser> cruiserDetails
                = new ItemDetailsDisplayer<ICruiser>(
                    itemDetailsPanel.LeftCruiserDetails,
                    itemDetailsPanel.RightCruiserDetails);
            _ = profileDetails.Initialize(soundPlayer);
            _itemDetailsManager = new ItemDetails.ItemDetailsManager(buildingDetails, unitDetails, cruiserDetails);
            _itemDetailsManager.ProfileDetails = profileDetails;

            _comparingFamilyTracker = new ComparingItemFamilyTracker();
            ComparisonStateTracker comparisonStateTracker = new ComparisonStateTracker(_comparingFamilyTracker.ComparingFamily, _itemDetailsManager);

            selectCruiserButton
                .Initialise(
                    soundPlayer,
                    cruiserDetails,
                    comparisonStateTracker,
                    new HullNameToKey(DataProvider.GameModel.UnlockedHulls));

            selectBuildingButton.Initialise(
                soundPlayer,
                buildingDetails,
                new BuildingNameToKey(DataProvider.GameModel.UnlockedBuildings),
                _comparingFamilyTracker.ComparingFamily,
                _comparingFamilyTracker);

            selectUnitButton.Initialise(
                soundPlayer,
                unitDetails,
                new UnitNameToKey(DataProvider.GameModel.UnlockedUnits),
                _comparingFamilyTracker.ComparingFamily,
                _comparingFamilyTracker);

            limitDisplayer.Initialise(
                buildingDetails,
                unitDetails,
                profileDetails,
                _comparingFamilyTracker);

            IList<ItemButton> itemButtons
                = itemPanels.Initialise(
                    _itemDetailsManager,
                    ItemType.Hull,
                    _comparingFamilyTracker,
                    selectCruiserButton.SelectedHull,
                    soundPlayer);

            _itemButtons = itemButtons;
            bodykitDetails.RegisterSelectedHull(selectCruiserButton.SelectedHull);
            _loadoutItemColourController = new LoadoutItemColourControllerV2(_itemDetailsManager, itemButtons);
            categoryButtonsPanel.Initialise(itemPanels, _comparingFamilyTracker.ComparingFamily, soundPlayer, DataProvider.GameModel, itemButtons, _comparingFamilyTracker);
            homeButton.Initialise(soundPlayer, this);
            profileButton.Initialise(soundPlayer, ShowProfile);
            if (shopButton != null)
                shopButton.Initialise(soundPlayer, GoToBodykitsShop);

            // Subscribe to purchase mode events from all detail controllers
            var allUnitControllers = GetComponentsInChildren<UnitDetailController>(true);
            foreach (var ctrl in allUnitControllers)
            {
                if (ctrl != null)
                    ctrl.PurchaseModeToggled += OnAnyPurchaseModeToggled;
            }

            var allBuildingControllers = GetComponentsInChildren<BuildingDetailController>(true);
            foreach (var ctrl in allBuildingControllers)
            {
                if (ctrl != null)
                    ctrl.PurchaseModeToggled += OnAnyPurchaseModeToggled;
            }

            var allBodykitControllers = GetComponentsInChildren<BodykitDetailController>(true);
            foreach (var ctrl in allBodykitControllers)
            {
                if (ctrl != null)
                    ctrl.PurchaseModeToggled += OnAnyPurchaseModeToggled;
            }

            // Track panel changes to reset selection UI state and handle categories without variants
            itemPanels.PotentialMatchChange += OnPanelChanged;

            ShowPlayerHull();

            Logging.Log(Tags.SCREENS_SCENE_GOD, "END");
        }

        private void OnAnyPurchaseModeToggled(bool showPurchase)
        {
            _purchaseModeActive = showPurchase;
            ToggleSelectionUi(!showPurchase);
        }

        private void OnPanelChanged(object sender, System.EventArgs e)
        {
            // Defer final visibility until after the first item details render
            _purchaseModeActive = false;
            ToggleSelectionUi(true);
            StartCoroutine(ApplyPanelChangeUiNextFrame());
        }

        private IEnumerator ApplyPanelChangeUiNextFrame()
        {
            yield return null; // wait one frame for ShowDetails to run
            ToggleSelectionUi(!_purchaseModeActive);
        }

        private void ToggleSelectionUi(bool show)
        {
            // Marker-based discovery: toggle any SelectionUiGroup components under this screen
            var selectionGroups = GetComponentsInChildren<SelectionUiGroup>(true);
            foreach (var m in selectionGroups)
            {
                if (m == null || m.gameObject == null)
                    continue;
                bool isLimitTracker = limitTrackerGroup != null && m.gameObject == limitTrackerGroup;
                // Hide limit tracker on hulls and profile screens
                bool shouldShowLimitTracker = !isLimitTracker || (!itemPanels.IsMatch(Items.ItemType.Hull) && _itemDetailsManager.SelectedItemFamily != Items.ItemFamily.Profile);
                m.gameObject.SetActive(show && shouldShowLimitTracker);
            }
        }

        public void AddHeckle(HeckleData heckleData)
        {
            itemPanels.AddHeckle(heckleData);
            /*IList<ItemButton> itemButtons
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

        public void ShowProfile()
        {
            itemPanels.CurrentlyShownPanel?.Hide();
            itemPanels.ShowHecklePanel();
            _itemDetailsManager.ShowProfile();
        }

        private void GoToBodykitsShop()
        {
            _screensSceneGod.GoToBodykitsShop();
        }


        private void ShowPlayerHull()
        {
            ICruiser playerCruiser = PrefabFactory.GetCruiserPrefab(DataProvider.GameModel.PlayerLoadout.Hull);
            _itemDetailsManager.ShowDetails(playerCruiser);
            _itemDetailsManager.ShowDetails(StaticPrefabKeys.Hulls.GetHullType(DataProvider.GameModel.PlayerLoadout.Hull));
            _comparingFamilyTracker.SetComparingFamily(ItemFamily.Hulls);
            _comparingFamilyTracker.SetComparingFamily(null);
            ToggleSelectionUi(true);

            // Scroll to the player's selected hull in the hull panel
            ScrollToSelectedHull();
        }

        private void ScrollToSelectedHull()
        {
            ItemsPanel hullPanel = itemPanels.GetPanel(Items.ItemType.Hull);
            if (hullPanel != null)
            {
                HullKey selectedHullKey = DataProvider.GameModel.PlayerLoadout.Hull;

                // Find the hull button that corresponds to the player's selected hull
                foreach (ItemButton button in hullPanel.GetAllButtons())
                {
                    if (button is HullButtonV2 hullButton && hullButton.GetHullKey().Equals(selectedHullKey))
                    {
                        // Scroll to this button and make sure it's registered as selected
                        hullPanel.ScrollToButton(button);
                        hullPanel.RegisterSelection(button);
                        break;
                    }
                }
            }
        }

        public void RefreshBodykitsUI()
        {
            _itemDetailsManager.ShowDetails(StaticPrefabKeys.Hulls.GetHullType(DataProvider.GameModel.PlayerLoadout.Hull));
        }

        public override void Cancel()
        {
            DataProvider.SaveGame();
            _ = DataProvider.CloudSave();
            _comparingFamilyTracker.SetComparingFamily(null);
            _screensSceneGod.GotoHubScreen();
        }

        public void DisposeManagedState()
        {
            categoryButtonsPanel.DisposeManagedState();
        }
    }
}