using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Buttons.ClickHandlers;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Sorting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public class BuildMenuController : MonoBehaviour, IBuildMenu
	{
        private IMenu _currentMenu;
        private BuildingCategoriesMenu _buildingCategoriesMenu;
        private BuildingMenus _buildingMenus;
        private UnitMenus _unitMenus;

		public void Initialise(
			IUIManager uiManager,
            IList<IBuildingGroup> buildingGroups, 
            IDictionary<UnitCategory, IList<IBuildableWrapper<IUnit>>> units,
            IBuildableSorterFactory sorterFactory,
            IButtonVisibilityFilters buttonVisibilityFilters,
            ISpriteProvider spriteProvider,
            IPlayerCruiserFocusHelper playerCruiserFocusHelper)
		{
            Helper.AssertIsNotNull(
                uiManager,
                buildingGroups,
                units,
                sorterFactory,
                buttonVisibilityFilters,
                spriteProvider,
                playerCruiserFocusHelper);

            // Building categories menu
            _buildingCategoriesMenu = GetComponentInChildren<BuildingCategoriesMenu>();
            Assert.IsNotNull(_buildingCategoriesMenu);
            _buildingCategoriesMenu.Initialise(uiManager, buttonVisibilityFilters, buildingGroups);
            _currentMenu = _buildingCategoriesMenu;

            // Building menus
            _buildingMenus = GetComponentInChildren<BuildingMenus>();
            Assert.IsNotNull(_buildingMenus);
            IBuildableSorter<IBuilding> buildingSorter = sorterFactory.CreateBuildingSorter();
            IDictionary<BuildingCategory, IList<IBuildableWrapper<IBuilding>>> categoryToBuildings = ConvertGroupsToDictionary(buildingGroups);
            IBuildingClickHandler buildingClickHandler = new BuildingClickHandler(playerCruiserFocusHelper, uiManager);
            _buildingMenus.Initialise(categoryToBuildings, uiManager, buttonVisibilityFilters, buildingSorter, spriteProvider, buildingClickHandler);

            // Unit menus
            IUnitClickHandler unitClickHandler = new UnitClickHandler(uiManager);
            _unitMenus = GetComponentInChildren<UnitMenus>();
            Assert.IsNotNull(_unitMenus);
            IBuildableSorter<IUnit> unitSorter = sorterFactory.CreateUnitSorter();
            _unitMenus.Initialise(units, uiManager, buttonVisibilityFilters, unitSorter, unitClickHandler);
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

        public void HideBuildMenu()
		{
			ChangePanel(_buildingCategoriesMenu);
            _currentMenu.IsVisible = false;
		}

		public void ShowBuildMenu()
		{
            _currentMenu.IsVisible = true;
		}

		public void ShowBuildingGroupsMenu()
		{
			ChangePanel(_buildingCategoriesMenu);
		}

		public void ShowBuildingGroupMenu(BuildingCategory buildingCategory)
		{
			ChangePanel(_buildingMenus.GetBuildablesMenu(buildingCategory));
		}

		public void ShowUnitsMenu(IFactory factory)
		{
            IBuildablesMenu unitMenu = _unitMenus.GetBuildablesMenu(factory.UnitCategory);
			ChangePanel(unitMenu, factory);
		}

        /// <summary>
        /// Always want to dismiss the current menu, even if we are switching to the same menu.
        /// This is because the activation parameter may have changed.  Ie, the user may be 
        /// switching from the aircraft units menu for one factory to another factory.
        /// </summary>
		private void ChangePanel(IMenu menu, object activationParameter = null)
		{
			if (_currentMenu != null)
			{
				_currentMenu.OnDismissing();
                _currentMenu.IsVisible = false;
			}

			menu.OnPresenting(activationParameter);
            menu.IsVisible = true;
			_currentMenu = menu;
		}

        public IBuildingCategoryButton GetCategoryButton(BuildingCategory category)
        {
            return _buildingCategoriesMenu.GetCategoryButton(category);
        }

        public ReadOnlyCollection<IBuildableButton> GetBuildableButtons(BuildingCategory category)
        {
            IBuildablesMenu buildMenuForCategory = _buildingMenus.GetBuildablesMenu(category);
            return buildMenuForCategory.BuildableButtons;
        }
	}
}
