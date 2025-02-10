using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Utils.Properties;
using System;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback
{
    /// <summary>
    /// When drones go from idle to active, play a drones sound.
    /// </summary>
    public class PvPDroneSoundFeedback : IManagedDisposable
    {
        private readonly IBroadcastingProperty<bool> _parentCruiserHasActiveDrones;
        private readonly IAudioSource _audioSource;

        public PvPDroneSoundFeedback(IBroadcastingProperty<bool> parentCruiserHasActiveDrones, IAudioSource audioSource)
        {
            PvPHelper.AssertIsNotNull(parentCruiserHasActiveDrones, audioSource);

            _parentCruiserHasActiveDrones = parentCruiserHasActiveDrones;
            _audioSource = audioSource;

            _parentCruiserHasActiveDrones.ValueChanged += _parentCruiserHasActiveDrones_ValueChanged;
        }

        private void _parentCruiserHasActiveDrones_ValueChanged(object sender, EventArgs e)
        {
            if (_parentCruiserHasActiveDrones.Value)
            {
                _audioSource?.Play(isSpatial: true);
            }
        }

        public void DisposeManagedState()
        {
            _parentCruiserHasActiveDrones.ValueChanged -= _parentCruiserHasActiveDrones_ValueChanged;
        }
    }
}