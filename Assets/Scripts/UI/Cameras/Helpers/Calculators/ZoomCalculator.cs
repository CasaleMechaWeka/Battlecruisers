using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityCommon.PlatformAbstractions;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Helpers.Calculators
{
    public class ZoomCalculator : IZoomCalculator
    {
        private readonly ICamera _camera;
        private readonly ITime _time;
        private readonly IRange<float> _validOrthographicSizes;
        private readonly ISettingsManager _settingsManager;
        private readonly ILevelToMultiplierConverter _zoomConverter;
        private readonly float _zoomScale;

        public ZoomCalculator(
            ICamera camera, 
            ITime time, 
            IRange<float> validOrthographicSizes,
            ISettingsManager settingsManager,
            ILevelToMultiplierConverter zoomConverter,
            float zoomScale)
        {
            Helper.AssertIsNotNull(camera, time, validOrthographicSizes, settingsManager, zoomConverter);
            Assert.IsTrue(zoomScale > 0);

            _camera = camera;
            _time = time;
            _validOrthographicSizes = validOrthographicSizes;
            _settingsManager = settingsManager;
            _zoomConverter = zoomConverter;
            _zoomScale = zoomScale;
        }

        public float FindOrthographicSizeDelta(float mouseScrollDeltaY)
        {
            // The more zoomed out the camera is, the greater our delta should be
            float orthographicProportion = _camera.OrthographicSize / _validOrthographicSizes.Max;

            return
                Mathf.Abs(mouseScrollDeltaY) *
                orthographicProportion *
                _zoomScale *
                _time.UnscaledDeltaTime *
                _zoomConverter.LevelToMultiplier(_settingsManager.ZoomSpeedLevel);
        }
    }
}