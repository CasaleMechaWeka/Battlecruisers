using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Music;
using BattleCruisers.Utils.Audio;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.UI.Music
{
    public class LayeredMusicPlayerTest
    {
        private ILayeredMusicPlayer _musicPlayer;
        private IAudioVolumeFade _audioVolumeFade;
        private IAudioSource _primarySource, _secondarySource;
        private ISettingsManager _settingsManager;

        [SetUp]
        public void TestSetup()
        {
            _audioVolumeFade = Substitute.For<IAudioVolumeFade>();
            _primarySource = Substitute.For<IAudioSource>();
            _secondarySource = Substitute.For<IAudioSource>();
            _settingsManager = Substitute.For<ISettingsManager>();

            _musicPlayer = new LayeredMusicPlayer(_audioVolumeFade, _primarySource, _secondarySource, _settingsManager);

            _settingsManager.MusicVolume.Returns(1.25f);
        }

        [Test]
        public void Play_WhileDisposed_Throws()
        {
            _musicPlayer.DisposeManagedState();
            Assert.Throws<UnityAsserts.AssertionException>(() => _musicPlayer.Play());
        }

        [Test]
        public void Play()
        {
            _primarySource.IsPlaying.Returns(false);
            _musicPlayer.Play();

            _primarySource.Received().Volume = _settingsManager.MusicVolume;
            _primarySource.Received().Play(isSpatial: false, loop: true);

            _secondarySource.Received().Volume = 0;
            _secondarySource.Received().Play(isSpatial: false, loop: true);
        }

        [Test]
        public void Play_AlreadyPlaying()
        {
            _primarySource.IsPlaying.Returns(true);
            _musicPlayer.Play();
            _primarySource.DidNotReceiveWithAnyArgs().Volume = default;
        }

        [Test]
        public void PlaySecondary_WhileDisposed_Throws()
        {
            _musicPlayer.DisposeManagedState();
            Assert.Throws<UnityAsserts.AssertionException>(() => _musicPlayer.PlaySecondary());
        }

        [Test]
        public void PlaySecondary()
        {
            _musicPlayer.PlaySecondary();
            _audioVolumeFade.Received().FadeToVolume(_secondarySource, targetVolume: _settingsManager.MusicVolume, LayeredMusicPlayer.FADE_TIME_IN_S);
        }

        [Test]
        public void StopSecondary_WhileDisposed_Throws()
        {
            _musicPlayer.DisposeManagedState();
            Assert.Throws<UnityAsserts.AssertionException>(() => _musicPlayer.StopSecondary());
        }

        [Test]
        public void StopSecondary()
        {
            _musicPlayer.StopSecondary();
            _audioVolumeFade.Received().FadeToVolume(_secondarySource, targetVolume: 0, LayeredMusicPlayer.FADE_TIME_IN_S);
        }

        [Test]
        public void Stop_WhileDisposed_Throws()
        {
            _musicPlayer.DisposeManagedState();
            Assert.Throws<UnityAsserts.AssertionException>(() => _musicPlayer.Stop());
        }

        [Test]
        public void Stop_NotPlaying()
        {
            _primarySource.IsPlaying.Returns(false);
            _musicPlayer.Stop();
            _primarySource.DidNotReceive().Stop();
        }

        [Test]
        public void Stop()
        {
            _primarySource.IsPlaying.Returns(true);
            _musicPlayer.Stop();

            _primarySource.Received().Stop();
            _secondarySource.Received().Stop();
        }

        [Test]
        public void _settingsManager_SettingsSaved_WhilePlayingSecondary()
        {
            _musicPlayer.PlaySecondary();

            _settingsManager.SettingsSaved += Raise.Event();

            _audioVolumeFade.Received().Stop();
            _primarySource.Received().Volume = _settingsManager.MusicVolume;
            _secondarySource.Received().Volume = _settingsManager.MusicVolume;
        }

        [Test]
        public void _settingsManager_SettingsSaved_WhileNotPlayingSecondary()
        {
            _settingsManager.SettingsSaved += Raise.Event();

            _audioVolumeFade.Received().Stop();
            _primarySource.Received().Volume = _settingsManager.MusicVolume;
            _secondarySource.Received().Volume = 0;
        }

        [Test]
        public void DisposeManagedState()
        {
            _musicPlayer.DisposeManagedState();

            _primarySource.Received().FreeAudioClip();
            _secondarySource.Received().FreeAudioClip();

            _settingsManager.SettingsSaved += Raise.Event();
            _audioVolumeFade.DidNotReceive().Stop();
        }

        [Test]
        public void DoubleDisposeManagedState()
        {
            // First dispose
            _musicPlayer.DisposeManagedState();

            _primarySource.Received().FreeAudioClip();
            _primarySource.ClearReceivedCalls();
            _secondarySource.Received().FreeAudioClip();
            _secondarySource.ClearReceivedCalls();

            // Second dispose
            _musicPlayer.DisposeManagedState();

            _primarySource.DidNotReceive().FreeAudioClip();
            _secondarySource.DidNotReceive().FreeAudioClip();
        }
    }
}