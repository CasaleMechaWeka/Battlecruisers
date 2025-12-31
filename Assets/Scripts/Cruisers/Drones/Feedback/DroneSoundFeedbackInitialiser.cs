using BattleCruisers.UI.Sound.AudioSources;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Utils.Properties;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Drones.Feedback
{
    public class DroneSoundFeedbackInitialiser : MonoBehaviour
    {
        public DroneSoundFeedback Initialise(
            IBroadcastingProperty<bool> parentCruiserHasActiveDrones)
        {
            Helper.AssertIsNotNull(parentCruiserHasActiveDrones);

            AudioSource audioSource = GetComponentInChildren<AudioSource>();
            Assert.IsNotNull(audioSource);

            return
                new DroneSoundFeedback(
                    parentCruiserHasActiveDrones,
                    new EffectVolumeAudioSource(
                        new AudioSourceBC(audioSource), 2));
        }
    }
}