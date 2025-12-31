using System.Collections.Generic;
using System.Linq;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Static.Strategies.Requests;
using BattleCruisers.Utils;
using UnityEngine.Assertions;

namespace BattleCruisers.Data.Static.Strategies
{
    public class Strategy
    {
        public IList<BuildingKey> BaseStrategy;
        public IEnumerable<OffensiveRequest> Offensives { get; }

        public Strategy(IList<BuildingKey> prefabKeys, OffensiveRequest[] offensives)
        {
            Assert.IsNotNull(prefabKeys);
            Assert.IsTrue(offensives.Length != 0);

            // we do this so we can later modify the stragegy, e.g. based on cruiser type and not get
            // exceptions if a readonly collection was passed as argument
            BaseStrategy = new List<BuildingKey>(prefabKeys);
            Offensives = offensives;
        }

        public Strategy(Strategy strategyToCopy)
        {
            BaseStrategy = strategyToCopy.BaseStrategy;

            // Offensive requests can be modified.  Hence, make a copy so
            // that modifying the offensive requests of one strategy does
            // not affect other strategies.
            Offensives
                = strategyToCopy.Offensives
                    .Select(originalOffensiveRequest => new OffensiveRequest(originalOffensiveRequest))
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
