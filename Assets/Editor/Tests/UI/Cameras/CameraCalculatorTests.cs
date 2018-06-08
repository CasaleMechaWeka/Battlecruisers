using BattleCruisers.Cruisers;
using BattleCruisers.Data.Settings;
using BattleCruisers.UI.Cameras;
using BattleCruisers.Utils.PlatformAbstractions;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace BattleCruisers.Tests.UI.Cameras
{
    public class CameraCalculatorTests
    {
        private ICameraCalculator _calculator;

        private ICamera _camera;
        private ISettingsManager _settingsManager;
        private ICruiser _cruiser;

        // Copied from CameraCalculator
        private const float CRUISER_WIDTH_MULTIPLIER = 1.2f;
        private const float CRUISER_CAMERA_POSITION_ADJUSTMENT_MULTIPLIER = 0.08f;
        private const float WATER_RATIO = 0.35f;
        private const float MAX_WATER_Y = -1.5f;
        private const float SCROLL_SPEED_GRADIENT = 1.43f;  // 10/7
        private const float SCROLL_SPEED_CONSTANT = 2.86f;  // 20/7

        [SetUp]
        public void SetuUp()
        {
            _camera = Substitute.For<ICamera>();
            _camera.Aspect.Returns(5);

            _settingsManager = Substitute.For<ISettingsManager>();

            _calculator = new CameraCalculator(_camera, _settingsManager);

            _cruiser = Substitute.For<ICruiser>();
        }

        [Test]
        public void FindCameraOrthographicSize()
        {
            _cruiser.Size.Returns(new Vector2(123, 123));

            float desiredWidth = _cruiser.Size.x * CRUISER_WIDTH_MULTIPLIER;
            float desiredHeight = desiredWidth / _camera.Aspect;
            float expectedOrthographicSize = desiredHeight / 2;

            Assert.AreEqual(expectedOrthographicSize, _calculator.FindCameraOrthographicSize(_cruiser));
        }

        [Test]
        public void FindCameraOrthographicSize_BelowMinClamps()
        {
            _cruiser.Size.Returns(new Vector2(1.2f, 12));
            Assert.AreEqual(CameraCalculator.MIN_CAMERA_ORTHOGRAPHIC_SIZE, _calculator.FindCameraOrthographicSize(_cruiser));
        }
    }
}
