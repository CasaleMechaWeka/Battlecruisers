using BattleCruisers.Utils;

namespace BattleCruisers.UI.Cameras.Adjusters
{
    /// <summary>
    /// Smoothly moves camera to target postion and orthographic size.
    /// </summary>
    /// FELIX  Test :)
    public class SmoothCameraAdjuster : ICameraAdjuster
    {
        private readonly ICameraTargetProvider _cameraTargetProvider;
        private readonly ISmoothZoomAdjuster _zoomAdjuster;
        private readonly ISmoothPositionAdjuster _positionAdjuster;

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

        public void AdjustCamera(float deltaTime)
        {
            _zoomAdjuster.AdjustZoom(_cameraTargetProvider.Target.OrthographicSize);
            _positionAdjuster.AdjustPosition(_cameraTargetProvider.Target.Position);
        }
    }
}