using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.UI.Sound.Wind;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;
using BCUtils = BattleCruisers.Utils;

namespace BattleCruisers.Scenes.Test.Utilities.Sound
{
    public class WindButtonsPanelController : MonoBehaviour
    {
        private IWindManager _windManager;
        public WindInitialiser windInitialiser;

        public void Initialise(ICamera camera, ICameraCalculatorSettings cameraCalculatorSettings)
        {
            BCUtils.Helper.AssertIsNotNull(camera, cameraCalculatorSettings);

            Assert.IsNotNull(windInitialiser);
            _windManager = windInitialiser.Initialise(camera, cameraCalculatorSettings);
        }

        public void StartWind()
        {
            _windManager.Play();
        }

        public void StopWind()
        {
            _windManager.Stop();
        }
    }
}