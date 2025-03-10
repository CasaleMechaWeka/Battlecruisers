using BattleCruisers.Buildables;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.Targets.Helpers
{
    public class BasicCalculator : IRangeCalculator
    {
        public bool IsInRange(ITransform parentTransform, ITarget target, float rangeInM)
        {
            return Vector2.Distance(target.Transform.Position, parentTransform.Position) <= rangeInM;
        }
    }
}