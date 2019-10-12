using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using System.Threading.Tasks;

namespace BattleCruisers.UI.Sound
{
    /// <summary>
    /// Plays a single sound at a time.
    /// 
    /// If a second sound is played while the previous sound has not completed playing,
    /// then the first sound is stopped.
    /// </summary>
    public class SingleSoundPlayer : ISingleSoundPlayer
    {
        private readonly ISoundFetcher _soundFetcher;
        private readonly IAudioSource _audioSource;

        public bool IsPlayingSound => _audioSource.IsPlaying;

        public SingleSoundPlayer(ISoundFetcher soundFetcher, IAudioSource audioSource)
        {
            Helper.AssertIsNotNull(soundFetcher, audioSource);

            _soundFetcher = soundFetcher;
            _audioSource = audioSource;
        }

        public async Task PlaySoundAsync(ISoundKey soundKey, bool loop = false)
        {
            // Not playing sound spatially, so position is irrelevant.
            IAudioClipWrapper soundToPlay = await _soundFetcher.GetSoundAsync(soundKey);
            _audioSource.Stop();
            _audioSource.AudioClip = soundToPlay;
            _audioSource.Play(isSpatial: false, loop: loop);
        }

        public void Stop()
        {
            _audioSource.Stop();
        }
    }
}
