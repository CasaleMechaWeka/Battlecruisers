using BattleCruisers.UI.Music;
using BattleCruisers.UI.Sound;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Music
{
    public class MusicPlayerTests
    {
        private IMusicPlayer _musicPlayer;
        private ISingleSoundPlayer _soundPlayer;
        private ISoundKey _soundToPlay;

        [SetUp]
        public void TestSetup()
        {
            _soundPlayer = Substitute.For<ISingleSoundPlayer>();
            _musicPlayer = new MusicPlayer(_soundPlayer);

            _soundToPlay = Substitute.For<ISoundKey>();
        }

        [Test]
        public void PlayScreensSceneMusic_PlaysCorrectMusic()
        {
            _musicPlayer.PlayScreensSceneMusic();
            _soundPlayer.Received().PlaySound(_soundToPlay, loop: true);
        }

        [Test]
        public void PlayBattleSceneMusic_PlaysCorrectMusic()
        {
            _musicPlayer.PlayBattleSceneMusic();
            _soundPlayer.Received().PlaySound(_soundToPlay, loop: true);
        }

        [Test]
        public void PlayDangerMusic_PlaysCorrectMusic()
        {
            _musicPlayer.PlayDangerMusic();
            _soundPlayer.Received().PlaySound(_soundToPlay, loop: true);
        }

        [Test]
        public void PlayVictoryMusic_PlaysCorrectMusic()
        {
            _musicPlayer.PlayVictoryMusic();
            _soundPlayer.Received().PlaySound(_soundToPlay, loop: true);
        }

        [Test]
        public void PlayCurrentlyPlayingMusic_DoesNothing()
        {
            // First time playing music
            _musicPlayer.PlayScreensSceneMusic();
            _soundPlayer.Received().PlaySound(_soundToPlay, loop: true);

            // Second time playing same music
            _soundPlayer.ClearReceivedCalls();
            _musicPlayer.PlayScreensSceneMusic();
            _soundPlayer.DidNotReceiveWithAnyArgs().PlaySound(null, default(bool));
        }
    }
}