using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Sound
{
    public class AudioSourceGroupTests
    {
        private AudioSourceGroup _audioSourceGroup;
        private IAudioSource _audioSource1, _audioSource2;
        private ISettingsManager _settingsManager;

        [SetUp]
        public void TestSetup()
        {
            _audioSource1 = Substitute.For<IAudioSource>();
            _audioSource2 = Substitute.For<IAudioSource>();
            _settingsManager = Substitute.For<ISettingsManager>();
            _settingsManager.EffectVolume.Returns(0.33f);

            _audioSourceGroup = new AudioSourceGroup(_settingsManager, _audioSource1, _audioSource2);

            _audioSource1.Received().Volume = _settingsManager.EffectVolume;
            _audioSource2.Received().Volume = _settingsManager.EffectVolume;
        }

        [Test]
        public void _settingsManager_SettingsSaved()
        {
            _audioSource1.ClearReceivedCalls();
            _audioSource2.ClearReceivedCalls();

            _settingsManager.SettingsSaved += Raise.Event();

            _audioSource1.Received().Volume = _settingsManager.EffectVolume;
            _audioSource2.Received().Volume = _settingsManager.EffectVolume;
        }

        [Test]
        public void DisposeManagedState()
        {
            _audioSourceGroup.DisposeManagedState();

            _audioSource1.ClearReceivedCalls();
            _audioSource2.ClearReceivedCalls();
            
            _settingsManager.SettingsSaved += Raise.Event();
            
            _audioSource1.DidNotReceiveWithAnyArgs().Volume = default;
            _audioSource2.DidNotReceiveWithAnyArgs().Volume = default;
        }
    }
}