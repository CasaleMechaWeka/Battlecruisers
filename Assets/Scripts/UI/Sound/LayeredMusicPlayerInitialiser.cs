using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityEngine;

namespace BattleCruisers.UI.Sound
{
    public class LayeredMusicPlayerInitialiser : MonoBehaviour
    {
        public ILayeredMusicPlayer CreatePlayer(
            // FELIX  Use SoundKeyPair instead :)
            ISoundFetcher soundFetcher,
            ISoundKey primarySoundKey,
            ISoundKey secondarySoundKey)
        {
            Helper.AssertIsNotNull(soundFetcher, primarySoundKey, secondarySoundKey);

            AudioSource primarySource = transform.FindNamedComponent<AudioSource>("PrimaryAudioSource");
            IAudioClipWrapper primaryClip = soundFetcher.GetSound(primarySoundKey);
            primarySource.clip = primaryClip.AudioClip;

            AudioSource secondarySource = transform.FindNamedComponent<AudioSource>("SecondaryAudioSource");
            IAudioClipWrapper secondaryClip = soundFetcher.GetSound(secondarySoundKey);
            secondarySource.clip = secondaryClip.AudioClip;

            return 
                new LayeredMusicPlayer(
                    new AudioSourceBC(primarySource), 
                    new AudioSourceBC(secondarySource));
        }
    }
}