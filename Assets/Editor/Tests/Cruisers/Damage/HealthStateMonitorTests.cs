using BattleCruisers.Buildables;
using BattleCruisers.Cruisers.Damage;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Cruisers.Damage
{
    public class HealthStateMonitorTests
    {
        private IHealthStateMonitor _healthStateMonitor;
        private IDamagable _damagable;
        private float _maxHealth, _damagedThreshold, _severelyDamagedThreashold;
        private int _healthStateChangedEventCount;

        [SetUp]
        public void TestSetup()
        {
            _damagable = Substitute.For<IDamagable>();
            _healthStateMonitor = new HealthStateMonitor(_damagable);

            _maxHealth = 1000;
            _damagedThreshold = HealthStateMonitor.DAMAGED_THRESHOLD * _maxHealth;
            _severelyDamagedThreashold = HealthStateMonitor.SEVERELY_DAMAGED_THRESHOLD * _maxHealth;

            _damagable.MaxHealth.Returns(1000);

            _healthStateChangedEventCount = 0;
            _healthStateMonitor.HealthStateChanged += (sender, e) => _healthStateChangedEventCount++;
        }

        [Test]
        public void InitialState()
        {
            Assert.AreEqual(HealthState.FullHealth, _healthStateMonitor.HealthState);
        }

        [Test]
        public void HealthChanged_SameHealthState_DoesNotEmitEvent()
        {
            ExpectHealthChange(newHealth: _maxHealth, expectStateChange: false, expectedState: HealthState.FullHealth);
        }

        [Test]
        public void FullHealth_ToSlightlyDamaged()
        {
            ExpectHealthChange(newHealth: _maxHealth - 1, expectStateChange: true, expectedState: HealthState.SlightlyDamaged);
        }

        [Test]
        public void SlightlyDamaged_ToDamaged()
        {
            ExpectHealthChange(newHealth: _maxHealth - 1, expectStateChange: true, expectedState: HealthState.SlightlyDamaged);
            ExpectHealthChange(newHealth: _damagedThreshold - 1, expectStateChange: true, expectedState: HealthState.Damaged);
        }

        [Test]
        public void Damaged_ToSeverelyDamaged()
        {
            ExpectHealthChange(newHealth: _damagedThreshold - 1, expectStateChange: true, expectedState: HealthState.Damaged);
            ExpectHealthChange(newHealth: _severelyDamagedThreashold - 1, expectStateChange: true, expectedState: HealthState.SeverelyDamaged);
        }

        [Test]
        public void SeverelyDamaged_ToDamaged()
        {
            ExpectHealthChange(newHealth: _severelyDamagedThreashold - 1, expectStateChange: true, expectedState: HealthState.SeverelyDamaged);
            ExpectHealthChange(newHealth: _severelyDamagedThreashold + 1, expectStateChange: true, expectedState: HealthState.Damaged);
        }

        [Test]
        public void Damaged_ToSlightlyDamaged()
        {
            ExpectHealthChange(newHealth: _damagedThreshold - 1, expectStateChange: true, expectedState: HealthState.Damaged);
            ExpectHealthChange(newHealth: _damagedThreshold + 1, expectStateChange: true, expectedState: HealthState.SlightlyDamaged);
        }

        [Test]
        public void SlightlyDamaged_ToFullHealth()
        {
            ExpectHealthChange(newHealth: _maxHealth - 1, expectStateChange: true, expectedState: HealthState.SlightlyDamaged);
            ExpectHealthChange(newHealth: _maxHealth, expectStateChange: true, expectedState: HealthState.FullHealth);
        }

        [Test]
        public void FullHealth_ToSeverelyDamaged()
        {
            ExpectHealthChange(newHealth: _severelyDamagedThreashold - 1, expectStateChange: true, expectedState: HealthState.SeverelyDamaged);
        }

        [Test]
        public void Destroyed_EmitsNoHealthState_Unsubscribes()
        {
            _damagable.Destroyed += Raise.EventWith(new DestroyedEventArgs(null));

            Assert.AreEqual(1, _healthStateChangedEventCount);
            Assert.AreEqual(HealthState.NoHealth, _healthStateMonitor.HealthState);

            // Subsequent events are ignored
            _damagable.Destroyed += Raise.EventWith(new DestroyedEventArgs(null));
            Assert.AreEqual(1, _healthStateChangedEventCount);

            _damagable.Health.Returns(1);
            _damagable.HealthChanged += Raise.Event();
            Assert.AreEqual(1, _healthStateChangedEventCount);
        }

        private void ExpectHealthChange(float newHealth, bool expectStateChange, HealthState expectedState)
        {
            int expectedEventCount = expectStateChange ? _healthStateChangedEventCount + 1: _healthStateChangedEventCount;
            _damagable.Health.Returns(newHealth);

            _damagable.HealthChanged += Raise.Event();

            Assert.AreEqual(expectedEventCount, _healthStateChangedEventCount);
            Assert.AreEqual(expectedState, _healthStateMonitor.HealthState);
        }
    }
}