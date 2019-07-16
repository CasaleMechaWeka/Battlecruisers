using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using System;
using UnityEngine;

namespace BattleCruisers.UI.Cameras.Targets.Providers
{
    public class ScrollWheelCameraTargetProvider : UserInputCameraTargetProvider
    {
        private readonly ICamera _camera;
        private readonly ICameraCalculator _cameraCalculator;
        private readonly IInput _input;
        private readonly IRange<float> _validOrthographicSizes;
        private readonly IUpdater _updater;
        private readonly IZoomCalculator _zoomCalculator;
        private bool _duringUserInput;

        public ScrollWheelCameraTargetProvider(
            ICamera camera,
            ICameraCalculator cameraCalculator,
            IInput input, 
            IRange<float> validOrthographicSizes, 
            IUpdater updater,
            IZoomCalculator zoomCalculator)
        {
            Helper.AssertIsNotNull(camera, cameraCalculator, input, validOrthographicSizes, updater, zoomCalculator);

            _camera = camera;
            _cameraCalculator = cameraCalculator;
            _input = input;
            _validOrthographicSizes = validOrthographicSizes;
            _updater = updater;
            _zoomCalculator = zoomCalculator;
            _duringUserInput = false;

            _updater.Updated += _updater_Updated;

            Target = new CameraTarget(camera.Transform.Position, camera.OrthographicSize);
        }

        private void _updater_Updated(object sender, EventArgs e)
        {
            if (_input.MouseScrollDelta.y == 0)
            {
                if (_duringUserInput)
                {
                    _duringUserInput = false;
                    RaiseUserInputEnded();
                }

                return;
            }

            if (!_duringUserInput)
            {
                _duringUserInput = true;
                RaiseUserInputStarted();
            }

            float zoomDelta = _zoomCalculator.FindZoomDelta(_input.MouseScrollDelta.y);

            if (_input.MouseScrollDelta.y < 0)
            {
                Target = ZoomOut(zoomDelta);
            }
            else
            {
                Target = ZoomIn(zoomDelta);
            }
        }

        // FELIX  Avoid duplicate code with SwipeCameraTargetProvider :)
        private ICameraTarget ZoomOut(float scrollDelta)
        {
            Logging.Log(Tags.SCROLL_WHEEL_NAVIGATION, $"scrollDelta: {scrollDelta}");

            // Find target camera orthographic size
            float targetOrthographicSize = _camera.OrthographicSize + scrollDelta;
            targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, _validOrthographicSizes.Min, _validOrthographicSizes.Max);
            Logging.Log(Tags.SCROLL_WHEEL_NAVIGATION, $"targetOrthographicSize: {targetOrthographicSize}  currentOrthographicSize: {_camera.OrthographicSize}");

            // Find target camera x position
            IRange<float> validXPositions = _cameraCalculator.FindValidCameraXPositions(targetOrthographicSize);
            float targetXPosition = Mathf.Clamp(_camera.Transform.Position.x, validXPositions.Min, validXPositions.Max);
            Logging.Log(Tags.SCROLL_WHEEL_NAVIGATION, $"targetXPosition: {targetXPosition}  currentXPosition: {_camera.Transform.Position.x}");

            // Find target camera y position
            float targetYPosition = _cameraCalculator.FindCameraYPosition(targetOrthographicSize);
            Logging.Log(Tags.SCROLL_WHEEL_NAVIGATION, $"targetYPosition: {targetYPosition}  currentYPosition: {_camera.Transform.Position.y}");

            return
                new CameraTarget(
                    new Vector3(targetXPosition, targetYPosition, _camera.Transform.Position.z),
                    targetOrthographicSize);
        }

        private ICameraTarget ZoomIn(float scrollDelta)
        {
            Logging.Log(Tags.SCROLL_WHEEL_NAVIGATION, $"scrollDelta: {scrollDelta}");

            // Find target camera orthographic size
            float targetOrthographicSize = _camera.OrthographicSize - scrollDelta;
            targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, _validOrthographicSizes.Min, _validOrthographicSizes.Max);
            Logging.Log(Tags.SCROLL_WHEEL_NAVIGATION, $"targetOrthographicSize: {targetOrthographicSize}  currentOrthographicSize: {_camera.OrthographicSize}");

            // Find target camera x position, zoom towards mouse
            Vector3 mouseWorldPosition = _camera.ScreenToWorldPoint(_input.MousePosition);
            Vector3 mouseViewportPosition = _camera.WorldToViewportPoint(mouseWorldPosition);
            Vector3 mouseZoomPosition
                = _cameraCalculator.FindZoomingCameraPosition(
                    mouseWorldPosition,
                    mouseViewportPosition,
                    targetOrthographicSize,
                    _camera.Aspect,
                    _camera.Transform.Position.z);
            Logging.Log(Tags.SCROLL_WHEEL_NAVIGATION, $"mouseWorldPosition: {mouseWorldPosition}  mouseZoomPosition: {mouseZoomPosition}");

            IRange<float> validXPositions = _cameraCalculator.FindValidCameraXPositions(targetOrthographicSize);
            float targetXPosition = Mathf.Clamp(mouseZoomPosition.x, validXPositions.Min, validXPositions.Max);
            Logging.Log(Tags.SCROLL_WHEEL_NAVIGATION, $"targetXPosition: {targetXPosition}  currentXPosition: {_camera.Transform.Position.x}");

            // Find target camera y position
            float targetYPosition = _cameraCalculator.FindCameraYPosition(targetOrthographicSize);
            Logging.Log(Tags.SCROLL_WHEEL_NAVIGATION, $"targetYPosition: {targetYPosition}  currentYPosition: {_camera.Transform.Position.y}");

            return
                new CameraTarget(
                    new Vector3(targetXPosition, targetYPosition, _camera.Transform.Position.z),
                    targetOrthographicSize);
        }
    }
}