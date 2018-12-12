using BattleCruisers.UI.Cameras.Targets.Providers;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Adjusters
{
    /// <summary>
    /// Instantly updates camera to target position.  Results in jerky
    /// camera movement.
    /// </summary>
    public class InstantCameraAdjuster : CameraAdjuster
    {
        private readonly ICamera _camera;

        public InstantCameraAdjuster(ICameraTargetProvider cameraTargetProvider, ICamera camera)
            : base(cameraTargetProvider)
        {
            Assert.IsNotNull(camera);
            _camera = camera;
        }

        public override void AdjustCamera()
        {
            _camera.Transform.Position = _cameraTargetProvider.Target.Position;
            _camera.OrthographicSize = _cameraTargetProvider.Target.OrthographicSize;
            InvokeCompletedAdjustmentEvent();
        }
    }
}