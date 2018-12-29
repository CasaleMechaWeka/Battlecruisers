using BattleCruisers.Cruisers.Damage;
using BattleCruisers.Effects.Smoke;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Effects.Smoke
{
    public class SmokeEmitterTests
    {
        // Keep reference to avoid garbage collection
#pragma warning disable CS0414  // Variable is assigned but never used
        private SmokeEmitter _smokeEmitter;
#pragma warning restore CS0414  // Variable is assigned but never used

        private IHealthStateMonitor _healthStateMonitor;
        private ISmoke _smoke;

        [SetUp]
        public void TestSetup()
        {
            _healthStateMonitor = Substitute.For<IHealthStateMonitor>();
            _smoke = Substitute.For<ISmoke>();

            // FELIX  Fix :P
            _smokeEmitter = new SmokeEmitter(_healthStateMonitor, _smoke, showSmokeWhenDestroyed: true);
        }

        [Test]
        public void HealthStateChanged_FullHealth_SetsNoSmokeStrength()
        {
            ExpectHealthStateChange(HealthState.FullHealth, SmokeStrength.None);
        }

        [Test]
        public void HealthStateChanged_SlightlyDamaged_SetsWeakSmokeStrength()
        {
            ExpectHealthStateChange(HealthState.SlightlyDamaged, SmokeStrength.Weak);
        }

        [Test]
        public void HealthStateChanged_Damaged_SetsNormalSmokeStrength()
        {
            ExpectHealthStateChange(HealthState.Damaged, SmokeStrength.Normal);
        }

        [Test]
        public void HealthStateChanged_Severely_SetsStrongSmokeStrength()
        {
            ExpectHealthStateChange(HealthState.SeverelyDamaged, SmokeStrength.Strong);
        }

        private void ExpectHealthStateChange(HealthState newHealthState, SmokeStrength expectedSmokeStrength)
        {
            _healthStateMonitor.HealthState.Returns(newHealthState);
            _healthStateMonitor.HealthStateChanged += Raise.Event();
            _smoke.Received().SmokeStrength = expectedSmokeStrength;
        }
    }
}