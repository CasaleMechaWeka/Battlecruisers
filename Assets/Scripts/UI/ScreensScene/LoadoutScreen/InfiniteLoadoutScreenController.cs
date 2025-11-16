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
    public class InfiniteLoadoutScreenController : ScreenController, IManagedDisposable
    {
        private ItemDetails.ItemDetailsManager _itemDetailsManager;
        private ComparingItemFamilyTracker _comparingFamilyTracker;
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

        public ProfileDetailsController profileDetails;
        public BodykitDetailController bodykitDetails;
        public BuildingDetailController buildingDetails;
        public UnitDetailController unitDetails;

        public CanvasGroupButton profileButton;
        public CanvasGroupButton shopButton;


        private SingleSoundPlayer _soundPlayer;
        private IList<ItemButton> _itemButtons = new List<ItemButton>();

        public void Initialise(
            ScreensSceneGod screensSceneGod,
            SingleSoundPlayer soundPlayer)
        {
            _soundPlayer = soundPlayer;
            _screensSceneGod = screensSceneGod;
            Logging.Log(Tags.SCREENS_SCENE_GOD, "START");

            base.Initialise(screensSceneGod);

            Helper.AssertIsNotNull(itemDetailsPanel, itemPanels, categoryButtonsPanel, compareButton, selectCruiserButton, homeButton, profileButton);

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

            compareButton.Initialise(soundPlayer, _itemDetailsManager, _comparingFamilyTracker, comparisonStateTracker);
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
            shopButton.Initialise(soundPlayer, GoToBodykitsShop);

            ShowPlayerHull();

            Logging.Log(Tags.SCREENS_SCENE_GOD, "END");
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
            ICruiser playerCruiser = PrefabFactory.GetCruiserPrefab(DataProvider.GameModel.PlayerLoadout.Hull);
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
            return hullKey.PrefabName switch
            {
                "Trident" => HullType.Trident,
                "BlackRig" => HullType.BlackRig,
                "BasicRig" => HullType.BasicRig,
                "Bullshark" => HullType.Bullshark,
                "Cricket" => HullType.Cricket,
                "Eagle" => HullType.Eagle,
                "Flea" => HullType.Flea,
                "Goatherd" => HullType.Goatherd,
                "Hammerhead" => HullType.Hammerhead,
                "Longbow" => HullType.Longbow,
                "Megalodon" => HullType.Megalodon,
                "Megalith" => HullType.Megalith,
                "Microlodon" => HullType.Microlodon,
                "Raptor" => HullType.Raptor,
                "Rickshaw" => HullType.Rickshaw,
                "Rockjaw" => HullType.Rockjaw,
                "Pistol" => HullType.Pistol,
                "Shepherd" => HullType.Shepherd,
                "TasDevil" => HullType.TasDevil,
                "FortNova" => HullType.FortNova,
                "Zumwalt" => HullType.Zumwalt,
                "Yucalux" => HullType.Yucalux,
                "TekGnosis" => HullType.TekGnosis,
                "Salvage" => HullType.Salvage,
                "Orac" => HullType.Orac,
                "Middlodon" => HullType.Middlodon,
                "Essex" => HullType.Essex,
                "Axiom" => HullType.Axiom,
                "October" => HullType.October,
                "EndlessWall" => HullType.EndlessWall,
                "AlphaSpace" => HullType.AlphaSpace,
                "Arkdeso" => HullType.Arkdeso,
                _ => HullType.Yeti,
            };

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