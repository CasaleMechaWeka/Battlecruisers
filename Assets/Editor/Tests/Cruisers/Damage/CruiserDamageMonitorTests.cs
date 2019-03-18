using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Damage;
using BattleCruisers.Tests.Utils.Extensions;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Cruisers.Damage
{
    public class CruiserDamageMonitorTests
    {
        private CruiserDamageMonitor _monitor;
        private ICruiser _cruiser;
        private IBuilding _building;
        private int _eventCount;

        [SetUp]
        public void TestSetup()
        {
            _cruiser = Substitute.For<ICruiser>();
            _monitor = new CruiserDamageMonitor(_cruiser);

            _building = Substitute.For<IBuilding>();

            _eventCount = 0;
            _monitor.CruiserOrBuildingDamaged += (sender, e) => _eventCount++;
        }

        [Test]
        public void CruiserDamaged_EmitsEvent()
        {
            _cruiser.Damaged += Raise.EventWith(new DamagedEventArgs(null));
            Assert.AreEqual(1, _eventCount);
        }

        [Test]
        public void BuildingDamaged_EmitsEvent()
        {
            _cruiser.BuildingMonitor.EmitBuildingStarted(_building);
            _building.Damaged += Raise.EventWith(new DamagedEventArgs(null));

            Assert.AreEqual(1, _eventCount);
        }

        [Test]
        public void BuildingDestroyed_Unsubsribes()
        {
            // Subscribe to Damaged event
            _cruiser.BuildingMonitor.EmitBuildingStarted(_building);

            // Unsubscribe from Damaged event
            _cruiser.BuildingMonitor.EmitBuildingDestroyed(_building);

            // Damaged event should be ignored
            _building.Damaged += Raise.EventWith(new DamagedEventArgs(null));
            Assert.AreEqual(0, _eventCount);
        }

        [Test]
        public void Dispose_Unsubsribes()
        {
            _monitor.DisposeManagedState();

            _cruiser.Damaged += Raise.EventWith(new DamagedEventArgs(null));
            Assert.AreEqual(0, _eventCount);

            _cruiser.BuildingMonitor.EmitBuildingStarted(_building);
            _building.Damaged += Raise.EventWith(new DamagedEventArgs(null));
            Assert.AreEqual(0, _eventCount);
        }
    }
}