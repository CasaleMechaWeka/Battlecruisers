using BattleCruisers.Buildables;
using BattleCruisers.Utils;
using System;

namespace BattleCruisers.Cruisers
{
    // FELIX  Update tests :)
    public class CruiserDestroyedMonitor : ICruiserDestroyedMonitor
    {
        private readonly ICruiser _playerCruiser, _aiCruiser;

        public event EventHandler<CruiserDestroyedEventArgs> CruiserDestroyed;

        public CruiserDestroyedMonitor(ICruiser playerCruiser, ICruiser aiCruiser)
        {
            Helper.AssertIsNotNull(playerCruiser, aiCruiser);

            _playerCruiser = playerCruiser;
            _aiCruiser = aiCruiser;

            _playerCruiser.Destroyed += _playerCruiser_Destroyed;
            _aiCruiser.Destroyed += _aiCruiser_Destroyed;
        }

        private void _playerCruiser_Destroyed(object sender, DestroyedEventArgs e)
        {
            OnCruiserDestroyed(false);
        }

        private void _aiCruiser_Destroyed(object sender, DestroyedEventArgs e)
        {
            OnCruiserDestroyed(true);
        }

        private void OnCruiserDestroyed(bool wasVictory)
        {
            _playerCruiser.Destroyed -= _playerCruiser_Destroyed;
            _aiCruiser.Destroyed -= _aiCruiser_Destroyed;

            CruiserDestroyed?.Invoke(this, new CruiserDestroyedEventArgs(wasVictory));
        }
    }
}