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
using System.Linq;

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
        private IBuildableButton _button1, _button2;

        [SetUp]
        public void TestSetup()
        {
            _buildingMenus = Substitute.For<IBuildableMenus<BuildingCategory>>();

            // Menu 1
            _buildablesMenu1 = Substitute.For<IBuildablesMenu>();
            _buildingCategory1 = BuildingCategory.Ultra;
            _buildingMenus.GetBuildablesMenu(_buildingCategory1).Returns(_buildablesMenu1);
            _button1 = Substitute.For<IBuildableButton>();
            IReadOnlyCollection<IBuildableButton> menu1Buttons = new List<IBuildableButton>() { _button1 }.AsReadOnly();
            _buildablesMenu1.BuildableButtons.Returns(menu1Buttons);

            IReadOnlyCollection<IBuildablesMenu> buildingMenus = new List<IBuildablesMenu>() { _buildablesMenu1 }.AsReadOnly();
            _buildingMenus.Menus.Returns(buildingMenus);

            // Menu 2
            _buildablesMenu2 = Substitute.For<IBuildablesMenu>();
            _buildingCategory2 = BuildingCategory.Offence;
            _buildingMenus.GetBuildablesMenu(_buildingCategory2).Returns(_buildablesMenu2);
            _button2 = Substitute.For<IBuildableButton>();
            IReadOnlyCollection<IBuildableButton> menu2Buttons = new List<IBuildableButton>() { _button2 }.AsReadOnly();
            _buildablesMenu2.BuildableButtons.Returns(menu2Buttons);
            _unitMenus = Substitute.For<IBuildableMenus<UnitCategory>>();
            IReadOnlyCollection<IBuildablesMenu> unitMenus = new List<IBuildablesMenu>() { _buildablesMenu2 }.AsReadOnly();
            _unitMenus.Menus.Returns(unitMenus);

            _selectorPanel = Substitute.For<IPanel>();
            _buildingCategoriesMenu = Substitute.For<IBuildingCategoriesMenu>();

            _buildMenu = new BuildMenu(_selectorPanel, _buildingCategoriesMenu, _buildingMenus, _unitMenus);
        }

        [Test]
        public void BuildableButtons()
        {
            Assert.AreEqual(2, _buildMenu.BuildableButtons.Count);
            Assert.IsTrue(_buildMenu.BuildableButtons.Contains(_button1));
            Assert.IsTrue(_buildMenu.BuildableButtons.Contains(_button2));
        }

        [Test]
        public void ShowBuildingGroupMenu_DifferentMenu()
        {
            _buildMenu.ShowBuildingGroupMenu(_buildingCategory1);
            ReceivedShowMenu(_buildablesMenu1, activationParameter: null);
        }

        [Test]
        public void ShowBuildingGroupMenu_SameMenu()
        {
            _buildMenu.ShowBuildingGroupMenu(_buildingCategory1);
            _buildablesMenu1.ClearReceivedCalls();
            _selectorPanel.ClearReceivedCalls();

            _buildMenu.ShowBuildingGroupMenu(_buildingCategory1);

            ReceivedHideCurrentlyShownMenu(_buildablesMenu1);
            _buildablesMenu1.DidNotReceiveWithAnyArgs().OnPresenting(default);
            _selectorPanel.DidNotReceive().Show();
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
        public void GetBuildingCategoryButton()
        {
            IBuildingCategoryButton buildingCategoryButton = Substitute.For<IBuildingCategoryButton>();
            _buildingCategoriesMenu.GetCategoryButton(_buildingCategory1).Returns(buildingCategoryButton);

            IBuildingCategoryButton returnedButton = _buildMenu.GetBuildingCategoryButton(_buildingCategory1);

            Assert.AreSame(buildingCategoryButton, returnedButton);
        }

        [Test]
        public void GetBuildingButtons()
        {
            ReadOnlyCollection<IBuildableButton> buildableButtons = new ReadOnlyCollection<IBuildableButton>(new List<IBuildableButton>());
            _buildablesMenu1.BuildableButtons.Returns(buildableButtons);

            ReadOnlyCollection<IBuildableButton> returnedButtons = _buildMenu.GetBuildingButtons(_buildingCategory1);

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