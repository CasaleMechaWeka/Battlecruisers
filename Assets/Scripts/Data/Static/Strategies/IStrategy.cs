using System.Collections.Generic;
using BattleCruisers.Data.Static.Strategies.Requests;

namespace BattleCruisers.Data.Static.Strategies
{
    public interface IStrategy
    {
        IBaseStrategy BaseStrategy { get; }
        IEnumerable<IOffensiveRequest> Offensives { get; }
    }
}
