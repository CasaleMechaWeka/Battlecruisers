using BattleCruisers.Buildables;
using BattleCruisers.Utils.PlatformAbstractions;

namespace BattleCruisers.Targets.Helpers
{
    public class BasicCalculator : IRangeCalculator
    {
        public bool IsInRange(ITransform parentTransform, ITarget target, float rangeInM)
        {
            return (target.Transform.Position - parentTransform.Position).sqrMagnitude <= rangeInM * rangeInM;
        }
    }
}