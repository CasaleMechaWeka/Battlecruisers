using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.Presentables;
using BattleCruisers.UI.Filters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Sorting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public class NEWBuildMenuController : MonoBehaviour, IBuildMenu
	{
        private IMenu _currentMenu;
        private BuildingCategoriesMenu _buildingCategoriesMenu;
        private BuildingMenus _buildingMenus;
        private UnitMenus _unitMenus;

        // FELIX  Remnove unused :)
		private IUIManager _uiManager;
        private IUIFactory _uiFactory;
		private IList<IBuildingGroup> _buildingGroups;
        private IDictionary<BuildingCategory, BuildingsMenuController> _buildingGroupPanels;
        private IDictionary<UnitCategory, PresentableController> _unitGroupPanels;
        private IDictionary<BuildingCategory, IBuildingCategoryButton> _categoryToCategoryButtons;
        private IDictionary<BuildingCategory, ReadOnlyCollection<IBuildableButton>> _categoryToBuildableButtons;

        // FELIX  Gropu some parameters?  Perhaps filters?
		public void Initialise(
			IUIManager uiManager,
            IUIFactory uiFactory,
            IList<IBuildingGroup> buildingGroups, 
            IDictionary<UnitCategory, IList<IBuildableWrapper<IUnit>>> units,
            IBuildableSorterFactory sorterFactory,
            IBroadcastingFilter<BuildingCategory> shouldCategoryButtonsBeEnabledFilter,
            IBroadcastingFilter<IBuildable> shouldBuildingButtonsBeEnabledFilter,
            ISpriteProvider spriteProvider,
            IBroadcastingFilter<IBuildable> shouldUnitButtonsBeEnabledFilter,
            IUnitClickHandler unitClickHandler)
		{
            Helper.AssertIsNotNull(
                uiManager,
                uiFactory,
                buildingGroups,
                units,
                sorterFactory,
                shouldCategoryButtonsBeEnabledFilter,
                shouldBuildingButtonsBeEnabledFilter,
                spriteProvider,
                shouldUnitButtonsBeEnabledFilter,
                unitClickHandler);

            _uiManager = uiManager;
            _uiFactory = uiFactory;
			_buildingGroups = buildingGroups;
            _categoryToCategoryButtons = new Dictionary<BuildingCategory, IBuildingCategoryButton>();
            _categoryToBuildableButtons = new Dictionary<BuildingCategory, ReadOnlyCollection<IBuildableButton>>();

            // Building categories menu
            _buildingCategoriesMenu = GetComponentInChildren<BuildingCategoriesMenu>();
            Assert.IsNotNull(_buildingCategoriesMenu);
            _buildingCategoriesMenu.Initialise(buildingGroups, uiManager, shouldCategoryButtonsBeEnabledFilter);
            _currentMenu = _buildingCategoriesMenu;

            // Building menus
            _buildingMenus = GetComponentInChildren<BuildingMenus>();
            Assert.IsNotNull(_buildingMenus);
            IBuildableSorter<IBuilding> buildingSorter = sorterFactory.CreateBuildingSorter();
            IDictionary<BuildingCategory, IList<IBuildableWrapper<IBuilding>>> categoryToBuildings = ConvertGroupsToDictionary(buildingGroups);
            _buildingMenus.Initialise(categoryToBuildings, uiManager, shouldBuildingButtonsBeEnabledFilter, buildingSorter);

            // Unit menus
            _unitMenus = GetComponentInChildren<UnitMenus>();
            Assert.IsNotNull(_unitMenus);
            IBuildableSorter<IUnit> unitSorter = sorterFactory.CreateUnitSorter();
            _unitMenus.Initialise(units, uiManager, shouldUnitButtonsBeEnabledFilter, unitSorter);
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
			ChangePanel(_buildingMenus.GetBuildablesPanel(buildingCategory));
		}

		public void ShowUnitsMenu(IFactory factory)
		{
            IBuildablesMenu unitMenu = _unitMenus.GetBuildablesPanel(factory.UnitCategory);
			ChangePanel(unitMenu, factory);
		}

        // FELIX  Rename panel to menu :)
        /// <summary>
        /// Always want to dismiss the current panel, even if we are switching to the same panel.
        /// This is because the activation parameter may have changed.  Ie, the user may be 
        /// switching from the aircraft units panel for one factory to another factory.
        /// </summary>
		private void ChangePanel(IMenu panel, object activationParameter = null)
		{
			if (_currentMenu != null)
			{
				_currentMenu.OnDismissing();
                _currentMenu.IsVisible = false;
			}

			panel.OnPresenting(activationParameter);
            panel.IsVisible = true;
			_currentMenu = panel;
		}

        public IBuildingCategoryButton GetCategoryButton(BuildingCategory category)
        {
            return _buildingCategoriesMenu.GetCategoryButton(category);
        }

        public ReadOnlyCollection<IBuildableButton> GetBuildableButtons(BuildingCategory category)
        {
            IBuildablesMenu buildMenuForCategory = _buildingMenus.GetBuildablesPanel(category);
            return buildMenuForCategory.BuildableButtons;
        }
	}
}
