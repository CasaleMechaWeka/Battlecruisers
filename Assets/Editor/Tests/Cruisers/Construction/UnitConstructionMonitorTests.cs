using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Construction;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Cruisers.Construction
{
    public class UnitConstructionMonitorTests
    {
        private UnitConstructionMonitor _monitor;
        private ICruiserController _cruiser;
        private IFactory _factory;
        private StartedUnitConstructionEventArgs _lastEventArgs;

        [SetUp]
        public void TestSetup()
        {
            _cruiser = Substitute.For<ICruiserController>();
            _monitor = new UnitConstructionMonitor(_cruiser);

            _factory = Substitute.For<IFactory>();
            _lastEventArgs = null;

            _monitor.StartedBuildingUnit += (sender, e) => _lastEventArgs = e;
        }

        [Test]
        public void FactoryStartsUnit_EmitsEvent()
        {
            _cruiser.BuildingCompleted += Raise.EventWith(new CompletedBuildingConstructionEventArgs(_factory));
            StartedUnitConstructionEventArgs eventArgs = new StartedUnitConstructionEventArgs(unit: null);
            _factory.StartedBuildingUnit += Raise.EventWith(eventArgs);

            Assert.IsNotNull(_lastEventArgs);
            Assert.AreSame(eventArgs, _lastEventArgs);
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
            Assert.IsNull(_lastEventArgs);
        }

        [Test]
        public void Dispose_UnsubscribesFromCruiser()
        {
            _monitor.DisposeManagedState();

            // This should no longer be listend to
            _cruiser.BuildingCompleted += Raise.EventWith(new CompletedBuildingConstructionEventArgs(_factory));
            
            // Assert event is no longer subscribed to
            _factory.StartedBuildingUnit += Raise.EventWith(new StartedUnitConstructionEventArgs(unit: null));
            Assert.IsNull(_lastEventArgs);
        }
    }
}