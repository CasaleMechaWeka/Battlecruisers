using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    public class PvPCruiserDestroyedMonitor : IPvPCruiserDestroyedMonitor
    {
        private readonly IPvPCruiser _playerCruiser, _enemyCruiser;

        public event EventHandler<PvPCruiserDestroyedEventArgs> CruiserDestroyed;

        public PvPCruiserDestroyedMonitor(IPvPCruiser playerCruiser, IPvPCruiser aiCruiser)
        {
            PvPHelper.AssertIsNotNull(playerCruiser, aiCruiser);

            _playerCruiser = playerCruiser;
            _enemyCruiser = aiCruiser;

            _playerCruiser.Destroyed += _playerCruiser_Destroyed;
            _enemyCruiser.Destroyed += _aiCruiser_Destroyed;
        }

        private void _playerCruiser_Destroyed(object sender, PvPDestroyedEventArgs e)
        {
            OnCruiserDestroyed(false);
        }

        private void _aiCruiser_Destroyed(object sender, PvPDestroyedEventArgs e)
        {
            OnCruiserDestroyed(true);
        }

        private void OnCruiserDestroyed(bool wasVictory)
        {
            _playerCruiser.Destroyed -= _playerCruiser_Destroyed;
            _enemyCruiser.Destroyed -= _aiCruiser_Destroyed;

            CruiserDestroyed?.Invoke(this, new PvPCruiserDestroyedEventArgs(wasVictory));
        }
    }
}