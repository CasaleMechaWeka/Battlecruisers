using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.ClickHandlers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Sorting;
using BattleCruisers.Utils.Fetchers.Sprites;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.BuildMenus
{
    public class PvPBuildMenuInitialiser : MonoBehaviour
    {
        public AudioClip buildingButtonSelectedSound, selectorOpeningSound;

        public IPvPBuildMenu Initialise(
            PvPCruiser playerCruiser,
            IPvPUIManager uiManager,
            IList<IPvPBuildingGroup> buildingGroups,
            IDictionary<PvPUnitCategory, IList<IPvPBuildableWrapper<IPvPUnit>>> units,
            IPvPBuildableSorterFactory sorterFactory,
            IPvPButtonVisibilityFilters buttonVisibilityFilters,
            ISpriteProvider spriteProvider,
            IPvPPlayerCruiserFocusHelper playerCruiserFocusHelper,
            IPvPPrioritisedSoundPlayer eventSoundPlayer,
            IPvPSingleSoundPlayer uiSoundPlayer,
            IPvPPopulationLimitMonitor populationLimitMonitor,
            bool flipClickAndDragIcon)
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
                populationLimitMonitor,
                flipClickAndDragIcon);
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
                    new AudioClipWrapper(buildingButtonSelectedSound));
            buildingMenus.Initialise(categoryToBuildings, uiManager, buttonVisibilityFilters, buildingSorter, spriteProvider, uiSoundPlayer, buildingClickHandler, flipClickAndDragIcon);

            // Unit menus
            IPvPUnitClickHandler unitClickHandler
                = new PvPUnitClickHandler(
                    playerCruiser,
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
                    new AudioClipWrapper(selectorOpeningSound));
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
