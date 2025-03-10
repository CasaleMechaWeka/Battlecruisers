using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Helpers.Calculators
{
    public class EdgeScrollCalculator : IEdgeScrollCalculator
    {
        private readonly ITime _time;
        private readonly ISettingsManager _settingsManager;
        private readonly ILevelToMultiplierConverter _scrollLevelConverter;
        private readonly ICamera _camera;
        private readonly IRange<float> _validOrthographicSizes;
        private readonly float _scrollMultiplier;

        public EdgeScrollCalculator(
            ITime time,
            ISettingsManager settingsManager,
            ILevelToMultiplierConverter scrollLevelConverter,
            ICamera camera,
            IRange<float> validOrthographicSizes,
            float scrollMultiplier)
        {
            Helper.AssertIsNotNull(time, settingsManager, scrollLevelConverter, camera, validOrthographicSizes);
            Assert.IsTrue(scrollMultiplier > 0);

            _time = time;
            _settingsManager = settingsManager;
            _scrollLevelConverter = scrollLevelConverter;
            _camera = camera;
            _validOrthographicSizes = validOrthographicSizes;
            _scrollMultiplier = scrollMultiplier;
        }

        public float FindCameraPositionDeltaMagnituteInM()
        {
            // The more zoomed out the camera is, the greater our delta should be
            float orthographicProportion = _camera.OrthographicSize / _validOrthographicSizes.Max;

            return
                _scrollMultiplier *
                _time.UnscaledDeltaTime *
                orthographicProportion *
                _scrollLevelConverter.LevelToMultiplier(_settingsManager.ScrollSpeedLevel);
        }
    }
}