using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using UnityCommon.Properties;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers.Drones.Feedback
{
    public class DroneSoundFeedbackInitialiser : MonoBehaviour
    {
        public DroneSoundFeedback Initialise(
            IBroadcastingProperty<bool> parentCruiserHasActiveDrones,
            ISettingsManager settingsManager)
        {
            Helper.AssertIsNotNull(parentCruiserHasActiveDrones, settingsManager);

            AudioSource audioSource = GetComponentInChildren<AudioSource>();
            Assert.IsNotNull(audioSource);

            return
                new DroneSoundFeedback(
                    parentCruiserHasActiveDrones,
                    new EffectVolumeAudioSource(
                        new AudioSourceBC(audioSource),
                        settingsManager));
        }
    }
}