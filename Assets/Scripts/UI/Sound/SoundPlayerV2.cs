using BattleCruisers.Utils.PlatformAbstractions.UI;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Sound
{
    // FELIX  Renome (remove V2), once legacy sound player is removed :)
    public class SoundPlayerV2 : ISoundPlayer
    {
        // FELIX  Use pool :)  (so current sound is not interrupted)
        private readonly IAudioSource _audioSource;

        public SoundPlayerV2(IAudioSource audioSource)
        {
            Assert.IsNotNull(audioSource);
            _audioSource = audioSource;
        }

        public Task PlaySoundAsync(ISoundKey soundKey, Vector2 position)
        {
            // FELIX  Fix :)
            throw new System.NotImplementedException();
        }

        // FELIX  Used by test scene, so prioritise this :)
        public void PlaySound(IAudioClipWrapper sound, Vector2 position)
        {
            //// FELIX  Remove, once using pools
            //_audioSource.Stop();

            _audioSource.AudioClip = sound;
            _audioSource.Play();
        }
    }
}