using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityCommon.PlatformAbstractions.Time;

namespace BattleCruisers.UI.Cameras.Helpers.Calculators
{
    public class EdgeScrollCalculator : IEdgeScrollCalculator
    {
        private readonly ITime _time;
        private readonly ISettingsManager _settingsManager;
        private readonly ILevelToMultiplierConverter _scrollLevelConverter;
        private readonly ICamera _camera;
        private readonly IRange<float> _validOrthographicSizes;

        public const float SCROLL_SCALE = 2048;

        public EdgeScrollCalculator(
            ITime time,
            ISettingsManager settingsManager,
            ILevelToMultiplierConverter scrollLevelConverter,
            ICamera camera,
            IRange<float> validOrthographicSizes)
        {
            Helper.AssertIsNotNull(time, settingsManager, scrollLevelConverter, camera, validOrthographicSizes);

            _time = time;
            _settingsManager = settingsManager;
            _scrollLevelConverter = scrollLevelConverter;
            _camera = camera;
            _validOrthographicSizes = validOrthographicSizes;
        }

        public float FindCameraPositionDeltaMagnituteInM()
        {
            // The more zoomed out the camera is, the greater our delta should be
            float orthographicProportion = _camera.OrthographicSize / _validOrthographicSizes.Max;

            return
                SCROLL_SCALE *
                _time.UnscaledDeltaTime *
                orthographicProportion *
                _scrollLevelConverter.LevelToMultiplier(_settingsManager.ScrollSpeedLevel);
        }
    }
}