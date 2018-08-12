using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Utils.BattleScene
{
    public class PauseGameManagerTests
    {
        private IPauseGameManager _manager;
        private ITime _time;
        private int _pausedGameCount, _resumedGameCount;

        [SetUp]
        public void TestSetup()
        {
            _time = Substitute.For<ITime>();
            _manager = new PauseGameManager(_time);

            _pausedGameCount = 0;
            _manager.GamePaused += (sender, e) => _pausedGameCount++;

            _resumedGameCount = 0;
            _manager.GameResumed += (sender, e) => _resumedGameCount++;
        }

        [Test]
        public void PauseGame_SetsTimeScale_EmitsEvent()
        {
            SetTimeScaleAsPlaying();

            _manager.PauseGame();

            Assert.AreEqual(1, _pausedGameCount);
            Assert.AreEqual(0, _time.TimeScale);
        }

        [Test]
        public void DoublePauseGame_DoesNothingTheSecondTime()
        {
            SetTimeScaleAsPlaying();

            // Pause game the first time pauses game
            _manager.PauseGame();

            Assert.AreEqual(1, _pausedGameCount);
            Assert.AreEqual(0, _time.TimeScale);

            // Pause game the second time does nothing
            _time.ClearReceivedCalls();

            _manager.PauseGame();

            Assert.AreEqual(1, _pausedGameCount);
            _time.DidNotReceiveWithAnyArgs().TimeScale = default(float);
        }

        [Test]
        public void ResumeGame_SetsTimeScale_EmitsEvent()
        {
            SetTimeScaleAsPaused();

            _manager.ResumeGame();

            Assert.AreEqual(1, _resumedGameCount);
            Assert.AreEqual(1, _time.TimeScale);
        }

        [Test]
        public void DoubleResumeGame_DoesNothingSecondTime()
        {
            SetTimeScaleAsPaused();

            // Resume game the first time resumes game
            _manager.ResumeGame();

            Assert.AreEqual(1, _resumedGameCount);
            Assert.AreEqual(1, _time.TimeScale);

            // Resume game the second time does nothing
            _time.ClearReceivedCalls();

            _manager.ResumeGame();

            Assert.AreEqual(1, _resumedGameCount);
            _time.DidNotReceiveWithAnyArgs().TimeScale = default(float);
        }

        private void SetTimeScaleAsPlaying()
        {
            _time.TimeScale = 1;
        }

        private void SetTimeScaleAsPaused()
        {
            _time.TimeScale = 0;
        }
    }
}