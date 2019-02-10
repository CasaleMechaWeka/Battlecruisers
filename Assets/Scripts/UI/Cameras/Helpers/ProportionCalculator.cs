using BattleCruisers.Utils.DataStrctures;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Cameras.Helpers
{
    // FELIX  Test :D
    public class ProportionCalculator : IProportionCalculator
    {
        public float FindProportion(float value, IRange<float> valueRange)
        {
            Assert.IsNotNull(valueRange);

            float clampedValue = Mathf.Clamp(value, valueRange.Min, valueRange.Max);
            float range = valueRange.Max - valueRange.Min;
            return (clampedValue - valueRange.Min) / range;
        }

        public float FindProportionalValue(float proportion, IRange<float> valueRange)
        {
            Assert.IsNotNull(valueRange);
            Assert.IsTrue(proportion >= 0);
            Assert.IsTrue(proportion <= 1);

            float valueDifference = valueRange.Max - valueRange.Min;
            float valueOffset = proportion * valueDifference;
            return valueRange.Min + valueOffset;
        }
    }
}