using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene;
using System;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene
{
    /// <summary>
    /// The game can be ended in two ways:
    /// 1. A cruiser is destroyed
    /// 2. The user quits
    /// </summary>
    public class PvPGameEndMonitor : IPvPGameEndMonitor
    {
        private readonly IPvPCruiserDestroyedMonitor _cruiserDestroyedMonitor;
        private readonly PvPBattleSceneGodTunnel _battleSceneGodTunnel;
        private readonly IPvPGameEndHandler _gameEndHandler;

        public event EventHandler GameEnded;
        public PvPGameEndMonitor(
            IPvPCruiserDestroyedMonitor cruiserDestroyedMonitor,
            PvPBattleSceneGodTunnel battleSceneGodTunnel,
            IPvPGameEndHandler gameEndHandler)
        {
            PvPHelper.AssertIsNotNull(cruiserDestroyedMonitor, battleSceneGodTunnel, gameEndHandler);

            _cruiserDestroyedMonitor = cruiserDestroyedMonitor;
            _cruiserDestroyedMonitor.CruiserDestroyed += _cruiserDestroyedMonitor_CruiserDestroyed;

            _battleSceneGodTunnel = battleSceneGodTunnel;
            _battleSceneGodTunnel.BattleCompleted.OnValueChanged += _battleCompletionHandler_BattleCompleted;

            _gameEndHandler = gameEndHandler;
        }

        // May or may not happen (ie, user may quit from menu, in which case no cruiser is destroyed)
        private void _cruiserDestroyedMonitor_CruiserDestroyed(object sender, PvPCruiserDestroyedEventArgs e)
        {
            _cruiserDestroyedMonitor.CruiserDestroyed -= _cruiserDestroyedMonitor_CruiserDestroyed;

            _gameEndHandler.HandleCruiserDestroyed(e.WasPlayerVictory, GetTotalDestructionScore());

            GameEnded?.Invoke(this, EventArgs.Empty);
        }

        // Always happens
        private void _battleCompletionHandler_BattleCompleted(Tunnel_BattleCompletedState oldVal, Tunnel_BattleCompletedState newVal)
        {
            if(newVal == Tunnel_BattleCompletedState.Completed)
            {
                _battleSceneGodTunnel.BattleCompleted.OnValueChanged -= _battleCompletionHandler_BattleCompleted;
                _cruiserDestroyedMonitor.CruiserDestroyed -= _cruiserDestroyedMonitor_CruiserDestroyed;

                _gameEndHandler.HandleGameEnd();

                GameEnded?.Invoke(this, EventArgs.Empty);

                _battleSceneGodTunnel.BattleCompleted.Value = Tunnel_BattleCompletedState.None;
            }

        }

        private static long GetTotalDestructionScore()
        {
            Dictionary<PvPTargetType, PvPDeadBuildableCounter> deadBuildables = PvPBattleSceneGodServer.deadBuildables;
            long ds = 0;
            foreach (KeyValuePair<PvPTargetType, PvPDeadBuildableCounter> kvp in deadBuildables)
            {
                ds += kvp.Value.GetTotalDamageInCredits();
            }
            return ds;
        }
    }
}