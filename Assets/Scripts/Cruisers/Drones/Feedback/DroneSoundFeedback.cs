using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System;
using UnityCommon.Properties;

namespace BattleCruisers.Cruisers.Drones.Feedback
{
    /// <summary>
    /// When drones go from idle to active, play a drones sound.
    /// </summary>
    public class DroneSoundFeedback : IManagedDisposable
    {
        private readonly IBroadcastingProperty<bool> _parentCruiserHasActiveDrones;
        private readonly IAudioSource _audioSource;

        public DroneSoundFeedback(IBroadcastingProperty<bool> parentCruiserHasActiveDrones, IAudioSource audioSource)
        {
            Helper.AssertIsNotNull(parentCruiserHasActiveDrones, audioSource);

            _parentCruiserHasActiveDrones = parentCruiserHasActiveDrones;
            _audioSource = audioSource;

            _parentCruiserHasActiveDrones.ValueChanged += _parentCruiserHasActiveDrones_ValueChanged;
        }

        private void _parentCruiserHasActiveDrones_ValueChanged(object sender, EventArgs e)
        {
            Logging.Log(Tags.DRONE_FEEDBACK, $"{_parentCruiserHasActiveDrones.Value}");

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