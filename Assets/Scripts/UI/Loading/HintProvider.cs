using BattleCruisers.Utils;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Loading
{
    public class HintProvider : IHintProvider
    {
        private readonly IList<string> _hints;

        public HintProvider(IList<string> hints)
        {
            Helper.AssertIsNotNull(hints);
            Assert.IsTrue(hints.Count > 0);

            _hints = hints;
        }

        public string GetHint()
        {
            int randomIndex = RandomGenerator.Range(0, _hints.Count - 1);
            return _hints[randomIndex];
        }
    }
}