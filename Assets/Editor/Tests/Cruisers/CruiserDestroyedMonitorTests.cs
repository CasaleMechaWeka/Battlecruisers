using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Cruisers
{
    public class CruiserDestroyedMonitorTests
    {
        private CruiserDestroyedMonitor _monitor;
        private ICruiser _playerCruiser, _aiCruiser;
        private int _destroyedEventCount;

        [SetUp]
        public void TestSetup()
        {
            _playerCruiser = Substitute.For<ICruiser>();
            _aiCruiser = Substitute.For<ICruiser>();

            _monitor = new CruiserDestroyedMonitor(_playerCruiser, _aiCruiser);

            _destroyedEventCount = 0;
            _monitor.CruiserDestroyed += (sender, e) => _destroyedEventCount++;
        }

        [Test]
        public void PlayerCruiserDestroyed()
        {
            _playerCruiser.Destroyed += Raise.EventWith(new DestroyedEventArgs(_playerCruiser));
            Assert.AreEqual(1, _destroyedEventCount);
        }

        [Test]
        public void AICruiserDestroyed()
        {
            _aiCruiser.Destroyed += Raise.EventWith(new DestroyedEventArgs(_aiCruiser));
            Assert.AreEqual(1, _destroyedEventCount);
        }

        private void CheckUnsubscribed()
        {
            _destroyedEventCount = 0;

            _playerCruiser.Destroyed += Raise.EventWith(new DestroyedEventArgs(_playerCruiser));
            _aiCruiser.Destroyed += Raise.EventWith(new DestroyedEventArgs(_aiCruiser));

            Assert.AreEqual(0, _destroyedEventCount);
        }
    }
}