using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.Presentables;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Sorting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public class BuildMenuController : MonoBehaviour, IBuildMenu
	{
		private IUIManager _uiManager;
        private IUIFactory _uiFactory;
		private IList<IBuildingGroup> _buildingGroups;
        private IDictionary<BuildingCategory, BuildingsMenuController> _buildingGroupPanels;
        private IDictionary<UnitCategory, PresentableController> _unitGroupPanels;
        private PresentableController _currentPanel, _homePanel;
        private IDictionary<BuildingCategory, IBuildingCategoryButton> _categoryToCategoryButtons;
        private IDictionary<BuildingCategory, ReadOnlyCollection<IBuildableButton>> _categoryToBuildableButtons;

		public void Initialise(
			IUIManager uiManager,
            IUIFactory uiFactory,
            IList<IBuildingGroup> buildingGroups, 
            IDictionary<UnitCategory, IList<IBuildableWrapper<IUnit>>> units,
            IBuildableSorterFactory sorterFactory)
		{
            Helper.AssertIsNotNull(uiManager, uiFactory, buildingGroups, units, sorterFactory);

            _uiManager = uiManager;
            _uiFactory = uiFactory;
			_buildingGroups = buildingGroups;
            _categoryToCategoryButtons = new Dictionary<BuildingCategory, IBuildingCategoryButton>();
            _categoryToBuildableButtons = new Dictionary<BuildingCategory, ReadOnlyCollection<IBuildableButton>>();

			// Create main menu panel
			GameObject homePanelGameObject = _uiFactory.CreatePanel(isActive: true);
            _homePanel = homePanelGameObject.AddComponent<PresentableController>();
			_currentPanel = _homePanel;
			_homePanel.Initialise();

			// Create building category buttons
			HorizontalLayoutGroup homeButtonGroup = _homePanel.GetComponent<HorizontalLayoutGroup>();
            IBuildableSorter<IBuilding> buildingSorter = sorterFactory.CreateBuildingSorter();

            _buildingGroupPanels = new Dictionary<BuildingCategory, BuildingsMenuController>(_buildingGroups.Count);

			for (int i = 0; i < _buildingGroups.Count; ++i)
			{
				// Create category button
				IBuildingGroup buildingGroup = _buildingGroups[i];
                IBuildingCategoryButton categoryButton = _uiFactory.CreateBuildingCategoryButton(homeButtonGroup, buildingGroup);
                _categoryToCategoryButtons.Add(categoryButton.Category, categoryButton);

				// Create category panel
				GameObject panelGameObject = _uiFactory.CreatePanel(isActive: false);
				BuildingsMenuController buildingsMenu = panelGameObject.AddComponent<BuildingsMenuController>();
                // FELIX  Sort before passing to buildings menu :)
                buildingsMenu.Initialise(_uiFactory, buildingGroup.Buildings, buildingSorter);
				
                _buildingGroupPanels[buildingGroup.BuildingCategory] = buildingsMenu;
                _categoryToBuildableButtons.Add(buildingGroup.BuildingCategory, buildingsMenu.BuildableButtons);
			}

            // Create menu UI for units
            IBuildableSorter<IUnit> unitSorter = sorterFactory.CreateUnitSorter();
			_unitGroupPanels = new Dictionary<UnitCategory, PresentableController>();

			foreach (UnitCategory unitCategory in units.Keys)
			{
				GameObject panelGameObject = _uiFactory.CreatePanel(isActive: false);
				UnitsMenuController unitsMenu = panelGameObject.AddComponent<UnitsMenuController>();
                unitsMenu.Initialise(_uiManager, _uiFactory, units[unitCategory], unitSorter);
				_unitGroupPanels[unitCategory] = unitsMenu;
			}
		}

		public void HideBuildMenu()
		{
			ChangePanel(_homePanel);
			_currentPanel.gameObject.SetActive(false);
		}

		public void ShowBuildMenu()
		{
			_currentPanel.gameObject.SetActive(true);
		}

		public void ShowBuildingGroupsMenu()
		{
			ChangePanel(_homePanel);
		}

		public void ShowBuildingGroupMenu(BuildingCategory buildingCategory)
		{
			if (!_buildingGroupPanels.ContainsKey(buildingCategory))
			{
				throw new ArgumentException();
			}

			PresentableController panel = _buildingGroupPanels[buildingCategory];
			ChangePanel(panel);
		}

		public void ShowUnitsMenu(IFactory factory)
		{
			if (!_unitGroupPanels.ContainsKey(factory.UnitCategory))
			{
				throw new ArgumentException();
			}

			PresentableController panel = _unitGroupPanels[factory.UnitCategory];
			ChangePanel(panel, factory);
		}

        /// <summary>
        /// Always want to dismiss the current panel, even if we are switching to the same panel.
        /// This is because the activation parameter may have changed.  Ie, the user may be 
        /// switching from the aircraft units panel for one factory to another factory.
        /// </summary>
		private void ChangePanel(PresentableController panel, object activationParameter = null)
		{
			if (_currentPanel != null)
			{
				_currentPanel.OnDismissing();
				_currentPanel.gameObject.SetActive(false);
			}

			panel.OnPresenting(activationParameter);
			panel.gameObject.SetActive(true);
			_currentPanel = panel;
		}

        public IBuildingCategoryButton GetCategoryButton(BuildingCategory category)
        {
            Assert.IsTrue(_categoryToCategoryButtons.ContainsKey(category));
            return _categoryToCategoryButtons[category];
        }

        public ReadOnlyCollection<IBuildableButton> GetBuildableButtons(BuildingCategory category)
        {
            Assert.IsTrue(_categoryToBuildableButtons.ContainsKey(category));
            return _categoryToBuildableButtons[category];
        }
	}
}
