using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.UI.Cameras
{
    public class CameraComponents : ICameraComponents
    {
        public ICamera MainCamera { get; }
        public ICameraAdjuster CameraAdjuster { get; }
        public ICameraFocuser CameraFocuser { get; }
        public ICruiserDeathCameraFocuser CruiserDeathCameraFocuser { get; }
        public Skybox Skybox { get; }
        public ICameraCalculatorSettings Settings { get; }

        public CameraComponents(
            ICamera mainCamera,
            ICameraAdjuster cameraAdjuster,
            ICameraFocuser cameraFocuser,
            ICruiserDeathCameraFocuser cruiserDeathCameraFocuser,
            Skybox skybox,
            ICameraCalculatorSettings settings)
        {
            Helper.AssertIsNotNull(mainCamera, cameraAdjuster, cameraFocuser, cruiserDeathCameraFocuser, skybox, settings);

            MainCamera = mainCamera;
            CameraAdjuster = cameraAdjuster;
            CameraFocuser = cameraFocuser;
            CruiserDeathCameraFocuser = cruiserDeathCameraFocuser;
            Skybox = skybox;
            Settings = settings;
        }
    }
}