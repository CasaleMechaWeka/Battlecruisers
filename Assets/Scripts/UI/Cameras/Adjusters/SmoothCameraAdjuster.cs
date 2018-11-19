using System;
using BattleCruisers.UI.Cameras.Targets.Providers;
using BattleCruisers.Utils;

namespace BattleCruisers.UI.Cameras.Adjusters
{
    /// <summary>
    /// Smoothly moves camera to target postion and orthographic size.
    /// </summary>
    /// FELIX  Update tests :)
    public class SmoothCameraAdjuster : ICameraAdjuster
    {
        private readonly ICameraTargetProvider _cameraTargetProvider;
        private readonly ISmoothZoomAdjuster _zoomAdjuster;
        private readonly ISmoothPositionAdjuster _positionAdjuster;

        public event EventHandler CompletedAdjustment;

        public SmoothCameraAdjuster(
            ICameraTargetProvider cameraTargetProvider, 
            ISmoothZoomAdjuster zoomAdjuster,
            ISmoothPositionAdjuster positionAdjuster)
        {
            Helper.AssertIsNotNull(cameraTargetProvider, zoomAdjuster, positionAdjuster);

            _cameraTargetProvider = cameraTargetProvider;
            _zoomAdjuster = zoomAdjuster;
            _positionAdjuster = positionAdjuster;
        }

        public void AdjustCamera()
        {
            bool reachedTargetZoom = _zoomAdjuster.AdjustZoom(_cameraTargetProvider.Target.OrthographicSize);
            bool reachedTargetPosition = _positionAdjuster.AdjustPosition(_cameraTargetProvider.Target.Position);

            bool reachedTarget = reachedTargetZoom && reachedTargetPosition;
            if (reachedTarget && CompletedAdjustment != null)
            {
                CompletedAdjustment.Invoke(this, EventArgs.Empty);
            }
        }
    }
}