using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.BattleScene.Buttons;
using BattleCruisers.UI.Panels;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.BuildMenus
{
    public class PvPBuildMenu : IPvPBuildMenu
    {
        private readonly IBuildingCategoriesMenu _buildingCategoriesMenu;
        private readonly IPvPBuildableMenus<BuildingCategory> _buildingMenus;
        private readonly IPvPBuildableMenus<UnitCategory> _unitMenus;
        private readonly SingleSoundPlayer _uiSoundPlayer;
        private readonly AudioClipWrapper _selectorOpeningSound;
        private IMenu _currentMenu, _lastShownMenu;

        public ISlidingPanel SelectorPanel { get; }
        public IReadOnlyCollection<IPvPBuildableButton> BuildableButtons { get; }

        public PvPBuildMenu(
            ISlidingPanel selectorPanel,
            IBuildingCategoriesMenu buildingCategoriesMenu,
            IPvPBuildableMenus<BuildingCategory> buildingMenus,
            IPvPBuildableMenus<UnitCategory> unitMenus,
            SingleSoundPlayer uiSoundPlayer,
            AudioClipWrapper selectorOpeningSound)
        {
            PvPHelper.AssertIsNotNull(selectorPanel, buildingCategoriesMenu, buildingMenus, unitMenus, uiSoundPlayer, selectorOpeningSound);

            SelectorPanel = selectorPanel;
            _buildingCategoriesMenu = buildingCategoriesMenu;
            _buildingMenus = buildingMenus;
            _unitMenus = unitMenus;
            _uiSoundPlayer = uiSoundPlayer;
            _selectorOpeningSound = selectorOpeningSound;

            _currentMenu = null;
            _lastShownMenu = null;

            BuildableButtons = FindBuildableButtons();
        }

        private IReadOnlyCollection<IPvPBuildableButton> FindBuildableButtons()
        {
            List<IPvPBuildableButton> buttons = new List<IPvPBuildableButton>();

            foreach (IPvPBuildablesMenu menu in _buildingMenus.Menus)
            {
                buttons.AddRange(menu.BuildableButtons);
            }

            foreach (IPvPBuildablesMenu menu in _unitMenus.Menus)
            {
                buttons.AddRange(menu.BuildableButtons);
            }

            return buttons;
        }

        public void ShowBuildingGroupMenu(BuildingCategory buildingCategory)
        {
            IPvPBuildablesMenu menuToShow = _buildingMenus.GetBuildablesMenu(buildingCategory);

            if (ReferenceEquals(_currentMenu, menuToShow))
            {
                HideCurrentlyShownMenu();
            }
            else
            {
                ShowMenu(menuToShow);
            }
        }

        public void ShowUnitsMenu(IPvPFactory factory)
        {
            IPvPBuildablesMenu unitMenu = _unitMenus.GetBuildablesMenu(factory.UnitCategory);
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

            SelectorPanel.Show();

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

                SelectorPanel.Hide();
            }
        }

        public IBuildingCategoryButton GetBuildingCategoryButton(BuildingCategory category)
        {
            return _buildingCategoriesMenu.GetCategoryButton(category);
        }

        public ReadOnlyCollection<IPvPBuildableButton> GetBuildingButtons(BuildingCategory category)
        {
            IPvPBuildablesMenu buildMenuForCategory = _buildingMenus.GetBuildablesMenu(category);
            return buildMenuForCategory.BuildableButtons;
        }
    }
}
