using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.UI.BattleScene.Buttons;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.BattleScene.Buttons
{
    public class UnitClickHandlerTests
    {
        private IUnitClickHandler _clickHandler;
        private IBuildableWrapper<IUnit> _unit;
        private IFactory _factory;

        [SetUp]
        public void TestSetup()
        {
            _clickHandler = new UnitClickHandler();

            _unit = Substitute.For<IBuildableWrapper<IUnit>>();
            _factory = Substitute.For<IFactory>();
        }

        [Test]
        public void HandleUnitClick_SameUnit_IsPaused_ResumesUnit()
        {
            _factory.UnitWrapper.Returns(_unit);
            _factory.IsUnitPaused.Value.Returns(true);

            _clickHandler.HandleUnitClick(_unit, _factory);

            _factory.Received().ResumeBuildingUnit();
        }

        [Test]
        public void HandleUnitClick_SameUnit_IsNotPaused_PausesUnit()
        {
            _factory.UnitWrapper.Returns(_unit);
            _factory.IsUnitPaused.Value.Returns(false);

            _clickHandler.HandleUnitClick(_unit, _factory);

            _factory.Received().PauseBuildingUnit();
        }

        [Test]
        public void HandleUnitClick_NotSameUnit_StartsNewUnit()
        {
            _factory.UnitWrapper.Returns((IBuildableWrapper<IUnit>)null);

            _clickHandler.HandleUnitClick(_unit, _factory);

            _factory.Received().StartBuildingUnit(_unit);
        }
    }
}