using System.Collections.Generic;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.AI
{
    public interface IBuildOrderProvider
    {
		IList<IPrefabKey> GetBasicBuildOrder(int levelNum);
		IList<IPrefabKey> GetAdvancedBuildOrder(int levelNum);
		IList<IPrefabKey> GetAntiAirBuildOrder(int levelNum);
        IList<IPrefabKey> GetAntiNavalBuildOrder(int levelNum);
	}
}