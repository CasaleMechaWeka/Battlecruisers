using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.UI.Cameras
{
    public class CameraComponents : ICameraComponents
    {
        public ICameraAdjuster CameraAdjuster { get; }
        public INavigationWheel NavigationWheel { get; }
        public ICameraFocuser CameraFocuser { get; }
        public ICruiserDeathCameraFocuser CruiserDeathCameraFocuser { get; }
        public Skybox Skybox { get; }

        public CameraComponents(
            ICameraAdjuster cameraAdjuster,
            INavigationWheel navigationWheel,
            ICameraFocuser cameraFocuser,
            ICruiserDeathCameraFocuser cruiserDeathCameraFocuser,
            Skybox skybox)
        {
            Helper.AssertIsNotNull(cameraAdjuster, navigationWheel, cameraFocuser, cruiserDeathCameraFocuser, skybox);

            CameraAdjuster = cameraAdjuster;
            NavigationWheel = navigationWheel;
            CameraFocuser = cameraFocuser;
            CruiserDeathCameraFocuser = cruiserDeathCameraFocuser;
            Skybox = skybox;
        }
    }
}