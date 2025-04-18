using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Sound.Players
{
    public static class SoundPlayer
    {
        public static async Task PlaySoundAsync(ISoundKey soundKey, Vector2 position)
        {
            Assert.IsNotNull(soundKey);
            AudioClipWrapper sound = await SoundFetcher.GetSoundAsync(soundKey);
            PlaySound(sound.AudioClip, position);
        }

        public static void PlaySound(AudioClip clip, Vector2 position)
        {
            Assert.IsNotNull(clip);

            GameObject sourceObject = new GameObject("Audio Source");
            sourceObject.transform.position = position;

            AudioSource source = sourceObject.AddComponent<AudioSource>();
            EffectVolumeAudioSource effectVolumeAudioSource = new EffectVolumeAudioSource(new AudioSourceBC(source));

            effectVolumeAudioSource.AudioClip = new AudioClipWrapper(clip);

            // we do not want thousands of AudioSources by the end of a long battle
            Object.Destroy(sourceObject, clip.length);
        }
    }
}