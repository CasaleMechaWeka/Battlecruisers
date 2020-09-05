using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.Panels;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BattleCruisers.UI.BattleScene.BuildMenus
{
    public class BuildMenu : IBuildMenu
	{
        private readonly IPanel _selectorPanel;
        private readonly IBuildingCategoriesMenu _buildingCategoriesMenu;
        private readonly IBuildableMenus<BuildingCategory> _buildingMenus;
        private readonly IBuildableMenus<UnitCategory> _unitMenus;
        private readonly ISingleSoundPlayer _uiSoundPlayer;
        private readonly IAudioClipWrapper _selectorOpeningSound;
        private IMenu _currentMenu, _lastShownMenu;

        public IReadOnlyCollection<IBuildableButton> BuildableButtons { get; }

        public BuildMenu(
			IPanel selectorPanel,
            IBuildingCategoriesMenu buildingCategoriesMenu,
            IBuildableMenus<BuildingCategory> buildingMenus,
            IBuildableMenus<UnitCategory> unitMenus,
            ISingleSoundPlayer uiSoundPlayer,
            IAudioClipWrapper selectorOpeningSound)
		{
            Helper.AssertIsNotNull(selectorPanel, buildingCategoriesMenu, buildingMenus, unitMenus, uiSoundPlayer, selectorOpeningSound);

            _selectorPanel = selectorPanel;
            _buildingCategoriesMenu = buildingCategoriesMenu;
            _buildingMenus = buildingMenus;
            _unitMenus = unitMenus;
            _uiSoundPlayer = uiSoundPlayer;
            _selectorOpeningSound = selectorOpeningSound;

            _currentMenu = null;
            _lastShownMenu = null;

            BuildableButtons = FindBuildableButtons();
        }

        private IReadOnlyCollection<IBuildableButton> FindBuildableButtons()
        {
            List<IBuildableButton> buttons = new List<IBuildableButton>();

            foreach (IBuildablesMenu menu in _buildingMenus.Menus)
            {
                buttons.AddRange(menu.BuildableButtons);
            }

            foreach (IBuildablesMenu menu in _unitMenus.Menus)
            {
                buttons.AddRange(menu.BuildableButtons);
            }

            return buttons;
        }

		public void ShowBuildingGroupMenu(BuildingCategory buildingCategory)
		{
            IBuildablesMenu menuToShow = _buildingMenus.GetBuildablesMenu(buildingCategory);
            
            if (ReferenceEquals(_currentMenu, menuToShow))
            {
                HideCurrentlyShownMenu();
            }
            else
            {
                ShowMenu(menuToShow);
            }
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
            if (_currentMenu == null)
            {
                _uiSoundPlayer.PlaySound(_selectorOpeningSound);
            }

            HideCurrentlyShownMenu();

            _selectorPanel.Show();

            if (_lastShownMenu != null)
            {
                _lastShownMenu.IsVisible = false;
                _lastShownMenu = null;
            }

            menu.OnPresenting(activationParameter);
            menu.IsVisible = true;
            _currentMenu = menu;
        }

        public void HideCurrentlyShownMenu()
        {
			if (_currentMenu != null)
			{
                _lastShownMenu = _currentMenu;
				_currentMenu.OnDismissing();
                _currentMenu = null;

                _selectorPanel.Hide();
			}
        }

        public IBuildingCategoryButton GetBuildingCategoryButton(BuildingCategory category)
        {
            return _buildingCategoriesMenu.GetCategoryButton(category);
        }

        public ReadOnlyCollection<IBuildableButton> GetBuildingButtons(BuildingCategory category)
        {
            IBuildablesMenu buildMenuForCategory = _buildingMenus.GetBuildablesMenu(category);
            return buildMenuForCategory.BuildableButtons;
        }
    }
}
