using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Helpers.Calculators
{
    public class ScrollCalculator : IScrollCalculator
    {
        private readonly ICamera _camera;
        private readonly ITime _time;
        private readonly IRange<float> _validOrthographicSizes;
        private readonly ISettingsManager _settingsManager;
        private readonly ILevelToMultiplierConverter _scrollLevelConverter;
        private readonly float _scrollMultiplier;

        public ScrollCalculator(
            ICamera camera,
            ITime time,
            IRange<float> validOrthographicSizes,
            ISettingsManager settingsManager,
            ILevelToMultiplierConverter scrollLevelConverter,
            float scrollMultiplier)
        {
            Helper.AssertIsNotNull(camera, time, validOrthographicSizes, settingsManager, scrollLevelConverter);
            Assert.IsTrue(scrollMultiplier > 0);

            _camera = camera;
            _time = time;
            _validOrthographicSizes = validOrthographicSizes;
            _settingsManager = settingsManager;
            _scrollLevelConverter = scrollLevelConverter;
            _scrollMultiplier = scrollMultiplier;
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
                _scrollMultiplier *
                _time.UnscaledDeltaTime *
                _scrollLevelConverter.LevelToMultiplier(_settingsManager.ScrollSpeedLevel);
        }
    }
}