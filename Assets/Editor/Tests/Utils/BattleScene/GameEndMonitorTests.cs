using BattleCruisers.Cruisers;
using BattleCruisers.Utils.BattleScene;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Utils.BattleScene
{
    public class GameEndMonitorTests
    {
        private IGameEndMonitor _gameEndMonitor;
        private ICruiserDestroyedMonitor _cruiserDestroyedMonitor;
        private IBattleCompletionHandler _battleCompletionHandler;
        private IGameEndHandler _gameEndHandler;
        private int _gameEndedCount;

        [SetUp]
        public void TestSetup()
        {
            _cruiserDestroyedMonitor = Substitute.For<ICruiserDestroyedMonitor>();
            _battleCompletionHandler = Substitute.For<IBattleCompletionHandler>();
            _gameEndHandler = Substitute.For<IGameEndHandler>();

            _gameEndMonitor = new GameEndMonitor(_cruiserDestroyedMonitor, _battleCompletionHandler, _gameEndHandler);

            _gameEndedCount = 0;
            _gameEndMonitor.GameEnded += (sender, e) => _gameEndedCount++;
        }

        [Test]
        public void CruiserDestroyed()
        {
            bool wasPlayerVictory = true;
            _cruiserDestroyedMonitor.CruiserDestroyed += Raise.EventWith(new CruiserDestroyedEventArgs(wasPlayerVictory));

            Assert.AreEqual(1, _gameEndedCount);
            _gameEndHandler.Received().HandleCruiserDestroyed(wasPlayerVictory);

            // Check destroyed event has been unsuscribed
            _cruiserDestroyedMonitor.CruiserDestroyed += Raise.EventWith(new CruiserDestroyedEventArgs(wasPlayerVictory));
            Assert.AreEqual(1, _gameEndedCount);

            // Check battle completed event has not been unsubscribed
            _battleCompletionHandler.BattleCompleted += Raise.Event();
            Assert.AreEqual(2, _gameEndedCount);
        }

        [Test]
        public void BattleCompleted()
        {
            _battleCompletionHandler.BattleCompleted += Raise.Event();

            Assert.AreEqual(1, _gameEndedCount);
            _gameEndHandler.Received().HandleGameEnd();

            // Check destroyed event has been unsuscribed
            _cruiserDestroyedMonitor.CruiserDestroyed += Raise.EventWith(new CruiserDestroyedEventArgs(default));
            Assert.AreEqual(1, _gameEndedCount);

            // Check battle completed event has been unsubscribed
            _battleCompletionHandler.BattleCompleted += Raise.Event();
            Assert.AreEqual(1, _gameEndedCount);
        }
    }
}