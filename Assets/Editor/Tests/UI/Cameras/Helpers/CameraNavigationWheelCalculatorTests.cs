using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Cameras.Helpers;
using BattleCruisers.Utils.DataStrctures;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.UI.Cameras.Helpers
{
    public class CameraNavigationWheelCalculatorTests
    {
        private ICameraNavigationWheelCalculator _cameraNavigationWheelCalculator;
        private INavigationWheelPanel _navigationWheelPanel;
        private ICameraCalculator _cameraCalculator;
        private IRange<float> _validOrthographicSizeRange;
        private IProportionCalculator _proportionCalculator;

        [SetUp]
        public void TestSetup()
        {
            _navigationWheelPanel = Substitute.For<INavigationWheelPanel>();
            _cameraCalculator = Substitute.For<ICameraCalculator>();
            _validOrthographicSizeRange = Substitute.For<IRange<float>>();
            _proportionCalculator = Substitute.For<IProportionCalculator>();

            _cameraNavigationWheelCalculator 
                = new CameraNavigationWheelCalculator(
                    _navigationWheelPanel, 
                    _cameraCalculator, 
                    _validOrthographicSizeRange, 
                    _proportionCalculator);

            UnityAsserts.Assert.raiseExceptions = true;
        }

        [Test]
        public void FindOrthographicSize_ValidProportion_FindsOrthographicSize()
        {
            float yProportion = 0.5f;
            _navigationWheelPanel.FindYProportion().Returns(yProportion);
            _validOrthographicSizeRange.Min.Returns(5);
            _validOrthographicSizeRange.Max.Returns(15);

            float expectedProportion = 10;
            _proportionCalculator.FindProportionalValue(yProportion, _validOrthographicSizeRange).Returns(expectedProportion);

            Assert.AreEqual(expectedProportion, _cameraNavigationWheelCalculator.FindOrthographicSize());
        }

        [Test]
        public void FindCameraPosition()
        {
            // Calls FindOrthographicSize() :)
            float yProportion = 0.5f;
            _navigationWheelPanel.FindYProportion().Returns(yProportion);
            _validOrthographicSizeRange.Min.Returns(5);
            _validOrthographicSizeRange.Max.Returns(15);
            float desiredOrthographicSize = 10;
            _proportionCalculator.FindProportionalValue(yProportion, _validOrthographicSizeRange).Returns(desiredOrthographicSize);

            float desiredCameraYPosition = 17;
            _cameraCalculator.FindCameraYPosition(desiredOrthographicSize).Returns(desiredCameraYPosition);

            IRange<float> validCameraXPositions = new Range<float>(min: -4, max: -2);
            _cameraCalculator.FindValidCameraXPositions(desiredOrthographicSize).Returns(validCameraXPositions);

            float xProportion = 0.5f;
            _navigationWheelPanel.FindXProportion().Returns(xProportion);

            float desiredCameraXPosition = -3;
            _proportionCalculator.FindProportionalValue(xProportion, validCameraXPositions).Returns(desiredCameraXPosition);

            Vector2 expectedCameraPosition = new Vector2(desiredCameraXPosition, desiredCameraYPosition);

            Assert.AreEqual(expectedCameraPosition, _cameraNavigationWheelCalculator.FindCameraPosition());
        }

        [Test]
        public void FindNavigationWheelPosition()
        {
            // FELIX  :D
        }
    }
}