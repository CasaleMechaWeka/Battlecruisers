using BattleCruisers.AI.FactoryManagers;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
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
        private IFactory _navalFactory, _airFactory;

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
            
			_navalFactory = Substitute.For<IFactory>();
			_navalFactory.UnitCategory.Returns(UnitCategory.Naval);
            _navalFactory.UnitWrapper = null;

            _airFactory = Substitute.For<IFactory>();
            _airFactory.UnitCategory.Returns(UnitCategory.Aircraft);
            _airFactory.UnitWrapper = null;
        }

        [Test]
        public void MatchingFactoryBuilt_SetsUnit()
        {
            _friendlyCruiser.StartedConstruction += Raise.EventWith(_friendlyCruiser, new StartedConstructionEventArgs(_navalFactory));
            Assert.IsNull(_navalFactory.UnitWrapper);
            _navalFactory.CompletedBuildable += Raise.Event();
            Assert.AreSame(_unit, _navalFactory.UnitWrapper);
        }

        [Test]
        public void NonMatchingFactoryBuilt_DoesNotSetUnit()
        {
            _friendlyCruiser.StartedConstruction += Raise.EventWith(_friendlyCruiser, new StartedConstructionEventArgs(_airFactory));
            Assert.IsNull(_airFactory.UnitWrapper);
			_navalFactory.CompletedBuildable += Raise.Event();
			Assert.IsNull(_airFactory.UnitWrapper);
        }

		[Test]
		public void NavalFactory_UnitCompleted_SetsUnit()
		{
            // Factory completed
			_friendlyCruiser.StartedConstruction += Raise.EventWith(_friendlyCruiser, new StartedConstructionEventArgs(_navalFactory));
            _navalFactory.CompletedBuildable += Raise.Event();
			Assert.AreNotSame(_unit2, _navalFactory.UnitWrapper);

            // Unit completed
            _navalFactory.CompletedBuildingUnit += Raise.EventWith(_navalFactory, new CompletedConstructionEventArgs(_unit.Buildable));
			Assert.AreSame(_unit2, _navalFactory.UnitWrapper);
		}
    }
}
