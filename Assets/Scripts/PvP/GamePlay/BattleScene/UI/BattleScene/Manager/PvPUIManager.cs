using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.Tutorial.Explanation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.BuildMenus;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.InGameHints;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Utils.Localisation;
using UnityEngine;
using UnityEngine.Assertions;
using Unity.Netcode;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager
{
    public class PvPUIManager : IPvPUIManager
    {
        private PvPCruiser _playerCruiser, _enemyCruiser;
        private IPvPBuildMenu _buildMenu;
        private IPvPItemDetailsManager _detailsManager;
        private IPvPPrioritisedSoundPlayer _soundPlayer;
        private IPvPSingleSoundPlayer _uiSoundPlayer;
        private IPvPBuilding lastClickedBuilding;
        private IPvPUnit lastClickedUnit;
        private IPvPCruiser lastClickedCruiser;
        private IPvPTarget lastClickedBuildable;
        private int lastClickedType = -1; //-1 is for nothing, 0 is for buildings and 1 is for units

        private IPvPTarget _shownItem;
        // private IPvPExplanationPanel _explanationPanel;
        private IPvPHintDisplayer _hintDisplayer;

        // public void SetExplanationPanel(IExplanationPanel explanationPanelValue)
        // {
        //     _explanationPanel = explanationPanelValue;
        // }

        private IPvPTarget ShownItem
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

        private void _shownItem_Destroyed(object sender, PvPDestroyedEventArgs e)
        {
            HideItemDetails();
        }

        public virtual void HideItemDetails()
        {
            // Logging.LogMethod(Tags.UI_MANAGER);

            _detailsManager.HideDetails();


            // ServerRpc call
            _playerCruiser.PvP_UnhighlightSlotsServerRpc();
            // _playerCruiser.SlotHighlighter.UnhighlightSlots();
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

        public void SelectBuildingGroup(PvPBuildingCategory buildingCategory)
        {
            // Logging.LogMethod(Tags.UI_MANAGER);

            HideItemDetails();
            _buildMenu.ShowBuildingGroupMenu(buildingCategory);
        }

        public void SelectBuildingFromMenu(IPvPBuildableWrapper<IPvPBuilding> buildingWrapper)
        {
            // Logging.LogMethod(Tags.UI_MANAGER);

            _playerCruiser.SelectedBuildingPrefab = buildingWrapper;

            // ServerRpc call
            _playerCruiser.PvP_SelectedBuildingPrefabServerRpc(buildingWrapper.Buildable.Category, buildingWrapper.Buildable.PrefabName);


            _detailsManager.ShowDetails(buildingWrapper.Buildable);

            //      bool wasAnySlotHighlighted = _playerCruiser.SlotHighlighter.HighlightAvailableSlots(buildingWrapper.Buildable.SlotSpecification);

            // ServerRpc call
            _playerCruiser.PvP_HighlightAvailableSlotsServerRpc(buildingWrapper.Buildable.SlotSpecification.SlotType, buildingWrapper.Buildable.SlotSpecification.BuildingFunction, buildingWrapper.Buildable.SlotSpecification.PreferFromFront);

            // ILocTable _commonStrings = await LocTableFactory.Instance.LoadTutorialTableAsync();

            // if (_explanationPanel != null)
            // {
            //     if (_hintDisplayer == null)
            //     {
            //         _hintDisplayer = new PvPNonRepeatingHintDisplayer(new PvPHintDisplayer(_explanationPanel));
            //     }
            //     _hintDisplayer.ShowHint(_commonStrings.GetString("Steps/Touchdrag"));
            // }


    /*        if (!wasAnySlotHighlighted)
            {
                _soundPlayer.PlaySound(PvPPrioritisedSoundKeys.PvPEvents.PvPCruiser.NoBuildingSlotsLeft);
                _playerCruiser.SlotHighlighter.HighlightSlots(buildingWrapper.Buildable.SlotSpecification);
            }*/
        }

        public virtual void SelectBuilding(IPvPBuilding building)
        {
            // Logging.LogMethod(Tags.UI_MANAGER);

            _detailsManager.ShowDetails(building);
            _detailsManager.SelectBuilding(building);
            ShownItem = building;
            lastClickedBuilding = building;
            lastClickedBuildable = building;
            lastClickedType = 0;
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
            Debug.Log(cruiser == null ? "cruiser is null" : "cruiser is not null");
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
            _detailsManager.ShowDetails(unit);
            ShownItem = unit;
        }

        public void UnpeakBuildingDetails()
        {
            if (lastClickedBuildable != null)
            {
                if (lastClickedType == 0)
                {
                    _detailsManager.ShowDetails(lastClickedBuilding);
                }
                else if (lastClickedType == 1)
                {
                    _detailsManager.ShowDetails(lastClickedUnit);
                }
                else
                {
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
                    _detailsManager.ShowDetails(lastClickedBuilding);
                }
                else if (lastClickedType == 1)
                {
                    _detailsManager.ShowDetails(lastClickedUnit);
                }
                else
                {
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
