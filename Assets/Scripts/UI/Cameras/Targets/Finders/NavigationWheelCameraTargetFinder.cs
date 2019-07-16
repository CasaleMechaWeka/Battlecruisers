using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.UI.Cameras.Targets.Finders
{
    public class NavigationWheelCameraTargetFinder : ICameraTargetFinder
    {
        private readonly ICameraNavigationWheelCalculator _cameraNavigationWheelCalculator;
        private readonly ICamera _camera;

        public NavigationWheelCameraTargetFinder(
            ICameraNavigationWheelCalculator cameraNavigationWheelCalculator, 
            ICamera camera)
        {
            Helper.AssertIsNotNull(cameraNavigationWheelCalculator, camera);

            _cameraNavigationWheelCalculator = cameraNavigationWheelCalculator;
            _camera = camera;
        }

        public ICameraTarget FindCameraTarget()
        {
            Vector2 targetCameraPosition2D = _cameraNavigationWheelCalculator.FindCameraPosition();
            Vector3 targetCameraPosition3D = new Vector3(targetCameraPosition2D.x, targetCameraPosition2D.y, _camera.Transform.Position.z);
            float targetCameraOrthographicSize = _cameraNavigationWheelCalculator.FindOrthographicSize();

            return new CameraTarget(targetCameraPosition3D, targetCameraOrthographicSize);
        }
    }
}