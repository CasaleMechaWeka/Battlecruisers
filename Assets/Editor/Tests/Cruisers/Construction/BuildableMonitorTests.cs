using NUnit.Framework;
using NSubstitute;
using UnityEngine;
using UnityAsserts = UnityEngine.Assertions;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Cruisers;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Units;
using System.Linq;

namespace BattleCruisers.Tests.Cruisers.Construction
{
    public class BuildableMonitorTests
    {
        private IBuildableMonitor _buildableMonitor;
        private ICruiserController _cruiser;
        private IBuilding _building;
        private IUnit _unit;

        [SetUp]
        public void TestSetup()
        {
            _cruiser = Substitute.For<ICruiserController>();
            _buildableMonitor = new BuildableMonitor(_cruiser);

            _building = Substitute.For<IBuilding>();
            _unit = Substitute.For<IUnit>();

            UnityAsserts.Assert.raiseExceptions = true;
        }

        [Test]
        public void InitialState()
        {
            Assert.AreEqual(0, _buildableMonitor.AliveBuildings.Count);
            Assert.AreEqual(0, _buildableMonitor.AliveUnits.Count);
        }

        [Test]
        public void BuildingCompleted_AddsBuilding()
        {
            _cruiser.BuildingCompleted += Raise.EventWith(new CompletedBuildingConstructionEventArgs(_building));

            Assert.AreEqual(1, _buildableMonitor.AliveBuildings);
            Assert.AreSame(_building, _buildableMonitor.AliveBuildings.First());
        }

        [Test]
        public void SameBuildingCompletedAgain_Throws()
        {
            _cruiser.BuildingCompleted += Raise.EventWith(new CompletedBuildingConstructionEventArgs(_building));
            Assert.Throws<UnityAsserts.AssertionException>(() => _cruiser.BuildingCompleted += Raise.EventWith(new CompletedBuildingConstructionEventArgs(_building)));
        }

        [Test]
        public void BuildingDestroyed_RemovesBuilding()
        {
            // Complete building
            _cruiser.BuildingCompleted += Raise.EventWith(new CompletedBuildingConstructionEventArgs(_building));
            Assert.AreEqual(1, _buildableMonitor.AliveBuildings);
            Assert.AreSame(_building, _buildableMonitor.AliveBuildings.First());

            // Destroy building
            //_building.Destroyed += Raise.EventWith(new BuildingDestroyedEventArgs())
        }
    }
}