using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Sound.Pools
{
    public class AudioSourceInitialiser : Prefab
    {
        [SerializeField]
        private AudioSource _audioSource;
        public int type = -1;

        public AudioSourcePoolable Initialise()
        {
            Assert.IsNotNull(_audioSource);
            Helper.AssertIsNotNull(FactoryProvider.DeferrerProvider.RealTimeDeferrer);

            return
                new AudioSourcePoolable(
                    new EffectVolumeAudioSource(
                        new AudioSourceBC(_audioSource), type),
                    FactoryProvider.DeferrerProvider.RealTimeDeferrer);
        }
    }
}