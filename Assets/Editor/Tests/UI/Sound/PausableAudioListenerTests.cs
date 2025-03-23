using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.UI.Sound
{
    public class PausableAudioListenerTests
    {
        private PausableAudioListener _pausableAudioListener;
        private AudioListenerBC _audioListener;
        private PauseGameManager _pauseGameManager;

        [SetUp]
        public void TestSetup()
        {
            _audioListener = Substitute.For<AudioListenerBC>();
            _pauseGameManager = Substitute.For<PauseGameManager>();

            _pausableAudioListener
                = new PausableAudioListener(
                    _audioListener,
                    _pauseGameManager);
        }

        [Test]
        public void PauseGameManager_GamePaused()
        {
            _pauseGameManager.GamePaused += Raise.Event();
            _audioListener.Received().Pause();
        }

        [Test]
        public void PauseGameManager_GameResumed()
        {
            _pauseGameManager.GameResumed += Raise.Event();
            _audioListener.Received().Resume();
        }
    }
}