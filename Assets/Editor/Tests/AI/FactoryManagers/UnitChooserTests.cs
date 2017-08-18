using System.Collections.Generic;
using BattleCruisers.AI.FactoryManagers;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.AI.FactoryManagers
{
	public class UnitChooserTests
	{
        private IUnitChooser _unitChooser;
        private IList<IBuildableWrapper<IUnit>> _units;
        private IBuildableWrapper<IUnit> _unit2Drones, _unit4Drones, _unit6Drones;

		[SetUp]
		public void SetuUp()
		{
			UnityAsserts.Assert.raiseExceptions = true;

            _unit2Drones = CreateUnit(numOfDrones: 2);
            _unit4Drones = CreateUnit(numOfDrones: 4);
            _unit6Drones = CreateUnit(numOfDrones: 6);

            _units = new List<IBuildableWrapper<IUnit>>()
            {
                _unit2Drones,
                _unit4Drones,
                _unit6Drones
            };

            _unitChooser = new MostExpensiveUnitChooser(_units);
		}

        private IBuildableWrapper<IUnit> CreateUnit(int numOfDrones)
        {
            IBuildableWrapper<IUnit> unit = Substitute.For<IBuildableWrapper<IUnit>>();
            unit.Buildable.Returns(Substitute.For<IUnit>());
            unit.Buildable.NumOfDronesRequired.Returns(numOfDrones);
            return unit;
        }

		[Test]
		public void Constructor_NullUnitsThrows()
		{
            Assert.Throws<UnityAsserts.AssertionException>(() => new MostExpensiveUnitChooser(units: null));
		}

		[Test]
		public void Constructor_EmptyUnitsThrows()
		{
            IList<IBuildableWrapper<IUnit>> units = new List<IBuildableWrapper<IUnit>>();
			Assert.Throws<UnityAsserts.AssertionException>(() => new MostExpensiveUnitChooser(units));
		}

        [Test]
        public void ChooseUnit_ReturnsNullIfCannotAffordAnyUnits()
        {
            Assert.IsNull(_unitChooser.ChooseUnit(numOfDrones: 0));
        }

        [Test]
        public void ChooseUnit_ReturnsMostExpensiveUnit_2Drones()
        {
            Assert.AreSame(_unit2Drones, _unitChooser.ChooseUnit(numOfDrones: 2));
        }

		[Test]
		public void ChooseUnit_ReturnsMostExpensiveUnit_4Drones()
		{
			Assert.AreSame(_unit4Drones, _unitChooser.ChooseUnit(numOfDrones: 4));
		}

        [Test]
        public void ChooseUnit_ReturnsMostExpensiveUnit_6Drones()
        {
            Assert.AreSame(_unit6Drones, _unitChooser.ChooseUnit(numOfDrones: 6));
        }

        [Test]
        public void ChooseUnit_ReturnsMostExpensiveUnit_8Drones()
        {
            Assert.AreSame(_unit6Drones, _unitChooser.ChooseUnit(numOfDrones: 8));
        }
	}
}
