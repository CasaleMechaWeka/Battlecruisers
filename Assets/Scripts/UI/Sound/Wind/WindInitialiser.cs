using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.Audio;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Sound.Wind
{
    public class WindInitialiser : MonoBehaviour
    {
        public AudioSource audioSource;

        public IWindManager Initialise(
            ICamera camera, 
            ICameraCalculatorSettings cameraCalculatorSettings,
            ISettingsManager settingsManager)
        {
            Assert.IsNotNull(audioSource);
            Helper.AssertIsNotNull(camera, cameraCalculatorSettings, settingsManager);

            return
                new WindManager(
                    new AudioSourceBC(audioSource),
                    camera,
                    new VolumeCalculator(
                        new ProportionCalculator(),
                        cameraCalculatorSettings.ValidOrthographicSizes,
                        settingsManager));
        }
    }
}