using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Targets;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Helpers
{
    // FELIX  Update tests :)
    public class CameraNavigationWheelCalculator : ICameraNavigationWheelCalculator
    {
        private readonly INavigationWheelPanel _navigationWheelPanel;
        private readonly ICameraCalculator _cameraCalculator;
        private readonly IRange<float> _validOrthographicSizeRange;

        public CameraNavigationWheelCalculator(
            INavigationWheelPanel navigationWheelPanel,
            ICameraCalculator cameraCalculator,
            IRange<float> validOrthographicSizeRange)
        {
            Helper.AssertIsNotNull(navigationWheelPanel, cameraCalculator, validOrthographicSizeRange);

            _navigationWheelPanel = navigationWheelPanel;
            _cameraCalculator = cameraCalculator;
            _validOrthographicSizeRange = validOrthographicSizeRange;
        }

        public float FindOrthographicSize()
        {
            return FindProportionalValue(_navigationWheelPanel.FindYProportion(), _validOrthographicSizeRange);
        }

        public Vector2 FindCameraPosition()
        {
            float desiredOrthographicSize = FindOrthographicSize();
            float desiredCameraYPosition = _cameraCalculator.FindCameraYPosition(desiredOrthographicSize);

            IRange<float> validCameraXPositions = _cameraCalculator.FindValidCameraXPositions(desiredOrthographicSize);
            float xProportion = _navigationWheelPanel.FindXProportion();
            float desiredCameraXPosition = FindProportionalValue(xProportion, validCameraXPositions);

            return new Vector2(desiredCameraXPosition, desiredCameraYPosition);
        }

        // FELIX  Test needing the clamps :)
        public Vector2 FindNavigationWheelPosition(ICameraTarget cameraTarget)
        {
            Assert.IsNotNull(cameraTarget);

            // Find y-position from camera orthographic size
            float clampedOrthographicSize = Mathf.Clamp(cameraTarget.OrthographicSize, _validOrthographicSizeRange.Min, _validOrthographicSizeRange.Max);
            float orthographicSizeProportion = FindProportionalValue(clampedOrthographicSize, _validOrthographicSizeRange);
            float navigationWheelYPosition = _navigationWheelPanel.FindYPosition(orthographicSizeProportion);

            // Find x-position from camera x-position
            IRange<float> validCameraXPositions = _cameraCalculator.FindValidCameraXPositions(clampedOrthographicSize);
            float clampedXPosion = Mathf.Clamp(cameraTarget.Position.x, validCameraXPositions.Min, validCameraXPositions.Max);
            float xPositionProportion = FindProportionalValue(clampedXPosion, validCameraXPositions);
            float navigationWheelXPosition = _navigationWheelPanel.FindXPosition(xPositionProportion, navigationWheelYPosition);

            return new Vector2(navigationWheelXPosition, navigationWheelYPosition);
        }

        private float FindProportionalValue(float proportion, IRange<float> valueRange)
        {
            Assert.IsTrue(proportion >= 0);
            Assert.IsTrue(proportion <= 1);

            float valueDifference = valueRange.Max - valueRange.Min;
            float valueOffset = proportion * valueDifference;
            return valueRange.Min + valueOffset;
        }
    }
}