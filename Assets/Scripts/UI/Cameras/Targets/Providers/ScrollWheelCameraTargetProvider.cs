using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using System;
using UnityEngine;

namespace BattleCruisers.UI.Cameras.Targets.Providers
{
    // FELIX  Implement :P
    public interface IMagic
    {

    }

    // FELIX  Implement :P
    public interface IUpdater
    {
        event EventHandler Update;
    }

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

        // FELIX  Adjust?
        private const float SCROLL_SCALE = 0.1f;

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
            IUpdater updater)
        {
            Helper.AssertIsNotNull(camera, cameraCalculator, input, validOrthographicSizes, updater);

            _camera = camera;
            _cameraCalculator = cameraCalculator;
            _input = input;
            _validOrthographicSizes = validOrthographicSizes;
            _updater = updater;

            _updater.Update += _updater_Updated;
        }

        private void _updater_Updated(object sender, EventArgs e)
        {
            if (_input.MouseScrollDelta.y == 0)
            {
                return;
            }

            float scrollDelta = Mathf.Abs(_input.MouseScrollDelta.y) * SCROLL_SCALE;

            if (_input.MouseScrollDelta.y > 0)
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
            // Find target camera orthographic size
            float targetOrthographicSize = _camera.OrthographicSize + scrollDelta;
            if (targetOrthographicSize > _validOrthographicSizes.Max)
            {
                targetOrthographicSize = _validOrthographicSizes.Max;
            }

            // Find target camera x position
            IRange<float> validXPositions = _cameraCalculator.FindValidCameraXPositions(targetOrthographicSize);
            // FELIX  Check passing Min & Max = 0 does not throw for Clamp() :P
            float targetXPosition = Mathf.Clamp(_camera.Transform.Position.x, validXPositions.Min, validXPositions.Max);

            // Find target camera y position
            float targetYPosition = _cameraCalculator.FindCameraYPosition(targetOrthographicSize);

            return
                new CameraTarget(
                    new Vector3(targetXPosition, targetYPosition, _camera.Transform.Position.z),
                    targetOrthographicSize);
        }

        private ICameraTarget ZoomIn(float scrollDelta)
        {
            // Find target camera orthographic size
            float targetOrthographicSize = _camera.OrthographicSize - scrollDelta;
            if (targetOrthographicSize > _validOrthographicSizes.Min)
            {
                targetOrthographicSize = _validOrthographicSizes.Min;
            }

            // Find target camera x position, zoom towards mouse
            IRange<float> validXPositions = _cameraCalculator.FindValidCameraXPositions(targetOrthographicSize);
            float targetXPosition = Mathf.Clamp(_input.MousePosition.x, validXPositions.Min, validXPositions.Max);

            // Find target camera y position
            float targetYPosition = _cameraCalculator.FindCameraYPosition(targetOrthographicSize);

            return
                new CameraTarget(
                    new Vector3(targetXPosition, targetYPosition, _camera.Transform.Position.z),
                    targetOrthographicSize);
        }
    }
}