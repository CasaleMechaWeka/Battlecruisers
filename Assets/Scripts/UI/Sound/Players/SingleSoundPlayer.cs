using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BattleCruisers.UI.Sound.Players
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

        public async Task<AsyncOperationHandle<AudioClip>> PlaySoundAsync(ISoundKey soundKey, bool loop = false)
        {
            Logging.Log(Tags.SOUND, $"{soundKey.Name}  loop: {loop}");
            Debug.Log($"{soundKey.Name}  loop: {loop}");

            IAudioClipWrapper soundToPlay = await _soundFetcher.GetSoundAsync(soundKey);
            PlaySound(soundToPlay, loop);
            return soundToPlay.Handle;
        }

        public void PlaySound(IAudioClipWrapper sound, bool loop = false)
        {
            Logging.Log(Tags.SOUND, $"{sound}  loop: {loop}");

            _audioSource.Stop();
            _audioSource.AudioClip = sound;
            // Not playing sound spatially, so position is irrelevant.
            _audioSource.Play(isSpatial: false, loop: loop);
        }

        public void Stop()
        {
            Logging.LogMethod(Tags.SOUND);
            _audioSource.Stop();
        }
    }
}
