using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.UI.BattleScene.Buttons.ClickHandlers;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Utils.Sorting;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public class BuildMenuInitialiser : MonoBehaviour
    {
        public AudioClip buildingButtonSelectedSound, selectorOpeningSound;

        public BuildMenu Initialise(
            UIManager uiManager,
            IList<IBuildingGroup> buildingGroups,
            IDictionary<UnitCategory, IList<IBuildableWrapper<IUnit>>> units,
            ButtonVisibilityFilters buttonVisibilityFilters,
            IPlayerCruiserFocusHelper playerCruiserFocusHelper,
            IPrioritisedSoundPlayer eventSoundPlayer,
            SingleSoundPlayer uiSoundPlayer,
            IPopulationLimitMonitor populationLimitMonitor)
        {
            Helper.AssertIsNotNull(
                uiManager,
                buildingGroups,
                units,
                buttonVisibilityFilters,
                playerCruiserFocusHelper,
                eventSoundPlayer,
                uiSoundPlayer,
                populationLimitMonitor);
            Helper.AssertIsNotNull(buildingButtonSelectedSound, selectorOpeningSound);

            // Selector panel
            SelectorPanelController selectorPanel = GetComponentInChildren<SelectorPanelController>();
            Assert.IsNotNull(selectorPanel);
            selectorPanel.Initialise(uiManager, buttonVisibilityFilters, uiSoundPlayer);

            // Building categories menu
            BuildingCategoriesMenu buildingCategoriesMenu = GetComponentInChildren<BuildingCategoriesMenu>();
            Assert.IsNotNull(buildingCategoriesMenu);
            buildingCategoriesMenu.Initialise(uiSoundPlayer, uiManager, buttonVisibilityFilters, buildingGroups);

            // Building menus
            BuildingMenus buildingMenus = GetComponentInChildren<BuildingMenus>();
            Assert.IsNotNull(buildingMenus);
            IBuildableSorter<IBuilding> buildingSorter = new BuildingUnlockedLevelSorter();
            IDictionary<BuildingCategory, IList<IBuildableWrapper<IBuilding>>> categoryToBuildings = ConvertGroupsToDictionary(buildingGroups);
            BuildingClickHandler buildingClickHandler
                = new BuildingClickHandler(
                    uiManager,
                    eventSoundPlayer,
                    uiSoundPlayer,
                    playerCruiserFocusHelper,
                    new AudioClipWrapper(buildingButtonSelectedSound));
            buildingMenus.Initialise(categoryToBuildings, uiManager, buttonVisibilityFilters, buildingSorter, uiSoundPlayer, buildingClickHandler);

            // Unit menus
            UnitClickHandler unitClickHandler
                = new UnitClickHandler(
                    uiManager,
                    eventSoundPlayer,
                    uiSoundPlayer,
                    new PopulationLimitReachedDecider(populationLimitMonitor));
            UnitMenus unitMenus = GetComponentInChildren<UnitMenus>();
            Assert.IsNotNull(unitMenus);
            IBuildableSorter<IUnit> unitSorter = new UnitUnlockedLevelSorter();
            unitMenus.Initialise(units, uiManager, buttonVisibilityFilters, unitSorter, uiSoundPlayer, unitClickHandler);

            return
                new BuildMenu(
                    selectorPanel,
                    buildingCategoriesMenu,
                    buildingMenus,
                    unitMenus,
                    uiSoundPlayer,
                    new AudioClipWrapper(selectorOpeningSound));
        }

        private IDictionary<BuildingCategory, IList<IBuildableWrapper<IBuilding>>> ConvertGroupsToDictionary(IList<IBuildingGroup> buildingGroups)
        {
            IDictionary<BuildingCategory, IList<IBuildableWrapper<IBuilding>>> categoryToBuildings = new Dictionary<BuildingCategory, IList<IBuildableWrapper<IBuilding>>>();

            foreach (IBuildingGroup group in buildingGroups)
            {
                categoryToBuildings.Add(group.BuildingCategory, group.Buildings);
            }

            return categoryToBuildings;
        }
    }
}
