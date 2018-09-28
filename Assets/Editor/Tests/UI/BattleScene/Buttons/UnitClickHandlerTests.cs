using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.BattleScene.Buttons.ClickHandlers;
using BattleCruisers.UI.BattleScene.Manager;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.BattleScene.Buttons
{
    public class UnitClickHandlerTests
    {
        private IUnitClickHandler _clickHandler;
        private IUIManager _uiManager;
        private IBuildableWrapper<IUnit> _unitWrapper;
        private IUnit _unit;
        private IFactory _factory;

        [SetUp]
        public void TestSetup()
        {
            _uiManager = Substitute.For<IUIManager>();
            _clickHandler = new UnitClickHandler(_uiManager);

            _unit = Substitute.For<IUnit>();
            _unitWrapper = Substitute.For<IBuildableWrapper<IUnit>>();
            _unitWrapper.Buildable.Returns(_unit);
            _factory = Substitute.For<IFactory>();
        }

        [Test]
        public void HandleUnitClick_SameUnit_IsPaused_ResumesUnit()
        {
            _factory.UnitWrapper.Returns(_unitWrapper);
            _factory.IsUnitPaused.Value.Returns(true);

            _clickHandler.HandleClick(_unitWrapper, _factory);

            _factory.Received().ResumeBuildingUnit();
        }

        [Test]
        public void HandleUnitClick_SameUnit_IsNotPaused_PausesUnit()
        {
            _factory.UnitWrapper.Returns(_unitWrapper);
            _factory.IsUnitPaused.Value.Returns(false);

            _clickHandler.HandleClick(_unitWrapper, _factory);

            _factory.Received().PauseBuildingUnit();
        }

        [Test]
        public void HandleUnitClick_NotSameUnit_StartsNewUnit()
        {
            _factory.UnitWrapper.Returns((IBuildableWrapper<IUnit>)null);

            _clickHandler.HandleClick(_unitWrapper, _factory);

            _factory.Received().StartBuildingUnit(_unitWrapper);
        }

        [Test]
        public void HandleUnitClick_ShowsUnitDetails()
        {
            _clickHandler.HandleClick(_unitWrapper, _factory);
            _uiManager.Received().ShowUnitDetails(_unit);
        }
    }
}