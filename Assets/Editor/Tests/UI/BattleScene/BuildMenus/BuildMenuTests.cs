using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI;
using BattleCruisers.UI.BattleScene.BuildMenus;
using BattleCruisers.UI.BattleScene.Buttons;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BattleCruisers.Tests.UI.BattleScene.BuildMenus
{
    public class BuildMenuTests
    {
        private IBuildMenu _buildMenu;
        private IPanel _selectorPanel;
        private IBuildingCategoriesMenu _buildingCategoriesMenu;
        private IBuildableMenus<BuildingCategory> _buildingMenus;
        private IBuildableMenus<UnitCategory> _unitMenus;
        private IBuildablesMenu _buildablesMenu1, _buildablesMenu2;
        private BuildingCategory _buildingCategory1, _buildingCategory2;

        [SetUp]
        public void TestSetup()
        {
            _selectorPanel = Substitute.For<IPanel>();
            _buildingCategoriesMenu = Substitute.For<IBuildingCategoriesMenu>();
            _buildingMenus = Substitute.For<IBuildableMenus<BuildingCategory>>();
            _unitMenus = Substitute.For<IBuildableMenus<UnitCategory>>();

            _buildMenu = new BuildMenu(_selectorPanel, _buildingCategoriesMenu, _buildingMenus, _unitMenus);

            _buildablesMenu1 = Substitute.For<IBuildablesMenu>();
            _buildingCategory1 = BuildingCategory.Ultra;
            _buildingMenus.GetBuildablesMenu(_buildingCategory1).Returns(_buildablesMenu1);

            _buildablesMenu2 = Substitute.For<IBuildablesMenu>();
            _buildingCategory2 = BuildingCategory.Offence;
            _buildingMenus.GetBuildablesMenu(_buildingCategory2).Returns(_buildablesMenu2);
        }

        [Test]
        public void ShowBuildingGroupMenu()
        {
            _buildMenu.ShowBuildingGroupMenu(_buildingCategory1);
            ReceivedShowMenu(_buildablesMenu1, activationParameter: null);
        }

        [Test]
        public void ShowUnitsMenu()
        {
            IFactory unitFactory = Substitute.For<IFactory>();
            unitFactory.UnitCategory.Returns(UnitCategory.Naval);
            _unitMenus.GetBuildablesMenu(unitFactory.UnitCategory).Returns(_buildablesMenu1);

            _buildMenu.ShowUnitsMenu(unitFactory);

            ReceivedShowMenu(_buildablesMenu1, unitFactory);
        }

        [Test]
        public void ShowMenu_WhileAlreadyShowingMenu()
        {
            // Show first menu
            _buildMenu.ShowBuildingGroupMenu(_buildingCategory1);
            ReceivedShowMenu(_buildablesMenu1, activationParameter: null);

            // Show second menu
            _buildMenu.ShowBuildingGroupMenu(_buildingCategory2);

            ReceivedHideCurrentlyShownMenu(_buildablesMenu1);
            ReceivedShowMenu(_buildablesMenu2, activationParameter: null);
        }

        [Test]
        public void HideCurrentlyShownMenu()
        {
            // Show menu
            _buildMenu.ShowBuildingGroupMenu(_buildingCategory1);
            ReceivedShowMenu(_buildablesMenu1, activationParameter: null);

            // Hide menu
            _buildMenu.HideCurrentlyShownMenu();
            ReceivedHideCurrentlyShownMenu(_buildablesMenu1);
        }

        [Test]
        public void GetCategoryButton()
        {
            IBuildingCategoryButton buildingCategoryButton = Substitute.For<IBuildingCategoryButton>();
            _buildingCategoriesMenu.GetCategoryButton(_buildingCategory1).Returns(buildingCategoryButton);

            IBuildingCategoryButton returnedButton = _buildMenu.GetCategoryButton(_buildingCategory1);

            Assert.AreSame(buildingCategoryButton, returnedButton);
        }

        [Test]
        public void GetBuildableButtons()
        {
            ReadOnlyCollection<IBuildableButton> buildableButtons = new ReadOnlyCollection<IBuildableButton>(new List<IBuildableButton>());
            _buildablesMenu1.BuildableButtons.Returns(buildableButtons);

            ReadOnlyCollection<IBuildableButton> returnedButtons = _buildMenu.GetBuildableButtons(_buildingCategory1);

            Assert.AreSame(buildableButtons, returnedButtons);
        }

        private void ReceivedShowMenu(IMenu shownMenu, object activationParameter)
        {
            _selectorPanel.Received().Show();
            shownMenu.Received().OnPresenting(activationParameter);
            shownMenu.Received().IsVisible = true;
        }

        private void ReceivedHideCurrentlyShownMenu(IMenu hiddenMenu)
        {
            hiddenMenu.Received().OnDismissing();
            hiddenMenu.Received().IsVisible = false;
            _selectorPanel.Received().Hide();
        }
    }
}