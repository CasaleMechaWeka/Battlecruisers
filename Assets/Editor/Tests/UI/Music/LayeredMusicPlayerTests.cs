using BattleCruisers.UI.Music;
using BattleCruisers.Utils.Audio;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Music
{
    public class LayeredMusicPlayerTest
    {
        private ILayeredMusicPlayer _musicPlayer;
        private IAudioVolumeFade _audioVolumeFade;
        private IAudioSource _primarySource, _secondarySource;

        [SetUp]
        public void TestSetup()
        {
            _audioVolumeFade = Substitute.For<IAudioVolumeFade>();
            _primarySource = Substitute.For<IAudioSource>();
            _secondarySource = Substitute.For<IAudioSource>();

            _musicPlayer = new LayeredMusicPlayer(_audioVolumeFade, _primarySource, _secondarySource);
        }

        [Test]
        public void Play()
        {
            _primarySource.IsPlaying.Returns(false);
            _musicPlayer.Play();

            _primarySource.Received().Volume = LayeredMusicPlayer.MAX_VOLUME;
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
        public void PlaySecondary()
        {
            _musicPlayer.PlaySecondary();
            _audioVolumeFade.Received().FadeToVolume(_secondarySource, targetVolume: LayeredMusicPlayer.MAX_VOLUME, LayeredMusicPlayer.FADE_TIME_IN_S);
        }

        [Test]
        public void StopSecondary()
        {
            _musicPlayer.StopSecondary();
            _audioVolumeFade.Received().FadeToVolume(_secondarySource, targetVolume: 0, LayeredMusicPlayer.FADE_TIME_IN_S);
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
    }
}