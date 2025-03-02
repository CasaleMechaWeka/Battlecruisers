using BattleCruisers.Data.Static.Strategies.Requests;
using BattleCruisers.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static.Strategies
{
    public class PvPStrategy : IPvPStrategy
    {
        public IPvPBaseStrategy BaseStrategy { get; }
        public IEnumerable<IOffensiveRequest> Offensives { get; }

        public PvPStrategy(IPvPBaseStrategy baseStrategy, IOffensiveRequest[] offensives)
        {
            Assert.IsNotNull(baseStrategy);
            Assert.IsTrue(offensives.Length != 0);

            BaseStrategy = baseStrategy;
            Offensives = offensives;
        }

        public PvPStrategy(IPvPStrategy strategyToCopy)
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
            PvPStrategy other = obj as PvPStrategy;
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
