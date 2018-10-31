using BattleCruisers.UI.BattleScene.Navigation;
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
            return FindProportionalValue(_validOrthographicSizeRange, _navigationWheelPanel.FindYProportion());
        }

        public Vector2 FindCameraPosition()
        {
            float desiredOrthographicSize = FindOrthographicSize();
            float desiredCameraYPosition = _cameraCalculator.FindCameraYPosition(desiredOrthographicSize);

            IRange<float> validCameraXPositions = _cameraCalculator.FindValidCameraXPositions(desiredOrthographicSize);
            float xProportion = _navigationWheelPanel.FindXProportion();
            float desiredCameraXPosition = FindProportionalValue(validCameraXPositions, xProportion);

            return new Vector2(desiredCameraXPosition, desiredCameraYPosition);
        }

        private float FindProportionalValue(IRange<float> valueRange, float proportion)
        {
            Assert.IsTrue(proportion >= 0);
            Assert.IsTrue(proportion <= 1);

            float valueDifference = valueRange.Max - valueRange.Min;
            float valueOffset = proportion * valueDifference;
            return valueRange.Min + valueOffset;
        }
    }
}