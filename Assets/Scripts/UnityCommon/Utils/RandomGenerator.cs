using System.Collections.Generic;
using System.Linq;
using UnityCommon.DataStrctures;
using UnityEngine;
using UnityEngine.Assertions;

namespace UnityCommon.Utils
{
    public class RandomGenerator : IRandomGenerator
    {
        public float RangeFromCenter(float center, float radius)
        {
            Assert.IsTrue(radius > 0);
            return Range(center - radius, center + radius);
        }

        public float Range(IRange<float> range)
        {
            Assert.IsNotNull(range);
            return Range(range.Min, range.Max);
        }

        public float Range(float minInclusive, float maxInclusive)
        {
            Assert.IsTrue(minInclusive < maxInclusive);
            return Random.Range(minInclusive, maxInclusive);
        }

        public int Range(IRange<int> range)
        {
            Assert.IsNotNull(range);
            return Range(range.Min, range.Max);
        }

        public int Range(int minInclusive, int maxInclusive)
        {
            Assert.IsTrue(minInclusive < maxInclusive);

            float randomFloat = Random.Range(minInclusive - 0.5f, maxInclusive + 0.5f);
            return Mathf.RoundToInt(randomFloat);
        }

        public float Randomise(float baseValue, float maxChangeByProportionOfBaseValue, ChangeDirection changeDirection)
        {
            Assert.IsTrue(maxChangeByProportionOfBaseValue >= 0);
            float maxChange = Mathf.Abs(baseValue) * maxChangeByProportionOfBaseValue;
            float floor = changeDirection == ChangeDirection.Up ? baseValue : baseValue - maxChange;
            float ceiling = changeDirection == ChangeDirection.Down ? baseValue : baseValue + maxChange;

            return Range(floor, ceiling);
        }

        public bool NextBool()
        {
            return Random.value > 0.5f;
        }

        public TItem RandomItem<TItem>(IEnumerable<TItem> items, int numOfItems)
        {
            int index = Range(0, numOfItems - 1);
            return items.ElementAt(index);
        }
    }
}
