using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCruisers.UI.Music
{
    public class LayeredMusicPlayerInitialiser : MonoBehaviour
    {
        public async Task<ILayeredMusicPlayer> CreatePlayerAsync(
            ISoundFetcher soundFetcher,
            SoundKeyPair soundKeys)
        {
            Helper.AssertIsNotNull(soundFetcher, soundKeys);

            AudioSource primarySource = transform.FindNamedComponent<AudioSource>("PrimaryAudioSource");
            IAudioClipWrapper primaryClip = await soundFetcher.GetSoundAsync(soundKeys.PrimaryKey);
            primarySource.clip = primaryClip.AudioClip;

            AudioSource secondarySource = transform.FindNamedComponent<AudioSource>("SecondaryAudioSource");
            IAudioClipWrapper secondaryClip = await soundFetcher.GetSoundAsync(soundKeys.SecondaryKey);
            secondarySource.clip = secondaryClip.AudioClip;

            return 
                new LayeredMusicPlayer(
                    new AudioSourceBC(primarySource), 
                    new AudioSourceBC(secondarySource));
        }
    }
}