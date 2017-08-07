using System.Collections.Generic;
using BattleCruisers.AI.Providers.Strategies;

namespace BattleCruisers.Data.Static
{
    public interface ILevelStrategies
    {
		IList<IStrategy> AdaptiveStrategies { get; }
		IList<IStrategy> BasicStrategies { get; }
    }
}
