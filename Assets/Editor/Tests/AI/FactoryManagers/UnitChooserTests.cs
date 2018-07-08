using BattleCruisers.AI.FactoryManagers;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.AI.FactoryManagers
{
    public class DummyUnitChooser : UnitChooser
    {
        public void SetChosenUnit(IBuildableWrapper<IUnit> newChosenUnit)
        {
            ChosenUnit = newChosenUnit;
        }

        public override void Dispose() { }
    }

    public class UnitChooserTests
    {
        private DummyUnitChooser _unitChooser;
        private IBuildableWrapper<IUnit> _unit1, _unit2;
        private int _changedCounter;

        [SetUp]
        public void TestSetup()
        {
            _unitChooser = new DummyUnitChooser();

            _unit1 = Substitute.For<IBuildableWrapper<IUnit>>();
            _unit2 = Substitute.For<IBuildableWrapper<IUnit>>();

            _changedCounter = 0;
            _unitChooser.ChosenUnitChanged += (sender, e) => _changedCounter++;
            Assert.AreEqual(0, _changedCounter);
        }

        [Test]
        public void ChosenUnit_NullToUnit_EmitsChangedEvent()
        {
            _unitChooser.SetChosenUnit(_unit1);
            Assert.AreEqual(1, _changedCounter);
        }

        [Test]
        public void ChosenUnit_NullToNull_DoesNotEmitsChangedEvent()
        {
            _unitChooser.SetChosenUnit(null);
            Assert.AreEqual(0, _changedCounter);
        }

        [Test]
        public void ChosenUnit_UnitToDifferentUnit_EmitsChangedEvent()
        {
            _unitChooser.SetChosenUnit(_unit1);
            Assert.AreEqual(1, _changedCounter);

            _unitChooser.SetChosenUnit(_unit2);
            Assert.AreEqual(2, _changedCounter);
        }

        [Test]
        public void ChosenUnit_UnitToSameUnit_DoesNotEmitsChangedEvent()
        {
            _unitChooser.SetChosenUnit(_unit1);
            Assert.AreEqual(1, _changedCounter);

            _unitChooser.SetChosenUnit(_unit1);
            Assert.AreEqual(1, _changedCounter);
        }
    }
}
