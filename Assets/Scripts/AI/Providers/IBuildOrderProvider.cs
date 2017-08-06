using System.Collections.Generic;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.AI.Providers
{
    public interface IBuildOrderProvider
    {
        IList<IPrefabKey> AntiRocketBuildOrder { get; }

        IList<IPrefabKey> GetBasicBuildOrder(int levelNum);
        IList<IPrefabKey> GetBaseBuildOrder(int levelNum);
        IList<IPrefabKey> GetAntiAirBuildOrder(int levelNum);
        IList<IPrefabKey> GetAntiNavalBuildOrder(int levelNum);
		bool IsAntiRocketBuildOrderAvailable(int levelNum);
	}
}