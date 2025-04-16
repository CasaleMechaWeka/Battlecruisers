using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Sound.Players
{
    public class SoundPlayer
    {
        public async Task PlaySoundAsync(ISoundKey soundKey, Vector2 position)
        {
            Assert.IsNotNull(soundKey);
            AudioClipWrapper sound = await SoundFetcher.GetSoundAsync(soundKey);
            PlaySound(sound, position);
        }

        // TODO: this is a mess
        public void PlaySound(AudioClipWrapper sound, Vector2 position)
        {
            Assert.IsNotNull(sound);
            IAudioSource source = PrefabFactory.CreateAudioSource();
            source.IsActive = true;
            source.AudioClip = sound;
            source.Position = position;
            source.Play();
        }
    }
}