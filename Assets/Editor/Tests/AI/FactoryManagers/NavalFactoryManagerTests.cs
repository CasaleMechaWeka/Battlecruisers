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
        private IBuildableWrapper<IUnit> _unit, _unit2;
        private IFactory _navalFactory, _airFactory;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _friendlyCruiser = Substitute.For<ICruiserController>();
            _droneManager = Substitute.For<IDroneManager>();
            _droneManager.NumOfDrones.Returns(12);
            _unit = Substitute.For<IBuildableWrapper<IUnit>>();
            _unit2 = Substitute.For<IBuildableWrapper<IUnit>>();
            _unitChooser = Substitute.For<IUnitChooser>();
            _unitChooser.ChooseUnit(numOfDrones: -12).ReturnsForAnyArgs(_unit, _unit2);
            new NavalFactoryManager(_friendlyCruiser, _droneManager, _unitChooser);
            
			_navalFactory = Substitute.For<IFactory>();
			_navalFactory.UnitCategory.Returns(UnitCategory.Naval);
            _navalFactory.UnitWrapper = null;

            _airFactory = Substitute.For<IFactory>();
            _airFactory.UnitCategory.Returns(UnitCategory.Aircraft);
            _airFactory.UnitWrapper = null;
        }

        [Test]
        public void Constructor_ChoosesUnit()
        {
            _unitChooser.ClearReceivedCalls();
            new NavalFactoryManager(_friendlyCruiser, _droneManager, _unitChooser);
            _unitChooser.Received().ChooseUnit(_droneManager.NumOfDrones);
        }

        [Test]
        public void NavalFactoryBuilt_SetsUnit()
        {
            _friendlyCruiser.StartedConstruction += Raise.EventWith(_friendlyCruiser, new StartedConstructionEventArgs(_navalFactory));
            Assert.IsNull(_navalFactory.UnitWrapper);
            _navalFactory.CompletedBuildable += Raise.Event();
            Assert.AreSame(_unit, _navalFactory.UnitWrapper);
        }

        [Test]
        public void AirFactoryBuilt_DoesNotSetUnit()
        {
            _friendlyCruiser.StartedConstruction += Raise.EventWith(_friendlyCruiser, new StartedConstructionEventArgs(_airFactory));
            Assert.IsNull(_airFactory.UnitWrapper);
			_navalFactory.CompletedBuildable += Raise.Event();
			Assert.IsNull(_airFactory.UnitWrapper);
        }

        [Test]
        public void NumOfDronesChanged_NewNumOfDronesIsGreater_ChoosesUnit()
        {
            _unitChooser.ClearReceivedCalls();
            _droneManager.DroneNumChanged += Raise.EventWith(_droneManager, new DroneNumChangedEventArgs(oldNumOfDrones: 2, newNumOfDrones: 3));
            _unitChooser.Received().ChooseUnit(numOfDrones: 3);
		}

		[Test]
		public void NumOfDronesChanged_NewNumOfDronesIsNotGreater_DoesNotChoosesUnit()
		{
			_unitChooser.ClearReceivedCalls();
			_droneManager.DroneNumChanged += Raise.EventWith(_droneManager, new DroneNumChangedEventArgs(oldNumOfDrones: 4, newNumOfDrones: 3));
            _unitChooser.DidNotReceiveWithAnyArgs().ChooseUnit(numOfDrones: -12);
		}

		[Test]
		public void NavalFactory_UnitCompleted_SetsUnit()
		{
			_friendlyCruiser.StartedConstruction += Raise.EventWith(_friendlyCruiser, new StartedConstructionEventArgs(_navalFactory));
            _navalFactory.CompletedBuildable += Raise.Event();
			Assert.AreNotSame(_unit2, _navalFactory.UnitWrapper);

            // Trigger ChooseUnit()
			_droneManager.DroneNumChanged += Raise.EventWith(_droneManager, new DroneNumChangedEventArgs(oldNumOfDrones: 2, newNumOfDrones: 3));

            _navalFactory.CompletedBuildingUnit += Raise.EventWith(_navalFactory, new CompletedConstructionEventArgs(_unit.Buildable));
			Assert.AreSame(_unit2, _navalFactory.UnitWrapper);
		}
    }
}
