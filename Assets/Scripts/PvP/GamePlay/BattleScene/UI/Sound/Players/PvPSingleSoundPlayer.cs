using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players
{
    /// <summary>
    /// Plays a single sound at a time.
    /// 
    /// If a second sound is played while the previous sound has not completed playing,
    /// then the first sound is stopped.
    /// </summary>
    public class PvPSingleSoundPlayer : IPvPSingleSoundPlayer
    {
        private readonly ISoundFetcher _soundFetcher;
        private readonly IPvPAudioSource _audioSource;

        public bool IsPlayingSound => _audioSource.IsPlaying;

        public PvPSingleSoundPlayer(ISoundFetcher soundFetcher, IPvPAudioSource audioSource)
        {
            PvPHelper.AssertIsNotNull(soundFetcher, audioSource);

            _soundFetcher = soundFetcher;
            _audioSource = audioSource;
        }

        public async Task<AsyncOperationHandle<AudioClip>> PlaySoundAsync(ISoundKey soundKey, bool loop = false)
        {
            // Logging.Log(Tags.SOUND, $"{soundKey.Name}  loop: {loop}");

            IAudioClipWrapper soundToPlay = await _soundFetcher.GetSoundAsync(soundKey);
            PlaySound(soundToPlay, loop);
            return soundToPlay.Handle;
        }

        public void PlaySound(IAudioClipWrapper sound, bool loop = false)
        {
            // Logging.Log(Tags.SOUND, $"{sound}  loop: {loop}");

            _audioSource.Stop();
            _audioSource.AudioClip = sound;
            // Not playing sound spatially, so position is irrelevant.
            _audioSource.Play(isSpatial: false, loop: loop);
        }

        public void Stop()
        {
            // Logging.LogMethod(Tags.SOUND);
            _audioSource.Stop();
        }
    }
}
