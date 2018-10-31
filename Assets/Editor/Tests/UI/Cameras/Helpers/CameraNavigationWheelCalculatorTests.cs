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

        [SetUp]
        public void TestSetup()
        {
            _navigationWheelPanel = Substitute.For<INavigationWheelPanel>();
            _cameraCalculator = Substitute.For<ICameraCalculator>();
            _validOrthographicSizeRange = Substitute.For<IRange<float>>();

            _cameraNavigationWheelCalculator = new CameraNavigationWheelCalculator(_navigationWheelPanel, _cameraCalculator, _validOrthographicSizeRange);

            UnityAsserts.Assert.raiseExceptions = true;
        }

        #region FindOrthographicSize
        [Test]
        public void FindOrthographicSize_TooSmallProportion_Throws()
        {
            _navigationWheelPanel.FindYProportion().Returns(-0.01f);
            Assert.Throws<UnityAsserts.AssertionException>(() => _cameraNavigationWheelCalculator.FindOrthographicSize());
        }

        [Test]
        public void FindOrthographicSize_TooBigProportion_Throws()
        {
            _navigationWheelPanel.FindYProportion().Returns(1.01f);
            Assert.Throws<UnityAsserts.AssertionException>(() => _cameraNavigationWheelCalculator.FindOrthographicSize());
        }

        [Test]
        public void FindOrthographicSize_ValidProportion_FindsOrthographicSize()
        {
            _navigationWheelPanel.FindYProportion().Returns(0.5f);
            _validOrthographicSizeRange.Min.Returns(5);
            _validOrthographicSizeRange.Max.Returns(15);

            Assert.AreEqual(10, _cameraNavigationWheelCalculator.FindOrthographicSize());
        }
        #endregion FindOrthographicSize

        #region FindCameraPosition
        [Test]
        public void FindCameraPosition()
        {
            // Calls FindOrthographicSize() :)
            _navigationWheelPanel.FindYProportion().Returns(0.5f);
            _validOrthographicSizeRange.Min.Returns(5);
            _validOrthographicSizeRange.Max.Returns(15);
            float desiredOrthographicSize = 10;

            float desiredCameraYPosition = 17;
            _cameraCalculator.FindCameraYPosition(desiredOrthographicSize).Returns(desiredCameraYPosition);

            IRange<float> validCameraXPositions = new Range<float>(min: -4, max: -2);
            _cameraCalculator.FindValidCameraXPositions(desiredOrthographicSize).Returns(validCameraXPositions);

            float xProportion = 0.5f;
            _navigationWheelPanel.FindXProportion().Returns(xProportion);

            float desiredCameraXPosition = -3;
            Vector2 expectedCameraPosition = new Vector2(desiredCameraXPosition, desiredCameraYPosition);

            Assert.AreEqual(expectedCameraPosition, _cameraNavigationWheelCalculator.FindCameraPosition());
        }
        #endregion FindCameraPosition
    }
}