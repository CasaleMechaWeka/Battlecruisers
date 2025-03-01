using BattleCruisers.Buildables;
using BattleCruisers.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    public class PvPCruiserDestroyedMonitor : ICruiserDestroyedMonitor
    {
        private readonly IPvPCruiser _playerACruiser, _playerBCruiser;

        public event EventHandler<CruiserDestroyedEventArgs> CruiserDestroyed;

        public PvPCruiserDestroyedMonitor(IPvPCruiser playerCruiser, IPvPCruiser aiCruiser)
        {
            PvPHelper.AssertIsNotNull(playerCruiser, aiCruiser);

            _playerACruiser = playerCruiser;
            _playerBCruiser = aiCruiser;

            _playerACruiser.Destroyed += _playerACruiser_Destroyed;
            _playerBCruiser.Destroyed += _playerBCruiser_Destroyed;
        }

        private void _playerACruiser_Destroyed(object sender, DestroyedEventArgs e)
        {
            OnCruiserDestroyed(false); // leftplayer lost, rightplayer win
        }

        private void _playerBCruiser_Destroyed(object sender, DestroyedEventArgs e)
        {
            OnCruiserDestroyed(true); //  leftplayer win, rightplayer lost
        }

        private void OnCruiserDestroyed(bool wasVictory)
        {
            _playerACruiser.Destroyed -= _playerACruiser_Destroyed;
            _playerBCruiser.Destroyed -= _playerBCruiser_Destroyed;
            CruiserDestroyed?.Invoke(this, new CruiserDestroyedEventArgs(wasVictory));
        }
    }
}