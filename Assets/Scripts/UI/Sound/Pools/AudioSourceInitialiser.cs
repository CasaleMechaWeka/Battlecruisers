using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Sound.Pools
{
    // TODO: kill this stupid script

    [RequireComponent(typeof(AudioSource))]
    public class AudioSourceInitialiser : Prefab
    {
        [SerializeField]
        private AudioSource _audioSource;
        const int type = -1;

        public EffectVolumeAudioSource Initialise()
        {
            Assert.IsNotNull(_audioSource);
            return new EffectVolumeAudioSource(new AudioSourceBC(_audioSource), type);
        }

        void Reset()               // editor‑only
        {
            _audioSource = GetComponent<AudioSource>();
        }
    }
}