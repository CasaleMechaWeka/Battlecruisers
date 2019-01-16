using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Buildings.Factories.Spawning;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Buildables.Buildings.Factories.Spawning
{
    public class UnitSpawnTimerTests
    {
        private IUnitSpawnTimer _unitSpawnTimer;
        private IFactory _factory;
        private ITime _time;
        private IUnit _unit;

        [SetUp]
        public void TestSetup()
        {
            _factory = Substitute.For<IFactory>();
            _time = Substitute.For<ITime>();
            _unitSpawnTimer = new UnitSpawnTimer(_factory, _time);

            _time.TimeSinceGameStartInS.Returns(0);
            _unit = Substitute.For<IUnit>();
        }

        [Test]
        public void InitialState()
        {
            float expectedTime = _time.TimeSinceGameStartInS - float.MinValue;
            Assert.AreEqual(expectedTime, _unitSpawnTimer.TimeSinceFactoryWasClearInS);
        }

        [Test]
        public void UnitCompleted_ResetsTimer_AndUnsubscribesFromDestroyedEvent()
        {
            _factory.StartedBuildingUnit += Raise.EventWith(new StartedUnitConstructionEventArgs(_unit));

            _time.TimeSinceGameStartInS.Returns(1);
            _factory.CompletedBuildingUnit += Raise.EventWith(new CompletedUnitConstructionEventArgs(_unit));

            Assert.AreEqual(0, _unitSpawnTimer.TimeSinceFactoryWasClearInS);

            // Destroyed event unsubscribed
            _time.TimeSinceGameStartInS.Returns(2);
            _unit.Destroyed += Raise.EventWith(new DestroyedEventArgs(_unit));

            Assert.AreEqual(1, _unitSpawnTimer.TimeSinceFactoryWasClearInS);
        }

        [Test]
        public void UnitDestroyed_ResetsTimer_AndUnsubscribesFromDestroyedEvent()
        {
            _factory.StartedBuildingUnit += Raise.EventWith(new StartedUnitConstructionEventArgs(_unit));

            _time.TimeSinceGameStartInS.Returns(1);
            _unit.Destroyed += Raise.EventWith(new DestroyedEventArgs(_unit));

            Assert.AreEqual(0, _unitSpawnTimer.TimeSinceFactoryWasClearInS);

            // Destroyed event unsubscribed
            _time.TimeSinceGameStartInS.Returns(2);
            _unit.Destroyed += Raise.EventWith(new DestroyedEventArgs(_unit));

            Assert.AreEqual(1, _unitSpawnTimer.TimeSinceFactoryWasClearInS);
        }
    }
}