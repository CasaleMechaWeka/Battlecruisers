using BattleCruisers.UI.Music;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.UI.Music
{
    public class DangerMonitorSummariserTests
    {
        private IDangerMonitorSummariser _summariser;
        private IDangerMonitor _dangerMonitor;

        [SetUp]
        public void TestSetup()
        {
            _dangerMonitor = Substitute.For<IDangerMonitor>();
            _summariser = new DangerMonitorSummariser(_dangerMonitor);
        }

        [Test]
        public void Constructor()
        {
            Assert.IsFalse(_summariser.IsInDanger.Value);
        }

        [Test]
        public void IsInDanger()
        {
            // Danger start: 1
            _dangerMonitor.DangerStart += Raise.Event();
            Assert.IsTrue(_summariser.IsInDanger.Value);

            // Danger end: 0
            _dangerMonitor.DangerEnd += Raise.Event();
            Assert.IsFalse(_summariser.IsInDanger.Value);

            // Danger start: 1
            _dangerMonitor.DangerStart += Raise.Event();
            Assert.IsTrue(_summariser.IsInDanger.Value);

            // Danger start: 2
            _dangerMonitor.DangerStart += Raise.Event();
            Assert.IsTrue(_summariser.IsInDanger.Value);

            // Danger end: 1
            _dangerMonitor.DangerEnd += Raise.Event();
            Assert.IsTrue(_summariser.IsInDanger.Value);

            // Danger start: 2
            _dangerMonitor.DangerStart += Raise.Event();
            Assert.IsTrue(_summariser.IsInDanger.Value);

            // Danger end: 1
            _dangerMonitor.DangerEnd += Raise.Event();
            Assert.IsTrue(_summariser.IsInDanger.Value);

            // Danger end: 0
            _dangerMonitor.DangerEnd += Raise.Event();
            Assert.IsFalse(_summariser.IsInDanger.Value);
        }

        [Test]
        public void _dangerMonitor_DangerEnd_WithoutPreviousStart()
        {
            Assert.Throws<UnityAsserts.AssertionException>(() => _dangerMonitor.DangerEnd += Raise.Event());
        }
    }
}