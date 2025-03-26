using BattleCruisers.UI.Loading;
using BattleCruisers.Utils;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;

namespace BattleCruisers.Tests.UI.Loading
{
    public class HintProviderTests
    {
        [Test]
        public void GetHint()
        {
            IList<string> hints = new List<string>() { "a", "sweet", "hint" };
            RandomGenerator.Range(0, hints.Count - 1).Returns(2);

            IHintProvider hintProvider = new HintProvider(hints);
            Assert.AreEqual(hints[2], hintProvider.GetHint());
        }
    }
}