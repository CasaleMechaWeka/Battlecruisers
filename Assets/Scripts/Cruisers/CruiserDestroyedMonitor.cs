using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;

namespace BattleCruisers.Cruisers
{
    // FELIX  Test :)
    public class CruiserDestroyedMonitor
    {
        private readonly ICruiser _playerCruiser, _aiCruiser;
        private readonly IBattleCompletionHandler _battleCompletionHandler;
        private readonly IPauseGameManager _pauseGameManager;

        public CruiserDestroyedMonitor(
            ICruiser playerCruiser,
            ICruiser aiCruiser,
            IBattleCompletionHandler battleCompletionHandler, 
            IPauseGameManager pauseGameManager)
        {
            Helper.AssertIsNotNull(playerCruiser, aiCruiser, battleCompletionHandler, pauseGameManager);

            _playerCruiser = playerCruiser;
            _aiCruiser = aiCruiser;
            _battleCompletionHandler = battleCompletionHandler;
            _pauseGameManager = pauseGameManager;

            _playerCruiser.Destroyed += _playerCruiser_Destroyed;
            _aiCruiser.Destroyed += _aiCruiser_Destroyed;
        }

        private void _playerCruiser_Destroyed(object sender, DestroyedEventArgs e)
        {
            _pauseGameManager.PauseGame();
            _battleCompletionHandler.CompleteBattle(wasVictory: false);
        }

        private void _aiCruiser_Destroyed(object sender, DestroyedEventArgs e)
        {
            _pauseGameManager.PauseGame();
            _battleCompletionHandler.CompleteBattle(wasVictory: true);
        }
    }
}