using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Utils
{
    public class RandomGenerator : IRandomGenerator
    {
        public float RangeFromCenter(float center, float radius)
        {
            Assert.IsTrue(radius > 0);
            return Range(center - radius, center + radius);
        }
		
        public float Range(float minInclusive, float maxInclusive)
        {
            Assert.IsTrue(minInclusive < maxInclusive);
            return Random.Range(minInclusive, maxInclusive);
        }

        public int Range(int minInclusive, int maxInclusive)
        {
            Assert.IsTrue(minInclusive < maxInclusive);

            float randomFloat = Random.Range(minInclusive - 0.5f, maxInclusive + 0.5f);
            return Mathf.RoundToInt(randomFloat);
        }

        // FELIX  NEXT  Test.  Ie, do 100 with different ChangeDirection options :P
        // FELIX  Use in aircraft controller to randomise max velocity (downards :P)
        public float Randomise(float baseValue, float maxChangeByProportionOfBaseValue, ChangeDirection changeDirection)
        {
            Assert.IsTrue(maxChangeByProportionOfBaseValue >= 0);
            float maxChange = Mathf.Abs(baseValue) * maxChangeByProportionOfBaseValue;
            float floor = changeDirection == ChangeDirection.Up ? baseValue : baseValue - maxChange;
            float ceiling = changeDirection == ChangeDirection.Down ? baseValue : baseValue + maxChange;

            return Range(floor, ceiling);
        }
    }
}
