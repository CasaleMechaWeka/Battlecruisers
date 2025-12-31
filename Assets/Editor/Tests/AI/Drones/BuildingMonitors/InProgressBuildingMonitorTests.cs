using BattleCruisers.AI.Drones.BuildingMonitors;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Tests.Utils.Extensions;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.AI.Drones.BuildingMonitors
{
    public class InProgressBuildingMonitorTests
    {
        private InProgressBuildingMonitor _monitor;
        private ICruiserController _cruiser;
        private IBuilding _building;

        [SetUp]
        public void TestSetup()
        {
            _cruiser = Substitute.For<ICruiserController>();
            _monitor = new InProgressBuildingMonitor(_cruiser);

            _building = Substitute.For<IBuilding>();
        }

        [Test]
        public void Default_Empty()
        {
            Assert.AreEqual(0, _monitor.InProgressBuildings.Count);
        }

        [Test]
        public void ConstructionStarted_AddsBuilding()
        {
            _cruiser.StartConstructingBuilding(_building);

            Assert.AreEqual(1, _monitor.InProgressBuildings.Count);
            Assert.AreSame(_building, _monitor.InProgressBuildings[0]);
        }

        [Test]
        public void BuildingCompleted_RemovesBuildings()
        {
            _cruiser.StartConstructingBuilding(_building);
            CompleteBuliding(_building);

            Assert.AreEqual(0, _monitor.InProgressBuildings.Count);
        }

        [Test]
        public void BulidingDestroyed_RemovesBuliding()
        {
            _cruiser.StartConstructingBuilding(_building);
            DestroyBuilding(_building);

            Assert.AreEqual(0, _monitor.InProgressBuildings.Count);
        }

        [Test]
        public void Dispose_RemovesBulidings()
        {
            _cruiser.StartConstructingBuilding(_building);
            _monitor.DisposeManagedState();
            Assert.AreEqual(0, _monitor.InProgressBuildings.Count);
        }

        private void CompleteBuliding(IBuilding building)
        {
            building.CompletedBuildable += Raise.EventWith(new BuildingCompletedEventArgs(building));
        }

        private void DestroyBuilding(IBuilding building)
        {
            building.Destroyed += Raise.EventWith(new DestroyedEventArgs(building));
        }
    }
}