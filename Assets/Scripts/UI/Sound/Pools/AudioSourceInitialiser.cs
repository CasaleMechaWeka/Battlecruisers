using BattleCruisers.Utils.PlatformAbstractions.UI;
using BattleCruisers.Utils.Threading;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Sound.Pools
{
    public class AudioSourceInitialiser : Prefab
    {
        [SerializeField]
        private AudioSource _audioSource;

        public AudioSourcePoolable Initialise(IDeferrer realTimeDeferrer)
        {
            Assert.IsNotNull(_audioSource);
            Assert.IsNotNull(realTimeDeferrer);

            return 
                new AudioSourcePoolable(
                    new AudioSourceBC(_audioSource), 
                    realTimeDeferrer);
        }
    }
}