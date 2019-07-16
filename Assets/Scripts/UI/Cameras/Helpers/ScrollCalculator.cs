using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityCommon.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Helpers
{
    // FELIX  Test :)
    public class ScrollCalculator : IScrollCalculator
    {
        private readonly ICamera _camera;
        private readonly IDeltaTimeProvider _deltaTimeProvider;
        private readonly IRange<float> _validOrthographicSizes;
        private readonly ISettingsManager _settingsManager;
        private readonly IZoomConverter _zoomConverter;
        // FELIX  Respect setting for swipe speed:)

        public const float SCROLL_SCALE = 16;
        public const float ZOOM_SCALE = 240;

        public ScrollCalculator(
            ICamera camera,
            IDeltaTimeProvider deltaTimeProvider,
            IRange<float> validOrthographicSizes,
            ISettingsManager settingsManager,
            IZoomConverter zoomConverter
            )
        {
            Helper.AssertIsNotNull(camera, deltaTimeProvider, validOrthographicSizes, settingsManager, zoomConverter);

            _camera = camera;
            _deltaTimeProvider = deltaTimeProvider;
            _validOrthographicSizes = validOrthographicSizes;
            _settingsManager = settingsManager;
            _zoomConverter = zoomConverter;
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
                _deltaTimeProvider.UnscaledDeltaTime;
                //_zoomConverter.LevelToSpeed(_settingsManager.ScrollSpeedLevel);
        }

        // FELIX  Avoid duplication with ZoomCalculator :/  Inject ZOOM_MULTIPLIER?
        public float FindZoomDelta(float swipeDeltaY)
        {
            // The more zoomed out the camera is, the greater our delta should be
            float orthographicProportion = _camera.OrthographicSize / _validOrthographicSizes.Max;
            // Direction should be inverted, so swiping left should move the screen right
            //float directionMultiplier = -1;

            return
                Mathf.Abs(swipeDeltaY) *
                //directionMultiplier *
                orthographicProportion *
                ZOOM_SCALE *
                _deltaTimeProvider.UnscaledDeltaTime *
                _zoomConverter.LevelToSpeed(_settingsManager.ZoomSpeedLevel);
        }
    }
}