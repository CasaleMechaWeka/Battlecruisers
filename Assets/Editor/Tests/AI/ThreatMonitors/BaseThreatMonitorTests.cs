using BattleCruisers.AI.ThreatMonitors;
using NUnit.Framework;

namespace BattleCruisers.Tests.AI.ThreatMonitors
{
    public class DummyThreatMonitor : BaseThreatMonitor
    {
        public void SetThreatLevel(ThreatLevel newThreatLevel)
        {
            CurrentThreatLevel = newThreatLevel;
        }
    }

    public class BaseThreatMonitorTests
    {
        private DummyThreatMonitor _threatMonitor;
        private int _threatLevelChangedCount;

        [SetUp]
        public void TestSetup()
        {
            _threatMonitor = new DummyThreatMonitor();

            _threatLevelChangedCount = 0;
            _threatMonitor.ThreatLevelChanged += (sender, e) => _threatLevelChangedCount++;
        }

        [Test]
        public void InitialThreatLevelIsDefault()
        {
            Assert.AreEqual(ThreatLevel.None, _threatMonitor.CurrentThreatLevel);
        }

        [Test]
        public void ThreatLevelChanged_EmitsEvent()
        {
            _threatMonitor.SetThreatLevel(ThreatLevel.High);
            Assert.AreEqual(1, _threatLevelChangedCount);
        }

        [Test]
        public void ThreatLevelDidNotChange_DoesNotEmitEvent()
        {
            _threatMonitor.SetThreatLevel(ThreatLevel.None);
            Assert.AreEqual(0, _threatLevelChangedCount);
        }
    }
}