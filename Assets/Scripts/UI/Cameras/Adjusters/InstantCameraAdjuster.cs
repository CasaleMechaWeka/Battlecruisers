using BattleCruisers.UI.Cameras.Targets.Providers;
using BattleCruisers.Utils;
using BattleCruisers.Utils.PlatformAbstractions;

namespace BattleCruisers.UI.Cameras.Adjusters
{
    /// <summary>
    /// Instantly updates camera to target position.  Results in jerky
    /// camera movement.
    /// </summary>
    public class InstantCameraAdjuster : ICameraAdjuster
    {
        private readonly ICameraTargetProvider _cameraTargetProvider;
        private readonly ICamera _camera;

        public InstantCameraAdjuster(ICameraTargetProvider cameraTargetProvider, ICamera camera)
        {
            Helper.AssertIsNotNull(cameraTargetProvider, camera);

            _cameraTargetProvider = cameraTargetProvider;
            _camera = camera;
        }

        public bool AdjustCamera()
        {
            _camera.Transform.Position = _cameraTargetProvider.Target.Position;
            _camera.OrthographicSize = _cameraTargetProvider.Target.OrthographicSize;
            return true;
        }
    }
}