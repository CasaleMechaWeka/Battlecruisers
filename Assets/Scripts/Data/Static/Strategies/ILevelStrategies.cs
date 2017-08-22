using System.Collections.Generic;
using BattleCruisers.Data.Static.Strategies;

namespace BattleCruisers.Data.Static
{
    public interface ILevelStrategies
    {
		IList<IStrategy> AdaptiveStrategies { get; }
		IList<IStrategy> BasicStrategies { get; }
    }
}
