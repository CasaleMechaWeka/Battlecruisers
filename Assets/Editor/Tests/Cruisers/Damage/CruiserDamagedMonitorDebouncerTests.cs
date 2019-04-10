using BattleCruisers.Cruisers.Damage;
using NSubstitute;
using NUnit.Framework;
using UnityCommon.PlatformAbstractions;

namespace BattleCruisers.Tests.Cruisers.Damage
{
    public class CruiserDamagedMonitorDebouncerTests
    {
        private ICruiserDamageMonitor _debouncer, _coreMonitor;
        private ITime _time;
        private float _debounceTimeInS;
        private int _eventCount;

        [SetUp]
        public void TestSetup()
        {
            _coreMonitor = Substitute.For<ICruiserDamageMonitor>();
            _time = Substitute.For<ITime>();
            _debounceTimeInS = 10;

            _debouncer = new CruiserDamagedMonitorDebouncer(_coreMonitor, _time, _debounceTimeInS);

            _eventCount = 0;
            _debouncer.CruiserOrBuildingDamaged += (sender, e) => _eventCount++;
        }

        [Test]
        public void Damaged_FirstTime_ForwardsEvent()
        {
            _time.TimeSinceGameStartInS.Returns(0);
            _coreMonitor.CruiserOrBuildingDamaged += Raise.Event();
            Assert.AreEqual(1, _eventCount);
        }

        [Test]
        public void Damaged_WithinDebounceTime_DoesNotForwardEvent()
        {
            // First damaged
            _time.TimeSinceGameStartInS.Returns(0);
            _coreMonitor.CruiserOrBuildingDamaged += Raise.Event();
            Assert.AreEqual(1, _eventCount);

            // Damaged within debounce time
            _time.TimeSinceGameStartInS.Returns(_debounceTimeInS - 1);
            _coreMonitor.CruiserOrBuildingDamaged += Raise.Event();
            Assert.AreEqual(1, _eventCount);
        }

        [Test]
        public void Damaged_OutsideDebounceTime_ForwardsEvent()
        {
            // First damaged
            _time.TimeSinceGameStartInS.Returns(0);
            _coreMonitor.CruiserOrBuildingDamaged += Raise.Event();
            Assert.AreEqual(1, _eventCount);

            // Damaged outside of debounce time
            _time.TimeSinceGameStartInS.Returns(_debounceTimeInS + 1);
            _coreMonitor.CruiserOrBuildingDamaged += Raise.Event();
            Assert.AreEqual(2, _eventCount);
        }
    }
}