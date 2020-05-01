using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.UI.Sound
{
    public class SoundPlayer : ISoundPlayer
    {
        private readonly ISoundFetcher _soundFetcher;
        private readonly IAudioClipPlayer _audioClipPlayer;
        private readonly IGameObject _audioListener;

        public SoundPlayer(ISoundFetcher soundFetcher, IAudioClipPlayer audioClipPlayer, IGameObject audioListener)
        {
            Helper.AssertIsNotNull(soundFetcher, audioClipPlayer, audioListener);

            _soundFetcher = soundFetcher;
            _audioClipPlayer = audioClipPlayer;
            _audioListener = audioListener;
        }

        public async Task PlaySoundAsync(ISoundKey soundKey)
        {
            await PlaySoundAsync(soundKey, _audioListener.Position);
        }

        public async Task PlaySoundAsync(ISoundKey soundKey, Vector2 position)
        {
            IAudioClipWrapper sound = await _soundFetcher.GetSoundAsync(soundKey);
            PlaySound(sound, position);
        }

        public void PlaySound(IAudioClipWrapper sound, Vector2 position)
        {
            _audioClipPlayer.PlaySound(sound, position);
        }
    }
}
