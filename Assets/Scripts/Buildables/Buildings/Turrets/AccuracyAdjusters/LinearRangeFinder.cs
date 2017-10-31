using BattleCruisers.Utils.DataStrctures;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters
{
    public class LinearRangeFinder : IAngleRangeFinder
    {
        public IRange<float> FindFireAngleRange(IRange<float> onTargetRange, float accuracy)
        {
            Assert.IsTrue(onTargetRange.Max > onTargetRange.Min);

            float onTargetRangeSize = onTargetRange.Max - onTargetRange.Min;

            float fireAngleRangeSize = onTargetRangeSize / accuracy;
            Assert.IsTrue(fireAngleRangeSize > onTargetRangeSize);

            float totalErrorMargin = fireAngleRangeSize - onTargetRangeSize;
            float errorMarginEachSide = totalErrorMargin / 2;

            float minFireAngle = onTargetRange.Min - errorMarginEachSide;
            float maxFireAngle = onTargetRange.Max + errorMarginEachSide;

            return new Range<float>(minFireAngle, maxFireAngle);
        }
    }
}
