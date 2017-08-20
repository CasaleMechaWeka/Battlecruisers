using System.Collections.Generic;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.AI.Providers
{
    public interface IDynamicBuildOrder : IEnumerator<IPrefabKey>
    {
        // Empty
    }
}