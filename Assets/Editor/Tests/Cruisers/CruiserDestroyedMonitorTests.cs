using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Utils.BattleScene;
using NSubstitute;
using NUnit.Framework;

namespace BattleCruisers.Tests.Cruisers
{
    // FELIX  Update test :)
    public class CruiserDestroyedMonitorTests
    {
        private CruiserDestroyedMonitor _monitor;
        private ICruiser _playerCruiser, _aiCruiser;
        private IBattleCompletionHandler _battleCompletionHandler;
        private IPauseGameManager _pauseGameManager;

        [SetUp]
        public void TestSetup()
        {
            _playerCruiser = Substitute.For<ICruiser>();
            _aiCruiser = Substitute.For<ICruiser>();
            _battleCompletionHandler = Substitute.For<IBattleCompletionHandler>();
            _pauseGameManager = Substitute.For<IPauseGameManager>();

            //_monitor = new CruiserDestroyedMonitor(_playerCruiser, _aiCruiser, _battleCompletionHandler, _pauseGameManager);
        }

        [Test]
        public void PlayerCruiserDestroyed()
        {
            _playerCruiser.Destroyed += Raise.EventWith(new DestroyedEventArgs(_playerCruiser));

            _pauseGameManager.Received().PauseGame();
            _battleCompletionHandler.Received().CompleteBattle(wasVictory: false);
        }

        [Test]
        public void AICruiserDestroyed()
        {
            _aiCruiser.Destroyed += Raise.EventWith(new DestroyedEventArgs(_aiCruiser));

            _pauseGameManager.Received().PauseGame();
            _battleCompletionHandler.Received().CompleteBattle(wasVictory: true);
        }
    }
}