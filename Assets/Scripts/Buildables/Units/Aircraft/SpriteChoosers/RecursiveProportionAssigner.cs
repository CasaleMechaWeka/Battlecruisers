using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers
{
    /// <summary>
    /// Given:
    /// numOfOptions = 3
    /// baseCutoff = 0.5
    /// 
    /// Creates cutoffs:
    /// 0.5
    /// 0.25
    /// 0.125
    /// 
    /// Assign(int proportion):
    /// proportion  index
    /// > 0.5       0
    /// > 0.25      1
    /// rest        2
    /// </summary>
    /// FELIX  Remove :)
    public class RecursiveProportionAssigner : IAssigner
    {
        private readonly IList<float> _cutoffs;

		private const int MIN_NUM_OF_OPTIONS = 1;
        private const int MIN_BASE_CUTOFF = 0;
        private const int MAX_BASE_CUTOFF = 1;
        private const int MIN_PROPORTION = 0;
        private const int MAX_PROPORTION = 1;
		
        public const float DEFAULT_BASE_CUTOFF = 0.67f;

        public RecursiveProportionAssigner(int numOfOptions, float baseCutoff = DEFAULT_BASE_CUTOFF)
        {
            Assert.IsTrue(numOfOptions >= MIN_NUM_OF_OPTIONS);
            Assert.IsTrue(baseCutoff > MIN_BASE_CUTOFF);
            Assert.IsTrue(baseCutoff < MAX_BASE_CUTOFF);

            _cutoffs = new List<float>(numOfOptions);

            float cumulativeCutoff = baseCutoff;

            for (int i = 0; i < numOfOptions; ++i)
            {
                _cutoffs.Add(cumulativeCutoff);
                cumulativeCutoff *= cumulativeCutoff;
            }
        }

        public int Assign(float proportion)
        {
            Assert.IsTrue(proportion >= MIN_PROPORTION);
            Assert.IsTrue(proportion <= MAX_PROPORTION);

            for (int i = 0; i < _cutoffs.Count; ++i)
            {
                float cutoff = _cutoffs[i];

                if (proportion > cutoff)
                {
                    return i;
                }
            }

            return _cutoffs.Count - 1;
        }
    }
}
