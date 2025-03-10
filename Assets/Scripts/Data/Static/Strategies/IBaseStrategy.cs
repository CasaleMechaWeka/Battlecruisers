using System.Collections.Generic;
using BattleCruisers.Data.Models.PrefabKeys.Wrappers;

namespace BattleCruisers.Data.Static.Strategies
{
    public interface IBaseStrategy
    {
        IList<IPrefabKeyWrapper> BuildOrder { get; }
    }
}
