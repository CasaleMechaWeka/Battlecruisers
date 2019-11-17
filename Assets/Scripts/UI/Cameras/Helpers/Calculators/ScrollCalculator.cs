using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityCommon.PlatformAbstractions;

namespace BattleCruisers.UI.Cameras.Helpers.Calculators
{
    public class ScrollCalculator : IScrollCalculator
    {
        private readonly ICamera _camera;
        private readonly ITime _time;
        private readonly IRange<float> _validOrthographicSizes;
        private readonly ISettingsManager _settingsManager;
        private readonly ILevelToMultiplierConverter _scrollLevelConverter;

        public const float SCROLL_SCALE = 16;

        public ScrollCalculator(
            ICamera camera,
            ITime time,
            IRange<float> validOrthographicSizes,
            ISettingsManager settingsManager,
            ILevelToMultiplierConverter scrollLevelConverter)
        {
            Helper.AssertIsNotNull(camera, time, validOrthographicSizes, settingsManager, scrollLevelConverter);

            _camera = camera;
            _time = time;
            _validOrthographicSizes = validOrthographicSizes;
            _settingsManager = settingsManager;
            _scrollLevelConverter = scrollLevelConverter;
        }

        public float FindScrollDelta(float swipeDeltaX)
        {
            // The more zoomed out the camera is, the greater our delta should be
            float orthographicProportion = _camera.OrthographicSize / _validOrthographicSizes.Max;
            // Direction should be inverted, so swiping left should move the screen right
            float directionMultiplier = -1;

            return
                swipeDeltaX *
                directionMultiplier *
                orthographicProportion *
                SCROLL_SCALE *
                _time.UnscaledDeltaTime *
                _scrollLevelConverter.LevelToMultiplier(_settingsManager.ScrollSpeedLevel);
        }
    }
}