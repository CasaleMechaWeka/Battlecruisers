using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Audio;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Utils.Threading;
using System.Threading.Tasks;
using UnityCommon.PlatformAbstractions.Time;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Music
{
    public class LayeredMusicPlayerInitialiser : MonoBehaviour
    {
        public async Task<ILayeredMusicPlayer> CreatePlayerAsync(ISoundFetcher soundFetcher, SoundKeyPair soundKeys)
        {
            Helper.AssertIsNotNull(soundFetcher, soundKeys);

            AudioSource primarySource = transform.FindNamedComponent<AudioSource>("PrimaryAudioSource");
            IAudioClipWrapper primaryClip = await soundFetcher.GetSoundAsync(soundKeys.PrimaryKey);
            IAudioSource primary = new AudioSourceBC(primarySource)
            {
                AudioClip = primaryClip
            };

            AudioSource secondarySource = transform.FindNamedComponent<AudioSource>("SecondaryAudioSource");
            IAudioClipWrapper secondaryClip = await soundFetcher.GetSoundAsync(soundKeys.SecondaryKey);
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
                    secondary);
        }
    }
}