using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityEngine;

namespace BattleCruisers.UI.Music
{
    public class LayeredMusicPlayerInitialiser : MonoBehaviour
    {
        public ILayeredMusicPlayer CreatePlayer(
            ISoundFetcher soundFetcher,
            SoundKeyPair soundKeys)
        {
            Helper.AssertIsNotNull(soundFetcher, soundKeys);

            AudioSource primarySource = transform.FindNamedComponent<AudioSource>("PrimaryAudioSource");
            IAudioClipWrapper primaryClip = soundFetcher.GetSound(soundKeys.PrimaryKey);
            primarySource.clip = primaryClip.AudioClip;

            AudioSource secondarySource = transform.FindNamedComponent<AudioSource>("SecondaryAudioSource");
            IAudioClipWrapper secondaryClip = soundFetcher.GetSound(soundKeys.SecondaryKey);
            secondarySource.clip = secondaryClip.AudioClip;

            return 
                new LayeredMusicPlayer(
                    new AudioSourceBC(primarySource), 
                    new AudioSourceBC(secondarySource));
        }
    }
}