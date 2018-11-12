using BattleCruisers.Utils;

namespace BattleCruisers.UI.Cameras.Targets.Finders
{
    /// <summary>
    /// Wraps a NavigationWheelCameraTargetFinder.  If the camera target is
    /// within one of the three corners, adjusts the target to be exactly in
    /// that corner.
    /// 
    /// The three corners are:
    /// + Player cruiser
    /// + Overview
    /// + AI cruiser
    /// </summary>
    /// FELIX  Test :D
    public class NavigationWheelCornersCameraTargetFinder : ICameraTargetFinder
    {
        private readonly ICameraTargetFinder _coreTargetFinder;
        private readonly ICornerIdentifier _cornerIdentifier;
        private readonly ICornerCameraTargetProvider _cornerCameraTargetProvider;

        public NavigationWheelCornersCameraTargetFinder(
            ICameraTargetFinder coreTargetFinder,
            ICornerIdentifier cornerIdentifier,
            ICornerCameraTargetProvider cornerCameraTargetProvider)
        {
            Helper.AssertIsNotNull(coreTargetFinder, cornerIdentifier, cornerCameraTargetProvider);

            _coreTargetFinder = coreTargetFinder;
            _cornerIdentifier = cornerIdentifier;
            _cornerCameraTargetProvider = cornerCameraTargetProvider;
        }

        public ICameraTarget FindCameraTarget()
        {
            ICameraTarget cameraTarget = _coreTargetFinder.FindCameraTarget();
            CameraCorner? cameraCorner = _cornerIdentifier.FindCorner(cameraTarget);

            if (cameraCorner != null)
            {
                cameraTarget = _cornerCameraTargetProvider.GetTarget((CameraCorner)cameraCorner);
            }

            return cameraTarget;
        }
    }
}