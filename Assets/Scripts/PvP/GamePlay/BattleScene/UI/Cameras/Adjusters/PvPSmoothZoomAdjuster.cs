using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Adjusters
{
    public class PvPSmoothZoomAdjuster : IPvPSmoothZoomAdjuster
    {
        private readonly IPvPCamera _camera;
        private readonly IPvPTime _time;
        private readonly IPvPCameraSmoothTimeProvider _smoothTimeProvider;
        private float _cameraOrthographicSizeChangeVelocity;

        private const float ORTHOGRAPHIC_SIZE_EQUALITY_MARGIN = 0.1f;
        private const float MAX_SPEED = 1000;

        public PvPSmoothZoomAdjuster(IPvPCamera camera, IPvPTime time, IPvPCameraSmoothTimeProvider smoothTimeProvider)
        {
            PvPHelper.AssertIsNotNull(camera, time, smoothTimeProvider);

            _camera = camera;
            _time = time;
            _smoothTimeProvider = smoothTimeProvider;
            _cameraOrthographicSizeChangeVelocity = 0;
        }

        public bool AdjustZoom(float targetOrthographicSize)
        {
            bool isRightOrthographicSize = Mathf.Abs(_camera.OrthographicSize - targetOrthographicSize) < ORTHOGRAPHIC_SIZE_EQUALITY_MARGIN;

            if (!isRightOrthographicSize)
            {
                _camera.OrthographicSize
                    = Mathf.SmoothDamp(
                        _camera.OrthographicSize,
                        targetOrthographicSize,
                        ref _cameraOrthographicSizeChangeVelocity,
                        _smoothTimeProvider.SmoothTime,
                        MAX_SPEED,
                        _time.UnscaledDeltaTime);
            }
            else
            {
                _camera.OrthographicSize = targetOrthographicSize;
            }

            return isRightOrthographicSize;
        }
    }
}
