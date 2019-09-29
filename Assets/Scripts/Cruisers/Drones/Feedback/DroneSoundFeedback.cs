using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using System;
using UnityCommon.Properties;

namespace BattleCruisers.Cruisers.Drones.Feedback
{
    // FELIX  use, test
    /// <summary>
    /// If the parent cruiser has any active drones, play the drones sound.
    /// Otherwise, stop playing the drones sound.
    /// </summary>
    public class DroneSoundFeedback : IManagedDisposable
    {
        private readonly IBroadcastingProperty<bool> _parentCruiserHasActiveDrones;
        private readonly IAudioSource _audioSource;

        public DroneSoundFeedback(IBroadcastingProperty<bool> parentCruiserHasActiveDrones, IAudioSource audioSource)
        {
            Helper.AssertIsNotNull(parentCruiserHasActiveDrones, audioSource);

            _parentCruiserHasActiveDrones = parentCruiserHasActiveDrones;
            _parentCruiserHasActiveDrones.ValueChanged += _parentCruiserHasActiveDrones_ValueChanged;

            _audioSource = audioSource;
            _audioSource.Stop();
        }

        private void _parentCruiserHasActiveDrones_ValueChanged(object sender, EventArgs e)
        {
            if (_parentCruiserHasActiveDrones.Value)
            {
                _audioSource.Play();
            }
            else
            {
                _audioSource.Stop();
            }
        }

        public void DisposeManagedState()
        {
            _parentCruiserHasActiveDrones.ValueChanged -= _parentCruiserHasActiveDrones_ValueChanged;
        }
    }
}