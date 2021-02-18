using BattleCruisers.Buildables;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.Targets.Helpers
{
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