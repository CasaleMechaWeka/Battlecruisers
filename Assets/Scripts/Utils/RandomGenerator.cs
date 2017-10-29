using UnityEngine;

namespace BattleCruisers.Utils
{
    public class RandomGenerator : IRandomGenerator
    {
        public float Range(float minInclusive, float maxInclusive)
        {
            return Random.Range(minInclusive, maxInclusive);
        }
    }
}
