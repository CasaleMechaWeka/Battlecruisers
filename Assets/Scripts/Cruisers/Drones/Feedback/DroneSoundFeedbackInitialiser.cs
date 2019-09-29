using BattleCruisers.Utils.PlatformAbstractions.UI;
using UnityCommon.Properties;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Drones.Feedback
{
    public class DroneSoundFeedbackInitialiser : MonoBehaviour
    {
        public DroneSoundFeedback Initialise(IBroadcastingProperty<bool> parentCruiserHasActiveDrones)
        {
            Assert.IsNotNull(parentCruiserHasActiveDrones);

            AudioSource audioSource = GetComponentInChildren<AudioSource>();
            Assert.IsNotNull(audioSource);

            return
                new DroneSoundFeedback(
                    parentCruiserHasActiveDrones,
                    new AudioSourceBC(audioSource));
        }
    }
}