using BattleCruisers.AI.ThreatMonitors;
using NUnit.Framework;

namespace BattleCruisers.Tests.AI.ThreatMonitors
{
    public class ThreatChangeSnapshotTests
    {
        private ThreatChangeSnapshot _snapshot;

        [SetUp]
        public void TestSetup()
        {
            _snapshot = new ThreatChangeSnapshot(ThreatLevel.Low, 17.71f);
        }

        [Test]
        public void Equals_DifferentThreatLevel_ReturnsFalse()
        {
            ThreatChangeSnapshot otherSnapshot = new ThreatChangeSnapshot(ThreatLevel.High, _snapshot.ChangeTimeSinceGameStartInS);
            Assert.AreNotEqual(otherSnapshot, _snapshot);
        }

        [Test]
        public void Equals_DifferentTime_ReturnsFalse()
        {
            ThreatChangeSnapshot otherSnapshot = new ThreatChangeSnapshot(_snapshot.ThreatLevel, 28.82f);
            Assert.AreNotEqual(otherSnapshot, _snapshot);
        }

        [Test]
        public void Equals_SameThreatLevel_SameTime_ReturnsTrue()
        {
            ThreatChangeSnapshot otherSnapshot = new ThreatChangeSnapshot(_snapshot.ThreatLevel, _snapshot.ChangeTimeSinceGameStartInS);
            Assert.AreEqual(otherSnapshot, _snapshot);
        }
    }
}