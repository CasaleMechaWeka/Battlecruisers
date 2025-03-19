using BattleCruisers.Utils.DataStrctures;

namespace BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters
{
    public class LinearRangeFinder
    {
        /// <returns>
        /// The possible range of fire angles.  Includes the given on target range,
        /// with an error margin either side.
        /// </returns>
        public IRange<float> FindFireAngleRange(IRange<float> onTargetRange, float accuracy)
        {
            float onTargetRangeSize = onTargetRange.Max - onTargetRange.Min;
            float errorMarginEachSide = (onTargetRangeSize / accuracy - onTargetRangeSize) * 0.5f;

            return new Range<float>(onTargetRange.Min - errorMarginEachSide, onTargetRange.Max + errorMarginEachSide);
        }
    }
}
