using System.Collections.Generic;
using BattleCruisers.Data.Static.Strategies.Requests;
using UnityEngine.Assertions;

namespace BattleCruisers.Data.Static.Strategies
{
    public class Strategy : IStrategy
    {
        public IBaseStrategy BaseStrategy { get; private set; }
        public IEnumerable<IOffensiveRequest> Offensives { get; private set; }

        public Strategy(IBaseStrategy baseStrategy, IOffensiveRequest[] offensives)
        {
            Assert.IsNotNull(baseStrategy);
            Assert.IsTrue(offensives.Length != 0);

            BaseStrategy = baseStrategy;
            Offensives = offensives;
        }
    }
}
