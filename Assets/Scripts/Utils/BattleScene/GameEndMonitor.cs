using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Scenes.BattleScene;
using System;
using System.Collections.Generic;

namespace BattleCruisers.Utils.BattleScene
{
    /// <summary>
    /// The game can be ended in two ways:
    /// 1. A cruiser is destroyed
    /// 2. The user quits
    /// </summary>
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

        // May or may not happen (ie, user may quit from menu, in which case no cruiser is destroyed)
        private void _cruiserDestroyedMonitor_CruiserDestroyed(object sender, CruiserDestroyedEventArgs e)
        {
            _cruiserDestroyedMonitor.CruiserDestroyed -= _cruiserDestroyedMonitor_CruiserDestroyed;

            _gameEndHandler.HandleCruiserDestroyed(e.WasPlayerVictory, GetTotalDestructionScore());

            GameEnded?.Invoke(this, EventArgs.Empty);
        }

        // Always happens
        private void _battleCompletionHandler_BattleCompleted(object sender, EventArgs e)
        {
            _battleCompletionHandler.BattleCompleted -= _battleCompletionHandler_BattleCompleted;
            _cruiserDestroyedMonitor.CruiserDestroyed -= _cruiserDestroyedMonitor_CruiserDestroyed;

            _gameEndHandler.HandleGameEnd();

            GameEnded?.Invoke(this, EventArgs.Empty);
        }

        private static long GetTotalDestructionScore()
        {
            Dictionary<TargetType, DeadBuildableCounter> deadBuildables = BattleSceneGod.deadBuildables;
            long ds = 0;
            foreach(KeyValuePair<TargetType, DeadBuildableCounter> kvp in deadBuildables)
            {
                ds += kvp.Value.GetTotalDamageInCredits();
            }
            return ds;
        }
    }
}