using BattleCruisers.Utils.PlatformAbstractions;

namespace BattleCruisers.UI.Cameras.Targets
{
    public interface ICameraTargetEqualityCalculator
    {
        bool IsOnTarget(ICameraTarget target, ICamera camera, float orthographicSizeEqualityMargin, float positionEqualityMarginInM);
    }
}