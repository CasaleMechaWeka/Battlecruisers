using UnityEngine.Assertions;

namespace BattleCruisers.UI.Sound
{
    /// <summary>
    /// Plays a single sound at a time.
    /// 
    /// If a sound is requested while another sound is in progress, only plays
    /// the new sound if it has higher priority than the current sound.
    /// </summary>
    /// FELIX  Test :P
    public class PrioritisedSoundPlayer : IPrioritisedSoundPlayer
    {
        private readonly ISingleSoundPlayer _soundPlayer;
        private PrioritisedSoundKey _lastSoundKey;

        public PrioritisedSoundPlayer(ISingleSoundPlayer soundPlayer)
        {
            Assert.IsNotNull(soundPlayer);
            _soundPlayer = soundPlayer;
        }

        public void PlaySound(PrioritisedSoundKey soundKey)
        {
            if (_lastSoundKey == null
                || !_soundPlayer.IsPlayingSound
                || soundKey.Priority > _lastSoundKey.Priority)
            {
                _lastSoundKey = soundKey;
                _soundPlayer.PlaySound(soundKey.Key);
            }
        }
    }
}