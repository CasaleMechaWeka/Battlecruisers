using BattleCruisers.Cruisers;
using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Cameras.Helpers.Calculators;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityAsserts = UnityEngine.Assertions;

namespace BattleCruisers.Tests.UI.Cameras.Helpers.Calculators
{
    public class CameraCalculatorTests
    {
        private ICameraCalculator _calculator;
        private ICameraCalculatorSettings _settings;
        private ICamera _camera;
        private ICruiser _cruiser;

        [SetUp]
        public void SetuUp()
        {
            _camera = Substitute.For<ICamera>();
            _camera.Aspect.Returns(1.333333f);  // 4/3

            ISettingsManager settingsManager = Substitute.For<ISettingsManager>();
            settingsManager.ScrollSpeedLevel.Returns(3);

            _settings = new CameraCalculatorSettings(settingsManager, _camera.Aspect);

            _calculator = new CameraCalculator(_camera, _settings);

            _cruiser = Substitute.For<ICruiser>();
            _cruiser.Size.Returns(new Vector2(123, 123));

            UnityAsserts.Assert.raiseExceptions = true;
        }

        [Test]
        public void FindCameraOrthographicSize()
        {
            float desiredWidth = _cruiser.Size.x * _settings.CruiserWidthMultiplier;
            float desiredHeight = desiredWidth / _camera.Aspect;
            float expectedOrthographicSize = desiredHeight / 2;

            Assert.IsTrue(Mathf.Approximately(expectedOrthographicSize, _calculator.FindCameraOrthographicSize(_cruiser)));
        }

        [Test]
        public void FindCameraOrthographicSize_BelowMinClamps()
        {
            _cruiser.Size.Returns(new Vector2(1.2f, 12));
            Assert.IsTrue(Mathf.Approximately(_settings.ValidOrthographicSizes.Min, _calculator.FindCameraOrthographicSize(_cruiser)));
        }

        [Test]
        public void FindCameraYPosition()
        {
            float desiredOrthographicSize = 1.5f;
            float desiredHeight = 2 * desiredOrthographicSize;
            float expectedYPosition = desiredOrthographicSize + _settings.MaxWaterPositionY - (_settings.WaterProportion * desiredHeight);

            Assert.IsTrue(Mathf.Approximately(expectedYPosition, _calculator.FindCameraYPosition(desiredOrthographicSize)));
        }

        [Test]
        public void FindScrollSpeed()
        {
            float orthographicSize = 1.5f;
            float timeDelta = 0.04f;
            float scrollSpeedPerS = _settings.ScrollSpeedGradient * orthographicSize + _settings.ScrollSpeedConstant;
            float expectedScrollSpeed = scrollSpeedPerS * timeDelta * _settings.ScrollSpeed;

            Assert.IsTrue(Mathf.Approximately(expectedScrollSpeed, _calculator.FindScrollSpeed(orthographicSize, timeDelta)));
        }

        [Test]
        public void FindCruiserCameraPosition_IsPlayerCruiser()
        {
            float orthographicSize = 1.5f;
            float zValue = -10;
            _cruiser.IsPlayerCruiser.Returns(true);

            float xAdjustmentMagnitudeInM = _cruiser.Size.x * _settings.CruiserCameraPositionAdjustmentMultiplier;

            Vector3 expectedPosition =
                new Vector3(
                    _cruiser.Position.x + xAdjustmentMagnitudeInM,
                    _calculator.FindCameraYPosition(orthographicSize),
                    zValue);

            Assert.AreEqual(expectedPosition, _calculator.FindCruiserCameraPosition(_cruiser, orthographicSize, zValue));
        }

        [Test]
        public void FindCruiserCameraPosition_IsNotPlayerCruiser()
        {
            float orthographicSize = 1.5f;
            float zValue = -10;
            _cruiser.IsPlayerCruiser.Returns(false);

            float xAdjustmentMagnitudeInM = _cruiser.Size.x * _settings.CruiserCameraPositionAdjustmentMultiplier;

            Vector3 expectedPosition =
                new Vector3(
                    _cruiser.Position.x - xAdjustmentMagnitudeInM,
                    _calculator.FindCameraYPosition(orthographicSize),
                    zValue);

            Assert.AreEqual(expectedPosition, _calculator.FindCruiserCameraPosition(_cruiser, orthographicSize, zValue));
        }

        [Test]
        public void FindZoomingCameraPosition_CameraWasSouthWest()
        {
            Vector2 zoomTarget = new Vector2(-5, 3);
            // From when camera was (-1, -1)
            Vector2 targetViewportPosition = new Vector2(0.2f, 0.9f);
            float cameraOrthographicSize = 3;
            float cameraAspectRatio = 1.3333f;
            float cameraPositionZ = -10;

            float cameraHeight = 2 * cameraOrthographicSize;
            float cameraWidth = cameraAspectRatio * cameraHeight;

            float expectedX = (cameraWidth / 2) + zoomTarget.x - (targetViewportPosition.x * cameraWidth);
            float expectedY = (cameraHeight / 2) + zoomTarget.y - (targetViewportPosition.y * cameraHeight);
            Vector3 expectedCameraPosition = new Vector3(expectedX, expectedY, cameraPositionZ);

            Vector3 actualCameraPosition 
                = _calculator.FindZoomingCameraPosition(
                    zoomTarget, 
                    targetViewportPosition, 
                    cameraOrthographicSize, 
                    cameraAspectRatio, 
                    cameraPositionZ);

            Debug.Log("expectedCameraPosition: " + expectedCameraPosition + "  actualCameraPosition: " + actualCameraPosition);
            Assert.AreEqual(expectedCameraPosition, actualCameraPosition);
        }

        [Test]
        public void FindZoomingCameraPosition_CameraWasNorthEast()
        {
            Vector2 zoomTarget = new Vector2(-5, 3);
            // From when camera was (1, 1)
            Vector2 targetViewportPosition = new Vector2(0.1f, 0.7f);
            float cameraOrthographicSize = 3;
            float cameraAspectRatio = 1.3333f;
            float cameraPositionZ = -10;

            float cameraHeight = 2 * cameraOrthographicSize;
            float cameraWidth = cameraAspectRatio * cameraHeight;

            float expectedX = (cameraWidth / 2) + zoomTarget.x - (targetViewportPosition.x * cameraWidth);
            float expectedY = (cameraHeight / 2) + zoomTarget.y - (targetViewportPosition.y * cameraHeight);
            Vector3 expectedCameraPosition = new Vector3(expectedX, expectedY, cameraPositionZ);

            Vector3 actualCameraPosition
                = _calculator.FindZoomingCameraPosition(
                    zoomTarget,
                    targetViewportPosition,
                    cameraOrthographicSize,
                    cameraAspectRatio,
                    cameraPositionZ);

            Debug.Log("expectedCameraPosition: " + expectedCameraPosition + "  actualCameraPosition: " + actualCameraPosition);
            Assert.AreEqual(expectedCameraPosition, actualCameraPosition);
        }

        #region FindValidCameraXPositions
        [Test]
        public void FindValidCameraXPositions_TooSmallOrthographicSize_DoesNotThrow()
        {
            _calculator.FindValidCameraXPositions(_settings.ValidOrthographicSizes.Min - 0.0001f);
        }

        [Test]
        public void FindValidCameraXPositions_TooLargeOrthographicSize_DoesNotThrow()
        {
            _calculator.FindValidCameraXPositions(_settings.ValidOrthographicSizes.Max + 0.0001f);
        }

        [Test]
        public void FindValidCameraXPositions_ValidOrthographicSize()
        {
            float desiredOrthographicSize = _settings.ValidOrthographicSizes.Max;

            float cameraHeight = 2 * desiredOrthographicSize;
            float cameraWidth = _camera.Aspect * cameraHeight;
            float halfCameraWidth = cameraWidth / 2;

            float minValidX = _settings.CameraVisibleXRange.Min + halfCameraWidth;
            float maxValidX = _settings.CameraVisibleXRange.Max - halfCameraWidth;

            IRange<float> expectedRange = new Range<float>(minValidX, maxValidX);
            Assert.AreEqual(expectedRange, _calculator.FindValidCameraXPositions(desiredOrthographicSize));
        }
        #endregion FindValidCameraXPositions
    }
}
