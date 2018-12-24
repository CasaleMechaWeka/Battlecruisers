using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Targets.Finders
{
    public class CornerIdentifier : ICornerIdentifier
    {
        private readonly ICornerCutoffProvider _cornerCutoffProvider;

        public CornerIdentifier(ICornerCutoffProvider cornerCutoffProvider)
        {
            Assert.IsNotNull(cornerCutoffProvider);
            _cornerCutoffProvider = cornerCutoffProvider;
        }

        public CameraCorner? FindCorner(ICameraTarget cameraTarget)
        {
            Assert.IsNotNull(cameraTarget);

            if (cameraTarget.Position.x <= _cornerCutoffProvider.PlayerCruiserCornerXPositionCutoff)
            {
                return CameraCorner.PlayerCruiser;
            }
            else if (cameraTarget.Position.x >= _cornerCutoffProvider.AICruiserCornerXPositionCutoff)
            {
                return CameraCorner.AICruiser;
            }
            else if (cameraTarget.OrthographicSize >= _cornerCutoffProvider.OverviewOrthographicSizeCutoff)
            {
                return CameraCorner.Overview;
            }
            else
            {
                return null;
            }
        }
    }
}