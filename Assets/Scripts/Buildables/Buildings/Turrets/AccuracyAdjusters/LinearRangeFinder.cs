using BattleCruisers.Utils.DataStrctures;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters
{
    public class LinearRangeFinder : IAngleRangeFinder
    {
        /// <returns>
        /// The possible range of fire angles.  Includes the given on target range,
        /// with an error margin either side.
        /// </returns>
        public IRange<float> FindFireAngleRange(IRange<float> onTargetRange, float accuracy)
        {
			var t2 = onTargetRange.Min;
            var t1 = onTargetRange.Max;

            //Assert.IsTrue(onTargetRange.Max > onTargetRange.Min);

            float onTargetRangeSize = onTargetRange.Max - onTargetRange.Min;

            float fireAngleRangeSize = onTargetRangeSize / accuracy;
            //Assert.IsTrue(fireAngleRangeSize >= onTargetRangeSize);

            float totalErrorMargin = fireAngleRangeSize - onTargetRangeSize;
            float errorMarginEachSide = totalErrorMargin / 2;

            float minFireAngle = onTargetRange.Min - errorMarginEachSide;
            float maxFireAngle = onTargetRange.Max + errorMarginEachSide;

            return new Range<float>(minFireAngle, maxFireAngle);
        }
    }
}
