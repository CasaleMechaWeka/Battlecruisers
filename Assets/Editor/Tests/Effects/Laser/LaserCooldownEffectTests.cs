using BattleCruisers.Effects.Laser;
using BattleCruisers.Effects.ParticleSystems;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Timers;
using NSubstitute;
using NUnit.Framework;
using System;
using UnityCommon.Properties;

namespace BattleCruisers.Tests.Effects.Laser
{
    public class LaserCooldownEffectTests
    {
        private IManagedDisposable _laserCooldownEffect;
        private IBroadcastingProperty<bool> _isLaserFiring;
        private ILaserFlap _laserFlap;
        private IParticleSystemGroup _overheatingSmoke;
        private IDebouncer _laserStoppdDebouncer;
        private Action _debouncedAction;

        [SetUp]
        public void TestSetup()
        {
            _isLaserFiring = Substitute.For<IBroadcastingProperty<bool>>();
            _laserFlap = Substitute.For<ILaserFlap>();
            _overheatingSmoke = Substitute.For<IParticleSystemGroup>();
            _laserStoppdDebouncer = Substitute.For<IDebouncer>();

            _laserCooldownEffect
                = new LaserCooldownEffect(
                    _isLaserFiring,
                    _laserFlap,
                    _overheatingSmoke,
                    _laserStoppdDebouncer);

            _laserStoppdDebouncer.Debounce(Arg.Do<Action>(x => _debouncedAction = x));
        }

        [Test]
        public void IsLaserFiring_True()
        {
            // False => True
            _isLaserFiring.Value.Returns(true);
            _isLaserFiring.ValueChanged += Raise.Event();

            _laserFlap.Received().CloseFlap();

            // True => True
            _laserFlap.ClearReceivedCalls();
            _isLaserFiring.ValueChanged += Raise.Event();
            _laserFlap.DidNotReceive().CloseFlap();
        }

        [Test]
        public void IsLaserFiring_False()
        {
            // False => False
            _isLaserFiring.Value.Returns(false);
            _isLaserFiring.ValueChanged += Raise.Event();

            Assert.IsNull(_debouncedAction);

            // False => True
            _isLaserFiring.Value.Returns(true);
            _isLaserFiring.ValueChanged += Raise.Event();
            _laserFlap.ClearReceivedCalls();

            // True => False
            _isLaserFiring.Value.Returns(false);
            _isLaserFiring.ValueChanged += Raise.Event();

            Assert.IsNotNull(_debouncedAction);
            _laserFlap.DidNotReceive().OpenFlap();
            _overheatingSmoke.DidNotReceive().Play();

            _debouncedAction.Invoke();
            _laserFlap.Received().OpenFlap();
            _overheatingSmoke.Received().Play();
        }
    }
}