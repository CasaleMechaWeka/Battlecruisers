using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Properties;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.Cruisers.Drones.Feedback;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using BattleCruisers.UI.Sound.AudioSources;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback
{
    public class PvPDroneSoundFeedbackInitialiser : MonoBehaviour
    {
        public DroneSoundFeedback Initialise(
            IBroadcastingProperty<bool> parentCruiserHasActiveDrones,
            SettingsManager settingsManager)
        {
            Helper.AssertIsNotNull(parentCruiserHasActiveDrones, settingsManager);

            AudioSource audioSource = GetComponentInChildren<AudioSource>();
            Assert.IsNotNull(audioSource);

            return
                new DroneSoundFeedback(
                    parentCruiserHasActiveDrones,
                    new EffectVolumeAudioSource(
                        new AudioSourceBC(audioSource),
                        settingsManager, 2));
        }
    }
}