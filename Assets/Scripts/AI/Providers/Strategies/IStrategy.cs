using System.Collections.Generic;
using BattleCruisers.AI.Providers.Strategies.Requests;

namespace BattleCruisers.AI.Providers.Strategies
{
    public interface IStrategy
    {
        IBaseStrategy BaseStrategy { get; }
        IEnumerable<IOffensiveRequest> Offensives { get; }
    }
}
