using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.Utils;
using UnityEngine;

namespace BattleCruisers.UI.Cameras
{
    public class CameraComponents : ICameraComponents
    {
        public ICameraAdjuster CameraAdjuster { get; private set; }
        public INavigationWheel NavigationWheel { get; private set; }
        public ICameraFocuser CameraFocuser { get; private set; }
        public Skybox Skybox { get; private set; }

        public CameraComponents(
            ICameraAdjuster cameraAdjuster,
            INavigationWheel navigationWheel,
            ICameraFocuser cameraFocuser,
            Skybox skybox)
        {
            Helper.AssertIsNotNull(cameraAdjuster, navigationWheel, cameraFocuser, skybox);

            CameraAdjuster = cameraAdjuster;
            NavigationWheel = navigationWheel;
            CameraFocuser = cameraFocuser;
            Skybox = skybox;
        }
    }
}