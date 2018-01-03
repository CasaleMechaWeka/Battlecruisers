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
            return Random.Range(minInclusive, maxInclusive);
        }
    }
}
