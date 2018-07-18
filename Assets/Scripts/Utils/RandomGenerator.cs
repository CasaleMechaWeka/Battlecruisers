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
    }
}
