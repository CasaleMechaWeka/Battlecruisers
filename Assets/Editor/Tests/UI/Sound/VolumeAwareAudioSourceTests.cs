using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Sound
{
    public class DummyAudioSource : VolumeAwareAudioSource
    {
        public float StaticVolume = 0.33f;

        public DummyAudioSource(IAudioSource audioSource, ISettingsManager settingsManager) 
            : base(audioSource, settingsManager)
        {
        }

        protected override float GetVolume(ISettingsManager settingsManager)
        {
            return StaticVolume;
        }
    }

    public class VolumeAwareAudioSourceTests
    {
        private DummyAudioSource _volumeAwareAudioSource;
        private IAudioSource _audioSource;
        private ISettingsManager _settingsManager;

        [SetUp]
        public void TestSetup()
        {
            _audioSource = Substitute.For<IAudioSource>();
            _settingsManager = Substitute.For<ISettingsManager>();

            _volumeAwareAudioSource = new DummyAudioSource(_audioSource, _settingsManager);

            _audioSource.Received().Volume = _volumeAwareAudioSource.StaticVolume;
        }

        [Test]
        public void _settingsManager_SettingsSaved()
        {
            _audioSource.ClearReceivedCalls();

            _settingsManager.SettingsSaved += Raise.Event();

            _audioSource.Received().Volume = _volumeAwareAudioSource.StaticVolume;
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