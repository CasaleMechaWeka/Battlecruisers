using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityCommon.PlatformAbstractions;

namespace BattleCruisers.UI.Cameras.Helpers.Calculators
{
    // FELIX  Test :)
    public class ScrollCalculator : IScrollCalculator
    {
        private readonly ICamera _camera;
        private readonly IDeltaTimeProvider _deltaTimeProvider;
        private readonly IRange<float> _validOrthographicSizes;
        private readonly ISettingsManager _settingsManager;
        private readonly ILevelToMultiplierConverter _scrollLevelConverter;

        public const float SCROLL_SCALE = 32;

        public ScrollCalculator(
            ICamera camera,
            IDeltaTimeProvider deltaTimeProvider,
            IRange<float> validOrthographicSizes,
            ISettingsManager settingsManager,
            ILevelToMultiplierConverter scrollLevelConverter)
        {
            Helper.AssertIsNotNull(camera, deltaTimeProvider, validOrthographicSizes, settingsManager, scrollLevelConverter);

            _camera = camera;
            _deltaTimeProvider = deltaTimeProvider;
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
                _deltaTimeProvider.UnscaledDeltaTime *
                _scrollLevelConverter.LevelToMultiplier(_settingsManager.ScrollSpeedLevel);
        }
    }
}