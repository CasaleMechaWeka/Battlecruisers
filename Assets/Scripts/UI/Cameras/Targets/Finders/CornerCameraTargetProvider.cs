using System.Collections.Generic;
using BattleCruisers.Cruisers;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Targets.Finders
{
    public class CornerCameraTargetProvider : ICornerCameraTargetProvider
    {
        private readonly IDictionary<CameraCorner, ICameraTarget> _cornerToTarget;

        public CornerCameraTargetProvider(ICamera camera, ICameraCalculator cameraCalculator, ICruiser playerCruiser, ICruiser aiCruiser)
        {
            Helper.AssertIsNotNull(camera, cameraCalculator, playerCruiser, aiCruiser);

            _cornerToTarget = new Dictionary<CameraCorner, ICameraTarget>();

            ICameraTarget overviewTarget = FindOverviewTarget(camera, cameraCalculator);
            _cornerToTarget.Add(CameraCorner.Overview, overviewTarget);

            ICameraTarget playerCruiserTarget = FindCruiserTarget(camera, cameraCalculator, playerCruiser);
            _cornerToTarget.Add(CameraCorner.PlayerCruiser, playerCruiserTarget);

            ICameraTarget aiCruiserTarget = FindCruiserTarget(camera, cameraCalculator, aiCruiser);
            _cornerToTarget.Add(CameraCorner.AICruiser, aiCruiserTarget);
        }

        private ICameraTarget FindOverviewTarget(ICamera camera, ICameraCalculator cameraCalculator)
        {
            Vector3 targetPosition = camera.Transform.Position;
            targetPosition.y = cameraCalculator.FindCameraYPosition(camera.OrthographicSize);
            return new CameraTarget(targetPosition, camera.OrthographicSize);
        }

        private ICameraTarget FindCruiserTarget(ICamera camera, ICameraCalculator cameraCalculator, ICruiser cruiser)
        {
            float targetOrthographicSize = cameraCalculator.FindCameraOrthographicSize(cruiser);
            Vector3 targetPosition = cameraCalculator.FindCruiserCameraPosition(cruiser, targetOrthographicSize, camera.Transform.Position.z);
            return new CameraTarget(targetPosition, targetOrthographicSize);
        }

        public ICameraTarget GetTarget(CameraCorner cameraCorner)
        {
            Assert.IsTrue(_cornerToTarget.ContainsKey(cameraCorner));
            return _cornerToTarget[cameraCorner];
        }
    }
}
