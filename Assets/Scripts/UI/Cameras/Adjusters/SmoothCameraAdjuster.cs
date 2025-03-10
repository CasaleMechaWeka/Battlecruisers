using BattleCruisers.UI.Cameras.Targets.Providers;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.Cameras.Adjusters
{
    /// <summary>
    /// Smoothly moves camera to target postion and orthographic size.
    /// </summary>
    public class SmoothCameraAdjuster : CameraAdjuster
    {
        private readonly ISmoothZoomAdjuster _zoomAdjuster;
        private readonly ISmoothPositionAdjuster _positionAdjuster;

        public SmoothCameraAdjuster(
            ICameraTargetProvider cameraTargetProvider, 
            ISmoothZoomAdjuster zoomAdjuster,
            ISmoothPositionAdjuster positionAdjuster)
            : base(cameraTargetProvider)
        {
            Helper.AssertIsNotNull(zoomAdjuster, positionAdjuster);

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