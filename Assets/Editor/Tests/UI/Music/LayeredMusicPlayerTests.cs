using BattleCruisers.UI.Music;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.UI.Music
{
    public class LayeredMusicPlayerTest
    {
        private ILayeredMusicPlayer _musicPlayer;
        private IAudioSource _primarySource, _secondarySource;

        [SetUp]
        public void TestSetup()
        {
            _primarySource = Substitute.For<IAudioSource>();
            _secondarySource = Substitute.For<IAudioSource>();

            _musicPlayer = new LayeredMusicPlayer(_primarySource, _secondarySource);

            UnityAsserts.Assert.raiseExceptions = true;
        }

        [Test]
        public void Play()
        {
            _primarySource.IsPlaying.Returns(false);
            _musicPlayer.Play();

            _primarySource.Received().Volume = 1;
            _primarySource.Received().Play(isSpatial: false, loop: true);

            _secondarySource.Received().Volume = 0;
            _secondarySource.Received().Play(isSpatial: false, loop: true);
        }

        [Test]
        public void Play_AlreadyPlaying_Throws()
        {
            _primarySource.IsPlaying.Returns(true);
            Assert.Throws<UnityAsserts.AssertionException>(() => _musicPlayer.Play());
        }

        [Test]
        public void PlaySecondary()
        {
            _musicPlayer.PlaySecondary();
            _secondarySource.Received().Volume = 1;
        }

        [Test]
        public void StopSecondary()
        {
            _musicPlayer.StopSecondary();
            _secondarySource.Received().Volume = 0;
        }

        [Test]
        public void Stop_NotPlaying_Throws()
        {
            _primarySource.IsPlaying.Returns(false);
            Assert.Throws<UnityAsserts.AssertionException>(() => _musicPlayer.Stop());
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