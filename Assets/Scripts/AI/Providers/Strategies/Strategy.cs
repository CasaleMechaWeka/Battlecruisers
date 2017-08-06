using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BattleCruisers.AI.Providers.Strategies
{
    public class Strategy : IStrategy
    {
        public IBaseStrategy BaseStrategy { get; private set; }
        public IEnumerable<IBasicOffensiveRequest> Offensives { get; private set; }

        public Strategy(IBaseStrategy baseStrategy, params IBasicOffensiveRequest[] offensives)
        {
            Assert.IsNotNull(baseStrategy);
            Assert.IsTrue(offensives.Length != 0);

            BaseStrategy = baseStrategy;
            Offensives = offensives;
        }
    }
}
