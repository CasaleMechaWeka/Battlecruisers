using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers.Calculators;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Targets;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.DataStrctures;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers
{
    public class PvPDirectionalZoom : IPvPDirectionalZoom
    {
        private readonly IPvPCamera _camera;
        private readonly IPvPCameraCalculator _cameraCalculator;
        private readonly IPvPRange<float> _validOrthographicSizes;

        public PvPDirectionalZoom(IPvPCamera camera, IPvPCameraCalculator cameraCalculator, IPvPRange<float> validOrthographicSizes)
        {
            PvPHelper.AssertIsNotNull(camera, cameraCalculator, validOrthographicSizes);

            _camera = camera;
            _cameraCalculator = cameraCalculator;
            _validOrthographicSizes = validOrthographicSizes;
        }

        public IPvPCameraTarget ZoomOut(float orthographicSizeDelta)
        {
            // Logging.Verbose(Tags.DIRECTIONAL_ZOOM, $"orthographicSizeDelta: {orthographicSizeDelta}");

            // Find target camera orthographic size
            float targetOrthographicSize = _camera.OrthographicSize + orthographicSizeDelta;
            targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, _validOrthographicSizes.Min, _validOrthographicSizes.Max);
            // Logging.Verbose(Tags.DIRECTIONAL_ZOOM, $"targetOrthographicSize: {targetOrthographicSize}  currentOrthographicSize: {_camera.OrthographicSize}");

            // Find target camera x position
            IPvPRange<float> validXPositions = _cameraCalculator.FindValidCameraXPositions(targetOrthographicSize);
            float targetXPosition = Mathf.Clamp(_camera.Position.x, validXPositions.Min, validXPositions.Max);
            // Logging.Verbose(Tags.DIRECTIONAL_ZOOM, $"targetXPosition: {targetXPosition}  currentXPosition: {_camera.Position.x}");

            // Find target camera y position
            float targetYPosition = _cameraCalculator.FindCameraYPosition(targetOrthographicSize);
            // Logging.Verbose(Tags.DIRECTIONAL_ZOOM, $"targetYPosition: {targetYPosition}  currentYPosition: {_camera.Position.y}");

            return
                new PvPCameraTarget(
                    new Vector3(targetXPosition, targetYPosition, _camera.Position.z),
                    targetOrthographicSize);
        }

        public IPvPCameraTarget ZoomIn(float orthographicSizeDelta, Vector3 contactPosition)
        {
            // Logging.Verbose(Tags.DIRECTIONAL_ZOOM, $"orthographicSizeDelta: {orthographicSizeDelta}");

            // Find target camera orthographic size
            float targetOrthographicSize = _camera.OrthographicSize - orthographicSizeDelta;
            targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, _validOrthographicSizes.Min, _validOrthographicSizes.Max);
            // Logging.Verbose(Tags.DIRECTIONAL_ZOOM, $"targetOrthographicSize: {targetOrthographicSize}  currentOrthographicSize: {_camera.OrthographicSize}");

            // Find target camera x position, zoom towards contact point
            Vector3 contactWorldPosition = _camera.ScreenToWorldPoint(contactPosition);
            Vector3 contactViewportPosition = _camera.WorldToViewportPoint(contactWorldPosition);
            Vector3 contactZoomPosition
                = _cameraCalculator.FindZoomingCameraPosition(
                    contactWorldPosition,
                    contactViewportPosition,
                    targetOrthographicSize,
                    _camera.Aspect,
                    _camera.Position.z);
            // Logging.Verbose(Tags.DIRECTIONAL_ZOOM, $"contactWorldPosition: {contactWorldPosition}  contactZoomPosition: {contactZoomPosition}");

            IPvPRange<float> validXPositions = _cameraCalculator.FindValidCameraXPositions(targetOrthographicSize);
            float targetXPosition = Mathf.Clamp(contactZoomPosition.x, validXPositions.Min, validXPositions.Max);
            // Logging.Verbose(Tags.DIRECTIONAL_ZOOM, $"targetXPosition: {targetXPosition}  currentXPosition: {_camera.Position.x}");

            // Find target camera y position
            float targetYPosition = _cameraCalculator.FindCameraYPosition(targetOrthographicSize);
            // Logging.Verbose(Tags.DIRECTIONAL_ZOOM, $"targetYPosition: {targetYPosition}  currentYPosition: {_camera.Position.y}");

            return
                new PvPCameraTarget(
                    new Vector3(targetXPosition, targetYPosition, _camera.Position.z),
                    targetOrthographicSize);
        }
    }
}