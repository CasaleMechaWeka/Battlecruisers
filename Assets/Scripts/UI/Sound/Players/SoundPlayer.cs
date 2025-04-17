using BattleCruisers.Data;
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
            PlaySound(sound.AudioClip, position);
        }

        // TODO: this is a mess
        public void PlaySound(AudioClip clip, Vector2 position)
        {
            Assert.IsNotNull(clip);
            AudioSource.PlayClipAtPoint(clip, position, DataProvider.SettingsManager.EffectVolume);
        }
    }
}