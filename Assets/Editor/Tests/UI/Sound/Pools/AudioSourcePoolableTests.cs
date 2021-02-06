using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Sound.Pools;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Utils.Threading;
using NSubstitute;
using NUnit.Framework;
using System;
using UnityEngine;

namespace BattleCruisers.Tests.UI.Sound.Pools
{
    public class AudioSourcePoolableTests
    {
        private IAudioSourcePoolable _poolable;
        private IAudioSource _source;
        private IDeferrer _realTimeDeferrer;
        private ISettingsManager _settingsManager;
        private int _deactivatedCount;
        private float _soundLengthInS = 3.14f;
        private Action _deferredAction;
        private IAudioClipWrapper _sound;

        [SetUp]
        public void TestSetup()
        {
            _source = Substitute.For<IAudioSource>();
            _realTimeDeferrer = Substitute.For<IDeferrer>();
            // FELIX  Update tests :P
            _settingsManager = Substitute.For<ISettingsManager>();

            _poolable = new AudioSourcePoolable(_source, _realTimeDeferrer, _settingsManager);

            _sound = Substitute.For<IAudioClipWrapper>();
            _sound.Length.Returns(_soundLengthInS);

            _realTimeDeferrer.Defer(Arg.Do<Action>(action => _deferredAction = action), _soundLengthInS);

            _deactivatedCount = 0;
            _poolable.Deactivated += (sender, e) => _deactivatedCount++;
        }

        [Test]
        public void InitialState()
        {
            _source.Received().IsActive = false;
        }

        [Test]
        public void Activate()
        {
            _source.ClearReceivedCalls();
            AudioSourceActivationArgs activationArgs = new AudioSourceActivationArgs(_sound, new Vector2(1, 2));

            _poolable.Activate(activationArgs);

            _source.Received().IsActive = true;
            _source.Received().AudioClip = _sound;
            _source.Received().Position = activationArgs.Position;
            _source.Received().Play();
            Assert.IsNotNull(_deferredAction);
        }

        [Test]
        public void AudioClipFinishes()
        {
            Activate();
            _source.ClearReceivedCalls();

            _deferredAction.Invoke();

            _source.Received().IsActive = false;
            Assert.AreEqual(1, _deactivatedCount);
        }
    }
}