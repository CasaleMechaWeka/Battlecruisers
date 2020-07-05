using BattleCruisers.Buildables;
using BattleCruisers.Cruisers.Damage;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Cruisers.Damage
{
    public class HealthThresholdMonitorTests
    {
        private IHealthThresholdMonitor _monitor;
        private IDamagable _damagable;
        private float _thresholdHealth;
        private int _belowCount, _aboveCount;

        [SetUp]
        public void TestSetup()
        {
            _damagable = Substitute.For<IDamagable>();
            _damagable.Health.Returns(100);
            _damagable.MaxHealth.Returns(100);

            float thresholdProportion = 0.3f;
            _thresholdHealth = thresholdProportion * _damagable.MaxHealth;

            _monitor = new HealthThresholdMonitor(_damagable, thresholdProportion);

            _belowCount = 0;
            _monitor.DroppedBelowThreshold += (sender, e) => _belowCount++;

            _aboveCount = 0;
            _monitor.RoseAboveThreshold += (sender, e) => _aboveCount++;
        }

        [Test]
        public void HealthChange_AboveThreshold_DoesNotEmitEvent()
        {
            _damagable.Health.Returns(_thresholdHealth + 1);
            _damagable.HealthChanged += Raise.Event();
            Assert.AreEqual(0, _belowCount);
            Assert.AreEqual(0, _aboveCount);
        }

        [Test]
        public void HealthChange_BelowThresholdFirstTime_EmitsEvent()
        {
            _damagable.Health.Returns(_thresholdHealth - 1);
            _damagable.HealthChanged += Raise.Event();
            Assert.AreEqual(1, _belowCount);
            Assert.AreEqual(0, _aboveCount);
        }


        [Test]
        public void HealthChange_BelowThresholdSecondTime_DoesNotEmitEvent()
        {
            // Go below threshold
            _damagable.Health.Returns(_thresholdHealth - 1);
            _damagable.HealthChanged += Raise.Event();
            Assert.AreEqual(1, _belowCount);
            Assert.AreEqual(0, _aboveCount);

            // Go even further below threshold
            _damagable.Health.Returns(_thresholdHealth - 2);
            _damagable.HealthChanged += Raise.Event();
            Assert.AreEqual(1, _belowCount);
            Assert.AreEqual(0, _aboveCount);
        }

        [Test]
        public void HealthChange_BelowThreshold_AfterRegainingThreshold_EmitsEvent()
        {
            // Go below threshold
            _damagable.Health.Returns(_thresholdHealth - 1);
            _damagable.HealthChanged += Raise.Event();
            Assert.AreEqual(1, _belowCount);
            Assert.AreEqual(0, _aboveCount);

            // Go above threshold
            _damagable.Health.Returns(_thresholdHealth + 1);
            _damagable.HealthChanged += Raise.Event();
            Assert.AreEqual(1, _belowCount);
            Assert.AreEqual(1, _aboveCount);

            // Go below threshold again
            _damagable.Health.Returns(_thresholdHealth - 1);
            _damagable.HealthChanged += Raise.Event();
            Assert.AreEqual(2, _belowCount);
            Assert.AreEqual(1, _aboveCount);
        }
    }
}