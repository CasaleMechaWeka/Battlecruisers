using BattleCruisers.Utils;
using BattleCruisers.Utils.DataStrctures;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.UI.Cameras.Targets.Providers
{
    // FELIX  Test :)
    public class ZoomCalculator : IZoomCalculator
    {
        private readonly ICamera _camera;
        private readonly IDeltaTimeProvider _deltaTimeProvider;
        private readonly IRange<float> _validOrthographicSizes;

        private const float ZOOM_SCALE = 2400;

        public ZoomCalculator(ICamera camera, IDeltaTimeProvider deltaTimeProvider, IRange<float> validOrthographicSizes)
        {
            Helper.AssertIsNotNull(camera, deltaTimeProvider, validOrthographicSizes);

            _camera = camera;
            _deltaTimeProvider = deltaTimeProvider;
            _validOrthographicSizes = validOrthographicSizes;
        }

        public float FindZoomDelta(float mouseScrollDeltaY)
        {
            float orthographicProportion = _camera.OrthographicSize / _validOrthographicSizes.Max;
            return Mathf.Abs(mouseScrollDeltaY) * orthographicProportion * ZOOM_SCALE * _deltaTimeProvider.UnscaledDeltaTime;
        }
    }
}