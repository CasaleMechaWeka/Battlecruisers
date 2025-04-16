using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Sound.Pools
{
    // TODO: kill this stupid script
    public class AudioSourceInitialiser : Prefab
    {
        [SerializeField]
        private AudioSource _audioSource;
        const int type = -1;

        public IAudioSource Initialise()
        {
            Assert.IsNotNull(_audioSource);
            return new EffectVolumeAudioSource(new AudioSourceBC(_audioSource), type);
        }
    }
}