using BattleCruisers.Cruisers;
using System;

namespace BattleCruisers.Utils.BattleScene
{
    /// <summary>
    /// The game can be ended in two ways:
    /// 1. A cruiser is destroyed
    /// 2. The user quits
    /// </summary>
    /// FELIX  Test
    public class GameEndMonitor : IGameEndMonitor
    {
        private readonly ICruiserDestroyedMonitor _cruiserDestroyedMonitor;
        private readonly IBattleCompletionHandler _battleCompletionHandler;

        public event EventHandler GameEnded;

        public GameEndMonitor(ICruiserDestroyedMonitor cruiserDestroyedMonitor, IBattleCompletionHandler battleCompletionHandler)
        {
            Helper.AssertIsNotNull(cruiserDestroyedMonitor, battleCompletionHandler);

            _cruiserDestroyedMonitor = cruiserDestroyedMonitor;
            _cruiserDestroyedMonitor.CruiserDestroyed += _cruiserDestroyedMonitor_CruiserDestroyed;

            _battleCompletionHandler = battleCompletionHandler;
            _battleCompletionHandler.BattleCompleted += _battleCompletionHandler_BattleCompleted;
        }

        private void _cruiserDestroyedMonitor_CruiserDestroyed(object sender, EventArgs e)
        {
            OnGameEnded();
        }

        private void _battleCompletionHandler_BattleCompleted(object sender, EventArgs e)
        {
            OnGameEnded();
        }

        private void OnGameEnded()
        {
            _cruiserDestroyedMonitor.CruiserDestroyed -= _cruiserDestroyedMonitor_CruiserDestroyed;
            _battleCompletionHandler.BattleCompleted -= _battleCompletionHandler_BattleCompleted;

            GameEnded?.Invoke(this, EventArgs.Empty);
        }
    }
}