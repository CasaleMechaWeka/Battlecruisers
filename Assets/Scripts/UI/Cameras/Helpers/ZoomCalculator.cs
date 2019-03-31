using BattleCruisers.Data.Settings;
using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.UI.Cameras.Helpers
{
    public class ZoomCalculator : IZoomCalculator
    {
        private readonly ICamera _camera;
        private readonly IDeltaTimeProvider _deltaTimeProvider;
        private readonly IRange<float> _validOrthographicSizes;
        private readonly ISettingsManager _settingsManager;

        public const float ZOOM_SCALE = 2400;

        public ZoomCalculator(
            ICamera camera, 
            IDeltaTimeProvider deltaTimeProvider, 
            IRange<float> validOrthographicSizes,
            ISettingsManager settingsManager)
        {
            Helper.AssertIsNotNull(camera, deltaTimeProvider, validOrthographicSizes, settingsManager);

            _camera = camera;
            _deltaTimeProvider = deltaTimeProvider;
            _validOrthographicSizes = validOrthographicSizes;
            _settingsManager = settingsManager;
        }

        public float FindZoomDelta(float mouseScrollDeltaY)
        {
            float orthographicProportion = _camera.OrthographicSize / _validOrthographicSizes.Max;
            return
                Mathf.Abs(mouseScrollDeltaY) *
                orthographicProportion *
                ZOOM_SCALE *
                _deltaTimeProvider.UnscaledDeltaTime *
                _settingsManager.ZoomSpeedLevel;
        }
    }
}