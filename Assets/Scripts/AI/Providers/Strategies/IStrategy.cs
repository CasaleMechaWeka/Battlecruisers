using System.Collections.Generic;

namespace BattleCruisers.AI.Providers.Strategies
{
    public interface IStrategy
    {
        IBaseStrategy BaseStrategy { get; }
        IEnumerable<IBasicOffensiveRequest> Offensives { get; }
    }
}
