using BattleCruisers.Buildables;
using UnityCommon.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.Targets.Helpers
{
    // FELIX   Test :)
    public class BasicCalculator : IRangeCalculator
    {
        public bool IsInRange(ITransform parentTransform, ITarget target, float rangeInM)
        {
            return Vector2.Distance(target.Transform.Position, parentTransform.Position) <= rangeInM;
        }
    }
}