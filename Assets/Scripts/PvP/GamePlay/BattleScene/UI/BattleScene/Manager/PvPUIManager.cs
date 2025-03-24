using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.BuildMenus;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.HeckleMessage;
using UnityEngine.Assertions;
using BattleCruisers.UI.Sound.Players;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager
{
    public class PvPUIManager : IPvPUIManager
    {
        private PvPCruiser _playerCruiser, _enemyCruiser;
        private IPvPBuildMenu _buildMenu;
        private IPvPItemDetailsManager _detailsManager;
        private IPrioritisedSoundPlayer _soundPlayer;
        private ISingleSoundPlayer _uiSoundPlayer;
        private IPvPBuilding lastClickedBuilding;
        private IPvPUnit lastClickedUnit;
        private IPvPCruiser lastClickedCruiser;
        private ITarget lastClickedBuildable;
        private int lastClickedType = -1; //-1 is for nothing, 0 is for buildings and 1 is for units

        private ITarget _shownItem;
        // private IPvPExplanationPanel _explanationPanel;
        public PvPHecklePanelController hecklePanelController { get; set; }
        // public void SetExplanationPanel(IExplanationPanel explanationPanelValue)
        // {
        //     _explanationPanel = explanationPanelValue;
        // }

        private ITarget ShownItem
        {
            set
            {
                if (_shownItem != null)
                {
                    _shownItem.Destroyed -= _shownItem_Destroyed;
                }

                _shownItem = value;

                if (_shownItem != null)
                {
                    _shownItem.Destroyed += _shownItem_Destroyed;
                }
            }
        }

        // Not in constructor because of circular dependency with:
        // + Build menu
        // + Cruisers
        public void Initialise(PvPManagerArgs args)
        {
            Assert.IsNotNull(args);

            _buildMenu = args.BuildMenu;
            _detailsManager = args.DetailsManager;
            _playerCruiser = args.PlayerCruiser;
            _enemyCruiser = args.EnemyCruiser;
            _soundPlayer = args.SoundPlayer;
            _uiSoundPlayer = args.UISoundPlayer;
        }
        public void SetHecklePanel(PvPHecklePanelController hecklePanel)
        {
            hecklePanelController = hecklePanel;
        }
        private void _shownItem_Destroyed(object sender, DestroyedEventArgs e)
        {
            HideItemDetails();
        }

        public virtual void HideItemDetails()
        {
            // Logging.LogMethod(Tags.UI_MANAGER);

            _detailsManager.HideDetails();


            // ServerRpc call
            // _playerCruiser.PvP_UnhighlightSlotsServerRpc();
            _playerCruiser.SlotHighlighter.UnhighlightSlots();
            // _enemyCruiser.SlotHighlighter.UnhighlightSlots();

            ShownItem = null;
            lastClickedBuilding = null;
            lastClickedUnit = null;
            lastClickedType = -1;
            lastClickedBuildable = null;
        }

        public void HideCurrentlyShownMenu()
        {
            // Logging.LogMethod(Tags.UI_MANAGER);

            _detailsManager.HideDetails();

            // _playerCruiser.SlotHighlighter.UnhighlightSlots();

            _buildMenu.HideCurrentlyShownMenu();
            ShownItem = null;
            lastClickedBuilding = null;
            lastClickedUnit = null;
            lastClickedType = -1;
            lastClickedBuildable = null;
        }

        public void SelectBuildingGroup(BuildingCategory buildingCategory)
        {
            // Logging.LogMethod(Tags.UI_MANAGER);

            HideItemDetails();
            _buildMenu.ShowBuildingGroupMenu(buildingCategory);
        }

        public async void SelectBuildingFromMenu(IPvPBuildableWrapper<IPvPBuilding> buildingWrapper)
        {
            // Logging.LogMethod(Tags.UI_MANAGER);
            _playerCruiser.SelectedBuildingPrefab = buildingWrapper;
            int variant_index = await PvPBattleSceneGodClient.Instance.dataProvider.GameModel.PlayerLoadout.GetSelectedBuildingVariantIndex(PvPBattleSceneGodClient.Instance.factoryProvider.PrefabFactory, buildingWrapper.Buildable);
            _playerCruiser.VariantIndexOfSelectedBuilding = variant_index;
            // ServerRpc call
            _playerCruiser.PvP_SelectedBuildingPrefabServerRpc(buildingWrapper.Buildable.Category, buildingWrapper.Buildable.PrefabName, variant_index);

            if (hecklePanelController != null)
                hecklePanelController.Hide();
            _detailsManager.ShowDetails(buildingWrapper.Buildable);

            bool wasAnySlotHighlighted = _playerCruiser.SlotHighlighter.HighlightAvailableSlots(buildingWrapper.Buildable.SlotSpecification);

            if (!wasAnySlotHighlighted)
            {
                _soundPlayer.PlaySound(PrioritisedSoundKeys.Events.Cruiser.NoBuildingSlotsLeft);
                _playerCruiser.SlotHighlighter.HighlightSlots(buildingWrapper.Buildable.SlotSpecification);
            }

            // ServerRpc call`  
            //    _playerCruiser.PvP_HighlightAvailableSlotsServerRpc(buildingWrapper.Buildable.SlotSpecification.SlotType, buildingWrapper.Buildable.SlotSpecification.BuildingFunction, buildingWrapper.Buildable.SlotSpecification.PreferFromFront);

            // ILocTable _commonStrings = await LocTableFactory.LoadTutorialTableAsync();

            // if (_explanationPanel != null)
            // {
            //     if (_hintDisplayer == null)
            //     {
            //         _hintDisplayer = new PvPNonRepeatingHintDisplayer(new PvPHintDisplayer(_explanationPanel));
            //     }
            //     _hintDisplayer.ShowHint(LocTableFactory.CommonTable.GetString("Steps/Touchdrag"));
            // }



        }

        public virtual void SelectBuilding(IPvPBuilding building)
        {
            // Logging.LogMethod(Tags.UI_MANAGER);
            if (hecklePanelController != null)
                hecklePanelController.Hide();
            _detailsManager.ShowDetails(building);
            _detailsManager.SelectBuilding(building);
            ShownItem = building;
            lastClickedBuilding = building;
            lastClickedBuildable = building;
            lastClickedType = 0;
        }

        public virtual void HideSlotsIfCannotAffordable()
        {
            _playerCruiser.SlotHighlighter.UnhighlightSlots();
            _enemyCruiser.SlotHighlighter.UnhighlightSlots();
            /*            ShownItem = null;
                        lastClickedBuilding = null;
                        lastClickedUnit = null;
                        lastClickedType = -1;
                        lastClickedBuildable = null;*/
        }
        public void ShowFactoryUnits(IPvPFactory factory)
        {
            // Logging.LogMethod(Tags.UI_MANAGER);

            if (ReferenceEquals(factory.ParentCruiser, _playerCruiser))
            {
                _buildMenu.ShowUnitsMenu(factory);
                _uiSoundPlayer.PlaySound(factory.SelectedSound);
            }
        }

        public virtual void ShowUnitDetails(IPvPUnit unit)
        {
            // Logging.LogMethod(Tags.UI_MANAGER);
            if (hecklePanelController != null)
                hecklePanelController.Hide();
            _detailsManager.ShowDetails(unit);
            _detailsManager.SelectUnit(unit);
            ShownItem = unit;
            lastClickedUnit = unit;
            lastClickedBuildable = unit;
            lastClickedType = 1;
        }

        public virtual void ShowCruiserDetails(IPvPCruiser cruiser)
        {
            // Logging.LogMethod(Tags.UI_MANAGER);
            if (hecklePanelController != null)
                hecklePanelController.Hide();
            _detailsManager.ShowDetails(cruiser);
            ShownItem = cruiser;
            lastClickedCruiser = cruiser;
            lastClickedBuildable = cruiser;
            lastClickedType = 2;
        }

        public void PeakBuildingDetails(IPvPBuilding building)
        {
            _detailsManager.ShowDetails(building);
            ShownItem = building;
        }

        public void PeakUnitDetails(IPvPUnit unit)
        {
            if (hecklePanelController != null)
                hecklePanelController.Hide();
            _detailsManager.ShowDetails(unit);
            ShownItem = unit;
        }

        public void UnpeakBuildingDetails()
        {
            if (lastClickedBuildable != null)
            {
                if (lastClickedType == 0)
                {
                    if (hecklePanelController != null)
                        hecklePanelController.Hide();
                    _detailsManager.ShowDetails(lastClickedBuilding);
                }
                else if (lastClickedType == 1)
                {
                    if (hecklePanelController != null)
                        hecklePanelController.Hide();
                    _detailsManager.ShowDetails(lastClickedUnit);
                }
                else
                {
                    if (hecklePanelController != null)
                        hecklePanelController.Hide();
                    _detailsManager.ShowDetails(lastClickedCruiser);
                }
            }
            else
            {
                _detailsManager.HideDetails();
            }
        }

        public void UnpeakUnitDetails()
        {
            if (lastClickedBuildable != null)
            {
                if (lastClickedType == 0)
                {
                    if (hecklePanelController != null)
                        hecklePanelController.Hide();
                    _detailsManager.ShowDetails(lastClickedBuilding);
                }
                else if (lastClickedType == 1)
                {
                    if (hecklePanelController != null)
                        hecklePanelController.Hide();
                    _detailsManager.ShowDetails(lastClickedUnit);
                }
                else
                {
                    if (hecklePanelController != null)
                        hecklePanelController.Hide();
                    _detailsManager.ShowDetails(lastClickedCruiser);
                }
            }
            else
            {
                _detailsManager.HideDetails();
            }
        }
    }
}
