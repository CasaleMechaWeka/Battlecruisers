using BattleCruisers.Buildables;
using UnityCommon.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.Targets.Helpers
{
    // FELIX   Test :)
    public class SizeInclusiveCalculator : IRangeCalculator
    {
        public bool IsInRange(ITransform parentTransform, ITarget target, float rangeInM)
        {
            float distanceCenterToCenter = Vector2.Distance(target.Position, parentTransform.Position);
            float distanceCenterToEdge = distanceCenterToCenter - target.Size.x / 2;
            return distanceCenterToEdge <= rangeInM;
        }
    }
}