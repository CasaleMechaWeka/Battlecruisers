using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Utils.Threading;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Sound.Pools
{
    public class AudioSourceInitialiser : Prefab
    {
        [SerializeField]
        private AudioSource _audioSource;
        public int type = -1;

        public AudioSourcePoolable Initialise(IDeferrer realTimeDeferrer, ISettingsManager settingsManager)
        {
            Assert.IsNotNull(_audioSource);
            Helper.AssertIsNotNull(realTimeDeferrer, settingsManager);

            return 
                new AudioSourcePoolable(
                    new EffectVolumeAudioSource(
                        new AudioSourceBC(_audioSource),
                        settingsManager, type), 
                    realTimeDeferrer);
        }
    }
}