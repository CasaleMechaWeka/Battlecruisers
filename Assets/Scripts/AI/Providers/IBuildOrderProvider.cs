using System.Collections.Generic;
using BattleCruisers.Cruisers;
using BattleCruisers.Data.Models.PrefabKeys;

namespace BattleCruisers.AI.Providers
{
    public interface IBuildOrderProvider
    {
        IList<IPrefabKey> AntiRocketBuildOrder { get; }

        IList<IPrefabKey> GetBasicBuildOrder(int levelNum, ISlotWrapper slotWrapper);
        IList<IPrefabKey> GetAdaptiveBuildOrder(int levelNum, ISlotWrapper slotWrapper);
        IList<IPrefabKey> GetAntiAirBuildOrder(int levelNum);
        IList<IPrefabKey> GetAntiNavalBuildOrder(int levelNum);
		bool IsAntiRocketBuildOrderAvailable(int levelNum);
	}
}
