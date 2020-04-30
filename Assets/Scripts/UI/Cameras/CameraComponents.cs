using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.UI.Cameras
{
    public class CameraComponents : ICameraComponents
    {
        public ICamera MainCamera { get; }
        public ICameraAdjuster CameraAdjuster { get; }
        public INavigationWheel NavigationWheel { get; }
        public ICameraFocuser CameraFocuser { get; }
        public ICruiserDeathCameraFocuser CruiserDeathCameraFocuser { get; }
        public Skybox Skybox { get; }

        public CameraComponents(
            ICamera mainCamera,
            ICameraAdjuster cameraAdjuster,
            INavigationWheel navigationWheel,
            ICameraFocuser cameraFocuser,
            ICruiserDeathCameraFocuser cruiserDeathCameraFocuser,
            Skybox skybox)
        {
            Helper.AssertIsNotNull(mainCamera, cameraAdjuster, navigationWheel, cameraFocuser, cruiserDeathCameraFocuser, skybox);

            MainCamera = mainCamera;
            CameraAdjuster = cameraAdjuster;
            NavigationWheel = navigationWheel;
            CameraFocuser = cameraFocuser;
            CruiserDeathCameraFocuser = cruiserDeathCameraFocuser;
            Skybox = skybox;
        }
    }
}