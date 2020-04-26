using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;
using System;
using UnityEngine;

namespace BattleCruisers.UI.Cameras.Adjusters
{
    // FELIX  Test
    public class ParallaxCameraAdjuster : ICameraAdjuster
    {
        private readonly ICamera _mainCamera, _backgroundCamera;

        public ParallaxCameraAdjuster(ICamera mainCamera, ICamera backgroundCamera)
        {
            Helper.AssertIsNotNull(mainCamera, backgroundCamera);

            _mainCamera = mainCamera;
            _backgroundCamera = backgroundCamera;
        }

        public event EventHandler CompletedAdjustment;

        public void AdjustCamera()
        {
            // FELIX  Abstract to ICameraCalculator?
            float orthoSize = _mainCamera.OrthographicSize;
            float mainCameraZPosition = Mathf.Abs(_mainCamera.Transform.Position.z);
            float fieldOfView = Mathf.Atan(orthoSize / mainCameraZPosition) * Mathf.Rad2Deg * 2f;
            _backgroundCamera.FieldOfView = fieldOfView;

            Logging.Log(Tags.CAMERA, $"Main ortho size: {orthoSize}  Field of view: {fieldOfView}");
        }
    }
}