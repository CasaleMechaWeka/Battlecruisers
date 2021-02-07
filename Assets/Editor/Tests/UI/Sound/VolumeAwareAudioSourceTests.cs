using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Sound
{
    public class VolumeAwareAudioSourceTests
    {
        private VolumeAwareAudioSource _volumeAwareAudioSource;
        private IAudioSource _audioSource;
        private ISettingsManager _settingsManager;

        [SetUp]
        public void TestSetup()
        {
            _audioSource = Substitute.For<IAudioSource>();
            _settingsManager = Substitute.For<ISettingsManager>();
            _settingsManager.EffectVolume.Returns(0.33f);

            _volumeAwareAudioSource = new VolumeAwareAudioSource(_audioSource, _settingsManager);

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
        public void DisposeManagedState()
        {
            _volumeAwareAudioSource.DisposeManagedState();

            _audioSource.ClearReceivedCalls();
            _settingsManager.SettingsSaved += Raise.Event();
            _audioSource.DidNotReceiveWithAnyArgs().Volume = default;
        }
    }
}