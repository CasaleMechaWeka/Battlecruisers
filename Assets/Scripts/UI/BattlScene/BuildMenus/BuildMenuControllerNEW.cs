using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Buttons.ClickHandlers;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Sorting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    // FELIX  Separate functionality from initialisation, to make testable :D
    public class BuildMenuControllerNEW : MonoBehaviour, IBuildMenuNEW
	{
        private IMenu _currentMenu;
        private BuildingCategoriesMenuNEW _buildingCategoriesMenu;
        private BuildingMenus _buildingMenus;
        private UnitMenus _unitMenus;

        public GameObject selectorPanel;

		public void Initialise(
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
                selectorPanel,
                uiManager,
                buildingGroups,
                units,
                sorterFactory,
                buttonVisibilityFilters,
                spriteProvider,
                playerCruiserFocusHelper,
                soundPlayer);

            // Building categories menu
            _buildingCategoriesMenu = GetComponentInChildren<BuildingCategoriesMenuNEW>();
            Assert.IsNotNull(_buildingCategoriesMenu);
            _buildingCategoriesMenu.Initialise(uiManager, buttonVisibilityFilters, buildingGroups);
            _currentMenu = _buildingCategoriesMenu;

            // FELIX  Implement :P
            //// Building menus
            //_buildingMenus = GetComponentInChildren<BuildingMenus>();
            //Assert.IsNotNull(_buildingMenus);
            //IBuildableSorter<IBuilding> buildingSorter = sorterFactory.CreateBuildingSorter();
            //IDictionary<BuildingCategory, IList<IBuildableWrapper<IBuilding>>> categoryToBuildings = ConvertGroupsToDictionary(buildingGroups);
            //IBuildingClickHandler buildingClickHandler = new BuildingClickHandler(playerCruiserFocusHelper, uiManager, soundPlayer);
            //_buildingMenus.Initialise(categoryToBuildings, uiManager, buttonVisibilityFilters, buildingSorter, spriteProvider, buildingClickHandler);

            //// Unit menus
            //IUnitClickHandler unitClickHandler = new UnitClickHandler(uiManager, soundPlayer);
            //_unitMenus = GetComponentInChildren<UnitMenus>();
            //Assert.IsNotNull(_unitMenus);
            //IBuildableSorter<IUnit> unitSorter = sorterFactory.CreateUnitSorter();
            //_unitMenus.Initialise(units, uiManager, buttonVisibilityFilters, unitSorter, unitClickHandler);

            selectorPanel.SetActive(false);
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

		public void ShowBuildingGroupMenu(BuildingCategory buildingCategory)
		{
            // FELIX  Initialise _buildngMenus :)
            //ShowMenu(_buildingMenus.GetBuildablesMenu(buildingCategory));

            // FELIX  Remove once above line works :)
            selectorPanel.SetActive(true);
        }

        public void ShowUnitsMenu(IFactory factory)
		{
            IBuildablesMenu unitMenu = _unitMenus.GetBuildablesMenu(factory.UnitCategory);
            ShowMenu(unitMenu, factory);
        }

        /// <summary>
        /// Always want to dismiss the current menu, even if we are switching to the same menu.
        /// This is because the activation parameter may have changed.  Ie, the user may be 
        /// switching from the aircraft units menu for one factory to another factory.
        /// </summary>
		private void ShowMenu(IMenu menu, object activationParameter = null)
		{
            HideCurrentlyShownMenu();

            selectorPanel.SetActive(true);

			menu.OnPresenting(activationParameter);
            menu.IsVisible = true;
			_currentMenu = menu;
        }

        public void HideCurrentlyShownMenu()
        {
			if (_currentMenu != null)
			{
				_currentMenu.OnDismissing();
                _currentMenu.IsVisible = false;

                selectorPanel.SetActive(false);
			}
        }

        public IBuildingCategoryButton GetCategoryButton(BuildingCategory category)
        {
            return _buildingCategoriesMenu.GetCategoryButton(category);
        }

        // FELIX  Implement :P
        public ReadOnlyCollection<IBuildableButton> GetBuildableButtons(BuildingCategory category)
        {
            //IBuildablesMenu buildMenuForCategory = _buildingMenus.GetBuildablesMenu(category);
            //return buildMenuForCategory.BuildableButtons;
            throw new NotImplementedException();
        }
    }
}
