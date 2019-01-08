using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Data.Static.Strategies.Requests;
using BattleCruisers.Utils;
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

        public Strategy(IStrategy strategyToCopy)
        {
            BaseStrategy = strategyToCopy.BaseStrategy;
            
            // Offensive requests can be modified.  Hence, make a copy so
            // that modifying the offensive requests of one strategy does
            // not affect other strategies.
            Offensives
                = strategyToCopy.Offensives
                    .Select(originalOffensiveRequest => (IOffensiveRequest)new OffensiveRequest(originalOffensiveRequest))
                    .ToList();
        }

        public override bool Equals(object obj)
        {
            Strategy other = obj as Strategy;
            return
                other != null
                && other.BaseStrategy.SmartEquals(BaseStrategy)
                && Enumerable.SequenceEqual(other.Offensives, Offensives);
        }

        public override int GetHashCode()
        {
            return this.GetHashCode(BaseStrategy, Offensives);
        }
    }
}
