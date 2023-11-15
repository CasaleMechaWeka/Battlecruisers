using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Targets.Providers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Cameras.Adjusters
{
    /// <summary>
    /// Smoothly moves camera to target postion and orthographic size.
    /// </summary>
    public class PvPSmoothCameraAdjuster : PvPCameraAdjuster
    {
        private readonly IPvPSmoothZoomAdjuster _zoomAdjuster;
        private readonly IPvPSmoothPositionAdjuster _positionAdjuster;

        public PvPSmoothCameraAdjuster(
            IPvPCameraTargetProvider cameraTargetProvider,
            IPvPSmoothZoomAdjuster zoomAdjuster,
            IPvPSmoothPositionAdjuster positionAdjuster)
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