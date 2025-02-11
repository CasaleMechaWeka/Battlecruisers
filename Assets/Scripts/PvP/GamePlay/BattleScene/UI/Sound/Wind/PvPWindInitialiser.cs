using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.UI.Sound.Wind;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Wind
{
    public class PvPWindInitialiser : MonoBehaviour
    {
        public AudioSource audioSource;

        public IWindManager Initialise(
            ICamera camera,
            ICameraCalculatorSettings cameraCalculatorSettings,
            ISettingsManager settingsManager)
        {
            Assert.IsNotNull(audioSource);
            PvPHelper.AssertIsNotNull(camera, cameraCalculatorSettings, settingsManager);

            return
                new WindManager(
                    new AudioSourceBC(audioSource),
                    camera,
                    new VolumeCalculator(
                        new ProportionCalculator(),
                        cameraCalculatorSettings.ValidOrthographicSizes,
                        settingsManager),
                    settingsManager);
        }
    }
}