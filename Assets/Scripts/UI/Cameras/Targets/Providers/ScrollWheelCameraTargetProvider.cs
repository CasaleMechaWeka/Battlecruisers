using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using System;
using UnityEngine;

namespace BattleCruisers.UI.Cameras.Targets.Providers
{
    // FELIX  Use
    // FELIX  Test
    // FELIX  Avoid duplicate code with NavigationWheelCameraTargetProvider :)  (Target property :P)
    public class ScrollWheelCameraTargetProvider : ICameraTargetProvider
    {
        private readonly ICamera _camera;
        private readonly ICameraCalculator _cameraCalculator;
        private readonly IInput _input;
        private readonly IRange<float> _validOrthographicSizes;
        private readonly IUpdater _updater;
        private readonly IDeltaTimeProvider _deltaTimeProvider;

        // FELIX  Adjust?
        private const float SCROLL_SCALE = 2400;

        private ICameraTarget _target;
        public ICameraTarget Target
        {
            get { return _target; }
            set
            {
                if (_target != value)
                {
                    _target = value;

                    TargetChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        public event EventHandler TargetChanged;

        public ScrollWheelCameraTargetProvider(
            ICamera camera,
            ICameraCalculator cameraCalculator,
            IInput input, 
            IRange<float> validOrthographicSizes, 
            IUpdater updater,
            IDeltaTimeProvider deltaTimeProvider)
        {
            Helper.AssertIsNotNull(camera, cameraCalculator, input, validOrthographicSizes, updater, deltaTimeProvider);

            _camera = camera;
            _cameraCalculator = cameraCalculator;
            _input = input;
            _validOrthographicSizes = validOrthographicSizes;
            _updater = updater;
            _deltaTimeProvider = deltaTimeProvider;

            _updater.Updated += _updater_Updated;

            Target = new CameraTarget(camera.Transform.Position, camera.OrthographicSize);
        }

        private void _updater_Updated(object sender, EventArgs e)
        {
            if (_input.MouseScrollDelta.y == 0)
            {
                return;
            }

            // FELIX  Abstract?
            float orthographicProportion = _camera.OrthographicSize / _validOrthographicSizes.Max;
            float scrollDelta = Mathf.Abs(_input.MouseScrollDelta.y) * orthographicProportion * SCROLL_SCALE * _deltaTimeProvider.UnscaledDeltaTime;

            if (_input.MouseScrollDelta.y < 0)
            {
                Target = ZoomOut(scrollDelta);
            }
            else
            {
                Target = ZoomIn(scrollDelta);
            }
        }

        private ICameraTarget ZoomOut(float scrollDelta)
        {
            Logging.Log(Tags.SCROLL_WHEEL_NAVIGATION, $"scrollDelta: {scrollDelta}");

            // Find target camera orthographic size
            float targetOrthographicSize = _camera.OrthographicSize + scrollDelta;
            targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, _validOrthographicSizes.Min, _validOrthographicSizes.Max);
            Logging.Log(Tags.SCROLL_WHEEL_NAVIGATION, $"targetOrthographicSize: {targetOrthographicSize}  currentOrthographicSize: {_camera.OrthographicSize}");

            // Find target camera x position
            IRange<float> validXPositions = _cameraCalculator.FindValidCameraXPositions(targetOrthographicSize);
            // FELIX  Check passing Min & Max = 0 does not throw for Clamp() :P
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
            Vector3 mouseZoomPosition
                = _cameraCalculator.FindZoomingCameraPosition(
                    mouseWorldPosition,
                    _camera.WorldToViewportPoint(mouseWorldPosition),
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

            // Want zoom target to remain in the same location in the viewport
            //Vector2 cameraTargetPosition = new Vector2(targetXPosition, targetYPosition);
            //Vector3 zoomTargetPosition
            //    = _cameraCalculator.FindZoomingCameraPosition(
            //        cameraTargetPosition,
            //        _camera.WorldToViewportPoint(cameraTargetPosition),
            //        targetOrthographicSize,
            //        _camera.Aspect,
            //        _camera.Transform.Position.z);
            //Logging.Log(Tags.SCROLL_WHEEL_NAVIGATION, $"cameraTargetPosition: {cameraTargetPosition}  zoomTargetPosition: {zoomTargetPosition}");

            return
                new CameraTarget(
                    new Vector3(targetXPosition, targetYPosition, _camera.Transform.Position.z),
                    //zoomTargetPosition,
                    targetOrthographicSize);
        }
    }
}