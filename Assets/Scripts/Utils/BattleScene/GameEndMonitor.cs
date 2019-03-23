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
        private readonly IGameEndHandler _gameEndHandler;

        public event EventHandler GameEnded;

        public GameEndMonitor(
            ICruiserDestroyedMonitor cruiserDestroyedMonitor, 
            IBattleCompletionHandler battleCompletionHandler,
            IGameEndHandler gameEndHandler)
        {
            Helper.AssertIsNotNull(cruiserDestroyedMonitor, battleCompletionHandler, gameEndHandler);

            _cruiserDestroyedMonitor = cruiserDestroyedMonitor;
            _cruiserDestroyedMonitor.CruiserDestroyed += _cruiserDestroyedMonitor_CruiserDestroyed;

            _battleCompletionHandler = battleCompletionHandler;
            _battleCompletionHandler.BattleCompleted += _battleCompletionHandler_BattleCompleted;

            _gameEndHandler = gameEndHandler;
        }

        // May or may not happen
        private void _cruiserDestroyedMonitor_CruiserDestroyed(object sender, CruiserDestroyedEventArgs e)
        {
            _cruiserDestroyedMonitor.CruiserDestroyed -= _cruiserDestroyedMonitor_CruiserDestroyed;

            _gameEndHandler.HandleCruiserDestroyed(e.WasPlayerVictory);

            GameEnded?.Invoke(this, EventArgs.Empty);
        }

        // Always happens
        private void _battleCompletionHandler_BattleCompleted(object sender, EventArgs e)
        {
            _battleCompletionHandler.BattleCompleted -= _battleCompletionHandler_BattleCompleted;
            _cruiserDestroyedMonitor.CruiserDestroyed -= _cruiserDestroyedMonitor_CruiserDestroyed;

            GameEnded?.Invoke(this, EventArgs.Empty);
        }
    }
}