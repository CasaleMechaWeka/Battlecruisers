using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using NSubstitute;
using NUnit.Framework;
using UnityCommon.Properties;

namespace BattleCruisers.Tests.Cruisers.Drones.Feedback
{
    public class DroneSoundFeedbackTests
    {
        private IManagedDisposable _feedback;
        private IBroadcastingProperty<bool> _parentCruiserHasActiveDrones;
        private IAudioSource _audioSource;
        private ISettingsManager _settingsManager;

        [SetUp]
        public void TestSetup()
        {
            _parentCruiserHasActiveDrones = Substitute.For<IBroadcastingProperty<bool>>();
            _audioSource = Substitute.For<IAudioSource>();
            _settingsManager = Substitute.For<ISettingsManager>();

            _settingsManager.EffectVolume.Returns(0.72f);

            _feedback = new DroneSoundFeedback(_parentCruiserHasActiveDrones, _audioSource, _settingsManager);
        }

        [Test]
        public void InitialState()
        {
            _audioSource.Received().Volume = _settingsManager.EffectVolume;
        }

        [Test]
        public void _settingsManager_SettingsSaved()
        {
            _audioSource.ClearReceivedCalls();

            _settingsManager.SettingsSaved += Raise.Event();

            _audioSource.Received().Volume = _settingsManager.EffectVolume;
        }

        [Test]
        public void _parentCruiserHasActiveDrones_ValueChanged_True()
        {
            _parentCruiserHasActiveDrones.Value.Returns(true);
            _parentCruiserHasActiveDrones.ValueChanged += Raise.Event();

            _audioSource.Received().Play(isSpatial: true, loop: false);
        }

        [Test]
        public void _parentCruiserHasActiveDrones_ValueChanged_False()
        {
            _parentCruiserHasActiveDrones.Value.Returns(false);
            _parentCruiserHasActiveDrones.ValueChanged += Raise.Event();

            _audioSource.DidNotReceiveWithAnyArgs().Play(default, default);
        }

        [Test]
        public void DisposeManagedState()
        {
            _audioSource.ClearReceivedCalls();
            
            _feedback.DisposeManagedState();

            _parentCruiserHasActiveDrones.Value.Returns(true);
            _parentCruiserHasActiveDrones.ValueChanged += Raise.Event();

            _audioSource.DidNotReceiveWithAnyArgs().Play(default, default);

            _settingsManager.SettingsSaved += Raise.Event();
            _audioSource.DidNotReceiveWithAnyArgs().Volume = default;
        }
    }
}