using BattleCruisers.Buildables;
using BattleCruisers.Utils.PlatformAbstractions;

namespace BattleCruisers.Targets.Helpers
{
    public interface IRangeCalculator
    {
        bool IsInRange(ITransform parentTransform, ITarget target, float rangeInM);
    }
}