using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using UnityCommon.PlatformAbstractions.Time;

namespace BattleCruisers.UI.Cameras.Helpers.Calculators
{
    // FELIX  use, test
    public class EdgeScrollCalculator : IEdgeScrollCalculator
    {
        private readonly ITime _time;
        private readonly ISettingsManager _settingsManager;
        private readonly ILevelToMultiplierConverter _scrollLevelConverter;

        public const float SCROLL_SCALE = 128;

        public EdgeScrollCalculator(
            ITime time,
            ISettingsManager settingsManager,
            ILevelToMultiplierConverter scrollLevelConverter)
        {
            Helper.AssertIsNotNull(time, settingsManager, scrollLevelConverter);

            _time = time;
            _settingsManager = settingsManager;
            _scrollLevelConverter = scrollLevelConverter;
        }

        public float FindCameraPositionDeltaMagnituteInM()
        {
            return
                SCROLL_SCALE *
                _time.UnscaledDeltaTime *
                _scrollLevelConverter.LevelToMultiplier(_settingsManager.ScrollSpeedLevel);
        }
    }
}