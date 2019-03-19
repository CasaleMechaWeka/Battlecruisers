using BattleCruisers.AI.Drones.BuildingMonitors;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Cruisers.Construction;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.AI.Drones.BuildingMonitors
{
    public class FactoryMonitorTests
    {
        private IFactoryMonitor _monitor;
        private IFactory _factory;

        [SetUp]
        public void TestSetup()
        {
            _factory = Substitute.For<IFactory>();
            _monitor = new FactoryMonitor(_factory, desiredNumOfUnits: 1);
        }

        [Test]
        public void Factory_ReturnsFactory()
        {
            Assert.AreSame(_factory, _monitor.Factory);
        }

        [Test]
        public void HasFactoryBuiltDesiredNumOfUnits_False()
        {
            Assert.IsFalse(_monitor.HasFactoryBuiltDesiredNumOfUnits);
        }

        [Test]
        public void HasFactoryBuiltDesiredNumOfUnits_True()
        {
            _factory.CompletedBuildingUnit += Raise.EventWith(new UnitCompletedEventArgs(completedUnit: null));
            Assert.IsTrue(_monitor.HasFactoryBuiltDesiredNumOfUnits);
        }
    }
}