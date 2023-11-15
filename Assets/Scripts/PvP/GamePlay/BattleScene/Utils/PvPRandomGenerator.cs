using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.DataStrctures;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils
{
    public class PvPRandomGenerator : IPvPRandomGenerator
    {
        private static PvPRandomGenerator _instance;
        public static PvPRandomGenerator Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PvPRandomGenerator();
                }
                return _instance;
            }
        }

        public float Value => Random.value;

        private PvPRandomGenerator() { }

        public float RangeFromCenter(float center, float radius)
        {
            Assert.IsTrue(radius > 0);
            return Range(center - radius, center + radius);
        }

        public float Range(IPvPRange<float> range)
        {
            return Range(range.Min, range.Max);
        }

        public float Range(float minInclusive, float maxInclusive)
        {
            //Assert.IsTrue(minInclusive < maxInclusive, $"{minInclusive} should be < than {maxInclusive}");
            return Random.Range(minInclusive, maxInclusive);
        }

        public int Range(int minInclusive, int maxInclusive)
        {
            Assert.IsTrue(minInclusive <= maxInclusive);

            if (minInclusive == maxInclusive)
            {
                return minInclusive;
            }

            float randomFloat = Random.Range(minInclusive - 0.5f, maxInclusive + 0.5f);
            return Mathf.RoundToInt(randomFloat);
        }

        public float Randomise(float baseValue, float maxChangeByProportionOfBaseValue, PvPChangeDirection changeDirection)
        {
            Assert.IsTrue(maxChangeByProportionOfBaseValue >= 0);
            float maxChange = Mathf.Abs(baseValue) * maxChangeByProportionOfBaseValue;
            float floor = changeDirection == PvPChangeDirection.Up ? baseValue : baseValue - maxChange;
            float ceiling = changeDirection == PvPChangeDirection.Down ? baseValue : baseValue + maxChange;

            return Range(floor, ceiling);
        }

        public bool NextBool()
        {
            return Random.value > 0.5f;
        }

        public TItem RandomItem<TItem>(IList<TItem> items)
        {
            int index = Range(0, items.Count - 1);
            return items[index];
        }
    }
}
