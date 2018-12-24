using BattleCruisers.Data.Settings;
using BattleCruisers.Utils.DataStrctures;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Helpers
{
    public class CameraCalculatorSettings : ICameraCalculatorSettings
    {
        private readonly ISettingsManager _settingsManager;

        private const float DEFAULT_ASPECT_RATIO = 1.333f;  // 4/3
        // Just large enough to see both cruisers fully :)
        private const float DEFAULT_MAX_ORTHOGRAPHIC_SIZE = 33;  

        public float CruiserWidthMultiplier { get { return 1.2f; } }
        public float CruiserCameraPositionAdjustmentMultiplier { get { return 0.08f; } }

        public float WaterProportion { get { return 0.35f; } }
        public float MaxWaterPositionY { get { return -1.5f; } }

        // Based off the two points:  (5, 10) and (33, 50)
        public float ScrollSpeedGradient { get { return 1.43f; } }  // 10/7
        public float ScrollSpeedConstant { get { return 2.86f; } }  // 20/7
        public float ScrollSpeed { get { return _settingsManager.ScrollSpeed; } }

        public IRange<float> ValidOrthographicSizes { get; private set; }
        public IRange<float> CameraVisibleXRange { get; private set; }

        public CameraCalculatorSettings(ISettingsManager settingsManager, float cameraAspectRatio)
        {
            Assert.IsNotNull(settingsManager);

            _settingsManager = settingsManager;

            float adjustmentMultiplier = DEFAULT_ASPECT_RATIO / cameraAspectRatio;
            ValidOrthographicSizes = new Range<float>(min: 5, max: DEFAULT_MAX_ORTHOGRAPHIC_SIZE * adjustmentMultiplier);

            CameraVisibleXRange = FindCameraVisiableXRange(cameraAspectRatio);
        }

        // Assumes camera aspect ratio remains constant
        private IRange<float> FindCameraVisiableXRange(float cameraAspectRatio)
        {
            float maxHeight = 2 * ValidOrthographicSizes.Max;
            float maxWidth = maxHeight * cameraAspectRatio;
            float halfMaxWidth = maxWidth / 2;
            return new Range<float>(-halfMaxWidth, halfMaxWidth);
        }
    }
}