using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Helpers
{
    public class CameraNavigationWheelCalculator : ICameraNavigationWheelCalculator
    {
        private readonly INavigationWheelPanel _navigationWheelPanel;
        private readonly ICameraCalculator _cameraCalculator;
        private readonly IRange<float> _validOrthographicSizeRange;
        private readonly IProportionCalculator _proportionCalculator;

        public CameraNavigationWheelCalculator(
            INavigationWheelPanel navigationWheelPanel,
            ICameraCalculator cameraCalculator,
            IRange<float> validOrthographicSizeRange,
            IProportionCalculator proportionCalculator)
        {
            Helper.AssertIsNotNull(navigationWheelPanel, cameraCalculator, validOrthographicSizeRange, proportionCalculator);

            _navigationWheelPanel = navigationWheelPanel;
            _cameraCalculator = cameraCalculator;
            _validOrthographicSizeRange = validOrthographicSizeRange;
            _proportionCalculator = proportionCalculator;
        }

        public float FindOrthographicSize()
        {
            return _proportionCalculator.FindProportionalValue(_navigationWheelPanel.FindYProportion(), _validOrthographicSizeRange);
        }

        public Vector2 FindCameraPosition()
        {
            float desiredOrthographicSize = FindOrthographicSize();
            float desiredCameraYPosition = _cameraCalculator.FindCameraYPosition(desiredOrthographicSize);

            IRange<float> validCameraXPositions = _cameraCalculator.FindValidCameraXPositions(desiredOrthographicSize);
            float xProportion = _navigationWheelPanel.FindXProportion();
            float desiredCameraXPosition = _proportionCalculator.FindProportionalValue(xProportion, validCameraXPositions);

            return new Vector2(desiredCameraXPosition, desiredCameraYPosition);
        }

        public Vector2 FindNavigationWheelPosition(ICameraTarget cameraTarget)
        {
            Assert.IsNotNull(cameraTarget);

            // Find y-position from camera orthographic size
            float orthographicSizeProportion = _proportionCalculator.FindProportion(cameraTarget.OrthographicSize, _validOrthographicSizeRange);
            float navigationWheelYPosition = _navigationWheelPanel.FindYPosition(orthographicSizeProportion);

            // Find x-position from camera x-position
            float clampedOrthographicSize = Mathf.Clamp(cameraTarget.OrthographicSize, _validOrthographicSizeRange.Min, _validOrthographicSizeRange.Max);
            IRange<float> validCameraXPositions = _cameraCalculator.FindValidCameraXPositions(clampedOrthographicSize);
            float xPositionProportion = _proportionCalculator.FindProportion(cameraTarget.Position.x, validCameraXPositions);
            float navigationWheelXPosition = _navigationWheelPanel.FindXPosition(xPositionProportion, navigationWheelYPosition);

            Vector2 navigationWheelPosition = new Vector2(navigationWheelXPosition, navigationWheelYPosition);
            Logging.Log(Tags.CAMERA_NAVIGATION_WHEEL_CALCULATOR, $"cameraTarget: {cameraTarget}  navigationWheelPosition: {navigationWheelPosition}");
            return navigationWheelPosition;
        }
    }
}