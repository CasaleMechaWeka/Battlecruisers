using BattleCruisers.AI.FactoryManagers;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Tests.Utils.Extensions;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.AI.FactoryManagers
{
    public class FactoryManagerTests
    {
        private ICruiserController _friendlyCruiser;
        private IUnitChooser _unitChooser;
        private IBuildableWrapper<IUnit> _unit, _unit2;
        private IFactory _navalFactory, _navalFactory2, _notCompletedNavalFactory, _airFactory;

        [SetUp]
        public void SetuUp()
        {
            UnityAsserts.Assert.raiseExceptions = true;

            _friendlyCruiser = Substitute.For<ICruiserController>();
            _unit = Substitute.For<IBuildableWrapper<IUnit>>();
            _unit2 = Substitute.For<IBuildableWrapper<IUnit>>();
            _unitChooser = Substitute.For<IUnitChooser>();
            _unitChooser.ChosenUnit.Returns(_unit, _unit2);
            new FactoryManager(UnitCategory.Naval, _friendlyCruiser, _unitChooser);

            _navalFactory = CreateFactory(UnitCategory.Naval);
            _navalFactory2 = CreateFactory(UnitCategory.Naval);
            _notCompletedNavalFactory = CreateFactory(UnitCategory.Naval, BuildableState.InProgress);
            _airFactory = CreateFactory(UnitCategory.Aircraft);
        }

        private IFactory CreateFactory(UnitCategory unitCategory, BuildableState buildableState = BuildableState.Completed)
        {
            IFactory factory = Substitute.For<IFactory>();
            factory.UnitCategory.Returns(unitCategory);
            factory.UnitWrapper = null;
            factory.BuildableState.Returns(buildableState);
            return factory;
        }

        [Test]
        public void MatchingFactoryBuilt_SetsUnit()
        {
            _friendlyCruiser.StartConstructingBuilding(_navalFactory);
            Assert.IsNull(_navalFactory.UnitWrapper);
            _navalFactory.CompletedBuildable += Raise.Event();
            Assert.AreSame(_unit, _navalFactory.UnitWrapper);
        }

        [Test]
        public void NonMatchingFactoryBuilt_DoesNotSetUnit()
        {
            _friendlyCruiser.StartConstructingBuilding(_airFactory);
            Assert.IsNull(_airFactory.UnitWrapper);
			_navalFactory.CompletedBuildable += Raise.Event();
			Assert.IsNull(_airFactory.UnitWrapper);
        }

		[Test]
		public void NavalFactory_UnitCompleted_SetsUnit()
		{
            // Factory completed
            _friendlyCruiser.StartConstructingBuilding(_navalFactory);
            _navalFactory.CompletedBuildable += Raise.Event();
			Assert.AreNotSame(_unit2, _navalFactory.UnitWrapper);

            // Unit completed
            _navalFactory.CompletedBuildingUnit += Raise.EventWith(_navalFactory, new CompletedUnitConstructionEventArgs(_unit.Buildable));
			Assert.AreSame(_unit2, _navalFactory.UnitWrapper);
		}

        [Test]
        public void ChosenUnitChanged_SetsUnit_ForCompletedAndInactiveFactories()
        {
            // Need to start buliding factories for manager to keep track of them
            _friendlyCruiser.StartConstructingBuilding(_navalFactory);
            _friendlyCruiser.StartConstructingBuilding(_navalFactory2);
            _friendlyCruiser.StartConstructingBuilding(_notCompletedNavalFactory);

            _navalFactory.UnitWrapper = _unit;
            _navalFactory2.UnitWrapper = null;
            _notCompletedNavalFactory.UnitWrapper = null;

            _unitChooser.ChosenUnit.Returns(_unit2);
            _unitChooser.ChosenUnitChanged += Raise.Event();

            // Factory was already active, so unit unchanged
            Assert.AreSame(_unit, _navalFactory.UnitWrapper);

            // Factory was inactive, so unit assigned
            Assert.AreSame(_unit2, _navalFactory2.UnitWrapper);

            // Factory was inactive, but not yet completed.  So no unit assignment.
            Assert.IsNull(_notCompletedNavalFactory.UnitWrapper);
        }
    }
}
