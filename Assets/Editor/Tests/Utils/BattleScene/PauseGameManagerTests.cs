using BattleCruisers.Utils.BattleScene;
using NSubstitute;
using NUnit.Framework;
using BattleCruisers.Utils.PlatformAbstractions.Time;

namespace BattleCruisers.Tests.Utils.BattleScene
{
    public class PauseGameManagerTests
    {
        private IPauseGameManager _manager;
        private ITime _time;
        private int _pausedGameCount, _resumedGameCount;
        private float _prePauseTimeScale;

        [SetUp]
        public void TestSetup()
        {
            _time = Substitute.For<ITime>();

            _manager = new PauseGameManager(_time);
            Assert.AreEqual(1, _time.TimeScale);

            _prePauseTimeScale = 4;
            _time.TimeScale = _prePauseTimeScale;

            _pausedGameCount = 0;
            _manager.GamePaused += (sender, e) => _pausedGameCount++;

            _resumedGameCount = 0;
            _manager.GameResumed += (sender, e) => _resumedGameCount++;
        }

        [Test]
        public void PauseGame_SetsTimeScale_EmitsEvent()
        {
            _manager.PauseGame();

            Assert.AreEqual(1, _pausedGameCount);
            Assert.AreEqual(0, _time.TimeScale);
        }

        [Test]
        public void DoublePauseGame_DoesNothingTheSecondTime()
        {
            // Pause game the first time pauses game
            _manager.PauseGame();

            Assert.AreEqual(1, _pausedGameCount);
            Assert.AreEqual(0, _time.TimeScale);

            // Pause game the second time does nothing
            _time.ClearReceivedCalls();

            _manager.PauseGame();

            Assert.AreEqual(1, _pausedGameCount);
            _time.DidNotReceiveWithAnyArgs().TimeScale = default;
        }

        [Test]
        public void ResumeGame_SetsPreviousTimeScale_EmitsEvent()
        {
            _manager.PauseGame();

            _manager.ResumeGame();

            Assert.AreEqual(1, _resumedGameCount);
            Assert.AreEqual(_prePauseTimeScale, _time.TimeScale);
        }

        [Test]
        public void DoubleResumeGame_DoesNothingSecondTime()
        {
            _manager.PauseGame();

            // Resume game the first time resumes game
            _manager.ResumeGame();

            Assert.AreEqual(1, _resumedGameCount);
            Assert.AreEqual(_prePauseTimeScale, _time.TimeScale);

            // Resume game the second time does nothing
            _time.ClearReceivedCalls();

            _manager.ResumeGame();

            Assert.AreEqual(1, _resumedGameCount);
            _time.DidNotReceiveWithAnyArgs().TimeScale = default;
        }
    }
}