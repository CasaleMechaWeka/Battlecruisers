using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Construction;
using NSubstitute;
using NUnit.Framework;
using System.Linq;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Cruisers.Construction
{
    public class CruiserUnitMonitorTests
    {
        private CruiserUnitMonitor _unitMonitor;
        private ICruiserBuildingMonitor _buildingMonitor;
        private IFactory _factory;
        private StartedUnitConstructionEventArgs _lastStartedEventArgs;
        private CompletedUnitConstructionEventArgs _lastCompletedEventArgs;
        private int _destroyedCount;
        private IUnit _unit;

        [SetUp]
        public void TestSetup()
        {
            _buildingMonitor = Substitute.For<ICruiserBuildingMonitor>();
            _unitMonitor = new CruiserUnitMonitor(_buildingMonitor);

            _factory = Substitute.For<IFactory>();
            _lastStartedEventArgs = null;
            _lastCompletedEventArgs = null;

            _unitMonitor.UnitStarted += (sender, e) => _lastStartedEventArgs = e;
            _unitMonitor.UnitCompleted += (sender, e) => _lastCompletedEventArgs = e;

            _destroyedCount = 0;
            _unitMonitor.UnitDestroyed += (sender, e) => _destroyedCount++;

            _unit = Substitute.For<IUnit>();

            UnityAsserts.Assert.raiseExceptions = true;
        }

        [Test]
        public void InitialState()
        {
            Assert.AreEqual(0, _unitMonitor.AliveUnits.Count);
        }

        [Test]
        public void FactoryStartsUnit_EmitsEvent()
        {
            _buildingMonitor.BuildingCompleted += Raise.EventWith(new CompletedBuildingConstructionEventArgs(_factory));
            StartedUnitConstructionEventArgs eventArgs = new StartedUnitConstructionEventArgs(_unit);
            _factory.StartedBuildingUnit += Raise.EventWith(eventArgs);

            Assert.AreSame(eventArgs, _lastStartedEventArgs);
            Assert.AreEqual(0, _unitMonitor.AliveUnits.Count);
        }

        [Test]
        public void UnitDestroyed_AfterStarted_BeforeCompleted_EmitsEvent()
        {
            _buildingMonitor.BuildingCompleted += Raise.EventWith(new CompletedBuildingConstructionEventArgs(_factory));
            _factory.StartedBuildingUnit += Raise.EventWith(new StartedUnitConstructionEventArgs(_unit));
            _unit.Destroyed += Raise.EventWith(new DestroyedEventArgs(_unit));

            Assert.AreEqual(1, _destroyedCount);

            // Check destroyed event is unsubscribed
            _unit.Destroyed += Raise.EventWith(new DestroyedEventArgs(_unit));
            Assert.AreEqual(1, _destroyedCount);
        }

        [Test]
        public void FactoryCompletesUnit_EmitsEvent()
        {
            _buildingMonitor.BuildingCompleted += Raise.EventWith(new CompletedBuildingConstructionEventArgs(_factory));

            StartedUnitConstructionEventArgs startedEventArgs = new StartedUnitConstructionEventArgs(_unit);
            _factory.StartedBuildingUnit += Raise.EventWith(startedEventArgs);

            CompletedUnitConstructionEventArgs completedEventArgs = new CompletedUnitConstructionEventArgs(_unit);
            _factory.CompletedBuildingUnit += Raise.EventWith(completedEventArgs);

            Assert.AreSame(completedEventArgs, _lastCompletedEventArgs);
            Assert.AreEqual(1, _unitMonitor.AliveUnits.Count);
            Assert.AreSame(_unit, _unitMonitor.AliveUnits.First());
        }

        [Test]
        public void FactoryCompletesUnit_UnitPreviouslyCompleted_Throws()
        {
            _buildingMonitor.BuildingCompleted += Raise.EventWith(new CompletedBuildingConstructionEventArgs(_factory));

            // Unit completes
            CompletedUnitConstructionEventArgs completedEventArgs = new CompletedUnitConstructionEventArgs(_unit);
            _factory.CompletedBuildingUnit += Raise.EventWith(completedEventArgs);

            // Same unit completes again
            Assert.Throws<UnityAsserts.AssertionException>(() => _factory.CompletedBuildingUnit += Raise.EventWith(completedEventArgs));
        }

        [Test]
        public void UnitDestroyed_AfterCompleted_RemovesUnit_EmitsEvent()
        {
            _buildingMonitor.BuildingCompleted += Raise.EventWith(new CompletedBuildingConstructionEventArgs(_factory));
            _factory.StartedBuildingUnit += Raise.EventWith(new StartedUnitConstructionEventArgs(_unit));
            _factory.CompletedBuildingUnit += Raise.EventWith(new CompletedUnitConstructionEventArgs(_unit));

            _unit.Destroyed += Raise.EventWith(new DestroyedEventArgs(_unit));

            Assert.AreEqual(1, _destroyedCount);
            Assert.AreEqual(0, _unitMonitor.AliveUnits.Count);

            // Check destroyed event is unsubscribed
            _unit.Destroyed += Raise.EventWith(new DestroyedEventArgs(_unit));
            Assert.AreEqual(1, _destroyedCount);
        }

        [Test]
        public void FactoryDestroyed_UnsubsribesFromFactory()
        {
            // Subcribe to factory event
            _buildingMonitor.BuildingCompleted += Raise.EventWith(new CompletedBuildingConstructionEventArgs(_factory));
            
            // Unsubscribe from factory event
            _factory.Destroyed += Raise.EventWith(new DestroyedEventArgs(_factory));

            // Assert event is no longer subscribed to
            _factory.StartedBuildingUnit += Raise.EventWith(new StartedUnitConstructionEventArgs(_unit));
            Assert.IsNull(_lastStartedEventArgs);
            _factory.CompletedBuildingUnit += Raise.EventWith(new CompletedUnitConstructionEventArgs(_unit));
            Assert.IsNull(_lastCompletedEventArgs);
        }

        [Test]
        public void Dispose_UnsubscribesFromBuildingMonitor()
        {
            _unitMonitor.DisposeManagedState();

            // This should no longer be listend to
            _buildingMonitor.BuildingCompleted += Raise.EventWith(new CompletedBuildingConstructionEventArgs(_factory));
            
            // Assert event is no longer subscribed to
            _factory.StartedBuildingUnit += Raise.EventWith(new StartedUnitConstructionEventArgs(_unit));
            Assert.IsNull(_lastStartedEventArgs);
            _factory.CompletedBuildingUnit += Raise.EventWith(new CompletedUnitConstructionEventArgs(_unit));
            Assert.IsNull(_lastCompletedEventArgs);
        }
    }
}