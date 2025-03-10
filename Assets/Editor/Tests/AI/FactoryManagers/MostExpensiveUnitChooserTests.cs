using BattleCruisers.AI.FactoryManagers;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Drones;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.AI.FactoryManagers
{
    public class MostExpensiveUnitChooserTests
	{
        private IUnitChooser _unitChooser;
		private IDroneManager _droneManager;
		private IList<IBuildableWrapper<IUnit>> _units;
        private IBuildableWrapper<IUnit> _unit2Drones, _unit4Drones, _unit6Drones;

		[SetUp]
		public void SetuUp()
		{
            _unit2Drones = CreateUnit(numOfDrones: 2);
            _unit4Drones = CreateUnit(numOfDrones: 4);
            _unit6Drones = CreateUnit(numOfDrones: 6);

            _units = new List<IBuildableWrapper<IUnit>>()
            {
                _unit2Drones,
                _unit4Drones,
                _unit6Drones
            };

			_droneManager = Substitute.For<IDroneManager>();
			_droneManager.NumOfDrones.Returns(12);

            _unitChooser = new MostExpensiveUnitChooser(_units, _droneManager, new AffordableUnitFilter());
		}

        private IBuildableWrapper<IUnit> CreateUnit(int numOfDrones)
        {
            IBuildableWrapper<IUnit> unit = Substitute.For<IBuildableWrapper<IUnit>>();
            unit.Buildable.Returns(Substitute.For<IUnit>());
            unit.Buildable.NumOfDronesRequired.Returns(numOfDrones);
            return unit;
        }

        [Test]
		public void Constructor_EmptyUnitsThrows()
		{
            IList<IBuildableWrapper<IUnit>> units = new List<IBuildableWrapper<IUnit>>();
            Assert.Throws<UnityAsserts.AssertionException>(() => new MostExpensiveUnitChooser(units, _droneManager, new AffordableUnitFilter()));
		}

		[Test]
		public void Constructor_ChoosesUnit()
		{
            Assert.IsNotNull(_unitChooser.ChosenUnit);
		}

        [Test]
        public void ChooseUnit_ReturnsNullIfCannotAffordAnyUnits()
        {
            DroneNumberChanged(newDroneNum: 0, expectedChosenUnit: null);
        }

        [Test]
        public void ChooseUnit_ReturnsMostExpensiveUnit_2Drones()
        {
			DroneNumberChanged(2, _unit2Drones);
        }

		[Test]
		public void ChooseUnit_ReturnsMostExpensiveUnit_4Drones()
		{
			DroneNumberChanged(4, _unit4Drones);
		}

        [Test]
        public void ChooseUnit_ReturnsMostExpensiveUnit_6Drones()
        {
			DroneNumberChanged(6, _unit6Drones);
        }

        [Test]
        public void ChooseUnit_ReturnsMostExpensiveUnit_8Drones()
        {
            DroneNumberChanged(8, _unit6Drones);
        }

        private void DroneNumberChanged(int newDroneNum, IBuildableWrapper<IUnit> expectedChosenUnit)
        {
            _droneManager.NumOfDrones.Returns(newDroneNum);
            _droneManager.DroneNumChanged += Raise.EventWith(_droneManager, new DroneNumChangedEventArgs(newNumOfDrones: newDroneNum));
            Assert.AreSame(expectedChosenUnit, _unitChooser.ChosenUnit);
        }
	}
}
