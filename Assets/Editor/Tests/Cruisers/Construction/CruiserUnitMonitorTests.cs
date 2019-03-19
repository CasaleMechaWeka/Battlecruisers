using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings.Factories;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Tests.Utils.Extensions;
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
        private UnitStartedEventArgs _lastStartedEventArgs;
        private UnitCompletedEventArgs _lastCompletedEventArgs;
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
        public void FactoryStartsUnit_AddsUnit_EmitsEvent()
        {
            _buildingMonitor.EmitBuildingCompleted(_factory);

            UnitStartedEventArgs eventArgs = new UnitStartedEventArgs(_unit);
            _factory.StartedBuildingUnit += Raise.EventWith(eventArgs);

            Assert.AreSame(eventArgs, _lastStartedEventArgs);
            Assert.AreEqual(1, _unitMonitor.AliveUnits.Count);
            Assert.AreSame(_unit, _unitMonitor.AliveUnits.First());
        }

        [Test]
        public void FactoryStartsUnit_UnitPreviouslyStarted_Throws()
        {
            _buildingMonitor.EmitBuildingCompleted(_factory);

            // Unit completes
            _factory.StartBuildingUnit(_unit);
            // Same unit completes again
            Assert.Throws<UnityAsserts.AssertionException>(() => _factory.StartBuildingUnit(_unit));
        }

        [Test]
        public void FactoryCompletesUnit_EmitsEvent()
        {
            _buildingMonitor.EmitBuildingCompleted(_factory);
            _factory.StartBuildingUnit(_unit);

            UnitCompletedEventArgs completedEventArgs = new UnitCompletedEventArgs(_unit);
            _factory.CompletedBuildingUnit += Raise.EventWith(completedEventArgs);

            Assert.AreSame(completedEventArgs, _lastCompletedEventArgs);
        }

        [Test]
        public void UnitDestroyed_RemovesUnit_EmitsEvent()
        {
            _buildingMonitor.EmitBuildingCompleted(_factory);

            _factory.StartBuildingUnit(_unit);
            _factory.CompleteBuildingUnit(_unit);

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
            _buildingMonitor.EmitBuildingCompleted(_factory);
            
            // Unsubscribe from factory event
            _factory.Destroyed += Raise.EventWith(new DestroyedEventArgs(_factory));

            // Assert event is no longer subscribed to
            _factory.StartBuildingUnit(_unit);
            Assert.IsNull(_lastStartedEventArgs);
            _factory.CompleteBuildingUnit(_unit);
            Assert.IsNull(_lastCompletedEventArgs);
        }

        [Test]
        public void Dispose_UnsubscribesFromBuildingMonitor()
        {
            _unitMonitor.DisposeManagedState();

            // This should no longer be listend to
            _buildingMonitor.EmitBuildingCompleted(_factory);

            // Assert event is no longer subscribed to
            _factory.StartBuildingUnit(_unit);
            Assert.IsNull(_lastStartedEventArgs);
            _factory.CompleteBuildingUnit(_unit);
            Assert.IsNull(_lastCompletedEventArgs);
        }
    }
}