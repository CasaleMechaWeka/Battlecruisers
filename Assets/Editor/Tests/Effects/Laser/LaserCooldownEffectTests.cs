using BattleCruisers.Buildables.Buildings.Turrets.BarrelControllers.FireInterval;
using BattleCruisers.Effects.Laser;
using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Utils;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Effects.Laser
{
    public class LaserCooldownEffectTests
    {
        private IManagedDisposable _laserCooldownEffect;
        private IFireIntervalManager _fireIntervalManager;
        private ILaserFlap _laserFlap;
        private IParticleSystemGroup _overheatingSmoke;

        [SetUp]
        public void TestSetup()
        {
            _fireIntervalManager = Substitute.For<IFireIntervalManager>();
            _laserFlap = Substitute.For<ILaserFlap>();
            _overheatingSmoke = Substitute.For<IParticleSystemGroup>();
        }

        [Test]
        public void Constructor_ShouldFire()
        {
            _fireIntervalManager.ShouldFire.Value.Returns(true);
            CreateLaserEffect();
            AssertPlayEffects(shouldFire: true);
        }

        [Test]
        public void Constructor_ShouldNotFire()
        {
            _fireIntervalManager.ShouldFire.Value.Returns(false);
            CreateLaserEffect();
            AssertPlayEffects(shouldFire: false);
        }

        [Test]
        public void ShouldFire_ValueChanged_ToTrue()
        {
            CreateLaserEffect();
            ClearReceivedCalls();

            _fireIntervalManager.ShouldFire.Value.Returns(true);
            _fireIntervalManager.ShouldFire.ValueChanged += Raise.Event();

            AssertPlayEffects(shouldFire: true);
        }

        [Test]
        public void ShouldFire_ValueChanged_ToFalse()
        {
            CreateLaserEffect();
            ClearReceivedCalls();

            _fireIntervalManager.ShouldFire.Value.Returns(false);
            _fireIntervalManager.ShouldFire.ValueChanged += Raise.Event();

            AssertPlayEffects(shouldFire: false);
        }

        [Test]
        public void DisposeManagedState()
        {
            CreateLaserEffect();
            ClearReceivedCalls();
            _laserCooldownEffect.DisposeManagedState();

            _fireIntervalManager.ShouldFire.Value.Returns(true);
            _fireIntervalManager.ShouldFire.ValueChanged += Raise.Event();

            _laserFlap.DidNotReceive().CloseFlap();
        }

        private void CreateLaserEffect()
        {
            // FELIX  Fix
            _laserCooldownEffect = new LaserCooldownEffect(null, _laserFlap, _overheatingSmoke, null);
        }

        private void AssertPlayEffects(bool shouldFire)
        {
            if (shouldFire)
            {
                _laserFlap.Received().CloseFlap();
            }
            else
            {
                _laserFlap.Received().OpenFlap();
                _overheatingSmoke.Received().Play();
            }
        }

        private void ClearReceivedCalls()
        {
            _laserFlap.ClearReceivedCalls();
            _overheatingSmoke.ClearReceivedCalls();
        }
    }
}