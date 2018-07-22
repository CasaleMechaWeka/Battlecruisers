using BattleCruisers.AI.Drones.BuildingMonitors;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.AI.Drones.BuildingMonitors
{
    public class FactoryWastingDronesFilterTests
    {
        private IFilter<IFactoryMonitor> _filter;
        private IFactoryMonitor _factoryMonitor;
        private IFactory _factory;
        private IDroneConsumer _factoryDroneConsumer;

        [SetUp]
        public void TestSetup()
        {
            _filter = new FactoryWastingDronesFilter();

            _factoryMonitor = Substitute.For<IFactoryMonitor>();

            _factory = Substitute.For<IFactory>();
            _factoryMonitor.Factory.Returns(_factory);

            _factoryDroneConsumer = Substitute.For<IDroneConsumer>();
        }

        [Test]
        public void IsMatch_HasNotBuiltUnits_ReturnsFalse()
        {
            _factoryMonitor.HasFactoryBuiltDesiredNumOfUnits.Returns(false);
            Assert.IsFalse(_filter.IsMatch(_factoryMonitor));
        }

        [Test]
        public void IsMatch_NoDroneConsumer_ReturnsFalse()
        {
            _factoryMonitor.HasFactoryBuiltDesiredNumOfUnits.Returns(true);
            _factory.DroneConsumer.Returns((IDroneConsumer)null);

            Assert.IsFalse(_filter.IsMatch(_factoryMonitor));
        }

        [Test]
        public void IsMatch_DroneConsumerIsIdle_ReturnsFalse()
        {
            _factoryMonitor.HasFactoryBuiltDesiredNumOfUnits.Returns(true);
            _factory.DroneConsumer.Returns(_factoryDroneConsumer);
            _factoryDroneConsumer.State.Returns(DroneConsumerState.Idle);

            Assert.IsFalse(_filter.IsMatch(_factoryMonitor));
        }

        [Test]
        public void IsMatch_HasBuiltUnits_HasIdleDroneConsumer_ReturnsTrue()
        {
            _factoryMonitor.HasFactoryBuiltDesiredNumOfUnits.Returns(true);
            _factory.DroneConsumer.Returns(_factoryDroneConsumer);
            _factoryDroneConsumer.State.Returns(DroneConsumerState.Active);

            Assert.IsTrue(_filter.IsMatch(_factoryMonitor));
        }
    }
}