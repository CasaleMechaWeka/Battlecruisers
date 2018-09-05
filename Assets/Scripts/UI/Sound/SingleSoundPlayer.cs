using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityEngine;

namespace BattleCruisers.UI.Sound
{
    /// <summary>
    /// Plays a single sound at a time.
    /// 
    /// If a second sound is played while the previous sound has not completed playing,
    /// then the first sound is stopped.
    /// </summary>
    /// FELIX  Test :)
    public class SingleSoundPlayer : ISoundPlayer
    {
        private readonly ISoundFetcher _soundFetcher;
        private readonly IAudioSource _audioSource;

        public SingleSoundPlayer(ISoundFetcher soundFetcher, IAudioSource audioSource)
        {
            Helper.AssertIsNotNull(soundFetcher, audioSource);

            _soundFetcher = soundFetcher;
            _audioSource = audioSource;
        }

        public void PlaySound(ISoundKey soundKey)
        {
            PlaySound(soundKey, default(Vector2));
        }

        public void PlaySound(ISoundKey soundKey, Vector2 position)
        {
            // Not playing sound spatially, so position is irrelevant.
            IAudioClipWrapper soundToPlay = _soundFetcher.GetSound(soundKey);
            _audioSource.Stop();
            _audioSource.AudioClip = soundToPlay;
            _audioSource.Play(isSpatial: false);
        }
    }
}
