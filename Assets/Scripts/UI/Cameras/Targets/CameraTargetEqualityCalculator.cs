using BattleCruisers.Utils.PlatformAbstractions;

namespace BattleCruisers.UI.Cameras.Targets
{
    // FELIX  use, test
    public class CameraTargetEqualityCalculator : ICameraTargetEqualityCalculator
    {
        public bool IsOnTarget(
            ICameraTarget target,
            ICamera camera,
            float orthographicSizeEqualityMargin,
            float positionEqualityMarginInM)
        {
            return
                camera.OrthographicSize - target.OrthographicSize < orthographicSizeEqualityMargin
                && (camera.Position - target.Position).magnitude < positionEqualityMarginInM;
        }
    }
}