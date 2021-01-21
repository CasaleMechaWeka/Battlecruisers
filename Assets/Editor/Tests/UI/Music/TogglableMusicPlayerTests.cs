using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Music;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Music
{
    public class TogglableMusicPlayerTests
    {
        private IMusicPlayer _togglablePlayer, _corePlayer;
        private ISettingsManager _settings;

        [SetUp]
        public void TestSetup()
        {
            _corePlayer = Substitute.For<IMusicPlayer>();
            _settings = Substitute.For<ISettingsManager>();

            _togglablePlayer = new TogglableMusicPlayer(_corePlayer, _settings);
        }

        [Test]
        public void PlayScreensSceneMusic_MusicMuted()
        {
            _settings.MuteMusic.Returns(true);
            _togglablePlayer.PlayScreensSceneMusic();
            _corePlayer.DidNotReceive().PlayScreensSceneMusic();
        }

        [Test]
        public void PlayScreensSceneMusic_MusicNotMuted()
        {
            _settings.MuteMusic.Returns(false);
            _togglablePlayer.PlayScreensSceneMusic();
            _corePlayer.Received().PlayScreensSceneMusic();
        }

        [Test]
        public void PlayVictoryMusic_MusicMuted()
        {
            _settings.MuteMusic.Returns(true);
            _togglablePlayer.PlayVictoryMusic();
            _corePlayer.DidNotReceive().PlayVictoryMusic();
        }

        [Test]
        public void PlayVictoryMusic_MusicNotMuted()
        {
            _settings.MuteMusic.Returns(false);
            _togglablePlayer.PlayVictoryMusic();
            _corePlayer.Received().PlayVictoryMusic();
        }

        [Test]
        public void PlayDefeatMusic_MusicMuted()
        {
            _settings.MuteMusic.Returns(true);
            _togglablePlayer.PlayDefeatMusic();
            _corePlayer.DidNotReceive().PlayDefeatMusic();
        }

        [Test]
        public void PlayDefeatMusic_MusicNotMuted()
        {
            _settings.MuteMusic.Returns(false);
            _togglablePlayer.PlayDefeatMusic();
            _corePlayer.Received().PlayDefeatMusic();
        }

        [Test]
        public void Stop()
        {
            _togglablePlayer.Stop();
            _corePlayer.Received().Stop();
        }
    }
}