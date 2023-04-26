using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using System;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback
{
    /// <summary>
    /// When drones go from idle to active, play a drones sound.
    /// </summary>
    public class PvPDroneSoundFeedback : IPvPManagedDisposable
    {
        private readonly IPvPBroadcastingProperty<bool> _parentCruiserHasActiveDrones;
        private readonly IPvPAudioSource _audioSource;

        public PvPDroneSoundFeedback(IPvPBroadcastingProperty<bool> parentCruiserHasActiveDrones, IPvPAudioSource audioSource)
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
                _audioSource.Play(isSpatial: true);
            }
        }

        public void DisposeManagedState()
        {
            _parentCruiserHasActiveDrones.ValueChanged -= _parentCruiserHasActiveDrones_ValueChanged;
        }
    }
}