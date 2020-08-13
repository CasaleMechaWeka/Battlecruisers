using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.UI.BattleScene.Buttons.ClickHandlers;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Utils.Sorting;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public class BuildMenuInitialiser : MonoBehaviour
	{
        public AudioClip buildingButtonSelectedSound;

        public IBuildMenu Initialise(
			IUIManager uiManager,
            IList<IBuildingGroup> buildingGroups, 
            IDictionary<UnitCategory, IList<IBuildableWrapper<IUnit>>> units,
            IBuildableSorterFactory sorterFactory,
            IButtonVisibilityFilters buttonVisibilityFilters,
            ISpriteProvider spriteProvider,
            IPlayerCruiserFocusHelper playerCruiserFocusHelper,
            IPrioritisedSoundPlayer eventSoundPlayer,
            ISingleSoundPlayer uiSoundPlayer,
            IPopulationLimitMonitor populationLimitMonitor)
		{
            Helper.AssertIsNotNull(
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
            Assert.IsNotNull(buildingButtonSelectedSound);

            // Selector panel
            SelectorPanelController selectorPanel = GetComponentInChildren<SelectorPanelController>();
            Assert.IsNotNull(selectorPanel);
            selectorPanel.Initialise(uiManager, buttonVisibilityFilters, uiSoundPlayer);
            selectorPanel.Hide();

            // Building categories menu
            BuildingCategoriesMenu buildingCategoriesMenu = GetComponentInChildren<BuildingCategoriesMenu>();
            Assert.IsNotNull(buildingCategoriesMenu);
            buildingCategoriesMenu.Initialise(uiSoundPlayer, uiManager, buttonVisibilityFilters, buildingGroups);

            // Building menus
            BuildingMenus buildingMenus = GetComponentInChildren<BuildingMenus>();
            Assert.IsNotNull(buildingMenus);
            IBuildableSorter<IBuilding> buildingSorter = sorterFactory.CreateBuildingSorter();
            IDictionary<BuildingCategory, IList<IBuildableWrapper<IBuilding>>> categoryToBuildings = ConvertGroupsToDictionary(buildingGroups);
            IBuildingClickHandler buildingClickHandler 
                = new BuildingClickHandler(
                    uiManager, 
                    eventSoundPlayer,
                    uiSoundPlayer,
                    playerCruiserFocusHelper,
                    new AudioClipWrapper(buildingButtonSelectedSound));
            buildingMenus.Initialise(categoryToBuildings, uiManager, buttonVisibilityFilters, buildingSorter, spriteProvider, uiSoundPlayer, buildingClickHandler);

            // Unit menus
            IUnitClickHandler unitClickHandler
                = new UnitClickHandler(
                    uiManager,
                    eventSoundPlayer,
                    uiSoundPlayer,
                    new PopulationLimitReachedDecider(populationLimitMonitor));
            UnitMenus unitMenus = GetComponentInChildren<UnitMenus>();
            Assert.IsNotNull(unitMenus);
            IBuildableSorter<IUnit> unitSorter = sorterFactory.CreateUnitSorter();
            unitMenus.Initialise(units, uiManager, buttonVisibilityFilters, unitSorter, uiSoundPlayer, unitClickHandler);

            return
                new BuildMenu(
                    selectorPanel,
                    buildingCategoriesMenu,
                    buildingMenus,
                    unitMenus);
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
