using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.BattleScene.Buttons.ClickHandlers;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Sorting;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public class BuildMenuInitialiser : MonoBehaviour
	{
		public IBuildMenuNEW Initialise(
			IUIManager uiManager,
            IList<IBuildingGroup> buildingGroups, 
            IDictionary<UnitCategory, IList<IBuildableWrapper<IUnit>>> units,
            IBuildableSorterFactory sorterFactory,
            IButtonVisibilityFilters buttonVisibilityFilters,
            ISpriteProvider spriteProvider,
            IPlayerCruiserFocusHelper playerCruiserFocusHelper,
            IPrioritisedSoundPlayer soundPlayer)
		{
            Helper.AssertIsNotNull(
                uiManager,
                buildingGroups,
                units,
                sorterFactory,
                buttonVisibilityFilters,
                spriteProvider,
                playerCruiserFocusHelper,
                soundPlayer);

            // Selector panel
            SelectorPanelController selectorPanel = GetComponentInChildren<SelectorPanelController>();
            Assert.IsNotNull(selectorPanel);
            selectorPanel.Initialise(uiManager, buttonVisibilityFilters.DismissButtonVisibilityFilter);
            selectorPanel.Hide();

            // Building categories menu
            BuildingCategoriesMenuNEW buildingCategoriesMenu = GetComponentInChildren<BuildingCategoriesMenuNEW>();
            Assert.IsNotNull(buildingCategoriesMenu);
            buildingCategoriesMenu.Initialise(uiManager, buttonVisibilityFilters, buildingGroups);

            // Building menus
            BuildingMenus buildingMenus = GetComponentInChildren<BuildingMenus>();
            Assert.IsNotNull(buildingMenus);
            IBuildableSorter<IBuilding> buildingSorter = sorterFactory.CreateBuildingSorter();
            IDictionary<BuildingCategory, IList<IBuildableWrapper<IBuilding>>> categoryToBuildings = ConvertGroupsToDictionary(buildingGroups);
            IBuildingClickHandler buildingClickHandler = new BuildingClickHandler(playerCruiserFocusHelper, uiManager, soundPlayer);
            buildingMenus.Initialise(categoryToBuildings, uiManager, buttonVisibilityFilters, buildingSorter, spriteProvider, buildingClickHandler);

            // Unit menus
            IUnitClickHandler unitClickHandler = new UnitClickHandler(uiManager, soundPlayer);
            UnitMenus unitMenus = GetComponentInChildren<UnitMenus>();
            Assert.IsNotNull(unitMenus);
            IBuildableSorter<IUnit> unitSorter = sorterFactory.CreateUnitSorter();
            unitMenus.Initialise(units, uiManager, buttonVisibilityFilters, unitSorter, unitClickHandler);

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
