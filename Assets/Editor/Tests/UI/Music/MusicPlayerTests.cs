using BattleCruisers.Data.Static;
using BattleCruisers.UI.Music;
using BattleCruisers.UI.Sound;
using NSubstitute;
using NUnit.Framework;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.UI.Music
{
    public class MusicPlayerTests
    {
        private IMusicPlayer _musicPlayer;
        private ISingleSoundPlayer _soundPlayer;

        [SetUp]
        public void TestSetup()
        {
            _soundPlayer = Substitute.For<ISingleSoundPlayer>();
            _musicPlayer = new MusicPlayer(_soundPlayer);

            UnityAsserts.Assert.raiseExceptions = true;
        }

        [Test]
        public void PlayScreensSceneMusic()
        {
            _musicPlayer.PlayScreensSceneMusic();
            _soundPlayer.Received().PlaySound(SoundKeys.Music.MainTheme, loop: true);
        }

        [Test]
        public void PlayVictoryMusic()
        {
            _musicPlayer.PlayVictoryMusic();
            _soundPlayer.Received().PlaySound(SoundKeys.Music.Victory, loop: false);
        }

        [Test]
        public void PlayCurrentlyPlayingMusic_DoesNothing()
        {
            // First time playing music
            _musicPlayer.PlayScreensSceneMusic();
            _soundPlayer.Received().PlaySound(SoundKeys.Music.MainTheme, loop: true);

            // Second time playing same music
            _soundPlayer.ClearReceivedCalls();
            _musicPlayer.PlayScreensSceneMusic();
            _soundPlayer.DidNotReceiveWithAnyArgs().PlaySound(null, default);
        }

        [Test]
        public void Stop()
        {
            _musicPlayer.Stop();
            _soundPlayer.Received().Stop();
        }
    }
}