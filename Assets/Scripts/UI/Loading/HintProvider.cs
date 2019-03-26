using BattleCruisers.Utils;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Loading
{
    public class HintProvider : IHintProvider
    {
        private readonly IList<string> _hints;
        private readonly IRandomGenerator _random;

        public HintProvider(IList<string> hints, IRandomGenerator random)
        {
            Helper.AssertIsNotNull(hints, random);
            Assert.IsTrue(hints.Count > 0);

            _hints = hints;
            _random = random;
        }

        public string GetHint()
        {
            int randomIndex = _random.Range(0, _hints.Count - 1);
            return _hints[randomIndex];
        }
    }
}