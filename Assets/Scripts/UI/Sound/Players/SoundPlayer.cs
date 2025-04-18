using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.UI.Sound.Pools;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Fetchers.Cache;
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

        // TODO: this is a mess
        public static void PlaySound(AudioClip clip, Vector2 position)
        {
            Assert.IsNotNull(clip);
            AudioSourceInitialiser audioSourceInitialiser = Object.Instantiate(PrefabCache.AudioSource);
            EffectVolumeAudioSource source = audioSourceInitialiser.Initialise();
            GameObject sourceObject = audioSourceInitialiser.gameObject;

            source.AudioClip = new AudioClipWrapper(clip);
            sourceObject.transform.position = position;

            // we do not want thousands of AudioSources by the end of a long battle
            Object.Destroy(sourceObject, clip.length);
        }
    }
}