using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.UI.Cameras.Helpers
{
    public class DirectionalZoom : IDirectionalZoom
    {
        private readonly ICamera _camera;
        private readonly ICameraCalculator _cameraCalculator;
        private readonly IRange<float> _validOrthographicSizes;

        public DirectionalZoom(ICamera camera, ICameraCalculator cameraCalculator, IRange<float> validOrthographicSizes)
        {
            Helper.AssertIsNotNull(camera, cameraCalculator, validOrthographicSizes);

            _camera = camera;
            _cameraCalculator = cameraCalculator;
            _validOrthographicSizes = validOrthographicSizes;
        }

        public ICameraTarget ZoomOut(float orthographicSizeDelta)
        {
            Logging.Log(Tags.DIRECTIONAL_ZOOM, $"orthographicSizeDelta: {orthographicSizeDelta}");

            // Find target camera orthographic size
            float targetOrthographicSize = _camera.OrthographicSize + orthographicSizeDelta;
            targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, _validOrthographicSizes.Min, _validOrthographicSizes.Max);
            Logging.Log(Tags.DIRECTIONAL_ZOOM, $"targetOrthographicSize: {targetOrthographicSize}  currentOrthographicSize: {_camera.OrthographicSize}");

            // Find target camera x position
            IRange<float> validXPositions = _cameraCalculator.FindValidCameraXPositions(targetOrthographicSize);
            float targetXPosition = Mathf.Clamp(_camera.Transform.Position.x, validXPositions.Min, validXPositions.Max);
            Logging.Log(Tags.DIRECTIONAL_ZOOM, $"targetXPosition: {targetXPosition}  currentXPosition: {_camera.Transform.Position.x}");

            // Find target camera y position
            float targetYPosition = _cameraCalculator.FindCameraYPosition(targetOrthographicSize);
            Logging.Log(Tags.DIRECTIONAL_ZOOM, $"targetYPosition: {targetYPosition}  currentYPosition: {_camera.Transform.Position.y}");

            return
                new CameraTarget(
                    new Vector3(targetXPosition, targetYPosition, _camera.Transform.Position.z),
                    targetOrthographicSize);
        }

        public ICameraTarget ZoomIn(float orthographicSizeDelta, Vector3 contactPosition)
        {
            Logging.Log(Tags.DIRECTIONAL_ZOOM, $"orthographicSizeDelta: {orthographicSizeDelta}");

            // Find target camera orthographic size
            float targetOrthographicSize = _camera.OrthographicSize - orthographicSizeDelta;
            targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, _validOrthographicSizes.Min, _validOrthographicSizes.Max);
            Logging.Log(Tags.DIRECTIONAL_ZOOM, $"targetOrthographicSize: {targetOrthographicSize}  currentOrthographicSize: {_camera.OrthographicSize}");

            // Find target camera x position, zoom towards contact point
            Vector3 contactWorldPosition = _camera.ScreenToWorldPoint(contactPosition);
            Vector3 contactViewportPosition = _camera.WorldToViewportPoint(contactWorldPosition);
            Vector3 contactZoomPosition
                = _cameraCalculator.FindZoomingCameraPosition(
                    contactWorldPosition,
                    contactViewportPosition,
                    targetOrthographicSize,
                    _camera.Aspect,
                    _camera.Transform.Position.z);
            Logging.Log(Tags.DIRECTIONAL_ZOOM, $"contactWorldPosition: {contactWorldPosition}  contactZoomPosition: {contactZoomPosition}");

            IRange<float> validXPositions = _cameraCalculator.FindValidCameraXPositions(targetOrthographicSize);
            float targetXPosition = Mathf.Clamp(contactZoomPosition.x, validXPositions.Min, validXPositions.Max);
            Logging.Log(Tags.DIRECTIONAL_ZOOM, $"targetXPosition: {targetXPosition}  currentXPosition: {_camera.Transform.Position.x}");

            // Find target camera y position
            float targetYPosition = _cameraCalculator.FindCameraYPosition(targetOrthographicSize);
            Logging.Log(Tags.DIRECTIONAL_ZOOM, $"targetYPosition: {targetYPosition}  currentYPosition: {_camera.Transform.Position.y}");

            return
                new CameraTarget(
                    new Vector3(targetXPosition, targetYPosition, _camera.Transform.Position.z),
                    targetOrthographicSize);
        }
    }
}