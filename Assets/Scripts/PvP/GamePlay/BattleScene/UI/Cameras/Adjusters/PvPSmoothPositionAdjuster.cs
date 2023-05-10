using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Adjusters
{
    public class PvPSmoothPositionAdjuster : IPvPSmoothPositionAdjuster
    {
        private readonly IPvPCamera _camera;
        private readonly IPvPTime _time;
        private readonly IPvPCameraSmoothTimeProvider _smoothTimeProvider;
        private Vector3 _cameraPositionChangeVelocity;

        private const float POSITION_EQUALITY_MARGIN = 0.1f;
        private const float MIN_SMOOTH_TIME = 0;
        private const float MAX_SPEED = 1000;

        public PvPSmoothPositionAdjuster(IPvPCamera camera, IPvPTime time, IPvPCameraSmoothTimeProvider smoothTimeProvider)
        {
            PvPHelper.AssertIsNotNull(camera, time, smoothTimeProvider);

            _camera = camera;
            _time = time;
            _smoothTimeProvider = smoothTimeProvider;
            _cameraPositionChangeVelocity = Vector3.zero;
        }

        public bool AdjustPosition(Vector3 targetPosition)
        {
            bool isInPosition = (_camera.Position - targetPosition).magnitude < POSITION_EQUALITY_MARGIN;

            if (!isInPosition)
            {
                _camera.Position
                    = Vector3.SmoothDamp(
                        _camera.Position,
                        targetPosition,
                        ref _cameraPositionChangeVelocity,
                        _smoothTimeProvider.SmoothTime,
                        MAX_SPEED,
                        _time.UnscaledDeltaTime);
            }
            else
            {
                _camera.Position = targetPosition;
            }

            // Logging.Verbose(Tags.CAMERA, $"target: {targetPosition}  Actual: {_camera.Position}");

            return isInPosition;
        }
    }
}
