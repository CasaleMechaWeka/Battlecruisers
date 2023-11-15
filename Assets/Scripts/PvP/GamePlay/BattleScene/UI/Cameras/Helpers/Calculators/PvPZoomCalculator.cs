using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.DataStrctures;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Helpers.Calculators
{
    public class PvPZoomCalculator : IPvPZoomCalculator
    {
        private readonly IPvPCamera _camera;
        private readonly IPvPTime _time;
        private readonly IPvPRange<float> _validOrthographicSizes;
        private readonly float _zoomScale, _zoomSettingsMultiplier;

        public PvPZoomCalculator(
            IPvPCamera camera,
            IPvPTime time,
            IPvPRange<float> validOrthographicSizes,
            float zoomScale,
            float zoomSettingsMultiplier)
        {
            PvPHelper.AssertIsNotNull(camera, time, validOrthographicSizes);
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