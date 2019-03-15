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
        private readonly IVariableDelayDeferrer _deferrer;

        private const float POST_GAME_WAIT_TIME_IN_S = 5;

        public CruiserDestroyedMonitor(
            ICruiser playerCruiser,
            ICruiser aiCruiser,
            IBattleCompletionHandler battleCompletionHandler,
            IVariableDelayDeferrer deferrer)
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
        // + [Destroy all Losing Cruiser (LC) buildables]
        // + Make Victory Cruiser (VC) invincible (so in flight projectiles cannot destroy it)
        // + Auto navigate to LC, to watch sinking (and maybe nuke explosion) animation
        // + Stop global targetting
        //      + Bombers?
        //      + Offensives?
        // + Handle VC unit movement
        //      + Bombers?
        //      + Ships
        // + After watching sinking animation, go to post battle screen :)
        private void _playerCruiser_Destroyed(object sender, DestroyedEventArgs e)
        {
            _deferrer.Defer(() => _battleCompletionHandler.CompleteBattle(wasVictory: false), POST_GAME_WAIT_TIME_IN_S);
        }

        private void _aiCruiser_Destroyed(object sender, DestroyedEventArgs e)
        {
            _deferrer.Defer(() => _battleCompletionHandler.CompleteBattle(wasVictory: true), POST_GAME_WAIT_TIME_IN_S);
        }
    }
}