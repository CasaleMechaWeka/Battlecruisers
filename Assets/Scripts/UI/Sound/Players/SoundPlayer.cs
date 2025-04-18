using BattleCruisers.Data;
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

        // TODO: this is a mess
        public static void PlaySound(AudioClip clip, Vector2 position)
        {
            Assert.IsNotNull(clip);
            AudioSource.PlayClipAtPoint(clip, position, DataProvider.SettingsManager.EffectVolume);
        }
    }
}