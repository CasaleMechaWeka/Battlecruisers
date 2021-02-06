using BattleCruisers.Utils.DataStrctures;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Sound.Wind
{
    // FELIX  Interface, test (steal VolumeCalculatorTests?)
    public class ProportionCalculator : IProportionCalculator
    {
        public float FindProprtion(float value, IRange<float> range)
        {
            Assert.IsNotNull(range);

            value = Mathf.Clamp(value, range.Min, range.Max);

            float sizeAboveMin = value - range.Min;
            float rangeValue = range.Max - range.Min;

            return sizeAboveMin / rangeValue;
        }
    }
}