using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.ClickHandlers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers.Sprites;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Sorting;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.BuildMenus
{
    public class PvPBuildMenuInitialiser : MonoBehaviour
    {
        public AudioClip buildingButtonSelectedSound, selectorOpeningSound;

        public IPvPBuildMenu Initialise(
            IPvPUIManager uiManager,
            IList<IPvPBuildingGroup> buildingGroups,
            IDictionary<PvPUnitCategory, IList<IPvPBuildableWrapper<IPvPUnit>>> units,
            IPvPBuildableSorterFactory sorterFactory,
            IPvPButtonVisibilityFilters buttonVisibilityFilters,
            IPvPSpriteProvider spriteProvider,
            IPvPPlayerCruiserFocusHelper playerCruiserFocusHelper,
            IPvPPrioritisedSoundPlayer eventSoundPlayer,
            IPvPSingleSoundPlayer uiSoundPlayer,
            IPvPPopulationLimitMonitor populationLimitMonitor)
        {
            PvPHelper.AssertIsNotNull(
                uiManager,
                buildingGroups,
                units,
                sorterFactory,
                buttonVisibilityFilters,
                spriteProvider,
                playerCruiserFocusHelper,
                eventSoundPlayer,
                uiSoundPlayer,
                populationLimitMonitor);
            PvPHelper.AssertIsNotNull(buildingButtonSelectedSound, selectorOpeningSound);

            // Selector panel
            PvPSelectorPanelController selectorPanel = GetComponentInChildren<PvPSelectorPanelController>();
            Assert.IsNotNull(selectorPanel);
            selectorPanel.Initialise(uiManager, buttonVisibilityFilters, uiSoundPlayer);

            // Building categories menu
            PvPBuildingCategoriesMenu buildingCategoriesMenu = GetComponentInChildren<PvPBuildingCategoriesMenu>();
            Assert.IsNotNull(buildingCategoriesMenu);
            buildingCategoriesMenu.Initialise(uiSoundPlayer, uiManager, buttonVisibilityFilters, buildingGroups);

            // Building menus
            PvPBuildingMenus buildingMenus = GetComponentInChildren<PvPBuildingMenus>();
            Assert.IsNotNull(buildingMenus);
            IPvPBuildableSorter<IPvPBuilding> buildingSorter = sorterFactory.CreateBuildingSorter();
            IDictionary<PvPBuildingCategory, IList<IPvPBuildableWrapper<IPvPBuilding>>> categoryToBuildings = ConvertGroupsToDictionary(buildingGroups);
            IPvPBuildingClickHandler buildingClickHandler
                = new PvPBuildingClickHandler(
                    uiManager,
                    eventSoundPlayer,
                    uiSoundPlayer,
                    playerCruiserFocusHelper,
                    new PvPAudioClipWrapper(buildingButtonSelectedSound));
            buildingMenus.Initialise(categoryToBuildings, uiManager, buttonVisibilityFilters, buildingSorter, spriteProvider, uiSoundPlayer, buildingClickHandler);

            // Unit menus
            IPvPUnitClickHandler unitClickHandler
                = new PvPUnitClickHandler(
                    uiManager,
                    eventSoundPlayer,
                    uiSoundPlayer,
                    new PvPPopulationLimitReachedDecider(populationLimitMonitor));
            PvPUnitMenus unitMenus = GetComponentInChildren<PvPUnitMenus>();
            Assert.IsNotNull(unitMenus);
            IPvPBuildableSorter<IPvPUnit> unitSorter = sorterFactory.CreateUnitSorter();
            unitMenus.Initialise(units, uiManager, buttonVisibilityFilters, unitSorter, uiSoundPlayer, unitClickHandler);

            return
                new PvPBuildMenu(
                    selectorPanel,
                    buildingCategoriesMenu,
                    buildingMenus,
                    unitMenus,
                    uiSoundPlayer,
                    new PvPAudioClipWrapper(selectorOpeningSound));
        }

        private IDictionary<PvPBuildingCategory, IList<IPvPBuildableWrapper<IPvPBuilding>>> ConvertGroupsToDictionary(IList<IPvPBuildingGroup> buildingGroups)
        {
            IDictionary<PvPBuildingCategory, IList<IPvPBuildableWrapper<IPvPBuilding>>> categoryToBuildings = new Dictionary<PvPBuildingCategory, IList<IPvPBuildableWrapper<IPvPBuilding>>>();

            foreach (IPvPBuildingGroup group in buildingGroups)
            {
                categoryToBuildings.Add(group.BuildingCategory, group.Buildings);
            }

            return categoryToBuildings;
        }
    }
}
