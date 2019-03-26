using BattleCruisers.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace BattleCruisers.UI.Loading
{
    public class HintProvider : IHintProvider
    {
        private readonly IList<string> _hints;

        public HintProvider(IList<string> hints)
        {
            Assert.IsNotNull(hints);
            Assert.IsTrue(hints.Count > 0);

            _hints = hints;
        }

        public string GetHint()
        {
            return _hints.Shuffle().First();
        }
    }
}