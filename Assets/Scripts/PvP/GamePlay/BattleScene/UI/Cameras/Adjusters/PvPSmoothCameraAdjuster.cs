using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.UI.Cameras.Adjusters;
using BattleCruisers.UI.Cameras.Targets.Providers;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Adjusters
{
    /// <summary>
    /// Smoothly moves camera to target postion and orthographic size.
    /// </summary>
    public class PvPSmoothCameraAdjuster : PvPCameraAdjuster
    {
        private readonly ISmoothZoomAdjuster _zoomAdjuster;
        private readonly ISmoothPositionAdjuster _positionAdjuster;

        public PvPSmoothCameraAdjuster(
            ICameraTargetProvider cameraTargetProvider,
            ISmoothZoomAdjuster zoomAdjuster,
            ISmoothPositionAdjuster positionAdjuster)
            : base(cameraTargetProvider)
        {
            PvPHelper.AssertIsNotNull(zoomAdjuster, positionAdjuster);

            _zoomAdjuster = zoomAdjuster;
            _positionAdjuster = positionAdjuster;
        }

        public override void AdjustCamera()
        {
            bool reachedTargetZoom = _zoomAdjuster.AdjustZoom(_cameraTargetProvider.Target.OrthographicSize);
            bool reachedTargetPosition = _positionAdjuster.AdjustPosition(_cameraTargetProvider.Target.Position);

            bool reachedTarget = reachedTargetZoom && reachedTargetPosition;
            if (reachedTarget)
            {
                InvokeCompletedAdjustmentEvent();
            }
        }
    }
}