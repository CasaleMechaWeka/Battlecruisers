using System.Collections.Generic;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.AI.BuildOrders
{
    public interface IAntiUnitBuildOrderProvider
	{
        IList<IPrefabKey> CreateBuildOrder(int numOfDeckSlots, int levelNum);
    }
}
