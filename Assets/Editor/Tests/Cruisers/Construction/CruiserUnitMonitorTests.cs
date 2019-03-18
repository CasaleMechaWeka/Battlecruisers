using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Construction;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Cruisers.Construction
{
    public class CruiserUnitMonitorTests
    {
        private CruiserUnitMonitor _monitor;
        private ICruiserController _cruiser;
        private IFactory _factory;
        private StartedUnitConstructionEventArgs _lastStartedEventArgs;
        private CompletedUnitConstructionEventArgs _lastCompletedEventArgs;

        [SetUp]
        public void TestSetup()
        {
            _cruiser = Substitute.For<ICruiserController>();
            _monitor = new CruiserUnitMonitor(_cruiser);

            _factory = Substitute.For<IFactory>();
            _lastStartedEventArgs = null;
            _lastCompletedEventArgs = null;

            _monitor.UnitStarted += (sender, e) => _lastStartedEventArgs = e;
            _monitor.UnitCompleted += (sender, e) => _lastCompletedEventArgs = e;
        }

        [Test]
        public void FactoryStartsUnit_EmitsEvent()
        {
            _cruiser.BuildingCompleted += Raise.EventWith(new CompletedBuildingConstructionEventArgs(_factory));
            StartedUnitConstructionEventArgs eventArgs = new StartedUnitConstructionEventArgs(unit: null);
            _factory.StartedBuildingUnit += Raise.EventWith(eventArgs);

            Assert.IsNotNull(_lastStartedEventArgs);
            Assert.AreSame(eventArgs, _lastStartedEventArgs);
        }

        [Test]
        public void FactoryCompletesUnit_EmitsEvent()
        {
            _cruiser.BuildingCompleted += Raise.EventWith(new CompletedBuildingConstructionEventArgs(_factory));

            StartedUnitConstructionEventArgs startedEventArgs = new StartedUnitConstructionEventArgs(unit: null);
            _factory.StartedBuildingUnit += Raise.EventWith(startedEventArgs);

            CompletedUnitConstructionEventArgs completedEventArgs = new CompletedUnitConstructionEventArgs(unit: null);
            _factory.CompletedBuildingUnit += Raise.EventWith(completedEventArgs);

            Assert.IsNotNull(_lastCompletedEventArgs);
            Assert.AreSame(completedEventArgs, _lastCompletedEventArgs);
        }

        [Test]
        public void FactoryDestroyed_UnsubsribesFromFactory()
        {
            // Subcribe to factory event
            _cruiser.BuildingCompleted += Raise.EventWith(new CompletedBuildingConstructionEventArgs(_factory));
            
            // Unsubscribe from factory event
            _factory.Destroyed += Raise.EventWith(new DestroyedEventArgs(_factory));

            // Assert event is no longer subscribed to
            _factory.StartedBuildingUnit += Raise.EventWith(new StartedUnitConstructionEventArgs(unit: null));
            Assert.IsNull(_lastStartedEventArgs);
            _factory.CompletedBuildingUnit += Raise.EventWith(new CompletedUnitConstructionEventArgs(unit: null));
            Assert.IsNull(_lastCompletedEventArgs);
        }

        [Test]
        public void Dispose_UnsubscribesFromCruiser()
        {
            _monitor.DisposeManagedState();

            // This should no longer be listend to
            _cruiser.BuildingCompleted += Raise.EventWith(new CompletedBuildingConstructionEventArgs(_factory));
            
            // Assert event is no longer subscribed to
            _factory.StartedBuildingUnit += Raise.EventWith(new StartedUnitConstructionEventArgs(unit: null));
            Assert.IsNull(_lastStartedEventArgs);
            _factory.CompletedBuildingUnit += Raise.EventWith(new CompletedUnitConstructionEventArgs(unit: null));
            Assert.IsNull(_lastCompletedEventArgs);
        }
    }
}