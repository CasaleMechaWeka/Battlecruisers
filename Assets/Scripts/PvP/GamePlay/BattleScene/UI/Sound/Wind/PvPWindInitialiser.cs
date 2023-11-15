using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers.Calculators;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Audio;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Wind
{
    public class PvPWindInitialiser : MonoBehaviour
    {
        public AudioSource audioSource;

        public IPvPWindManager Initialise(
            IPvPCamera camera,
            IPvPCameraCalculatorSettings cameraCalculatorSettings,
            ISettingsManager settingsManager)
        {
            Assert.IsNotNull(audioSource);
            PvPHelper.AssertIsNotNull(camera, cameraCalculatorSettings, settingsManager);

            return
                new PvPWindManager(
                    new PvPAudioSourceBC(audioSource),
                    camera,
                    new PvPVolumeCalculator(
                        new PvPProportionCalculator(),
                        cameraCalculatorSettings.ValidOrthographicSizes,
                        settingsManager),
                    settingsManager);
        }
    }
}