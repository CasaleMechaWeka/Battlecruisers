using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Helpers.Calculators
{
    public class ZoomCalculator : IZoomCalculator
    {
        private readonly ICamera _camera;
        private readonly ITime _time;
        private readonly IRange<float> _validOrthographicSizes;
        private readonly float _zoomScale, _zoomSettingsMultiplier;

        public ZoomCalculator(
            ICamera camera, 
            ITime time, 
            IRange<float> validOrthographicSizes,
            float zoomScale,
            float zoomSettingsMultiplier)
        {
            Helper.AssertIsNotNull(camera, time, validOrthographicSizes);
            Assert.IsTrue(zoomScale > 0);
            Assert.IsTrue(zoomSettingsMultiplier > 0);

            _camera = camera;
            _time = time;
            _validOrthographicSizes = validOrthographicSizes;
            _zoomScale = zoomScale;
            _zoomSettingsMultiplier = zoomSettingsMultiplier;
        }

        public float FindMouseScrollOrthographicSizeDelta(float mouseScrollDeltaY)
        {
            return FindSizeDelta(mouseScrollDeltaY);
        }

        public float FindPinchZoomOrthographicSizeDelta(float pinchZoomDelta)
        {
            return FindSizeDelta(pinchZoomDelta);
        }

        private float FindSizeDelta(float inputDelta)
        {
            // The more zoomed out the camera is, the greater our delta should be
            float orthographicProportion = _camera.OrthographicSize / _validOrthographicSizes.Max;

            return
                Mathf.Abs(inputDelta) *
                orthographicProportion *
                _zoomScale *
                _time.UnscaledDeltaTime *
                _zoomSettingsMultiplier;
        }
    }
}