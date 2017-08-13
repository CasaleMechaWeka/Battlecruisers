using System.Collections.Generic;
using BattleCruisers.AI.Providers;

namespace BattleCruisers.Data.Models.PrefabKeys.Wrappers
{
    public class AntiNavalPrefabKeyWrapper : BasePrefabKeyWrapper
    {
        protected override IEnumerator<IPrefabKey> GetBuildOrder(IBuildOrders buildOrders)
        {
            return buildOrders.AntiNavalBuildOrder;
        }
    }
}
