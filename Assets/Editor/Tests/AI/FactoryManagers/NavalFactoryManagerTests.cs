using BattleCruisers.AI.FactoryManagers;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Drones;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.AI.FactoryManagers
{
    public class NavalFactoryManagerTests
    {
        private ICruiserController _friendlyCruiser;
        private IDroneManager _droneManager;
        private IUnitChooser _unitChooser;
        private IBuildableWrapper<IUnit> _unit;
        private IFactory _navalFactory, _airFactory;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _friendlyCruiser = Substitute.For<ICruiserController>();
            _droneManager = Substitute.For<IDroneManager>();
            _unit = Substitute.For<IBuildableWrapper<IUnit>>();
            _unitChooser = Substitute.For<IUnitChooser>();
            _unitChooser.ChooseUnit(numOfDrones: -12).ReturnsForAnyArgs(_unit);
            new NavalFactoryManager(_friendlyCruiser, _droneManager, _unitChooser);
            
			_navalFactory = Substitute.For<IFactory>();
			_navalFactory.UnitCategory.Returns(UnitCategory.Naval);
            _airFactory = Substitute.For<IFactory>();
            _airFactory.UnitCategory.Returns(UnitCategory.Aircraft);
        }

        [Test]
        public void NavalFactoryBuilt_SetsUnit()
        {
            _friendlyCruiser.StartedConstruction += Raise.EventWith(_friendlyCruiser, new StartedConstructionEventArgs(_navalFactory));
            Assert.AreNotSame(_unit, _navalFactory.UnitWrapper);
            _navalFactory.CompletedBuildable += Raise.Event();
            Assert.AreSame(_unit, _navalFactory.UnitWrapper);
        }
    }
}
