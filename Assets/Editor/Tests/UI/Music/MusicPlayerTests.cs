using BattleCruisers.UI.Music;
using BattleCruisers.UI.Sound;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Music
{
    public class MusicPlayerTests
    {
        private IMusicPlayer _musicPlayer;
        private IMusicProvider _musicProvider;
        private ISingleSoundPlayer _soundPlayer;
        private ISoundKey _soundToPlay;

        [SetUp]
        public void TestSetup()
        {
            _musicProvider = Substitute.For<IMusicProvider>();
            _soundPlayer = Substitute.For<ISingleSoundPlayer>();
            _musicPlayer = new MusicPlayer(_musicProvider, _soundPlayer);

            _soundToPlay = Substitute.For<ISoundKey>();
        }

        [Test]
        public void PlayScreensSceneMusic_PlaysCorrectMusic()
        {
            _musicProvider.ScreensSceneKey.Returns(_soundToPlay);
            _musicPlayer.PlayScreensSceneMusic();
            _soundPlayer.Received().PlaySound(_soundToPlay, loop: true);
        }

        [Test]
        public void PlayBattleSceneMusic_PlaysCorrectMusic()
        {
            _musicProvider.BattleSceneKey.Returns(_soundToPlay);
            _musicPlayer.PlayBattleSceneMusic();
            _soundPlayer.Received().PlaySound(_soundToPlay, loop: true);
        }

        [Test]
        public void PlayDangerMusic_PlaysCorrectMusic()
        {
            _musicProvider.DangerKey.Returns(_soundToPlay);
            _musicPlayer.PlayDangerMusic();
            _soundPlayer.Received().PlaySound(_soundToPlay, loop: true);
        }

        [Test]
        public void PlayVictoryMusic_PlaysCorrectMusic()
        {
            _musicProvider.VictoryKey.Returns(_soundToPlay);
            _musicPlayer.PlayVictoryMusic();
            _soundPlayer.Received().PlaySound(_soundToPlay, loop: true);
        }

        [Test]
        public void PlayCurrentlyPlayingMusic_DoesNothing()
        {
            // First time playing music
            _musicProvider.ScreensSceneKey.Returns(_soundToPlay);
            _musicPlayer.PlayScreensSceneMusic();
            _soundPlayer.Received().PlaySound(_soundToPlay, loop: true);

            // Second time playing same music
            _soundPlayer.ClearReceivedCalls();
            _musicPlayer.PlayScreensSceneMusic();
            _soundPlayer.DidNotReceiveWithAnyArgs().PlaySound(null, default(bool));
        }
    }
}