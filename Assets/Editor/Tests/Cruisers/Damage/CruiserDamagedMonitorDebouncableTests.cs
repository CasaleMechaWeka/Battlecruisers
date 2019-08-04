using BattleCruisers.Cruisers.Damage;
using NSubstitute;
using NUnit.Framework;
using System;

namespace BattleCruisers.Tests.Cruisers.Damage
{
    public class CruiserDamagedMonitorDebouncableTests
    {
        private CruiserDamagedMonitorDebouncable _debouncable;
        private ICruiserDamageMonitor _cruiserDamangeMonitor;
        private int _undebouncedEventCount, _debouncedEventCount;

        [SetUp]
        public void TestSetup()
        {
            _cruiserDamangeMonitor = Substitute.For<ICruiserDamageMonitor>();
            _debouncable = new CruiserDamagedMonitorDebouncable(_cruiserDamangeMonitor);

            _undebouncedEventCount = 0;
            _debouncable.UndebouncedEvent += (sender, e) => _undebouncedEventCount++;

            _debouncedEventCount = 0;
            _debouncable.CruiserOrBuildingDamaged += (sender, e) => _debouncedEventCount++;
        }

        [Test]
        public void UndebouncedEvent()
        {
            _cruiserDamangeMonitor.CruiserOrBuildingDamaged += Raise.Event();
            Assert.AreEqual(1, _undebouncedEventCount);
        }

        [Test]
        public void EmitDebouncedEvent()
        {
            _debouncable.EmitDebouncedEvent(EventArgs.Empty);
            Assert.AreEqual(1, _debouncedEventCount);
        }

        [Test]
        public void DisposeManagedState()
        {
            _debouncable.DisposeManagedState();
            _cruiserDamangeMonitor.CruiserOrBuildingDamaged += Raise.Event();
            Assert.AreEqual(0, _undebouncedEventCount);
        }
    }
}