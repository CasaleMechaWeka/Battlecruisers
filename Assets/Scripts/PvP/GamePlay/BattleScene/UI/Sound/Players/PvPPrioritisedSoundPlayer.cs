using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.Players;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players
{
    /// <summary>
    /// Plays a single sound at a time.
    /// 
    /// If a sound is requested while another sound is in progress, only plays
    /// the new sound if it has higher priority than the current sound.
    /// </summary>
    public class PvPPrioritisedSoundPlayer : IPrioritisedSoundPlayer
    {
        private readonly ISingleSoundPlayer _soundPlayer;
        private PrioritisedSoundKey _lastSoundKey;

        public bool Enabled { get; set; } = true;

        public PvPPrioritisedSoundPlayer(ISingleSoundPlayer soundPlayer)
        {
            Assert.IsNotNull(soundPlayer);
            _soundPlayer = soundPlayer;
        }

        public void PlaySound(PrioritisedSoundKey soundKey)
        {
            if (Enabled
                && (_lastSoundKey == null
                    || !_soundPlayer.IsPlayingSound
                    || soundKey.Priority > _lastSoundKey.Priority))
            {
                _lastSoundKey = soundKey;
                _soundPlayer.PlaySoundAsync(soundKey.Key);
            }
        }
    }
}