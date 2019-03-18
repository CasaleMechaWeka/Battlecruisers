using BattleCruisers.UI.Sound.ProjectileSpawners;
using BattleCruisers.Utils.Threading;
using NSubstitute;
using NUnit.Framework;
using System;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.UI.Sound.ProjectileSpawners
{
    public class LongSoundPlayerTests : ProjectileSpawnerSoundPlayerTestsBase
    {
        private IDeferrer _deferrer;
        private int _burstSize;
        private float _burstEndDelayInS;

        [SetUp]
        public override void TestSetup()
        {
            base.TestSetup();

            _deferrer = Substitute.For<IDeferrer>();
            _burstSize = 3;
            _burstEndDelayInS = 99.32f;

            _soundPlayer = new LongSoundPlayer(_audioClip, _audioSource, _deferrer, _burstSize, _burstEndDelayInS);

            UnityAsserts.Assert.raiseExceptions = true;
        }

        [Test]
        public void Constructor_InvalidBurstSize()
        {
            int invalidBurstSize = 1;
            Assert.Throws<UnityAsserts.AssertionException>(() => new LongSoundPlayer(_audioClip, _audioSource, _deferrer, invalidBurstSize, _burstEndDelayInS));
        }

        [Test]
        public void OnProjectileFired()
        {
            int numOfBursts = 3;

            for (int i = 0; i < numOfBursts; ++i)
            {
                // Start of burst
                _soundPlayer.OnProjectileFired();
                _audioSource.Received().Play();

                // During burst
                _audioSource.ClearReceivedCalls();
                _soundPlayer.OnProjectileFired();
                _audioSource.DidNotReceiveWithAnyArgs().Play();
                _audioSource.DidNotReceive().Stop();

                // After burst
                _soundPlayer.OnProjectileFired();
                _deferrer
                    .Received()
                    .Defer(
                        Arg.Is<Action>(callback => InvokeCallback(callback)),
                        _burstEndDelayInS);
                _audioSource.Received().Stop();
            }
        }

        private bool InvokeCallback(Action callback)
        {
            callback.Invoke();
            return true;
        }
    }
}
