using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityEngine;

namespace BattleCruisers.UI.Sound
{
    public class SoundManager : ISoundManager
    {
        private readonly ISoundFetcher _soundFetcher;
        private readonly ISoundPlayer _soundPlayer;

        public SoundManager(ISoundFetcher soundFetcher, ISoundPlayer soundPlayer)
        {
            Helper.AssertIsNotNull(soundFetcher, soundPlayer);

            _soundFetcher = soundFetcher;
            _soundPlayer = soundPlayer;
        }

        public void PlaySound(ISoundKey soundKey, Vector2 position)
        {
            IAudioClipWrapper sound = _soundFetcher.GetSound(soundKey);
            _soundPlayer.PlaySound(sound, position);
        }
    }
}
