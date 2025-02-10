using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.AudioSources;
using BattleCruisers.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using BattleCruisers.Utils.Properties;
using UnityEngine;
using UnityEngine.Assertions;
using BattleCruisers.Cruisers.Drones.Feedback;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones.Feedback
{
    public class PvPDroneSoundFeedbackInitialiser : MonoBehaviour
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
                    new PvPEffectVolumeAudioSource(
                        new PvPAudioSourceBC(audioSource),
                        settingsManager, 2));
        }
    }
}