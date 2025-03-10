using BattleCruisers.AI.FactoryManagers;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Tests.Utils.Extensions;
using NSubstitute;
using NUnit.Framework;

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
            factory.UnitWrapper.Returns((IBuildableWrapper<IUnit>)null);
            factory.BuildableState.Returns(buildableState);
            return factory;
        }

        [Test]
        public void MatchingFactoryBuilt_StartsBuildingUnit()
        {
            _friendlyCruiser.StartConstructingBuilding(_navalFactory);
            Assert.IsNull(_navalFactory.UnitWrapper);
            _navalFactory.CompletedBuildable += Raise.Event();
            _navalFactory.Received().StartBuildingUnit(_unit);
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
		public void NavalFactory_UnitCompleted_UpdatesChosenUnit()
		{
            // Factory completed
            _friendlyCruiser.StartConstructingBuilding(_navalFactory);
            _navalFactory.CompletedBuildable += Raise.Event();
			Assert.AreNotSame(_unit2, _navalFactory.UnitWrapper);

            // Unit completed
            _navalFactory.UnitCompleted += Raise.EventWith(_navalFactory, new UnitCompletedEventArgs(_unit.Buildable));
            _navalFactory.Received().StartBuildingUnit(_unit2);
		}

        [Test]
        public void ChosenUnitChanged_StartsBuildingUnit_ForCompletedAndInactiveFactories()
        {
            // Need to start buliding factories for manager to keep track of them
            _friendlyCruiser.StartConstructingBuilding(_navalFactory);
            _friendlyCruiser.StartConstructingBuilding(_navalFactory2);
            _friendlyCruiser.StartConstructingBuilding(_notCompletedNavalFactory);

            _navalFactory.UnitWrapper.Returns(_unit);
            _navalFactory2.UnitWrapper.Returns((IBuildableWrapper<IUnit>)null);
            _notCompletedNavalFactory.UnitWrapper.Returns((IBuildableWrapper<IUnit>)null);

            _unitChooser.ChosenUnit.Returns(_unit2);
            _unitChooser.ChosenUnitChanged += Raise.Event();

            // Factory was already active, so unit unchanged
            Assert.AreSame(_unit, _navalFactory.UnitWrapper);

            // Factory was inactive, so unit assigned
            _navalFactory2.Received().StartBuildingUnit(_unit2);

            // Factory was inactive, but not yet completed.  So no unit assignment.
            _notCompletedNavalFactory.DidNotReceiveWithAnyArgs().StartBuildingUnit(null);
        }
    }
}
