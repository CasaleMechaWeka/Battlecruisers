using System.Collections.Generic;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.AI.Providers.Strategies
{
    public interface IBaseStrategy
    {
        IList<IPrefabKeyWrapper> BuildOrder { get; }
    }
}
