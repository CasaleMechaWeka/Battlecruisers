using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Construction;
using NSubstitute;
using NUnit.Framework;
using System.Linq;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.Cruisers.Construction
{
    public class CruiserBuildingMonitorTests
    {
        private ICruiserBuildingMonitor _buildingMonitor;
        private ICruiserController _cruiser;
        private IBuilding _building;
        private int _startedCount, _completedCount, _destroyedCount;

        [SetUp]
        public void TestSetup()
        {
            _cruiser = Substitute.For<ICruiserController>();
            _buildingMonitor = new CruiserBuildingMonitor(_cruiser);

            _building = Substitute.For<IBuilding>();

            _startedCount = 0;
            _buildingMonitor.BuildingStarted += (sender, e) => _startedCount++;

            _completedCount = 0;
            _buildingMonitor.BuildingCompleted += (sender, e) => _completedCount++;

            _destroyedCount = 0;
            _buildingMonitor.BuildingDestroyed += (sender, e) => _destroyedCount++;

            UnityAsserts.Assert.raiseExceptions = true;
        }

        [Test]
        public void InitialState()
        {
            Assert.AreEqual(0, _buildingMonitor.AliveBuildings.Count);
        }

        [Test]
        public void BuildingStarted_ForwardsEvent()
        {
            _cruiser.BuildingStarted += Raise.EventWith(new BuildingStartedEventArgs(_building));

            Assert.AreEqual(1, _startedCount);
            Assert.AreEqual(0, _buildingMonitor.AliveBuildings.Count);
        }

        [Test]
        public void BuildingDestroyed_AfterStarted_BeforeCompleted_EmitsEvent()
        {
            _cruiser.BuildingStarted += Raise.EventWith(new BuildingStartedEventArgs(_building));
            _building.Destroyed += Raise.EventWith(new DestroyedEventArgs(_building));

            Assert.AreEqual(1, _destroyedCount);

            CheckEventsAreUnsubscribed();
        }

        [Test]
        public void BuildingCompleted_AddsBuilding_EmitsEvent()
        {
            _cruiser.BuildingStarted += Raise.EventWith(new BuildingStartedEventArgs(_building));
            _building.CompletedBuildable += Raise.Event();

            Assert.AreEqual(1, _buildingMonitor.AliveBuildings.Count);
            Assert.AreSame(_building, _buildingMonitor.AliveBuildings.First());
            Assert.AreEqual(1, _completedCount);
        }

        [Test]
        public void SameBuildingCompletedAgain_Throws()
        {
            // Start and complete building
            _cruiser.BuildingStarted += Raise.EventWith(new BuildingStartedEventArgs(_building));
            _building.CompletedBuildable += Raise.Event();

            // Start and complete SAME building
            _cruiser.BuildingStarted += Raise.EventWith(new BuildingStartedEventArgs(_building));
            Assert.Throws<UnityAsserts.AssertionException>(() => _building.CompletedBuildable += Raise.Event());
        }

        [Test]
        public void BuildingDestroyed_AfterCompleted_RemovesBuilding_EmitsEvent()
        {
            // Complete building
            _cruiser.BuildingStarted += Raise.EventWith(new BuildingStartedEventArgs(_building));
            _building.CompletedBuildable += Raise.Event();
            Assert.AreEqual(1, _buildingMonitor.AliveBuildings.Count);
            Assert.AreSame(_building, _buildingMonitor.AliveBuildings.First());

            // Destroy building
            _building.Destroyed += Raise.EventWith(new DestroyedEventArgs(_building));
            Assert.AreEqual(0, _buildingMonitor.AliveBuildings.Count);
            Assert.AreEqual(1, _destroyedCount);

            CheckEventsAreUnsubscribed();
        }

        private void CheckEventsAreUnsubscribed()
        {
            _completedCount = 0;
            _building.CompletedBuildable += Raise.EventWith(new BuildingCompletedEventArgs(_building));
            Assert.AreEqual(0, _completedCount);

            _destroyedCount = 0;
            _building.Destroyed += Raise.EventWith(new DestroyedEventArgs(_building));
            Assert.AreEqual(0, _destroyedCount);
        }
    }
}