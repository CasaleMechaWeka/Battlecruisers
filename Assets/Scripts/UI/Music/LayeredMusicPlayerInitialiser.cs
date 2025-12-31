using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Utils.Threading;
using System.Threading.Tasks;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Music
{
    public class LayeredMusicPlayerInitialiser : MonoBehaviour
    {
        public async Task<LayeredMusicPlayer> CreatePlayerAsync(
            SoundKeyPair soundKeys,
            SettingsManager settingsManager)
        {
            Helper.AssertIsNotNull(soundKeys, settingsManager);

            AudioSource primarySource = transform.FindNamedComponent<AudioSource>("PrimaryAudioSource");
            AudioClipWrapper primaryClip = await SoundFetcher.GetSoundAsync(soundKeys.PrimaryKey);
            IAudioSource primary = new AudioSourceBC(primarySource)
            {
                AudioClip = primaryClip
            };

            AudioSource secondarySource = transform.FindNamedComponent<AudioSource>("SecondaryAudioSource");
            AudioClipWrapper secondaryClip = await SoundFetcher.GetSoundAsync(soundKeys.SecondaryKey);
            secondarySource.clip = secondaryClip.AudioClip;
            IAudioSource secondary = new AudioSourceBC(secondarySource)
            {
                AudioClip = secondaryClip
            };

            CoroutineStarter coroutineStarter = GetComponent<CoroutineStarter>();
            Assert.IsNotNull(coroutineStarter);

            return
                new LayeredMusicPlayer(
                    new AudioVolumeFade(coroutineStarter, TimeBC.Instance),
                    primary,
                    secondary,
                    settingsManager);
        }
    }
}