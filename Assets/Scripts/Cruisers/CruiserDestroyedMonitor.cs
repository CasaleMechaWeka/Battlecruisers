using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Cruisers
{
    // FELIX  Update tests :)
    public class CruiserDestroyedMonitor
    {
        private readonly ICruiser _playerCruiser, _aiCruiser;
        private readonly IBattleCompletionHandler _battleCompletionHandler;
        private readonly IDeferrer _deferrer;

        private const float POST_GAME_WAIT_TIME_IN_S = 5;

        public CruiserDestroyedMonitor(
            ICruiser playerCruiser,
            ICruiser aiCruiser,
            IBattleCompletionHandler battleCompletionHandler,
            IDeferrer deferrer)
        {
            Helper.AssertIsNotNull(playerCruiser, aiCruiser, battleCompletionHandler, deferrer);

            _playerCruiser = playerCruiser;
            _aiCruiser = aiCruiser;
            _battleCompletionHandler = battleCompletionHandler;
            _deferrer = deferrer;

            _playerCruiser.Destroyed += _playerCruiser_Destroyed;
            _aiCruiser.Destroyed += _aiCruiser_Destroyed;
        }

        // FELIX:
        // + Make Victory Cruiser (VC) invincible (so in flight projectiles cannot destroy it)
        // + After watching sinking animation, go to post battle screen :)
        
        // + Destroy all Losing Cruiser (LC) buildables
        // + Auto navigate to LC, to watch sinking (and maybe nuke explosion) animation
        // + Handle VC unit movement
        //      + Ships => Stop them from moving :)
        // + Implement sinking animation :P
        private void _playerCruiser_Destroyed(object sender, DestroyedEventArgs e)
        {
            OnCruiserDestroyed(false, _aiCruiser, _playerCruiser);
        }

        private void _aiCruiser_Destroyed(object sender, DestroyedEventArgs e)
        {
            OnCruiserDestroyed(true, _playerCruiser, _aiCruiser);
        }

        private void OnCruiserDestroyed(bool wasVictory, ICruiser victoryCruiser, ICruiser losingCruiser)
        {
            victoryCruiser.MakeInvincible();

            _deferrer.Defer(() => _battleCompletionHandler.CompleteBattle(wasVictory), POST_GAME_WAIT_TIME_IN_S);
        }
    }
}