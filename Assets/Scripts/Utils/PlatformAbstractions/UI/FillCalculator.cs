using UnityEngine.Assertions;

namespace BattleCruisers.Utils.PlatformAbstractions.UI
{
    /// <summary>
    /// Converts between raw fillable amounts and adjusted fillable amounts.
    /// For example, for a minimum and maximum proportions of 0.25 and 0.75 
    /// respectively, the following converstions would happen:
    /// Raw Adjusted
    /// 0   -> 0.25
    /// 0.5 -> 0.5
    /// 1   -> 0.75
    /// </summary>
    public class FillCalculator : IFillCalculator
    {
        private readonly float _range;

        // Giving a proportion of 0 will set this minimum proportion on the underlying image
        private readonly float _minProportion;

        // Giving a proportion of 1 will set this maximum proportion on the underlying image
        private readonly float _maxProportion;

        public FillCalculator(float minProportion, float maxProportion)
        {
            Assert.IsTrue(minProportion >= 0);
            Assert.IsTrue(maxProportion <= 1);
            Assert.IsTrue(minProportion < maxProportion);

            _minProportion = minProportion;
            _maxProportion = maxProportion;
            _range = _maxProportion - _minProportion;
        }

        public float RawToAdjusted(float rawFillAmount)
        {
            float additionToMin = rawFillAmount * _range;
            return _minProportion + additionToMin;
        }

        public float AdjustedToRaw(float adjustedFillAmount)
        {
            float additionToMin = adjustedFillAmount - _minProportion;
            return additionToMin / _range;
        }
    }
}