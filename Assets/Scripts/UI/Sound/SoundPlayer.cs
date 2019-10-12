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
        private readonly ICamera _soleCamera;

        public SoundPlayer(ISoundFetcher soundFetcher, IAudioClipPlayer audioClipPlayer, ICamera soleCamera)
        {
            Helper.AssertIsNotNull(soundFetcher, audioClipPlayer, soleCamera);

            _soundFetcher = soundFetcher;
            _audioClipPlayer = audioClipPlayer;
            _soleCamera = soleCamera;
        }

        public async Task PlaySoundAsync(ISoundKey soundKey)
        {
            // Play sound at camera location.  Assumes there is only one camera in the game.
            await PlaySoundAsync(soundKey, _soleCamera.Transform.Position);
        }

        public async Task PlaySoundAsync(ISoundKey soundKey, Vector2 position)
        {
            IAudioClipWrapper sound = await _soundFetcher.GetSoundAsync(soundKey);
            _audioClipPlayer.PlaySound(sound, position);
        }
    }
}
