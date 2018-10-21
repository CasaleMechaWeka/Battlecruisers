using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.UI;
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

        public void PlaySound(ISoundKey soundKey)
        {
            // Play sound at camera location.  Assumes there is only one camera in the game.
            PlaySound(soundKey, _soleCamera.Transform.Position);
        }

        public void PlaySound(ISoundKey soundKey, Vector2 position)
        {
            IAudioClipWrapper sound = _soundFetcher.GetSound(soundKey);
            _audioClipPlayer.PlaySound(sound, position);
        }
    }
}
