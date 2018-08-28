using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityEngine;

namespace BattleCruisers.UI.Sound
{
    public class SoundPlayer : ISoundPlayer
    {
        private readonly ISoundFetcher _soundFetcher;
        private readonly IAudioClipPlayer _audioClipPlayer;

        public SoundPlayer(ISoundFetcher soundFetcher, IAudioClipPlayer audioClipPlayer)
        {
            Helper.AssertIsNotNull(soundFetcher, audioClipPlayer);

            _soundFetcher = soundFetcher;
            _audioClipPlayer = audioClipPlayer;
        }

        public void PlaySound(ISoundKey soundKey, Vector2 position)
        {
            IAudioClipWrapper sound = _soundFetcher.GetSound(soundKey);
            _audioClipPlayer.PlaySound(sound, position);
        }
    }
}
